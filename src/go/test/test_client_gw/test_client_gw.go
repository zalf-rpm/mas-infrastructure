package main

import (
	"context"
	"flag"
	"fmt"
	"image"
	"image/color"
	"image/png"
	"math"
	"os"

	"github.com/mazznoer/colorgrad"
	"github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/grid"
	"github.com/zalf-rpm/mas-infrastructure/src/go/commonlib"
)

func main() {

	sturdyRef := flag.String("sturdyref", "", "sturdy ref of the service")
	imgFileName := flag.String("img", "streamedImg.png", "image file name")

	flag.Parse()

	if *sturdyRef == "" {
		fmt.Println("Please specify a sturdy ref.")
		return
	}
	conMgr := commonlib.NewConnectionManager()
	// create a connection to the restorer
	initialSdr, err := conMgr.TryConnect(*sturdyRef, 1, 1, false)
	if err != nil {
		fmt.Println(err)
		return
	}
	// get the grid service interface
	gridClient := grid.Grid(*initialSdr)
	// get size
	resultDim, relDim := gridClient.Dimension(context.Background(), func(p grid.Grid_dimension_Params) error {
		fmt.Println("get dimension")
		return nil
	})

	resultsDim, err := resultDim.Struct()
	if err != nil {
		fmt.Println(err)
		return
	}
	maxRows := resultsDim.Rows()
	maxCols := resultsDim.Cols()
	relDim()

	// get no data value
	var noData float64
	resultNoData, relNoData := gridClient.NoDataValue(context.Background(), func(p grid.Grid_noDataValue_Params) error {
		fmt.Println("noDataValue")
		return nil
	})
	resultsNoData, err := resultNoData.Struct()
	if err != nil {
		fmt.Println(err)
		return
	}

	if resultsNoData.HasNodata() {
		val, err := resultsNoData.Nodata()
		if err != nil {
			fmt.Println(err)
			return
		}
		if val.Which() == grid.Grid_Value_Which_f {
			noData = val.F()
		} else if val.Which() == grid.Grid_Value_Which_i {
			noData = float64(val.I())
		}
	} else {
		fmt.Println("no data value")
	}
	relNoData()

	// setup image
	img := generatePic(int(maxCols), int(maxRows))
	grad := colorgrad.Viridis()

	// stream cells
	resultStream, relStream := gridClient.StreamCells(context.Background(), func(p grid.Grid_streamCells_Params) error {
		topleft, err := p.NewTopLeft()

		if err != nil {
			fmt.Println(err)
			return err
		}
		topleft.SetRow(0)
		topleft.SetCol(0)
		bottomright, err := p.NewBottomRight()
		if err != nil {
			fmt.Println(err)
			return err
		}
		bottomright.SetRow(maxRows - 1)
		bottomright.SetCol(maxCols - 1)

		fmt.Println("StreamCells")
		return nil
	})

	resultsStream, err := resultStream.Struct()
	if err != nil {
		fmt.Println(err)
		return
	}
	if resultsStream.HasCallback() {

		callBack := resultsStream.Callback()
		var sendIdx uint64 = 0
		for ; sendIdx < maxRows; sendIdx++ {
			resFut, release := callBack.SendCells(context.Background(), func(p grid.Grid_Callback_sendCells_Params) error {
				p.SetMaxCount(int64(maxCols))
				return nil
			})
			res, err := resFut.Struct()
			if err != nil {
				fmt.Println(err)
				break
			}
			if !res.HasLocations() {
				break
			}

			locations, err := res.Locations()
			if err != nil {
				fmt.Println(err)
				break
			}

			for i := 0; i < locations.Len(); i++ {
				loc := locations.At(i)

				if !loc.HasRowCol() {
					fmt.Println("no row col")
					break
				}
				if !loc.HasValue() {
					fmt.Println("no value")
					break
				}
				rowCol, err := loc.RowCol()
				if err != nil {
					fmt.Println(err)
					break
				}
				row := rowCol.Row()
				col := rowCol.Col()
				val, err := loc.Value()
				if err != nil {
					fmt.Println(err)
					break
				}
				value := 0.0
				if val.Which() == grid.Grid_Value_Which_f {
					value = val.F()
					img.Set(int(col), int(maxRows-row), ToColor(value*(-1), noData*(-1), 0, 10, &grad))

				}
			}
			release()
		}
		callBack.Release()
	}
	relStream()

	resultUnit, relUnit := gridClient.Unit(context.Background(), func(p grid.Grid_unit_Params) error {
		fmt.Println("unit")
		return nil
	})
	resultsUnit, err := resultUnit.Struct()
	if err != nil {
		fmt.Println(err)
		return
	}
	if resultsUnit.HasUnit() {
		unit, err := resultsUnit.Unit()
		if err != nil {
			fmt.Println(err)
			return
		}
		fmt.Println(unit)
	}
	relUnit()

	gridClient.Release()
	saveImg(img, *imgFileName)
}

func generatePic(width, height int) *image.RGBA {

	upLeft := image.Point{0, 0}
	lowRight := image.Point{width, height}

	img := image.NewRGBA(image.Rectangle{upLeft, lowRight})
	return img
}

func saveImg(img *image.RGBA, imgName string) {
	// Encode as PNG.
	f, _ := os.Create(imgName)
	png.Encode(f, img)
}

func ToColor(val, nodata, minValue, maxValue float64, grad *colorgrad.Gradient) color.RGBA {
	if val == nodata {
		return color.RGBA{188, 190, 198, 0xff} // blank
	}
	valRange := maxValue - minValue
	if val > maxValue {
		val = maxValue
	}
	if val < minValue {
		val = minValue
	}
	perc := math.Abs((val - minValue) / valRange)
	r, g, b := grad.At(perc).RGB255()
	texture := color.RGBA{r, g, b, 0xff} // blank

	return texture

}
