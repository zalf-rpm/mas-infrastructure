using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Data.Soil
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfc682227304e2281UL)]
    public class SoilCharacteristicData : ICapnpSerializable
    {
        public const UInt64 typeId = 0xfc682227304e2281UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            List = reader.List?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Data.Soil.SoilCharacteristicData.Data>(_));
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.List.Init(List, (_s1, _v1) => _v1?.serialize(_s1));
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public IReadOnlyList<Mas.Data.Soil.SoilCharacteristicData.Data> List
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
            public IReadOnlyList<Mas.Data.Soil.SoilCharacteristicData.Data.READER> List => ctx.ReadList(0).Cast(Mas.Data.Soil.SoilCharacteristicData.Data.READER.create);
            public bool HasList => ctx.IsStructFieldNonNull(0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 1);
            }

            public ListOfStructsSerializer<Mas.Data.Soil.SoilCharacteristicData.Data.WRITER> List
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Data.Soil.SoilCharacteristicData.Data.WRITER>>(0);
                set => Link(0, value);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xeafaab57e025db63UL)]
        public class Data : ICapnpSerializable
        {
            public const UInt64 typeId = 0xeafaab57e025db63UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                SoilType = reader.SoilType;
                SoilRawDensity = reader.SoilRawDensity;
                AirCapacity = reader.AirCapacity;
                FieldCapacity = reader.FieldCapacity;
                NFieldCapacity = reader.NFieldCapacity;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.SoilType = SoilType;
                writer.SoilRawDensity = SoilRawDensity;
                writer.AirCapacity = AirCapacity;
                writer.FieldCapacity = FieldCapacity;
                writer.NFieldCapacity = NFieldCapacity;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string SoilType
            {
                get;
                set;
            }

            public short SoilRawDensity
            {
                get;
                set;
            }

            public byte AirCapacity
            {
                get;
                set;
            }

            public byte FieldCapacity
            {
                get;
                set;
            }

            public byte NFieldCapacity
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
                public string SoilType => ctx.ReadText(0, null);
                public short SoilRawDensity => ctx.ReadDataShort(0UL, (short)0);
                public byte AirCapacity => ctx.ReadDataByte(16UL, (byte)0);
                public byte FieldCapacity => ctx.ReadDataByte(24UL, (byte)0);
                public byte NFieldCapacity => ctx.ReadDataByte(32UL, (byte)0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public string SoilType
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public short SoilRawDensity
                {
                    get => this.ReadDataShort(0UL, (short)0);
                    set => this.WriteData(0UL, value, (short)0);
                }

                public byte AirCapacity
                {
                    get => this.ReadDataByte(16UL, (byte)0);
                    set => this.WriteData(16UL, value, (byte)0);
                }

                public byte FieldCapacity
                {
                    get => this.ReadDataByte(24UL, (byte)0);
                    set => this.WriteData(24UL, value, (byte)0);
                }

                public byte NFieldCapacity
                {
                    get => this.ReadDataByte(32UL, (byte)0);
                    set => this.WriteData(32UL, value, (byte)0);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe4eb0a9bb0e5bb53UL)]
    public class SoilCharacteristicModifier : ICapnpSerializable
    {
        public const UInt64 typeId = 0xe4eb0a9bb0e5bb53UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            List = reader.List?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Data.Soil.SoilCharacteristicModifier.Data>(_));
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.List.Init(List, (_s1, _v1) => _v1?.serialize(_s1));
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public IReadOnlyList<Mas.Data.Soil.SoilCharacteristicModifier.Data> List
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
            public IReadOnlyList<Mas.Data.Soil.SoilCharacteristicModifier.Data.READER> List => ctx.ReadList(0).Cast(Mas.Data.Soil.SoilCharacteristicModifier.Data.READER.create);
            public bool HasList => ctx.IsStructFieldNonNull(0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 1);
            }

            public ListOfStructsSerializer<Mas.Data.Soil.SoilCharacteristicModifier.Data.WRITER> List
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Data.Soil.SoilCharacteristicModifier.Data.WRITER>>(0);
                set => Link(0, value);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa968a46ccde8b1b4UL)]
        public class Data : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa968a46ccde8b1b4UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                SoilType = reader.SoilType;
                OrganicMatter = reader.OrganicMatter;
                AirCapacity = reader.AirCapacity;
                FieldCapacity = reader.FieldCapacity;
                NFieldCapacity = reader.NFieldCapacity;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.SoilType = SoilType;
                writer.OrganicMatter = OrganicMatter;
                writer.AirCapacity = AirCapacity;
                writer.FieldCapacity = FieldCapacity;
                writer.NFieldCapacity = NFieldCapacity;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string SoilType
            {
                get;
                set;
            }

            public float OrganicMatter
            {
                get;
                set;
            }

            public byte AirCapacity
            {
                get;
                set;
            }

            public byte FieldCapacity
            {
                get;
                set;
            }

            public byte NFieldCapacity
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
                public string SoilType => ctx.ReadText(0, null);
                public float OrganicMatter => ctx.ReadDataFloat(0UL, 0F);
                public byte AirCapacity => ctx.ReadDataByte(32UL, (byte)0);
                public byte FieldCapacity => ctx.ReadDataByte(40UL, (byte)0);
                public byte NFieldCapacity => ctx.ReadDataByte(48UL, (byte)0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public string SoilType
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public float OrganicMatter
                {
                    get => this.ReadDataFloat(0UL, 0F);
                    set => this.WriteData(0UL, value, 0F);
                }

                public byte AirCapacity
                {
                    get => this.ReadDataByte(32UL, (byte)0);
                    set => this.WriteData(32UL, value, (byte)0);
                }

                public byte FieldCapacity
                {
                    get => this.ReadDataByte(40UL, (byte)0);
                    set => this.WriteData(40UL, value, (byte)0);
                }

                public byte NFieldCapacity
                {
                    get => this.ReadDataByte(48UL, (byte)0);
                    set => this.WriteData(48UL, value, (byte)0);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9b169bc96bb3d24bUL)]
    public class CapillaryRiseRate : ICapnpSerializable
    {
        public const UInt64 typeId = 0x9b169bc96bb3d24bUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            List = reader.List?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Data.Soil.CapillaryRiseRate.Data>(_));
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.List.Init(List, (_s1, _v1) => _v1?.serialize(_s1));
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public IReadOnlyList<Mas.Data.Soil.CapillaryRiseRate.Data> List
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
            public IReadOnlyList<Mas.Data.Soil.CapillaryRiseRate.Data.READER> List => ctx.ReadList(0).Cast(Mas.Data.Soil.CapillaryRiseRate.Data.READER.create);
            public bool HasList => ctx.IsStructFieldNonNull(0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 1);
            }

            public ListOfStructsSerializer<Mas.Data.Soil.CapillaryRiseRate.Data.WRITER> List
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Data.Soil.CapillaryRiseRate.Data.WRITER>>(0);
                set => Link(0, value);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb78a89c58fad885dUL)]
        public class Data : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb78a89c58fad885dUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                SoilType = reader.SoilType;
                Distance = reader.Distance;
                Rate = reader.Rate;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.SoilType = SoilType;
                writer.Distance = Distance;
                writer.Rate = Rate;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string SoilType
            {
                get;
                set;
            }

            public byte Distance
            {
                get;
                set;
            }

            public float Rate
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
                public string SoilType => ctx.ReadText(0, null);
                public byte Distance => ctx.ReadDataByte(0UL, (byte)0);
                public float Rate => ctx.ReadDataFloat(32UL, 0F);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public string SoilType
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public byte Distance
                {
                    get => this.ReadDataByte(0UL, (byte)0);
                    set => this.WriteData(0UL, value, (byte)0);
                }

                public float Rate
                {
                    get => this.ReadDataFloat(32UL, 0F);
                    set => this.WriteData(32UL, value, 0F);
                }
            }
        }
    }
}