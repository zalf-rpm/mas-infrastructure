package main

import (
	"bufio"
	"context"
	"flag"
	"fmt"
	"log"
	"os"
	"os/exec"
	"path/filepath"
	"strings"

	"github.com/google/uuid"
	"github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common"
)

// DEFAULTS:
const pathToChannelExe = "../../../cpp/common/_cmake_debug/Debug/channel.exe"

// const pathToChannelExe = "../../../go/fbp/channel/channel.exe"
const pathToMasDefault = "../../../"
const inDatasetSrDefault = "capnp://p1muS1-Eqy2eA1ijBMFDA_ouQ0-gwRJmvVZS6QYB52k=@192.168.56.1:55834/MmExYjgxYjYtMWM3YS00Y2MyLWFjNTMtNzA0MzIwNGRlZThj"
const pathToOutDirDefault = "../../../src/python/fbp/out/"

// fbp flow groundwater provider
func main() {
	pathToChannel := flag.String("pathToChannel", pathToChannelExe, "path to fbp channel")
	pathToMas := flag.String("pathMAS", pathToMasDefault, "path to mas root folder")
	inDatasetSr := flag.String("in_dataset_sr", inDatasetSrDefault, "input dataset sturdy ref")
	pathToOutDir := flag.String("path_to_out_dir", pathToOutDirDefault, "path to output directory")
	flag.Parse()

	// start fbp flow channels
	firstReaderSr := make(chan string)
	firstWriterSr := make(chan string)
	go setupFlowChannels(*pathToChannel, firstReaderSr, firstWriterSr)

	// wait for first reader and writer
	doneFR, doneFW := false, false
	first_reader_sr, first_writer_sr := "", ""
	for !(doneFR && doneFW) {
		select {
		case first_reader_sr = <-firstReaderSr:
			doneFR = true
		case first_writer_sr = <-firstWriterSr:
			doneFW = true
		}
	}
	fmt.Println("first reader: ", first_reader_sr)
	fmt.Println("first writer: ", first_writer_sr)

	conManager := NewConnectionManager()
	go conManager.Run()

	first_reader, err := conManager.TryConnect(first_reader_sr, 5, 5, true)
	if err != nil {
		log.Fatal(err)
	}
	firstReader := common.Channel_Reader(*first_reader)

	type flow struct {
		module1   string
		srNameOut string
		module2   string
		srNameIn  string
	}
	flowList := []flow{
		{"get_climate_locations", "out_sr", "timeseries_to_data", "in_sr"},
		{"timeseries_to_data", "out_sr", "aggregate_timeseries_data_monthly", "in_sr"},
		{"aggregate_timeseries_data_monthly", "out_sr_precip", "write_file_1", "in_sr"},
		{"aggregate_timeseries_data_monthly", "out_sr_tavg", "write_file_2", "in_sr"},
		{"aggregate_timeseries_data_monthly", "out_sr_globrad", "write_file_3", "in_sr"},
	}

	create_components := map[string]func([]string) *exec.Cmd{
		"get_climate_locations": func(srs []string) *exec.Cmd {
			args := []string{
				"python",
				fmt.Sprintf("%s/src/python/fbp/get_climate_locations.py", *pathToMas),
				fmt.Sprintf("dataset_sr=%s", *inDatasetSr),
				"continue_after_location_id=r:361/c:142",
				"no_of_locations_at_once=10000",
			}
			args = append(args, srs...)
			cmd := exec.Command(args[0], args[1:]...)
			return cmd
		},
		"timeseries_to_data": func(srs []string) *exec.Cmd {
			// args := []string{
			// 	fmt.Sprintf("%s/src/cpp/fbp/_cmake_debug/timeseries-to-data", *pathToMas),
			// 	"--in_type=capability",
			// 	"--subrange_start=2000-01-01",
			// 	"--subrange_end=2019-12-31",
			// 	"--subheader=tavg,globrad,precip",
			// }
			// for _, sr := range srs {
			// 	args = append(args, "--"+sr)
			// }
			args := []string{
				fmt.Sprintf("%s/src/python/fbp/timeseries_to_data.py", *pathToMas),
				"in_type=capability",
				"subrange_start=2000-01-01",
				"subrange_end=2019-12-31",
				"subheader=tavg,globrad,precip",
			}

			args = append(args, srs...)
			cmd := exec.Command(args[0], args[1:]...)

			return cmd
		},
		"aggregate_timeseries_data_monthly": func(srs []string) *exec.Cmd {
			args := []string{
				"python",
				fmt.Sprintf("%s/src/python/fbp/aggregate_timeseries_data_monthly.py", *pathToMas),
			}
			args = append(args, srs...)
			cmd := exec.Command(args[0], args[1:]...)
			return cmd
		},
		"write_file_1": func(srs []string) *exec.Cmd {
			args := []string{
				"python",
				fmt.Sprintf("%s/src/python/fbp/write_file.py", *pathToMas),
				"append=true",
				fmt.Sprintf("path_to_out_dir=%s", *pathToOutDir),
			}
			args = append(args, srs...)
			cmd := exec.Command(args[0], args[1:]...)
			return cmd
		},
		"write_file_2": func(srs []string) *exec.Cmd {
			args := []string{
				"python",
				fmt.Sprintf("%s/src/python/fbp/write_file.py", *pathToMas),
				"append=true",
				fmt.Sprintf("path_to_out_dir=%s", *pathToOutDir),
			}
			args = append(args, srs...)
			cmd := exec.Command(args[0], args[1:]...)
			return cmd
		},
		"write_file_3": func(srs []string) *exec.Cmd {
			args := []string{
				"python",
				fmt.Sprintf("%s/src/python/fbp/write_file.py", *pathToMas),
				"append=true",
				fmt.Sprintf("path_to_out_dir=%s", *pathToOutDir),
			}
			args = append(args, srs...)
			cmd := exec.Command(args[0], args[1:]...)
			return cmd
		},
	}
	compNameToComponent := map[string]map[string]string{}
	chanIdToInOutSrNames := map[string]map[string]string{}

	channels := []*exec.Cmd{}

	// start all channels for the flow
	for _, step := range flowList {
		chan_id := step.module1 + "->" + step.module2
		// start channel
		newchan := startChannel(*pathToChannel, chan_id+"|"+first_writer_sr, chan_id)
		channels = append(channels, newchan)

		chanIdToInOutSrNames[chan_id] = map[string]string{"out": step.srNameOut, "in": step.srNameIn}
		compNameToComponent[step.module1][step.srNameOut] = ""
		compNameToComponent[step.module2][step.srNameIn] = ""
	}

	// collect channel sturdy refs in order to start components
	components := map[string]*exec.Cmd{}
	for {
		// read from channels pairs of (component tuples, StartupInfo)
		fut, rel := firstReader.Read(context.Background(), func(c common.Channel_Reader_read_Params) error {
			return nil
		})
		defer rel()
		result, err := fut.Struct()
		if err != nil {
			log.Fatal(err)
		}
		p, err := result.Value()
		if err != nil {
			log.Fatal(err)
		}
		pair := common.Pair(p.Struct())

		idPtr, err := pair.Fst()
		if err != nil {
			log.Fatal(err)
		}
		id := idPtr.Text()
		//split id into conecting components
		token := strings.SplitN(id, "->", 2)
		start_comp_name, end_comp_name := token[0], token[1]

		// there should be code to start the components
		_, ok1 := create_components[start_comp_name]
		_, ok2 := create_components[end_comp_name]
		if ok1 && ok2 {
			infoPtr, err := pair.Snd()
			if err != nil {
				log.Fatal(err)
			}
			info := common.Channel_StartupInfo(infoPtr.Struct())

			start_sr_name := chanIdToInOutSrNames[id]["out"]
			end_sr_name := chanIdToInOutSrNames[id]["in"]

			iWSRs, err := info.WriterSRs()
			if err != nil {
				log.Fatal(err)
			}
			iWSRsstr, _ := iWSRs.At(0)
			compNameToComponent[start_comp_name][start_sr_name] = fmt.Sprintf("%s=%s", start_sr_name, iWSRsstr)
			iRSRs, err := info.ReaderSRs()
			if err != nil {
				log.Fatal(err)
			}
			iRSRsstr, _ := iRSRs.At(0)
			compNameToComponent[end_comp_name][end_sr_name] = fmt.Sprintf("%s=%s", end_sr_name, iRSRsstr)

			// sturdy refs for all ports of start component are available
			checkComponents := func(componentName string) (srs []string, notEmtpy bool) {
				srs = make([]string, 0, len(compNameToComponent[componentName]))
				notEmtpy = true
				for _, sr := range compNameToComponent[componentName] {
					if sr == "" {
						notEmtpy = false
						break
					}
					srs = append(srs, sr)
				}
				return srs, notEmtpy
			}

			if srs, notEmtpy := checkComponents(start_comp_name); notEmtpy {
				components[start_comp_name] = create_components[start_comp_name](srs)
				components[start_comp_name].Start()
			} else {
				log.Fatal("Flow Error: not all sturdy refs for component", start_comp_name, "are available")
			}

			if srs, notEmtpy := checkComponents(end_comp_name); notEmtpy {
				components[end_comp_name] = create_components[end_comp_name](srs)
				components[end_comp_name].Start()
			} else {
				log.Fatal("Flow Error: not all sturdy refs for component", end_comp_name, "are available")
			}

			if len(components) == len(compNameToComponent) {
				break
			}
		} else {
			log.Fatal("Flow Error: component not found")
		}

	}
	// wait for all components to finish
	for _, component := range components {
		component.Wait()
	}
	fmt.Println("Flow Info: all components finished")

	// terminate all channels
	for _, channel := range channels {
		channel.Process.Kill()
	}
	fmt.Println("Flow Info: all channels terminated")

}

