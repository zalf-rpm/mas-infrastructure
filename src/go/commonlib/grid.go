package commonlib

import (
	"context"
	"errors"
	"math"

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

	GridResolution       Resolution
	GridUnit             string
	NumRows              uint64
	NumCols              uint64
	NoDataType           interface{} // valid types are int64, float64, bool, uint64
	Bounds               LatLonBoundaries
	BoundsFromCellCenter LatLonBoundaries

	// functions that need to be implemented to use the grid capnp interface
	RowColToLatLon           func(row, col uint64) (LatLon, error)
	GetValueRowCol           func(row, col uint64) (interface{}, error)
	GetValueLatLon           func(lat, lon float64) (interface{}, RowCol, RowCol, error)
	GetValueRowColAggregated func(row, col uint64, resolution Resolution, agg string, includeAggParts bool) (interface{}, []*AggregationPart, error)
	GetValueLatLonAggregated func(lat, lon float64, resolution Resolution, agg string, includeAggParts bool) (interface{}, []*AggregationPart, error)
}

type Resolution struct {
	Value interface{}
}

func (g Grid) IsComparable(r Resolution) bool {
	_, rIsInt := r.Value.(int64)
	_, rIsfloat := r.Value.(float64)

	if _, isInt := g.GridResolution.Value.(int64); isInt && rIsInt {
		return true
	}
	if _, isFloat := g.GridResolution.Value.(float64); isFloat && rIsfloat {
		return true
	}

	return false
}

// Compare compares two resolutions
// 1 means r is bigger than other
// -1 means r is smaller than other
// 0 means r is equal to other
func (r Resolution) Compare(other Resolution) int {

	switch rVal := r.Value.(type) {
	case int64:
		val := rVal - other.Value.(int64)
		if val > 0 {
			return 1
		} else if val < 0 {
			return -1
		} else {
			return 0
		}
	case float64:
		val := rVal - other.Value.(float64)
		if val > 0 {
			return 1
		} else if val < 0 {
			return -1
		} else {
			return 0
		}
	}
	return -1
}

func NewGrid(restorer *Restorer, id, name, description string) *Grid {
	identifiable := &Identifiable{
		Id:          id,
		Name:        name,
		Description: description,
	}
	persitable := NewPersistable(restorer)
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

	return g.persistable.InitialSturdyRef()
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
	InterpolatedValue float64
}

