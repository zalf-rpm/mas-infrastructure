package commonlib

import (
	"context"
	"errors"

	"capnproto.org/go/capnp/v3"
	"github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common"
	"github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/grid"
	"github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/persistence"
)

type Grid struct {
	persistable *Persistable
	info        *Identifiable

	GridResolution       uint64
	NumRows              uint64
	NumCols              uint64
	NoDataType           interface{} // valid types are int64, float64, bool, uint64
	bounds               LatLonBoundaries
	boundsFromCellCenter LatLonBoundaries

	GetValueRowCol func(row, col uint64) interface{}
	GetValueLatLon func(lat, lon float64) interface{}
}

func NewGrid(saveChan chan *SaveMsg, id, name, description string) *Grid {
	identifiable := &Identifiable{
		Id:          id,
		Name:        name,
		Description: description,
	}
	persitable := &Persistable{
		saveChan: saveChan,
	}
	newGrid := &Grid{
		persistable: persitable,
		info:        identifiable,
	}

	restoreFunc := func() capnp.Client {
		return capnp.Client(grid.Grid_ServerToClient(newGrid))
	}
	newGrid.persistable.Cap = restoreFunc

	return newGrid
}

type LatLon struct {
	Lat float64
	Lon float64
}

type LatLonBoundaries struct {
	TopLeft     LatLon
	TopRight    LatLon
	BottomLeft  LatLon
	BottomRight LatLon
}

// Grid_Server interface
func (g *Grid) ClosestValueAt(c context.Context, call grid.Grid_closestValueAt) error { return nil }
func (g *Grid) Resolution(c context.Context, call grid.Grid_resolution) error {
	result, err := call.AllocResults()
	if err != nil {
		return err
	}
	result.SetRes(g.GridResolution)
	return nil
}

func (g *Grid) Dimension(c context.Context, call grid.Grid_dimension) error {
	result, err := call.AllocResults()
	if err != nil {
		return err
	}
	result.SetRows(g.NumRows)
	result.SetCols(g.NumCols)
	return nil
}

func (g *Grid) NoDataValue(c context.Context, call grid.Grid_noDataValue) error {
	result, err := call.AllocResults()
	if err != nil {
		return err
	}
	gridVal, err := result.NewNodata()
	if err != nil {
		return err
	}
	switch val := g.NoDataType.(type) {
	case int64:
		gridVal.SetI(val)
	case float64:
		gridVal.SetF(val)
	case bool:
		gridVal.SetNo(val)
	case uint64:
		gridVal.SetUi(val)
	default:
		return errors.New("unknown type for NoDataValue")
	}
	return result.SetNodata(gridVal)
}
func (g *Grid) ValueAt(c context.Context, call grid.Grid_valueAt) error {

	row := call.Args().Row()
	col := call.Args().Col()
	resolution := call.Args().Resolution()
	agg := call.Args().Agg()
	includeAggParts := call.Args().IncludeAggParts()

	result, err := call.AllocResults()
	if err != nil {
		return err
	}

	if agg.String() != "none" {

		//TBD: implement aggregation
		var listLen int32 = 1
		list, err := result.NewAggParts(listLen)
		if err != nil {
			return err
		}

		aggPart, err := grid.NewGrid_AggregationPart(list.Segment())
		if err != nil {
			return err
		}

	} else {
		gridVal, err := result.NewVal()
		if err != nil {
			return err
		}
		// get value from grid (row, col)
		value := g.GetValueRowCol(row, col)
		switch val := value.(type) {
		case int64:
			gridVal.SetI(val)
		case float64:
			gridVal.SetF(val)
		case bool:
			gridVal.SetNo(val)
		case uint64:
			gridVal.SetUi(val)
		default:
			return errors.New("unknown type for NoDataValue")
		}
	}

	// (row :UInt64, col :UInt64,
	// 	resolution :UInt64,
	// 	agg :Aggregation = none,
	// 	includeAggParts :Bool = false)
	// 	-> (val :Value, aggParts :List(AggregationPart));

	return nil
}
func (g *Grid) LatLonBounds(c context.Context, call grid.Grid_latLonBounds) error {
	useCellcenter := call.Args().UseCellCenter()

	result, err := call.AllocResults()
	if err != nil {
		return err
	}

	if useCellcenter {
		bl, err := result.NewBl()
		if err != nil {
			return err
		}
		bl.SetLat(g.boundsFromCellCenter.BottomLeft.Lat)
		bl.SetLon(g.boundsFromCellCenter.BottomLeft.Lon)
		err = result.SetBl(bl)
		if err != nil {
			return err
		}

		br, err := result.NewBr()
		if err != nil {
			return err
		}
		br.SetLat(g.boundsFromCellCenter.BottomRight.Lat)
		br.SetLon(g.boundsFromCellCenter.BottomRight.Lon)
		err = result.SetBr(br)
		if err != nil {
			return err
		}

		tl, err := result.NewTl()
		if err != nil {
			return err
		}
		tl.SetLat(g.boundsFromCellCenter.TopLeft.Lat)
		tl.SetLon(g.boundsFromCellCenter.TopLeft.Lon)
		err = result.SetTl(tl)
		if err != nil {
			return err
		}

		tr, err := result.NewTr()
		if err != nil {
			return err
		}
		tr.SetLat(g.boundsFromCellCenter.TopRight.Lat)
		tr.SetLon(g.boundsFromCellCenter.TopRight.Lon)
		err = result.SetTr(tr)
		if err != nil {
			return err
		}
	} else {
		bl, err := result.NewBl()
		if err != nil {
			return err
		}
		bl.SetLat(g.bounds.BottomLeft.Lat)
		bl.SetLon(g.bounds.BottomLeft.Lon)
		err = result.SetBl(bl)
		if err != nil {
			return err
		}
		br, err := result.NewBr()
		if err != nil {
			return err
		}
		br.SetLat(g.bounds.BottomRight.Lat)
		br.SetLon(g.bounds.BottomRight.Lon)
		err = result.SetBr(br)
		if err != nil {
			return err
		}

		tl, err := result.NewTl()
		if err != nil {
			return err
		}
		tl.SetLat(g.bounds.TopLeft.Lat)
		tl.SetLon(g.bounds.TopLeft.Lon)
		err = result.SetTl(tl)
		if err != nil {
			return err
		}

		tr, err := result.NewTr()
		if err != nil {
			return err
		}
		tr.SetLat(g.bounds.TopRight.Lat)
		tr.SetLon(g.bounds.TopRight.Lon)
		err = result.SetTr(tr)
		if err != nil {
			return err
		}
	}
	return nil
}
func (g *Grid) StreamCells(c context.Context, call grid.Grid_streamCells) error { return nil }

// Identifiable_Server interface
func (g *Grid) Info(c context.Context, call common.Identifiable_info) error {
	return g.info.Info(c, call)
}

// Persistence_Server interface
func (g *Grid) Save(c context.Context, call persistence.Persistent_save) error {
	return g.persistable.Save(c, call)
}
