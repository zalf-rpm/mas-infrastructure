package commonlib

import (
	"os"

	toml "github.com/pelletier/go-toml"
)

type Config struct {
	TLS struct {
		Use      bool     `toml:"use_tls"`
		KeyName  string   `toml:"key_name"`
		CertName string   `toml:"cert_name"`
		RootCAs  []string `toml:"root_cas"`
	}
}

func ReadConfig(configPath string) (*Config, error) {

	if configPath == "" {
		return DefaultConfig(), nil
	}
	_, err := os.Stat(configPath)
	if err != nil {
		return nil, err
	}
	fileData, err := os.ReadFile(configPath)
	if err != nil {
		return nil, err
	}
	config := DefaultConfig()

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
	}
}

func ConfigGen(configPath string) (*Config, error) {
	// read the config file, if it exists
	config, err := ReadConfig(configPath)
	if err != nil {
		if os.IsNotExist(err) {
			// create a default config file, if it does not exist and the flag is set
			config = DefaultConfig()
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
