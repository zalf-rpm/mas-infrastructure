using Capnp;
using Capnp.Rpc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Rpc
{
    [TypeId(0xf7485d56d6f20e7dUL)]
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

        [TypeId(0xbf24278c65f633ceUL), Proxy(typeof(AdminMaster_Proxy)), Skeleton(typeof(AdminMaster_Skeleton))]
        public interface IAdminMaster : Mas.Rpc.Common.IIdentifiable
        {
            Task<Mas.Rpc.Common.ICallback> RegisterModelInstanceFactory(string aModelId, Mas.Rpc.Cluster.IModelInstanceFactory aFactory, CancellationToken cancellationToken_ = default);
            Task<IReadOnlyList<Mas.Rpc.Cluster.IModelInstanceFactory>> AvailableModels(CancellationToken cancellationToken_ = default);
        }

        public class AdminMaster_Proxy : Proxy, IAdminMaster
        {
            public Task<Mas.Rpc.Common.ICallback> RegisterModelInstanceFactory(string aModelId, Mas.Rpc.Cluster.IModelInstanceFactory aFactory, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.AdminMaster.Params_registerModelInstanceFactory.WRITER>();
                var arg_ = new Mas.Rpc.Cluster.AdminMaster.Params_registerModelInstanceFactory()
                {AModelId = aModelId, AFactory = aFactory};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(13773177044365358030UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.Cluster.AdminMaster.Result_registerModelInstanceFactory>(d_);
                    return (r_.Unregister);
                }

                );
            }

            public Task<IReadOnlyList<Mas.Rpc.Cluster.IModelInstanceFactory>> AvailableModels(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.AdminMaster.Params_availableModels.WRITER>();
                var arg_ = new Mas.Rpc.Cluster.AdminMaster.Params_availableModels()
                {};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(13773177044365358030UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.Cluster.AdminMaster.Result_availableModels>(d_);
                    return (r_.Factories);
                }

                );
            }

            public async Task<Mas.Rpc.Common.IdInformation> Info(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Identifiable.Params_info.WRITER>();
                var arg_ = new Mas.Rpc.Common.Identifiable.Params_info()
                {};
                arg_.serialize(in_);
                var d_ = await Call(15298557119806335142UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.Common.Identifiable.Result_info>(d_);
                return (r_.Info);
            }
        }

        public class AdminMaster_Skeleton : Skeleton<IAdminMaster>
        {
            public AdminMaster_Skeleton()
            {
                SetMethodTable(RegisterModelInstanceFactory, AvailableModels);
            }

            public override ulong InterfaceId => 13773177044365358030UL;
            Task<AnswerOrCounterquestion> RegisterModelInstanceFactory(DeserializerState d_, CancellationToken cancellationToken_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.Cluster.AdminMaster.Params_registerModelInstanceFactory>(d_);
                return Impatient.MaybeTailCall(Impl.RegisterModelInstanceFactory(in_.AModelId, in_.AFactory, cancellationToken_), unregister =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.AdminMaster.Result_registerModelInstanceFactory.WRITER>();
                    var r_ = new Mas.Rpc.Cluster.AdminMaster.Result_registerModelInstanceFactory{Unregister = unregister};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> AvailableModels(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.AvailableModels(cancellationToken_), factories =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.AdminMaster.Result_availableModels.WRITER>();
                    var r_ = new Mas.Rpc.Cluster.AdminMaster.Result_availableModels{Factories = factories};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        public static class AdminMaster
        {
            [TypeId(0x943b54ee6f4de610UL)]
            public class Params_registerModelInstanceFactory : ICapnpSerializable
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

                public Mas.Rpc.Cluster.IModelInstanceFactory AFactory
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
                    public string AModelId => ctx.ReadText(0, "");
                    public Mas.Rpc.Cluster.IModelInstanceFactory AFactory => ctx.ReadCap<Mas.Rpc.Cluster.IModelInstanceFactory>(1);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 2);
                    }

                    public string AModelId
                    {
                        get => this.ReadText(0, "");
                        set => this.WriteText(0, value, "");
                    }

                    public Mas.Rpc.Cluster.IModelInstanceFactory AFactory
                    {
                        get => ReadCap<Mas.Rpc.Cluster.IModelInstanceFactory>(1);
                        set => LinkObject(1, value);
                    }
                }
            }

            [TypeId(0xe7434f81e2b1e3deUL)]
            public class Result_registerModelInstanceFactory : ICapnpSerializable
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

                public Mas.Rpc.Common.ICallback Unregister
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
                    public Mas.Rpc.Common.ICallback Unregister => ctx.ReadCap<Mas.Rpc.Common.ICallback>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Common.ICallback Unregister
                    {
                        get => ReadCap<Mas.Rpc.Common.ICallback>(0);
                        set => LinkObject(0, value);
                    }
                }
            }

            [TypeId(0xa0669b656ba6cc8eUL)]
            public class Params_availableModels : ICapnpSerializable
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

            [TypeId(0xd4bece01a7c4008fUL)]
            public class Result_availableModels : ICapnpSerializable
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

                public IReadOnlyList<Mas.Rpc.Cluster.IModelInstanceFactory> Factories
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
                    public IReadOnlyList<Mas.Rpc.Cluster.IModelInstanceFactory> Factories => ctx.ReadCapList<Mas.Rpc.Cluster.IModelInstanceFactory>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfCapsSerializer<Mas.Rpc.Cluster.IModelInstanceFactory> Factories
                    {
                        get => BuildPointer<ListOfCapsSerializer<Mas.Rpc.Cluster.IModelInstanceFactory>>(0);
                        set => Link(0, value);
                    }
                }
            }
        }

        [TypeId(0xec42c6df28354b60UL), Proxy(typeof(UserMaster_Proxy)), Skeleton(typeof(UserMaster_Skeleton))]
        public interface IUserMaster : Mas.Rpc.Common.IIdentifiable
        {
            Task<IReadOnlyList<Mas.Rpc.Cluster.IModelInstanceFactory>> AvailableModels(CancellationToken cancellationToken_ = default);
        }

        public class UserMaster_Proxy : Proxy, IUserMaster
        {
            public Task<IReadOnlyList<Mas.Rpc.Cluster.IModelInstanceFactory>> AvailableModels(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.UserMaster.Params_availableModels.WRITER>();
                var arg_ = new Mas.Rpc.Cluster.UserMaster.Params_availableModels()
                {};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(17024388203168484192UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.Cluster.UserMaster.Result_availableModels>(d_);
                    return (r_.Factories);
                }

                );
            }

            public async Task<Mas.Rpc.Common.IdInformation> Info(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Identifiable.Params_info.WRITER>();
                var arg_ = new Mas.Rpc.Common.Identifiable.Params_info()
                {};
                arg_.serialize(in_);
                var d_ = await Call(15298557119806335142UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.Common.Identifiable.Result_info>(d_);
                return (r_.Info);
            }
        }

        public class UserMaster_Skeleton : Skeleton<IUserMaster>
        {
            public UserMaster_Skeleton()
            {
                SetMethodTable(AvailableModels);
            }

            public override ulong InterfaceId => 17024388203168484192UL;
            Task<AnswerOrCounterquestion> AvailableModels(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.AvailableModels(cancellationToken_), factories =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.UserMaster.Result_availableModels.WRITER>();
                    var r_ = new Mas.Rpc.Cluster.UserMaster.Result_availableModels{Factories = factories};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        public static class UserMaster
        {
            [TypeId(0x9a80efc085eae065UL)]
            public class Params_availableModels : ICapnpSerializable
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

            [TypeId(0xb147e4fbf7081bdaUL)]
            public class Result_availableModels : ICapnpSerializable
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

                public IReadOnlyList<Mas.Rpc.Cluster.IModelInstanceFactory> Factories
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
                    public IReadOnlyList<Mas.Rpc.Cluster.IModelInstanceFactory> Factories => ctx.ReadCapList<Mas.Rpc.Cluster.IModelInstanceFactory>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfCapsSerializer<Mas.Rpc.Cluster.IModelInstanceFactory> Factories
                    {
                        get => BuildPointer<ListOfCapsSerializer<Mas.Rpc.Cluster.IModelInstanceFactory>>(0);
                        set => Link(0, value);
                    }
                }
            }
        }

        [TypeId(0xf849848fea5c4776UL), Proxy(typeof(Runtime_Proxy)), Skeleton(typeof(Runtime_Skeleton))]
        public interface IRuntime : Mas.Rpc.Common.IIdentifiable
        {
            Task<Mas.Rpc.Common.ICallback> RegisterModelInstanceFactory(string aModelId, Mas.Rpc.Cluster.IModelInstanceFactory aFactory, CancellationToken cancellationToken_ = default);
            Task<IReadOnlyList<Mas.Rpc.Cluster.IModelInstanceFactory>> AvailableModels(CancellationToken cancellationToken_ = default);
            Task<short> NumberOfCores(CancellationToken cancellationToken_ = default);
            Task<short> FreeNumberOfCores(CancellationToken cancellationToken_ = default);
            Task<short> ReserveNumberOfCores(short reserveCores, string aModelId, CancellationToken cancellationToken_ = default);
        }

        public class Runtime_Proxy : Proxy, IRuntime
        {
            public Task<Mas.Rpc.Common.ICallback> RegisterModelInstanceFactory(string aModelId, Mas.Rpc.Cluster.IModelInstanceFactory aFactory, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.Runtime.Params_registerModelInstanceFactory.WRITER>();
                var arg_ = new Mas.Rpc.Cluster.Runtime.Params_registerModelInstanceFactory()
                {AModelId = aModelId, AFactory = aFactory};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(17890976748353111926UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.Cluster.Runtime.Result_registerModelInstanceFactory>(d_);
                    return (r_.Unregister);
                }

                );
            }

            public Task<IReadOnlyList<Mas.Rpc.Cluster.IModelInstanceFactory>> AvailableModels(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.Runtime.Params_availableModels.WRITER>();
                var arg_ = new Mas.Rpc.Cluster.Runtime.Params_availableModels()
                {};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(17890976748353111926UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.Cluster.Runtime.Result_availableModels>(d_);
                    return (r_.Factories);
                }

                );
            }

            public async Task<short> NumberOfCores(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.Runtime.Params_numberOfCores.WRITER>();
                var arg_ = new Mas.Rpc.Cluster.Runtime.Params_numberOfCores()
                {};
                arg_.serialize(in_);
                var d_ = await Call(17890976748353111926UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.Cluster.Runtime.Result_numberOfCores>(d_);
                return (r_.Cores);
            }

            public async Task<short> FreeNumberOfCores(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.Runtime.Params_freeNumberOfCores.WRITER>();
                var arg_ = new Mas.Rpc.Cluster.Runtime.Params_freeNumberOfCores()
                {};
                arg_.serialize(in_);
                var d_ = await Call(17890976748353111926UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.Cluster.Runtime.Result_freeNumberOfCores>(d_);
                return (r_.Cores);
            }

            public async Task<short> ReserveNumberOfCores(short reserveCores, string aModelId, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.Runtime.Params_reserveNumberOfCores.WRITER>();
                var arg_ = new Mas.Rpc.Cluster.Runtime.Params_reserveNumberOfCores()
                {ReserveCores = reserveCores, AModelId = aModelId};
                arg_.serialize(in_);
                var d_ = await Call(17890976748353111926UL, 4, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.Cluster.Runtime.Result_reserveNumberOfCores>(d_);
                return (r_.ReservedCores);
            }

            public async Task<Mas.Rpc.Common.IdInformation> Info(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Identifiable.Params_info.WRITER>();
                var arg_ = new Mas.Rpc.Common.Identifiable.Params_info()
                {};
                arg_.serialize(in_);
                var d_ = await Call(15298557119806335142UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.Common.Identifiable.Result_info>(d_);
                return (r_.Info);
            }
        }

        public class Runtime_Skeleton : Skeleton<IRuntime>
        {
            public Runtime_Skeleton()
            {
                SetMethodTable(RegisterModelInstanceFactory, AvailableModels, NumberOfCores, FreeNumberOfCores, ReserveNumberOfCores);
            }

            public override ulong InterfaceId => 17890976748353111926UL;
            Task<AnswerOrCounterquestion> RegisterModelInstanceFactory(DeserializerState d_, CancellationToken cancellationToken_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.Cluster.Runtime.Params_registerModelInstanceFactory>(d_);
                return Impatient.MaybeTailCall(Impl.RegisterModelInstanceFactory(in_.AModelId, in_.AFactory, cancellationToken_), unregister =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.Runtime.Result_registerModelInstanceFactory.WRITER>();
                    var r_ = new Mas.Rpc.Cluster.Runtime.Result_registerModelInstanceFactory{Unregister = unregister};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> AvailableModels(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.AvailableModels(cancellationToken_), factories =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.Runtime.Result_availableModels.WRITER>();
                    var r_ = new Mas.Rpc.Cluster.Runtime.Result_availableModels{Factories = factories};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> NumberOfCores(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.NumberOfCores(cancellationToken_), cores =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.Runtime.Result_numberOfCores.WRITER>();
                    var r_ = new Mas.Rpc.Cluster.Runtime.Result_numberOfCores{Cores = cores};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> FreeNumberOfCores(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.FreeNumberOfCores(cancellationToken_), cores =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.Runtime.Result_freeNumberOfCores.WRITER>();
                    var r_ = new Mas.Rpc.Cluster.Runtime.Result_freeNumberOfCores{Cores = cores};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> ReserveNumberOfCores(DeserializerState d_, CancellationToken cancellationToken_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.Cluster.Runtime.Params_reserveNumberOfCores>(d_);
                return Impatient.MaybeTailCall(Impl.ReserveNumberOfCores(in_.ReserveCores, in_.AModelId, cancellationToken_), reservedCores =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.Runtime.Result_reserveNumberOfCores.WRITER>();
                    var r_ = new Mas.Rpc.Cluster.Runtime.Result_reserveNumberOfCores{ReservedCores = reservedCores};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        public static class Runtime
        {
            [TypeId(0xc3668a8f7946ce88UL)]
            public class Params_registerModelInstanceFactory : ICapnpSerializable
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

                public Mas.Rpc.Cluster.IModelInstanceFactory AFactory
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
                    public string AModelId => ctx.ReadText(0, "");
                    public Mas.Rpc.Cluster.IModelInstanceFactory AFactory => ctx.ReadCap<Mas.Rpc.Cluster.IModelInstanceFactory>(1);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 2);
                    }

                    public string AModelId
                    {
                        get => this.ReadText(0, "");
                        set => this.WriteText(0, value, "");
                    }

                    public Mas.Rpc.Cluster.IModelInstanceFactory AFactory
                    {
                        get => ReadCap<Mas.Rpc.Cluster.IModelInstanceFactory>(1);
                        set => LinkObject(1, value);
                    }
                }
            }

            [TypeId(0xa8dfab7b88664bd4UL)]
            public class Result_registerModelInstanceFactory : ICapnpSerializable
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

                public Mas.Rpc.Common.ICallback Unregister
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
                    public Mas.Rpc.Common.ICallback Unregister => ctx.ReadCap<Mas.Rpc.Common.ICallback>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Common.ICallback Unregister
                    {
                        get => ReadCap<Mas.Rpc.Common.ICallback>(0);
                        set => LinkObject(0, value);
                    }
                }
            }

            [TypeId(0xfe35aabe121add1aUL)]
            public class Params_availableModels : ICapnpSerializable
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

            [TypeId(0x93bdb3f5b6eecd29UL)]
            public class Result_availableModels : ICapnpSerializable
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

                public IReadOnlyList<Mas.Rpc.Cluster.IModelInstanceFactory> Factories
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
                    public IReadOnlyList<Mas.Rpc.Cluster.IModelInstanceFactory> Factories => ctx.ReadCapList<Mas.Rpc.Cluster.IModelInstanceFactory>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfCapsSerializer<Mas.Rpc.Cluster.IModelInstanceFactory> Factories
                    {
                        get => BuildPointer<ListOfCapsSerializer<Mas.Rpc.Cluster.IModelInstanceFactory>>(0);
                        set => Link(0, value);
                    }
                }
            }

            [TypeId(0x9b3d2c0c5054766cUL)]
            public class Params_numberOfCores : ICapnpSerializable
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

            [TypeId(0xe6b2589f9a250d7fUL)]
            public class Result_numberOfCores : ICapnpSerializable
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

            [TypeId(0xc224b7ff6089b64eUL)]
            public class Params_freeNumberOfCores : ICapnpSerializable
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

            [TypeId(0xf004ae32302172c6UL)]
            public class Result_freeNumberOfCores : ICapnpSerializable
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

            [TypeId(0xb4d00b302a119de9UL)]
            public class Params_reserveNumberOfCores : ICapnpSerializable
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
                    public string AModelId => ctx.ReadText(0, "");
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
                        get => this.ReadText(0, "");
                        set => this.WriteText(0, value, "");
                    }
                }
            }

            [TypeId(0xbcd8dd8cea624cbbUL)]
            public class Result_reserveNumberOfCores : ICapnpSerializable
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

        [TypeId(0xfd9959998f9f0ebeUL), Proxy(typeof(ModelInstanceFactory_Proxy)), Skeleton(typeof(ModelInstanceFactory_Skeleton))]
        public interface IModelInstanceFactory : Mas.Rpc.Common.IIdentifiable
        {
            Task<Mas.Rpc.Common.ICapHolder<object>> NewInstance(CancellationToken cancellationToken_ = default);
            Task<Mas.Rpc.Common.ICapHolder<IReadOnlyList<Mas.Rpc.Common.ListEntry<Mas.Rpc.Common.ICapHolder<object>>>>> NewInstances(short numberOfInstances, CancellationToken cancellationToken_ = default);
            Task<Mas.Rpc.Common.ICapHolder<Mas.Rpc.Common.ZmqPipelineAddresses>> NewCloudViaZmqPipelineProxies(short numberOfInstances, CancellationToken cancellationToken_ = default);
            Task<Mas.Rpc.Common.ICapHolder<object>> NewCloudViaProxy(short numberOfInstances, CancellationToken cancellationToken_ = default);
            Task<string> ModelId(CancellationToken cancellationToken_ = default);
            Task<Mas.Rpc.Common.ICallback> RegisterModelInstance(BareProxy instance, string registrationToken, CancellationToken cancellationToken_ = default);
            Task<Mas.Rpc.Common.ICapHolder<object>> RestoreSturdyRef(string sturdyRef, CancellationToken cancellationToken_ = default);
        }

        public class ModelInstanceFactory_Proxy : Proxy, IModelInstanceFactory
        {
            public Task<Mas.Rpc.Common.ICapHolder<object>> NewInstance(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.ModelInstanceFactory.Params_newInstance.WRITER>();
                var arg_ = new Mas.Rpc.Cluster.ModelInstanceFactory.Params_newInstance()
                {};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(18273735479106932414UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.Cluster.ModelInstanceFactory.Result_newInstance>(d_);
                    return (r_.Instance);
                }

                );
            }

            public Task<Mas.Rpc.Common.ICapHolder<IReadOnlyList<Mas.Rpc.Common.ListEntry<Mas.Rpc.Common.ICapHolder<object>>>>> NewInstances(short numberOfInstances, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.ModelInstanceFactory.Params_newInstances.WRITER>();
                var arg_ = new Mas.Rpc.Cluster.ModelInstanceFactory.Params_newInstances()
                {NumberOfInstances = numberOfInstances};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(18273735479106932414UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.Cluster.ModelInstanceFactory.Result_newInstances>(d_);
                    return (r_.Instances);
                }

                );
            }

            public Task<Mas.Rpc.Common.ICapHolder<Mas.Rpc.Common.ZmqPipelineAddresses>> NewCloudViaZmqPipelineProxies(short numberOfInstances, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.ModelInstanceFactory.Params_newCloudViaZmqPipelineProxies.WRITER>();
                var arg_ = new Mas.Rpc.Cluster.ModelInstanceFactory.Params_newCloudViaZmqPipelineProxies()
                {NumberOfInstances = numberOfInstances};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(18273735479106932414UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.Cluster.ModelInstanceFactory.Result_newCloudViaZmqPipelineProxies>(d_);
                    return (r_.ProxyAddresses);
                }

                );
            }

            public Task<Mas.Rpc.Common.ICapHolder<object>> NewCloudViaProxy(short numberOfInstances, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.ModelInstanceFactory.Params_newCloudViaProxy.WRITER>();
                var arg_ = new Mas.Rpc.Cluster.ModelInstanceFactory.Params_newCloudViaProxy()
                {NumberOfInstances = numberOfInstances};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(18273735479106932414UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.Cluster.ModelInstanceFactory.Result_newCloudViaProxy>(d_);
                    return (r_.Proxy);
                }

                );
            }

            public async Task<string> ModelId(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.ModelInstanceFactory.Params_modelId.WRITER>();
                var arg_ = new Mas.Rpc.Cluster.ModelInstanceFactory.Params_modelId()
                {};
                arg_.serialize(in_);
                var d_ = await Call(18273735479106932414UL, 4, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.Cluster.ModelInstanceFactory.Result_modelId>(d_);
                return (r_.Id);
            }

            public Task<Mas.Rpc.Common.ICallback> RegisterModelInstance(BareProxy instance, string registrationToken, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.ModelInstanceFactory.Params_registerModelInstance.WRITER>();
                var arg_ = new Mas.Rpc.Cluster.ModelInstanceFactory.Params_registerModelInstance()
                {Instance = instance, RegistrationToken = registrationToken};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(18273735479106932414UL, 5, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.Cluster.ModelInstanceFactory.Result_registerModelInstance>(d_);
                    return (r_.Unregister);
                }

                );
            }

            public Task<Mas.Rpc.Common.ICapHolder<object>> RestoreSturdyRef(string sturdyRef, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.ModelInstanceFactory.Params_restoreSturdyRef.WRITER>();
                var arg_ = new Mas.Rpc.Cluster.ModelInstanceFactory.Params_restoreSturdyRef()
                {SturdyRef = sturdyRef};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(18273735479106932414UL, 6, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.Cluster.ModelInstanceFactory.Result_restoreSturdyRef>(d_);
                    return (r_.Cap);
                }

                );
            }

            public async Task<Mas.Rpc.Common.IdInformation> Info(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Identifiable.Params_info.WRITER>();
                var arg_ = new Mas.Rpc.Common.Identifiable.Params_info()
                {};
                arg_.serialize(in_);
                var d_ = await Call(15298557119806335142UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.Common.Identifiable.Result_info>(d_);
                return (r_.Info);
            }
        }

        public class ModelInstanceFactory_Skeleton : Skeleton<IModelInstanceFactory>
        {
            public ModelInstanceFactory_Skeleton()
            {
                SetMethodTable(NewInstance, NewInstances, NewCloudViaZmqPipelineProxies, NewCloudViaProxy, ModelId, RegisterModelInstance, RestoreSturdyRef);
            }

            public override ulong InterfaceId => 18273735479106932414UL;
            Task<AnswerOrCounterquestion> NewInstance(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.NewInstance(cancellationToken_), instance =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.ModelInstanceFactory.Result_newInstance.WRITER>();
                    var r_ = new Mas.Rpc.Cluster.ModelInstanceFactory.Result_newInstance{Instance = instance};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> NewInstances(DeserializerState d_, CancellationToken cancellationToken_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.Cluster.ModelInstanceFactory.Params_newInstances>(d_);
                return Impatient.MaybeTailCall(Impl.NewInstances(in_.NumberOfInstances, cancellationToken_), instances =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.ModelInstanceFactory.Result_newInstances.WRITER>();
                    var r_ = new Mas.Rpc.Cluster.ModelInstanceFactory.Result_newInstances{Instances = instances};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> NewCloudViaZmqPipelineProxies(DeserializerState d_, CancellationToken cancellationToken_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.Cluster.ModelInstanceFactory.Params_newCloudViaZmqPipelineProxies>(d_);
                return Impatient.MaybeTailCall(Impl.NewCloudViaZmqPipelineProxies(in_.NumberOfInstances, cancellationToken_), proxyAddresses =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.ModelInstanceFactory.Result_newCloudViaZmqPipelineProxies.WRITER>();
                    var r_ = new Mas.Rpc.Cluster.ModelInstanceFactory.Result_newCloudViaZmqPipelineProxies{ProxyAddresses = proxyAddresses};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> NewCloudViaProxy(DeserializerState d_, CancellationToken cancellationToken_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.Cluster.ModelInstanceFactory.Params_newCloudViaProxy>(d_);
                return Impatient.MaybeTailCall(Impl.NewCloudViaProxy(in_.NumberOfInstances, cancellationToken_), proxy =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.ModelInstanceFactory.Result_newCloudViaProxy.WRITER>();
                    var r_ = new Mas.Rpc.Cluster.ModelInstanceFactory.Result_newCloudViaProxy{Proxy = proxy};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> ModelId(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.ModelId(cancellationToken_), id =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.ModelInstanceFactory.Result_modelId.WRITER>();
                    var r_ = new Mas.Rpc.Cluster.ModelInstanceFactory.Result_modelId{Id = id};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> RegisterModelInstance(DeserializerState d_, CancellationToken cancellationToken_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.Cluster.ModelInstanceFactory.Params_registerModelInstance>(d_);
                return Impatient.MaybeTailCall(Impl.RegisterModelInstance(in_.Instance, in_.RegistrationToken, cancellationToken_), unregister =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.ModelInstanceFactory.Result_registerModelInstance.WRITER>();
                    var r_ = new Mas.Rpc.Cluster.ModelInstanceFactory.Result_registerModelInstance{Unregister = unregister};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> RestoreSturdyRef(DeserializerState d_, CancellationToken cancellationToken_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.Cluster.ModelInstanceFactory.Params_restoreSturdyRef>(d_);
                return Impatient.MaybeTailCall(Impl.RestoreSturdyRef(in_.SturdyRef, cancellationToken_), cap =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Cluster.ModelInstanceFactory.Result_restoreSturdyRef.WRITER>();
                    var r_ = new Mas.Rpc.Cluster.ModelInstanceFactory.Result_restoreSturdyRef{Cap = cap};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        public static class ModelInstanceFactory
        {
            [TypeId(0x8bf81264d2f11274UL)]
            public class Params_newInstance : ICapnpSerializable
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

            [TypeId(0xf468b1dc515f841cUL)]
            public class Result_newInstance : ICapnpSerializable
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

                public Mas.Rpc.Common.ICapHolder<object> Instance
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
                    public Mas.Rpc.Common.ICapHolder<object> Instance => ctx.ReadCap<Mas.Rpc.Common.ICapHolder<object>>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Common.ICapHolder<object> Instance
                    {
                        get => ReadCap<Mas.Rpc.Common.ICapHolder<object>>(0);
                        set => LinkObject(0, value);
                    }
                }
            }

            [TypeId(0x985d83a2e2d7e204UL)]
            public class Params_newInstances : ICapnpSerializable
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

            [TypeId(0xbcacf6dde70da193UL)]
            public class Result_newInstances : ICapnpSerializable
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

                public Mas.Rpc.Common.ICapHolder<IReadOnlyList<Mas.Rpc.Common.ListEntry<Mas.Rpc.Common.ICapHolder<object>>>> Instances
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
                    public Mas.Rpc.Common.ICapHolder<IReadOnlyList<Mas.Rpc.Common.ListEntry<Mas.Rpc.Common.ICapHolder<object>>>> Instances => ctx.ReadCap<Mas.Rpc.Common.ICapHolder<IReadOnlyList<Mas.Rpc.Common.ListEntry<Mas.Rpc.Common.ICapHolder<object>>>>>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Common.ICapHolder<IReadOnlyList<Mas.Rpc.Common.ListEntry<Mas.Rpc.Common.ICapHolder<object>>>> Instances
                    {
                        get => ReadCap<Mas.Rpc.Common.ICapHolder<IReadOnlyList<Mas.Rpc.Common.ListEntry<Mas.Rpc.Common.ICapHolder<object>>>>>(0);
                        set => LinkObject(0, value);
                    }
                }
            }

            [TypeId(0x8b5d8251cf57c316UL)]
            public class Params_newCloudViaZmqPipelineProxies : ICapnpSerializable
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

            [TypeId(0xa81053c61d4d995cUL)]
            public class Result_newCloudViaZmqPipelineProxies : ICapnpSerializable
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

                public Mas.Rpc.Common.ICapHolder<Mas.Rpc.Common.ZmqPipelineAddresses> ProxyAddresses
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
                    public Mas.Rpc.Common.ICapHolder<Mas.Rpc.Common.ZmqPipelineAddresses> ProxyAddresses => ctx.ReadCap<Mas.Rpc.Common.ICapHolder<Mas.Rpc.Common.ZmqPipelineAddresses>>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Common.ICapHolder<Mas.Rpc.Common.ZmqPipelineAddresses> ProxyAddresses
                    {
                        get => ReadCap<Mas.Rpc.Common.ICapHolder<Mas.Rpc.Common.ZmqPipelineAddresses>>(0);
                        set => LinkObject(0, value);
                    }
                }
            }

            [TypeId(0xfea4c3f998b67621UL)]
            public class Params_newCloudViaProxy : ICapnpSerializable
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

            [TypeId(0xbaf979a1f5673019UL)]
            public class Result_newCloudViaProxy : ICapnpSerializable
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

                public Mas.Rpc.Common.ICapHolder<object> Proxy
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
                    public Mas.Rpc.Common.ICapHolder<object> Proxy => ctx.ReadCap<Mas.Rpc.Common.ICapHolder<object>>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Common.ICapHolder<object> Proxy
                    {
                        get => ReadCap<Mas.Rpc.Common.ICapHolder<object>>(0);
                        set => LinkObject(0, value);
                    }
                }
            }

            [TypeId(0xe4b6ea2bfbc474d8UL)]
            public class Params_modelId : ICapnpSerializable
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

            [TypeId(0xe3cf5a40e703e6daUL)]
            public class Result_modelId : ICapnpSerializable
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
                    public string Id => ctx.ReadText(0, "");
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public string Id
                    {
                        get => this.ReadText(0, "");
                        set => this.WriteText(0, value, "");
                    }
                }
            }

            [TypeId(0xbea41d4487c101c4UL)]
            public class Params_registerModelInstance : ICapnpSerializable
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

            [TypeId(0xca8fb2a4c16e5f08UL)]
            public class Result_registerModelInstance : ICapnpSerializable
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

                public Mas.Rpc.Common.ICallback Unregister
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
                    public Mas.Rpc.Common.ICallback Unregister => ctx.ReadCap<Mas.Rpc.Common.ICallback>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Common.ICallback Unregister
                    {
                        get => ReadCap<Mas.Rpc.Common.ICallback>(0);
                        set => LinkObject(0, value);
                    }
                }
            }

            [TypeId(0xd88a3f78cce2bc7dUL)]
            public class Params_restoreSturdyRef : ICapnpSerializable
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
                    public string SturdyRef => ctx.ReadText(0, "");
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public string SturdyRef
                    {
                        get => this.ReadText(0, "");
                        set => this.WriteText(0, value, "");
                    }
                }
            }

            [TypeId(0xe5cdfbf0462c5cfdUL)]
            public class Result_restoreSturdyRef : ICapnpSerializable
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

                public Mas.Rpc.Common.ICapHolder<object> Cap
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
                    public Mas.Rpc.Common.ICapHolder<object> Cap => ctx.ReadCap<Mas.Rpc.Common.ICapHolder<object>>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Common.ICapHolder<object> Cap
                    {
                        get => ReadCap<Mas.Rpc.Common.ICapHolder<object>>(0);
                        set => LinkObject(0, value);
                    }
                }
            }
        }
    }
}