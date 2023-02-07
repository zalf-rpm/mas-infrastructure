using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Schema.Climate
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xce396869eede9f10UL)]
    public enum GCM : ushort
    {
        cccmaCanEsm2,
        ichecEcEarth,
        ipslIpslCm5AMr,
        mirocMiroc5,
        mpiMMpiEsmLr,
        gfdlEsm4,
        ipslCm6aLr,
        mpiEsm12Hr,
        mriEsm20,
        ukesm10Ll,
        gswp3W5E5,
        mohcHadGem2Es
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8671dec53083e351UL)]
    public enum RCM : ushort
    {
        clmcomCclm4817,
        gericsRemo2015,
        knmiRacmo22E,
        smhiRca4,
        clmcomBtuCclm4817,
        mpiCscRemo2009,
        uhohWrf361H
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd3780ae416347aeeUL)]
    public enum SSP : ushort
    {
        ssp1,
        ssp2,
        ssp3,
        ssp4,
        ssp5
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8ef30778310c94ccUL)]
    public enum RCP : ushort
    {
        rcp19,
        rcp26,
        rcp34,
        rcp45,
        rcp60,
        rcp70,
        rcp85
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc8caacd1cd5da434UL)]
    public class EnsembleMember : ICapnpSerializable
    {
        public const UInt64 typeId = 0xc8caacd1cd5da434UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            R = reader.R;
            I = reader.I;
            P = reader.P;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.R = R;
            writer.I = I;
            writer.P = P;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public ushort R
        {
            get;
            set;
        }

        public ushort I
        {
            get;
            set;
        }

        public ushort P
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
            public ushort R => ctx.ReadDataUShort(0UL, (ushort)0);
            public ushort I => ctx.ReadDataUShort(16UL, (ushort)0);
            public ushort P => ctx.ReadDataUShort(32UL, (ushort)0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 0);
            }

            public ushort R
            {
                get => this.ReadDataUShort(0UL, (ushort)0);
                set => this.WriteData(0UL, value, (ushort)0);
            }

            public ushort I
            {
                get => this.ReadDataUShort(16UL, (ushort)0);
                set => this.WriteData(16UL, value, (ushort)0);
            }

            public ushort P
            {
                get => this.ReadDataUShort(32UL, (ushort)0);
                set => this.WriteData(32UL, value, (ushort)0);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfb36d2e966556db0UL)]
    public class Metadata : ICapnpSerializable
    {
        public const UInt64 typeId = 0xfb36d2e966556db0UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Entries = reader.Entries?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Climate.Metadata.Entry>(_));
            Info = reader.Info;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Entries.Init(Entries, (_s1, _v1) => _v1?.serialize(_s1));
            writer.Info = Info;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public IReadOnlyList<Mas.Schema.Climate.Metadata.Entry> Entries
        {
            get;
            set;
        }

        public Mas.Schema.Climate.Metadata.IInformation Info
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
            public IReadOnlyList<Mas.Schema.Climate.Metadata.Entry.READER> Entries => ctx.ReadList(0).Cast(Mas.Schema.Climate.Metadata.Entry.READER.create);
            public bool HasEntries => ctx.IsStructFieldNonNull(0);
            public Mas.Schema.Climate.Metadata.IInformation Info => ctx.ReadCap<Mas.Schema.Climate.Metadata.IInformation>(1);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 2);
            }

            public ListOfStructsSerializer<Mas.Schema.Climate.Metadata.Entry.WRITER> Entries
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Climate.Metadata.Entry.WRITER>>(0);
                set => Link(0, value);
            }

            public Mas.Schema.Climate.Metadata.IInformation Info
            {
                get => ReadCap<Mas.Schema.Climate.Metadata.IInformation>(1);
                set => LinkObject(1, value);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xab06444b30722e01UL), Proxy(typeof(Supported_Proxy)), Skeleton(typeof(Supported_Skeleton))]
        public interface ISupported : IDisposable
        {
            Task<IReadOnlyList<Mas.Schema.Common.IdInformation>> Categories(CancellationToken cancellationToken_ = default);
            Task<IReadOnlyList<Mas.Schema.Common.IdInformation>> SupportedValues(string typeId, CancellationToken cancellationToken_ = default);
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xab06444b30722e01UL)]
        public class Supported_Proxy : Proxy, ISupported
        {
            public async Task<IReadOnlyList<Mas.Schema.Common.IdInformation>> Categories(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Metadata.Supported.Params_Categories.WRITER>();
                var arg_ = new Mas.Schema.Climate.Metadata.Supported.Params_Categories()
                {};
                arg_?.serialize(in_);
                using (var d_ = await Call(12323612520071966209UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.Metadata.Supported.Result_Categories>(d_);
                    return (r_.Types);
                }
            }

            public async Task<IReadOnlyList<Mas.Schema.Common.IdInformation>> SupportedValues(string typeId, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Metadata.Supported.Params_SupportedValues.WRITER>();
                var arg_ = new Mas.Schema.Climate.Metadata.Supported.Params_SupportedValues()
                {TypeId = typeId};
                arg_?.serialize(in_);
                using (var d_ = await Call(12323612520071966209UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.Metadata.Supported.Result_SupportedValues>(d_);
                    return (r_.Values);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xab06444b30722e01UL)]
        public class Supported_Skeleton : Skeleton<ISupported>
        {
            public Supported_Skeleton()
            {
                SetMethodTable(Categories, SupportedValues);
            }

            public override ulong InterfaceId => 12323612520071966209UL;
            Task<AnswerOrCounterquestion> Categories(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    return Impatient.MaybeTailCall(Impl.Categories(cancellationToken_), types =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Metadata.Supported.Result_Categories.WRITER>();
                        var r_ = new Mas.Schema.Climate.Metadata.Supported.Result_Categories{Types = types};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> SupportedValues(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Climate.Metadata.Supported.Params_SupportedValues>(d_);
                    return Impatient.MaybeTailCall(Impl.SupportedValues(in_.TypeId, cancellationToken_), values =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Metadata.Supported.Result_SupportedValues.WRITER>();
                        var r_ = new Mas.Schema.Climate.Metadata.Supported.Result_SupportedValues{Values = values};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }
        }

        public static class Supported
        {
            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x95887677293b5682UL)]
            public class Params_Categories : ICapnpSerializable
            {
                public const UInt64 typeId = 0x95887677293b5682UL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe49e838ea9c34b40UL)]
            public class Result_Categories : ICapnpSerializable
            {
                public const UInt64 typeId = 0xe49e838ea9c34b40UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Types = reader.Types?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Common.IdInformation>(_));
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Types.Init(Types, (_s1, _v1) => _v1?.serialize(_s1));
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<Mas.Schema.Common.IdInformation> Types
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
                    public IReadOnlyList<Mas.Schema.Common.IdInformation.READER> Types => ctx.ReadList(0).Cast(Mas.Schema.Common.IdInformation.READER.create);
                    public bool HasTypes => ctx.IsStructFieldNonNull(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfStructsSerializer<Mas.Schema.Common.IdInformation.WRITER> Types
                    {
                        get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Common.IdInformation.WRITER>>(0);
                        set => Link(0, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc6d2329c05f7e208UL)]
            public class Params_SupportedValues : ICapnpSerializable
            {
                public const UInt64 typeId = 0xc6d2329c05f7e208UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    TypeId = reader.TypeId;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.TypeId = TypeId;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public string TypeId
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
                    public string TypeId => ctx.ReadText(0, null);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public string TypeId
                    {
                        get => this.ReadText(0, null);
                        set => this.WriteText(0, value, null);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe0a71ff36670f715UL)]
            public class Result_SupportedValues : ICapnpSerializable
            {
                public const UInt64 typeId = 0xe0a71ff36670f715UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Values = reader.Values?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Common.IdInformation>(_));
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Values.Init(Values, (_s1, _v1) => _v1?.serialize(_s1));
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<Mas.Schema.Common.IdInformation> Values
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
                    public IReadOnlyList<Mas.Schema.Common.IdInformation.READER> Values => ctx.ReadList(0).Cast(Mas.Schema.Common.IdInformation.READER.create);
                    public bool HasValues => ctx.IsStructFieldNonNull(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfStructsSerializer<Mas.Schema.Common.IdInformation.WRITER> Values
                    {
                        get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Common.IdInformation.WRITER>>(0);
                        set => Link(0, value);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc48e24c968a234dbUL)]
        public class Value : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc48e24c968a234dbUL;
            public enum WHICH : ushort
            {
                Text = 0,
                Float = 1,
                Int = 2,
                Bool = 3,
                Date = 4,
                undefined = 65535
            }

            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                switch (reader.which)
                {
                    case WHICH.Text:
                        Text = reader.Text;
                        break;
                    case WHICH.Float:
                        Float = reader.Float;
                        break;
                    case WHICH.Int:
                        Int = reader.Int;
                        break;
                    case WHICH.Bool:
                        Bool = reader.Bool;
                        break;
                    case WHICH.Date:
                        Date = CapnpSerializable.Create<Mas.Schema.Common.Date>(reader.Date);
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
                        case WHICH.Text:
                            _content = null;
                            break;
                        case WHICH.Float:
                            _content = 0;
                            break;
                        case WHICH.Int:
                            _content = 0;
                            break;
                        case WHICH.Bool:
                            _content = false;
                            break;
                        case WHICH.Date:
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
                    case WHICH.Text:
                        writer.Text = Text;
                        break;
                    case WHICH.Float:
                        writer.Float = Float.Value;
                        break;
                    case WHICH.Int:
                        writer.Int = Int.Value;
                        break;
                    case WHICH.Bool:
                        writer.Bool = Bool.Value;
                        break;
                    case WHICH.Date:
                        Date?.serialize(writer.Date);
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

            public string Text
            {
                get => _which == WHICH.Text ? (string)_content : null;
                set
                {
                    _which = WHICH.Text;
                    _content = value;
                }
            }

            public double? Float
            {
                get => _which == WHICH.Float ? (double? )_content : null;
                set
                {
                    _which = WHICH.Float;
                    _content = value;
                }
            }

            public long? Int
            {
                get => _which == WHICH.Int ? (long? )_content : null;
                set
                {
                    _which = WHICH.Int;
                    _content = value;
                }
            }

            public bool? Bool
            {
                get => _which == WHICH.Bool ? (bool? )_content : null;
                set
                {
                    _which = WHICH.Bool;
                    _content = value;
                }
            }

            public Mas.Schema.Common.Date Date
            {
                get => _which == WHICH.Date ? (Mas.Schema.Common.Date)_content : null;
                set
                {
                    _which = WHICH.Date;
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
                public string Text => which == WHICH.Text ? ctx.ReadText(0, null) : default;
                public double Float => which == WHICH.Float ? ctx.ReadDataDouble(64UL, 0) : default;
                public long Int => which == WHICH.Int ? ctx.ReadDataLong(64UL, 0L) : default;
                public bool Bool => which == WHICH.Bool ? ctx.ReadDataBool(64UL, false) : default;
                public Mas.Schema.Common.Date.READER Date => which == WHICH.Date ? ctx.ReadStruct(0, Mas.Schema.Common.Date.READER.create) : default;
                public bool HasDate => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(2, 1);
                }

                public WHICH which
                {
                    get => (WHICH)this.ReadDataUShort(0U, (ushort)0);
                    set => this.WriteData(0U, (ushort)value, (ushort)0);
                }

                public string Text
                {
                    get => which == WHICH.Text ? this.ReadText(0, null) : default;
                    set => this.WriteText(0, value, null);
                }

                public double Float
                {
                    get => which == WHICH.Float ? this.ReadDataDouble(64UL, 0) : default;
                    set => this.WriteData(64UL, value, 0);
                }

                public long Int
                {
                    get => which == WHICH.Int ? this.ReadDataLong(64UL, 0L) : default;
                    set => this.WriteData(64UL, value, 0L);
                }

                public bool Bool
                {
                    get => which == WHICH.Bool ? this.ReadDataBool(64UL, false) : default;
                    set => this.WriteData(64UL, value, false);
                }

                public Mas.Schema.Common.Date.WRITER Date
                {
                    get => which == WHICH.Date ? BuildPointer<Mas.Schema.Common.Date.WRITER>(0) : default;
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x85af7fea06d0820cUL)]
        public class Entry : ICapnpSerializable
        {
            public const UInt64 typeId = 0x85af7fea06d0820cUL;
            public enum WHICH : ushort
            {
                Gcm = 0,
                Rcm = 1,
                Historical = 2,
                Rcp = 3,
                Ssp = 4,
                EnsMem = 5,
                Version = 6,
                Start = 7,
                End = 8,
                Co2 = 9,
                Picontrol = 10,
                Description = 11,
                undefined = 65535
            }

            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                switch (reader.which)
                {
                    case WHICH.Gcm:
                        Gcm = reader.Gcm;
                        break;
                    case WHICH.Rcm:
                        Rcm = reader.Rcm;
                        break;
                    case WHICH.Historical:
                        which = reader.which;
                        break;
                    case WHICH.Rcp:
                        Rcp = reader.Rcp;
                        break;
                    case WHICH.Ssp:
                        Ssp = reader.Ssp;
                        break;
                    case WHICH.EnsMem:
                        EnsMem = CapnpSerializable.Create<Mas.Schema.Climate.EnsembleMember>(reader.EnsMem);
                        break;
                    case WHICH.Version:
                        Version = reader.Version;
                        break;
                    case WHICH.Start:
                        Start = CapnpSerializable.Create<Mas.Schema.Common.Date>(reader.Start);
                        break;
                    case WHICH.End:
                        End = CapnpSerializable.Create<Mas.Schema.Common.Date>(reader.End);
                        break;
                    case WHICH.Co2:
                        Co2 = reader.Co2;
                        break;
                    case WHICH.Picontrol:
                        which = reader.which;
                        break;
                    case WHICH.Description:
                        Description = reader.Description;
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
                        case WHICH.Gcm:
                            _content = (Mas.Schema.Climate.GCM)0;
                            break;
                        case WHICH.Rcm:
                            _content = (Mas.Schema.Climate.RCM)0;
                            break;
                        case WHICH.Historical:
                            break;
                        case WHICH.Rcp:
                            _content = (Mas.Schema.Climate.RCP)0;
                            break;
                        case WHICH.Ssp:
                            _content = (Mas.Schema.Climate.SSP)0;
                            break;
                        case WHICH.EnsMem:
                            _content = null;
                            break;
                        case WHICH.Version:
                            _content = null;
                            break;
                        case WHICH.Start:
                            _content = null;
                            break;
                        case WHICH.End:
                            _content = null;
                            break;
                        case WHICH.Co2:
                            _content = 0F;
                            break;
                        case WHICH.Picontrol:
                            break;
                        case WHICH.Description:
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
                    case WHICH.Gcm:
                        writer.Gcm = Gcm.Value;
                        break;
                    case WHICH.Rcm:
                        writer.Rcm = Rcm.Value;
                        break;
                    case WHICH.Historical:
                        break;
                    case WHICH.Rcp:
                        writer.Rcp = Rcp.Value;
                        break;
                    case WHICH.Ssp:
                        writer.Ssp = Ssp.Value;
                        break;
                    case WHICH.EnsMem:
                        EnsMem?.serialize(writer.EnsMem);
                        break;
                    case WHICH.Version:
                        writer.Version = Version;
                        break;
                    case WHICH.Start:
                        Start?.serialize(writer.Start);
                        break;
                    case WHICH.End:
                        End?.serialize(writer.End);
                        break;
                    case WHICH.Co2:
                        writer.Co2 = Co2.Value;
                        break;
                    case WHICH.Picontrol:
                        break;
                    case WHICH.Description:
                        writer.Description = Description;
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

            public Mas.Schema.Climate.GCM? Gcm
            {
                get => _which == WHICH.Gcm ? (Mas.Schema.Climate.GCM? )_content : null;
                set
                {
                    _which = WHICH.Gcm;
                    _content = value;
                }
            }

            public Mas.Schema.Climate.RCM? Rcm
            {
                get => _which == WHICH.Rcm ? (Mas.Schema.Climate.RCM? )_content : null;
                set
                {
                    _which = WHICH.Rcm;
                    _content = value;
                }
            }

            public Mas.Schema.Climate.RCP? Rcp
            {
                get => _which == WHICH.Rcp ? (Mas.Schema.Climate.RCP? )_content : null;
                set
                {
                    _which = WHICH.Rcp;
                    _content = value;
                }
            }

            public Mas.Schema.Climate.SSP? Ssp
            {
                get => _which == WHICH.Ssp ? (Mas.Schema.Climate.SSP? )_content : null;
                set
                {
                    _which = WHICH.Ssp;
                    _content = value;
                }
            }

            public Mas.Schema.Climate.EnsembleMember EnsMem
            {
                get => _which == WHICH.EnsMem ? (Mas.Schema.Climate.EnsembleMember)_content : null;
                set
                {
                    _which = WHICH.EnsMem;
                    _content = value;
                }
            }

            public string Version
            {
                get => _which == WHICH.Version ? (string)_content : null;
                set
                {
                    _which = WHICH.Version;
                    _content = value;
                }
            }

            public Mas.Schema.Common.Date Start
            {
                get => _which == WHICH.Start ? (Mas.Schema.Common.Date)_content : null;
                set
                {
                    _which = WHICH.Start;
                    _content = value;
                }
            }

            public Mas.Schema.Common.Date End
            {
                get => _which == WHICH.End ? (Mas.Schema.Common.Date)_content : null;
                set
                {
                    _which = WHICH.End;
                    _content = value;
                }
            }

            public float? Co2
            {
                get => _which == WHICH.Co2 ? (float? )_content : null;
                set
                {
                    _which = WHICH.Co2;
                    _content = value;
                }
            }

            public string Description
            {
                get => _which == WHICH.Description ? (string)_content : null;
                set
                {
                    _which = WHICH.Description;
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
                public WHICH which => (WHICH)ctx.ReadDataUShort(16U, (ushort)0);
                public Mas.Schema.Climate.GCM Gcm => which == WHICH.Gcm ? (Mas.Schema.Climate.GCM)ctx.ReadDataUShort(0UL, (ushort)0) : default;
                public Mas.Schema.Climate.RCM Rcm => which == WHICH.Rcm ? (Mas.Schema.Climate.RCM)ctx.ReadDataUShort(0UL, (ushort)0) : default;
                public Mas.Schema.Climate.RCP Rcp => which == WHICH.Rcp ? (Mas.Schema.Climate.RCP)ctx.ReadDataUShort(0UL, (ushort)0) : default;
                public Mas.Schema.Climate.SSP Ssp => which == WHICH.Ssp ? (Mas.Schema.Climate.SSP)ctx.ReadDataUShort(0UL, (ushort)0) : default;
                public Mas.Schema.Climate.EnsembleMember.READER EnsMem => which == WHICH.EnsMem ? ctx.ReadStruct(0, Mas.Schema.Climate.EnsembleMember.READER.create) : default;
                public bool HasEnsMem => ctx.IsStructFieldNonNull(0);
                public string Version => which == WHICH.Version ? ctx.ReadText(0, null) : default;
                public Mas.Schema.Common.Date.READER Start => which == WHICH.Start ? ctx.ReadStruct(0, Mas.Schema.Common.Date.READER.create) : default;
                public bool HasStart => ctx.IsStructFieldNonNull(0);
                public Mas.Schema.Common.Date.READER End => which == WHICH.End ? ctx.ReadStruct(0, Mas.Schema.Common.Date.READER.create) : default;
                public bool HasEnd => ctx.IsStructFieldNonNull(0);
                public float Co2 => which == WHICH.Co2 ? ctx.ReadDataFloat(32UL, 0F) : default;
                public string Description => which == WHICH.Description ? ctx.ReadText(0, null) : default;
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public WHICH which
                {
                    get => (WHICH)this.ReadDataUShort(16U, (ushort)0);
                    set => this.WriteData(16U, (ushort)value, (ushort)0);
                }

                public Mas.Schema.Climate.GCM Gcm
                {
                    get => which == WHICH.Gcm ? (Mas.Schema.Climate.GCM)this.ReadDataUShort(0UL, (ushort)0) : default;
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }

                public Mas.Schema.Climate.RCM Rcm
                {
                    get => which == WHICH.Rcm ? (Mas.Schema.Climate.RCM)this.ReadDataUShort(0UL, (ushort)0) : default;
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }

                public Mas.Schema.Climate.RCP Rcp
                {
                    get => which == WHICH.Rcp ? (Mas.Schema.Climate.RCP)this.ReadDataUShort(0UL, (ushort)0) : default;
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }

                public Mas.Schema.Climate.SSP Ssp
                {
                    get => which == WHICH.Ssp ? (Mas.Schema.Climate.SSP)this.ReadDataUShort(0UL, (ushort)0) : default;
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }

                public Mas.Schema.Climate.EnsembleMember.WRITER EnsMem
                {
                    get => which == WHICH.EnsMem ? BuildPointer<Mas.Schema.Climate.EnsembleMember.WRITER>(0) : default;
                    set => Link(0, value);
                }

                public string Version
                {
                    get => which == WHICH.Version ? this.ReadText(0, null) : default;
                    set => this.WriteText(0, value, null);
                }

                public Mas.Schema.Common.Date.WRITER Start
                {
                    get => which == WHICH.Start ? BuildPointer<Mas.Schema.Common.Date.WRITER>(0) : default;
                    set => Link(0, value);
                }

                public Mas.Schema.Common.Date.WRITER End
                {
                    get => which == WHICH.End ? BuildPointer<Mas.Schema.Common.Date.WRITER>(0) : default;
                    set => Link(0, value);
                }

                public float Co2
                {
                    get => which == WHICH.Co2 ? this.ReadDataFloat(32UL, 0F) : default;
                    set => this.WriteData(32UL, value, 0F);
                }

                public string Description
                {
                    get => which == WHICH.Description ? this.ReadText(0, null) : default;
                    set => this.WriteText(0, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc781edeab8160cb7UL), Proxy(typeof(Information_Proxy)), Skeleton(typeof(Information_Skeleton))]
        public interface IInformation : IDisposable
        {
            Task<Mas.Schema.Common.IdInformation> ForOne(Mas.Schema.Climate.Metadata.Entry entry, CancellationToken cancellationToken_ = default);
            Task<IReadOnlyList<Mas.Schema.Common.Pair<Mas.Schema.Climate.Metadata.Entry, Mas.Schema.Common.IdInformation>>> ForAll(CancellationToken cancellationToken_ = default);
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc781edeab8160cb7UL)]
        public class Information_Proxy : Proxy, IInformation
        {
            public async Task<Mas.Schema.Common.IdInformation> ForOne(Mas.Schema.Climate.Metadata.Entry entry, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Metadata.Information.Params_ForOne.WRITER>();
                var arg_ = new Mas.Schema.Climate.Metadata.Information.Params_ForOne()
                {Entry = entry};
                arg_?.serialize(in_);
                using (var d_ = await Call(14376033077909916855UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Common.IdInformation>(d_);
                    return r_;
                }
            }

            public async Task<IReadOnlyList<Mas.Schema.Common.Pair<Mas.Schema.Climate.Metadata.Entry, Mas.Schema.Common.IdInformation>>> ForAll(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Metadata.Information.Params_ForAll.WRITER>();
                var arg_ = new Mas.Schema.Climate.Metadata.Information.Params_ForAll()
                {};
                arg_?.serialize(in_);
                using (var d_ = await Call(14376033077909916855UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.Metadata.Information.Result_ForAll>(d_);
                    return (r_.All);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc781edeab8160cb7UL)]
        public class Information_Skeleton : Skeleton<IInformation>
        {
            public Information_Skeleton()
            {
                SetMethodTable(ForOne, ForAll);
            }

            public override ulong InterfaceId => 14376033077909916855UL;
            Task<AnswerOrCounterquestion> ForOne(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Climate.Metadata.Information.Params_ForOne>(d_);
                    return Impatient.MaybeTailCall(Impl.ForOne(in_.Entry, cancellationToken_), r_ =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.IdInformation.WRITER>();
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> ForAll(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    return Impatient.MaybeTailCall(Impl.ForAll(cancellationToken_), all =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Metadata.Information.Result_ForAll.WRITER>();
                        var r_ = new Mas.Schema.Climate.Metadata.Information.Result_ForAll{All = all};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }
        }

        public static class Information
        {
            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdf705ef1e0b7d506UL)]
            public class Params_ForOne : ICapnpSerializable
            {
                public const UInt64 typeId = 0xdf705ef1e0b7d506UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Entry = CapnpSerializable.Create<Mas.Schema.Climate.Metadata.Entry>(reader.Entry);
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    Entry?.serialize(writer.Entry);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Schema.Climate.Metadata.Entry Entry
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
                    public Mas.Schema.Climate.Metadata.Entry.READER Entry => ctx.ReadStruct(0, Mas.Schema.Climate.Metadata.Entry.READER.create);
                    public bool HasEntry => ctx.IsStructFieldNonNull(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Schema.Climate.Metadata.Entry.WRITER Entry
                    {
                        get => BuildPointer<Mas.Schema.Climate.Metadata.Entry.WRITER>(0);
                        set => Link(0, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe246d49c91fa330aUL)]
            public class Params_ForAll : ICapnpSerializable
            {
                public const UInt64 typeId = 0xe246d49c91fa330aUL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9f35030ba55fed78UL)]
            public class Result_ForAll : ICapnpSerializable
            {
                public const UInt64 typeId = 0x9f35030ba55fed78UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    All = reader.All?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Common.Pair<Mas.Schema.Climate.Metadata.Entry, Mas.Schema.Common.IdInformation>>(_));
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.All.Init(All, (_s1, _v1) => _v1?.serialize(_s1));
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<Mas.Schema.Common.Pair<Mas.Schema.Climate.Metadata.Entry, Mas.Schema.Common.IdInformation>> All
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
                    public IReadOnlyList<Mas.Schema.Common.Pair<Mas.Schema.Climate.Metadata.Entry, Mas.Schema.Common.IdInformation>.READER> All => ctx.ReadList(0).Cast(Mas.Schema.Common.Pair<Mas.Schema.Climate.Metadata.Entry, Mas.Schema.Common.IdInformation>.READER.create);
                    public bool HasAll => ctx.IsStructFieldNonNull(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfStructsSerializer<Mas.Schema.Common.Pair<Mas.Schema.Climate.Metadata.Entry, Mas.Schema.Common.IdInformation>.WRITER> All
                    {
                        get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Common.Pair<Mas.Schema.Climate.Metadata.Entry, Mas.Schema.Common.IdInformation>.WRITER>>(0);
                        set => Link(0, value);
                    }
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf635fdd1f05960f0UL), Proxy(typeof(Dataset_Proxy)), Skeleton(typeof(Dataset_Skeleton))]
    public interface IDataset : Mas.Schema.Common.IIdentifiable, Mas.Schema.Persistence.IPersistent
    {
        Task<Mas.Schema.Climate.Metadata> Metadata(CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Climate.ITimeSeries> ClosestTimeSeriesAt(Mas.Schema.Geo.LatLonCoord latlon, CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Climate.ITimeSeries> TimeSeriesAt(string locationId, CancellationToken cancellationToken_ = default);
        Task<IReadOnlyList<Mas.Schema.Climate.Location>> Locations(CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Climate.Dataset.IGetLocationsCallback> StreamLocations(string startAfterLocationId, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf635fdd1f05960f0UL)]
    public class Dataset_Proxy : Proxy, IDataset
    {
        public Task<Mas.Schema.Climate.Metadata> Metadata(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Dataset.Params_Metadata.WRITER>();
            var arg_ = new Mas.Schema.Climate.Dataset.Params_Metadata()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(17741365385218318576UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.Metadata>(d_);
                    return r_;
                }
            }

            );
        }

        public Task<Mas.Schema.Climate.ITimeSeries> ClosestTimeSeriesAt(Mas.Schema.Geo.LatLonCoord latlon, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Dataset.Params_ClosestTimeSeriesAt.WRITER>();
            var arg_ = new Mas.Schema.Climate.Dataset.Params_ClosestTimeSeriesAt()
            {Latlon = latlon};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(17741365385218318576UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.Dataset.Result_ClosestTimeSeriesAt>(d_);
                    return (r_.TimeSeries);
                }
            }

            );
        }

        public Task<Mas.Schema.Climate.ITimeSeries> TimeSeriesAt(string locationId, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Dataset.Params_TimeSeriesAt.WRITER>();
            var arg_ = new Mas.Schema.Climate.Dataset.Params_TimeSeriesAt()
            {LocationId = locationId};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(17741365385218318576UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.Dataset.Result_TimeSeriesAt>(d_);
                    return (r_.TimeSeries);
                }
            }

            );
        }

        public Task<IReadOnlyList<Mas.Schema.Climate.Location>> Locations(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Dataset.Params_Locations.WRITER>();
            var arg_ = new Mas.Schema.Climate.Dataset.Params_Locations()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(17741365385218318576UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.Dataset.Result_Locations>(d_);
                    return (r_.Locations);
                }
            }

            );
        }

        public Task<Mas.Schema.Climate.Dataset.IGetLocationsCallback> StreamLocations(string startAfterLocationId, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Dataset.Params_StreamLocations.WRITER>();
            var arg_ = new Mas.Schema.Climate.Dataset.Params_StreamLocations()
            {StartAfterLocationId = startAfterLocationId};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(17741365385218318576UL, 4, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.Dataset.Result_StreamLocations>(d_);
                    return (r_.LocationsCallback);
                }
            }

            );
        }

        public async Task<Mas.Schema.Persistence.Persistent.SaveResults> Save(Mas.Schema.Persistence.Persistent.SaveParams arg_, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.Persistent.SaveParams.WRITER>();
            arg_?.serialize(in_);
            using (var d_ = await Call(13954362354854972261UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Persistence.Persistent.SaveResults>(d_);
                return r_;
            }
        }

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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf635fdd1f05960f0UL)]
    public class Dataset_Skeleton : Skeleton<IDataset>
    {
        public Dataset_Skeleton()
        {
            SetMethodTable(Metadata, ClosestTimeSeriesAt, TimeSeriesAt, Locations, StreamLocations);
        }

        public override ulong InterfaceId => 17741365385218318576UL;
        Task<AnswerOrCounterquestion> Metadata(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Metadata(cancellationToken_), r_ =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Metadata.WRITER>();
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> ClosestTimeSeriesAt(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Climate.Dataset.Params_ClosestTimeSeriesAt>(d_);
                return Impatient.MaybeTailCall(Impl.ClosestTimeSeriesAt(in_.Latlon, cancellationToken_), timeSeries =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Dataset.Result_ClosestTimeSeriesAt.WRITER>();
                    var r_ = new Mas.Schema.Climate.Dataset.Result_ClosestTimeSeriesAt{TimeSeries = timeSeries};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> TimeSeriesAt(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Climate.Dataset.Params_TimeSeriesAt>(d_);
                return Impatient.MaybeTailCall(Impl.TimeSeriesAt(in_.LocationId, cancellationToken_), timeSeries =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Dataset.Result_TimeSeriesAt.WRITER>();
                    var r_ = new Mas.Schema.Climate.Dataset.Result_TimeSeriesAt{TimeSeries = timeSeries};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> Locations(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Locations(cancellationToken_), locations =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Dataset.Result_Locations.WRITER>();
                    var r_ = new Mas.Schema.Climate.Dataset.Result_Locations{Locations = locations};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> StreamLocations(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Climate.Dataset.Params_StreamLocations>(d_);
                return Impatient.MaybeTailCall(Impl.StreamLocations(in_.StartAfterLocationId, cancellationToken_), locationsCallback =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Dataset.Result_StreamLocations.WRITER>();
                    var r_ = new Mas.Schema.Climate.Dataset.Result_StreamLocations{LocationsCallback = locationsCallback};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class Dataset
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd61ba043f14fe175UL), Proxy(typeof(GetLocationsCallback_Proxy)), Skeleton(typeof(GetLocationsCallback_Skeleton))]
        public interface IGetLocationsCallback : IDisposable
        {
            Task<IReadOnlyList<Mas.Schema.Climate.Location>> NextLocations(long maxCount, CancellationToken cancellationToken_ = default);
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd61ba043f14fe175UL)]
        public class GetLocationsCallback_Proxy : Proxy, IGetLocationsCallback
        {
            public Task<IReadOnlyList<Mas.Schema.Climate.Location>> NextLocations(long maxCount, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Dataset.GetLocationsCallback.Params_NextLocations.WRITER>();
                var arg_ = new Mas.Schema.Climate.Dataset.GetLocationsCallback.Params_NextLocations()
                {MaxCount = maxCount};
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(15428101162159563125UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Climate.Dataset.GetLocationsCallback.Result_NextLocations>(d_);
                        return (r_.Locations);
                    }
                }

                );
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd61ba043f14fe175UL)]
        public class GetLocationsCallback_Skeleton : Skeleton<IGetLocationsCallback>
        {
            public GetLocationsCallback_Skeleton()
            {
                SetMethodTable(NextLocations);
            }

            public override ulong InterfaceId => 15428101162159563125UL;
            Task<AnswerOrCounterquestion> NextLocations(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Climate.Dataset.GetLocationsCallback.Params_NextLocations>(d_);
                    return Impatient.MaybeTailCall(Impl.NextLocations(in_.MaxCount, cancellationToken_), locations =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Dataset.GetLocationsCallback.Result_NextLocations.WRITER>();
                        var r_ = new Mas.Schema.Climate.Dataset.GetLocationsCallback.Result_NextLocations{Locations = locations};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }
        }

        public static class GetLocationsCallback
        {
            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe64112993dc4d4e0UL)]
            public class Params_NextLocations : ICapnpSerializable
            {
                public const UInt64 typeId = 0xe64112993dc4d4e0UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    MaxCount = reader.MaxCount;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.MaxCount = MaxCount;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public long MaxCount
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
                    public long MaxCount => ctx.ReadDataLong(0UL, 0L);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 0);
                    }

                    public long MaxCount
                    {
                        get => this.ReadDataLong(0UL, 0L);
                        set => this.WriteData(0UL, value, 0L);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfa8540d5d8065df1UL)]
            public class Result_NextLocations : ICapnpSerializable
            {
                public const UInt64 typeId = 0xfa8540d5d8065df1UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Locations = reader.Locations?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Climate.Location>(_));
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Locations.Init(Locations, (_s1, _v1) => _v1?.serialize(_s1));
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<Mas.Schema.Climate.Location> Locations
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
                    public IReadOnlyList<Mas.Schema.Climate.Location.READER> Locations => ctx.ReadList(0).Cast(Mas.Schema.Climate.Location.READER.create);
                    public bool HasLocations => ctx.IsStructFieldNonNull(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfStructsSerializer<Mas.Schema.Climate.Location.WRITER> Locations
                    {
                        get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Climate.Location.WRITER>>(0);
                        set => Link(0, value);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb4c346906ee84815UL)]
        public class Params_Metadata : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb4c346906ee84815UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb0496f3d284f4a13UL)]
        public class Params_ClosestTimeSeriesAt : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb0496f3d284f4a13UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Latlon = CapnpSerializable.Create<Mas.Schema.Geo.LatLonCoord>(reader.Latlon);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Latlon?.serialize(writer.Latlon);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Geo.LatLonCoord Latlon
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
                public Mas.Schema.Geo.LatLonCoord.READER Latlon => ctx.ReadStruct(0, Mas.Schema.Geo.LatLonCoord.READER.create);
                public bool HasLatlon => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Geo.LatLonCoord.WRITER Latlon
                {
                    get => BuildPointer<Mas.Schema.Geo.LatLonCoord.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xedee5faa03af6a1eUL)]
        public class Result_ClosestTimeSeriesAt : ICapnpSerializable
        {
            public const UInt64 typeId = 0xedee5faa03af6a1eUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                TimeSeries = reader.TimeSeries;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.TimeSeries = TimeSeries;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Climate.ITimeSeries TimeSeries
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
                public Mas.Schema.Climate.ITimeSeries TimeSeries => ctx.ReadCap<Mas.Schema.Climate.ITimeSeries>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Climate.ITimeSeries TimeSeries
                {
                    get => ReadCap<Mas.Schema.Climate.ITimeSeries>(0);
                    set => LinkObject(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd9f867b0a2a15d7fUL)]
        public class Params_TimeSeriesAt : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd9f867b0a2a15d7fUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                LocationId = reader.LocationId;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.LocationId = LocationId;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string LocationId
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
                public string LocationId => ctx.ReadText(0, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string LocationId
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe30c466e5bc2735cUL)]
        public class Result_TimeSeriesAt : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe30c466e5bc2735cUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                TimeSeries = reader.TimeSeries;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.TimeSeries = TimeSeries;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Climate.ITimeSeries TimeSeries
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
                public Mas.Schema.Climate.ITimeSeries TimeSeries => ctx.ReadCap<Mas.Schema.Climate.ITimeSeries>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Climate.ITimeSeries TimeSeries
                {
                    get => ReadCap<Mas.Schema.Climate.ITimeSeries>(0);
                    set => LinkObject(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd2a02e856c28d4baUL)]
        public class Params_Locations : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd2a02e856c28d4baUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaa8cfcdc401d5fddUL)]
        public class Result_Locations : ICapnpSerializable
        {
            public const UInt64 typeId = 0xaa8cfcdc401d5fddUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Locations = reader.Locations?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Climate.Location>(_));
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Locations.Init(Locations, (_s1, _v1) => _v1?.serialize(_s1));
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<Mas.Schema.Climate.Location> Locations
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
                public IReadOnlyList<Mas.Schema.Climate.Location.READER> Locations => ctx.ReadList(0).Cast(Mas.Schema.Climate.Location.READER.create);
                public bool HasLocations => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfStructsSerializer<Mas.Schema.Climate.Location.WRITER> Locations
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Climate.Location.WRITER>>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfca3f0f431b64506UL)]
        public class Params_StreamLocations : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfca3f0f431b64506UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                StartAfterLocationId = reader.StartAfterLocationId;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.StartAfterLocationId = StartAfterLocationId;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string StartAfterLocationId
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
                public string StartAfterLocationId => ctx.ReadText(0, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string StartAfterLocationId
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9ebadb578b79fa06UL)]
        public class Result_StreamLocations : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9ebadb578b79fa06UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                LocationsCallback = reader.LocationsCallback;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.LocationsCallback = LocationsCallback;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Climate.Dataset.IGetLocationsCallback LocationsCallback
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
                public Mas.Schema.Climate.Dataset.IGetLocationsCallback LocationsCallback => ctx.ReadCap<Mas.Schema.Climate.Dataset.IGetLocationsCallback>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Climate.Dataset.IGetLocationsCallback LocationsCallback
                {
                    get => ReadCap<Mas.Schema.Climate.Dataset.IGetLocationsCallback>(0);
                    set => LinkObject(0, value);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd7a67fec5f22e5a0UL)]
    public class MetaPlusData : ICapnpSerializable
    {
        public const UInt64 typeId = 0xd7a67fec5f22e5a0UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Meta = CapnpSerializable.Create<Mas.Schema.Climate.Metadata>(reader.Meta);
            Data = reader.Data;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            Meta?.serialize(writer.Meta);
            writer.Data = Data;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public Mas.Schema.Climate.Metadata Meta
        {
            get;
            set;
        }

        public Mas.Schema.Climate.IDataset Data
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
            public Mas.Schema.Climate.Metadata.READER Meta => ctx.ReadStruct(0, Mas.Schema.Climate.Metadata.READER.create);
            public bool HasMeta => ctx.IsStructFieldNonNull(0);
            public Mas.Schema.Climate.IDataset Data => ctx.ReadCap<Mas.Schema.Climate.IDataset>(1);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 2);
            }

            public Mas.Schema.Climate.Metadata.WRITER Meta
            {
                get => BuildPointer<Mas.Schema.Climate.Metadata.WRITER>(0);
                set => Link(0, value);
            }

            public Mas.Schema.Climate.IDataset Data
            {
                get => ReadCap<Mas.Schema.Climate.IDataset>(1);
                set => LinkObject(1, value);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe35760b4db5ab564UL)]
    public enum Element : ushort
    {
        tmin,
        tavg,
        tmax,
        precip,
        globrad,
        wind,
        sunhours,
        cloudamount,
        relhumid,
        airpress,
        vaporpress,
        co2,
        o3,
        et0,
        dewpointTemp
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x85ba7385f313fe19UL)]
    public class Location : ICapnpSerializable
    {
        public const UInt64 typeId = 0x85ba7385f313fe19UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Id = CapnpSerializable.Create<Mas.Schema.Common.IdInformation>(reader.Id);
            HeightNN = reader.HeightNN;
            Latlon = CapnpSerializable.Create<Mas.Schema.Geo.LatLonCoord>(reader.Latlon);
            TimeSeries = reader.TimeSeries;
            CustomData = reader.CustomData?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Climate.Location.KV>(_));
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            Id?.serialize(writer.Id);
            writer.HeightNN = HeightNN;
            Latlon?.serialize(writer.Latlon);
            writer.TimeSeries = TimeSeries;
            writer.CustomData.Init(CustomData, (_s1, _v1) => _v1?.serialize(_s1));
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public Mas.Schema.Common.IdInformation Id
        {
            get;
            set;
        }

        public float HeightNN
        {
            get;
            set;
        }

        public Mas.Schema.Geo.LatLonCoord Latlon
        {
            get;
            set;
        }

        public Mas.Schema.Climate.ITimeSeries TimeSeries
        {
            get;
            set;
        }

        public IReadOnlyList<Mas.Schema.Climate.Location.KV> CustomData
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
            public Mas.Schema.Common.IdInformation.READER Id => ctx.ReadStruct(0, Mas.Schema.Common.IdInformation.READER.create);
            public bool HasId => ctx.IsStructFieldNonNull(0);
            public float HeightNN => ctx.ReadDataFloat(0UL, 0F);
            public Mas.Schema.Geo.LatLonCoord.READER Latlon => ctx.ReadStruct(1, Mas.Schema.Geo.LatLonCoord.READER.create);
            public bool HasLatlon => ctx.IsStructFieldNonNull(1);
            public Mas.Schema.Climate.ITimeSeries TimeSeries => ctx.ReadCap<Mas.Schema.Climate.ITimeSeries>(2);
            public IReadOnlyList<Mas.Schema.Climate.Location.KV.READER> CustomData => ctx.ReadList(3).Cast(Mas.Schema.Climate.Location.KV.READER.create);
            public bool HasCustomData => ctx.IsStructFieldNonNull(3);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 4);
            }

            public Mas.Schema.Common.IdInformation.WRITER Id
            {
                get => BuildPointer<Mas.Schema.Common.IdInformation.WRITER>(0);
                set => Link(0, value);
            }

            public float HeightNN
            {
                get => this.ReadDataFloat(0UL, 0F);
                set => this.WriteData(0UL, value, 0F);
            }

            public Mas.Schema.Geo.LatLonCoord.WRITER Latlon
            {
                get => BuildPointer<Mas.Schema.Geo.LatLonCoord.WRITER>(1);
                set => Link(1, value);
            }

            public Mas.Schema.Climate.ITimeSeries TimeSeries
            {
                get => ReadCap<Mas.Schema.Climate.ITimeSeries>(2);
                set => LinkObject(2, value);
            }

            public ListOfStructsSerializer<Mas.Schema.Climate.Location.KV.WRITER> CustomData
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Climate.Location.KV.WRITER>>(3);
                set => Link(3, value);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc5fd13a53ae6d46aUL)]
        public class KV : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc5fd13a53ae6d46aUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Key = reader.Key;
                Value = CapnpSerializable.Create<object>(reader.Value);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Key = Key;
                writer.Value.SetObject(Value);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Key
            {
                get;
                set;
            }

            public object Value
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
                public string Key => ctx.ReadText(0, null);
                public DeserializerState Value => ctx.StructReadPointer(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public string Key
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public DynamicSerializerState Value
                {
                    get => BuildPointer<DynamicSerializerState>(1);
                    set => Link(1, value);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa7769f40fe6e6de8UL), Proxy(typeof(TimeSeries_Proxy)), Skeleton(typeof(TimeSeries_Skeleton))]
    public interface ITimeSeries : Mas.Schema.Common.IIdentifiable, Mas.Schema.Persistence.IPersistent
    {
        Task<Mas.Schema.Climate.TimeSeries.Resolution> Resolution(CancellationToken cancellationToken_ = default);
        Task<(Mas.Schema.Common.Date, Mas.Schema.Common.Date)> Range(CancellationToken cancellationToken_ = default);
        Task<IReadOnlyList<Mas.Schema.Climate.Element>> Header(CancellationToken cancellationToken_ = default);
        Task<IReadOnlyList<IReadOnlyList<float>>> Data(CancellationToken cancellationToken_ = default);
        Task<IReadOnlyList<IReadOnlyList<float>>> DataT(CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Climate.ITimeSeries> Subrange(Mas.Schema.Common.Date start, Mas.Schema.Common.Date end, CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Climate.ITimeSeries> Subheader(IReadOnlyList<Mas.Schema.Climate.Element> elements, CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Climate.Metadata> Metadata(CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Climate.Location> Location(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa7769f40fe6e6de8UL)]
    public class TimeSeries_Proxy : Proxy, ITimeSeries
    {
        public async Task<Mas.Schema.Climate.TimeSeries.Resolution> Resolution(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Params_Resolution.WRITER>();
            var arg_ = new Mas.Schema.Climate.TimeSeries.Params_Resolution()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(12067007353081196008UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Climate.TimeSeries.Result_Resolution>(d_);
                return (r_.Resolution);
            }
        }

        public async Task<(Mas.Schema.Common.Date, Mas.Schema.Common.Date)> Range(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Params_Range.WRITER>();
            var arg_ = new Mas.Schema.Climate.TimeSeries.Params_Range()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(12067007353081196008UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Climate.TimeSeries.Result_Range>(d_);
                return (r_.StartDate, r_.EndDate);
            }
        }

        public async Task<IReadOnlyList<Mas.Schema.Climate.Element>> Header(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Params_Header.WRITER>();
            var arg_ = new Mas.Schema.Climate.TimeSeries.Params_Header()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(12067007353081196008UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Climate.TimeSeries.Result_Header>(d_);
                return (r_.Header);
            }
        }

        public async Task<IReadOnlyList<IReadOnlyList<float>>> Data(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Params_Data.WRITER>();
            var arg_ = new Mas.Schema.Climate.TimeSeries.Params_Data()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(12067007353081196008UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Climate.TimeSeries.Result_Data>(d_);
                return (r_.Data);
            }
        }

        public async Task<IReadOnlyList<IReadOnlyList<float>>> DataT(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Params_DataT.WRITER>();
            var arg_ = new Mas.Schema.Climate.TimeSeries.Params_DataT()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(12067007353081196008UL, 4, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Climate.TimeSeries.Result_DataT>(d_);
                return (r_.Data);
            }
        }

        public Task<Mas.Schema.Climate.ITimeSeries> Subrange(Mas.Schema.Common.Date start, Mas.Schema.Common.Date end, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Params_Subrange.WRITER>();
            var arg_ = new Mas.Schema.Climate.TimeSeries.Params_Subrange()
            {Start = start, End = end};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(12067007353081196008UL, 5, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.TimeSeries.Result_Subrange>(d_);
                    return (r_.TimeSeries);
                }
            }

            );
        }

        public Task<Mas.Schema.Climate.ITimeSeries> Subheader(IReadOnlyList<Mas.Schema.Climate.Element> elements, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Params_Subheader.WRITER>();
            var arg_ = new Mas.Schema.Climate.TimeSeries.Params_Subheader()
            {Elements = elements};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(12067007353081196008UL, 6, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.TimeSeries.Result_Subheader>(d_);
                    return (r_.TimeSeries);
                }
            }

            );
        }

        public Task<Mas.Schema.Climate.Metadata> Metadata(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Params_Metadata.WRITER>();
            var arg_ = new Mas.Schema.Climate.TimeSeries.Params_Metadata()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(12067007353081196008UL, 7, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.Metadata>(d_);
                    return r_;
                }
            }

            );
        }

        public Task<Mas.Schema.Climate.Location> Location(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Params_Location.WRITER>();
            var arg_ = new Mas.Schema.Climate.TimeSeries.Params_Location()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(12067007353081196008UL, 8, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.Location>(d_);
                    return r_;
                }
            }

            );
        }

        public async Task<Mas.Schema.Persistence.Persistent.SaveResults> Save(Mas.Schema.Persistence.Persistent.SaveParams arg_, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.Persistent.SaveParams.WRITER>();
            arg_?.serialize(in_);
            using (var d_ = await Call(13954362354854972261UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Persistence.Persistent.SaveResults>(d_);
                return r_;
            }
        }

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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa7769f40fe6e6de8UL)]
    public class TimeSeries_Skeleton : Skeleton<ITimeSeries>
    {
        public TimeSeries_Skeleton()
        {
            SetMethodTable(Resolution, Range, Header, Data, DataT, Subrange, Subheader, Metadata, Location);
        }

        public override ulong InterfaceId => 12067007353081196008UL;
        Task<AnswerOrCounterquestion> Resolution(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Resolution(cancellationToken_), resolution =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Result_Resolution.WRITER>();
                    var r_ = new Mas.Schema.Climate.TimeSeries.Result_Resolution{Resolution = resolution};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> Range(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Range(cancellationToken_), (startDate, endDate) =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Result_Range.WRITER>();
                    var r_ = new Mas.Schema.Climate.TimeSeries.Result_Range{StartDate = startDate, EndDate = endDate};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> Header(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Header(cancellationToken_), header =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Result_Header.WRITER>();
                    var r_ = new Mas.Schema.Climate.TimeSeries.Result_Header{Header = header};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> Data(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Data(cancellationToken_), data =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Result_Data.WRITER>();
                    var r_ = new Mas.Schema.Climate.TimeSeries.Result_Data{Data = data};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> DataT(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.DataT(cancellationToken_), data =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Result_DataT.WRITER>();
                    var r_ = new Mas.Schema.Climate.TimeSeries.Result_DataT{Data = data};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> Subrange(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Climate.TimeSeries.Params_Subrange>(d_);
                return Impatient.MaybeTailCall(Impl.Subrange(in_.Start, in_.End, cancellationToken_), timeSeries =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Result_Subrange.WRITER>();
                    var r_ = new Mas.Schema.Climate.TimeSeries.Result_Subrange{TimeSeries = timeSeries};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> Subheader(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Climate.TimeSeries.Params_Subheader>(d_);
                return Impatient.MaybeTailCall(Impl.Subheader(in_.Elements, cancellationToken_), timeSeries =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Result_Subheader.WRITER>();
                    var r_ = new Mas.Schema.Climate.TimeSeries.Result_Subheader{TimeSeries = timeSeries};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> Metadata(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Metadata(cancellationToken_), r_ =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Metadata.WRITER>();
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> Location(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Location(cancellationToken_), r_ =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Location.WRITER>();
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class TimeSeries
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb466cacf63ec03c2UL)]
        public enum Resolution : ushort
        {
            daily,
            hourly
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xea3f0519d272fdd1UL)]
        public class Params_Resolution : ICapnpSerializable
        {
            public const UInt64 typeId = 0xea3f0519d272fdd1UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcd0eadd9a1a66ed6UL)]
        public class Result_Resolution : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcd0eadd9a1a66ed6UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Resolution = reader.Resolution;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Resolution = Resolution;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Climate.TimeSeries.Resolution Resolution
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
                public Mas.Schema.Climate.TimeSeries.Resolution Resolution => (Mas.Schema.Climate.TimeSeries.Resolution)ctx.ReadDataUShort(0UL, (ushort)0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public Mas.Schema.Climate.TimeSeries.Resolution Resolution
                {
                    get => (Mas.Schema.Climate.TimeSeries.Resolution)this.ReadDataUShort(0UL, (ushort)0);
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xff6bcf0c6b23c916UL)]
        public class Params_Range : ICapnpSerializable
        {
            public const UInt64 typeId = 0xff6bcf0c6b23c916UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb9ec27f476022c1bUL)]
        public class Result_Range : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb9ec27f476022c1bUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                StartDate = CapnpSerializable.Create<Mas.Schema.Common.Date>(reader.StartDate);
                EndDate = CapnpSerializable.Create<Mas.Schema.Common.Date>(reader.EndDate);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                StartDate?.serialize(writer.StartDate);
                EndDate?.serialize(writer.EndDate);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Common.Date StartDate
            {
                get;
                set;
            }

            public Mas.Schema.Common.Date EndDate
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
                public Mas.Schema.Common.Date.READER StartDate => ctx.ReadStruct(0, Mas.Schema.Common.Date.READER.create);
                public bool HasStartDate => ctx.IsStructFieldNonNull(0);
                public Mas.Schema.Common.Date.READER EndDate => ctx.ReadStruct(1, Mas.Schema.Common.Date.READER.create);
                public bool HasEndDate => ctx.IsStructFieldNonNull(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public Mas.Schema.Common.Date.WRITER StartDate
                {
                    get => BuildPointer<Mas.Schema.Common.Date.WRITER>(0);
                    set => Link(0, value);
                }

                public Mas.Schema.Common.Date.WRITER EndDate
                {
                    get => BuildPointer<Mas.Schema.Common.Date.WRITER>(1);
                    set => Link(1, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8fd77002ae8a97a1UL)]
        public class Params_Header : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8fd77002ae8a97a1UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8976146f144fa050UL)]
        public class Result_Header : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8976146f144fa050UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Header = reader.Header;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Header.Init(Header);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<Mas.Schema.Climate.Element> Header
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
                public IReadOnlyList<Mas.Schema.Climate.Element> Header => ctx.ReadList(0).CastEnums(_0 => (Mas.Schema.Climate.Element)_0);
                public bool HasHeader => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfPrimitivesSerializer<Mas.Schema.Climate.Element> Header
                {
                    get => BuildPointer<ListOfPrimitivesSerializer<Mas.Schema.Climate.Element>>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8e78986bc45d7dcdUL)]
        public class Params_Data : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8e78986bc45d7dcdUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9c3d3448d73eeae9UL)]
        public class Result_Data : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9c3d3448d73eeae9UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Data = reader.Data;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Data.Init(Data, (_s2, _v2) => _s2.Init(_v2));
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<IReadOnlyList<float>> Data
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
                public IReadOnlyList<IReadOnlyList<float>> Data => ctx.ReadList(0).Cast(_0 => _0.RequireList().CastFloat());
                public bool HasData => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfPointersSerializer<ListOfPrimitivesSerializer<float>> Data
                {
                    get => BuildPointer<ListOfPointersSerializer<ListOfPrimitivesSerializer<float>>>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xeff8f923b1853525UL)]
        public class Params_DataT : ICapnpSerializable
        {
            public const UInt64 typeId = 0xeff8f923b1853525UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc2e0dec0a6ea94fbUL)]
        public class Result_DataT : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc2e0dec0a6ea94fbUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Data = reader.Data;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Data.Init(Data, (_s2, _v2) => _s2.Init(_v2));
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<IReadOnlyList<float>> Data
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
                public IReadOnlyList<IReadOnlyList<float>> Data => ctx.ReadList(0).Cast(_0 => _0.RequireList().CastFloat());
                public bool HasData => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfPointersSerializer<ListOfPrimitivesSerializer<float>> Data
                {
                    get => BuildPointer<ListOfPointersSerializer<ListOfPrimitivesSerializer<float>>>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf8aa5b6fe2496feeUL)]
        public class Params_Subrange : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf8aa5b6fe2496feeUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Start = CapnpSerializable.Create<Mas.Schema.Common.Date>(reader.Start);
                End = CapnpSerializable.Create<Mas.Schema.Common.Date>(reader.End);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Start?.serialize(writer.Start);
                End?.serialize(writer.End);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Common.Date Start
            {
                get;
                set;
            }

            public Mas.Schema.Common.Date End
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
                public Mas.Schema.Common.Date.READER Start => ctx.ReadStruct(0, Mas.Schema.Common.Date.READER.create);
                public bool HasStart => ctx.IsStructFieldNonNull(0);
                public Mas.Schema.Common.Date.READER End => ctx.ReadStruct(1, Mas.Schema.Common.Date.READER.create);
                public bool HasEnd => ctx.IsStructFieldNonNull(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public Mas.Schema.Common.Date.WRITER Start
                {
                    get => BuildPointer<Mas.Schema.Common.Date.WRITER>(0);
                    set => Link(0, value);
                }

                public Mas.Schema.Common.Date.WRITER End
                {
                    get => BuildPointer<Mas.Schema.Common.Date.WRITER>(1);
                    set => Link(1, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf7dfe7147d09b732UL)]
        public class Result_Subrange : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf7dfe7147d09b732UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                TimeSeries = reader.TimeSeries;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.TimeSeries = TimeSeries;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Climate.ITimeSeries TimeSeries
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
                public Mas.Schema.Climate.ITimeSeries TimeSeries => ctx.ReadCap<Mas.Schema.Climate.ITimeSeries>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Climate.ITimeSeries TimeSeries
                {
                    get => ReadCap<Mas.Schema.Climate.ITimeSeries>(0);
                    set => LinkObject(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8cc364dee8f693b8UL)]
        public class Params_Subheader : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8cc364dee8f693b8UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Elements = reader.Elements;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Elements.Init(Elements);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<Mas.Schema.Climate.Element> Elements
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
                public IReadOnlyList<Mas.Schema.Climate.Element> Elements => ctx.ReadList(0).CastEnums(_0 => (Mas.Schema.Climate.Element)_0);
                public bool HasElements => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfPrimitivesSerializer<Mas.Schema.Climate.Element> Elements
                {
                    get => BuildPointer<ListOfPrimitivesSerializer<Mas.Schema.Climate.Element>>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc3238163cae880dfUL)]
        public class Result_Subheader : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc3238163cae880dfUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                TimeSeries = reader.TimeSeries;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.TimeSeries = TimeSeries;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Climate.ITimeSeries TimeSeries
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
                public Mas.Schema.Climate.ITimeSeries TimeSeries => ctx.ReadCap<Mas.Schema.Climate.ITimeSeries>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Climate.ITimeSeries TimeSeries
                {
                    get => ReadCap<Mas.Schema.Climate.ITimeSeries>(0);
                    set => LinkObject(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xce2cc4225c956634UL)]
        public class Params_Metadata : ICapnpSerializable
        {
            public const UInt64 typeId = 0xce2cc4225c956634UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcb329eb01b0fa313UL)]
        public class Params_Location : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcb329eb01b0fa313UL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf1c1ccf59bc6964fUL)]
    public class TimeSeriesData : ICapnpSerializable
    {
        public const UInt64 typeId = 0xf1c1ccf59bc6964fUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Data = reader.Data;
            IsTransposed = reader.IsTransposed;
            Header = reader.Header;
            StartDate = CapnpSerializable.Create<Mas.Schema.Common.Date>(reader.StartDate);
            EndDate = CapnpSerializable.Create<Mas.Schema.Common.Date>(reader.EndDate);
            Resolution = reader.Resolution;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Data.Init(Data, (_s2, _v2) => _s2.Init(_v2));
            writer.IsTransposed = IsTransposed;
            writer.Header.Init(Header);
            StartDate?.serialize(writer.StartDate);
            EndDate?.serialize(writer.EndDate);
            writer.Resolution = Resolution;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public IReadOnlyList<IReadOnlyList<float>> Data
        {
            get;
            set;
        }

        public bool IsTransposed
        {
            get;
            set;
        }

        public IReadOnlyList<Mas.Schema.Climate.Element> Header
        {
            get;
            set;
        }

        public Mas.Schema.Common.Date StartDate
        {
            get;
            set;
        }

        public Mas.Schema.Common.Date EndDate
        {
            get;
            set;
        }

        public Mas.Schema.Climate.TimeSeries.Resolution Resolution
        {
            get;
            set;
        }

        = Mas.Schema.Climate.TimeSeries.Resolution.daily;
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
            public IReadOnlyList<IReadOnlyList<float>> Data => ctx.ReadList(0).Cast(_0 => _0.RequireList().CastFloat());
            public bool HasData => ctx.IsStructFieldNonNull(0);
            public bool IsTransposed => ctx.ReadDataBool(0UL, false);
            public IReadOnlyList<Mas.Schema.Climate.Element> Header => ctx.ReadList(1).CastEnums(_0 => (Mas.Schema.Climate.Element)_0);
            public bool HasHeader => ctx.IsStructFieldNonNull(1);
            public Mas.Schema.Common.Date.READER StartDate => ctx.ReadStruct(2, Mas.Schema.Common.Date.READER.create);
            public bool HasStartDate => ctx.IsStructFieldNonNull(2);
            public Mas.Schema.Common.Date.READER EndDate => ctx.ReadStruct(3, Mas.Schema.Common.Date.READER.create);
            public bool HasEndDate => ctx.IsStructFieldNonNull(3);
            public Mas.Schema.Climate.TimeSeries.Resolution Resolution => (Mas.Schema.Climate.TimeSeries.Resolution)ctx.ReadDataUShort(16UL, (ushort)0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 4);
            }

            public ListOfPointersSerializer<ListOfPrimitivesSerializer<float>> Data
            {
                get => BuildPointer<ListOfPointersSerializer<ListOfPrimitivesSerializer<float>>>(0);
                set => Link(0, value);
            }

            public bool IsTransposed
            {
                get => this.ReadDataBool(0UL, false);
                set => this.WriteData(0UL, value, false);
            }

            public ListOfPrimitivesSerializer<Mas.Schema.Climate.Element> Header
            {
                get => BuildPointer<ListOfPrimitivesSerializer<Mas.Schema.Climate.Element>>(1);
                set => Link(1, value);
            }

            public Mas.Schema.Common.Date.WRITER StartDate
            {
                get => BuildPointer<Mas.Schema.Common.Date.WRITER>(2);
                set => Link(2, value);
            }

            public Mas.Schema.Common.Date.WRITER EndDate
            {
                get => BuildPointer<Mas.Schema.Common.Date.WRITER>(3);
                set => Link(3, value);
            }

            public Mas.Schema.Climate.TimeSeries.Resolution Resolution
            {
                get => (Mas.Schema.Climate.TimeSeries.Resolution)this.ReadDataUShort(16UL, (ushort)0);
                set => this.WriteData(16UL, (ushort)value, (ushort)0);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfe7d08d4352b0c5fUL), Proxy(typeof(Service_Proxy)), Skeleton(typeof(Service_Skeleton))]
    public interface IService : Mas.Schema.Common.IIdentifiable, Mas.Schema.Persistence.IPersistent
    {
        Task<IReadOnlyList<Mas.Schema.Climate.MetaPlusData>> GetAvailableDatasets(CancellationToken cancellationToken_ = default);
        Task<IReadOnlyList<Mas.Schema.Climate.IDataset>> GetDatasetsFor(Mas.Schema.Climate.Metadata template, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfe7d08d4352b0c5fUL)]
    public class Service_Proxy : Proxy, IService
    {
        public Task<IReadOnlyList<Mas.Schema.Climate.MetaPlusData>> GetAvailableDatasets(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Service.Params_GetAvailableDatasets.WRITER>();
            var arg_ = new Mas.Schema.Climate.Service.Params_GetAvailableDatasets()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(18337822965240630367UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.Service.Result_GetAvailableDatasets>(d_);
                    return (r_.Datasets);
                }
            }

            );
        }

        public Task<IReadOnlyList<Mas.Schema.Climate.IDataset>> GetDatasetsFor(Mas.Schema.Climate.Metadata template, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Service.Params_GetDatasetsFor.WRITER>();
            var arg_ = new Mas.Schema.Climate.Service.Params_GetDatasetsFor()
            {Template = template};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(18337822965240630367UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.Service.Result_GetDatasetsFor>(d_);
                    return (r_.Datasets);
                }
            }

            );
        }

        public async Task<Mas.Schema.Persistence.Persistent.SaveResults> Save(Mas.Schema.Persistence.Persistent.SaveParams arg_, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.Persistent.SaveParams.WRITER>();
            arg_?.serialize(in_);
            using (var d_ = await Call(13954362354854972261UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Persistence.Persistent.SaveResults>(d_);
                return r_;
            }
        }

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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfe7d08d4352b0c5fUL)]
    public class Service_Skeleton : Skeleton<IService>
    {
        public Service_Skeleton()
        {
            SetMethodTable(GetAvailableDatasets, GetDatasetsFor);
        }

        public override ulong InterfaceId => 18337822965240630367UL;
        Task<AnswerOrCounterquestion> GetAvailableDatasets(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.GetAvailableDatasets(cancellationToken_), datasets =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Service.Result_GetAvailableDatasets.WRITER>();
                    var r_ = new Mas.Schema.Climate.Service.Result_GetAvailableDatasets{Datasets = datasets};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> GetDatasetsFor(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Climate.Service.Params_GetDatasetsFor>(d_);
                return Impatient.MaybeTailCall(Impl.GetDatasetsFor(in_.Template, cancellationToken_), datasets =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.Service.Result_GetDatasetsFor.WRITER>();
                    var r_ = new Mas.Schema.Climate.Service.Result_GetDatasetsFor{Datasets = datasets};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class Service
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x804cca489405d451UL)]
        public class Params_GetAvailableDatasets : ICapnpSerializable
        {
            public const UInt64 typeId = 0x804cca489405d451UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x916880859435c6e8UL)]
        public class Result_GetAvailableDatasets : ICapnpSerializable
        {
            public const UInt64 typeId = 0x916880859435c6e8UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Datasets = reader.Datasets?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Climate.MetaPlusData>(_));
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Datasets.Init(Datasets, (_s1, _v1) => _v1?.serialize(_s1));
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<Mas.Schema.Climate.MetaPlusData> Datasets
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
                public IReadOnlyList<Mas.Schema.Climate.MetaPlusData.READER> Datasets => ctx.ReadList(0).Cast(Mas.Schema.Climate.MetaPlusData.READER.create);
                public bool HasDatasets => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfStructsSerializer<Mas.Schema.Climate.MetaPlusData.WRITER> Datasets
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Climate.MetaPlusData.WRITER>>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9d7d1f83dda3e6dbUL)]
        public class Params_GetDatasetsFor : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9d7d1f83dda3e6dbUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Template = CapnpSerializable.Create<Mas.Schema.Climate.Metadata>(reader.Template);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Template?.serialize(writer.Template);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Climate.Metadata Template
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
                public Mas.Schema.Climate.Metadata.READER Template => ctx.ReadStruct(0, Mas.Schema.Climate.Metadata.READER.create);
                public bool HasTemplate => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Climate.Metadata.WRITER Template
                {
                    get => BuildPointer<Mas.Schema.Climate.Metadata.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcd95f79174b0eab0UL)]
        public class Result_GetDatasetsFor : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcd95f79174b0eab0UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Datasets = reader.Datasets;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Datasets.Init(Datasets);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<Mas.Schema.Climate.IDataset> Datasets
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
                public IReadOnlyList<Mas.Schema.Climate.IDataset> Datasets => ctx.ReadCapList<Mas.Schema.Climate.IDataset>(0);
                public bool HasDatasets => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfCapsSerializer<Mas.Schema.Climate.IDataset> Datasets
                {
                    get => BuildPointer<ListOfCapsSerializer<Mas.Schema.Climate.IDataset>>(0);
                    set => Link(0, value);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa418c26cc59929d9UL), Proxy(typeof(CSVTimeSeriesFactory_Proxy)), Skeleton(typeof(CSVTimeSeriesFactory_Skeleton))]
    public interface ICSVTimeSeriesFactory : Mas.Schema.Common.IIdentifiable
    {
        Task<(Mas.Schema.Climate.ITimeSeries, string)> Create(string csvData, Mas.Schema.Climate.CSVTimeSeriesFactory.CSVConfig config, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa418c26cc59929d9UL)]
    public class CSVTimeSeriesFactory_Proxy : Proxy, ICSVTimeSeriesFactory
    {
        public Task<(Mas.Schema.Climate.ITimeSeries, string)> Create(string csvData, Mas.Schema.Climate.CSVTimeSeriesFactory.CSVConfig config, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.CSVTimeSeriesFactory.Params_Create.WRITER>();
            var arg_ = new Mas.Schema.Climate.CSVTimeSeriesFactory.Params_Create()
            {CsvData = csvData, Config = config};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(11824414594088643033UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.CSVTimeSeriesFactory.Result_Create>(d_);
                    return (r_.Timeseries, r_.Error);
                }
            }

            );
        }

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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa418c26cc59929d9UL)]
    public class CSVTimeSeriesFactory_Skeleton : Skeleton<ICSVTimeSeriesFactory>
    {
        public CSVTimeSeriesFactory_Skeleton()
        {
            SetMethodTable(Create);
        }

        public override ulong InterfaceId => 11824414594088643033UL;
        Task<AnswerOrCounterquestion> Create(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Climate.CSVTimeSeriesFactory.Params_Create>(d_);
                return Impatient.MaybeTailCall(Impl.Create(in_.CsvData, in_.Config, cancellationToken_), (timeseries, error) =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.CSVTimeSeriesFactory.Result_Create.WRITER>();
                    var r_ = new Mas.Schema.Climate.CSVTimeSeriesFactory.Result_Create{Timeseries = timeseries, Error = error};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class CSVTimeSeriesFactory
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xeba81ca9f46690b8UL)]
        public class CSVConfig : ICapnpSerializable
        {
            public const UInt64 typeId = 0xeba81ca9f46690b8UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Sep = reader.Sep;
                HeaderMap = reader.HeaderMap?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Common.Pair<string, string>>(_));
                SkipLinesToHeader = reader.SkipLinesToHeader;
                SkipLinesFromHeaderToData = reader.SkipLinesFromHeaderToData;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Sep = Sep;
                writer.HeaderMap.Init(HeaderMap, (_s1, _v1) => _v1?.serialize(_s1));
                writer.SkipLinesToHeader = SkipLinesToHeader;
                writer.SkipLinesFromHeaderToData = SkipLinesFromHeaderToData;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
                Sep = Sep ?? ",";
            }

            public string Sep
            {
                get;
                set;
            }

            public IReadOnlyList<Mas.Schema.Common.Pair<string, string>> HeaderMap
            {
                get;
                set;
            }

            public short SkipLinesToHeader
            {
                get;
                set;
            }

            = 0;
            public short SkipLinesFromHeaderToData
            {
                get;
                set;
            }

            = 1;
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
                public string Sep => ctx.ReadText(0, ",");
                public IReadOnlyList<Mas.Schema.Common.Pair<string, string>.READER> HeaderMap => ctx.ReadList(1).Cast(Mas.Schema.Common.Pair<string, string>.READER.create);
                public bool HasHeaderMap => ctx.IsStructFieldNonNull(1);
                public short SkipLinesToHeader => ctx.ReadDataShort(0UL, (short)0);
                public short SkipLinesFromHeaderToData => ctx.ReadDataShort(16UL, (short)1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 2);
                }

                public string Sep
                {
                    get => this.ReadText(0, ",");
                    set => this.WriteText(0, value, ",");
                }

                public ListOfStructsSerializer<Mas.Schema.Common.Pair<string, string>.WRITER> HeaderMap
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Common.Pair<string, string>.WRITER>>(1);
                    set => Link(1, value);
                }

                public short SkipLinesToHeader
                {
                    get => this.ReadDataShort(0UL, (short)0);
                    set => this.WriteData(0UL, value, (short)0);
                }

                public short SkipLinesFromHeaderToData
                {
                    get => this.ReadDataShort(16UL, (short)1);
                    set => this.WriteData(16UL, value, (short)1);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcfaa8d2601750547UL)]
        public class Params_Create : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcfaa8d2601750547UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                CsvData = reader.CsvData;
                Config = CapnpSerializable.Create<Mas.Schema.Climate.CSVTimeSeriesFactory.CSVConfig>(reader.Config);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.CsvData = CsvData;
                Config?.serialize(writer.Config);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string CsvData
            {
                get;
                set;
            }

            public Mas.Schema.Climate.CSVTimeSeriesFactory.CSVConfig Config
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
                public string CsvData => ctx.ReadText(0, null);
                public Mas.Schema.Climate.CSVTimeSeriesFactory.CSVConfig.READER Config => ctx.ReadStruct(1, Mas.Schema.Climate.CSVTimeSeriesFactory.CSVConfig.READER.create);
                public bool HasConfig => ctx.IsStructFieldNonNull(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public string CsvData
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public Mas.Schema.Climate.CSVTimeSeriesFactory.CSVConfig.WRITER Config
                {
                    get => BuildPointer<Mas.Schema.Climate.CSVTimeSeriesFactory.CSVConfig.WRITER>(1);
                    set => Link(1, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xefefafebc8ae5534UL)]
        public class Result_Create : ICapnpSerializable
        {
            public const UInt64 typeId = 0xefefafebc8ae5534UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Timeseries = reader.Timeseries;
                Error = reader.Error;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Timeseries = Timeseries;
                writer.Error = Error;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Climate.ITimeSeries Timeseries
            {
                get;
                set;
            }

            public string Error
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
                public Mas.Schema.Climate.ITimeSeries Timeseries => ctx.ReadCap<Mas.Schema.Climate.ITimeSeries>(0);
                public string Error => ctx.ReadText(1, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public Mas.Schema.Climate.ITimeSeries Timeseries
                {
                    get => ReadCap<Mas.Schema.Climate.ITimeSeries>(0);
                    set => LinkObject(0, value);
                }

                public string Error
                {
                    get => this.ReadText(1, null);
                    set => this.WriteText(1, value, null);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe1f480ef979784b2UL), Proxy(typeof(AlterTimeSeriesWrapper_Proxy)), Skeleton(typeof(AlterTimeSeriesWrapper_Skeleton))]
    public interface IAlterTimeSeriesWrapper : Mas.Schema.Climate.ITimeSeries
    {
        Task<Mas.Schema.Climate.ITimeSeries> WrappedTimeSeries(CancellationToken cancellationToken_ = default);
        Task<IReadOnlyList<Mas.Schema.Climate.AlterTimeSeriesWrapper.Altered>> AlteredElements(CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Climate.ITimeSeries> Alter(Mas.Schema.Climate.AlterTimeSeriesWrapper.Altered desc, bool asNewTimeSeries, CancellationToken cancellationToken_ = default);
        Task Remove(Mas.Schema.Climate.Element alteredElement, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe1f480ef979784b2UL)]
    public class AlterTimeSeriesWrapper_Proxy : Proxy, IAlterTimeSeriesWrapper
    {
        public Task<Mas.Schema.Climate.ITimeSeries> WrappedTimeSeries(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.AlterTimeSeriesWrapper.Params_WrappedTimeSeries.WRITER>();
            var arg_ = new Mas.Schema.Climate.AlterTimeSeriesWrapper.Params_WrappedTimeSeries()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(16281780319380014258UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.AlterTimeSeriesWrapper.Result_WrappedTimeSeries>(d_);
                    return (r_.TimeSeries);
                }
            }

            );
        }

        public async Task<IReadOnlyList<Mas.Schema.Climate.AlterTimeSeriesWrapper.Altered>> AlteredElements(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.AlterTimeSeriesWrapper.Params_AlteredElements.WRITER>();
            var arg_ = new Mas.Schema.Climate.AlterTimeSeriesWrapper.Params_AlteredElements()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(16281780319380014258UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Climate.AlterTimeSeriesWrapper.Result_AlteredElements>(d_);
                return (r_.List);
            }
        }

        public Task<Mas.Schema.Climate.ITimeSeries> Alter(Mas.Schema.Climate.AlterTimeSeriesWrapper.Altered desc, bool asNewTimeSeries, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.AlterTimeSeriesWrapper.Params_Alter.WRITER>();
            var arg_ = new Mas.Schema.Climate.AlterTimeSeriesWrapper.Params_Alter()
            {Desc = desc, AsNewTimeSeries = asNewTimeSeries};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(16281780319380014258UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.AlterTimeSeriesWrapper.Result_Alter>(d_);
                    return (r_.TimeSeries);
                }
            }

            );
        }

        public async Task Remove(Mas.Schema.Climate.Element alteredElement, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.AlterTimeSeriesWrapper.Params_Remove.WRITER>();
            var arg_ = new Mas.Schema.Climate.AlterTimeSeriesWrapper.Params_Remove()
            {AlteredElement = alteredElement};
            arg_?.serialize(in_);
            using (var d_ = await Call(16281780319380014258UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Climate.AlterTimeSeriesWrapper.Result_Remove>(d_);
                return;
            }
        }

        public async Task<Mas.Schema.Climate.TimeSeries.Resolution> Resolution(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Params_Resolution.WRITER>();
            var arg_ = new Mas.Schema.Climate.TimeSeries.Params_Resolution()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(12067007353081196008UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Climate.TimeSeries.Result_Resolution>(d_);
                return (r_.Resolution);
            }
        }

        public async Task<(Mas.Schema.Common.Date, Mas.Schema.Common.Date)> Range(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Params_Range.WRITER>();
            var arg_ = new Mas.Schema.Climate.TimeSeries.Params_Range()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(12067007353081196008UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Climate.TimeSeries.Result_Range>(d_);
                return (r_.StartDate, r_.EndDate);
            }
        }

        public async Task<IReadOnlyList<Mas.Schema.Climate.Element>> Header(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Params_Header.WRITER>();
            var arg_ = new Mas.Schema.Climate.TimeSeries.Params_Header()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(12067007353081196008UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Climate.TimeSeries.Result_Header>(d_);
                return (r_.Header);
            }
        }

        public async Task<IReadOnlyList<IReadOnlyList<float>>> Data(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Params_Data.WRITER>();
            var arg_ = new Mas.Schema.Climate.TimeSeries.Params_Data()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(12067007353081196008UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Climate.TimeSeries.Result_Data>(d_);
                return (r_.Data);
            }
        }

        public async Task<IReadOnlyList<IReadOnlyList<float>>> DataT(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Params_DataT.WRITER>();
            var arg_ = new Mas.Schema.Climate.TimeSeries.Params_DataT()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(12067007353081196008UL, 4, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Climate.TimeSeries.Result_DataT>(d_);
                return (r_.Data);
            }
        }

        public Task<Mas.Schema.Climate.ITimeSeries> Subrange(Mas.Schema.Common.Date start, Mas.Schema.Common.Date end, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Params_Subrange.WRITER>();
            var arg_ = new Mas.Schema.Climate.TimeSeries.Params_Subrange()
            {Start = start, End = end};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(12067007353081196008UL, 5, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.TimeSeries.Result_Subrange>(d_);
                    return (r_.TimeSeries);
                }
            }

            );
        }

        public Task<Mas.Schema.Climate.ITimeSeries> Subheader(IReadOnlyList<Mas.Schema.Climate.Element> elements, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Params_Subheader.WRITER>();
            var arg_ = new Mas.Schema.Climate.TimeSeries.Params_Subheader()
            {Elements = elements};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(12067007353081196008UL, 6, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.TimeSeries.Result_Subheader>(d_);
                    return (r_.TimeSeries);
                }
            }

            );
        }

        public Task<Mas.Schema.Climate.Metadata> Metadata(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Params_Metadata.WRITER>();
            var arg_ = new Mas.Schema.Climate.TimeSeries.Params_Metadata()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(12067007353081196008UL, 7, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.Metadata>(d_);
                    return r_;
                }
            }

            );
        }

        public Task<Mas.Schema.Climate.Location> Location(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.TimeSeries.Params_Location.WRITER>();
            var arg_ = new Mas.Schema.Climate.TimeSeries.Params_Location()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(12067007353081196008UL, 8, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.Location>(d_);
                    return r_;
                }
            }

            );
        }

        public async Task<Mas.Schema.Persistence.Persistent.SaveResults> Save(Mas.Schema.Persistence.Persistent.SaveParams arg_, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.Persistent.SaveParams.WRITER>();
            arg_?.serialize(in_);
            using (var d_ = await Call(13954362354854972261UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Persistence.Persistent.SaveResults>(d_);
                return r_;
            }
        }

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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe1f480ef979784b2UL)]
    public class AlterTimeSeriesWrapper_Skeleton : Skeleton<IAlterTimeSeriesWrapper>
    {
        public AlterTimeSeriesWrapper_Skeleton()
        {
            SetMethodTable(WrappedTimeSeries, AlteredElements, Alter, Remove);
        }

        public override ulong InterfaceId => 16281780319380014258UL;
        Task<AnswerOrCounterquestion> WrappedTimeSeries(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.WrappedTimeSeries(cancellationToken_), timeSeries =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.AlterTimeSeriesWrapper.Result_WrappedTimeSeries.WRITER>();
                    var r_ = new Mas.Schema.Climate.AlterTimeSeriesWrapper.Result_WrappedTimeSeries{TimeSeries = timeSeries};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> AlteredElements(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.AlteredElements(cancellationToken_), list =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.AlterTimeSeriesWrapper.Result_AlteredElements.WRITER>();
                    var r_ = new Mas.Schema.Climate.AlterTimeSeriesWrapper.Result_AlteredElements{List = list};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> Alter(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Climate.AlterTimeSeriesWrapper.Params_Alter>(d_);
                return Impatient.MaybeTailCall(Impl.Alter(in_.Desc, in_.AsNewTimeSeries, cancellationToken_), timeSeries =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.AlterTimeSeriesWrapper.Result_Alter.WRITER>();
                    var r_ = new Mas.Schema.Climate.AlterTimeSeriesWrapper.Result_Alter{TimeSeries = timeSeries};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        async Task<AnswerOrCounterquestion> Remove(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Climate.AlterTimeSeriesWrapper.Params_Remove>(d_);
                await Impl.Remove(in_.AlteredElement, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.AlterTimeSeriesWrapper.Result_Remove.WRITER>();
                return s_;
            }
        }
    }

    public static class AlterTimeSeriesWrapper
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd085b9baf390bec5UL)]
        public class Altered : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd085b9baf390bec5UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Element = reader.Element;
                Value = reader.Value;
                Type = reader.Type;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Element = Element;
                writer.Value = Value;
                writer.Type = Type;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Climate.Element Element
            {
                get;
                set;
            }

            public float Value
            {
                get;
                set;
            }

            public Mas.Schema.Climate.AlterTimeSeriesWrapper.AlterType Type
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
                public Mas.Schema.Climate.Element Element => (Mas.Schema.Climate.Element)ctx.ReadDataUShort(0UL, (ushort)0);
                public float Value => ctx.ReadDataFloat(32UL, 0F);
                public Mas.Schema.Climate.AlterTimeSeriesWrapper.AlterType Type => (Mas.Schema.Climate.AlterTimeSeriesWrapper.AlterType)ctx.ReadDataUShort(16UL, (ushort)0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public Mas.Schema.Climate.Element Element
                {
                    get => (Mas.Schema.Climate.Element)this.ReadDataUShort(0UL, (ushort)0);
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }

                public float Value
                {
                    get => this.ReadDataFloat(32UL, 0F);
                    set => this.WriteData(32UL, value, 0F);
                }

                public Mas.Schema.Climate.AlterTimeSeriesWrapper.AlterType Type
                {
                    get => (Mas.Schema.Climate.AlterTimeSeriesWrapper.AlterType)this.ReadDataUShort(16UL, (ushort)0);
                    set => this.WriteData(16UL, (ushort)value, (ushort)0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb5dd785107c358caUL)]
        public enum AlterType : ushort
        {
            @add,
            mul
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe31f26eed9fb36a9UL)]
        public class Params_WrappedTimeSeries : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe31f26eed9fb36a9UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfb2eddb58f90f7aaUL)]
        public class Result_WrappedTimeSeries : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfb2eddb58f90f7aaUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                TimeSeries = reader.TimeSeries;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.TimeSeries = TimeSeries;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Climate.ITimeSeries TimeSeries
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
                public Mas.Schema.Climate.ITimeSeries TimeSeries => ctx.ReadCap<Mas.Schema.Climate.ITimeSeries>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Climate.ITimeSeries TimeSeries
                {
                    get => ReadCap<Mas.Schema.Climate.ITimeSeries>(0);
                    set => LinkObject(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcba0220cda41869eUL)]
        public class Params_AlteredElements : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcba0220cda41869eUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdd5b75b5bc711766UL)]
        public class Result_AlteredElements : ICapnpSerializable
        {
            public const UInt64 typeId = 0xdd5b75b5bc711766UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                List = reader.List?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Climate.AlterTimeSeriesWrapper.Altered>(_));
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

            public IReadOnlyList<Mas.Schema.Climate.AlterTimeSeriesWrapper.Altered> List
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
                public IReadOnlyList<Mas.Schema.Climate.AlterTimeSeriesWrapper.Altered.READER> List => ctx.ReadList(0).Cast(Mas.Schema.Climate.AlterTimeSeriesWrapper.Altered.READER.create);
                public bool HasList => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfStructsSerializer<Mas.Schema.Climate.AlterTimeSeriesWrapper.Altered.WRITER> List
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Climate.AlterTimeSeriesWrapper.Altered.WRITER>>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd36b1e9c2929e6e4UL)]
        public class Params_Alter : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd36b1e9c2929e6e4UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Desc = CapnpSerializable.Create<Mas.Schema.Climate.AlterTimeSeriesWrapper.Altered>(reader.Desc);
                AsNewTimeSeries = reader.AsNewTimeSeries;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Desc?.serialize(writer.Desc);
                writer.AsNewTimeSeries = AsNewTimeSeries;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Climate.AlterTimeSeriesWrapper.Altered Desc
            {
                get;
                set;
            }

            public bool AsNewTimeSeries
            {
                get;
                set;
            }

            = false;
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
                public Mas.Schema.Climate.AlterTimeSeriesWrapper.Altered.READER Desc => ctx.ReadStruct(0, Mas.Schema.Climate.AlterTimeSeriesWrapper.Altered.READER.create);
                public bool HasDesc => ctx.IsStructFieldNonNull(0);
                public bool AsNewTimeSeries => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public Mas.Schema.Climate.AlterTimeSeriesWrapper.Altered.WRITER Desc
                {
                    get => BuildPointer<Mas.Schema.Climate.AlterTimeSeriesWrapper.Altered.WRITER>(0);
                    set => Link(0, value);
                }

                public bool AsNewTimeSeries
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc4a1ec6280be841cUL)]
        public class Result_Alter : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc4a1ec6280be841cUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                TimeSeries = reader.TimeSeries;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.TimeSeries = TimeSeries;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Climate.ITimeSeries TimeSeries
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
                public Mas.Schema.Climate.ITimeSeries TimeSeries => ctx.ReadCap<Mas.Schema.Climate.ITimeSeries>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Climate.ITimeSeries TimeSeries
                {
                    get => ReadCap<Mas.Schema.Climate.ITimeSeries>(0);
                    set => LinkObject(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdb7bfcfe4d45ff53UL)]
        public class Params_Remove : ICapnpSerializable
        {
            public const UInt64 typeId = 0xdb7bfcfe4d45ff53UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                AlteredElement = reader.AlteredElement;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.AlteredElement = AlteredElement;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Climate.Element AlteredElement
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
                public Mas.Schema.Climate.Element AlteredElement => (Mas.Schema.Climate.Element)ctx.ReadDataUShort(0UL, (ushort)0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public Mas.Schema.Climate.Element AlteredElement
                {
                    get => (Mas.Schema.Climate.Element)this.ReadDataUShort(0UL, (ushort)0);
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf44980b23013003bUL)]
        public class Result_Remove : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf44980b23013003bUL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc5f12df0a2a52744UL), Proxy(typeof(AlterTimeSeriesWrapperFactory_Proxy)), Skeleton(typeof(AlterTimeSeriesWrapperFactory_Skeleton))]
    public interface IAlterTimeSeriesWrapperFactory : Mas.Schema.Common.IIdentifiable
    {
        Task<Mas.Schema.Climate.IAlterTimeSeriesWrapper> Wrap(Mas.Schema.Climate.ITimeSeries timeSeries, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc5f12df0a2a52744UL)]
    public class AlterTimeSeriesWrapperFactory_Proxy : Proxy, IAlterTimeSeriesWrapperFactory
    {
        public Task<Mas.Schema.Climate.IAlterTimeSeriesWrapper> Wrap(Mas.Schema.Climate.ITimeSeries timeSeries, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Climate.AlterTimeSeriesWrapperFactory.Params_Wrap.WRITER>();
            var arg_ = new Mas.Schema.Climate.AlterTimeSeriesWrapperFactory.Params_Wrap()
            {TimeSeries = timeSeries};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(14263232006403204932UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Climate.AlterTimeSeriesWrapperFactory.Result_Wrap>(d_);
                    return (r_.Wrapper);
                }
            }

            );
        }

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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc5f12df0a2a52744UL)]
    public class AlterTimeSeriesWrapperFactory_Skeleton : Skeleton<IAlterTimeSeriesWrapperFactory>
    {
        public AlterTimeSeriesWrapperFactory_Skeleton()
        {
            SetMethodTable(Wrap);
        }

        public override ulong InterfaceId => 14263232006403204932UL;
        Task<AnswerOrCounterquestion> Wrap(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Climate.AlterTimeSeriesWrapperFactory.Params_Wrap>(d_);
                return Impatient.MaybeTailCall(Impl.Wrap(in_.TimeSeries, cancellationToken_), wrapper =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Climate.AlterTimeSeriesWrapperFactory.Result_Wrap.WRITER>();
                    var r_ = new Mas.Schema.Climate.AlterTimeSeriesWrapperFactory.Result_Wrap{Wrapper = wrapper};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class AlterTimeSeriesWrapperFactory
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x95064806dc018bfeUL)]
        public class Params_Wrap : ICapnpSerializable
        {
            public const UInt64 typeId = 0x95064806dc018bfeUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                TimeSeries = reader.TimeSeries;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.TimeSeries = TimeSeries;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Climate.ITimeSeries TimeSeries
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
                public Mas.Schema.Climate.ITimeSeries TimeSeries => ctx.ReadCap<Mas.Schema.Climate.ITimeSeries>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Climate.ITimeSeries TimeSeries
                {
                    get => ReadCap<Mas.Schema.Climate.ITimeSeries>(0);
                    set => LinkObject(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb48982ac9bcd5d11UL)]
        public class Result_Wrap : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb48982ac9bcd5d11UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Wrapper = reader.Wrapper;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Wrapper = Wrapper;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Climate.IAlterTimeSeriesWrapper Wrapper
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
                public Mas.Schema.Climate.IAlterTimeSeriesWrapper Wrapper => ctx.ReadCap<Mas.Schema.Climate.IAlterTimeSeriesWrapper>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Climate.IAlterTimeSeriesWrapper Wrapper
                {
                    get => ReadCap<Mas.Schema.Climate.IAlterTimeSeriesWrapper>(0);
                    set => LinkObject(0, value);
                }
            }
        }
    }

    public static partial class PipeliningSupportExtensions_climate
    {
        static readonly MemberAccessPath Path_mas_schema_climate_Dataset_metadata_Info = new MemberAccessPath(1U);
        public static Mas.Schema.Climate.Metadata.IInformation Info(this Task<Mas.Schema.Climate.Metadata> task)
        {
            async Task<IDisposable> AwaitProxy() => (await task).Info;
            return (Mas.Schema.Climate.Metadata.IInformation)CapabilityReflection.CreateProxy<Mas.Schema.Climate.Metadata.IInformation>(Impatient.Access(task, Path_mas_schema_climate_Dataset_metadata_Info, AwaitProxy()));
        }

        static readonly MemberAccessPath Path_mas_schema_climate_TimeSeries_location_TimeSeries = new MemberAccessPath(2U);
        public static Mas.Schema.Climate.ITimeSeries TimeSeries(this Task<Mas.Schema.Climate.Location> task)
        {
            async Task<IDisposable> AwaitProxy() => (await task).TimeSeries;
            return (Mas.Schema.Climate.ITimeSeries)CapabilityReflection.CreateProxy<Mas.Schema.Climate.ITimeSeries>(Impatient.Access(task, Path_mas_schema_climate_TimeSeries_location_TimeSeries, AwaitProxy()));
        }
    }
}