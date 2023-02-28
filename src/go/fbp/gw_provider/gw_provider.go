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
	"time"

	"github.com/google/uuid"
	"github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common"
)

// DEFAULTS:
const pathToChannelExe = "../../../cpp/common/_cmake_debug/Debug/channel.exe"

// const pathToChannelExe = "../../../go/fbp/channel/channel.exe"
const pathToMasDefault = "../../../../"
const inDatasetSrDefault = "capnp://ipFwgkCH_o-8yWOkJb2BCGCPvfGS_NsSC7tY4QYLrZ8=@192.168.56.1:51846/MTViOTE4M2UtOTFmYy00NDZiLWIxYjYtYmEyM2E1MjA2Yzk0"
const pathToOutDirDefault = "./out/"

// fbp flow groundwater provider
func main() {
	pathToChannel := flag.String("pathToChannel", pathToChannelExe, "path to fbp channel")
	pathToMas := flag.String("pathMAS", pathToMasDefault, "path to mas root folder")
	inDatasetSr := flag.String("in_dataset_sr", inDatasetSrDefault, "input dataset sturdy ref")
	pathToOutDir := flag.String("path_to_out_dir", pathToOutDirDefault, "path to output directory")
	flag.Parse()

	// create output directory
	if _, err := os.Stat(*pathToOutDir); os.IsNotExist(err) {
		err = os.MkdirAll(*pathToOutDir, 0755)
		if err != nil {
			log.Fatal(err)
		}
	}

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

	// start fbp flow channels
	firstReaderSr := make(chan string)
	firstWriterSr := make(chan string)
	channels := make([]chan bool, 0, len(flowList))
	firstKill := make(chan bool)
	channels = append(channels, firstKill)
	go setupFlowChannels(*pathToChannel, firstReaderSr, firstWriterSr, firstKill)

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

	// // convert to sturdy ref and back
	// first_reader_srSR, _ := NewSturdyRefByString(first_reader_sr)
	// // reset host to localhost
	// first_reader_srSR.vat.address.host = "127.0.0.1"
	// first_reader_sr = first_reader_srSR.String()
	// firstWriterSrSR, _ := NewSturdyRefByString(first_writer_sr)
	// firstWriterSrSR.vat.address.host = "127.0.0.1"
	// first_writer_sr = firstWriterSrSR.String()

	conManager := NewConnectionManager()
	go conManager.Run()

	first_reader, err := conManager.TryConnect(first_reader_sr, 5, 5, true)
	if err != nil {
		log.Fatal(err)
	}
	firstReader := common.Channel_Reader(*first_reader)

	// verify that all components exist
	getComponentPath := func(module string) string {
		executable := fmt.Sprintf("%s/%s", *pathToMas, module)

		executable, err := filepath.Abs(executable)
		if err != nil {
			log.Fatal(err)
		}
		if _, err := os.Stat(executable); os.IsNotExist(err) {
			log.Fatal(err)
		}
		return executable
	}
	create_components := map[string]func([]string) *exec.Cmd{
		"get_climate_locations": func(srs []string) *exec.Cmd {
			args := []string{
				"python",
				getComponentPath("src/python/fbp/get_climate_locations.py"),
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
				"python",
				getComponentPath("src/python/fbp/timeseries_to_data.py"),
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
				getComponentPath("src/python/fbp/aggregate_timeseries_data_monthly.py"),
			}
			args = append(args, srs...)
			cmd := exec.Command(args[0], args[1:]...)
			return cmd
		},
		"write_file_1": func(srs []string) *exec.Cmd {
			args := []string{
				"python",
				getComponentPath("src/python/fbp/write_file.py"),
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
				getComponentPath("/src/python/fbp/write_file.py"),
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
				getComponentPath("src/python/fbp/write_file.py"),
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

	// start all channels for the flow
	for _, step := range flowList {
		chan_id := step.module1 + "->" + step.module2
		// start channel
		newChan := make(chan bool)
		channels = append(channels, newChan)
		go startChannel(*pathToChannel, chan_id+"|"+first_writer_sr, chan_id, newChan)

		time.Sleep(2 * time.Second)
		chanIdToInOutSrNames[chan_id] = map[string]string{"out": step.srNameOut, "in": step.srNameIn}

		compNameToComponent[step.module1] = map[string]string{step.srNameOut: ""}

		compNameToComponent[step.module2] = map[string]string{step.srNameIn: ""}
	}

	// sleep for a while to let channels start
	time.Sleep(1 * time.Second)

	// collect channel sturdy refs in order to start components
	componentsStarted := 0
	componentDone := make(chan bool)
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
			iWSRsstr, err := iWSRs.At(0)
			if err != nil || iWSRsstr == "" {
				log.Fatal("iWSRsstr is empty")
			}
			compNameToComponent[start_comp_name][start_sr_name] = fmt.Sprintf("%s=%s", start_sr_name, iWSRsstr)
			iRSRs, err := info.ReaderSRs()
			if err != nil {
				log.Fatal(err)
			}
			iRSRsstr, err := iRSRs.At(0)
			if err != nil || iRSRsstr == "" {
				log.Fatal("iRSRsstr is empty")
			}
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
				go startComponent(start_comp_name, create_components[start_comp_name](srs), componentDone)
				componentsStarted++
			}

			if srs, notEmtpy := checkComponents(end_comp_name); notEmtpy {
				go startComponent(end_comp_name, create_components[end_comp_name](srs), componentDone)
				componentsStarted++
			}

			if componentsStarted == len(compNameToComponent) {
				break
			}
		} else {
			log.Fatal("Flow Error: component not found")
		}

	}
	// wait for all components to finish
	numComponentsFinished := 0
	for numComponentsFinished < componentsStarted {
		<-componentDone
		numComponentsFinished++
	}
	fmt.Println("Flow Info: all components finished")

	// terminate all channels
	for _, killChannel := range channels {
		killChannel <- true
	}

	fmt.Println("Flow Info: all channels terminated")
	time.Sleep(5 * time.Second)
}

func startComponent(name string, cmd *exec.Cmd, done chan bool) {

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
			fmt.Println(name, "stderr Out: ", text)
		}
	}()

	err = cmd.Start()
	if err != nil {
		log.Println(name, err)
		//log.Fatal(name, err)
		done <- false
		return
	}
	err = cmd.Wait()

	if err != nil {
		log.Println(name, err)
	}
	done <- true
}

func startChannel(pathToChannel string, writer_sr string, name string, killChan <-chan bool) {
	if name == "" {
		name = uuid.New().String()
	}

	args := []string{
		fmt.Sprintf("--name=chan_%s", name),
		fmt.Sprintf("--startup_info_writer_sr=%s", writer_sr),
		"--verbose",
	}

	cmd := exec.Command(pathToChannel, args...)

	go func() {
		<-killChan // wait for kill signal
		if cmd.Process != nil {
			err := cmd.Process.Kill()
			if err != nil {
				log.Println(name, err)
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
			fmt.Println(name, "stderr Out: ", text)
		}
	}()

	err = cmd.Start()
	if err != nil {
		log.Fatal(name, err)
	}
	err = cmd.Wait()

	if err != nil {
		log.Println(name, err)
	}

}

// start fbp flow channels
func setupFlowChannels(pathToChannel string, firstReaderSr, firstWriterSr chan<- string, killChan <-chan bool) {

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
			fmt.Println("First Channel: ", text)
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
			fmt.Println("First strErr: ", text)
		}
	}()
	go func() {
		<-killChan // wait for kill signal
		if cmd.Process != nil {
			err := cmd.Process.Kill()
			if err != nil {
				log.Println("First", err)
			}
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
