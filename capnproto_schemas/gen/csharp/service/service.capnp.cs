using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Rpc.Service
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfec1f88b198df649UL), Proxy(typeof(Admin_Proxy)), Skeleton(typeof(Admin_Skeleton))]
    public interface IAdmin : IDisposable
    {
        Task Heartbeat(CancellationToken cancellationToken_ = default);
        Task SetTimeout(ulong seconds, CancellationToken cancellationToken_ = default);
        Task Stop(CancellationToken cancellationToken_ = default);
        Task<Mas.Rpc.Common.IdInformation> Identity(CancellationToken cancellationToken_ = default);
        Task UpdateIdentity(Mas.Rpc.Common.IdInformation arg_, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfec1f88b198df649UL)]
    public class Admin_Proxy : Proxy, IAdmin
    {
        public async Task Heartbeat(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Service.Admin.Params_Heartbeat.WRITER>();
            var arg_ = new Mas.Rpc.Service.Admin.Params_Heartbeat()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(18357226832451728969UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.Service.Admin.Result_Heartbeat>(d_);
                return;
            }
        }

        public async Task SetTimeout(ulong seconds, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Service.Admin.Params_SetTimeout.WRITER>();
            var arg_ = new Mas.Rpc.Service.Admin.Params_SetTimeout()
            {Seconds = seconds};
            arg_?.serialize(in_);
            using (var d_ = await Call(18357226832451728969UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.Service.Admin.Result_SetTimeout>(d_);
                return;
            }
        }

        public async Task Stop(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Service.Admin.Params_Stop.WRITER>();
            var arg_ = new Mas.Rpc.Service.Admin.Params_Stop()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(18357226832451728969UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.Service.Admin.Result_Stop>(d_);
                return;
            }
        }

        public async Task<Mas.Rpc.Common.IdInformation> Identity(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Service.Admin.Params_Identity.WRITER>();
            var arg_ = new Mas.Rpc.Service.Admin.Params_Identity()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(18357226832451728969UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.Common.IdInformation>(d_);
                return r_;
            }
        }

        public async Task UpdateIdentity(Mas.Rpc.Common.IdInformation arg_, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.IdInformation.WRITER>();
            arg_?.serialize(in_);
            using (var d_ = await Call(18357226832451728969UL, 4, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.Service.Admin.Result_UpdateIdentity>(d_);
                return;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfec1f88b198df649UL)]
    public class Admin_Skeleton : Skeleton<IAdmin>
    {
        public Admin_Skeleton()
        {
            SetMethodTable(Heartbeat, SetTimeout, Stop, Identity, UpdateIdentity);
        }

        public override ulong InterfaceId => 18357226832451728969UL;
        async Task<AnswerOrCounterquestion> Heartbeat(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                await Impl.Heartbeat(cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Rpc.Service.Admin.Result_Heartbeat.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetTimeout(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.Service.Admin.Params_SetTimeout>(d_);
                await Impl.SetTimeout(in_.Seconds, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Rpc.Service.Admin.Result_SetTimeout.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> Stop(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                await Impl.Stop(cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Rpc.Service.Admin.Result_Stop.WRITER>();
                return s_;
            }
        }

        Task<AnswerOrCounterquestion> Identity(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Identity(cancellationToken_), r_ =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Common.IdInformation.WRITER>();
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        async Task<AnswerOrCounterquestion> UpdateIdentity(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                await Impl.UpdateIdentity(CapnpSerializable.Create<Mas.Rpc.Common.IdInformation>(d_), cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Rpc.Service.Admin.Result_UpdateIdentity.WRITER>();
                return s_;
            }
        }
    }

    public static class Admin
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd627f31bdae7f234UL)]
        public class Params_Heartbeat : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd627f31bdae7f234UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe01c2290ae549759UL)]
        public class Result_Heartbeat : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe01c2290ae549759UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9abf358a691110fdUL)]
        public class Params_SetTimeout : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9abf358a691110fdUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Seconds = reader.Seconds;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Seconds = Seconds;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public ulong Seconds
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
                public ulong Seconds => ctx.ReadDataULong(0UL, 0UL);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public ulong Seconds
                {
                    get => this.ReadDataULong(0UL, 0UL);
                    set => this.WriteData(0UL, value, 0UL);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd6fd194a1ac74bc1UL)]
        public class Result_SetTimeout : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd6fd194a1ac74bc1UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xddc3d3cd37ef5b78UL)]
        public class Params_Stop : ICapnpSerializable
        {
            public const UInt64 typeId = 0xddc3d3cd37ef5b78UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xce2a1a063e759787UL)]
        public class Result_Stop : ICapnpSerializable
        {
            public const UInt64 typeId = 0xce2a1a063e759787UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xff4271628d295896UL)]
        public class Params_Identity : ICapnpSerializable
        {
            public const UInt64 typeId = 0xff4271628d295896UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb15e79db08e2ab2cUL)]
        public class Result_UpdateIdentity : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb15e79db08e2ab2cUL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaba5829222c213cbUL), Proxy(typeof(SimpleFactory_Proxy)), Skeleton(typeof(SimpleFactory_Skeleton))]
    public interface ISimpleFactory : Mas.Rpc.Common.IIdentifiable
    {
        Task<IReadOnlyList<Mas.Rpc.Common.IIdentifiable>> Create(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaba5829222c213cbUL)]
    public class SimpleFactory_Proxy : Proxy, ISimpleFactory
    {
        public Task<IReadOnlyList<Mas.Rpc.Common.IIdentifiable>> Create(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Service.SimpleFactory.Params_Create.WRITER>();
            var arg_ = new Mas.Rpc.Service.SimpleFactory.Params_Create()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(12368435515802915787UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.Service.SimpleFactory.Result_Create>(d_);
                    return (r_.Caps);
                }
            }

            );
        }

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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaba5829222c213cbUL)]
    public class SimpleFactory_Skeleton : Skeleton<ISimpleFactory>
    {
        public SimpleFactory_Skeleton()
        {
            SetMethodTable(Create);
        }

        public override ulong InterfaceId => 12368435515802915787UL;
        Task<AnswerOrCounterquestion> Create(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Create(cancellationToken_), caps =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Service.SimpleFactory.Result_Create.WRITER>();
                    var r_ = new Mas.Rpc.Service.SimpleFactory.Result_Create{Caps = caps};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class SimpleFactory
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa8296fdc60dcb6ddUL)]
        public class Params_Create : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa8296fdc60dcb6ddUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x89a33828e0de1eaaUL)]
        public class Result_Create : ICapnpSerializable
        {
            public const UInt64 typeId = 0x89a33828e0de1eaaUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Caps = reader.Caps;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Caps.Init(Caps);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<Mas.Rpc.Common.IIdentifiable> Caps
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
                public IReadOnlyList<Mas.Rpc.Common.IIdentifiable> Caps => ctx.ReadCapList<Mas.Rpc.Common.IIdentifiable>(0);
                public bool HasCaps => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfCapsSerializer<Mas.Rpc.Common.IIdentifiable> Caps
                {
                    get => BuildPointer<ListOfCapsSerializer<Mas.Rpc.Common.IIdentifiable>>(0);
                    set => Link(0, value);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8ab0ecb99c269c7fUL), Proxy(typeof(Factory_Proxy<>)), Skeleton(typeof(Factory_Skeleton<>))]
    public interface IFactory<TPayload> : Mas.Rpc.Common.IIdentifiable where TPayload : class
    {
        Task<Mas.Rpc.Service.Factory<TPayload>.AccessInfo> Create(Mas.Rpc.Service.Factory<TPayload>.CreateParams arg_, CancellationToken cancellationToken_ = default);
        Task<IReadOnlyList<string>> ServiceInterfaceNames(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8ab0ecb99c269c7fUL)]
    public class Factory_Proxy<TPayload> : Proxy, IFactory<TPayload> where TPayload : class
    {
        public Task<Mas.Rpc.Service.Factory<TPayload>.AccessInfo> Create(Mas.Rpc.Service.Factory<TPayload>.CreateParams arg_, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Service.Factory<TPayload>.CreateParams.WRITER>();
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(9993747855068011647UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.Service.Factory<TPayload>.AccessInfo>(d_);
                    return r_;
                }
            }

            );
        }

        public async Task<IReadOnlyList<string>> ServiceInterfaceNames(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Service.Factory<TPayload>.Params_ServiceInterfaceNames.WRITER>();
            var arg_ = new Mas.Rpc.Service.Factory<TPayload>.Params_ServiceInterfaceNames()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(9993747855068011647UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.Service.Factory<TPayload>.Result_ServiceInterfaceNames>(d_);
                return (r_.Names);
            }
        }

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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8ab0ecb99c269c7fUL)]
    public class Factory_Skeleton<TPayload> : Skeleton<IFactory<TPayload>> where TPayload : class
    {
        public Factory_Skeleton()
        {
            SetMethodTable(Create, ServiceInterfaceNames);
        }

        public override ulong InterfaceId => 9993747855068011647UL;
        Task<AnswerOrCounterquestion> Create(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Create(CapnpSerializable.Create<Mas.Rpc.Service.Factory<TPayload>.CreateParams>(d_), cancellationToken_), r_ =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Service.Factory<TPayload>.AccessInfo.WRITER>();
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> ServiceInterfaceNames(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.ServiceInterfaceNames(cancellationToken_), names =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Service.Factory<TPayload>.Result_ServiceInterfaceNames.WRITER>();
                    var r_ = new Mas.Rpc.Service.Factory<TPayload>.Result_ServiceInterfaceNames{Names = names};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class Factory<TPayload>
        where TPayload : class
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc2b88517ccaa9197UL)]
        public class CreateParams : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc2b88517ccaa9197UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                TimeoutSeconds = reader.TimeoutSeconds;
                InterfaceNameToRegistrySR = reader.InterfaceNameToRegistrySR?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Rpc.Common.Pair<string, string>>(_));
                MsgPayload = CapnpSerializable.Create<TPayload>(reader.MsgPayload);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.TimeoutSeconds = TimeoutSeconds;
                writer.InterfaceNameToRegistrySR.Init(InterfaceNameToRegistrySR, (_s1, _v1) => _v1?.serialize(_s1));
                writer.MsgPayload.SetObject(MsgPayload);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public ulong TimeoutSeconds
            {
                get;
                set;
            }

            = 3600UL;
            public IReadOnlyList<Mas.Rpc.Common.Pair<string, string>> InterfaceNameToRegistrySR
            {
                get;
                set;
            }

            public TPayload MsgPayload
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
                public ulong TimeoutSeconds => ctx.ReadDataULong(0UL, 3600UL);
                public IReadOnlyList<Mas.Rpc.Common.Pair<string, string>.READER> InterfaceNameToRegistrySR => ctx.ReadList(0).Cast(Mas.Rpc.Common.Pair<string, string>.READER.create);
                public bool HasInterfaceNameToRegistrySR => ctx.IsStructFieldNonNull(0);
                public DeserializerState MsgPayload => ctx.StructReadPointer(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 2);
                }

                public ulong TimeoutSeconds
                {
                    get => this.ReadDataULong(0UL, 3600UL);
                    set => this.WriteData(0UL, value, 3600UL);
                }

                public ListOfStructsSerializer<Mas.Rpc.Common.Pair<string, string>.WRITER> InterfaceNameToRegistrySR
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Rpc.Common.Pair<string, string>.WRITER>>(0);
                    set => Link(0, value);
                }

                public DynamicSerializerState MsgPayload
                {
                    get => BuildPointer<DynamicSerializerState>(1);
                    set => Link(1, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb9816a53df7cb62eUL)]
        public class AccessInfo : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb9816a53df7cb62eUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                AdminCap = reader.AdminCap;
                ServiceCaps = reader.ServiceCaps;
                Error = reader.Error;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.AdminCap = AdminCap;
                writer.ServiceCaps.Init(ServiceCaps);
                writer.Error = Error;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public BareProxy AdminCap
            {
                get;
                set;
            }

            public IReadOnlyList<Mas.Rpc.Common.IIdentifiable> ServiceCaps
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
                public BareProxy AdminCap => ctx.ReadCap(0);
                public IReadOnlyList<Mas.Rpc.Common.IIdentifiable> ServiceCaps => ctx.ReadCapList<Mas.Rpc.Common.IIdentifiable>(1);
                public bool HasServiceCaps => ctx.IsStructFieldNonNull(1);
                public string Error => ctx.ReadText(2, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 3);
                }

                public BareProxy AdminCap
                {
                    get => ReadCap<BareProxy>(0);
                    set => LinkObject(0, value);
                }

                public ListOfCapsSerializer<Mas.Rpc.Common.IIdentifiable> ServiceCaps
                {
                    get => BuildPointer<ListOfCapsSerializer<Mas.Rpc.Common.IIdentifiable>>(1);
                    set => Link(1, value);
                }

                public string Error
                {
                    get => this.ReadText(2, null);
                    set => this.WriteText(2, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd4d567352ab3882aUL)]
        public class Params_ServiceInterfaceNames : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd4d567352ab3882aUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa962c127b5dccf05UL)]
        public class Result_ServiceInterfaceNames : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa962c127b5dccf05UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Names = reader.Names;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Names.Init(Names);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<string> Names
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
                public IReadOnlyList<string> Names => ctx.ReadList(0).CastText2();
                public bool HasNames => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfTextSerializer Names
                {
                    get => BuildPointer<ListOfTextSerializer>(0);
                    set => Link(0, value);
                }
            }
        }
    }
}