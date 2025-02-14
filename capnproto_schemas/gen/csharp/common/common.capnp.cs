using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Schema.Common
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd4cb7ecbfe03dad3UL)]
    public class IdInformation : ICapnpSerializable
    {
        public const UInt64 typeId = 0xd4cb7ecbfe03dad3UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Id = reader.Id;
            Name = reader.Name;
            Description = reader.Description;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Id = Id;
            writer.Name = Name;
            writer.Description = Description;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Description
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
            public string Id => ctx.ReadText(0, null);
            public string Name => ctx.ReadText(1, null);
            public string Description => ctx.ReadText(2, null);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 3);
            }

            public string Id
            {
                get => this.ReadText(0, null);
                set => this.WriteText(0, value, null);
            }

            public string Name
            {
                get => this.ReadText(1, null);
                set => this.WriteText(1, value, null);
            }

            public string Description
            {
                get => this.ReadText(2, null);
                set => this.WriteText(2, value, null);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb2afd1cb599c48d5UL), Proxy(typeof(Identifiable_Proxy)), Skeleton(typeof(Identifiable_Skeleton))]
    public interface IIdentifiable : IDisposable
    {
        Task<Mas.Schema.Common.IdInformation> Info(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb2afd1cb599c48d5UL)]
    public class Identifiable_Proxy : Proxy, IIdentifiable
    {
        public async Task<Mas.Schema.Common.IdInformation> Info(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.Identifiable.Params_Info.WRITER>();
            var arg_ = new Mas.Schema.Common.Identifiable.Params_Info()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(12875740530987518165UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Common.IdInformation>(d_);
                return r_;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb2afd1cb599c48d5UL)]
    public class Identifiable_Skeleton : Skeleton<IIdentifiable>
    {
        public Identifiable_Skeleton()
        {
            SetMethodTable(Info);
        }

        public override ulong InterfaceId => 12875740530987518165UL;
        Task<AnswerOrCounterquestion> Info(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Info(cancellationToken_), r_ =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.IdInformation.WRITER>();
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class Identifiable
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9d8aa1cf1e49deb1UL)]
        public class Params_Info : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9d8aa1cf1e49deb1UL;
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
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xed6c098b67cad454UL)]
    public class StructuredText : ICapnpSerializable
    {
        public const UInt64 typeId = 0xed6c098b67cad454UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Value = reader.Value;
            Structure = CapnpSerializable.Create<Mas.Schema.Common.StructuredText.structure>(reader.Structure);
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Value = Value;
            Structure?.serialize(writer.Structure);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public string Value
        {
            get;
            set;
        }

        public Mas.Schema.Common.StructuredText.structure Structure
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
            public string Value => ctx.ReadText(0, null);
            public structure.READER Structure => new structure.READER(ctx);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 1);
            }

            public string Value
            {
                get => this.ReadText(0, null);
                set => this.WriteText(0, value, null);
            }

            public structure.WRITER Structure
            {
                get => Rewrap<structure.WRITER>();
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe8cbf552b1c262ccUL)]
        public class structure : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe8cbf552b1c262ccUL;
            public enum WHICH : ushort
            {
                None = 0,
                Json = 1,
                Xml = 2,
                undefined = 65535
            }

            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                switch (reader.which)
                {
                    case WHICH.None:
                        which = reader.which;
                        break;
                    case WHICH.Json:
                        which = reader.which;
                        break;
                    case WHICH.Xml:
                        which = reader.which;
                        break;
                }

                applyDefaults();
            }

            private WHICH _which = WHICH.undefined;
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
                        case WHICH.None:
                            break;
                        case WHICH.Json:
                            break;
                        case WHICH.Xml:
                            break;
                    }
                }
            }

            public void serialize(WRITER writer)
            {
                writer.which = which;
                switch (which)
                {
                    case WHICH.None:
                        break;
                    case WHICH.Json:
                        break;
                    case WHICH.Xml:
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
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                }

                public WHICH which
                {
                    get => (WHICH)this.ReadDataUShort(0U, (ushort)0);
                    set => this.WriteData(0U, (ushort)value, (ushort)0);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe17592335373b246UL)]
    public class Value : ICapnpSerializable
    {
        public const UInt64 typeId = 0xe17592335373b246UL;
        public enum WHICH : ushort
        {
            F64 = 0,
            F32 = 1,
            I64 = 2,
            I32 = 3,
            I16 = 4,
            I8 = 5,
            Ui64 = 6,
            Ui32 = 7,
            Ui16 = 8,
            Ui8 = 9,
            B = 10,
            T = 11,
            D = 12,
            P = 13,
            Cap = 14,
            Lf64 = 15,
            Lf32 = 16,
            Li64 = 17,
            Li32 = 18,
            Li16 = 19,
            Li8 = 20,
            Lui64 = 21,
            Lui32 = 22,
            Lui16 = 23,
            Lui8 = 24,
            Lb = 25,
            Lt = 26,
            Ld = 27,
            Lcap = 28,
            undefined = 65535
        }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            switch (reader.which)
            {
                case WHICH.F64:
                    F64 = reader.F64;
                    break;
                case WHICH.F32:
                    F32 = reader.F32;
                    break;
                case WHICH.I64:
                    I64 = reader.I64;
                    break;
                case WHICH.I32:
                    I32 = reader.I32;
                    break;
                case WHICH.I16:
                    I16 = reader.I16;
                    break;
                case WHICH.I8:
                    I8 = reader.I8;
                    break;
                case WHICH.Ui64:
                    Ui64 = reader.Ui64;
                    break;
                case WHICH.Ui32:
                    Ui32 = reader.Ui32;
                    break;
                case WHICH.Ui16:
                    Ui16 = reader.Ui16;
                    break;
                case WHICH.Ui8:
                    Ui8 = reader.Ui8;
                    break;
                case WHICH.B:
                    B = reader.B;
                    break;
                case WHICH.T:
                    T = reader.T;
                    break;
                case WHICH.D:
                    D = reader.D;
                    break;
                case WHICH.P:
                    P = CapnpSerializable.Create<object>(reader.P);
                    break;
                case WHICH.Cap:
                    Cap = reader.Cap;
                    break;
                case WHICH.Lf64:
                    Lf64 = reader.Lf64;
                    break;
                case WHICH.Lf32:
                    Lf32 = reader.Lf32;
                    break;
                case WHICH.Li64:
                    Li64 = reader.Li64;
                    break;
                case WHICH.Li32:
                    Li32 = reader.Li32;
                    break;
                case WHICH.Li16:
                    Li16 = reader.Li16;
                    break;
                case WHICH.Li8:
                    Li8 = reader.Li8;
                    break;
                case WHICH.Lui64:
                    Lui64 = reader.Lui64;
                    break;
                case WHICH.Lui32:
                    Lui32 = reader.Lui32;
                    break;
                case WHICH.Lui16:
                    Lui16 = reader.Lui16;
                    break;
                case WHICH.Lui8:
                    Lui8 = reader.Lui8;
                    break;
                case WHICH.Lb:
                    Lb = reader.Lb;
                    break;
                case WHICH.Lt:
                    Lt = reader.Lt;
                    break;
                case WHICH.Ld:
                    Ld = reader.Ld;
                    break;
                case WHICH.Lcap:
                    Lcap = reader.Lcap;
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
                    case WHICH.F64:
                        _content = 0;
                        break;
                    case WHICH.F32:
                        _content = 0F;
                        break;
                    case WHICH.I64:
                        _content = 0;
                        break;
                    case WHICH.I32:
                        _content = 0;
                        break;
                    case WHICH.I16:
                        _content = 0;
                        break;
                    case WHICH.I8:
                        _content = 0;
                        break;
                    case WHICH.Ui64:
                        _content = 0;
                        break;
                    case WHICH.Ui32:
                        _content = 0;
                        break;
                    case WHICH.Ui16:
                        _content = 0;
                        break;
                    case WHICH.Ui8:
                        _content = 0;
                        break;
                    case WHICH.B:
                        _content = false;
                        break;
                    case WHICH.T:
                        _content = null;
                        break;
                    case WHICH.D:
                        _content = null;
                        break;
                    case WHICH.P:
                        _content = null;
                        break;
                    case WHICH.Cap:
                        _content = null;
                        break;
                    case WHICH.Lf64:
                        _content = null;
                        break;
                    case WHICH.Lf32:
                        _content = null;
                        break;
                    case WHICH.Li64:
                        _content = null;
                        break;
                    case WHICH.Li32:
                        _content = null;
                        break;
                    case WHICH.Li16:
                        _content = null;
                        break;
                    case WHICH.Li8:
                        _content = null;
                        break;
                    case WHICH.Lui64:
                        _content = null;
                        break;
                    case WHICH.Lui32:
                        _content = null;
                        break;
                    case WHICH.Lui16:
                        _content = null;
                        break;
                    case WHICH.Lui8:
                        _content = null;
                        break;
                    case WHICH.Lb:
                        _content = null;
                        break;
                    case WHICH.Lt:
                        _content = null;
                        break;
                    case WHICH.Ld:
                        _content = null;
                        break;
                    case WHICH.Lcap:
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
                case WHICH.F64:
                    writer.F64 = F64.Value;
                    break;
                case WHICH.F32:
                    writer.F32 = F32.Value;
                    break;
                case WHICH.I64:
                    writer.I64 = I64.Value;
                    break;
                case WHICH.I32:
                    writer.I32 = I32.Value;
                    break;
                case WHICH.I16:
                    writer.I16 = I16.Value;
                    break;
                case WHICH.I8:
                    writer.I8 = I8.Value;
                    break;
                case WHICH.Ui64:
                    writer.Ui64 = Ui64.Value;
                    break;
                case WHICH.Ui32:
                    writer.Ui32 = Ui32.Value;
                    break;
                case WHICH.Ui16:
                    writer.Ui16 = Ui16.Value;
                    break;
                case WHICH.Ui8:
                    writer.Ui8 = Ui8.Value;
                    break;
                case WHICH.B:
                    writer.B = B.Value;
                    break;
                case WHICH.T:
                    writer.T = T;
                    break;
                case WHICH.D:
                    writer.D.Init(D);
                    break;
                case WHICH.P:
                    writer.P.SetObject(P);
                    break;
                case WHICH.Cap:
                    writer.Cap = Cap;
                    break;
                case WHICH.Lf64:
                    writer.Lf64.Init(Lf64);
                    break;
                case WHICH.Lf32:
                    writer.Lf32.Init(Lf32);
                    break;
                case WHICH.Li64:
                    writer.Li64.Init(Li64);
                    break;
                case WHICH.Li32:
                    writer.Li32.Init(Li32);
                    break;
                case WHICH.Li16:
                    writer.Li16.Init(Li16);
                    break;
                case WHICH.Li8:
                    writer.Li8.Init(Li8);
                    break;
                case WHICH.Lui64:
                    writer.Lui64.Init(Lui64);
                    break;
                case WHICH.Lui32:
                    writer.Lui32.Init(Lui32);
                    break;
                case WHICH.Lui16:
                    writer.Lui16.Init(Lui16);
                    break;
                case WHICH.Lui8:
                    writer.Lui8.Init(Lui8);
                    break;
                case WHICH.Lb:
                    writer.Lb.Init(Lb);
                    break;
                case WHICH.Lt:
                    writer.Lt.Init(Lt);
                    break;
                case WHICH.Ld:
                    writer.Ld.Init(Ld, (_s1, _v1) => _s1.Init(_v1));
                    break;
                case WHICH.Lcap:
                    writer.Lcap.Init(Lcap);
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

        public double? F64
        {
            get => _which == WHICH.F64 ? (double? )_content : null;
            set
            {
                _which = WHICH.F64;
                _content = value;
            }
        }

        public float? F32
        {
            get => _which == WHICH.F32 ? (float? )_content : null;
            set
            {
                _which = WHICH.F32;
                _content = value;
            }
        }

        public long? I64
        {
            get => _which == WHICH.I64 ? (long? )_content : null;
            set
            {
                _which = WHICH.I64;
                _content = value;
            }
        }

        public int? I32
        {
            get => _which == WHICH.I32 ? (int? )_content : null;
            set
            {
                _which = WHICH.I32;
                _content = value;
            }
        }

        public short? I16
        {
            get => _which == WHICH.I16 ? (short? )_content : null;
            set
            {
                _which = WHICH.I16;
                _content = value;
            }
        }

        public sbyte? I8
        {
            get => _which == WHICH.I8 ? (sbyte? )_content : null;
            set
            {
                _which = WHICH.I8;
                _content = value;
            }
        }

        public ulong? Ui64
        {
            get => _which == WHICH.Ui64 ? (ulong? )_content : null;
            set
            {
                _which = WHICH.Ui64;
                _content = value;
            }
        }

        public uint? Ui32
        {
            get => _which == WHICH.Ui32 ? (uint? )_content : null;
            set
            {
                _which = WHICH.Ui32;
                _content = value;
            }
        }

        public ushort? Ui16
        {
            get => _which == WHICH.Ui16 ? (ushort? )_content : null;
            set
            {
                _which = WHICH.Ui16;
                _content = value;
            }
        }

        public byte? Ui8
        {
            get => _which == WHICH.Ui8 ? (byte? )_content : null;
            set
            {
                _which = WHICH.Ui8;
                _content = value;
            }
        }

        public bool? B
        {
            get => _which == WHICH.B ? (bool? )_content : null;
            set
            {
                _which = WHICH.B;
                _content = value;
            }
        }

        public string T
        {
            get => _which == WHICH.T ? (string)_content : null;
            set
            {
                _which = WHICH.T;
                _content = value;
            }
        }

        public IReadOnlyList<byte> D
        {
            get => _which == WHICH.D ? (IReadOnlyList<byte>)_content : null;
            set
            {
                _which = WHICH.D;
                _content = value;
            }
        }

        public object P
        {
            get => _which == WHICH.P ? (object)_content : null;
            set
            {
                _which = WHICH.P;
                _content = value;
            }
        }

        public BareProxy Cap
        {
            get => _which == WHICH.Cap ? (BareProxy)_content : null;
            set
            {
                _which = WHICH.Cap;
                _content = value;
            }
        }

        public IReadOnlyList<double> Lf64
        {
            get => _which == WHICH.Lf64 ? (IReadOnlyList<double>)_content : null;
            set
            {
                _which = WHICH.Lf64;
                _content = value;
            }
        }

        public IReadOnlyList<float> Lf32
        {
            get => _which == WHICH.Lf32 ? (IReadOnlyList<float>)_content : null;
            set
            {
                _which = WHICH.Lf32;
                _content = value;
            }
        }

        public IReadOnlyList<long> Li64
        {
            get => _which == WHICH.Li64 ? (IReadOnlyList<long>)_content : null;
            set
            {
                _which = WHICH.Li64;
                _content = value;
            }
        }

        public IReadOnlyList<int> Li32
        {
            get => _which == WHICH.Li32 ? (IReadOnlyList<int>)_content : null;
            set
            {
                _which = WHICH.Li32;
                _content = value;
            }
        }

        public IReadOnlyList<short> Li16
        {
            get => _which == WHICH.Li16 ? (IReadOnlyList<short>)_content : null;
            set
            {
                _which = WHICH.Li16;
                _content = value;
            }
        }

        public IReadOnlyList<sbyte> Li8
        {
            get => _which == WHICH.Li8 ? (IReadOnlyList<sbyte>)_content : null;
            set
            {
                _which = WHICH.Li8;
                _content = value;
            }
        }

        public IReadOnlyList<ulong> Lui64
        {
            get => _which == WHICH.Lui64 ? (IReadOnlyList<ulong>)_content : null;
            set
            {
                _which = WHICH.Lui64;
                _content = value;
            }
        }

        public IReadOnlyList<uint> Lui32
        {
            get => _which == WHICH.Lui32 ? (IReadOnlyList<uint>)_content : null;
            set
            {
                _which = WHICH.Lui32;
                _content = value;
            }
        }

        public IReadOnlyList<ushort> Lui16
        {
            get => _which == WHICH.Lui16 ? (IReadOnlyList<ushort>)_content : null;
            set
            {
                _which = WHICH.Lui16;
                _content = value;
            }
        }

        public IReadOnlyList<byte> Lui8
        {
            get => _which == WHICH.Lui8 ? (IReadOnlyList<byte>)_content : null;
            set
            {
                _which = WHICH.Lui8;
                _content = value;
            }
        }

        public IReadOnlyList<bool> Lb
        {
            get => _which == WHICH.Lb ? (IReadOnlyList<bool>)_content : null;
            set
            {
                _which = WHICH.Lb;
                _content = value;
            }
        }

        public IReadOnlyList<string> Lt
        {
            get => _which == WHICH.Lt ? (IReadOnlyList<string>)_content : null;
            set
            {
                _which = WHICH.Lt;
                _content = value;
            }
        }

        public IReadOnlyList<IReadOnlyList<byte>> Ld
        {
            get => _which == WHICH.Ld ? (IReadOnlyList<IReadOnlyList<byte>>)_content : null;
            set
            {
                _which = WHICH.Ld;
                _content = value;
            }
        }

        public IReadOnlyList<BareProxy> Lcap
        {
            get => _which == WHICH.Lcap ? (IReadOnlyList<BareProxy>)_content : null;
            set
            {
                _which = WHICH.Lcap;
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
            public WHICH which => (WHICH)ctx.ReadDataUShort(64U, (ushort)0);
            public double F64 => which == WHICH.F64 ? ctx.ReadDataDouble(0UL, 0) : default;
            public float F32 => which == WHICH.F32 ? ctx.ReadDataFloat(0UL, 0F) : default;
            public long I64 => which == WHICH.I64 ? ctx.ReadDataLong(0UL, 0L) : default;
            public int I32 => which == WHICH.I32 ? ctx.ReadDataInt(0UL, 0) : default;
            public short I16 => which == WHICH.I16 ? ctx.ReadDataShort(0UL, (short)0) : default;
            public sbyte I8 => which == WHICH.I8 ? ctx.ReadDataSByte(0UL, (sbyte)0) : default;
            public ulong Ui64 => which == WHICH.Ui64 ? ctx.ReadDataULong(0UL, 0UL) : default;
            public uint Ui32 => which == WHICH.Ui32 ? ctx.ReadDataUInt(0UL, 0U) : default;
            public ushort Ui16 => which == WHICH.Ui16 ? ctx.ReadDataUShort(0UL, (ushort)0) : default;
            public byte Ui8 => which == WHICH.Ui8 ? ctx.ReadDataByte(0UL, (byte)0) : default;
            public bool B => which == WHICH.B ? ctx.ReadDataBool(0UL, false) : default;
            public string T => which == WHICH.T ? ctx.ReadText(0, null) : default;
            public IReadOnlyList<byte> D => which == WHICH.D ? ctx.ReadList(0).CastByte() : default;
            public DeserializerState P => which == WHICH.P ? ctx.StructReadPointer(0) : default;
            public BareProxy Cap => which == WHICH.Cap ? ctx.ReadCap(0) : default;
            public IReadOnlyList<double> Lf64 => which == WHICH.Lf64 ? ctx.ReadList(0).CastDouble() : default;
            public bool HasLf64 => ctx.IsStructFieldNonNull(0);
            public IReadOnlyList<float> Lf32 => which == WHICH.Lf32 ? ctx.ReadList(0).CastFloat() : default;
            public bool HasLf32 => ctx.IsStructFieldNonNull(0);
            public IReadOnlyList<long> Li64 => which == WHICH.Li64 ? ctx.ReadList(0).CastLong() : default;
            public bool HasLi64 => ctx.IsStructFieldNonNull(0);
            public IReadOnlyList<int> Li32 => which == WHICH.Li32 ? ctx.ReadList(0).CastInt() : default;
            public bool HasLi32 => ctx.IsStructFieldNonNull(0);
            public IReadOnlyList<short> Li16 => which == WHICH.Li16 ? ctx.ReadList(0).CastShort() : default;
            public bool HasLi16 => ctx.IsStructFieldNonNull(0);
            public IReadOnlyList<sbyte> Li8 => which == WHICH.Li8 ? ctx.ReadList(0).CastSByte() : default;
            public bool HasLi8 => ctx.IsStructFieldNonNull(0);
            public IReadOnlyList<ulong> Lui64 => which == WHICH.Lui64 ? ctx.ReadList(0).CastULong() : default;
            public bool HasLui64 => ctx.IsStructFieldNonNull(0);
            public IReadOnlyList<uint> Lui32 => which == WHICH.Lui32 ? ctx.ReadList(0).CastUInt() : default;
            public bool HasLui32 => ctx.IsStructFieldNonNull(0);
            public IReadOnlyList<ushort> Lui16 => which == WHICH.Lui16 ? ctx.ReadList(0).CastUShort() : default;
            public bool HasLui16 => ctx.IsStructFieldNonNull(0);
            public IReadOnlyList<byte> Lui8 => which == WHICH.Lui8 ? ctx.ReadList(0).CastByte() : default;
            public bool HasLui8 => ctx.IsStructFieldNonNull(0);
            public IReadOnlyList<bool> Lb => which == WHICH.Lb ? ctx.ReadList(0).CastBool() : default;
            public bool HasLb => ctx.IsStructFieldNonNull(0);
            public IReadOnlyList<string> Lt => which == WHICH.Lt ? ctx.ReadList(0).CastText2() : default;
            public bool HasLt => ctx.IsStructFieldNonNull(0);
            public IReadOnlyList<IReadOnlyList<byte>> Ld => which == WHICH.Ld ? ctx.ReadList(0).CastData() : default;
            public bool HasLd => ctx.IsStructFieldNonNull(0);
            public IReadOnlyList<BareProxy> Lcap => which == WHICH.Lcap ? ctx.ReadCapList<BareProxy>(0) : default;
            public bool HasLcap => ctx.IsStructFieldNonNull(0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(2, 1);
            }

            public WHICH which
            {
                get => (WHICH)this.ReadDataUShort(64U, (ushort)0);
                set => this.WriteData(64U, (ushort)value, (ushort)0);
            }

            public double F64
            {
                get => which == WHICH.F64 ? this.ReadDataDouble(0UL, 0) : default;
                set => this.WriteData(0UL, value, 0);
            }

            public float F32
            {
                get => which == WHICH.F32 ? this.ReadDataFloat(0UL, 0F) : default;
                set => this.WriteData(0UL, value, 0F);
            }

            public long I64
            {
                get => which == WHICH.I64 ? this.ReadDataLong(0UL, 0L) : default;
                set => this.WriteData(0UL, value, 0L);
            }

            public int I32
            {
                get => which == WHICH.I32 ? this.ReadDataInt(0UL, 0) : default;
                set => this.WriteData(0UL, value, 0);
            }

            public short I16
            {
                get => which == WHICH.I16 ? this.ReadDataShort(0UL, (short)0) : default;
                set => this.WriteData(0UL, value, (short)0);
            }

            public sbyte I8
            {
                get => which == WHICH.I8 ? this.ReadDataSByte(0UL, (sbyte)0) : default;
                set => this.WriteData(0UL, value, (sbyte)0);
            }

            public ulong Ui64
            {
                get => which == WHICH.Ui64 ? this.ReadDataULong(0UL, 0UL) : default;
                set => this.WriteData(0UL, value, 0UL);
            }

            public uint Ui32
            {
                get => which == WHICH.Ui32 ? this.ReadDataUInt(0UL, 0U) : default;
                set => this.WriteData(0UL, value, 0U);
            }

            public ushort Ui16
            {
                get => which == WHICH.Ui16 ? this.ReadDataUShort(0UL, (ushort)0) : default;
                set => this.WriteData(0UL, value, (ushort)0);
            }

            public byte Ui8
            {
                get => which == WHICH.Ui8 ? this.ReadDataByte(0UL, (byte)0) : default;
                set => this.WriteData(0UL, value, (byte)0);
            }

            public bool B
            {
                get => which == WHICH.B ? this.ReadDataBool(0UL, false) : default;
                set => this.WriteData(0UL, value, false);
            }

            public string T
            {
                get => which == WHICH.T ? this.ReadText(0, null) : default;
                set => this.WriteText(0, value, null);
            }

            public ListOfPrimitivesSerializer<byte> D
            {
                get => which == WHICH.D ? BuildPointer<ListOfPrimitivesSerializer<byte>>(0) : default;
                set => Link(0, value);
            }

            public DynamicSerializerState P
            {
                get => which == WHICH.P ? BuildPointer<DynamicSerializerState>(0) : default;
                set => Link(0, value);
            }

            public BareProxy Cap
            {
                get => which == WHICH.Cap ? ReadCap<BareProxy>(0) : default;
                set => LinkObject(0, value);
            }

            public ListOfPrimitivesSerializer<double> Lf64
            {
                get => which == WHICH.Lf64 ? BuildPointer<ListOfPrimitivesSerializer<double>>(0) : default;
                set => Link(0, value);
            }

            public ListOfPrimitivesSerializer<float> Lf32
            {
                get => which == WHICH.Lf32 ? BuildPointer<ListOfPrimitivesSerializer<float>>(0) : default;
                set => Link(0, value);
            }

            public ListOfPrimitivesSerializer<long> Li64
            {
                get => which == WHICH.Li64 ? BuildPointer<ListOfPrimitivesSerializer<long>>(0) : default;
                set => Link(0, value);
            }

            public ListOfPrimitivesSerializer<int> Li32
            {
                get => which == WHICH.Li32 ? BuildPointer<ListOfPrimitivesSerializer<int>>(0) : default;
                set => Link(0, value);
            }

            public ListOfPrimitivesSerializer<short> Li16
            {
                get => which == WHICH.Li16 ? BuildPointer<ListOfPrimitivesSerializer<short>>(0) : default;
                set => Link(0, value);
            }

            public ListOfPrimitivesSerializer<sbyte> Li8
            {
                get => which == WHICH.Li8 ? BuildPointer<ListOfPrimitivesSerializer<sbyte>>(0) : default;
                set => Link(0, value);
            }

            public ListOfPrimitivesSerializer<ulong> Lui64
            {
                get => which == WHICH.Lui64 ? BuildPointer<ListOfPrimitivesSerializer<ulong>>(0) : default;
                set => Link(0, value);
            }

            public ListOfPrimitivesSerializer<uint> Lui32
            {
                get => which == WHICH.Lui32 ? BuildPointer<ListOfPrimitivesSerializer<uint>>(0) : default;
                set => Link(0, value);
            }

            public ListOfPrimitivesSerializer<ushort> Lui16
            {
                get => which == WHICH.Lui16 ? BuildPointer<ListOfPrimitivesSerializer<ushort>>(0) : default;
                set => Link(0, value);
            }

            public ListOfPrimitivesSerializer<byte> Lui8
            {
                get => which == WHICH.Lui8 ? BuildPointer<ListOfPrimitivesSerializer<byte>>(0) : default;
                set => Link(0, value);
            }

            public ListOfBitsSerializer Lb
            {
                get => which == WHICH.Lb ? BuildPointer<ListOfBitsSerializer>(0) : default;
                set => Link(0, value);
            }

            public ListOfTextSerializer Lt
            {
                get => which == WHICH.Lt ? BuildPointer<ListOfTextSerializer>(0) : default;
                set => Link(0, value);
            }

            public ListOfPointersSerializer<ListOfPrimitivesSerializer<byte>> Ld
            {
                get => which == WHICH.Ld ? BuildPointer<ListOfPointersSerializer<ListOfPrimitivesSerializer<byte>>>(0) : default;
                set => Link(0, value);
            }

            public ListOfCapsSerializer<BareProxy> Lcap
            {
                get => which == WHICH.Lcap ? BuildPointer<ListOfCapsSerializer<BareProxy>>(0) : default;
                set => Link(0, value);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb9d4864725174733UL)]
    public class Pair<TF, TS> : ICapnpSerializable where TF : class where TS : class
    {
        public const UInt64 typeId = 0xb9d4864725174733UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Fst = CapnpSerializable.Create<TF>(reader.Fst);
            Snd = CapnpSerializable.Create<TS>(reader.Snd);
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Fst.SetObject(Fst);
            writer.Snd.SetObject(Snd);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public TF Fst
        {
            get;
            set;
        }

        public TS Snd
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
            public DeserializerState Fst => ctx.StructReadPointer(0);
            public DeserializerState Snd => ctx.StructReadPointer(1);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 2);
            }

            public DynamicSerializerState Fst
            {
                get => BuildPointer<DynamicSerializerState>(0);
                set => Link(0, value);
            }

            public DynamicSerializerState Snd
            {
                get => BuildPointer<DynamicSerializerState>(1);
                set => Link(1, value);
            }
        }
    }
}