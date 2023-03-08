package commonlib

import (
	"context"
	"errors"

	"capnproto.org/go/capnp/v3"
	"github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common"
	"github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/grid"
	"github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/persistence"
)

// Grid is a struct that implements the Grid_Server interface
// for immutable grids, the functions that need to be implemented are
// GetValueRowCol, GetValueLatLon
// GetValueRowColAggregated, GetValueLatLonAggregated
// not thread safe if the grid below is changing
type Grid struct {
	persistable *Persistable
	info        *Identifiable

	GridResolution       uint64
	NumRows              uint64
	NumCols              uint64
	NoDataType           interface{} // valid types are int64, float64, bool, uint64
	Bounds               LatLonBoundaries
	BoundsFromCellCenter LatLonBoundaries

	// functions that need to be implemented to use the grid capnp interface
	GetValueRowCol           func(row, col uint64) (interface{}, error)
	GetValueLatLon           func(lat, lon float64) (interface{}, RowCol, RowCol, error)
	GetValueRowColAggregated func(row, col uint64, resolution uint64, agg string, includeAggParts bool) (interface{}, []AggregationPart, error)
	GetValueLatLonAggregated func(lat, lon float64, resolution uint64, agg string, includeAggParts bool) (interface{}, []AggregationPart, error)
}

