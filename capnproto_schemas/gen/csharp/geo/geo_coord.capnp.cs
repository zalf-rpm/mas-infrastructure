using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Rpc.Geo
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe529b4deb322ece8UL)]
    public enum CoordType : ushort
    {
        gk,
        utm,
        latlon
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb79427a74eb97fc0UL)]
    public class EPSG : ICapnpSerializable
    {
        public const UInt64 typeId = 0xb79427a74eb97fc0UL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xeb1acd255e40f049UL)]
    public class UTMCoord : ICapnpSerializable
    {
        public const UInt64 typeId = 0xeb1acd255e40f049UL;
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
            public string LatitudeBand => ctx.ReadText(0, null);
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
                get => this.ReadText(0, null);
                set => this.WriteText(0, value, null);
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xecf1fc3039cc8ffbUL)]
    public class LatLonCoord : ICapnpSerializable
    {
        public const UInt64 typeId = 0xecf1fc3039cc8ffbUL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x97ff7d61786091aeUL)]
    public class GKCoord : ICapnpSerializable
    {
        public const UInt64 typeId = 0x97ff7d61786091aeUL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc88fb91c1e6986e2UL)]
    public class Point2D : ICapnpSerializable
    {
        public const UInt64 typeId = 0xc88fb91c1e6986e2UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            X = reader.X;
            Y = reader.Y;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.X = X;
            writer.Y = Y;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public long X
        {
            get;
            set;
        }

        public long Y
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
            public long X => ctx.ReadDataLong(0UL, 0L);
            public long Y => ctx.ReadDataLong(64UL, 0L);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(2, 0);
            }

            public long X
            {
                get => this.ReadDataLong(0UL, 0L);
                set => this.WriteData(0UL, value, 0L);
            }

            public long Y
            {
                get => this.ReadDataLong(64UL, 0L);
                set => this.WriteData(64UL, value, 0L);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb8f6a6192a7359f8UL)]
    public class Coord : ICapnpSerializable
    {
        public const UInt64 typeId = 0xb8f6a6192a7359f8UL;
        public enum WHICH : ushort
        {
            Gk = 0,
            Latlon = 1,
            Utm = 2,
            P2D = 3,
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
                case WHICH.P2D:
                    P2D = CapnpSerializable.Create<Mas.Rpc.Geo.Point2D>(reader.P2D);
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
                    case WHICH.P2D:
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
                case WHICH.P2D:
                    P2D?.serialize(writer.P2D);
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

        public Mas.Rpc.Geo.Point2D P2D
        {
            get => _which == WHICH.P2D ? (Mas.Rpc.Geo.Point2D)_content : null;
            set
            {
                _which = WHICH.P2D;
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
            public bool HasGk => ctx.IsStructFieldNonNull(0);
            public Mas.Rpc.Geo.LatLonCoord.READER Latlon => which == WHICH.Latlon ? ctx.ReadStruct(0, Mas.Rpc.Geo.LatLonCoord.READER.create) : default;
            public bool HasLatlon => ctx.IsStructFieldNonNull(0);
            public Mas.Rpc.Geo.UTMCoord.READER Utm => which == WHICH.Utm ? ctx.ReadStruct(0, Mas.Rpc.Geo.UTMCoord.READER.create) : default;
            public bool HasUtm => ctx.IsStructFieldNonNull(0);
            public Mas.Rpc.Geo.Point2D.READER P2D => which == WHICH.P2D ? ctx.ReadStruct(0, Mas.Rpc.Geo.Point2D.READER.create) : default;
            public bool HasP2D => ctx.IsStructFieldNonNull(0);
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

            public Mas.Rpc.Geo.Point2D.WRITER P2D
            {
                get => which == WHICH.P2D ? BuildPointer<Mas.Rpc.Geo.Point2D.WRITER>(0) : default;
                set => Link(0, value);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb952dbe83866da4aUL)]
    public class RectBounds<TCoordinateType> : ICapnpSerializable where TCoordinateType : class
    {
        public const UInt64 typeId = 0xb952dbe83866da4aUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Tl = CapnpSerializable.Create<TCoordinateType>(reader.Tl);
            Br = CapnpSerializable.Create<TCoordinateType>(reader.Br);
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Tl.SetObject(Tl);
            writer.Br.SetObject(Br);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public TCoordinateType Tl
        {
            get;
            set;
        }

        public TCoordinateType Br
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
            public DeserializerState Tl => ctx.StructReadPointer(0);
            public DeserializerState Br => ctx.StructReadPointer(1);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 2);
            }

            public DynamicSerializerState Tl
            {
                get => BuildPointer<DynamicSerializerState>(0);
                set => Link(0, value);
            }

            public DynamicSerializerState Br
            {
                get => BuildPointer<DynamicSerializerState>(1);
                set => Link(1, value);
            }
        }
    }
}