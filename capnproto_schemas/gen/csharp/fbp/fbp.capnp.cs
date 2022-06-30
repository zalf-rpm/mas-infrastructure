using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Schema.Fbp
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd717ff7d6815a6b0UL), Proxy(typeof(Component_Proxy)), Skeleton(typeof(Component_Skeleton))]
    public interface IComponent : IDisposable
    {
        Task SetupPorts(IReadOnlyList<Mas.Schema.Fbp.Component.NameToPort> inPorts, IReadOnlyList<Mas.Schema.Fbp.Component.NameToPort> outPorts, CancellationToken cancellationToken_ = default);
        Task Stop(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd717ff7d6815a6b0UL)]
    public class Component_Proxy : Proxy, IComponent
    {
        public async Task SetupPorts(IReadOnlyList<Mas.Schema.Fbp.Component.NameToPort> inPorts, IReadOnlyList<Mas.Schema.Fbp.Component.NameToPort> outPorts, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Component.Params_SetupPorts.WRITER>();
            var arg_ = new Mas.Schema.Fbp.Component.Params_SetupPorts()
            {InPorts = inPorts, OutPorts = outPorts};
            arg_?.serialize(in_);
            using (var d_ = await Call(15499137556701095600UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Fbp.Component.Result_SetupPorts>(d_);
                return;
            }
        }

        public async Task Stop(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Component.Params_Stop.WRITER>();
            var arg_ = new Mas.Schema.Fbp.Component.Params_Stop()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(15499137556701095600UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Fbp.Component.Result_Stop>(d_);
                return;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd717ff7d6815a6b0UL)]
    public class Component_Skeleton : Skeleton<IComponent>
    {
        public Component_Skeleton()
        {
            SetMethodTable(SetupPorts, Stop);
        }

        public override ulong InterfaceId => 15499137556701095600UL;
        async Task<AnswerOrCounterquestion> SetupPorts(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Fbp.Component.Params_SetupPorts>(d_);
                await Impl.SetupPorts(in_.InPorts, in_.OutPorts, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Component.Result_SetupPorts.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> Stop(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                await Impl.Stop(cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Component.Result_Stop.WRITER>();
                return s_;
            }
        }
    }

    public static class Component
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf77095186c3c4f65UL)]
        public class NameToPort : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf77095186c3c4f65UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Name = reader.Name;
                Port = reader.Port;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Name = Name;
                writer.Port = Port;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Name
            {
                get;
                set;
            }

            public BareProxy Port
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
                public string Name => ctx.ReadText(0, null);
                public BareProxy Port => ctx.ReadCap(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public string Name
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public BareProxy Port
                {
                    get => ReadCap<BareProxy>(1);
                    set => LinkObject(1, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf5b257d7fba7ed60UL)]
        public class Params_SetupPorts : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf5b257d7fba7ed60UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                InPorts = reader.InPorts?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Fbp.Component.NameToPort>(_));
                OutPorts = reader.OutPorts?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Fbp.Component.NameToPort>(_));
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.InPorts.Init(InPorts, (_s1, _v1) => _v1?.serialize(_s1));
                writer.OutPorts.Init(OutPorts, (_s1, _v1) => _v1?.serialize(_s1));
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<Mas.Schema.Fbp.Component.NameToPort> InPorts
            {
                get;
                set;
            }

            public IReadOnlyList<Mas.Schema.Fbp.Component.NameToPort> OutPorts
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
                public IReadOnlyList<Mas.Schema.Fbp.Component.NameToPort.READER> InPorts => ctx.ReadList(0).Cast(Mas.Schema.Fbp.Component.NameToPort.READER.create);
                public bool HasInPorts => ctx.IsStructFieldNonNull(0);
                public IReadOnlyList<Mas.Schema.Fbp.Component.NameToPort.READER> OutPorts => ctx.ReadList(1).Cast(Mas.Schema.Fbp.Component.NameToPort.READER.create);
                public bool HasOutPorts => ctx.IsStructFieldNonNull(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public ListOfStructsSerializer<Mas.Schema.Fbp.Component.NameToPort.WRITER> InPorts
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Fbp.Component.NameToPort.WRITER>>(0);
                    set => Link(0, value);
                }

                public ListOfStructsSerializer<Mas.Schema.Fbp.Component.NameToPort.WRITER> OutPorts
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Fbp.Component.NameToPort.WRITER>>(1);
                    set => Link(1, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xda58608ec3b1dfa6UL)]
        public class Result_SetupPorts : ICapnpSerializable
        {
            public const UInt64 typeId = 0xda58608ec3b1dfa6UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbe5bb9ba1de54674UL)]
        public class Params_Stop : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbe5bb9ba1de54674UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbe0c6a5a76e75105UL)]
        public class Result_Stop : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbe0c6a5a76e75105UL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9f6bf783c59ae53fUL), Proxy(typeof(Input_Proxy)), Skeleton(typeof(Input_Skeleton))]
    public interface IInput : IDisposable
    {
        Task Close(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9f6bf783c59ae53fUL)]
    public class Input_Proxy : Proxy, IInput
    {
        public async Task Close(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Input.Params_Close.WRITER>();
            var arg_ = new Mas.Schema.Fbp.Input.Params_Close()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(11487547419866621247UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Fbp.Input.Result_Close>(d_);
                return;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9f6bf783c59ae53fUL)]
    public class Input_Skeleton : Skeleton<IInput>
    {
        public Input_Skeleton()
        {
            SetMethodTable(Close);
        }

        public override ulong InterfaceId => 11487547419866621247UL;
        async Task<AnswerOrCounterquestion> Close(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                await Impl.Close(cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Input.Result_Close.WRITER>();
                return s_;
            }
        }
    }

    public static class Input
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd21817ccd00e3d80UL), Proxy(typeof(Reader_Proxy)), Skeleton(typeof(Reader_Skeleton))]
        public interface IReader : IDisposable
        {
            Task<object> Read(CancellationToken cancellationToken_ = default);
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd21817ccd00e3d80UL)]
        public class Reader_Proxy : Proxy, IReader
        {
            public Task<object> Read(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Input.Reader.Params_Read.WRITER>();
                var arg_ = new Mas.Schema.Fbp.Input.Reader.Params_Read()
                {};
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(15138876315837283712UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Fbp.Input.Reader.Result_Read>(d_);
                        return (r_.Value);
                    }
                }

                );
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd21817ccd00e3d80UL)]
        public class Reader_Skeleton : Skeleton<IReader>
        {
            public Reader_Skeleton()
            {
                SetMethodTable(Read);
            }

            public override ulong InterfaceId => 15138876315837283712UL;
            Task<AnswerOrCounterquestion> Read(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    return Impatient.MaybeTailCall(Impl.Read(cancellationToken_), value =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Input.Reader.Result_Read.WRITER>();
                        var r_ = new Mas.Schema.Fbp.Input.Reader.Result_Read{Value = value};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }
        }

        public static class Reader
        {
            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xad9b89c132c7c6aaUL)]
            public class Params_Read : ICapnpSerializable
            {
                public const UInt64 typeId = 0xad9b89c132c7c6aaUL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd863f5e4c1939673UL)]
            public class Result_Read : ICapnpSerializable
            {
                public const UInt64 typeId = 0xd863f5e4c1939673UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Value = CapnpSerializable.Create<object>(reader.Value);
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Value.SetObject(Value);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
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
                    public DeserializerState Value => ctx.StructReadPointer(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public DynamicSerializerState Value
                    {
                        get => BuildPointer<DynamicSerializerState>(0);
                        set => Link(0, value);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfb9b181fea82028aUL), Proxy(typeof(Writer_Proxy)), Skeleton(typeof(Writer_Skeleton))]
        public interface IWriter : IDisposable
        {
            Task Write(object value, CancellationToken cancellationToken_ = default);
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfb9b181fea82028aUL)]
        public class Writer_Proxy : Proxy, IWriter
        {
            public async Task Write(object value, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Input.Writer.Params_Write.WRITER>();
                var arg_ = new Mas.Schema.Fbp.Input.Writer.Params_Write()
                {Value = value};
                arg_?.serialize(in_);
                using (var d_ = await Call(18130111250267505290UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Fbp.Input.Writer.Result_Write>(d_);
                    return;
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfb9b181fea82028aUL)]
        public class Writer_Skeleton : Skeleton<IWriter>
        {
            public Writer_Skeleton()
            {
                SetMethodTable(Write);
            }

            public override ulong InterfaceId => 18130111250267505290UL;
            async Task<AnswerOrCounterquestion> Write(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Fbp.Input.Writer.Params_Write>(d_);
                    await Impl.Write(in_.Value, cancellationToken_);
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.Input.Writer.Result_Write.WRITER>();
                    return s_;
                }
            }
        }

        public static class Writer
        {
            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe5a47f5477f0222dUL)]
            public class Params_Write : ICapnpSerializable
            {
                public const UInt64 typeId = 0xe5a47f5477f0222dUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Value = CapnpSerializable.Create<object>(reader.Value);
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Value.SetObject(Value);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
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
                    public DeserializerState Value => ctx.StructReadPointer(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public DynamicSerializerState Value
                    {
                        get => BuildPointer<DynamicSerializerState>(0);
                        set => Link(0, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcd975c65ddfc7918UL)]
            public class Result_Write : ICapnpSerializable
            {
                public const UInt64 typeId = 0xcd975c65ddfc7918UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdf66370e686c9b09UL)]
        public class Params_Close : ICapnpSerializable
        {
            public const UInt64 typeId = 0xdf66370e686c9b09UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9d2556ab64354282UL)]
        public class Result_Close : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9d2556ab64354282UL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9dc72eab4c0686c7UL), Proxy(typeof(InputArray_Proxy)), Skeleton(typeof(InputArray_Skeleton))]
    public interface IInputArray : IDisposable
    {
        Task Send(byte at, object data, CancellationToken cancellationToken_ = default);
        Task Close(byte at, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9dc72eab4c0686c7UL)]
    public class InputArray_Proxy : Proxy, IInputArray
    {
        public async Task Send(byte at, object data, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.InputArray.Params_Send.WRITER>();
            var arg_ = new Mas.Schema.Fbp.InputArray.Params_Send()
            {At = at, Data = data};
            arg_?.serialize(in_);
            using (var d_ = await Call(11369107097569887943UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Fbp.InputArray.Result_Send>(d_);
                return;
            }
        }

        public async Task Close(byte at, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.InputArray.Params_Close.WRITER>();
            var arg_ = new Mas.Schema.Fbp.InputArray.Params_Close()
            {At = at};
            arg_?.serialize(in_);
            using (var d_ = await Call(11369107097569887943UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Fbp.InputArray.Result_Close>(d_);
                return;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9dc72eab4c0686c7UL)]
    public class InputArray_Skeleton : Skeleton<IInputArray>
    {
        public InputArray_Skeleton()
        {
            SetMethodTable(Send, Close);
        }

        public override ulong InterfaceId => 11369107097569887943UL;
        async Task<AnswerOrCounterquestion> Send(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Fbp.InputArray.Params_Send>(d_);
                await Impl.Send(in_.At, in_.Data, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.InputArray.Result_Send.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> Close(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Fbp.InputArray.Params_Close>(d_);
                await Impl.Close(in_.At, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Schema.Fbp.InputArray.Result_Close.WRITER>();
                return s_;
            }
        }
    }

    public static class InputArray
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd6f73232158ce7e9UL)]
        public class Params_Send : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd6f73232158ce7e9UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                At = reader.At;
                Data = CapnpSerializable.Create<object>(reader.Data);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.At = At;
                writer.Data.SetObject(Data);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public byte At
            {
                get;
                set;
            }

            public object Data
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
                public byte At => ctx.ReadDataByte(0UL, (byte)0);
                public DeserializerState Data => ctx.StructReadPointer(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public byte At
                {
                    get => this.ReadDataByte(0UL, (byte)0);
                    set => this.WriteData(0UL, value, (byte)0);
                }

                public DynamicSerializerState Data
                {
                    get => BuildPointer<DynamicSerializerState>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa27dca50a85ba1b3UL)]
        public class Result_Send : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa27dca50a85ba1b3UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa3ea6f6acc75c72aUL)]
        public class Params_Close : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa3ea6f6acc75c72aUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                At = reader.At;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.At = At;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public byte At
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
                public byte At => ctx.ReadDataByte(0UL, (byte)0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public byte At
                {
                    get => this.ReadDataByte(0UL, (byte)0);
                    set => this.WriteData(0UL, value, (byte)0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe8439136add88b78UL)]
        public class Result_Close : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe8439136add88b78UL;
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
}