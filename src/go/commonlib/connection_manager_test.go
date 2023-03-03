package commonlib

import (
	"reflect"
	"testing"
)

func TestNewSturdyRefByString(t *testing.T) {
	type args struct {
		sturdyRef string
	}
	tests := []struct {
		name    string
		args    args
		want    string
		wantErr bool
	}{
		{
			name: "withSturdyRef",
			args: args{
				sturdyRef: "capnp://iHM7vB14BUhug6woXlvLzqjJu9b29xhSQkhYLZrYz3I=@192.168.56.1:58381/Mzk2YTdlYTEtODliMi00YTc5LTljNDItZWNmMWMzZDNjOTQ3",
			},
			want:    "capnp://iHM7vB14BUhug6woXlvLzqjJu9b29xhSQkhYLZrYz3I=@192.168.56.1:58381/Mzk2YTdlYTEtODliMi00YTc5LTljNDItZWNmMWMzZDNjOTQ3",
			wantErr: false,
		},
		{
			name: "connecion without sturdyRef",
			args: args{
				sturdyRef: "capnp://iHM7vB14BUhug6woXlvLzqjJu9b29xhSQkhYLZrYz3I=@192.168.56.1:58381",
			},
			want:    "capnp://iHM7vB14BUhug6woXlvLzqjJu9b29xhSQkhYLZrYz3I=@192.168.56.1:58381",
			wantErr: false,
		},
		{
			name: "failed to parse sturdyRef",
			args: args{
				sturdyRef: "capnp://Ga6MfZuG2SmRB-OqZy_vuUjLWB8KlTPO2lstySI2bqs@10.10.25.107:59606/M2QxN2RiMGUtYTJjZC00NWM3LWFiMjktZTk3MTMwNGNlZTc5",
			},
			want:    "capnp://Ga6MfZuG2SmRB-OqZy_vuUjLWB8KlTPO2lstySI2bqs=@10.10.25.107:59606/M2QxN2RiMGUtYTJjZC00NWM3LWFiMjktZTk3MTMwNGNlZTc5",
			wantErr: false,
		},
	}
	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			got, err := NewSturdyRefByString(tt.args.sturdyRef)
			if (err != nil) != tt.wantErr {
				t.Errorf("NewSturdyRefByString() error = %v, wantErr %v", err, tt.wantErr)
				return
			}
			if !tt.wantErr {
				sturdyRefAsString := got.String()
				if !reflect.DeepEqual(sturdyRefAsString, tt.want) {
					t.Errorf("NewSturdyRefByString() = %v, want %v", got, tt.want)
				}
			}
		})
	}
}
