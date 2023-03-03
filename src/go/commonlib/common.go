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
	SelfRestore *Restorer
	//function to create a capability of the object
	Cap func() capnp.Client
}

// Persistent_Server interface
func (p *Persistable) Save(c context.Context, call persistence.Persistent_save) error {

	if p.SelfRestore != nil {
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
				if _, ok := p.SelfRestore.owner[owner]; !ok {
					return errors.New("no owner with this guid")
				}
			}
		}

		result, err := call.AllocResults()
		if err != nil {
			return err
		}

		sr, unsaveSr := p.SelfRestore.save(p, owner)

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
	transient, err := srl.NewTransient()
	if err != nil {
		return true, err
	}
	l, err := capnp.NewText(transient.Segment(), sr.localRef)
	if err != nil {
		return true, err
	}
	err = transient.SetLocalRef(l.ToPtr())
	if err != nil {
		return true, err
	}
	vat, err := transient.NewVat()
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
	err = transient.SetVat(vat)
	if err != nil {
		return true, err
	}
	err = srl.SetTransient(transient)
	if err != nil {
		return true, err
	}
	return false, nil
}

type SturdyRefToken string

type Restorer struct {
	issuedSturdyRefTokens map[SturdyRefToken]func() capnp.Client // SturdyRefToken to capability generator function
	withdrawActions       map[SturdyRefToken]func()
	sign_pk               *[32]byte
	sign_sk               *[64]byte
	host                  string
	port                  uint16
	restorerVatId         vatId
	owner                 map[string]*[32]byte
}

// NewRestorer creates a new Restorer
func NewRestorer() Restorer {

	// Generate a key pair for signing sturdyRefs
	pub, priv, err := sign.GenerateKey(rand.Reader)
	if err != nil {
		panic(err)
	}
	hostname, err := os.Hostname()
	if err != nil {
		panic(err)
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
		withdrawActions:       map[SturdyRefToken]func(){},
		sign_pk:               pub,
		sign_sk:               priv,
		host:                  hostname,
		port:                  0,
		restorerVatId:         restorerVatId,
		owner:                 map[string]*[32]byte{},
	}
	return restorer
}

// Restorer_Server interface
func (r *Restorer) Restore(c context.Context, call persistence.Restorer_restore) error {

	srToken := ""
	if call.Args().HasSealedFor() {
		// if call has a sealed for, then we need to open the token with the owner's key
		// and get the local ref
		owner, err := call.Args().SealedFor()
		if err != nil {
			return err
		}
		ownerGuid, err := owner.Guid()
		if err != nil {
			return err
		}
		localRef, err := call.Args().LocalRef()
		if err != nil {
			return err
		}
		srToken = localRef.Text()
		out := make([]byte, 0, len([]byte(srToken)))
		if _, ok := r.owner[ownerGuid]; !ok {
			return errors.New("no owner with this guid")
		}
		sign.Open(out, []byte(srToken), r.owner[ownerGuid])
		srToken = string(out)

	} else {
		localRef, err := call.Args().LocalRef()
		if err != nil {
			return err
		}
		srToken = localRef.Text()
	}

	if _, ok := r.issuedSturdyRefTokens[SturdyRefToken(srToken)]; !ok {
		return errors.New("no such token")
	}
	cap := r.issuedSturdyRefTokens[SturdyRefToken(srToken)]()

	results, err := call.AllocResults()
	if err != nil {
		return err
	}
	err = results.SetCap(cap)

	return err
}

// save - saves a capability and returns a SturdyRef to it and a SturdyRef to the unsave method
func (r *Restorer) save(persistentObj *Persistable, owner string) (sr *SturdyRef, unsaveSR *SturdyRef) {

	guid := uuid.New().String()
	uguid := uuid.New().String()
	if owner != "" {
		if _, ok := r.owner[owner]; !ok {
			panic("No owner with this guid")
		}
		out := make([]byte, 0, len([]byte(guid)))

		sign.Open(out, []byte(guid), r.owner[owner])
		guid = string(out)

		out = make([]byte, 0, len([]byte(uguid)))
		sign.Open(out, []byte(uguid), r.owner[owner])
		uguid = string(out)
	}
	sr = NewSturdyRef(r.restorerVatId.toSlice(), r.host, r.port, guid)
	unsaveSR = NewSturdyRef(r.restorerVatId.toSlice(), r.host, r.port, uguid)

	r.issuedSturdyRefTokens[SturdyRefToken(guid)] = persistentObj.Cap
	r.withdrawActions[SturdyRefToken(uguid)] = func() {
		delete(r.issuedSturdyRefTokens, SturdyRefToken(guid))
		delete(r.withdrawActions, SturdyRefToken(uguid))
	}

	return sr, unsaveSR
}
