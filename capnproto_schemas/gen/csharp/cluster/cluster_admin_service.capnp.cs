using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Schema.Cluster
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf7485d56d6f20e7dUL)]
    public class Cluster : ICapnpSerializable
    {
        public const UInt64 typeId = 0xf7485d56d6f20e7dUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbf24278c65f633ceUL), Proxy(typeof(AdminMaster_Proxy)), Skeleton(typeof(AdminMaster_Skeleton))]
        public interface IAdminMaster : Mas.Schema.Common.IIdentifiable
        {
            Task<Mas.Schema.Common.ICallback> RegisterModelInstanceFactory(string aModelId, Mas.Schema.Cluster.Cluster.IModelInstanceFactory aFactory, CancellationToken cancellationToken_ = default);
            Task<IReadOnlyList<Mas.Schema.Cluster.Cluster.IModelInstanceFactory>> AvailableModels(CancellationToken cancellationToken_ = default);
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbf24278c65f633ceUL)]
        public class AdminMaster_Proxy : Proxy, IAdminMaster
        {
            public Task<Mas.Schema.Common.ICallback> RegisterModelInstanceFactory(string aModelId, Mas.Schema.Cluster.Cluster.IModelInstanceFactory aFactory, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.AdminMaster.Params_RegisterModelInstanceFactory.WRITER>();
                var arg_ = new Mas.Schema.Cluster.Cluster.AdminMaster.Params_RegisterModelInstanceFactory()
                {AModelId = aModelId, AFactory = aFactory};
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(13773177044365358030UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.AdminMaster.Result_RegisterModelInstanceFactory>(d_);
                        return (r_.Unregister);
                    }
                }

                );
            }

            public Task<IReadOnlyList<Mas.Schema.Cluster.Cluster.IModelInstanceFactory>> AvailableModels(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.AdminMaster.Params_AvailableModels.WRITER>();
                var arg_ = new Mas.Schema.Cluster.Cluster.AdminMaster.Params_AvailableModels()
                {};
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(13773177044365358030UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.AdminMaster.Result_AvailableModels>(d_);
                        return (r_.Factories);
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbf24278c65f633ceUL)]
        public class AdminMaster_Skeleton : Skeleton<IAdminMaster>
        {
            public AdminMaster_Skeleton()
            {
                SetMethodTable(RegisterModelInstanceFactory, AvailableModels);
            }

            public override ulong InterfaceId => 13773177044365358030UL;
            Task<AnswerOrCounterquestion> RegisterModelInstanceFactory(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.AdminMaster.Params_RegisterModelInstanceFactory>(d_);
                    return Impatient.MaybeTailCall(Impl.RegisterModelInstanceFactory(in_.AModelId, in_.AFactory, cancellationToken_), unregister =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.AdminMaster.Result_RegisterModelInstanceFactory.WRITER>();
                        var r_ = new Mas.Schema.Cluster.Cluster.AdminMaster.Result_RegisterModelInstanceFactory{Unregister = unregister};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> AvailableModels(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    return Impatient.MaybeTailCall(Impl.AvailableModels(cancellationToken_), factories =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.AdminMaster.Result_AvailableModels.WRITER>();
                        var r_ = new Mas.Schema.Cluster.Cluster.AdminMaster.Result_AvailableModels{Factories = factories};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }
        }

        public static class AdminMaster
        {
            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x943b54ee6f4de610UL)]
            public class Params_RegisterModelInstanceFactory : ICapnpSerializable
            {
                public const UInt64 typeId = 0x943b54ee6f4de610UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    AModelId = reader.AModelId;
                    AFactory = reader.AFactory;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.AModelId = AModelId;
                    writer.AFactory = AFactory;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public string AModelId
                {
                    get;
                    set;
                }

                public Mas.Schema.Cluster.Cluster.IModelInstanceFactory AFactory
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
                    public string AModelId => ctx.ReadText(0, null);
                    public Mas.Schema.Cluster.Cluster.IModelInstanceFactory AFactory => ctx.ReadCap<Mas.Schema.Cluster.Cluster.IModelInstanceFactory>(1);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 2);
                    }

                    public string AModelId
                    {
                        get => this.ReadText(0, null);
                        set => this.WriteText(0, value, null);
                    }

                    public Mas.Schema.Cluster.Cluster.IModelInstanceFactory AFactory
                    {
                        get => ReadCap<Mas.Schema.Cluster.Cluster.IModelInstanceFactory>(1);
                        set => LinkObject(1, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe7434f81e2b1e3deUL)]
            public class Result_RegisterModelInstanceFactory : ICapnpSerializable
            {
                public const UInt64 typeId = 0xe7434f81e2b1e3deUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Unregister = reader.Unregister;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Unregister = Unregister;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Schema.Common.ICallback Unregister
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
                    public Mas.Schema.Common.ICallback Unregister => ctx.ReadCap<Mas.Schema.Common.ICallback>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Schema.Common.ICallback Unregister
                    {
                        get => ReadCap<Mas.Schema.Common.ICallback>(0);
                        set => LinkObject(0, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa0669b656ba6cc8eUL)]
            public class Params_AvailableModels : ICapnpSerializable
            {
                public const UInt64 typeId = 0xa0669b656ba6cc8eUL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd4bece01a7c4008fUL)]
            public class Result_AvailableModels : ICapnpSerializable
            {
                public const UInt64 typeId = 0xd4bece01a7c4008fUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Factories = reader.Factories;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Factories.Init(Factories);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<Mas.Schema.Cluster.Cluster.IModelInstanceFactory> Factories
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
                    public IReadOnlyList<Mas.Schema.Cluster.Cluster.IModelInstanceFactory> Factories => ctx.ReadCapList<Mas.Schema.Cluster.Cluster.IModelInstanceFactory>(0);
                    public bool HasFactories => ctx.IsStructFieldNonNull(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfCapsSerializer<Mas.Schema.Cluster.Cluster.IModelInstanceFactory> Factories
                    {
                        get => BuildPointer<ListOfCapsSerializer<Mas.Schema.Cluster.Cluster.IModelInstanceFactory>>(0);
                        set => Link(0, value);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xec42c6df28354b60UL), Proxy(typeof(UserMaster_Proxy)), Skeleton(typeof(UserMaster_Skeleton))]
        public interface IUserMaster : Mas.Schema.Common.IIdentifiable
        {
            Task<IReadOnlyList<Mas.Schema.Cluster.Cluster.IModelInstanceFactory>> AvailableModels(CancellationToken cancellationToken_ = default);
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xec42c6df28354b60UL)]
        public class UserMaster_Proxy : Proxy, IUserMaster
        {
            public Task<IReadOnlyList<Mas.Schema.Cluster.Cluster.IModelInstanceFactory>> AvailableModels(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.UserMaster.Params_AvailableModels.WRITER>();
                var arg_ = new Mas.Schema.Cluster.Cluster.UserMaster.Params_AvailableModels()
                {};
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(17024388203168484192UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.UserMaster.Result_AvailableModels>(d_);
                        return (r_.Factories);
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xec42c6df28354b60UL)]
        public class UserMaster_Skeleton : Skeleton<IUserMaster>
        {
            public UserMaster_Skeleton()
            {
                SetMethodTable(AvailableModels);
            }

            public override ulong InterfaceId => 17024388203168484192UL;
            Task<AnswerOrCounterquestion> AvailableModels(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    return Impatient.MaybeTailCall(Impl.AvailableModels(cancellationToken_), factories =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.UserMaster.Result_AvailableModels.WRITER>();
                        var r_ = new Mas.Schema.Cluster.Cluster.UserMaster.Result_AvailableModels{Factories = factories};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }
        }

        public static class UserMaster
        {
            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9a80efc085eae065UL)]
            public class Params_AvailableModels : ICapnpSerializable
            {
                public const UInt64 typeId = 0x9a80efc085eae065UL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb147e4fbf7081bdaUL)]
            public class Result_AvailableModels : ICapnpSerializable
            {
                public const UInt64 typeId = 0xb147e4fbf7081bdaUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Factories = reader.Factories;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Factories.Init(Factories);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<Mas.Schema.Cluster.Cluster.IModelInstanceFactory> Factories
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
                    public IReadOnlyList<Mas.Schema.Cluster.Cluster.IModelInstanceFactory> Factories => ctx.ReadCapList<Mas.Schema.Cluster.Cluster.IModelInstanceFactory>(0);
                    public bool HasFactories => ctx.IsStructFieldNonNull(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfCapsSerializer<Mas.Schema.Cluster.Cluster.IModelInstanceFactory> Factories
                    {
                        get => BuildPointer<ListOfCapsSerializer<Mas.Schema.Cluster.Cluster.IModelInstanceFactory>>(0);
                        set => Link(0, value);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf849848fea5c4776UL), Proxy(typeof(Runtime_Proxy)), Skeleton(typeof(Runtime_Skeleton))]
        public interface IRuntime : Mas.Schema.Common.IIdentifiable
        {
            Task<Mas.Schema.Common.ICallback> RegisterModelInstanceFactory(string aModelId, Mas.Schema.Cluster.Cluster.IModelInstanceFactory aFactory, CancellationToken cancellationToken_ = default);
            Task<IReadOnlyList<Mas.Schema.Cluster.Cluster.IModelInstanceFactory>> AvailableModels(CancellationToken cancellationToken_ = default);
            Task<short> NumberOfCores(CancellationToken cancellationToken_ = default);
            Task<short> FreeNumberOfCores(CancellationToken cancellationToken_ = default);
            Task<short> ReserveNumberOfCores(short reserveCores, string aModelId, CancellationToken cancellationToken_ = default);
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf849848fea5c4776UL)]
        public class Runtime_Proxy : Proxy, IRuntime
        {
            public Task<Mas.Schema.Common.ICallback> RegisterModelInstanceFactory(string aModelId, Mas.Schema.Cluster.Cluster.IModelInstanceFactory aFactory, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.Runtime.Params_RegisterModelInstanceFactory.WRITER>();
                var arg_ = new Mas.Schema.Cluster.Cluster.Runtime.Params_RegisterModelInstanceFactory()
                {AModelId = aModelId, AFactory = aFactory};
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(17890976748353111926UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.Runtime.Result_RegisterModelInstanceFactory>(d_);
                        return (r_.Unregister);
                    }
                }

                );
            }

            public Task<IReadOnlyList<Mas.Schema.Cluster.Cluster.IModelInstanceFactory>> AvailableModels(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.Runtime.Params_AvailableModels.WRITER>();
                var arg_ = new Mas.Schema.Cluster.Cluster.Runtime.Params_AvailableModels()
                {};
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(17890976748353111926UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.Runtime.Result_AvailableModels>(d_);
                        return (r_.Factories);
                    }
                }

                );
            }

            public async Task<short> NumberOfCores(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.Runtime.Params_NumberOfCores.WRITER>();
                var arg_ = new Mas.Schema.Cluster.Cluster.Runtime.Params_NumberOfCores()
                {};
                arg_?.serialize(in_);
                using (var d_ = await Call(17890976748353111926UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.Runtime.Result_NumberOfCores>(d_);
                    return (r_.Cores);
                }
            }

            public async Task<short> FreeNumberOfCores(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.Runtime.Params_FreeNumberOfCores.WRITER>();
                var arg_ = new Mas.Schema.Cluster.Cluster.Runtime.Params_FreeNumberOfCores()
                {};
                arg_?.serialize(in_);
                using (var d_ = await Call(17890976748353111926UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.Runtime.Result_FreeNumberOfCores>(d_);
                    return (r_.Cores);
                }
            }

            public async Task<short> ReserveNumberOfCores(short reserveCores, string aModelId, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.Runtime.Params_ReserveNumberOfCores.WRITER>();
                var arg_ = new Mas.Schema.Cluster.Cluster.Runtime.Params_ReserveNumberOfCores()
                {ReserveCores = reserveCores, AModelId = aModelId};
                arg_?.serialize(in_);
                using (var d_ = await Call(17890976748353111926UL, 4, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.Runtime.Result_ReserveNumberOfCores>(d_);
                    return (r_.ReservedCores);
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf849848fea5c4776UL)]
        public class Runtime_Skeleton : Skeleton<IRuntime>
        {
            public Runtime_Skeleton()
            {
                SetMethodTable(RegisterModelInstanceFactory, AvailableModels, NumberOfCores, FreeNumberOfCores, ReserveNumberOfCores);
            }

            public override ulong InterfaceId => 17890976748353111926UL;
            Task<AnswerOrCounterquestion> RegisterModelInstanceFactory(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.Runtime.Params_RegisterModelInstanceFactory>(d_);
                    return Impatient.MaybeTailCall(Impl.RegisterModelInstanceFactory(in_.AModelId, in_.AFactory, cancellationToken_), unregister =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.Runtime.Result_RegisterModelInstanceFactory.WRITER>();
                        var r_ = new Mas.Schema.Cluster.Cluster.Runtime.Result_RegisterModelInstanceFactory{Unregister = unregister};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> AvailableModels(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    return Impatient.MaybeTailCall(Impl.AvailableModels(cancellationToken_), factories =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.Runtime.Result_AvailableModels.WRITER>();
                        var r_ = new Mas.Schema.Cluster.Cluster.Runtime.Result_AvailableModels{Factories = factories};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> NumberOfCores(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    return Impatient.MaybeTailCall(Impl.NumberOfCores(cancellationToken_), cores =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.Runtime.Result_NumberOfCores.WRITER>();
                        var r_ = new Mas.Schema.Cluster.Cluster.Runtime.Result_NumberOfCores{Cores = cores};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> FreeNumberOfCores(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    return Impatient.MaybeTailCall(Impl.FreeNumberOfCores(cancellationToken_), cores =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.Runtime.Result_FreeNumberOfCores.WRITER>();
                        var r_ = new Mas.Schema.Cluster.Cluster.Runtime.Result_FreeNumberOfCores{Cores = cores};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> ReserveNumberOfCores(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.Runtime.Params_ReserveNumberOfCores>(d_);
                    return Impatient.MaybeTailCall(Impl.ReserveNumberOfCores(in_.ReserveCores, in_.AModelId, cancellationToken_), reservedCores =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.Runtime.Result_ReserveNumberOfCores.WRITER>();
                        var r_ = new Mas.Schema.Cluster.Cluster.Runtime.Result_ReserveNumberOfCores{ReservedCores = reservedCores};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }
        }

        public static class Runtime
        {
            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc3668a8f7946ce88UL)]
            public class Params_RegisterModelInstanceFactory : ICapnpSerializable
            {
                public const UInt64 typeId = 0xc3668a8f7946ce88UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    AModelId = reader.AModelId;
                    AFactory = reader.AFactory;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.AModelId = AModelId;
                    writer.AFactory = AFactory;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public string AModelId
                {
                    get;
                    set;
                }

                public Mas.Schema.Cluster.Cluster.IModelInstanceFactory AFactory
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
                    public string AModelId => ctx.ReadText(0, null);
                    public Mas.Schema.Cluster.Cluster.IModelInstanceFactory AFactory => ctx.ReadCap<Mas.Schema.Cluster.Cluster.IModelInstanceFactory>(1);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 2);
                    }

                    public string AModelId
                    {
                        get => this.ReadText(0, null);
                        set => this.WriteText(0, value, null);
                    }

                    public Mas.Schema.Cluster.Cluster.IModelInstanceFactory AFactory
                    {
                        get => ReadCap<Mas.Schema.Cluster.Cluster.IModelInstanceFactory>(1);
                        set => LinkObject(1, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa8dfab7b88664bd4UL)]
            public class Result_RegisterModelInstanceFactory : ICapnpSerializable
            {
                public const UInt64 typeId = 0xa8dfab7b88664bd4UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Unregister = reader.Unregister;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Unregister = Unregister;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Schema.Common.ICallback Unregister
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
                    public Mas.Schema.Common.ICallback Unregister => ctx.ReadCap<Mas.Schema.Common.ICallback>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Schema.Common.ICallback Unregister
                    {
                        get => ReadCap<Mas.Schema.Common.ICallback>(0);
                        set => LinkObject(0, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfe35aabe121add1aUL)]
            public class Params_AvailableModels : ICapnpSerializable
            {
                public const UInt64 typeId = 0xfe35aabe121add1aUL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x93bdb3f5b6eecd29UL)]
            public class Result_AvailableModels : ICapnpSerializable
            {
                public const UInt64 typeId = 0x93bdb3f5b6eecd29UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Factories = reader.Factories;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Factories.Init(Factories);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<Mas.Schema.Cluster.Cluster.IModelInstanceFactory> Factories
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
                    public IReadOnlyList<Mas.Schema.Cluster.Cluster.IModelInstanceFactory> Factories => ctx.ReadCapList<Mas.Schema.Cluster.Cluster.IModelInstanceFactory>(0);
                    public bool HasFactories => ctx.IsStructFieldNonNull(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfCapsSerializer<Mas.Schema.Cluster.Cluster.IModelInstanceFactory> Factories
                    {
                        get => BuildPointer<ListOfCapsSerializer<Mas.Schema.Cluster.Cluster.IModelInstanceFactory>>(0);
                        set => Link(0, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9b3d2c0c5054766cUL)]
            public class Params_NumberOfCores : ICapnpSerializable
            {
                public const UInt64 typeId = 0x9b3d2c0c5054766cUL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe6b2589f9a250d7fUL)]
            public class Result_NumberOfCores : ICapnpSerializable
            {
                public const UInt64 typeId = 0xe6b2589f9a250d7fUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Cores = reader.Cores;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Cores = Cores;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public short Cores
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
                    public short Cores => ctx.ReadDataShort(0UL, (short)0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 0);
                    }

                    public short Cores
                    {
                        get => this.ReadDataShort(0UL, (short)0);
                        set => this.WriteData(0UL, value, (short)0);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc224b7ff6089b64eUL)]
            public class Params_FreeNumberOfCores : ICapnpSerializable
            {
                public const UInt64 typeId = 0xc224b7ff6089b64eUL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf004ae32302172c6UL)]
            public class Result_FreeNumberOfCores : ICapnpSerializable
            {
                public const UInt64 typeId = 0xf004ae32302172c6UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Cores = reader.Cores;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Cores = Cores;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public short Cores
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
                    public short Cores => ctx.ReadDataShort(0UL, (short)0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 0);
                    }

                    public short Cores
                    {
                        get => this.ReadDataShort(0UL, (short)0);
                        set => this.WriteData(0UL, value, (short)0);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb4d00b302a119de9UL)]
            public class Params_ReserveNumberOfCores : ICapnpSerializable
            {
                public const UInt64 typeId = 0xb4d00b302a119de9UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    ReserveCores = reader.ReserveCores;
                    AModelId = reader.AModelId;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.ReserveCores = ReserveCores;
                    writer.AModelId = AModelId;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public short ReserveCores
                {
                    get;
                    set;
                }

                public string AModelId
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
                    public short ReserveCores => ctx.ReadDataShort(0UL, (short)0);
                    public string AModelId => ctx.ReadText(0, null);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 1);
                    }

                    public short ReserveCores
                    {
                        get => this.ReadDataShort(0UL, (short)0);
                        set => this.WriteData(0UL, value, (short)0);
                    }

                    public string AModelId
                    {
                        get => this.ReadText(0, null);
                        set => this.WriteText(0, value, null);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbcd8dd8cea624cbbUL)]
            public class Result_ReserveNumberOfCores : ICapnpSerializable
            {
                public const UInt64 typeId = 0xbcd8dd8cea624cbbUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    ReservedCores = reader.ReservedCores;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.ReservedCores = ReservedCores;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public short ReservedCores
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
                    public short ReservedCores => ctx.ReadDataShort(0UL, (short)0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 0);
                    }

                    public short ReservedCores
                    {
                        get => this.ReadDataShort(0UL, (short)0);
                        set => this.WriteData(0UL, value, (short)0);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfd9959998f9f0ebeUL), Proxy(typeof(ModelInstanceFactory_Proxy)), Skeleton(typeof(ModelInstanceFactory_Skeleton))]
        public interface IModelInstanceFactory : Mas.Schema.Common.IIdentifiable
        {
            Task<Mas.Schema.Common.ICapHolder<object>> NewInstance(CancellationToken cancellationToken_ = default);
            Task<Mas.Schema.Common.ICapHolder<IReadOnlyList<Mas.Schema.Common.ListEntry<Mas.Schema.Common.ICapHolder<object>>>>> NewInstances(short numberOfInstances, CancellationToken cancellationToken_ = default);
            Task<Mas.Schema.Common.ICapHolder<Mas.Schema.Common.ZmqPipelineAddresses>> NewCloudViaZmqPipelineProxies(short numberOfInstances, CancellationToken cancellationToken_ = default);
            Task<Mas.Schema.Common.ICapHolder<object>> NewCloudViaProxy(short numberOfInstances, CancellationToken cancellationToken_ = default);
            Task<string> ModelId(CancellationToken cancellationToken_ = default);
            Task<Mas.Schema.Common.ICallback> RegisterModelInstance(BareProxy instance, string registrationToken, CancellationToken cancellationToken_ = default);
            Task<Mas.Schema.Common.ICapHolder<object>> RestoreSturdyRef(string sturdyRef, CancellationToken cancellationToken_ = default);
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfd9959998f9f0ebeUL)]
        public class ModelInstanceFactory_Proxy : Proxy, IModelInstanceFactory
        {
            public Task<Mas.Schema.Common.ICapHolder<object>> NewInstance(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Params_NewInstance.WRITER>();
                var arg_ = new Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Params_NewInstance()
                {};
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(18273735479106932414UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_NewInstance>(d_);
                        return (r_.Instance);
                    }
                }

                );
            }

            public Task<Mas.Schema.Common.ICapHolder<IReadOnlyList<Mas.Schema.Common.ListEntry<Mas.Schema.Common.ICapHolder<object>>>>> NewInstances(short numberOfInstances, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Params_NewInstances.WRITER>();
                var arg_ = new Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Params_NewInstances()
                {NumberOfInstances = numberOfInstances};
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(18273735479106932414UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_NewInstances>(d_);
                        return (r_.Instances);
                    }
                }

                );
            }

            public Task<Mas.Schema.Common.ICapHolder<Mas.Schema.Common.ZmqPipelineAddresses>> NewCloudViaZmqPipelineProxies(short numberOfInstances, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Params_NewCloudViaZmqPipelineProxies.WRITER>();
                var arg_ = new Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Params_NewCloudViaZmqPipelineProxies()
                {NumberOfInstances = numberOfInstances};
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(18273735479106932414UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_NewCloudViaZmqPipelineProxies>(d_);
                        return (r_.ProxyAddresses);
                    }
                }

                );
            }

            public Task<Mas.Schema.Common.ICapHolder<object>> NewCloudViaProxy(short numberOfInstances, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Params_NewCloudViaProxy.WRITER>();
                var arg_ = new Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Params_NewCloudViaProxy()
                {NumberOfInstances = numberOfInstances};
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(18273735479106932414UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_NewCloudViaProxy>(d_);
                        return (r_.Proxy);
                    }
                }

                );
            }

            public async Task<string> ModelId(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Params_ModelId.WRITER>();
                var arg_ = new Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Params_ModelId()
                {};
                arg_?.serialize(in_);
                using (var d_ = await Call(18273735479106932414UL, 4, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_ModelId>(d_);
                    return (r_.Id);
                }
            }

            public Task<Mas.Schema.Common.ICallback> RegisterModelInstance(BareProxy instance, string registrationToken, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Params_RegisterModelInstance.WRITER>();
                var arg_ = new Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Params_RegisterModelInstance()
                {Instance = instance, RegistrationToken = registrationToken};
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(18273735479106932414UL, 5, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_RegisterModelInstance>(d_);
                        return (r_.Unregister);
                    }
                }

                );
            }

            public Task<Mas.Schema.Common.ICapHolder<object>> RestoreSturdyRef(string sturdyRef, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Params_RestoreSturdyRef.WRITER>();
                var arg_ = new Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Params_RestoreSturdyRef()
                {SturdyRef = sturdyRef};
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(18273735479106932414UL, 6, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_RestoreSturdyRef>(d_);
                        return (r_.Cap);
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfd9959998f9f0ebeUL)]
        public class ModelInstanceFactory_Skeleton : Skeleton<IModelInstanceFactory>
        {
            public ModelInstanceFactory_Skeleton()
            {
                SetMethodTable(NewInstance, NewInstances, NewCloudViaZmqPipelineProxies, NewCloudViaProxy, ModelId, RegisterModelInstance, RestoreSturdyRef);
            }

            public override ulong InterfaceId => 18273735479106932414UL;
            Task<AnswerOrCounterquestion> NewInstance(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    return Impatient.MaybeTailCall(Impl.NewInstance(cancellationToken_), instance =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_NewInstance.WRITER>();
                        var r_ = new Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_NewInstance{Instance = instance};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> NewInstances(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Params_NewInstances>(d_);
                    return Impatient.MaybeTailCall(Impl.NewInstances(in_.NumberOfInstances, cancellationToken_), instances =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_NewInstances.WRITER>();
                        var r_ = new Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_NewInstances{Instances = instances};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> NewCloudViaZmqPipelineProxies(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Params_NewCloudViaZmqPipelineProxies>(d_);
                    return Impatient.MaybeTailCall(Impl.NewCloudViaZmqPipelineProxies(in_.NumberOfInstances, cancellationToken_), proxyAddresses =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_NewCloudViaZmqPipelineProxies.WRITER>();
                        var r_ = new Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_NewCloudViaZmqPipelineProxies{ProxyAddresses = proxyAddresses};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> NewCloudViaProxy(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Params_NewCloudViaProxy>(d_);
                    return Impatient.MaybeTailCall(Impl.NewCloudViaProxy(in_.NumberOfInstances, cancellationToken_), proxy =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_NewCloudViaProxy.WRITER>();
                        var r_ = new Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_NewCloudViaProxy{Proxy = proxy};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> ModelId(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    return Impatient.MaybeTailCall(Impl.ModelId(cancellationToken_), id =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_ModelId.WRITER>();
                        var r_ = new Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_ModelId{Id = id};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> RegisterModelInstance(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Params_RegisterModelInstance>(d_);
                    return Impatient.MaybeTailCall(Impl.RegisterModelInstance(in_.Instance, in_.RegistrationToken, cancellationToken_), unregister =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_RegisterModelInstance.WRITER>();
                        var r_ = new Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_RegisterModelInstance{Unregister = unregister};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> RestoreSturdyRef(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Params_RestoreSturdyRef>(d_);
                    return Impatient.MaybeTailCall(Impl.RestoreSturdyRef(in_.SturdyRef, cancellationToken_), cap =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_RestoreSturdyRef.WRITER>();
                        var r_ = new Mas.Schema.Cluster.Cluster.ModelInstanceFactory.Result_RestoreSturdyRef{Cap = cap};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }
        }

        public static class ModelInstanceFactory
        {
            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8bf81264d2f11274UL)]
            public class Params_NewInstance : ICapnpSerializable
            {
                public const UInt64 typeId = 0x8bf81264d2f11274UL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf468b1dc515f841cUL)]
            public class Result_NewInstance : ICapnpSerializable
            {
                public const UInt64 typeId = 0xf468b1dc515f841cUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Instance = reader.Instance;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Instance = Instance;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Schema.Common.ICapHolder<object> Instance
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
                    public Mas.Schema.Common.ICapHolder<object> Instance => ctx.ReadCap<Mas.Schema.Common.ICapHolder<object>>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Schema.Common.ICapHolder<object> Instance
                    {
                        get => ReadCap<Mas.Schema.Common.ICapHolder<object>>(0);
                        set => LinkObject(0, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x985d83a2e2d7e204UL)]
            public class Params_NewInstances : ICapnpSerializable
            {
                public const UInt64 typeId = 0x985d83a2e2d7e204UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    NumberOfInstances = reader.NumberOfInstances;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.NumberOfInstances = NumberOfInstances;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public short NumberOfInstances
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
                    public short NumberOfInstances => ctx.ReadDataShort(0UL, (short)0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 0);
                    }

                    public short NumberOfInstances
                    {
                        get => this.ReadDataShort(0UL, (short)0);
                        set => this.WriteData(0UL, value, (short)0);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbcacf6dde70da193UL)]
            public class Result_NewInstances : ICapnpSerializable
            {
                public const UInt64 typeId = 0xbcacf6dde70da193UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Instances = reader.Instances;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Instances = Instances;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Schema.Common.ICapHolder<IReadOnlyList<Mas.Schema.Common.ListEntry<Mas.Schema.Common.ICapHolder<object>>>> Instances
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
                    public Mas.Schema.Common.ICapHolder<IReadOnlyList<Mas.Schema.Common.ListEntry<Mas.Schema.Common.ICapHolder<object>>>> Instances => ctx.ReadCap<Mas.Schema.Common.ICapHolder<IReadOnlyList<Mas.Schema.Common.ListEntry<Mas.Schema.Common.ICapHolder<object>>>>>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Schema.Common.ICapHolder<IReadOnlyList<Mas.Schema.Common.ListEntry<Mas.Schema.Common.ICapHolder<object>>>> Instances
                    {
                        get => ReadCap<Mas.Schema.Common.ICapHolder<IReadOnlyList<Mas.Schema.Common.ListEntry<Mas.Schema.Common.ICapHolder<object>>>>>(0);
                        set => LinkObject(0, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8b5d8251cf57c316UL)]
            public class Params_NewCloudViaZmqPipelineProxies : ICapnpSerializable
            {
                public const UInt64 typeId = 0x8b5d8251cf57c316UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    NumberOfInstances = reader.NumberOfInstances;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.NumberOfInstances = NumberOfInstances;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public short NumberOfInstances
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
                    public short NumberOfInstances => ctx.ReadDataShort(0UL, (short)0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 0);
                    }

                    public short NumberOfInstances
                    {
                        get => this.ReadDataShort(0UL, (short)0);
                        set => this.WriteData(0UL, value, (short)0);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa81053c61d4d995cUL)]
            public class Result_NewCloudViaZmqPipelineProxies : ICapnpSerializable
            {
                public const UInt64 typeId = 0xa81053c61d4d995cUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    ProxyAddresses = reader.ProxyAddresses;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.ProxyAddresses = ProxyAddresses;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Schema.Common.ICapHolder<Mas.Schema.Common.ZmqPipelineAddresses> ProxyAddresses
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
                    public Mas.Schema.Common.ICapHolder<Mas.Schema.Common.ZmqPipelineAddresses> ProxyAddresses => ctx.ReadCap<Mas.Schema.Common.ICapHolder<Mas.Schema.Common.ZmqPipelineAddresses>>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Schema.Common.ICapHolder<Mas.Schema.Common.ZmqPipelineAddresses> ProxyAddresses
                    {
                        get => ReadCap<Mas.Schema.Common.ICapHolder<Mas.Schema.Common.ZmqPipelineAddresses>>(0);
                        set => LinkObject(0, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfea4c3f998b67621UL)]
            public class Params_NewCloudViaProxy : ICapnpSerializable
            {
                public const UInt64 typeId = 0xfea4c3f998b67621UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    NumberOfInstances = reader.NumberOfInstances;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.NumberOfInstances = NumberOfInstances;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public short NumberOfInstances
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
                    public short NumberOfInstances => ctx.ReadDataShort(0UL, (short)0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 0);
                    }

                    public short NumberOfInstances
                    {
                        get => this.ReadDataShort(0UL, (short)0);
                        set => this.WriteData(0UL, value, (short)0);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbaf979a1f5673019UL)]
            public class Result_NewCloudViaProxy : ICapnpSerializable
            {
                public const UInt64 typeId = 0xbaf979a1f5673019UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Proxy = reader.Proxy;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Proxy = Proxy;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Schema.Common.ICapHolder<object> Proxy
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
                    public Mas.Schema.Common.ICapHolder<object> Proxy => ctx.ReadCap<Mas.Schema.Common.ICapHolder<object>>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Schema.Common.ICapHolder<object> Proxy
                    {
                        get => ReadCap<Mas.Schema.Common.ICapHolder<object>>(0);
                        set => LinkObject(0, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe4b6ea2bfbc474d8UL)]
            public class Params_ModelId : ICapnpSerializable
            {
                public const UInt64 typeId = 0xe4b6ea2bfbc474d8UL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe3cf5a40e703e6daUL)]
            public class Result_ModelId : ICapnpSerializable
            {
                public const UInt64 typeId = 0xe3cf5a40e703e6daUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Id = reader.Id;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Id = Id;
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
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public string Id
                    {
                        get => this.ReadText(0, null);
                        set => this.WriteText(0, value, null);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbea41d4487c101c4UL)]
            public class Params_RegisterModelInstance : ICapnpSerializable
            {
                public const UInt64 typeId = 0xbea41d4487c101c4UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Instance = reader.Instance;
                    RegistrationToken = reader.RegistrationToken;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Instance = Instance;
                    writer.RegistrationToken = RegistrationToken;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                    RegistrationToken = RegistrationToken ?? "";
                }

                public BareProxy Instance
                {
                    get;
                    set;
                }

                public string RegistrationToken
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
                    public BareProxy Instance => ctx.ReadCap(0);
                    public string RegistrationToken => ctx.ReadText(1, "");
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 2);
                    }

                    public BareProxy Instance
                    {
                        get => ReadCap<BareProxy>(0);
                        set => LinkObject(0, value);
                    }

                    public string RegistrationToken
                    {
                        get => this.ReadText(1, "");
                        set => this.WriteText(1, value, "");
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xca8fb2a4c16e5f08UL)]
            public class Result_RegisterModelInstance : ICapnpSerializable
            {
                public const UInt64 typeId = 0xca8fb2a4c16e5f08UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Unregister = reader.Unregister;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Unregister = Unregister;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Schema.Common.ICallback Unregister
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
                    public Mas.Schema.Common.ICallback Unregister => ctx.ReadCap<Mas.Schema.Common.ICallback>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Schema.Common.ICallback Unregister
                    {
                        get => ReadCap<Mas.Schema.Common.ICallback>(0);
                        set => LinkObject(0, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd88a3f78cce2bc7dUL)]
            public class Params_RestoreSturdyRef : ICapnpSerializable
            {
                public const UInt64 typeId = 0xd88a3f78cce2bc7dUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    SturdyRef = reader.SturdyRef;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.SturdyRef = SturdyRef;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public string SturdyRef
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
                    public string SturdyRef => ctx.ReadText(0, null);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public string SturdyRef
                    {
                        get => this.ReadText(0, null);
                        set => this.WriteText(0, value, null);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe5cdfbf0462c5cfdUL)]
            public class Result_RestoreSturdyRef : ICapnpSerializable
            {
                public const UInt64 typeId = 0xe5cdfbf0462c5cfdUL;
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

                public Mas.Schema.Common.ICapHolder<object> Cap
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
                    public Mas.Schema.Common.ICapHolder<object> Cap => ctx.ReadCap<Mas.Schema.Common.ICapHolder<object>>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Schema.Common.ICapHolder<object> Cap
                    {
                        get => ReadCap<Mas.Schema.Common.ICapHolder<object>>(0);
                        set => LinkObject(0, value);
                    }
                }
            }
        }
    }
}