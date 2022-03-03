using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Rpc.Common
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
        Task<Mas.Rpc.Common.IdInformation> Info(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb2afd1cb599c48d5UL)]
    public class Identifiable_Proxy : Proxy, IIdentifiable
    {
        public async Task<Mas.Rpc.Common.IdInformation> Info(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Identifiable.Params_Info.WRITER>();
            var arg_ = new Mas.Rpc.Common.Identifiable.Params_Info()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(12875740530987518165UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.Common.IdInformation>(d_);
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
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Common.IdInformation.WRITER>();
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
            Structure = CapnpSerializable.Create<Mas.Rpc.Common.StructuredText.structure>(reader.Structure);
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

        public Mas.Rpc.Common.StructuredText.structure Structure
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
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Callback.Params_Call.WRITER>();
            var arg_ = new Mas.Rpc.Common.Callback.Params_Call()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(10387839295393100055UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.Common.Callback.Result_Call>(d_);
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
                var s_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Callback.Result_Call.WRITER>();
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
    public interface IAction : Mas.Rpc.Persistence.IPersistent
    {
        Task Do(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9dd4e2c3d76e4587UL)]
    public class Action_Proxy : Proxy, IAction
    {
        public async Task Do(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Action.Params_Do.WRITER>();
            var arg_ = new Mas.Rpc.Common.Action.Params_Do()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(11372964289778173319UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.Common.Action.Result_Do>(d_);
                return;
            }
        }

        public async Task<(string, string)> Save(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Persistence.Persistent.Params_Save.WRITER>();
            var arg_ = new Mas.Rpc.Persistence.Persistent.Params_Save()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(13954362354854972261UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.Persistence.Persistent.Result_Save>(d_);
                return (r_.SturdyRef, r_.UnsaveSR);
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
                var s_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Action.Result_Do.WRITER>();
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
    public interface IAction1 : Mas.Rpc.Persistence.IPersistent
    {
        Task Do(object p, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc12db9a9ae07a763UL)]
    public class Action1_Proxy : Proxy, IAction1
    {
        public async Task Do(object p, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Action1.Params_Do.WRITER>();
            var arg_ = new Mas.Rpc.Common.Action1.Params_Do()
            {P = p};
            arg_?.serialize(in_);
            using (var d_ = await Call(13919986161692419939UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.Common.Action1.Result_Do>(d_);
                return;
            }
        }

        public async Task<(string, string)> Save(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Persistence.Persistent.Params_Save.WRITER>();
            var arg_ = new Mas.Rpc.Persistence.Persistent.Params_Save()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(13954362354854972261UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.Persistence.Persistent.Result_Save>(d_);
                return (r_.SturdyRef, r_.UnsaveSR);
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
                var in_ = CapnpSerializable.Create<Mas.Rpc.Common.Action1.Params_Do>(d_);
                await Impl.Do(in_.P, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Action1.Result_Do.WRITER>();
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
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Factory<TInput, TOutput>.Params_Produce.WRITER>();
            var arg_ = new Mas.Rpc.Common.Factory<TInput, TOutput>.Params_Produce()
            {In = @in};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(12135500100874563289UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.Common.Factory<TInput, TOutput>.Result_Produce>(d_);
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
                var in_ = CapnpSerializable.Create<Mas.Rpc.Common.Factory<TInput, TOutput>.Params_Produce>(d_);
                return Impatient.MaybeTailCall(Impl.Produce(in_.In, cancellationToken_), @out =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Factory<TInput, TOutput>.Result_Produce.WRITER>();
                    var r_ = new Mas.Rpc.Common.Factory<TInput, TOutput>.Result_Produce{Out = @out};
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
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.CapHolder<TObject>.Params_Cap.WRITER>();
            var arg_ = new Mas.Rpc.Common.CapHolder<TObject>.Params_Cap()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(14612428527877857431UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.Common.CapHolder<TObject>.Result_Cap>(d_);
                    return (r_.Object);
                }
            }

            );
        }

        public async Task Release(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.CapHolder<TObject>.Params_Release.WRITER>();
            var arg_ = new Mas.Rpc.Common.CapHolder<TObject>.Params_Release()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(14612428527877857431UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.Common.CapHolder<TObject>.Result_Release>(d_);
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
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Common.CapHolder<TObject>.Result_Cap.WRITER>();
                    var r_ = new Mas.Rpc.Common.CapHolder<TObject>.Result_Cap{Object = @object};
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
                var s_ = SerializerState.CreateForRpc<Mas.Rpc.Common.CapHolder<TObject>.Result_Release.WRITER>();
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
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Stopable.Params_Stop.WRITER>();
            var arg_ = new Mas.Rpc.Common.Stopable.Params_Stop()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(14879402799272964426UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.Common.Stopable.Result_Stop>(d_);
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
                var s_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Stopable.Result_Stop.WRITER>();
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
}