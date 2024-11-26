package commonlib

import (
	"context"
	"encoding/binary"
	"errors"
	"fmt"
	"os"

	"capnproto.org/go/capnp/v3"
	"github.com/google/uuid"
	"github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common"
	"github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/persistence"

	"crypto/rand"

	"golang.org/x/crypto/nacl/sign"
)

type Identifiable struct {
	Id          string // uuid
	Name        string
	Description string
}

// Identifiable_Server interface

func (i *Identifiable) Info(c context.Context, ii common.Identifiable_info) error {

	results, err := ii.AllocResults()
	if err != nil {
		return err
	}
	err = results.SetId(i.Id)
	if err != nil {
		return err
	}
	err = results.SetName(i.Name)
	if err != nil {
		return err
	}
	err = results.SetDescription(i.Description)
	return err
}

type Persistable struct {
	saveChan chan *SaveMsg
	//function to create a capability of the object
	Cap func() capnp.Client
}

func NewPersistable(restorer *Restorer) *Persistable {
	persitable := &Persistable{
		saveChan: restorer.saveMsgC,
	}
	return persitable
}

func (p *Persistable) InitialSturdyRef() (*SturdyRef, error) {

	saveMsg := &SaveMsg{
		persitentObj: p,
		owner:        "",
		returnChan:   make(chan SaveAnswer),
	}
	p.saveChan <- saveMsg
	answer := <-saveMsg.returnChan
	if answer.err != nil {
		return nil, answer.err
	}
	sr := answer.sr
	return sr, nil
}

// Persistent_Server interface
func (p *Persistable) Save(c context.Context, call persistence.Persistent_save) error {

	if p.saveChan != nil {
		var owner string
		if call.Args().HasSealFor() {
			sealForOwner, err := call.Args().SealFor()
			if err != nil {
				return err
			}
			if sealForOwner.HasGuid() {
				owner, err = sealForOwner.Guid()
				if err != nil {
					return err
				}
			}
		}

		saveMsg := &SaveMsg{
			persitentObj: p,
			owner:        owner,
			returnChan:   make(chan SaveAnswer),
		}
		p.saveChan <- saveMsg
		answer := <-saveMsg.returnChan
		if answer.err != nil {
			return answer.err
		}
		sr := answer.sr
		unsaveSr := answer.unsave

		result, err := call.AllocResults()
		if err != nil {
			return err
		}

		srl, err := result.NewSturdyRef()
		if err != nil {
			return err
		}
		// if not vat.. vat is dat?
		shouldReturn, returnValue := p.sturdyRefToMessage(srl, sr)
		if shouldReturn {
			return returnValue
		}
		err = result.SetSturdyRef(srl)
		if err != nil {
			return err
		}

		srUnsavel, err := result.NewUnsaveSR()
		if err != nil {
			return err
		}
		shouldReturn, returnValue = p.sturdyRefToMessage(srUnsavel, unsaveSr)
		if shouldReturn {
			return returnValue
		}
		err = result.SetUnsaveSR(srUnsavel)
		if err != nil {
			return err
		}

	}

	return fmt.Errorf("no restorer set")
}

func (*Persistable) sturdyRefToMessage(srl persistence.SturdyRef, sr *SturdyRef) (bool, error) {

	// Stored - Reference to an object in long-term storage.(not Implemented)
	// 256-bit object key. This both identifies the object and may serve as a symmetric key for
	// decrypting the object.

	// srStored, err := srl.NewStored()
	// srStored.SetKey0(0)
	// srStored.SetKey1(1)
	// srStored.SetKey2(2)
	// srStored.SetKey3(3)
	// err = srl.SetStored(srStored)
	// if err != nil {
	// 	return true, err
	// }
	strT, err := srl.NewLocalRef()
	if err != nil {
		return true, err
	}
	err = strT.SetText(sr.localRef)
	if err != nil {
		return true, err
	}
	err = srl.SetLocalRef(strT)
	if err != nil {
		return true, err
	}
	vat, err := srl.NewVat()
	if err != nil {
		return true, err
	}
	newVatId, err := vat.NewId()
	if err != nil {
		return true, err
	}
	newVatId.SetPublicKey0(sr.vat.id.publicKey0)
	newVatId.SetPublicKey1(sr.vat.id.publicKey1)
	newVatId.SetPublicKey2(sr.vat.id.publicKey2)
	newVatId.SetPublicKey3(sr.vat.id.publicKey3)
	err = vat.SetId(newVatId)
	if err != nil {
		return true, err
	}
	newVatAddr, err := vat.NewAddress()
	if err != nil {
		return true, err
	}
	err = newVatAddr.SetHost(sr.vat.address.host)
	if err != nil {
		return true, err
	}
	newVatAddr.SetPort(sr.vat.address.port)
	if IsIpv6(sr.vat.address.host) {
		newVatAddr.SetIp6()
	}
	err = vat.SetAddress(newVatAddr)
	if err != nil {
		return true, err
	}
	err = srl.SetVat(vat)
	if err != nil {
		return true, err
	}
	return false, nil
}