// Grid_Server interface
func (g *Grid) ClosestValueAt(c context.Context, call grid.Grid_closestValueAt) error {
	latLon, err := call.Args().LatlonCoord()
	if err != nil {
		return err
	}
	lat := latLon.Lat()
	lon := latLon.Lon()

	var resolution Resolution = Resolution{}
	if call.Args().HasResolution() {
		resolutionMsg, err := call.Args().Resolution()
		if err != nil {
			return err
		}

		if resolutionMsg.Which() == grid.Grid_Resolution_Which_meter {
			resolution.Value = resolutionMsg.Meter()
		}
		if resolutionMsg.Which() == grid.Grid_Resolution_Which_degree {
			resolution.Value = resolutionMsg.Degree()
		}
		if !g.IsComparable(resolution) {
			return errors.New("Resolution is not comparable")
		}
	} else {
		resolution = g.GridResolution
	}
	agg := call.Args().Agg()
	includeAggParts := call.Args().IncludeAggParts()
	ignoreNoData := call.Args().IgnoreNoData()
	returnRowCols := call.Args().ReturnRowCols()

	result, err := call.AllocResults()
	if err != nil {
		return err
	}
	var returnVal interface{}
	var aggrParts []*AggregationPart
	var topLeftrowCol RowCol
	var bottomRightRowCol RowCol
	if g.GridResolution.Compare(resolution) >= 0 {
		// get value from grid (lat, lon)
		if g.GetValueLatLon == nil {
			return errors.New("Grid.GetValueLatLon is not implemented")
		}
		returnVal, topLeftrowCol, bottomRightRowCol, err = g.GetValueLatLon(lat, lon)
		if err != nil {
			return err
		}
	} else if g.GridResolution.Compare(resolution) < 0 && agg.String() == "none" {
		returnVal = 0
	} else if g.GridResolution.Compare(resolution) < 0 && agg.String() != "none" {

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
				aggPart.SetIValue(aggrParts[i].InterpolatedValue)
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
	res, err := result.NewRes()
	if err != nil {
		return err
	}
	if val, ok := g.GridResolution.Value.(float64); ok {
		res.SetDegree(val)
	} else if val, ok := g.GridResolution.Value.(int64); ok {
		res.SetMeter(val)
	}
	return result.SetRes(res)
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

	var resolution Resolution = Resolution{}
	if call.Args().HasResolution() {

		resolutionMsg, err := call.Args().Resolution()
		if err != nil {
			return err
		}

		if resolutionMsg.Which() == grid.Grid_Resolution_Which_meter {
			resolution.Value = resolutionMsg.Meter()
		}
		if resolutionMsg.Which() == grid.Grid_Resolution_Which_degree {
			resolution.Value = resolutionMsg.Degree()
		}
		if !g.IsComparable(resolution) {
			return errors.New("Resolution is not comparable")
		}
	} else {
		resolution = g.GridResolution
	}
	agg := call.Args().Agg()
	includeAggParts := call.Args().IncludeAggParts()

	result, err := call.AllocResults()
	if err != nil {
		return err
	}
	var returnVal interface{}
	var aggrParts []*AggregationPart
	if g.GridResolution.Compare(resolution) >= 0 {
		if g.GetValueRowCol == nil {
			return errors.New("Grid.GetValueRowCol is not implemented")
		}
		// get value from grid (row, col)
		returnVal, err = g.GetValueRowCol(row, col)
		if err != nil {
			return err
		}
	} else if g.GridResolution.Compare(resolution) < 0 && agg.String() == "none" {
		returnVal = 0
	} else if g.GridResolution.Compare(resolution) < 0 && agg.String() != "none" {

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
				aggPart.SetIValue(part.InterpolatedValue)
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

func (g *Grid) Unit(c context.Context, call grid.Grid_unit) error {

	result, err := call.AllocResults()
	if err != nil {
		return err
	}

	return result.SetUnit(g.GridUnit)
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

	streamingCallback := &StreamingCallback{
		topLeft:      RowCol{},
		bottomRight:  RowCol{},
		currIndexRow: 0,
		currIndexCol: 0,
		g:            g,
	}
	if call.Args().HasTopLeft() {
		tl, err := call.Args().TopLeft()
		if err != nil {
			return err
		}
		streamingCallback.topLeft.Row = tl.Row()
		streamingCallback.topLeft.Col = tl.Col()
	} else {
		streamingCallback.topLeft.Row = 0
		streamingCallback.topLeft.Col = 0
	}
	if call.Args().HasBottomRight() {
		br, err := call.Args().BottomRight()
		if err != nil {
			return err
		}
		streamingCallback.bottomRight.Row = br.Row()
		streamingCallback.bottomRight.Col = br.Col()
	} else {
		streamingCallback.bottomRight.Row = g.NumRows - 1
		streamingCallback.bottomRight.Col = g.NumCols - 1
	}
	if streamingCallback.topLeft.Row > streamingCallback.bottomRight.Row {
		return errors.New("topLeft.Row > bottomRight.Row")
	}
	if streamingCallback.topLeft.Col > streamingCallback.bottomRight.Col {
		return errors.New("topLeft.Col > bottomRight.Col")
	}
	if streamingCallback.bottomRight.Row >= g.NumRows {
		return errors.New("bottomRight.Row >= NumRows")
	}
	if streamingCallback.bottomRight.Col >= g.NumCols {
		return errors.New("bottomRight.Col >= NumCols")
	}

	streamingCallback.currIndexRow = streamingCallback.topLeft.Row
	streamingCallback.currIndexCol = streamingCallback.topLeft.Col

	result, err := call.AllocResults()
	if err != nil {
		return err
	}

	sc := grid.Grid_Callback_ServerToClient(streamingCallback)
	err = result.SetCallback(sc)
	if err != nil {
		return err
	}
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

type StreamingCallback struct {
	topLeft      RowCol
	bottomRight  RowCol
	currIndexRow uint64
	currIndexCol uint64
	g            *Grid
}

func (cs *StreamingCallback) SendCells(ctx context.Context, call grid.Grid_Callback_sendCells) error {

	maxNumCells := call.Args().MaxCount()
	if maxNumCells <= 0 {
		return errors.New("maxNumCells <= 0")
	}

	result, err := call.AllocResults()
	if err != nil {
		return err
	}
	// calculate the number of cells to send
	numCellsToSend := uint64(0)
	if cs.currIndexRow != cs.bottomRight.Row || cs.currIndexCol != cs.bottomRight.Col {

		fullRows := (cs.bottomRight.Row - cs.currIndexRow) * (cs.bottomRight.Col - cs.topLeft.Col + 1)
		partialRow := cs.bottomRight.Col - cs.currIndexCol + 1
		numCellsToSend = fullRows + partialRow
	}
	if numCellsToSend > uint64(maxNumCells) {
		numCellsToSend = uint64(maxNumCells)
	}
	if numCellsToSend == 0 {
		return nil
	}
	if numCellsToSend > math.MaxInt32 {
		return errors.New("numCellsToSend > Int32.MaxValue")
	}
	locList, err := result.NewLocations(int32(numCellsToSend))
	if err != nil {
		return err
	}

	index := -1
	currRow := cs.currIndexRow
	currCol := cs.currIndexCol
	for ; index < int(maxNumCells-1) && currRow <= cs.bottomRight.Row; currRow++ {
		for ; index < int(maxNumCells-1) && currCol <= cs.bottomRight.Col; currCol++ {
			index++
			loc, err := grid.NewGrid_Location(locList.Segment())
			if err != nil {
				return err
			}

			rowCol, err := loc.NewRowCol()
			if err != nil {
				return err
			}
			rowCol.SetRow(currRow)
			rowCol.SetCol(currCol)

			val, err := loc.NewValue()
			if err != nil {
				return err
			}

			iVal, err := cs.g.GetValueRowCol(currRow, currCol)
			if err != nil {
				return err
			}
			err = SetGridValue(val, iVal)
			if err != nil {
				return err
			}
			latLon, err := loc.NewLatLonCoord()
			if err != nil {
				return err
			}
			latLonCoords, err := cs.g.RowColToLatLon(currRow, currCol)
			if err != nil {
				return err
			}
			latLon.SetLat(latLonCoords.Lat)
			latLon.SetLon(latLonCoords.Lon)

			err = locList.Set(index, loc)
			if err != nil {
				return err
			}
		}
		currCol = cs.topLeft.Col
	}

	err = result.SetLocations(locList)

	cs.currIndexRow = currRow
	cs.currIndexCol = currCol
	return err
}
