using Capnp;
using Capnp.Rpc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Rpc
{
    [TypeId(0xd89203b7b00a6d59UL)]
    public class Common : ICapnpSerializable
    {
        public const UInt64 typeId = 0xd89203b7b00a6d59UL;
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

        [TypeId(0xc1e88022f03bc355UL)]
        public class IdInformation : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc1e88022f03bc355UL;
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
                public string Id => ctx.ReadText(0, "");
                public string Name => ctx.ReadText(1, "");
                public string Description => ctx.ReadText(2, "");
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 3);
                }

                public string Id
                {
                    get => this.ReadText(0, "");
                    set => this.WriteText(0, value, "");
                }

                public string Name
                {
                    get => this.ReadText(1, "");
                    set => this.WriteText(1, value, "");
                }

                public string Description
                {
                    get => this.ReadText(2, "");
                    set => this.WriteText(2, value, "");
                }
            }
        }

        [TypeId(0xd44f64a52e1468a6UL), Proxy(typeof(Identifiable_Proxy)), Skeleton(typeof(Identifiable_Skeleton))]
        public interface IIdentifiable : IDisposable
        {
            Task<Mas.Rpc.Common.IdInformation> Info(CancellationToken cancellationToken_ = default);
        }

        public class Identifiable_Proxy : Proxy, IIdentifiable
        {
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

        public class Identifiable_Skeleton : Skeleton<IIdentifiable>
        {
            public Identifiable_Skeleton()
            {
                SetMethodTable(Info);
            }

            public override ulong InterfaceId => 15298557119806335142UL;
            Task<AnswerOrCounterquestion> Info(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.Info(cancellationToken_), info =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Identifiable.Result_info.WRITER>();
                    var r_ = new Mas.Rpc.Common.Identifiable.Result_info{Info = info};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        public static class Identifiable
        {
            [TypeId(0x9950d55529e91ba0UL)]
            public class Params_info : ICapnpSerializable
            {
                public const UInt64 typeId = 0x9950d55529e91ba0UL;
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

            [TypeId(0x8d7b2e2a4c01138aUL)]
            public class Result_info : ICapnpSerializable
            {
                public const UInt64 typeId = 0x8d7b2e2a4c01138aUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Info = CapnpSerializable.Create<Mas.Rpc.Common.IdInformation>(reader.Info);
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    Info?.serialize(writer.Info);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Rpc.Common.IdInformation Info
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
                    public Mas.Rpc.Common.IdInformation.READER Info => ctx.ReadStruct(0, Mas.Rpc.Common.IdInformation.READER.create);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Common.IdInformation.WRITER Info
                    {
                        get => BuildPointer<Mas.Rpc.Common.IdInformation.WRITER>(0);
                        set => Link(0, value);
                    }
                }
            }
        }

        [TypeId(0xa4bd27a85f562872UL)]
        public class Date : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa4bd27a85f562872UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Year = reader.Year;
                Month = reader.Month;
                Day = reader.Day;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Year = Year;
                writer.Month = Month;
                writer.Day = Day;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public short Year
            {
                get;
                set;
            }

            public byte Month
            {
                get;
                set;
            }

            public byte Day
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
                public short Year => ctx.ReadDataShort(0UL, (short)0);
                public byte Month => ctx.ReadDataByte(16UL, (byte)0);
                public byte Day => ctx.ReadDataByte(24UL, (byte)0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public short Year
                {
                    get => this.ReadDataShort(0UL, (short)0);
                    set => this.WriteData(0UL, value, (short)0);
                }

                public byte Month
                {
                    get => this.ReadDataByte(16UL, (byte)0);
                    set => this.WriteData(16UL, value, (byte)0);
                }

                public byte Day
                {
                    get => this.ReadDataByte(24UL, (byte)0);
                    set => this.WriteData(24UL, value, (byte)0);
                }
            }
        }

        [TypeId(0xbed10b06f1b79eb3UL)]
        public class StructuredText : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbed10b06f1b79eb3UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Value = reader.Value;
                Structure = CapnpSerializable.Create<Mas.Rpc.Common.StructuredText.@structure>(reader.Structure);
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

            public Mas.Rpc.Common.StructuredText.@structure Structure
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
                public string Value => ctx.ReadText(0, "");
                public @structure.READER Structure => new @structure.READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public string Value
                {
                    get => this.ReadText(0, "");
                    set => this.WriteText(0, value, "");
                }

                public @structure.WRITER Structure
                {
                    get => Rewrap<@structure.WRITER>();
                }
            }

            [TypeId(0xc1c75fe79f0fe26cUL)]
            public class @structure : ICapnpSerializable
            {
                public const UInt64 typeId = 0xc1c75fe79f0fe26cUL;
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

        [TypeId(0x8dc8065af1bec3b8UL), Proxy(typeof(Callback_Proxy)), Skeleton(typeof(Callback_Skeleton))]
        public interface ICallback : IDisposable
        {
            Task Call(CancellationToken cancellationToken_ = default);
        }

        public class Callback_Proxy : Proxy, ICallback
        {
            public async Task Call(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Callback.Params_call.WRITER>();
                var arg_ = new Mas.Rpc.Common.Callback.Params_call()
                {};
                arg_.serialize(in_);
                var d_ = await Call(10216422742362604472UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.Common.Callback.Result_call>(d_);
                return;
            }
        }

        public class Callback_Skeleton : Skeleton<ICallback>
        {
            public Callback_Skeleton()
            {
                SetMethodTable(Call);
            }

            public override ulong InterfaceId => 10216422742362604472UL;
            async Task<AnswerOrCounterquestion> Call(DeserializerState d_, CancellationToken cancellationToken_)
            {
                await Impl.Call(cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Callback.Result_call.WRITER>();
                return s_;
            }
        }

        public static class Callback
        {
            [TypeId(0xc34eb90aee0c8e2fUL)]
            public class Params_call : ICapnpSerializable
            {
                public const UInt64 typeId = 0xc34eb90aee0c8e2fUL;
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

            [TypeId(0x9f643e1356ed9727UL)]
            public class Result_call : ICapnpSerializable
            {
                public const UInt64 typeId = 0x9f643e1356ed9727UL;
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

        [TypeId(0xd982646e7cfce3d2UL), Proxy(typeof(Registry_Proxy)), Skeleton(typeof(Registry_Skeleton))]
        public interface IRegistry : IDisposable
        {
            Task<Mas.Rpc.Common.Registry.IUnregister> Register<TObject>(TObject @object, string registrationToken, CancellationToken cancellationToken_ = default)
                where TObject : class;
        }

        public class Registry_Proxy : Proxy, IRegistry
        {
            public Task<Mas.Rpc.Common.Registry.IUnregister> Register<TObject>(TObject @object, string registrationToken, CancellationToken cancellationToken_ = default)
                where TObject : class
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Registry.Params_register<TObject>.WRITER>();
                var arg_ = new Mas.Rpc.Common.Registry.Params_register<TObject>()
                {Object = @object, RegistrationToken = registrationToken};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(15673200078908875730UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.Common.Registry.Result_register<TObject>>(d_);
                    return (r_.Unregister);
                }

                );
            }
        }

        public class Registry_Skeleton : Skeleton<IRegistry>
        {
            public Registry_Skeleton()
            {
                SetMethodTable(Register<AnyPointer>);
            }

            public override ulong InterfaceId => 15673200078908875730UL;
            Task<AnswerOrCounterquestion> Register<TObject>(DeserializerState d_, CancellationToken cancellationToken_)
                where TObject : class
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.Common.Registry.Params_register<TObject>>(d_);
                return Impatient.MaybeTailCall(Impl.Register<TObject>(in_.Object, in_.RegistrationToken, cancellationToken_), unregister =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Registry.Result_register<TObject>.WRITER>();
                    var r_ = new Mas.Rpc.Common.Registry.Result_register<TObject>{Unregister = unregister};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        public static class Registry
        {
            [TypeId(0xa1c30bb328b82d55UL), Proxy(typeof(Unregister_Proxy)), Skeleton(typeof(Unregister_Skeleton))]
            public interface IUnregister : IDisposable
            {
                Task Unregister(CancellationToken cancellationToken_ = default);
            }

            public class Unregister_Proxy : Proxy, IUnregister
            {
                public async Task Unregister(CancellationToken cancellationToken_ = default)
                {
                    var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Registry.Unregister.Params_unregister.WRITER>();
                    var arg_ = new Mas.Rpc.Common.Registry.Unregister.Params_unregister()
                    {};
                    arg_.serialize(in_);
                    var d_ = await Call(11656173124675186005UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                    var r_ = CapnpSerializable.Create<Mas.Rpc.Common.Registry.Unregister.Result_unregister>(d_);
                    return;
                }
            }

            public class Unregister_Skeleton : Skeleton<IUnregister>
            {
                public Unregister_Skeleton()
                {
                    SetMethodTable(Unregister);
                }

                public override ulong InterfaceId => 11656173124675186005UL;
                async Task<AnswerOrCounterquestion> Unregister(DeserializerState d_, CancellationToken cancellationToken_)
                {
                    await Impl.Unregister(cancellationToken_);
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Registry.Unregister.Result_unregister.WRITER>();
                    return s_;
                }
            }

            public static class Unregister
            {
                [TypeId(0x92b0c6bf2051ee7cUL)]
                public class Params_unregister : ICapnpSerializable
                {
                    public const UInt64 typeId = 0x92b0c6bf2051ee7cUL;
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

                [TypeId(0xf948914fe2e1ae63UL)]
                public class Result_unregister : ICapnpSerializable
                {
                    public const UInt64 typeId = 0xf948914fe2e1ae63UL;
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

            [TypeId(0xc55db28522e06bb7UL)]
            public class Params_register<TObject> : ICapnpSerializable where TObject : class
            {
                public const UInt64 typeId = 0xc55db28522e06bb7UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Object = CapnpSerializable.Create<TObject>(reader.Object);
                    RegistrationToken = reader.RegistrationToken;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Object.SetObject(Object);
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

                public TObject Object
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
                    public DeserializerState Object => ctx.StructReadPointer(0);
                    public string RegistrationToken => ctx.ReadText(1, "");
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 2);
                    }

                    public DynamicSerializerState Object
                    {
                        get => BuildPointer<DynamicSerializerState>(0);
                        set => Link(0, value);
                    }

                    public string RegistrationToken
                    {
                        get => this.ReadText(1, "");
                        set => this.WriteText(1, value, "");
                    }
                }
            }

            [TypeId(0x9e83aef1da458368UL)]
            public class Result_register<TObject> : ICapnpSerializable where TObject : class
            {
                public const UInt64 typeId = 0x9e83aef1da458368UL;
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

                public Mas.Rpc.Common.Registry.IUnregister Unregister
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
                    public Mas.Rpc.Common.Registry.IUnregister Unregister => ctx.ReadCap<Mas.Rpc.Common.Registry.IUnregister>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Common.Registry.IUnregister Unregister
                    {
                        get => ReadCap<Mas.Rpc.Common.Registry.IUnregister>(0);
                        set => LinkObject(0, value);
                    }
                }
            }
        }

        [TypeId(0xccb3c229ffabc9feUL)]
        public class ZmqPipelineAddresses : ICapnpSerializable
        {
            public const UInt64 typeId = 0xccb3c229ffabc9feUL;
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
                public string Input => ctx.ReadText(0, "");
                public string Output => ctx.ReadText(1, "");
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public string Input
                {
                    get => this.ReadText(0, "");
                    set => this.WriteText(0, value, "");
                }

                public string Output
                {
                    get => this.ReadText(1, "");
                    set => this.WriteText(1, value, "");
                }
            }
        }

        [TypeId(0x813e612b327a0a66UL), Proxy(typeof(CapHolder_Proxy<>)), Skeleton(typeof(CapHolder_Skeleton<>))]
        public interface ICapHolder<TCapType> : Capnp.IPersistent<string, string> where TCapType : class
        {
            Task<TCapType> Cap(CancellationToken cancellationToken_ = default);
            Task Release(CancellationToken cancellationToken_ = default);
        }

        public class CapHolder_Proxy<TCapType> : Proxy, ICapHolder<TCapType> where TCapType : class
        {
            public Task<TCapType> Cap(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.CapHolder<TCapType>.Params_cap.WRITER>();
                var arg_ = new Mas.Rpc.Common.CapHolder<TCapType>.Params_cap()
                {};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(9312987917607111270UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.Common.CapHolder<TCapType>.Result_cap>(d_);
                    return (r_.Cap);
                }

                );
            }

            public async Task Release(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.CapHolder<TCapType>.Params_release.WRITER>();
                var arg_ = new Mas.Rpc.Common.CapHolder<TCapType>.Params_release()
                {};
                arg_.serialize(in_);
                var d_ = await Call(9312987917607111270UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.Common.CapHolder<TCapType>.Result_release>(d_);
                return;
            }

            public async Task<Capnp.Persistent<string, string>.SaveResults> Save(Capnp.Persistent<string, string>.SaveParams arg_, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Capnp.Persistent<string, string>.SaveParams.WRITER>();
                arg_.serialize(in_);
                var d_ = await Call(14468694717054801553UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Capnp.Persistent<string, string>.SaveResults>(d_);
                return r_;
            }
        }

        public class CapHolder_Skeleton<TCapType> : Skeleton<ICapHolder<TCapType>> where TCapType : class
        {
            public CapHolder_Skeleton()
            {
                SetMethodTable(Cap, Release);
            }

            public override ulong InterfaceId => 9312987917607111270UL;
            Task<AnswerOrCounterquestion> Cap(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.Cap(cancellationToken_), cap =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Common.CapHolder<TCapType>.Result_cap.WRITER>();
                    var r_ = new Mas.Rpc.Common.CapHolder<TCapType>.Result_cap{Cap = cap};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            async Task<AnswerOrCounterquestion> Release(DeserializerState d_, CancellationToken cancellationToken_)
            {
                await Impl.Release(cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Rpc.Common.CapHolder<TCapType>.Result_release.WRITER>();
                return s_;
            }
        }

        public static class CapHolder<TCapType>
            where TCapType : class
        {
            [TypeId(0xff566db7b81c34d6UL)]
            public class Params_cap : ICapnpSerializable
            {
                public const UInt64 typeId = 0xff566db7b81c34d6UL;
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

            [TypeId(0xa123ddcae40f0c56UL)]
            public class Result_cap : ICapnpSerializable
            {
                public const UInt64 typeId = 0xa123ddcae40f0c56UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Cap = CapnpSerializable.Create<TCapType>(reader.Cap);
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Cap.SetObject(Cap);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public TCapType Cap
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
                    public DeserializerState Cap => ctx.StructReadPointer(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public DynamicSerializerState Cap
                    {
                        get => BuildPointer<DynamicSerializerState>(0);
                        set => Link(0, value);
                    }
                }
            }

            [TypeId(0xd1066c1fa8672ae2UL)]
            public class Params_release : ICapnpSerializable
            {
                public const UInt64 typeId = 0xd1066c1fa8672ae2UL;
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

            [TypeId(0xb64d4ec78c5a8b35UL)]
            public class Result_release : ICapnpSerializable
            {
                public const UInt64 typeId = 0xb64d4ec78c5a8b35UL;
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

        [TypeId(0xf1206b672c90bd62UL)]
        public class ListEntry<TPointerType> : ICapnpSerializable where TPointerType : class
        {
            public const UInt64 typeId = 0xf1206b672c90bd62UL;
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

        [TypeId(0xa3022e802083f281UL), Proxy(typeof(Stopable_Proxy)), Skeleton(typeof(Stopable_Skeleton))]
        public interface IStopable : IDisposable
        {
            Task Stop(CancellationToken cancellationToken_ = default);
        }

        public class Stopable_Proxy : Proxy, IStopable
        {
            public async Task Stop(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Stopable.Params_stop.WRITER>();
                var arg_ = new Mas.Rpc.Common.Stopable.Params_stop()
                {};
                arg_.serialize(in_);
                var d_ = await Call(11746001905971884673UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.Common.Stopable.Result_stop>(d_);
                return;
            }
        }

        public class Stopable_Skeleton : Skeleton<IStopable>
        {
            public Stopable_Skeleton()
            {
                SetMethodTable(Stop);
            }

            public override ulong InterfaceId => 11746001905971884673UL;
            async Task<AnswerOrCounterquestion> Stop(DeserializerState d_, CancellationToken cancellationToken_)
            {
                await Impl.Stop(cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Stopable.Result_stop.WRITER>();
                return s_;
            }
        }

        public static class Stopable
        {
            [TypeId(0xba89e46d3ad8a20cUL)]
            public class Params_stop : ICapnpSerializable
            {
                public const UInt64 typeId = 0xba89e46d3ad8a20cUL;
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

            [TypeId(0x8e8519c23684f925UL)]
            public class Result_stop : ICapnpSerializable
            {
                public const UInt64 typeId = 0x8e8519c23684f925UL;
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
}