type SturdyRefToken string

type Restorer struct {
	issuedSturdyRefTokens map[SturdyRefToken]func() capnp.Client // SturdyRefToken to capability generator function
	withdrawActions       map[SturdyRefToken]*ReleaseSturdyRefAction
	sign_pk               *[32]byte
	sign_sk               *[64]byte
	host                  string
	port                  uint16
	restorerVatId         vatId
	owner                 map[string]*[32]byte

	restoreMsgC chan *RestoreMsg
	saveMsgC    chan *SaveMsg
	deleteMsgC  chan *DeleteMsg
}

// NewRestorer creates a new Restorer
func NewRestorer(hostname string, port uint16) *Restorer {

	// Generate a key pair for signing sturdyRefs
	pub, priv, err := sign.GenerateKey(rand.Reader)
	if err != nil {
		panic(err)
	}
	if hostname == "" {
		hostname, err = os.Hostname() // fallback to hostname from os
		if err != nil {
			panic(err)
		}
	}
	if port == 0 {
		p, err := GetFreePort() // fallback to random port
		if err != nil {
			panic(err)
		}
		port = uint16(p)
	}

	// Create a vatId for the restorer vat from the public key
	restorerVatId := vatId{
		publicKey0: binary.LittleEndian.Uint64(pub[0:8]),
		publicKey1: binary.LittleEndian.Uint64(pub[8:16]),
		publicKey2: binary.LittleEndian.Uint64(pub[16:24]),
		publicKey3: binary.LittleEndian.Uint64(pub[24:32]),
	}

	restorer := Restorer{
		issuedSturdyRefTokens: map[SturdyRefToken]func() capnp.Client{},
		withdrawActions:       map[SturdyRefToken]*ReleaseSturdyRefAction{},
		sign_pk:               pub,
		sign_sk:               priv,
		host:                  hostname,
		port:                  port,
		restorerVatId:         restorerVatId,
		owner:                 map[string]*[32]byte{},
		restoreMsgC:           make(chan *RestoreMsg),
		saveMsgC:              make(chan *SaveMsg),
		deleteMsgC:            make(chan *DeleteMsg),
	}

	go restorer.messageLoop()
	return &restorer
}
func (r *Restorer) BootstrapSturdyRef() *SturdyRef {
	return NewSturdyRef(r.restorerVatId.toSlice(), r.host, r.port, "")
}
func (r *Restorer) Host() string {
	return r.host
}
func (r *Restorer) Port() uint16 {
	return r.port
}
func (r *Restorer) messageLoop() {
	for {
		select {
		case msg := <-r.saveMsgC:
			sr, unsave, err := r.save(msg.persitentObj, msg.owner)
			msg.returnChan <- SaveAnswer{
				sr:     sr,
				unsave: unsave,
				err:    err,
			}
		case msg := <-r.deleteMsgC:
			delete(r.issuedSturdyRefTokens, SturdyRefToken(msg.srToken))
			delete(r.withdrawActions, SturdyRefToken(msg.unsaveToken))
		case msg := <-r.restoreMsgC:

			srTokenbytes := msg.localRef
			srToken := ""
			ownerGuid := msg.owner
			if ownerGuid != "" {
				out := make([]byte, 0, len([]byte(srToken)))
				if _, ok := r.owner[ownerGuid]; !ok {
					msg.returnChan <- RestoreAnswer{err: errors.New("no owner with this guid")}
				}
				sign.Open(out, srTokenbytes, r.owner[ownerGuid])
				srToken = string(out)
			} else {
				srToken = string(srTokenbytes)
			}
			if _, ok := r.issuedSturdyRefTokens[SturdyRefToken(srToken)]; !ok {
				msg.returnChan <- RestoreAnswer{err: errors.New("no such token")}
			} else {
				cap := r.issuedSturdyRefTokens[SturdyRefToken(srToken)]()
				msg.returnChan <- RestoreAnswer{cap: cap}
			}
		}
	}
}

