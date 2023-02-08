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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x902904cd51bff117UL), Proxy(typeof(Callback_Proxy)), Skeleton(typeof(Callback_Skeleton))]
    public interface ICallback : IDisposable
    {
        Task Call(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x902904cd51bff117UL)]
    public class Callback_Proxy : Proxy, ICallback
    {
        public async Task Call(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.Callback.Params_Call.WRITER>();
            var arg_ = new Mas.Schema.Common.Callback.Params_Call()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(10387839295393100055UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Common.Callback.Result_Call>(d_);
                return;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x902904cd51bff117UL)]
    public class Callback_Skeleton : Skeleton<ICallback>
    {
        public Callback_Skeleton()
        {
            SetMethodTable(Call);
        }

        public override ulong InterfaceId => 10387839295393100055UL;
        async Task<AnswerOrCounterquestion> Call(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                await Impl.Call(cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.Callback.Result_Call.WRITER>();
                return s_;
            }
        }
    }

    public static class Callback
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x83b4353989cbcb47UL)]
        public class Params_Call : ICapnpSerializable
        {
            public const UInt64 typeId = 0x83b4353989cbcb47UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb16b6184cf8b8acfUL)]
        public class Result_Call : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb16b6184cf8b8acfUL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9dd4e2c3d76e4587UL), Proxy(typeof(Action_Proxy)), Skeleton(typeof(Action_Skeleton))]
    public interface IAction : Mas.Schema.Persistence.IPersistent
    {
        Task Do(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9dd4e2c3d76e4587UL)]
    public class Action_Proxy : Proxy, IAction
    {
        public async Task Do(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.Action.Params_Do.WRITER>();
            var arg_ = new Mas.Schema.Common.Action.Params_Do()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(11372964289778173319UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Common.Action.Result_Do>(d_);
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
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9dd4e2c3d76e4587UL)]
    public class Action_Skeleton : Skeleton<IAction>
    {
        public Action_Skeleton()
        {
            SetMethodTable(Do);
        }

        public override ulong InterfaceId => 11372964289778173319UL;
        async Task<AnswerOrCounterquestion> Do(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                await Impl.Do(cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.Action.Result_Do.WRITER>();
                return s_;
            }
        }
    }

    public static class Action
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf9033a317ba0d0c6UL)]
        public class Params_Do : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf9033a317ba0d0c6UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcc040dde1a99ddffUL)]
        public class Result_Do : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcc040dde1a99ddffUL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc12db9a9ae07a763UL), Proxy(typeof(Action1_Proxy)), Skeleton(typeof(Action1_Skeleton))]
    public interface IAction1 : Mas.Schema.Persistence.IPersistent
    {
        Task Do(object p, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc12db9a9ae07a763UL)]
    public class Action1_Proxy : Proxy, IAction1
    {
        public async Task Do(object p, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.Action1.Params_Do.WRITER>();
            var arg_ = new Mas.Schema.Common.Action1.Params_Do()
            {P = p};
            arg_?.serialize(in_);
            using (var d_ = await Call(13919986161692419939UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Common.Action1.Result_Do>(d_);
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
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc12db9a9ae07a763UL)]
    public class Action1_Skeleton : Skeleton<IAction1>
    {
        public Action1_Skeleton()
        {
            SetMethodTable(Do);
        }

        public override ulong InterfaceId => 13919986161692419939UL;
        async Task<AnswerOrCounterquestion> Do(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Common.Action1.Params_Do>(d_);
                await Impl.Do(in_.P, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.Action1.Result_Do.WRITER>();
                return s_;
            }
        }
    }

    public static class Action1
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa1eb17b1112501daUL)]
        public class Params_Do : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa1eb17b1112501daUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                P = CapnpSerializable.Create<object>(reader.P);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.P.SetObject(P);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public object P
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
                public DeserializerState P => ctx.StructReadPointer(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public DynamicSerializerState P
                {
                    get => BuildPointer<DynamicSerializerState>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfd65c3cd7f2f47faUL)]
        public class Result_Do : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfd65c3cd7f2f47faUL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa869f50b8c586ed9UL), Proxy(typeof(Factory_Proxy<, >)), Skeleton(typeof(Factory_Skeleton<, >))]
    public interface IFactory<TInput, TOutput> : IDisposable where TInput : class where TOutput : class
    {
        Task<TOutput> Produce(TInput @in, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa869f50b8c586ed9UL)]
    public class Factory_Proxy<TInput, TOutput> : Proxy, IFactory<TInput, TOutput> where TInput : class where TOutput : class
    {
        public Task<TOutput> Produce(TInput @in, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.Factory<TInput, TOutput>.Params_Produce.WRITER>();
            var arg_ = new Mas.Schema.Common.Factory<TInput, TOutput>.Params_Produce()
            {In = @in};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(12135500100874563289UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Common.Factory<TInput, TOutput>.Result_Produce>(d_);
                    return (r_.Out);
                }
            }

            );
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa869f50b8c586ed9UL)]
    public class Factory_Skeleton<TInput, TOutput> : Skeleton<IFactory<TInput, TOutput>> where TInput : class where TOutput : class
    {
        public Factory_Skeleton()
        {
            SetMethodTable(Produce);
        }

        public override ulong InterfaceId => 12135500100874563289UL;
        Task<AnswerOrCounterquestion> Produce(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Common.Factory<TInput, TOutput>.Params_Produce>(d_);
                return Impatient.MaybeTailCall(Impl.Produce(in_.In, cancellationToken_), @out =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.Factory<TInput, TOutput>.Result_Produce.WRITER>();
                    var r_ = new Mas.Schema.Common.Factory<TInput, TOutput>.Result_Produce{Out = @out};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class Factory<TInput, TOutput>
        where TInput : class where TOutput : class
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfd058bbd1f9508cdUL)]
        public class Params_Produce : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfd058bbd1f9508cdUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                In = CapnpSerializable.Create<TInput>(reader.In);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.In.SetObject(In);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public TInput In
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
                public DeserializerState In => ctx.StructReadPointer(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public DynamicSerializerState In
                {
                    get => BuildPointer<DynamicSerializerState>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd010f77f1bdf0522UL)]
        public class Result_Produce : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd010f77f1bdf0522UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Out = CapnpSerializable.Create<TOutput>(reader.Out);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Out.SetObject(Out);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public TOutput Out
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
                public DeserializerState Out => ctx.StructReadPointer(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public DynamicSerializerState Out
                {
                    get => BuildPointer<DynamicSerializerState>(0);
                    set => Link(0, value);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfe04fe97ba25a27eUL)]
    public class ZmqPipelineAddresses : ICapnpSerializable
    {
        public const UInt64 typeId = 0xfe04fe97ba25a27eUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Input = reader.Input;
            Output = reader.Output;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Input = Input;
            writer.Output = Output;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public string Input
        {
            get;
            set;
        }

        public string Output
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
            public string Input => ctx.ReadText(0, null);
            public string Output => ctx.ReadText(1, null);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 2);
            }

            public string Input
            {
                get => this.ReadText(0, null);
                set => this.WriteText(0, value, null);
            }

            public string Output
            {
                get => this.ReadText(1, null);
                set => this.WriteText(1, value, null);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf1c80d9ce9dfd993UL), Proxy(typeof(ValueHolder_Proxy<>)), Skeleton(typeof(ValueHolder_Skeleton<>))]
    public interface IValueHolder<TT> : Mas.Schema.Persistence.IPersistent where TT : class
    {
        Task<TT> Value(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf1c80d9ce9dfd993UL)]
    public class ValueHolder_Proxy<TT> : Proxy, IValueHolder<TT> where TT : class
    {
        public Task<TT> Value(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.ValueHolder<TT>.Params_Value.WRITER>();
            var arg_ = new Mas.Schema.Common.ValueHolder<TT>.Params_Value()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(17422190126072584595UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Common.ValueHolder<TT>.Result_Value>(d_);
                    return (r_.Val);
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
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf1c80d9ce9dfd993UL)]
    public class ValueHolder_Skeleton<TT> : Skeleton<IValueHolder<TT>> where TT : class
    {
        public ValueHolder_Skeleton()
        {
            SetMethodTable(Value);
        }

        public override ulong InterfaceId => 17422190126072584595UL;
        Task<AnswerOrCounterquestion> Value(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Value(cancellationToken_), val =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.ValueHolder<TT>.Result_Value.WRITER>();
                    var r_ = new Mas.Schema.Common.ValueHolder<TT>.Result_Value{Val = val};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class ValueHolder<TT>
        where TT : class
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfb528c3db0280a11UL)]
        public class Params_Value : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfb528c3db0280a11UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xade9d46971ea9ee3UL)]
        public class Result_Value : ICapnpSerializable
        {
            public const UInt64 typeId = 0xade9d46971ea9ee3UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Val = CapnpSerializable.Create<TT>(reader.Val);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Val.SetObject(Val);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public TT Val
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
                public DeserializerState Val => ctx.StructReadPointer(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public DynamicSerializerState Val
                {
                    get => BuildPointer<DynamicSerializerState>(0);
                    set => Link(0, value);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x89f6c5dd387cc101UL), Proxy(typeof(AnyValueHolder_Proxy)), Skeleton(typeof(AnyValueHolder_Skeleton))]
    public interface IAnyValueHolder : Mas.Schema.Persistence.IPersistent
    {
        Task<object> Value(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x89f6c5dd387cc101UL)]
    public class AnyValueHolder_Proxy : Proxy, IAnyValueHolder
    {
        public Task<object> Value(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.AnyValueHolder.Params_Value.WRITER>();
            var arg_ = new Mas.Schema.Common.AnyValueHolder.Params_Value()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(9941350781393092865UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Common.AnyValueHolder.Result_Value>(d_);
                    return (r_.Val);
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
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x89f6c5dd387cc101UL)]
    public class AnyValueHolder_Skeleton : Skeleton<IAnyValueHolder>
    {
        public AnyValueHolder_Skeleton()
        {
            SetMethodTable(Value);
        }

        public override ulong InterfaceId => 9941350781393092865UL;
        Task<AnswerOrCounterquestion> Value(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Value(cancellationToken_), val =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.AnyValueHolder.Result_Value.WRITER>();
                    var r_ = new Mas.Schema.Common.AnyValueHolder.Result_Value{Val = val};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class AnyValueHolder
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc9851222d70aff42UL)]
        public class Params_Value : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc9851222d70aff42UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb5fca46714e53e71UL)]
        public class Result_Value : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb5fca46714e53e71UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Val = CapnpSerializable.Create<object>(reader.Val);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Val.SetObject(Val);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public object Val
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
                public DeserializerState Val => ctx.StructReadPointer(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public DynamicSerializerState Val
                {
                    get => BuildPointer<DynamicSerializerState>(0);
                    set => Link(0, value);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcac9c6537df1a097UL), Proxy(typeof(CapHolder_Proxy<>)), Skeleton(typeof(CapHolder_Skeleton<>))]
    public interface ICapHolder<TObject> : IDisposable where TObject : class
    {
        Task<TObject> Cap(CancellationToken cancellationToken_ = default);
        Task Release(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcac9c6537df1a097UL)]
    public class CapHolder_Proxy<TObject> : Proxy, ICapHolder<TObject> where TObject : class
    {
        public Task<TObject> Cap(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.CapHolder<TObject>.Params_Cap.WRITER>();
            var arg_ = new Mas.Schema.Common.CapHolder<TObject>.Params_Cap()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(14612428527877857431UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Common.CapHolder<TObject>.Result_Cap>(d_);
                    return (r_.Object);
                }
            }

            );
        }

        public async Task Release(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.CapHolder<TObject>.Params_Release.WRITER>();
            var arg_ = new Mas.Schema.Common.CapHolder<TObject>.Params_Release()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(14612428527877857431UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Common.CapHolder<TObject>.Result_Release>(d_);
                return;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcac9c6537df1a097UL)]
    public class CapHolder_Skeleton<TObject> : Skeleton<ICapHolder<TObject>> where TObject : class
    {
        public CapHolder_Skeleton()
        {
            SetMethodTable(Cap, Release);
        }

        public override ulong InterfaceId => 14612428527877857431UL;
        Task<AnswerOrCounterquestion> Cap(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Cap(cancellationToken_), @object =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.CapHolder<TObject>.Result_Cap.WRITER>();
                    var r_ = new Mas.Schema.Common.CapHolder<TObject>.Result_Cap{Object = @object};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        async Task<AnswerOrCounterquestion> Release(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                await Impl.Release(cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.CapHolder<TObject>.Result_Release.WRITER>();
                return s_;
            }
        }
    }

    public static class CapHolder<TObject>
        where TObject : class
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xda52b34d937fa814UL)]
        public class Params_Cap : ICapnpSerializable
        {
            public const UInt64 typeId = 0xda52b34d937fa814UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdc9b0f483595691fUL)]
        public class Result_Cap : ICapnpSerializable
        {
            public const UInt64 typeId = 0xdc9b0f483595691fUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Object = CapnpSerializable.Create<TObject>(reader.Object);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Object.SetObject(Object);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public TObject Object
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
                public DeserializerState Object => ctx.StructReadPointer(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public DynamicSerializerState Object
                {
                    get => BuildPointer<DynamicSerializerState>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x82449708d4fd120dUL)]
        public class Params_Release : ICapnpSerializable
        {
            public const UInt64 typeId = 0x82449708d4fd120dUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc1374ccca01e2b53UL)]
        public class Result_Release : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc1374ccca01e2b53UL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xee543d7c305d56f6UL), Proxy(typeof(IdentifiableHolder_Proxy)), Skeleton(typeof(IdentifiableHolder_Skeleton))]
    public interface IIdentifiableHolder : Mas.Schema.Common.IIdentifiable
    {
        Task<Mas.Schema.Common.IIdentifiable> Cap(CancellationToken cancellationToken_ = default);
        Task Release(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xee543d7c305d56f6UL)]
    public class IdentifiableHolder_Proxy : Proxy, IIdentifiableHolder
    {
        public Task<Mas.Schema.Common.IIdentifiable> Cap(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.IdentifiableHolder.Params_Cap.WRITER>();
            var arg_ = new Mas.Schema.Common.IdentifiableHolder.Params_Cap()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(17173418882667206390UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Common.IdentifiableHolder.Result_Cap>(d_);
                    return (r_.Cap);
                }
            }

            );
        }

        public async Task Release(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.IdentifiableHolder.Params_Release.WRITER>();
            var arg_ = new Mas.Schema.Common.IdentifiableHolder.Params_Release()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(17173418882667206390UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Common.IdentifiableHolder.Result_Release>(d_);
                return;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xee543d7c305d56f6UL)]
    public class IdentifiableHolder_Skeleton : Skeleton<IIdentifiableHolder>
    {
        public IdentifiableHolder_Skeleton()
        {
            SetMethodTable(Cap, Release);
        }

        public override ulong InterfaceId => 17173418882667206390UL;
        Task<AnswerOrCounterquestion> Cap(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Cap(cancellationToken_), cap =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.IdentifiableHolder.Result_Cap.WRITER>();
                    var r_ = new Mas.Schema.Common.IdentifiableHolder.Result_Cap{Cap = cap};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        async Task<AnswerOrCounterquestion> Release(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                await Impl.Release(cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.IdentifiableHolder.Result_Release.WRITER>();
                return s_;
            }
        }
    }

    public static class IdentifiableHolder
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x93a1b0e66bcacfbbUL)]
        public class Params_Cap : ICapnpSerializable
        {
            public const UInt64 typeId = 0x93a1b0e66bcacfbbUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb9fe596d7cadbdccUL)]
        public class Result_Cap : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb9fe596d7cadbdccUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Cap = reader.Cap;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Cap = Cap;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Common.IIdentifiable Cap
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
                public Mas.Schema.Common.IIdentifiable Cap => ctx.ReadCap<Mas.Schema.Common.IIdentifiable>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Common.IIdentifiable Cap
                {
                    get => ReadCap<Mas.Schema.Common.IIdentifiable>(0);
                    set => LinkObject(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd69f2da1efe3faafUL)]
        public class Params_Release : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd69f2da1efe3faafUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8ca159db7de3927bUL)]
        public class Result_Release : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8ca159db7de3927bUL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc201bf46dd40051eUL)]
    public class ListEntry<TPointerType> : ICapnpSerializable where TPointerType : class
    {
        public const UInt64 typeId = 0xc201bf46dd40051eUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Entry = CapnpSerializable.Create<TPointerType>(reader.Entry);
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Entry.SetObject(Entry);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public TPointerType Entry
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
            public DeserializerState Entry => ctx.StructReadPointer(0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 1);
            }

            public DynamicSerializerState Entry
            {
                get => BuildPointer<DynamicSerializerState>(0);
                set => Link(0, value);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xce7e4202f09e314aUL), Proxy(typeof(Stopable_Proxy)), Skeleton(typeof(Stopable_Skeleton))]
    public interface IStopable : IDisposable
    {
        Task Stop(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xce7e4202f09e314aUL)]
    public class Stopable_Proxy : Proxy, IStopable
    {
        public async Task Stop(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.Stopable.Params_Stop.WRITER>();
            var arg_ = new Mas.Schema.Common.Stopable.Params_Stop()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(14879402799272964426UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Common.Stopable.Result_Stop>(d_);
                return;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xce7e4202f09e314aUL)]
    public class Stopable_Skeleton : Skeleton<IStopable>
    {
        public Stopable_Skeleton()
        {
            SetMethodTable(Stop);
        }

        public override ulong InterfaceId => 14879402799272964426UL;
        async Task<AnswerOrCounterquestion> Stop(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                await Impl.Stop(cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.Stopable.Result_Stop.WRITER>();
                return s_;
            }
        }
    }

    public static class Stopable
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x98a27c9476315729UL)]
        public class Params_Stop : ICapnpSerializable
        {
            public const UInt64 typeId = 0x98a27c9476315729UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd3a3b24aaa056a5cUL)]
        public class Result_Stop : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd3a3b24aaa056a5cUL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd67792aa3fc241beUL)]
    public class LL<TH, TT> : ICapnpSerializable where TH : class where TT : class
    {
        public const UInt64 typeId = 0xd67792aa3fc241beUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Head = CapnpSerializable.Create<TH>(reader.Head);
            Tail = CapnpSerializable.Create<TT>(reader.Tail);
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Head.SetObject(Head);
            writer.Tail.SetObject(Tail);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public TH Head
        {
            get;
            set;
        }

        public TT Tail
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
            public DeserializerState Head => ctx.StructReadPointer(0);
            public DeserializerState Tail => ctx.StructReadPointer(1);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 2);
            }

            public DynamicSerializerState Head
            {
                get => BuildPointer<DynamicSerializerState>(0);
                set => Link(0, value);
            }

            public DynamicSerializerState Tail
            {
                get => BuildPointer<DynamicSerializerState>(1);
                set => Link(1, value);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa8b91e2c1f8c929aUL), Proxy(typeof(Clock_Proxy<>)), Skeleton(typeof(Clock_Skeleton<>))]
    public interface IClock<TT> : IDisposable where TT : class
    {
        Task Tick(TT time, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa8b91e2c1f8c929aUL)]
    public class Clock_Proxy<TT> : Proxy, IClock<TT> where TT : class
    {
        public async Task Tick(TT time, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.Clock<TT>.Params_Tick.WRITER>();
            var arg_ = new Mas.Schema.Common.Clock<TT>.Params_Tick()
            {Time = time};
            arg_?.serialize(in_);
            using (var d_ = await Call(12157781843920065178UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Common.Clock<TT>.Result_Tick>(d_);
                return;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa8b91e2c1f8c929aUL)]
    public class Clock_Skeleton<TT> : Skeleton<IClock<TT>> where TT : class
    {
        public Clock_Skeleton()
        {
            SetMethodTable(Tick);
        }

        public override ulong InterfaceId => 12157781843920065178UL;
        async Task<AnswerOrCounterquestion> Tick(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Common.Clock<TT>.Params_Tick>(d_);
                await Impl.Tick(in_.Time, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.Clock<TT>.Result_Tick.WRITER>();
                return s_;
            }
        }
    }

    public static class Clock<TT>
        where TT : class
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfd0735841a7da108UL)]
        public class Params_Tick : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfd0735841a7da108UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Time = CapnpSerializable.Create<TT>(reader.Time);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Time.SetObject(Time);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public TT Time
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
                public DeserializerState Time => ctx.StructReadPointer(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public DynamicSerializerState Time
                {
                    get => BuildPointer<DynamicSerializerState>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc953edb8b6293fafUL)]
        public class Result_Tick : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc953edb8b6293fafUL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd39ff99bbab1a74eUL)]
    public class IP : ICapnpSerializable
    {
        public const UInt64 typeId = 0xd39ff99bbab1a74eUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Attributes = reader.Attributes?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Common.IP.KV>(_));
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

        public IReadOnlyList<Mas.Schema.Common.IP.KV> Attributes
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
            public IReadOnlyList<Mas.Schema.Common.IP.KV.READER> Attributes => ctx.ReadList(0).Cast(Mas.Schema.Common.IP.KV.READER.create);
            public bool HasAttributes => ctx.IsStructFieldNonNull(0);
            public DeserializerState Content => ctx.StructReadPointer(1);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 2);
            }

            public ListOfStructsSerializer<Mas.Schema.Common.IP.KV.WRITER> Attributes
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Common.IP.KV.WRITER>>(0);
                set => Link(0, value);
            }

            public DynamicSerializerState Content
            {
                get => BuildPointer<DynamicSerializerState>(1);
                set => Link(1, value);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb07588184ad8aac5UL)]
        public class KV : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb07588184ad8aac5UL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf0c0f9413a3083beUL), Proxy(typeof(Channel_Proxy<>)), Skeleton(typeof(Channel_Skeleton<>))]
    public interface IChannel<TV> : Mas.Schema.Common.IIdentifiable, Mas.Schema.Persistence.IPersistent where TV : class
    {
        Task SetBufferSize(ulong size, CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Common.Channel<TV>.IReader> Reader(CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Common.Channel<TV>.IWriter> Writer(CancellationToken cancellationToken_ = default);
        Task<(Mas.Schema.Common.Channel<TV>.IReader, Mas.Schema.Common.Channel<TV>.IWriter)> Endpoints(CancellationToken cancellationToken_ = default);
        Task SetAutoCloseSemantics(Mas.Schema.Common.Channel<TV>.CloseSemantics cs, CancellationToken cancellationToken_ = default);
        Task Close(bool waitForEmptyBuffer, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf0c0f9413a3083beUL)]
    public class Channel_Proxy<TV> : Proxy, IChannel<TV> where TV : class
    {
        public async Task SetBufferSize(ulong size, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.Channel<TV>.Params_SetBufferSize.WRITER>();
            var arg_ = new Mas.Schema.Common.Channel<TV>.Params_SetBufferSize()
            {Size = size};
            arg_?.serialize(in_);
            using (var d_ = await Call(17348139823175599038UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Common.Channel<TV>.Result_SetBufferSize>(d_);
                return;
            }
        }

        public Task<Mas.Schema.Common.Channel<TV>.IReader> Reader(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.Channel<TV>.Params_Reader.WRITER>();
            var arg_ = new Mas.Schema.Common.Channel<TV>.Params_Reader()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(17348139823175599038UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Common.Channel<TV>.Result_Reader>(d_);
                    return (r_.R);
                }
            }

            );
        }

        public Task<Mas.Schema.Common.Channel<TV>.IWriter> Writer(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.Channel<TV>.Params_Writer.WRITER>();
            var arg_ = new Mas.Schema.Common.Channel<TV>.Params_Writer()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(17348139823175599038UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Common.Channel<TV>.Result_Writer>(d_);
                    return (r_.W);
                }
            }

            );
        }

        public Task<(Mas.Schema.Common.Channel<TV>.IReader, Mas.Schema.Common.Channel<TV>.IWriter)> Endpoints(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.Channel<TV>.Params_Endpoints.WRITER>();
            var arg_ = new Mas.Schema.Common.Channel<TV>.Params_Endpoints()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(17348139823175599038UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Common.Channel<TV>.Result_Endpoints>(d_);
                    return (r_.R, r_.W);
                }
            }

            );
        }

        public async Task SetAutoCloseSemantics(Mas.Schema.Common.Channel<TV>.CloseSemantics cs, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.Channel<TV>.Params_SetAutoCloseSemantics.WRITER>();
            var arg_ = new Mas.Schema.Common.Channel<TV>.Params_SetAutoCloseSemantics()
            {Cs = cs};
            arg_?.serialize(in_);
            using (var d_ = await Call(17348139823175599038UL, 4, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Common.Channel<TV>.Result_SetAutoCloseSemantics>(d_);
                return;
            }
        }

        public async Task Close(bool waitForEmptyBuffer, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.Channel<TV>.Params_Close.WRITER>();
            var arg_ = new Mas.Schema.Common.Channel<TV>.Params_Close()
            {WaitForEmptyBuffer = waitForEmptyBuffer};
            arg_?.serialize(in_);
            using (var d_ = await Call(17348139823175599038UL, 5, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Common.Channel<TV>.Result_Close>(d_);
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf0c0f9413a3083beUL)]
    public class Channel_Skeleton<TV> : Skeleton<IChannel<TV>> where TV : class
    {
        public Channel_Skeleton()
        {
            SetMethodTable(SetBufferSize, Reader, Writer, Endpoints, SetAutoCloseSemantics, Close);
        }

        public override ulong InterfaceId => 17348139823175599038UL;
        async Task<AnswerOrCounterquestion> SetBufferSize(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Common.Channel<TV>.Params_SetBufferSize>(d_);
                await Impl.SetBufferSize(in_.Size, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.Channel<TV>.Result_SetBufferSize.WRITER>();
                return s_;
            }
        }

        Task<AnswerOrCounterquestion> Reader(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Reader(cancellationToken_), r =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.Channel<TV>.Result_Reader.WRITER>();
                    var r_ = new Mas.Schema.Common.Channel<TV>.Result_Reader{R = r};
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
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.Channel<TV>.Result_Writer.WRITER>();
                    var r_ = new Mas.Schema.Common.Channel<TV>.Result_Writer{W = w};
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
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.Channel<TV>.Result_Endpoints.WRITER>();
                    var r_ = new Mas.Schema.Common.Channel<TV>.Result_Endpoints{R = r, W = w};
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
                var in_ = CapnpSerializable.Create<Mas.Schema.Common.Channel<TV>.Params_SetAutoCloseSemantics>(d_);
                await Impl.SetAutoCloseSemantics(in_.Cs, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.Channel<TV>.Result_SetAutoCloseSemantics.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> Close(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Common.Channel<TV>.Params_Close>(d_);
                await Impl.Close(in_.WaitForEmptyBuffer, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.Channel<TV>.Result_Close.WRITER>();
                return s_;
            }
        }
    }

    public static class Channel<TV>
        where TV : class
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x956ee3f21ad6b221UL)]
        public enum CloseSemantics : ushort
        {
            fbp,
            no
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x876b422c6839e6b2UL)]
        public class Msg : ICapnpSerializable
        {
            public const UInt64 typeId = 0x876b422c6839e6b2UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb7c45baf591227b6UL)]
        public class StartupInfo : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb7c45baf591227b6UL;
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

            public Mas.Schema.Common.Channel<TV>.CloseSemantics CloseSemantics
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
                public Mas.Schema.Common.Channel<TV>.CloseSemantics CloseSemantics => (Mas.Schema.Common.Channel<TV>.CloseSemantics)ctx.ReadDataUShort(64UL, (ushort)0);
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

                public Mas.Schema.Common.Channel<TV>.CloseSemantics CloseSemantics
                {
                    get => (Mas.Schema.Common.Channel<TV>.CloseSemantics)this.ReadDataUShort(64UL, (ushort)0);
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9c656810b30decd7UL), Proxy(typeof(Channel<>.Reader_Proxy)), Skeleton(typeof(Channel<>.Reader_Skeleton))]
        public interface IReader : IDisposable
        {
            Task<Mas.Schema.Common.Channel<TV>.Msg> Read(CancellationToken cancellationToken_ = default);
            Task Close(CancellationToken cancellationToken_ = default);
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9c656810b30decd7UL)]
        public class Reader_Proxy : Proxy, IReader
        {
            public Task<Mas.Schema.Common.Channel<TV>.Msg> Read(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.Channel<TV>.Reader.Params_Read.WRITER>();
                var arg_ = new Mas.Schema.Common.Channel<TV>.Reader.Params_Read()
                {};
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(11269528063497333975UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Common.Channel<TV>.Msg>(d_);
                        return r_;
                    }
                }

                );
            }

            public async Task Close(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.Channel<TV>.Reader.Params_Close.WRITER>();
                var arg_ = new Mas.Schema.Common.Channel<TV>.Reader.Params_Close()
                {};
                arg_?.serialize(in_);
                using (var d_ = await Call(11269528063497333975UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Common.Channel<TV>.Reader.Result_Close>(d_);
                    return;
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9c656810b30decd7UL)]
        public class Reader_Skeleton : Skeleton<IReader>
        {
            public Reader_Skeleton()
            {
                SetMethodTable(Read, Close);
            }

            public override ulong InterfaceId => 11269528063497333975UL;
            Task<AnswerOrCounterquestion> Read(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    return Impatient.MaybeTailCall(Impl.Read(cancellationToken_), r_ =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.Channel<TV>.Msg.WRITER>();
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
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.Channel<TV>.Reader.Result_Close.WRITER>();
                    return s_;
                }
            }
        }

        public static class Reader
        {
            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf8dc00b2d60ea82fUL)]
            public class Params_Read : ICapnpSerializable
            {
                public const UInt64 typeId = 0xf8dc00b2d60ea82fUL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x926107b4c88d431fUL)]
            public class Params_Close : ICapnpSerializable
            {
                public const UInt64 typeId = 0x926107b4c88d431fUL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xec0d27e49a0f9f3aUL)]
            public class Result_Close : ICapnpSerializable
            {
                public const UInt64 typeId = 0xec0d27e49a0f9f3aUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9b5844944dc0f458UL), Proxy(typeof(Channel<>.Writer_Proxy)), Skeleton(typeof(Channel<>.Writer_Skeleton))]
        public interface IWriter : IDisposable
        {
            Task Write(Mas.Schema.Common.Channel<TV>.Msg arg_, CancellationToken cancellationToken_ = default);
            Task Close(CancellationToken cancellationToken_ = default);
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9b5844944dc0f458UL)]
        public class Writer_Proxy : Proxy, IWriter
        {
            public async Task Write(Mas.Schema.Common.Channel<TV>.Msg arg_, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.Channel<TV>.Msg.WRITER>();
                arg_?.serialize(in_);
                using (var d_ = await Call(11193772277579707480UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Common.Channel<TV>.Writer.Result_Write>(d_);
                    return;
                }
            }

            public async Task Close(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.Channel<TV>.Writer.Params_Close.WRITER>();
                var arg_ = new Mas.Schema.Common.Channel<TV>.Writer.Params_Close()
                {};
                arg_?.serialize(in_);
                using (var d_ = await Call(11193772277579707480UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Common.Channel<TV>.Writer.Result_Close>(d_);
                    return;
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9b5844944dc0f458UL)]
        public class Writer_Skeleton : Skeleton<IWriter>
        {
            public Writer_Skeleton()
            {
                SetMethodTable(Write, Close);
            }

            public override ulong InterfaceId => 11193772277579707480UL;
            async Task<AnswerOrCounterquestion> Write(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    await Impl.Write(CapnpSerializable.Create<Mas.Schema.Common.Channel<TV>.Msg>(d_), cancellationToken_);
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.Channel<TV>.Writer.Result_Write.WRITER>();
                    return s_;
                }
            }

            async Task<AnswerOrCounterquestion> Close(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    await Impl.Close(cancellationToken_);
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.Channel<TV>.Writer.Result_Close.WRITER>();
                    return s_;
                }
            }
        }

        public static class Writer
        {
            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x98be830bb53c6eb9UL)]
            public class Result_Write : ICapnpSerializable
            {
                public const UInt64 typeId = 0x98be830bb53c6eb9UL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb86feee7ac7bebecUL)]
            public class Params_Close : ICapnpSerializable
            {
                public const UInt64 typeId = 0xb86feee7ac7bebecUL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x86215e947f0afb85UL)]
            public class Result_Close : ICapnpSerializable
            {
                public const UInt64 typeId = 0x86215e947f0afb85UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xee378f318f32b853UL)]
        public class Params_SetBufferSize : ICapnpSerializable
        {
            public const UInt64 typeId = 0xee378f318f32b853UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xabd31dc62bd9a48bUL)]
        public class Result_SetBufferSize : ICapnpSerializable
        {
            public const UInt64 typeId = 0xabd31dc62bd9a48bUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb6563114436deea9UL)]
        public class Params_Reader : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb6563114436deea9UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x89aeecad59dc62eeUL)]
        public class Result_Reader : ICapnpSerializable
        {
            public const UInt64 typeId = 0x89aeecad59dc62eeUL;
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

            public Mas.Schema.Common.Channel<TV>.IReader R
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
                public Mas.Schema.Common.Channel<TV>.IReader R => ctx.ReadCap<Mas.Schema.Common.Channel<TV>.IReader>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Common.Channel<TV>.IReader R
                {
                    get => ReadCap<Mas.Schema.Common.Channel<TV>.IReader>(0);
                    set => LinkObject(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9882f67fa6ace6a1UL)]
        public class Params_Writer : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9882f67fa6ace6a1UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd7e3695f7166e987UL)]
        public class Result_Writer : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd7e3695f7166e987UL;
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

            public Mas.Schema.Common.Channel<TV>.IWriter W
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
                public Mas.Schema.Common.Channel<TV>.IWriter W => ctx.ReadCap<Mas.Schema.Common.Channel<TV>.IWriter>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Common.Channel<TV>.IWriter W
                {
                    get => ReadCap<Mas.Schema.Common.Channel<TV>.IWriter>(0);
                    set => LinkObject(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdb1f001689bbac5dUL)]
        public class Params_Endpoints : ICapnpSerializable
        {
            public const UInt64 typeId = 0xdb1f001689bbac5dUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc8a1bc4350302330UL)]
        public class Result_Endpoints : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc8a1bc4350302330UL;
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

            public Mas.Schema.Common.Channel<TV>.IReader R
            {
                get;
                set;
            }

            public Mas.Schema.Common.Channel<TV>.IWriter W
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
                public Mas.Schema.Common.Channel<TV>.IReader R => ctx.ReadCap<Mas.Schema.Common.Channel<TV>.IReader>(0);
                public Mas.Schema.Common.Channel<TV>.IWriter W => ctx.ReadCap<Mas.Schema.Common.Channel<TV>.IWriter>(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public Mas.Schema.Common.Channel<TV>.IReader R
                {
                    get => ReadCap<Mas.Schema.Common.Channel<TV>.IReader>(0);
                    set => LinkObject(0, value);
                }

                public Mas.Schema.Common.Channel<TV>.IWriter W
                {
                    get => ReadCap<Mas.Schema.Common.Channel<TV>.IWriter>(1);
                    set => LinkObject(1, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd8727b5440681ed4UL)]
        public class Params_SetAutoCloseSemantics : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd8727b5440681ed4UL;
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

            public Mas.Schema.Common.Channel<TV>.CloseSemantics Cs
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
                public Mas.Schema.Common.Channel<TV>.CloseSemantics Cs => (Mas.Schema.Common.Channel<TV>.CloseSemantics)ctx.ReadDataUShort(0UL, (ushort)0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public Mas.Schema.Common.Channel<TV>.CloseSemantics Cs
                {
                    get => (Mas.Schema.Common.Channel<TV>.CloseSemantics)this.ReadDataUShort(0UL, (ushort)0);
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe8ba8300eb17a23cUL)]
        public class Result_SetAutoCloseSemantics : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe8ba8300eb17a23cUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf8224774a4d4d6f5UL)]
        public class Params_Close : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf8224774a4d4d6f5UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf20dfadcec8a0e13UL)]
        public class Result_Close : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf20dfadcec8a0e13UL;
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

    public static partial class PipeliningSupportExtensions_common
    {
        static readonly MemberAccessPath Path_mas_schema_common_Channel_endpoints_W = new MemberAccessPath(1U);
        public static Mas.Schema.Common.Channel<TV>.IWriter W<TV>(this Task<(Mas.Schema.Common.Channel<TV>.IReader, Mas.Schema.Common.Channel<TV>.IWriter)> task) where TV : class
        {
            async Task<IDisposable> AwaitProxy() => (await task).Item2;
            return (Mas.Schema.Common.Channel<TV>.IWriter)CapabilityReflection.CreateProxy<Mas.Schema.Common.Channel<TV>.IWriter>(Impatient.Access(task, Path_mas_schema_common_Channel_endpoints_W, AwaitProxy()));
        }
    }
}