func startChannel(pathToChannel string, writer_sr string, name string) *exec.Cmd {
	if name == "" {
		name = uuid.New().String()
	}

	args := []string{
		fmt.Sprintf("--name=chan_%s", name),
		fmt.Sprintf("--startup_info_writer_sr=%s", writer_sr),
	}

	cmd := exec.Command(pathToChannel, args...)
	return cmd
}

// start fbp flow channels
func setupFlowChannels(pathToChannel string, firstReaderSr, firstWriterSr chan<- string) {

	// start external process
	args := []string{
		fmt.Sprintf("--name=chan_%s", uuid.New().String()),
		"--output_srs",
		"--verbose",
	}
	absPath, err := filepath.Abs(pathToChannel)
	if err != nil {
		fmt.Printf("Failed to resolve path to channel '%v '", pathToChannel)
		return
	}
	// check if file exists
	if _, err := os.Stat(absPath); os.IsNotExist(err) {
		fmt.Printf("File does not exist, pathToChannel: '%v'", absPath)
	}

	cmd := exec.Command(absPath, args...)
	cmdOut, err := cmd.StdoutPipe()
	if err != nil {
		fmt.Printf("Failed to start channel '%v '", cmd)
		return
	}

	outScanner := bufio.NewScanner(cmdOut)
	outScanner.Split(bufio.ScanLines)

	go func() {
		// read stdout for writer and reader
		channelNameReceived := 0
		for outScanner.Scan() {
			text := outScanner.Text()
			fmt.Println("Channel Out: ", text)
			if channelNameReceived < 2 {
				s := strings.SplitN(text, "=", 2)
				if len(s) == 2 {
					id := s[0]
					sr := s[1]
					if id == "readerSR" {
						firstReaderSr <- sr
						channelNameReceived++
					} else if id == "writerSR" {
						firstWriterSr <- sr
						channelNameReceived++
					}
				}
			}
		}
	}()

	cmdErr, err := cmd.StderrPipe()
	if err != nil {
		fmt.Printf("Failed to start channel '%v '", cmd)
		return
	}
	outErrScanner := bufio.NewScanner(cmdErr)
	outErrScanner.Split(bufio.ScanLines)

	go func() {
		// read stdout for writer and reader
		for outErrScanner.Scan() {
			text := outErrScanner.Text()
			fmt.Println("Std ERR Out: ", text)
		}
	}()

	err = cmd.Start()
	if err != nil {
		log.Fatal(err)
	}
	err = cmd.Wait()

	if err != nil {
		log.Fatal(err)
	}
}
