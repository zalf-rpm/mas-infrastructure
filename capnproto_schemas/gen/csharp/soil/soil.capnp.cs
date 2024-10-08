using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Schema.Soil
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc2e4a3c8ff61b40aUL)]
    public enum SType : ushort
    {
        unknown,
        ka5
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9e391ae1c6cd2567UL)]
    public enum PropertyName : ushort
    {
        soilType,
        sand,
        clay,
        silt,
        pH,
        sceleton,
        organicCarbon,
        organicMatter,
        bulkDensity,
        rawDensity,
        fieldCapacity,
        permanentWiltingPoint,
        saturation,
        soilMoisture,
        soilWaterConductivityCoefficient,
        ammonium,
        nitrate,
        cnRatio,
        inGroundwater,
        impenetrable
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x984640f05b3ada4fUL)]
    public class Layer : ICapnpSerializable
    {
        public const UInt64 typeId = 0x984640f05b3ada4fUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Properties = reader.Properties?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Soil.Layer.Property>(_));
            Size = reader.Size;
            Description = reader.Description;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Properties.Init(Properties, (_s1, _v1) => _v1?.serialize(_s1));
            writer.Size = Size;
            writer.Description = Description;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public IReadOnlyList<Mas.Schema.Soil.Layer.Property> Properties
        {
            get;
            set;
        }

        public float Size
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
            public IReadOnlyList<Mas.Schema.Soil.Layer.Property.READER> Properties => ctx.ReadList(0).Cast(Mas.Schema.Soil.Layer.Property.READER.create);
            public bool HasProperties => ctx.IsStructFieldNonNull(0);
            public float Size => ctx.ReadDataFloat(0UL, 0F);
            public string Description => ctx.ReadText(1, null);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 2);
            }

            public ListOfStructsSerializer<Mas.Schema.Soil.Layer.Property.WRITER> Properties
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Soil.Layer.Property.WRITER>>(0);
                set => Link(0, value);
            }

            public float Size
            {
                get => this.ReadDataFloat(0UL, 0F);
                set => this.WriteData(0UL, value, 0F);
            }

            public string Description
            {
                get => this.ReadText(1, null);
                set => this.WriteText(1, value, null);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x92f4b81bcfdb71b0UL)]
        public class Property : ICapnpSerializable
        {
            public const UInt64 typeId = 0x92f4b81bcfdb71b0UL;
            public enum WHICH : ushort
            {
                F32Value = 0,
                BValue = 1,
                Type = 2,
                Unset = 3,
                undefined = 65535
            }

            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                switch (reader.which)
                {
                    case WHICH.F32Value:
                        F32Value = reader.F32Value;
                        break;
                    case WHICH.BValue:
                        BValue = reader.BValue;
                        break;
                    case WHICH.Type:
                        Type = reader.Type;
                        break;
                    case WHICH.Unset:
                        which = reader.which;
                        break;
                }

                Name = reader.Name;
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
                        case WHICH.F32Value:
                            _content = 0F;
                            break;
                        case WHICH.BValue:
                            _content = false;
                            break;
                        case WHICH.Type:
                            _content = null;
                            break;
                        case WHICH.Unset:
                            break;
                    }
                }
            }

            public void serialize(WRITER writer)
            {
                writer.which = which;
                switch (which)
                {
                    case WHICH.F32Value:
                        writer.F32Value = F32Value.Value;
                        break;
                    case WHICH.BValue:
                        writer.BValue = BValue.Value;
                        break;
                    case WHICH.Type:
                        writer.Type = Type;
                        break;
                    case WHICH.Unset:
                        break;
                }

                writer.Name = Name;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Soil.PropertyName Name
            {
                get;
                set;
            }

            public float? F32Value
            {
                get => _which == WHICH.F32Value ? (float? )_content : null;
                set
                {
                    _which = WHICH.F32Value;
                    _content = value;
                }
            }

            public bool? BValue
            {
                get => _which == WHICH.BValue ? (bool? )_content : null;
                set
                {
                    _which = WHICH.BValue;
                    _content = value;
                }
            }

            public string Type
            {
                get => _which == WHICH.Type ? (string)_content : null;
                set
                {
                    _which = WHICH.Type;
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
                public Mas.Schema.Soil.PropertyName Name => (Mas.Schema.Soil.PropertyName)ctx.ReadDataUShort(0UL, (ushort)0);
                public float F32Value => which == WHICH.F32Value ? ctx.ReadDataFloat(32UL, 0F) : default;
                public bool BValue => which == WHICH.BValue ? ctx.ReadDataBool(32UL, false) : default;
                public string Type => which == WHICH.Type ? ctx.ReadText(0, null) : default;
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

                public Mas.Schema.Soil.PropertyName Name
                {
                    get => (Mas.Schema.Soil.PropertyName)this.ReadDataUShort(0UL, (ushort)0);
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }

                public float F32Value
                {
                    get => which == WHICH.F32Value ? this.ReadDataFloat(32UL, 0F) : default;
                    set => this.WriteData(32UL, value, 0F);
                }

                public bool BValue
                {
                    get => which == WHICH.BValue ? this.ReadDataBool(32UL, false) : default;
                    set => this.WriteData(32UL, value, false);
                }

                public string Type
                {
                    get => which == WHICH.Type ? this.ReadText(0, null) : default;
                    set => this.WriteText(0, value, null);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbd4065087e22ca0dUL)]
    public class Query : ICapnpSerializable
    {
        public const UInt64 typeId = 0xbd4065087e22ca0dUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Mandatory = reader.Mandatory;
            Optional = reader.Optional;
            OnlyRawData = reader.OnlyRawData;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Mandatory.Init(Mandatory);
            writer.Optional.Init(Optional);
            writer.OnlyRawData = OnlyRawData;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public IReadOnlyList<Mas.Schema.Soil.PropertyName> Mandatory
        {
            get;
            set;
        }

        public IReadOnlyList<Mas.Schema.Soil.PropertyName> Optional
        {
            get;
            set;
        }

        public bool OnlyRawData
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
            public IReadOnlyList<Mas.Schema.Soil.PropertyName> Mandatory => ctx.ReadList(0).CastEnums(_0 => (Mas.Schema.Soil.PropertyName)_0);
            public bool HasMandatory => ctx.IsStructFieldNonNull(0);
            public IReadOnlyList<Mas.Schema.Soil.PropertyName> Optional => ctx.ReadList(1).CastEnums(_0 => (Mas.Schema.Soil.PropertyName)_0);
            public bool HasOptional => ctx.IsStructFieldNonNull(1);
            public bool OnlyRawData => ctx.ReadDataBool(0UL, true);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 2);
            }

            public ListOfPrimitivesSerializer<Mas.Schema.Soil.PropertyName> Mandatory
            {
                get => BuildPointer<ListOfPrimitivesSerializer<Mas.Schema.Soil.PropertyName>>(0);
                set => Link(0, value);
            }

            public ListOfPrimitivesSerializer<Mas.Schema.Soil.PropertyName> Optional
            {
                get => BuildPointer<ListOfPrimitivesSerializer<Mas.Schema.Soil.PropertyName>>(1);
                set => Link(1, value);
            }

            public bool OnlyRawData
            {
                get => this.ReadDataBool(0UL, true);
                set => this.WriteData(0UL, value, true);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbf4e1b07ad88943fUL)]
        public class Result : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbf4e1b07ad88943fUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Failed = reader.Failed;
                Mandatory = reader.Mandatory;
                Optional = reader.Optional;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Failed = Failed;
                writer.Mandatory.Init(Mandatory);
                writer.Optional.Init(Optional);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Failed
            {
                get;
                set;
            }

            public IReadOnlyList<Mas.Schema.Soil.PropertyName> Mandatory
            {
                get;
                set;
            }

            public IReadOnlyList<Mas.Schema.Soil.PropertyName> Optional
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
                public bool Failed => ctx.ReadDataBool(0UL, false);
                public IReadOnlyList<Mas.Schema.Soil.PropertyName> Mandatory => ctx.ReadList(0).CastEnums(_0 => (Mas.Schema.Soil.PropertyName)_0);
                public bool HasMandatory => ctx.IsStructFieldNonNull(0);
                public IReadOnlyList<Mas.Schema.Soil.PropertyName> Optional => ctx.ReadList(1).CastEnums(_0 => (Mas.Schema.Soil.PropertyName)_0);
                public bool HasOptional => ctx.IsStructFieldNonNull(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 2);
                }

                public bool Failed
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }

                public ListOfPrimitivesSerializer<Mas.Schema.Soil.PropertyName> Mandatory
                {
                    get => BuildPointer<ListOfPrimitivesSerializer<Mas.Schema.Soil.PropertyName>>(0);
                    set => Link(0, value);
                }

                public ListOfPrimitivesSerializer<Mas.Schema.Soil.PropertyName> Optional
                {
                    get => BuildPointer<ListOfPrimitivesSerializer<Mas.Schema.Soil.PropertyName>>(1);
                    set => Link(1, value);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdf4bbf1c883a8790UL)]
    public class ProfileData : ICapnpSerializable
    {
        public const UInt64 typeId = 0xdf4bbf1c883a8790UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Layers = reader.Layers?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Soil.Layer>(_));
            PercentageOfArea = reader.PercentageOfArea;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Layers.Init(Layers, (_s1, _v1) => _v1?.serialize(_s1));
            writer.PercentageOfArea = PercentageOfArea;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public IReadOnlyList<Mas.Schema.Soil.Layer> Layers
        {
            get;
            set;
        }

        public float PercentageOfArea
        {
            get;
            set;
        }

        = 100F;
        public struct READER
        {
            readonly DeserializerState ctx;
            public READER(DeserializerState ctx)
            {
                this.ctx = ctx;
            }

            public static READER create(DeserializerState ctx) => new READER(ctx);
            public static implicit operator DeserializerState(READER reader) => reader.ctx;
            public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            public IReadOnlyList<Mas.Schema.Soil.Layer.READER> Layers => ctx.ReadList(0).Cast(Mas.Schema.Soil.Layer.READER.create);
            public bool HasLayers => ctx.IsStructFieldNonNull(0);
            public float PercentageOfArea => ctx.ReadDataFloat(0UL, 100F);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 1);
            }

            public ListOfStructsSerializer<Mas.Schema.Soil.Layer.WRITER> Layers
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Soil.Layer.WRITER>>(0);
                set => Link(0, value);
            }

            public float PercentageOfArea
            {
                get => this.ReadDataFloat(0UL, 100F);
                set => this.WriteData(0UL, value, 100F);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xff67c2a593419c29UL), Proxy(typeof(Profile_Proxy)), Skeleton(typeof(Profile_Skeleton))]
    public interface IProfile : Mas.Schema.Common.IIdentifiable, Mas.Schema.Persistence.IPersistent
    {
        Task<Mas.Schema.Soil.ProfileData> Data(CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Geo.LatLonCoord> GeoLocation(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xff67c2a593419c29UL)]
    public class Profile_Proxy : Proxy, IProfile
    {
        public async Task<Mas.Schema.Soil.ProfileData> Data(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Soil.Profile.Params_Data.WRITER>();
            var arg_ = new Mas.Schema.Soil.Profile.Params_Data()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(18403892418668764201UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Soil.ProfileData>(d_);
                return r_;
            }
        }

        public async Task<Mas.Schema.Geo.LatLonCoord> GeoLocation(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Soil.Profile.Params_GeoLocation.WRITER>();
            var arg_ = new Mas.Schema.Soil.Profile.Params_GeoLocation()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(18403892418668764201UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Geo.LatLonCoord>(d_);
                return r_;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xff67c2a593419c29UL)]
    public class Profile_Skeleton : Skeleton<IProfile>
    {
        public Profile_Skeleton()
        {
            SetMethodTable(Data, GeoLocation);
        }

        public override ulong InterfaceId => 18403892418668764201UL;
        Task<AnswerOrCounterquestion> Data(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Data(cancellationToken_), r_ =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Soil.ProfileData.WRITER>();
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> GeoLocation(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.GeoLocation(cancellationToken_), r_ =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Geo.LatLonCoord.WRITER>();
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class Profile
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe704b695746374e2UL)]
        public class Params_Data : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe704b695746374e2UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8d2a23f8fd1b9151UL)]
        public class Params_GeoLocation : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8d2a23f8fd1b9151UL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa09aa71427dc64e1UL), Proxy(typeof(Service_Proxy)), Skeleton(typeof(Service_Skeleton))]
    public interface IService : Mas.Schema.Common.IIdentifiable, Mas.Schema.Persistence.IPersistent
    {
        Task<Mas.Schema.Soil.Query.Result> CheckAvailableParameters(Mas.Schema.Soil.Query arg_, CancellationToken cancellationToken_ = default);
        Task<(IReadOnlyList<Mas.Schema.Soil.PropertyName>, IReadOnlyList<Mas.Schema.Soil.PropertyName>)> GetAllAvailableParameters(bool onlyRawData, CancellationToken cancellationToken_ = default);
        Task<IReadOnlyList<Mas.Schema.Soil.IProfile>> ClosestProfilesAt(Mas.Schema.Geo.LatLonCoord coord, Mas.Schema.Soil.Query query, CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Soil.Service.IStream> StreamAllProfiles(Mas.Schema.Soil.Query arg_, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa09aa71427dc64e1UL)]
    public class Service_Proxy : Proxy, IService
    {
        public async Task<Mas.Schema.Soil.Query.Result> CheckAvailableParameters(Mas.Schema.Soil.Query arg_, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Soil.Query.WRITER>();
            arg_?.serialize(in_);
            using (var d_ = await Call(11572745897491850465UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Soil.Query.Result>(d_);
                return r_;
            }
        }

        public async Task<(IReadOnlyList<Mas.Schema.Soil.PropertyName>, IReadOnlyList<Mas.Schema.Soil.PropertyName>)> GetAllAvailableParameters(bool onlyRawData, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Soil.Service.Params_GetAllAvailableParameters.WRITER>();
            var arg_ = new Mas.Schema.Soil.Service.Params_GetAllAvailableParameters()
            {OnlyRawData = onlyRawData};
            arg_?.serialize(in_);
            using (var d_ = await Call(11572745897491850465UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Soil.Service.Result_GetAllAvailableParameters>(d_);
                return (r_.Mandatory, r_.Optional);
            }
        }

        public Task<IReadOnlyList<Mas.Schema.Soil.IProfile>> ClosestProfilesAt(Mas.Schema.Geo.LatLonCoord coord, Mas.Schema.Soil.Query query, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Soil.Service.Params_ClosestProfilesAt.WRITER>();
            var arg_ = new Mas.Schema.Soil.Service.Params_ClosestProfilesAt()
            {Coord = coord, Query = query};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(11572745897491850465UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Soil.Service.Result_ClosestProfilesAt>(d_);
                    return (r_.Profiles);
                }
            }

            );
        }

        public Task<Mas.Schema.Soil.Service.IStream> StreamAllProfiles(Mas.Schema.Soil.Query arg_, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Soil.Query.WRITER>();
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(11572745897491850465UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Soil.Service.Result_StreamAllProfiles>(d_);
                    return (r_.AllProfiles);
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa09aa71427dc64e1UL)]
    public class Service_Skeleton : Skeleton<IService>
    {
        public Service_Skeleton()
        {
            SetMethodTable(CheckAvailableParameters, GetAllAvailableParameters, ClosestProfilesAt, StreamAllProfiles);
        }

        public override ulong InterfaceId => 11572745897491850465UL;
        Task<AnswerOrCounterquestion> CheckAvailableParameters(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.CheckAvailableParameters(CapnpSerializable.Create<Mas.Schema.Soil.Query>(d_), cancellationToken_), r_ =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Soil.Query.Result.WRITER>();
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> GetAllAvailableParameters(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Soil.Service.Params_GetAllAvailableParameters>(d_);
                return Impatient.MaybeTailCall(Impl.GetAllAvailableParameters(in_.OnlyRawData, cancellationToken_), (mandatory, optional) =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Soil.Service.Result_GetAllAvailableParameters.WRITER>();
                    var r_ = new Mas.Schema.Soil.Service.Result_GetAllAvailableParameters{Mandatory = mandatory, Optional = optional};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> ClosestProfilesAt(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Soil.Service.Params_ClosestProfilesAt>(d_);
                return Impatient.MaybeTailCall(Impl.ClosestProfilesAt(in_.Coord, in_.Query, cancellationToken_), profiles =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Soil.Service.Result_ClosestProfilesAt.WRITER>();
                    var r_ = new Mas.Schema.Soil.Service.Result_ClosestProfilesAt{Profiles = profiles};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> StreamAllProfiles(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.StreamAllProfiles(CapnpSerializable.Create<Mas.Schema.Soil.Query>(d_), cancellationToken_), allProfiles =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Soil.Service.Result_StreamAllProfiles.WRITER>();
                    var r_ = new Mas.Schema.Soil.Service.Result_StreamAllProfiles{AllProfiles = allProfiles};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class Service
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf4f8ab568ffbc939UL), Proxy(typeof(Stream_Proxy)), Skeleton(typeof(Stream_Skeleton))]
        public interface IStream : IDisposable
        {
            Task<IReadOnlyList<Mas.Schema.Soil.IProfile>> NextProfiles(long maxCount, CancellationToken cancellationToken_ = default);
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf4f8ab568ffbc939UL)]
        public class Stream_Proxy : Proxy, IStream
        {
            public Task<IReadOnlyList<Mas.Schema.Soil.IProfile>> NextProfiles(long maxCount, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Soil.Service.Stream.Params_NextProfiles.WRITER>();
                var arg_ = new Mas.Schema.Soil.Service.Stream.Params_NextProfiles()
                {MaxCount = maxCount};
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(17652047127749839161UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Soil.Service.Stream.Result_NextProfiles>(d_);
                        return (r_.Profiles);
                    }
                }

                );
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf4f8ab568ffbc939UL)]
        public class Stream_Skeleton : Skeleton<IStream>
        {
            public Stream_Skeleton()
            {
                SetMethodTable(NextProfiles);
            }

            public override ulong InterfaceId => 17652047127749839161UL;
            Task<AnswerOrCounterquestion> NextProfiles(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Soil.Service.Stream.Params_NextProfiles>(d_);
                    return Impatient.MaybeTailCall(Impl.NextProfiles(in_.MaxCount, cancellationToken_), profiles =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Soil.Service.Stream.Result_NextProfiles.WRITER>();
                        var r_ = new Mas.Schema.Soil.Service.Stream.Result_NextProfiles{Profiles = profiles};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }
        }

        public static class Stream
        {
            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x81da248df613bc05UL)]
            public class Params_NextProfiles : ICapnpSerializable
            {
                public const UInt64 typeId = 0x81da248df613bc05UL;
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

                = 100L;
                public struct READER
                {
                    readonly DeserializerState ctx;
                    public READER(DeserializerState ctx)
                    {
                        this.ctx = ctx;
                    }

                    public static READER create(DeserializerState ctx) => new READER(ctx);
                    public static implicit operator DeserializerState(READER reader) => reader.ctx;
                    public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                    public long MaxCount => ctx.ReadDataLong(0UL, 100L);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 0);
                    }

                    public long MaxCount
                    {
                        get => this.ReadDataLong(0UL, 100L);
                        set => this.WriteData(0UL, value, 100L);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9f7ae4c2748bddf8UL)]
            public class Result_NextProfiles : ICapnpSerializable
            {
                public const UInt64 typeId = 0x9f7ae4c2748bddf8UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Profiles = reader.Profiles;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Profiles.Init(Profiles);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<Mas.Schema.Soil.IProfile> Profiles
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
                    public IReadOnlyList<Mas.Schema.Soil.IProfile> Profiles => ctx.ReadCapList<Mas.Schema.Soil.IProfile>(0);
                    public bool HasProfiles => ctx.IsStructFieldNonNull(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfCapsSerializer<Mas.Schema.Soil.IProfile> Profiles
                    {
                        get => BuildPointer<ListOfCapsSerializer<Mas.Schema.Soil.IProfile>>(0);
                        set => Link(0, value);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8dec5fd8eb3e7c27UL)]
        public class Params_GetAllAvailableParameters : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8dec5fd8eb3e7c27UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                OnlyRawData = reader.OnlyRawData;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.OnlyRawData = OnlyRawData;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool OnlyRawData
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
                public bool OnlyRawData => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool OnlyRawData
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x98a2bf8e6ad97ee3UL)]
        public class Result_GetAllAvailableParameters : ICapnpSerializable
        {
            public const UInt64 typeId = 0x98a2bf8e6ad97ee3UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mandatory = reader.Mandatory;
                Optional = reader.Optional;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mandatory.Init(Mandatory);
                writer.Optional.Init(Optional);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<Mas.Schema.Soil.PropertyName> Mandatory
            {
                get;
                set;
            }

            public IReadOnlyList<Mas.Schema.Soil.PropertyName> Optional
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
                public IReadOnlyList<Mas.Schema.Soil.PropertyName> Mandatory => ctx.ReadList(0).CastEnums(_0 => (Mas.Schema.Soil.PropertyName)_0);
                public bool HasMandatory => ctx.IsStructFieldNonNull(0);
                public IReadOnlyList<Mas.Schema.Soil.PropertyName> Optional => ctx.ReadList(1).CastEnums(_0 => (Mas.Schema.Soil.PropertyName)_0);
                public bool HasOptional => ctx.IsStructFieldNonNull(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public ListOfPrimitivesSerializer<Mas.Schema.Soil.PropertyName> Mandatory
                {
                    get => BuildPointer<ListOfPrimitivesSerializer<Mas.Schema.Soil.PropertyName>>(0);
                    set => Link(0, value);
                }

                public ListOfPrimitivesSerializer<Mas.Schema.Soil.PropertyName> Optional
                {
                    get => BuildPointer<ListOfPrimitivesSerializer<Mas.Schema.Soil.PropertyName>>(1);
                    set => Link(1, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdb97e739bf9693c1UL)]
        public class Params_ClosestProfilesAt : ICapnpSerializable
        {
            public const UInt64 typeId = 0xdb97e739bf9693c1UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Coord = CapnpSerializable.Create<Mas.Schema.Geo.LatLonCoord>(reader.Coord);
                Query = CapnpSerializable.Create<Mas.Schema.Soil.Query>(reader.Query);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Coord?.serialize(writer.Coord);
                Query?.serialize(writer.Query);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Geo.LatLonCoord Coord
            {
                get;
                set;
            }

            public Mas.Schema.Soil.Query Query
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
                public Mas.Schema.Geo.LatLonCoord.READER Coord => ctx.ReadStruct(0, Mas.Schema.Geo.LatLonCoord.READER.create);
                public bool HasCoord => ctx.IsStructFieldNonNull(0);
                public Mas.Schema.Soil.Query.READER Query => ctx.ReadStruct(1, Mas.Schema.Soil.Query.READER.create);
                public bool HasQuery => ctx.IsStructFieldNonNull(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public Mas.Schema.Geo.LatLonCoord.WRITER Coord
                {
                    get => BuildPointer<Mas.Schema.Geo.LatLonCoord.WRITER>(0);
                    set => Link(0, value);
                }

                public Mas.Schema.Soil.Query.WRITER Query
                {
                    get => BuildPointer<Mas.Schema.Soil.Query.WRITER>(1);
                    set => Link(1, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa0915e668c9317adUL)]
        public class Result_ClosestProfilesAt : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa0915e668c9317adUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Profiles = reader.Profiles;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Profiles.Init(Profiles);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<Mas.Schema.Soil.IProfile> Profiles
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
                public IReadOnlyList<Mas.Schema.Soil.IProfile> Profiles => ctx.ReadCapList<Mas.Schema.Soil.IProfile>(0);
                public bool HasProfiles => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfCapsSerializer<Mas.Schema.Soil.IProfile> Profiles
                {
                    get => BuildPointer<ListOfCapsSerializer<Mas.Schema.Soil.IProfile>>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd1a74147c92b7957UL)]
        public class Result_StreamAllProfiles : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd1a74147c92b7957UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                AllProfiles = reader.AllProfiles;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.AllProfiles = AllProfiles;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Soil.Service.IStream AllProfiles
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
                public Mas.Schema.Soil.Service.IStream AllProfiles => ctx.ReadCap<Mas.Schema.Soil.Service.IStream>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Soil.Service.IStream AllProfiles
                {
                    get => ReadCap<Mas.Schema.Soil.Service.IStream>(0);
                    set => LinkObject(0, value);
                }
            }
        }
    }
}