type RestoreMsg struct {
	localRef []byte // srToken
	owner    string

	returnChan chan RestoreAnswer
}

type RestoreAnswer struct {
	cap capnp.Client
	err error
}

type SaveMsg struct {
	persitentObj *Persistable
	owner        string

	returnChan chan SaveAnswer
}
type SaveAnswer struct {
	sr     *SturdyRef
	unsave *SturdyRef
	err    error
}

type DeleteMsg struct {
	srToken     string
	unsaveToken string
}

// Restorer_Server interface
func (r *Restorer) Restore(c context.Context, call persistence.Restorer_restore) error {

	ownerGuid := ""
	if call.Args().HasSealedBy() {
		// if call has a sealed by, then we need to open the token with the owner's key
		// and get the local ref
		owner, err := call.Args().SealedBy()
		if err != nil {
			return err
		}
		ownerGuid, err = owner.Guid()
		if err != nil {
			return err
		}
	}
	var srTokenBytes []byte
	localRef, err := call.Args().LocalRef()
	if err != nil {
		return err
	}
	if localRef.HasText() {
		srToken, err := localRef.Text()
		if err != nil {
			return err
		}
		// string to byte array
		srTokenBytes = []byte(srToken)

	} else if localRef.HasData() {
		srTokenBytes, err = localRef.Data()
		if err != nil {
			return err
		}
	}

	restoreMsg := RestoreMsg{
		localRef:   srTokenBytes,
		owner:      ownerGuid,
		returnChan: make(chan RestoreAnswer),
	}
	r.restoreMsgC <- &restoreMsg

	answer := <-restoreMsg.returnChan
	if answer.err != nil {
		return answer.err
	}
	cap := answer.cap

	results, err := call.AllocResults()
	if err != nil {
		return err
	}
	err = results.SetCap(cap)

	return err
}

// save - saves a capability and returns a SturdyRef to it and a SturdyRef to the unsave method
func (r *Restorer) save(persistentObj *Persistable, owner string) (sr *SturdyRef, unsaveSR *SturdyRef, err error) {

	guid := uuid.New().String()
	uguid := uuid.New().String()
	if owner != "" {
		// check if owner exists
		if _, ok := r.owner[owner]; !ok {
			return nil, nil, errors.New("no owner with this guid")
		}
		// sign the guid and uguid with the owner's key
		out := make([]byte, 0, len([]byte(guid)))

		sign.Open(out, []byte(guid), r.owner[owner])
		guid = string(out)

		out = make([]byte, 0, len([]byte(uguid)))
		sign.Open(out, []byte(uguid), r.owner[owner])
		uguid = string(out)
	}
	// create the SturdyRef
	sr = NewSturdyRef(r.restorerVatId.toSlice(), r.host, r.port, guid)
	// create the unsave/delete SturdyRef
	unsaveSR = NewSturdyRef(r.restorerVatId.toSlice(), r.host, r.port, uguid)

	r.issuedSturdyRefTokens[SturdyRefToken(guid)] = persistentObj.Cap

	r.withdrawActions[SturdyRefToken(uguid)] = NewAction(r.saveMsgC, func() error {
		// delete the SturdyRef
		deleteMsg := DeleteMsg{
			srToken:     guid,
			unsaveToken: uguid,
		}
		r.deleteMsgC <- &deleteMsg
		return nil
	})

	return sr, unsaveSR, err
}

// ReleaseSturdyRefAction
type ReleaseSturdyRefAction struct {
	persistable *Persistable
	do          func() error
}

func NewAction(doSave chan *SaveMsg, doAction func() error) *ReleaseSturdyRefAction {

	action := ReleaseSturdyRefAction{
		persistable: &Persistable{
			saveChan: doSave,
		},
		do: doAction,
	}

	restoreFunc := func() capnp.Client {
		return capnp.Client(persistence.Persistent_ReleaseSturdyRef_ServerToClient(&action))
	}
	action.persistable.Cap = restoreFunc

	return &action
}

// ReleaseSturdyRef_Server interface
func (a *ReleaseSturdyRefAction) ReleaseSR(c context.Context, call persistence.Persistent_ReleaseSturdyRef_releaseSR) error {

	// do something
	return a.do()
}

func (a *ReleaseSturdyRefAction) Save(c context.Context, call persistence.Persistent_save) error {
	return a.persistable.Save(c, call)
}
