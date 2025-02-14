package commonlib

import (
	"os"

	toml "github.com/pelletier/go-toml"
)

type ConfigConfigurator interface {
	GetDefaultConfig() *Config
}

type Config struct {
	TLS struct {
		Use      bool     `toml:"use_tls"`
		KeyName  string   `toml:"key_name"`
		CertName string   `toml:"cert_name"`
		RootCAs  []string `toml:"root_cas"`
	}
	Data map[string]interface{}
}

func ReadConfig(configPath string, conf ConfigConfigurator) (*Config, error) {
	var config *Config
	if conf != nil {
		config = conf.GetDefaultConfig()
	} else {
		config = DefaultConfig()
	}

	if configPath == "" {
		return config, nil
	}
	_, err := os.Stat(configPath)
	if err != nil {
		return nil, err
	}
	fileData, err := os.ReadFile(configPath)
	if err != nil {
		return nil, err
	}

	// file data should be in toml format
	err = toml.Unmarshal(fileData, config)
	if err != nil {
		return nil, err
	}
	return config, nil
}

func (c *Config) WriteConfig(configPath string) error {
	fileData, err := toml.Marshal(c)
	if err != nil {
		return err
	}
	file, err := os.OpenFile(configPath, os.O_CREATE|os.O_WRONLY, 0644)
	if err != nil {
		return err
	}
	_, err = file.Write(fileData)

	return err
}

func DefaultConfig() *Config {
	return &Config{
		TLS: struct {
			Use      bool     "toml:\"use_tls\""
			KeyName  string   "toml:\"key_name\""
			CertName string   "toml:\"cert_name\""
			RootCAs  []string "toml:\"root_cas\""
		}{
			Use:      false,
			KeyName:  "config/server.key",
			CertName: "config/server.crt",
			RootCAs:  []string{"config/ca.crt"},
		},

		Data: map[string]interface{}{
			"Service": map[string]interface{}{
				"Name":        "service1",
				"Id":          "service1Id",
				"Description": "a service1 Description",
				"Host":        "localhost",
				"Port":        0,
			},
		},
	}
}

func ConfigGen(configPath string, conf ConfigConfigurator) (*Config, error) {
	// read the config file, if it exists
	config, err := ReadConfig(configPath, conf)
	if err != nil {
		if os.IsNotExist(err) {
			// create a default config file, if it does not exist and the flag is set
			if conf != nil {
				config = conf.GetDefaultConfig()
			} else {
				config = DefaultConfig()
			}
			err = config.WriteConfig(configPath)
			if err != nil {
				return nil, err
			}
		} else {
			return nil, err
		}
	}
	return config, nil
}
