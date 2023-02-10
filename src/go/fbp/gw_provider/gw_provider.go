package main

import (
	"bufio"
	"flag"
	"fmt"
	"log"
	"os/exec"
	"strings"

	uuid "github.com/google/uuid"
)

// fbp flow groundwater provider
func main() {
	pathToChannel := flag.String("path", "", "path to fbp channel")
	flag.Parse()

	// start fbp flow channels
	firstReaderSr := make(chan string)
	firstWriterSr := make(chan string)
	go setupFlowChannels(*pathToChannel, firstReaderSr, firstWriterSr)

	// wait for first reader and writer
	fr, fw := <-firstReaderSr, <-firstWriterSr
	fmt.Println("first reader: ", fr)
	fmt.Println("first writer: ", fw)

	//conManager := common.ConnectionManager()

	//first_reader = conManager.try_connect(first_reader_sr, cast_as=common_capnp.Channel.Reader)

}

// start fbp flow channels
func setupFlowChannels(pathToChannel string, firstReaderSr, firstWriterSr chan<- string) {

	// start external process
	args := []string{
		fmt.Sprintf("--name=chan_%s", uuid.New().String()),
		"--output_srs",
	}
	cmd := exec.Command(pathToChannel, args...)
	cmdOut, err := cmd.StdoutPipe()
	if err != nil {
		fmt.Printf("Failed to start channel '%v '", cmd)
		return
	}

	outScanner := bufio.NewScanner(cmdOut)
	outScanner.Split(bufio.ScanLines)

	go func() {
		// read stdout for writer and reader
		for outScanner.Scan() {
			text := outScanner.Text()
			s := strings.SplitN(text, "=", 1)
			if len(s) == 2 {
				id := s[0]
				sr := s[1]
				if id == "readerSR" {
					firstReaderSr <- sr
				} else if id == "writerSR" {
					firstWriterSr <- sr
				} else {
					fmt.Println("Unknown id: ", id)
				}
			}
		}
	}()

	err = cmd.Wait()

	if err != nil {
		log.Fatal(err)
	}

}
