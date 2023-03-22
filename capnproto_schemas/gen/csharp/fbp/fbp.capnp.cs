using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Schema.Fbp
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaf0a1dc4709a5ccfUL)]
    public class IP : ICapnpSerializable
    {
        public const UInt64 typeId = 0xaf0a1dc4709a5ccfUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Attributes = reader.Attributes?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Fbp.IP.KV>(_));
            Content = CapnpSerializable.Create<object>(reader.Content);
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Attributes.Init(Attributes, (_s1, _v1) => _v1?.serialize(_s1));
            writer.Content.SetObject(Content);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public IReadOnlyList<Mas.Schema.Fbp.IP.KV> Attributes
        {
            get;
            set;
        }

        public object Content
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
            public IReadOnlyList<Mas.Schema.Fbp.IP.KV.READER> Attributes => ctx.ReadList(0).Cast(Mas.Schema.Fbp.IP.KV.READER.create);
            public bool HasAttributes => ctx.IsStructFieldNonNull(0);
            public DeserializerState Content => ctx.StructReadPointer(1);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 2);
            }

            public ListOfStructsSerializer<Mas.Schema.Fbp.IP.KV.WRITER> Attributes
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Fbp.IP.KV.WRITER>>(0);
                set => Link(0, value);
            }

            public DynamicSerializerState Content
            {
                get => BuildPointer<DynamicSerializerState>(1);
                set => Link(1, value);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9e9e5391e0c499e6UL)]
        public class KV : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9e9e5391e0c499e6UL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9c62c32b2ff2b1e8UL), Proxy(typeof(Channel_Proxy<>)), Skeleton(typeof(Channel_Skeleton<>))]
    public interface IChannel<TV> : Mas.Schema.Common.IIdentifiable, Mas.Schema.Persistence.IPersistent where TV : class
    {
        Task SetBufferSize(ulong size, CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Fbp.Channel<TV>.IReader> Reader(CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Fbp.Channel<TV>.IWriter> Writer(CancellationToken cancellationToken_ = default);
        Task<(Mas.Schema.Fbp.Channel<TV>.IReader, Mas.Schema.Fbp.Channel<TV>.IWriter)> Endpoints(CancellationToken cancellationToken_ = default);
        Task SetAutoCloseSemantics(Mas.Schema.Fbp.Channel<TV>.CloseSemantics cs, CancellationToken cancellationToken_ = default);
        Task Close(bool waitForEmptyBuffer, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9c62c32b2ff2b1e8UL)]
    public class Channel_Proxy<TV> : Proxy, IChannel<TV> where TV : class
    {
        public async Task SetBufferSize(ulong size, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Channel<TV>.Params_SetBufferSize.WRITER>();
            var arg_ = new Mas.Schema.Fbp.Channel<TV>.Params_SetBufferSize()
            {Size = size};
            arg_?.serialize(in_);
            using (var d_ = await Call(11268783807889846760UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Fbp.Channel<TV>.Result_SetBufferSize>(d_);
                return;
            }
        }

        public Task<Mas.Schema.Fbp.Channel<TV>.IReader> Reader(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Channel<TV>.Params_Reader.WRITER>();
            var arg_ = new Mas.Schema.Fbp.Channel<TV>.Params_Reader()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(11268783807889846760UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Fbp.Channel<TV>.Result_Reader>(d_);
                    return (r_.R);
                }
            }

            );
        }

        public Task<Mas.Schema.Fbp.Channel<TV>.IWriter> Writer(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Channel<TV>.Params_Writer.WRITER>();
            var arg_ = new Mas.Schema.Fbp.Channel<TV>.Params_Writer()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(11268783807889846760UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Fbp.Channel<TV>.Result_Writer>(d_);
                    return (r_.W);
                }
            }

            );
        }

        public Task<(Mas.Schema.Fbp.Channel<TV>.IReader, Mas.Schema.Fbp.Channel<TV>.IWriter)> Endpoints(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Channel<TV>.Params_Endpoints.WRITER>();
            var arg_ = new Mas.Schema.Fbp.Channel<TV>.Params_Endpoints()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(11268783807889846760UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Fbp.Channel<TV>.Result_Endpoints>(d_);
                    return (r_.R, r_.W);
                }
            }

            );
        }

        public async Task SetAutoCloseSemantics(Mas.Schema.Fbp.Channel<TV>.CloseSemantics cs, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Channel<TV>.Params_SetAutoCloseSemantics.WRITER>();
            var arg_ = new Mas.Schema.Fbp.Channel<TV>.Params_SetAutoCloseSemantics()
            {Cs = cs};
            arg_?.serialize(in_);
            using (var d_ = await Call(11268783807889846760UL, 4, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Fbp.Channel<TV>.Result_SetAutoCloseSemantics>(d_);
                return;
            }
        }

        public async Task Close(bool waitForEmptyBuffer, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Channel<TV>.Params_Close.WRITER>();
            var arg_ = new Mas.Schema.Fbp.Channel<TV>.Params_Close()
            {WaitForEmptyBuffer = waitForEmptyBuffer};
            arg_?.serialize(in_);
            using (var d_ = await Call(11268783807889846760UL, 5, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Fbp.Channel<TV>.Result_Close>(d_);
                return;
            }
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9c62c32b2ff2b1e8UL)]
    public class Channel_Skeleton<TV> : Skeleton<IChannel<TV>> where TV : class
    {
        public Channel_Skeleton()
        {
            SetMethodTable(SetBufferSize, Reader, Writer, Endpoints, SetAutoCloseSemantics, Close);
        }

        public override ulong InterfaceId => 11268783807889846760UL;
        async Task<AnswerOrCounterquestion> SetBufferSize(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Fbp.Channel<TV>.Params_SetBufferSize>(d_);
                await Impl.SetBufferSize(in_.Size, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Channel<TV>.Result_SetBufferSize.WRITER>();
                return s_;
            }
        }

        Task<AnswerOrCounterquestion> Reader(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Reader(cancellationToken_), r =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Channel<TV>.Result_Reader.WRITER>();
                    var r_ = new Mas.Schema.Fbp.Channel<TV>.Result_Reader{R = r};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> Writer(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Writer(cancellationToken_), w =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Channel<TV>.Result_Writer.WRITER>();
                    var r_ = new Mas.Schema.Fbp.Channel<TV>.Result_Writer{W = w};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> Endpoints(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Endpoints(cancellationToken_), (r, w) =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Channel<TV>.Result_Endpoints.WRITER>();
                    var r_ = new Mas.Schema.Fbp.Channel<TV>.Result_Endpoints{R = r, W = w};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        async Task<AnswerOrCounterquestion> SetAutoCloseSemantics(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Fbp.Channel<TV>.Params_SetAutoCloseSemantics>(d_);
                await Impl.SetAutoCloseSemantics(in_.Cs, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Channel<TV>.Result_SetAutoCloseSemantics.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> Close(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Fbp.Channel<TV>.Params_Close>(d_);
                await Impl.Close(in_.WaitForEmptyBuffer, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Channel<TV>.Result_Close.WRITER>();
                return s_;
            }
        }
    }

    public static class Channel<TV>
        where TV : class
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa8d787cae7e0b243UL)]
        public enum CloseSemantics : ushort
        {
            fbp,
            no
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd5b512f4bcd0aa2eUL)]
        public class Msg : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd5b512f4bcd0aa2eUL;
            public enum WHICH : ushort
            {
                Value = 0,
                Done = 1,
                undefined = 65535
            }

            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                switch (reader.which)
                {
                    case WHICH.Value:
                        Value = CapnpSerializable.Create<TV>(reader.Value);
                        break;
                    case WHICH.Done:
                        which = reader.which;
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
                        case WHICH.Value:
                            _content = null;
                            break;
                        case WHICH.Done:
                            break;
                    }
                }
            }

            public void serialize(WRITER writer)
            {
                writer.which = which;
                switch (which)
                {
                    case WHICH.Value:
                        writer.Value.SetObject(Value);
                        break;
                    case WHICH.Done:
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

            public TV Value
            {
                get => _which == WHICH.Value ? (TV)_content : null;
                set
                {
                    _which = WHICH.Value;
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
                public DeserializerState Value => which == WHICH.Value ? ctx.StructReadPointer(0) : default;
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

                public DynamicSerializerState Value
                {
                    get => which == WHICH.Value ? BuildPointer<DynamicSerializerState>(0) : default;
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe3d7a3237f175028UL)]
        public class StartupInfo : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe3d7a3237f175028UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                BufferSize = reader.BufferSize;
                CloseSemantics = reader.CloseSemantics;
                ChannelSR = reader.ChannelSR;
                ReaderSRs = reader.ReaderSRs;
                WriterSRs = reader.WriterSRs;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.BufferSize = BufferSize;
                writer.CloseSemantics = CloseSemantics;
                writer.ChannelSR = ChannelSR;
                writer.ReaderSRs.Init(ReaderSRs);
                writer.WriterSRs.Init(WriterSRs);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public ulong BufferSize
            {
                get;
                set;
            }

            public Mas.Schema.Fbp.Channel<TV>.CloseSemantics CloseSemantics
            {
                get;
                set;
            }

            public string ChannelSR
            {
                get;
                set;
            }

            public IReadOnlyList<string> ReaderSRs
            {
                get;
                set;
            }

            public IReadOnlyList<string> WriterSRs
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
                public ulong BufferSize => ctx.ReadDataULong(0UL, 0UL);
                public Mas.Schema.Fbp.Channel<TV>.CloseSemantics CloseSemantics => (Mas.Schema.Fbp.Channel<TV>.CloseSemantics)ctx.ReadDataUShort(64UL, (ushort)0);
                public string ChannelSR => ctx.ReadText(0, null);
                public IReadOnlyList<string> ReaderSRs => ctx.ReadList(1).CastText2();
                public bool HasReaderSRs => ctx.IsStructFieldNonNull(1);
                public IReadOnlyList<string> WriterSRs => ctx.ReadList(2).CastText2();
                public bool HasWriterSRs => ctx.IsStructFieldNonNull(2);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(2, 3);
                }

                public ulong BufferSize
                {
                    get => this.ReadDataULong(0UL, 0UL);
                    set => this.WriteData(0UL, value, 0UL);
                }

                public Mas.Schema.Fbp.Channel<TV>.CloseSemantics CloseSemantics
                {
                    get => (Mas.Schema.Fbp.Channel<TV>.CloseSemantics)this.ReadDataUShort(64UL, (ushort)0);
                    set => this.WriteData(64UL, (ushort)value, (ushort)0);
                }

                public string ChannelSR
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public ListOfTextSerializer ReaderSRs
                {
                    get => BuildPointer<ListOfTextSerializer>(1);
                    set => Link(1, value);
                }

                public ListOfTextSerializer WriterSRs
                {
                    get => BuildPointer<ListOfTextSerializer>(2);
                    set => Link(2, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8bc69192f3bc97ccUL), Proxy(typeof(Reader_Proxy)), Skeleton(typeof(Reader_Skeleton))]
        public interface IReader : IDisposable
        {
            Task<Mas.Schema.Fbp.Channel<TV>.Msg> Read(CancellationToken cancellationToken_ = default);
            Task Close(CancellationToken cancellationToken_ = default);
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8bc69192f3bc97ccUL)]
        public class Reader_Proxy : Proxy, IReader
        {
            public Task<Mas.Schema.Fbp.Channel<TV>.Msg> Read(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Channel<TV>.Reader.Params_Read.WRITER>();
                var arg_ = new Mas.Schema.Fbp.Channel<TV>.Reader.Params_Read()
                {};
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(10071897677001168844UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Fbp.Channel<TV>.Msg>(d_);
                        return r_;
                    }
                }

                );
            }

            public async Task Close(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Channel<TV>.Reader.Params_Close.WRITER>();
                var arg_ = new Mas.Schema.Fbp.Channel<TV>.Reader.Params_Close()
                {};
                arg_?.serialize(in_);
                using (var d_ = await Call(10071897677001168844UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Fbp.Channel<TV>.Reader.Result_Close>(d_);
                    return;
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8bc69192f3bc97ccUL)]
        public class Reader_Skeleton : Skeleton<IReader>
        {
            public Reader_Skeleton()
            {
                SetMethodTable(Read, Close);
            }

            public override ulong InterfaceId => 10071897677001168844UL;
            Task<AnswerOrCounterquestion> Read(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    return Impatient.MaybeTailCall(Impl.Read(cancellationToken_), r_ =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Channel<TV>.Msg.WRITER>();
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            async Task<AnswerOrCounterquestion> Close(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    await Impl.Close(cancellationToken_);
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Channel<TV>.Reader.Result_Close.WRITER>();
                    return s_;
                }
            }
        }

        public static class Reader
        {
            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc0335d99db8b2ba5UL)]
            public class Params_Read : ICapnpSerializable
            {
                public const UInt64 typeId = 0xc0335d99db8b2ba5UL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9428ea64f18c41c8UL)]
            public class Params_Close : ICapnpSerializable
            {
                public const UInt64 typeId = 0x9428ea64f18c41c8UL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb3fe08a1bf53821aUL)]
            public class Result_Close : ICapnpSerializable
            {
                public const UInt64 typeId = 0xb3fe08a1bf53821aUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf7fec613b4a8c79fUL), Proxy(typeof(Writer_Proxy)), Skeleton(typeof(Writer_Skeleton))]
        public interface IWriter : IDisposable
        {
            Task Write(Mas.Schema.Fbp.Channel<TV>.Msg arg_, CancellationToken cancellationToken_ = default);
            Task Close(CancellationToken cancellationToken_ = default);
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf7fec613b4a8c79fUL)]
        public class Writer_Proxy : Proxy, IWriter
        {
            public async Task Write(Mas.Schema.Fbp.Channel<TV>.Msg arg_, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Channel<TV>.Msg.WRITER>();
                arg_?.serialize(in_);
                using (var d_ = await Call(17869938159390345119UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Fbp.Channel<TV>.Writer.Result_Write>(d_);
                    return;
                }
            }

            public async Task Close(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Channel<TV>.Writer.Params_Close.WRITER>();
                var arg_ = new Mas.Schema.Fbp.Channel<TV>.Writer.Params_Close()
                {};
                arg_?.serialize(in_);
                using (var d_ = await Call(17869938159390345119UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Fbp.Channel<TV>.Writer.Result_Close>(d_);
                    return;
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf7fec613b4a8c79fUL)]
        public class Writer_Skeleton : Skeleton<IWriter>
        {
            public Writer_Skeleton()
            {
                SetMethodTable(Write, Close);
            }

            public override ulong InterfaceId => 17869938159390345119UL;
            async Task<AnswerOrCounterquestion> Write(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    await Impl.Write(CapnpSerializable.Create<Mas.Schema.Fbp.Channel<TV>.Msg>(d_), cancellationToken_);
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Channel<TV>.Writer.Result_Write.WRITER>();
                    return s_;
                }
            }

            async Task<AnswerOrCounterquestion> Close(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    await Impl.Close(cancellationToken_);
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Channel<TV>.Writer.Result_Close.WRITER>();
                    return s_;
                }
            }
        }

        public static class Writer
        {
            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xce9f24b8ec149524UL)]
            public class Result_Write : ICapnpSerializable
            {
                public const UInt64 typeId = 0xce9f24b8ec149524UL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbadc988dda3d1e50UL)]
            public class Params_Close : ICapnpSerializable
            {
                public const UInt64 typeId = 0xbadc988dda3d1e50UL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcb02dc91e18e58c9UL)]
            public class Result_Close : ICapnpSerializable
            {
                public const UInt64 typeId = 0xcb02dc91e18e58c9UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x92101e3b7a761333UL)]
        public class Params_SetBufferSize : ICapnpSerializable
        {
            public const UInt64 typeId = 0x92101e3b7a761333UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Size = reader.Size;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Size = Size;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public ulong Size
            {
                get;
                set;
            }

            = 1UL;
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
                public ulong Size => ctx.ReadDataULong(0UL, 1UL);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public ulong Size
                {
                    get => this.ReadDataULong(0UL, 1UL);
                    set => this.WriteData(0UL, value, 1UL);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfe6a08d5e0712c23UL)]
        public class Result_SetBufferSize : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfe6a08d5e0712c23UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe607c9dd64da04c4UL)]
        public class Params_Reader : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe607c9dd64da04c4UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb135ffc9ccc9eca6UL)]
        public class Result_Reader : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb135ffc9ccc9eca6UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                R = reader.R;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.R = R;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Fbp.Channel<TV>.IReader R
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
                public Mas.Schema.Fbp.Channel<TV>.IReader R => ctx.ReadCap<Mas.Schema.Fbp.Channel<TV>.IReader>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Fbp.Channel<TV>.IReader R
                {
                    get => ReadCap<Mas.Schema.Fbp.Channel<TV>.IReader>(0);
                    set => LinkObject(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbe611d34e368e109UL)]
        public class Params_Writer : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbe611d34e368e109UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb47b53679e985c7eUL)]
        public class Result_Writer : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb47b53679e985c7eUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                W = reader.W;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.W = W;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Fbp.Channel<TV>.IWriter W
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
                public Mas.Schema.Fbp.Channel<TV>.IWriter W => ctx.ReadCap<Mas.Schema.Fbp.Channel<TV>.IWriter>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Fbp.Channel<TV>.IWriter W
                {
                    get => ReadCap<Mas.Schema.Fbp.Channel<TV>.IWriter>(0);
                    set => LinkObject(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd23f817e914373d8UL)]
        public class Params_Endpoints : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd23f817e914373d8UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf37401d21f8d97bbUL)]
        public class Result_Endpoints : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf37401d21f8d97bbUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                R = reader.R;
                W = reader.W;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.R = R;
                writer.W = W;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Fbp.Channel<TV>.IReader R
            {
                get;
                set;
            }

            public Mas.Schema.Fbp.Channel<TV>.IWriter W
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
                public Mas.Schema.Fbp.Channel<TV>.IReader R => ctx.ReadCap<Mas.Schema.Fbp.Channel<TV>.IReader>(0);
                public Mas.Schema.Fbp.Channel<TV>.IWriter W => ctx.ReadCap<Mas.Schema.Fbp.Channel<TV>.IWriter>(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public Mas.Schema.Fbp.Channel<TV>.IReader R
                {
                    get => ReadCap<Mas.Schema.Fbp.Channel<TV>.IReader>(0);
                    set => LinkObject(0, value);
                }

                public Mas.Schema.Fbp.Channel<TV>.IWriter W
                {
                    get => ReadCap<Mas.Schema.Fbp.Channel<TV>.IWriter>(1);
                    set => LinkObject(1, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb49836b545583addUL)]
        public class Params_SetAutoCloseSemantics : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb49836b545583addUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Cs = reader.Cs;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Cs = Cs;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Fbp.Channel<TV>.CloseSemantics Cs
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
                public Mas.Schema.Fbp.Channel<TV>.CloseSemantics Cs => (Mas.Schema.Fbp.Channel<TV>.CloseSemantics)ctx.ReadDataUShort(0UL, (ushort)0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public Mas.Schema.Fbp.Channel<TV>.CloseSemantics Cs
                {
                    get => (Mas.Schema.Fbp.Channel<TV>.CloseSemantics)this.ReadDataUShort(0UL, (ushort)0);
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc0fc6e5a3fcb3206UL)]
        public class Result_SetAutoCloseSemantics : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc0fc6e5a3fcb3206UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x95d8ad01c1113d9cUL)]
        public class Params_Close : ICapnpSerializable
        {
            public const UInt64 typeId = 0x95d8ad01c1113d9cUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                WaitForEmptyBuffer = reader.WaitForEmptyBuffer;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.WaitForEmptyBuffer = WaitForEmptyBuffer;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool WaitForEmptyBuffer
            {
                get;
                set;
            }

            = true;
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
                public bool WaitForEmptyBuffer => ctx.ReadDataBool(0UL, true);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool WaitForEmptyBuffer
                {
                    get => this.ReadDataBool(0UL, true);
                    set => this.WriteData(0UL, value, true);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcc079ad60f1363b7UL)]
        public class Result_Close : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcc079ad60f1363b7UL;
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

    public static partial class PipeliningSupportExtensions_fbp
    {
        static readonly MemberAccessPath Path_mas_schema_fbp_Channel_endpoints_W = new MemberAccessPath(1U);
        public static Mas.Schema.Fbp.Channel<TV>.IWriter W(this Task<(Mas.Schema.Fbp.Channel<TV>.IReader, Mas.Schema.Fbp.Channel<TV>.IWriter)> task)
        {
            async Task<IDisposable> AwaitProxy() => (await task).Item2;
            return (Mas.Schema.Fbp.Channel<TV>.IWriter)CapabilityReflection.CreateProxy<Mas.Schema.Fbp.Channel<TV>.IWriter>(Impatient.Access(task, Path_mas_schema_fbp_Channel_endpoints_W, AwaitProxy()));
        }
    }
}