using Capnp;
using Capnp.Rpc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Rpc
{
    [TypeId(0xd3e8862096a899f8UL)]
    public class Geo : ICapnpSerializable
    {
        public const UInt64 typeId = 0xd3e8862096a899f8UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public struct READER
        {
            readonly DeserializerState ctx;
            public READER(DeserializerState ctx)
            {
                this.ctx = ctx;
            }

            public static READER create(DeserializerState ctx) => new READER(ctx);
            public static implicit operator DeserializerState(READER reader) => reader.ctx;
            public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 0);
            }
        }

        [TypeId(0xa32f5a379668969dUL)]
        public class EPSG : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa32f5a379668969dUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [TypeId(0xe1d4f73f7e0b96abUL)]
        public class UTMCoord : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe1d4f73f7e0b96abUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Zone = reader.Zone;
                LatitudeBand = reader.LatitudeBand;
                R = reader.R;
                H = reader.H;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Zone = Zone;
                writer.LatitudeBand = LatitudeBand;
                writer.R = R;
                writer.H = H;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public byte Zone
            {
                get;
                set;
            }

            public string LatitudeBand
            {
                get;
                set;
            }

            public long R
            {
                get;
                set;
            }

            public long H
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public byte Zone => ctx.ReadDataByte(0UL, (byte)0);
                public string LatitudeBand => ctx.ReadText(0, "");
                public long R => ctx.ReadDataLong(64UL, 0L);
                public long H => ctx.ReadDataLong(128UL, 0L);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(3, 1);
                }

                public byte Zone
                {
                    get => this.ReadDataByte(0UL, (byte)0);
                    set => this.WriteData(0UL, value, (byte)0);
                }

                public string LatitudeBand
                {
                    get => this.ReadText(0, "");
                    set => this.WriteText(0, value, "");
                }

                public long R
                {
                    get => this.ReadDataLong(64UL, 0L);
                    set => this.WriteData(64UL, value, 0L);
                }

                public long H
                {
                    get => this.ReadDataLong(128UL, 0L);
                    set => this.WriteData(128UL, value, 0L);
                }
            }
        }

        [TypeId(0x8fc9e37d5436bd5cUL)]
        public class LatLonCoord : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8fc9e37d5436bd5cUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Lat = reader.Lat;
                Lon = reader.Lon;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Lat = Lat;
                writer.Lon = Lon;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double Lat
            {
                get;
                set;
            }

            public double Lon
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public double Lat => ctx.ReadDataDouble(0UL, 0);
                public double Lon => ctx.ReadDataDouble(64UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(2, 0);
                }

                public double Lat
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public double Lon
                {
                    get => this.ReadDataDouble(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }
            }
        }

        [TypeId(0xe4061d9b3f7347dcUL)]
        public class GKCoord : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe4061d9b3f7347dcUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                MeridianNo = reader.MeridianNo;
                R = reader.R;
                H = reader.H;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.MeridianNo = MeridianNo;
                writer.R = R;
                writer.H = H;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public byte MeridianNo
            {
                get;
                set;
            }

            public long R
            {
                get;
                set;
            }

            public long H
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public byte MeridianNo => ctx.ReadDataByte(0UL, (byte)0);
                public long R => ctx.ReadDataLong(64UL, 0L);
                public long H => ctx.ReadDataLong(128UL, 0L);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(3, 0);
                }

                public byte MeridianNo
                {
                    get => this.ReadDataByte(0UL, (byte)0);
                    set => this.WriteData(0UL, value, (byte)0);
                }

                public long R
                {
                    get => this.ReadDataLong(64UL, 0L);
                    set => this.WriteData(64UL, value, 0L);
                }

                public long H
                {
                    get => this.ReadDataLong(128UL, 0L);
                    set => this.WriteData(128UL, value, 0L);
                }
            }
        }

        [TypeId(0xbc80f2de675ff32bUL)]
        public class Coord : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbc80f2de675ff32bUL;
            public enum WHICH : ushort
            {
                Gk = 0,
                Latlon = 1,
                Utm = 2,
                undefined = 65535
            }

            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                switch (reader.which)
                {
                    case WHICH.Gk:
                        Gk = CapnpSerializable.Create<Mas.Rpc.Geo.GKCoord>(reader.Gk);
                        break;
                    case WHICH.Latlon:
                        Latlon = CapnpSerializable.Create<Mas.Rpc.Geo.LatLonCoord>(reader.Latlon);
                        break;
                    case WHICH.Utm:
                        Utm = CapnpSerializable.Create<Mas.Rpc.Geo.UTMCoord>(reader.Utm);
                        break;
                }

                applyDefaults();
            }

            private WHICH _which = WHICH.undefined;
            private object _content;
            public WHICH which
            {
                get => _which;
                set
                {
                    if (value == _which)
                        return;
                    _which = value;
                    switch (value)
                    {
                        case WHICH.Gk:
                            _content = null;
                            break;
                        case WHICH.Latlon:
                            _content = null;
                            break;
                        case WHICH.Utm:
                            _content = null;
                            break;
                    }
                }
            }

            public void serialize(WRITER writer)
            {
                writer.which = which;
                switch (which)
                {
                    case WHICH.Gk:
                        Gk?.serialize(writer.Gk);
                        break;
                    case WHICH.Latlon:
                        Latlon?.serialize(writer.Latlon);
                        break;
                    case WHICH.Utm:
                        Utm?.serialize(writer.Utm);
                        break;
                }
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Rpc.Geo.GKCoord Gk
            {
                get => _which == WHICH.Gk ? (Mas.Rpc.Geo.GKCoord)_content : null;
                set
                {
                    _which = WHICH.Gk;
                    _content = value;
                }
            }

            public Mas.Rpc.Geo.LatLonCoord Latlon
            {
                get => _which == WHICH.Latlon ? (Mas.Rpc.Geo.LatLonCoord)_content : null;
                set
                {
                    _which = WHICH.Latlon;
                    _content = value;
                }
            }

            public Mas.Rpc.Geo.UTMCoord Utm
            {
                get => _which == WHICH.Utm ? (Mas.Rpc.Geo.UTMCoord)_content : null;
                set
                {
                    _which = WHICH.Utm;
                    _content = value;
                }
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public WHICH which => (WHICH)ctx.ReadDataUShort(0U, (ushort)0);
                public Mas.Rpc.Geo.GKCoord.READER Gk => which == WHICH.Gk ? ctx.ReadStruct(0, Mas.Rpc.Geo.GKCoord.READER.create) : default;
                public Mas.Rpc.Geo.LatLonCoord.READER Latlon => which == WHICH.Latlon ? ctx.ReadStruct(0, Mas.Rpc.Geo.LatLonCoord.READER.create) : default;
                public Mas.Rpc.Geo.UTMCoord.READER Utm => which == WHICH.Utm ? ctx.ReadStruct(0, Mas.Rpc.Geo.UTMCoord.READER.create) : default;
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public WHICH which
                {
                    get => (WHICH)this.ReadDataUShort(0U, (ushort)0);
                    set => this.WriteData(0U, (ushort)value, (ushort)0);
                }

                public Mas.Rpc.Geo.GKCoord.WRITER Gk
                {
                    get => which == WHICH.Gk ? BuildPointer<Mas.Rpc.Geo.GKCoord.WRITER>(0) : default;
                    set => Link(0, value);
                }

                public Mas.Rpc.Geo.LatLonCoord.WRITER Latlon
                {
                    get => which == WHICH.Latlon ? BuildPointer<Mas.Rpc.Geo.LatLonCoord.WRITER>(0) : default;
                    set => Link(0, value);
                }

                public Mas.Rpc.Geo.UTMCoord.WRITER Utm
                {
                    get => which == WHICH.Utm ? BuildPointer<Mas.Rpc.Geo.UTMCoord.WRITER>(0) : default;
                    set => Link(0, value);
                }
            }
        }
    }
}