func NewGrid(restorer *Restorer, id, name, description string) *Grid {
	identifiable := &Identifiable{
		Id:          id,
		Name:        name,
		Description: description,
	}
	persitable := &Persistable{
		saveChan: restorer.saveMsgC,
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

func (g *Grid) InitialSturdyRef() (*SturdyRef, error) {

	saveMsg := &SaveMsg{
		persitentObj: g.persistable,
		owner:        "",
		returnChan:   make(chan SaveAnswer),
	}
	g.persistable.saveChan <- saveMsg
	answer := <-saveMsg.returnChan
	if answer.err != nil {
		return nil, answer.err
	}
	sr := answer.sr
	return sr, nil
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
type RowCol struct {
	Row uint64
	Col uint64
}
type AggregationPart struct {
	OriginalValue     interface{} // valid types are int64, float64, bool, uint64
	RowColTuple       RowCol
	AreaWeight        float64
	interpolatedValue float64
}

// Grid_Server interface
func (g *Grid) ClosestValueAt(c context.Context, call grid.Grid_closestValueAt) error {
	latLon, err := call.Args().LatlonCoord()
	if err != nil {
		return err
	}
	lat := latLon.Lat()
	lon := latLon.Lon()
	resolution := call.Args().Resolution()
	agg := call.Args().Agg()
	includeAggParts := call.Args().IncludeAggParts()
	ignoreNoData := call.Args().IgnoreNoData()
	returnRowCols := call.Args().ReturnRowCols()

	result, err := call.AllocResults()
	if err != nil {
		return err
	}
	var returnVal interface{}
	var aggrParts []AggregationPart
	var topLeftrowCol RowCol
	var bottomRightRowCol RowCol
	if resolution >= g.GridResolution {
		// get value from grid (lat, lon)
		if g.GetValueLatLon == nil {
			return errors.New("Grid.GetValueLatLon is not implemented")
		}
		returnVal, topLeftrowCol, bottomRightRowCol, err = g.GetValueLatLon(lat, lon)
		if err != nil {
			return err
		}
	} else if resolution < g.GridResolution && agg.String() == "none" {
		returnVal = 0
	} else if resolution < g.GridResolution && agg.String() != "none" {

		// call grid implementation of aggregation
		if g.GetValueLatLonAggregated == nil {
			return errors.New("Grid.GetValueLatLonAggregated is not implemented")
		}
		returnVal, aggrParts, err = g.GetValueLatLonAggregated(lat, lon, resolution, agg.String(), ignoreNoData)
		if err != nil {
			return err
		}
		if includeAggParts {
			lenAggrParts := len(aggrParts)
			aggList, err := result.NewAggParts(int32(lenAggrParts))
			if err != nil {
				return err
			}
			for i := 0; i < lenAggrParts; i++ {
				aggPart, err := grid.NewGrid_AggregationPart(aggList.Segment())
				if err != nil {
					return err
				}
				aggPart.SetAreaFrac(aggrParts[i].AreaWeight)
				aggPart.SetIValue(aggrParts[i].interpolatedValue)
				origVal, err := aggPart.NewValue()
				if err != nil {
					return err
				}
				SetGridValue(origVal, aggrParts[i].OriginalValue)
				aggPart.SetValue(origVal)
				rowcol, err := aggPart.NewRowCol()
				if err != nil {
					return err
				}
				rowcol.SetRow(aggrParts[i].RowColTuple.Row)
				rowcol.SetCol(aggrParts[i].RowColTuple.Col)
				aggPart.SetRowCol(rowcol)
				aggList.Set(i, aggPart)
			}
		}

	}

	if returnRowCols {
		// get row and col of closest value
		rowCol, err := result.NewBr()
		if err != nil {
			return err
		}
		rowCol.SetRow(bottomRightRowCol.Row)
		rowCol.SetCol(bottomRightRowCol.Col)
		result.SetBr(rowCol)
		rowCol, err = result.NewTl()
		if err != nil {
			return err
		}
		rowCol.SetRow(topLeftrowCol.Row)
		rowCol.SetCol(topLeftrowCol.Col)
		result.SetTl(rowCol)
	}
	val, err := result.NewVal()
	if err != nil {
		return err
	}
	err = SetGridValue(val, returnVal)
	if err != nil {
		return err
	}
	err = result.SetVal(val)
	if err != nil {
		return err
	}
	return nil
}
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
	err = SetGridValue(gridVal, g.NoDataType)
	if err != nil {
		return err
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
	var returnVal interface{}
	var aggrParts []AggregationPart
	if resolution >= g.GridResolution {
		if g.GetValueRowCol == nil {
			return errors.New("Grid.GetValueRowCol is not implemented")
		}
		// get value from grid (row, col)
		returnVal, err = g.GetValueRowCol(row, col)
		if err != nil {
			return err
		}
	} else if resolution < g.GridResolution && agg.String() == "none" {
		returnVal = 0
	} else if resolution < g.GridResolution && agg.String() != "none" {

		if g.GetValueRowColAggregated == nil {
			return errors.New("Grid.GetValueRowColAggregated is not implemented")
		}
		// call grid implementation of aggregation
		returnVal, aggrParts, err = g.GetValueRowColAggregated(row, col, resolution, agg.String(), includeAggParts)
		if err != nil {
			return err
		}
		if includeAggParts {
			var listLen int32 = int32(len(aggrParts))
			list, err := result.NewAggParts(listLen)
			if err != nil {
				return err
			}
			for i, part := range aggrParts {
				aggPart, err := grid.NewGrid_AggregationPart(list.Segment())
				if err != nil {
					return err
				}
				rowCol, err := aggPart.NewRowCol()
				if err != nil {
					return err
				}
				rowCol.SetRow(part.RowColTuple.Row)
				rowCol.SetCol(part.RowColTuple.Col)
				aggPart.SetRowCol(rowCol)

				aggPart.SetAreaFrac(part.AreaWeight)
				aggPart.SetIValue(part.interpolatedValue)
				val, err := aggPart.NewValue()
				if err != nil {
					return err
				}
				SetGridValue(val, part.OriginalValue)
				aggPart.SetValue(val)
				list.Set(i, aggPart)
			}
		}
	}

	// set value to result
	gridVal, err := result.NewVal()
	if err != nil {
		return err
	}
	SetGridValue(gridVal, returnVal)
	result.SetVal(gridVal)

	return nil
}

func SetGridValue(gV grid.Grid_Value, val interface{}) error {
	switch val := val.(type) {
	case int64:
		gV.SetI(val)
	case float64:
		gV.SetF(val)
	case bool:
		gV.SetNo(val)
	case uint64:
		gV.SetUi(val)
	default:
		return errors.New("unknown type for value")
	}
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
		bl.SetLat(g.BoundsFromCellCenter.BottomLeft.Lat)
		bl.SetLon(g.BoundsFromCellCenter.BottomLeft.Lon)
		err = result.SetBl(bl)
		if err != nil {
			return err
		}

		br, err := result.NewBr()
		if err != nil {
			return err
		}
		br.SetLat(g.BoundsFromCellCenter.BottomRight.Lat)
		br.SetLon(g.BoundsFromCellCenter.BottomRight.Lon)
		err = result.SetBr(br)
		if err != nil {
			return err
		}

		tl, err := result.NewTl()
		if err != nil {
			return err
		}
		tl.SetLat(g.BoundsFromCellCenter.TopLeft.Lat)
		tl.SetLon(g.BoundsFromCellCenter.TopLeft.Lon)
		err = result.SetTl(tl)
		if err != nil {
			return err
		}

		tr, err := result.NewTr()
		if err != nil {
			return err
		}
		tr.SetLat(g.BoundsFromCellCenter.TopRight.Lat)
		tr.SetLon(g.BoundsFromCellCenter.TopRight.Lon)
		err = result.SetTr(tr)
		if err != nil {
			return err
		}
	} else {
		bl, err := result.NewBl()
		if err != nil {
			return err
		}
		bl.SetLat(g.Bounds.BottomLeft.Lat)
		bl.SetLon(g.Bounds.BottomLeft.Lon)
		err = result.SetBl(bl)
		if err != nil {
			return err
		}
		br, err := result.NewBr()
		if err != nil {
			return err
		}
		br.SetLat(g.Bounds.BottomRight.Lat)
		br.SetLon(g.Bounds.BottomRight.Lon)
		err = result.SetBr(br)
		if err != nil {
			return err
		}

		tl, err := result.NewTl()
		if err != nil {
			return err
		}
		tl.SetLat(g.Bounds.TopLeft.Lat)
		tl.SetLon(g.Bounds.TopLeft.Lon)
		err = result.SetTl(tl)
		if err != nil {
			return err
		}

		tr, err := result.NewTr()
		if err != nil {
			return err
		}
		tr.SetLat(g.Bounds.TopRight.Lat)
		tr.SetLon(g.Bounds.TopRight.Lon)
		err = result.SetTr(tr)
		if err != nil {
			return err
		}
	}
	return nil
}
func (g *Grid) StreamCells(c context.Context, call grid.Grid_streamCells) error {

	return nil
}

// Identifiable_Server interface
func (g *Grid) Info(c context.Context, call common.Identifiable_info) error {
	return g.info.Info(c, call)
}

// Persistence_Server interface
func (g *Grid) Save(c context.Context, call persistence.Persistent_save) error {
	return g.persistable.Save(c, call)
}
