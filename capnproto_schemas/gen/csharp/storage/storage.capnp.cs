using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Schema.Storage
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe69f958aa2386f06UL), Proxy(typeof(Store_Proxy)), Skeleton(typeof(Store_Skeleton))]
    public interface IStore : IDisposable
    {
        Task<Mas.Schema.Storage.Store.IContainer> NewContainer(Mas.Schema.Common.IdInformation arg_, CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Storage.Store.IContainer> ContainerWithId(string id, CancellationToken cancellationToken_ = default);
        Task<IReadOnlyList<Mas.Schema.Storage.Store.IContainer>> ListContainers(CancellationToken cancellationToken_ = default);
        Task<bool> RemoveContainer(string id, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe69f958aa2386f06UL)]
    public class Store_Proxy : Proxy, IStore
    {
        public Task<Mas.Schema.Storage.Store.IContainer> NewContainer(Mas.Schema.Common.IdInformation arg_, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.IdInformation.WRITER>();
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(16618165572680052486UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Result_NewContainer>(d_);
                    return (r_.Container);
                }
            }

            );
        }

        public Task<Mas.Schema.Storage.Store.IContainer> ContainerWithId(string id, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Params_ContainerWithId.WRITER>();
            var arg_ = new Mas.Schema.Storage.Store.Params_ContainerWithId()
            {Id = id};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(16618165572680052486UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Result_ContainerWithId>(d_);
                    return (r_.Container);
                }
            }

            );
        }

        public Task<IReadOnlyList<Mas.Schema.Storage.Store.IContainer>> ListContainers(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Params_ListContainers.WRITER>();
            var arg_ = new Mas.Schema.Storage.Store.Params_ListContainers()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(16618165572680052486UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Result_ListContainers>(d_);
                    return (r_.Containers);
                }
            }

            );
        }

        public async Task<bool> RemoveContainer(string id, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Params_RemoveContainer.WRITER>();
            var arg_ = new Mas.Schema.Storage.Store.Params_RemoveContainer()
            {Id = id};
            arg_?.serialize(in_);
            using (var d_ = await Call(16618165572680052486UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Result_RemoveContainer>(d_);
                return (r_.Success);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe69f958aa2386f06UL)]
    public class Store_Skeleton : Skeleton<IStore>
    {
        public Store_Skeleton()
        {
            SetMethodTable(NewContainer, ContainerWithId, ListContainers, RemoveContainer);
        }

        public override ulong InterfaceId => 16618165572680052486UL;
        Task<AnswerOrCounterquestion> NewContainer(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.NewContainer(CapnpSerializable.Create<Mas.Schema.Common.IdInformation>(d_), cancellationToken_), container =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Result_NewContainer.WRITER>();
                    var r_ = new Mas.Schema.Storage.Store.Result_NewContainer{Container = container};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> ContainerWithId(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Params_ContainerWithId>(d_);
                return Impatient.MaybeTailCall(Impl.ContainerWithId(in_.Id, cancellationToken_), container =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Result_ContainerWithId.WRITER>();
                    var r_ = new Mas.Schema.Storage.Store.Result_ContainerWithId{Container = container};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> ListContainers(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.ListContainers(cancellationToken_), containers =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Result_ListContainers.WRITER>();
                    var r_ = new Mas.Schema.Storage.Store.Result_ListContainers{Containers = containers};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> RemoveContainer(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Params_RemoveContainer>(d_);
                return Impatient.MaybeTailCall(Impl.RemoveContainer(in_.Id, cancellationToken_), success =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Result_RemoveContainer.WRITER>();
                    var r_ = new Mas.Schema.Storage.Store.Result_RemoveContainer{Success = success};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class Store
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x878131f45567ae62UL), Proxy(typeof(Container_Proxy)), Skeleton(typeof(Container_Skeleton))]
        public interface IContainer : Mas.Schema.Common.IIdentifiable, Mas.Schema.Persistence.IPersistent
        {
            Task<bool> ImportData(IReadOnlyList<byte> data, CancellationToken cancellationToken_ = default);
            Task<IReadOnlyList<byte>> ExportData(CancellationToken cancellationToken_ = default);
            Task<IReadOnlyList<Mas.Schema.Storage.Store.Container.Object>> ListObjects(CancellationToken cancellationToken_ = default);
            Task<Mas.Schema.Storage.Store.Container.Object> GetObject(string key, CancellationToken cancellationToken_ = default);
            Task<bool> AddObject(Mas.Schema.Storage.Store.Container.Object @object, CancellationToken cancellationToken_ = default);
            Task<bool> RemoveObject(string key, CancellationToken cancellationToken_ = default);
            Task<bool> Clear(CancellationToken cancellationToken_ = default);
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x878131f45567ae62UL)]
        public class Container_Proxy : Proxy, IContainer
        {
            public async Task<bool> ImportData(IReadOnlyList<byte> data, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Params_ImportData.WRITER>();
                var arg_ = new Mas.Schema.Storage.Store.Container.Params_ImportData()
                {Data = data};
                arg_?.serialize(in_);
                using (var d_ = await Call(9764140392590585442UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Result_ImportData>(d_);
                    return (r_.Success);
                }
            }

            public async Task<IReadOnlyList<byte>> ExportData(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Params_ExportData.WRITER>();
                var arg_ = new Mas.Schema.Storage.Store.Container.Params_ExportData()
                {};
                arg_?.serialize(in_);
                using (var d_ = await Call(9764140392590585442UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Result_ExportData>(d_);
                    return (r_.Data);
                }
            }

            public async Task<IReadOnlyList<Mas.Schema.Storage.Store.Container.Object>> ListObjects(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Params_ListObjects.WRITER>();
                var arg_ = new Mas.Schema.Storage.Store.Container.Params_ListObjects()
                {};
                arg_?.serialize(in_);
                using (var d_ = await Call(9764140392590585442UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Result_ListObjects>(d_);
                    return (r_.Objects);
                }
            }

            public async Task<Mas.Schema.Storage.Store.Container.Object> GetObject(string key, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Params_GetObject.WRITER>();
                var arg_ = new Mas.Schema.Storage.Store.Container.Params_GetObject()
                {Key = key};
                arg_?.serialize(in_);
                using (var d_ = await Call(9764140392590585442UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Result_GetObject>(d_);
                    return (r_.Object);
                }
            }

            public async Task<bool> AddObject(Mas.Schema.Storage.Store.Container.Object @object, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Params_AddObject.WRITER>();
                var arg_ = new Mas.Schema.Storage.Store.Container.Params_AddObject()
                {Object = @object};
                arg_?.serialize(in_);
                using (var d_ = await Call(9764140392590585442UL, 4, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Result_AddObject>(d_);
                    return (r_.Success);
                }
            }

            public async Task<bool> RemoveObject(string key, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Params_RemoveObject.WRITER>();
                var arg_ = new Mas.Schema.Storage.Store.Container.Params_RemoveObject()
                {Key = key};
                arg_?.serialize(in_);
                using (var d_ = await Call(9764140392590585442UL, 5, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Result_RemoveObject>(d_);
                    return (r_.Success);
                }
            }

            public async Task<bool> Clear(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Params_Clear.WRITER>();
                var arg_ = new Mas.Schema.Storage.Store.Container.Params_Clear()
                {};
                arg_?.serialize(in_);
                using (var d_ = await Call(9764140392590585442UL, 6, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Result_Clear>(d_);
                    return (r_.Success);
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x878131f45567ae62UL)]
        public class Container_Skeleton : Skeleton<IContainer>
        {
            public Container_Skeleton()
            {
                SetMethodTable(ImportData, ExportData, ListObjects, GetObject, AddObject, RemoveObject, Clear);
            }

            public override ulong InterfaceId => 9764140392590585442UL;
            Task<AnswerOrCounterquestion> ImportData(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Params_ImportData>(d_);
                    return Impatient.MaybeTailCall(Impl.ImportData(in_.Data, cancellationToken_), success =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Result_ImportData.WRITER>();
                        var r_ = new Mas.Schema.Storage.Store.Container.Result_ImportData{Success = success};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> ExportData(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    return Impatient.MaybeTailCall(Impl.ExportData(cancellationToken_), data =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Result_ExportData.WRITER>();
                        var r_ = new Mas.Schema.Storage.Store.Container.Result_ExportData{Data = data};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> ListObjects(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    return Impatient.MaybeTailCall(Impl.ListObjects(cancellationToken_), objects =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Result_ListObjects.WRITER>();
                        var r_ = new Mas.Schema.Storage.Store.Container.Result_ListObjects{Objects = objects};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> GetObject(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Params_GetObject>(d_);
                    return Impatient.MaybeTailCall(Impl.GetObject(in_.Key, cancellationToken_), @object =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Result_GetObject.WRITER>();
                        var r_ = new Mas.Schema.Storage.Store.Container.Result_GetObject{Object = @object};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> AddObject(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Params_AddObject>(d_);
                    return Impatient.MaybeTailCall(Impl.AddObject(in_.Object, cancellationToken_), success =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Result_AddObject.WRITER>();
                        var r_ = new Mas.Schema.Storage.Store.Container.Result_AddObject{Success = success};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> RemoveObject(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Params_RemoveObject>(d_);
                    return Impatient.MaybeTailCall(Impl.RemoveObject(in_.Key, cancellationToken_), success =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Result_RemoveObject.WRITER>();
                        var r_ = new Mas.Schema.Storage.Store.Container.Result_RemoveObject{Success = success};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> Clear(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    return Impatient.MaybeTailCall(Impl.Clear(cancellationToken_), success =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Result_Clear.WRITER>();
                        var r_ = new Mas.Schema.Storage.Store.Container.Result_Clear{Success = success};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }
        }

        public static class Container
        {
            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaa5c23f54650f8aeUL)]
            public class Object : ICapnpSerializable
            {
                public const UInt64 typeId = 0xaa5c23f54650f8aeUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Key = reader.Key;
                    Value = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Object.value>(reader.Value);
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Key = Key;
                    Value?.serialize(writer.Value);
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

                public Mas.Schema.Storage.Store.Container.Object.value Value
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
                    public value.READER Value => new value.READER(ctx);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(2, 2);
                    }

                    public string Key
                    {
                        get => this.ReadText(0, null);
                        set => this.WriteText(0, value, null);
                    }

                    public value.WRITER Value
                    {
                        get => Rewrap<value.WRITER>();
                    }
                }

                [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe73a3ffcf21b4e5fUL)]
                public class value : ICapnpSerializable
                {
                    public const UInt64 typeId = 0xe73a3ffcf21b4e5fUL;
                    public enum WHICH : ushort
                    {
                        BoolValue = 0,
                        IntValue = 1,
                        FloatValue = 2,
                        TextValue = 3,
                        DataValue = 4,
                        AnyValue = 5,
                        undefined = 65535
                    }

                    void ICapnpSerializable.Deserialize(DeserializerState arg_)
                    {
                        var reader = READER.create(arg_);
                        switch (reader.which)
                        {
                            case WHICH.BoolValue:
                                BoolValue = reader.BoolValue;
                                break;
                            case WHICH.IntValue:
                                IntValue = reader.IntValue;
                                break;
                            case WHICH.FloatValue:
                                FloatValue = reader.FloatValue;
                                break;
                            case WHICH.TextValue:
                                TextValue = reader.TextValue;
                                break;
                            case WHICH.DataValue:
                                DataValue = reader.DataValue;
                                break;
                            case WHICH.AnyValue:
                                AnyValue = CapnpSerializable.Create<object>(reader.AnyValue);
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
                                case WHICH.BoolValue:
                                    _content = false;
                                    break;
                                case WHICH.IntValue:
                                    _content = 0;
                                    break;
                                case WHICH.FloatValue:
                                    _content = 0;
                                    break;
                                case WHICH.TextValue:
                                    _content = null;
                                    break;
                                case WHICH.DataValue:
                                    _content = null;
                                    break;
                                case WHICH.AnyValue:
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
                            case WHICH.BoolValue:
                                writer.BoolValue = BoolValue.Value;
                                break;
                            case WHICH.IntValue:
                                writer.IntValue = IntValue.Value;
                                break;
                            case WHICH.FloatValue:
                                writer.FloatValue = FloatValue.Value;
                                break;
                            case WHICH.TextValue:
                                writer.TextValue = TextValue;
                                break;
                            case WHICH.DataValue:
                                writer.DataValue.Init(DataValue);
                                break;
                            case WHICH.AnyValue:
                                writer.AnyValue.SetObject(AnyValue);
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

                    public bool? BoolValue
                    {
                        get => _which == WHICH.BoolValue ? (bool? )_content : null;
                        set
                        {
                            _which = WHICH.BoolValue;
                            _content = value;
                        }
                    }

                    public long? IntValue
                    {
                        get => _which == WHICH.IntValue ? (long? )_content : null;
                        set
                        {
                            _which = WHICH.IntValue;
                            _content = value;
                        }
                    }

                    public double? FloatValue
                    {
                        get => _which == WHICH.FloatValue ? (double? )_content : null;
                        set
                        {
                            _which = WHICH.FloatValue;
                            _content = value;
                        }
                    }

                    public string TextValue
                    {
                        get => _which == WHICH.TextValue ? (string)_content : null;
                        set
                        {
                            _which = WHICH.TextValue;
                            _content = value;
                        }
                    }

                    public IReadOnlyList<byte> DataValue
                    {
                        get => _which == WHICH.DataValue ? (IReadOnlyList<byte>)_content : null;
                        set
                        {
                            _which = WHICH.DataValue;
                            _content = value;
                        }
                    }

                    public object AnyValue
                    {
                        get => _which == WHICH.AnyValue ? (object)_content : null;
                        set
                        {
                            _which = WHICH.AnyValue;
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
                        public bool BoolValue => which == WHICH.BoolValue ? ctx.ReadDataBool(0UL, false) : default;
                        public long IntValue => which == WHICH.IntValue ? ctx.ReadDataLong(64UL, 0L) : default;
                        public double FloatValue => which == WHICH.FloatValue ? ctx.ReadDataDouble(64UL, 0) : default;
                        public string TextValue => which == WHICH.TextValue ? ctx.ReadText(1, null) : default;
                        public IReadOnlyList<byte> DataValue => which == WHICH.DataValue ? ctx.ReadList(1).CastByte() : default;
                        public DeserializerState AnyValue => which == WHICH.AnyValue ? ctx.StructReadPointer(1) : default;
                    }

                    public class WRITER : SerializerState
                    {
                        public WRITER()
                        {
                        }

                        public WHICH which
                        {
                            get => (WHICH)this.ReadDataUShort(16U, (ushort)0);
                            set => this.WriteData(16U, (ushort)value, (ushort)0);
                        }

                        public bool BoolValue
                        {
                            get => which == WHICH.BoolValue ? this.ReadDataBool(0UL, false) : default;
                            set => this.WriteData(0UL, value, false);
                        }

                        public long IntValue
                        {
                            get => which == WHICH.IntValue ? this.ReadDataLong(64UL, 0L) : default;
                            set => this.WriteData(64UL, value, 0L);
                        }

                        public double FloatValue
                        {
                            get => which == WHICH.FloatValue ? this.ReadDataDouble(64UL, 0) : default;
                            set => this.WriteData(64UL, value, 0);
                        }

                        public string TextValue
                        {
                            get => which == WHICH.TextValue ? this.ReadText(1, null) : default;
                            set => this.WriteText(1, value, null);
                        }

                        public ListOfPrimitivesSerializer<byte> DataValue
                        {
                            get => which == WHICH.DataValue ? BuildPointer<ListOfPrimitivesSerializer<byte>>(1) : default;
                            set => Link(1, value);
                        }

                        public DynamicSerializerState AnyValue
                        {
                            get => which == WHICH.AnyValue ? BuildPointer<DynamicSerializerState>(1) : default;
                            set => Link(1, value);
                        }
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9e138889be22cc5eUL)]
            public class Params_ImportData : ICapnpSerializable
            {
                public const UInt64 typeId = 0x9e138889be22cc5eUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Data = reader.Data;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Data.Init(Data);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<byte> Data
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
                    public IReadOnlyList<byte> Data => ctx.ReadList(0).CastByte();
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfPrimitivesSerializer<byte> Data
                    {
                        get => BuildPointer<ListOfPrimitivesSerializer<byte>>(0);
                        set => Link(0, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa914844d7351c9eeUL)]
            public class Result_ImportData : ICapnpSerializable
            {
                public const UInt64 typeId = 0xa914844d7351c9eeUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Success = reader.Success;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Success = Success;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public bool Success
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
                    public bool Success => ctx.ReadDataBool(0UL, false);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 0);
                    }

                    public bool Success
                    {
                        get => this.ReadDataBool(0UL, false);
                        set => this.WriteData(0UL, value, false);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x93fc14178e630994UL)]
            public class Params_ExportData : ICapnpSerializable
            {
                public const UInt64 typeId = 0x93fc14178e630994UL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xffe4319ac401d166UL)]
            public class Result_ExportData : ICapnpSerializable
            {
                public const UInt64 typeId = 0xffe4319ac401d166UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Data = reader.Data;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Data.Init(Data);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<byte> Data
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
                    public IReadOnlyList<byte> Data => ctx.ReadList(0).CastByte();
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfPrimitivesSerializer<byte> Data
                    {
                        get => BuildPointer<ListOfPrimitivesSerializer<byte>>(0);
                        set => Link(0, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdbf70a288c6933b1UL)]
            public class Params_ListObjects : ICapnpSerializable
            {
                public const UInt64 typeId = 0xdbf70a288c6933b1UL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa028d3ba03083872UL)]
            public class Result_ListObjects : ICapnpSerializable
            {
                public const UInt64 typeId = 0xa028d3ba03083872UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Objects = reader.Objects?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Object>(_));
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Objects.Init(Objects, (_s1, _v1) => _v1?.serialize(_s1));
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<Mas.Schema.Storage.Store.Container.Object> Objects
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
                    public IReadOnlyList<Mas.Schema.Storage.Store.Container.Object.READER> Objects => ctx.ReadList(0).Cast(Mas.Schema.Storage.Store.Container.Object.READER.create);
                    public bool HasObjects => ctx.IsStructFieldNonNull(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfStructsSerializer<Mas.Schema.Storage.Store.Container.Object.WRITER> Objects
                    {
                        get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Storage.Store.Container.Object.WRITER>>(0);
                        set => Link(0, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc4161d5db43ad669UL)]
            public class Params_GetObject : ICapnpSerializable
            {
                public const UInt64 typeId = 0xc4161d5db43ad669UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Key = reader.Key;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Key = Key;
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

                public struct READER
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
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public string Key
                    {
                        get => this.ReadText(0, null);
                        set => this.WriteText(0, value, null);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9bc1d764a970b846UL)]
            public class Result_GetObject : ICapnpSerializable
            {
                public const UInt64 typeId = 0x9bc1d764a970b846UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Object = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Object>(reader.Object);
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    Object?.serialize(writer.Object);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Schema.Storage.Store.Container.Object Object
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
                    public Mas.Schema.Storage.Store.Container.Object.READER Object => ctx.ReadStruct(0, Mas.Schema.Storage.Store.Container.Object.READER.create);
                    public bool HasObject => ctx.IsStructFieldNonNull(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Schema.Storage.Store.Container.Object.WRITER Object
                    {
                        get => BuildPointer<Mas.Schema.Storage.Store.Container.Object.WRITER>(0);
                        set => Link(0, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfde99170b27ac5ceUL)]
            public class Params_AddObject : ICapnpSerializable
            {
                public const UInt64 typeId = 0xfde99170b27ac5ceUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Object = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Object>(reader.Object);
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    Object?.serialize(writer.Object);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Schema.Storage.Store.Container.Object Object
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
                    public Mas.Schema.Storage.Store.Container.Object.READER Object => ctx.ReadStruct(0, Mas.Schema.Storage.Store.Container.Object.READER.create);
                    public bool HasObject => ctx.IsStructFieldNonNull(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Schema.Storage.Store.Container.Object.WRITER Object
                    {
                        get => BuildPointer<Mas.Schema.Storage.Store.Container.Object.WRITER>(0);
                        set => Link(0, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfbd938c95f64b7bfUL)]
            public class Result_AddObject : ICapnpSerializable
            {
                public const UInt64 typeId = 0xfbd938c95f64b7bfUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Success = reader.Success;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Success = Success;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public bool Success
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
                    public bool Success => ctx.ReadDataBool(0UL, false);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 0);
                    }

                    public bool Success
                    {
                        get => this.ReadDataBool(0UL, false);
                        set => this.WriteData(0UL, value, false);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc31c71f8d67b827bUL)]
            public class Params_RemoveObject : ICapnpSerializable
            {
                public const UInt64 typeId = 0xc31c71f8d67b827bUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Key = reader.Key;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Key = Key;
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

                public struct READER
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
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public string Key
                    {
                        get => this.ReadText(0, null);
                        set => this.WriteText(0, value, null);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xeb6f27dfc29bffadUL)]
            public class Result_RemoveObject : ICapnpSerializable
            {
                public const UInt64 typeId = 0xeb6f27dfc29bffadUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Success = reader.Success;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Success = Success;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public bool Success
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
                    public bool Success => ctx.ReadDataBool(0UL, false);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 0);
                    }

                    public bool Success
                    {
                        get => this.ReadDataBool(0UL, false);
                        set => this.WriteData(0UL, value, false);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfbef00fded9c8312UL)]
            public class Params_Clear : ICapnpSerializable
            {
                public const UInt64 typeId = 0xfbef00fded9c8312UL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x883b57737fba9e54UL)]
            public class Result_Clear : ICapnpSerializable
            {
                public const UInt64 typeId = 0x883b57737fba9e54UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Success = reader.Success;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Success = Success;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public bool Success
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
                    public bool Success => ctx.ReadDataBool(0UL, false);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 0);
                    }

                    public bool Success
                    {
                        get => this.ReadDataBool(0UL, false);
                        set => this.WriteData(0UL, value, false);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf32349bf3a9997acUL)]
        public class Result_NewContainer : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf32349bf3a9997acUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Container = reader.Container;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Container = Container;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Storage.Store.IContainer Container
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
                public Mas.Schema.Storage.Store.IContainer Container => ctx.ReadCap<Mas.Schema.Storage.Store.IContainer>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Storage.Store.IContainer Container
                {
                    get => ReadCap<Mas.Schema.Storage.Store.IContainer>(0);
                    set => LinkObject(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf517bec79f8d2744UL)]
        public class Params_ContainerWithId : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf517bec79f8d2744UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb2af26aeda5445e5UL)]
        public class Result_ContainerWithId : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb2af26aeda5445e5UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Container = reader.Container;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Container = Container;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Storage.Store.IContainer Container
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
                public Mas.Schema.Storage.Store.IContainer Container => ctx.ReadCap<Mas.Schema.Storage.Store.IContainer>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Storage.Store.IContainer Container
                {
                    get => ReadCap<Mas.Schema.Storage.Store.IContainer>(0);
                    set => LinkObject(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa466e92166fcce6eUL)]
        public class Params_ListContainers : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa466e92166fcce6eUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf82426685da256f9UL)]
        public class Result_ListContainers : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf82426685da256f9UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Containers = reader.Containers;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Containers.Init(Containers);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<Mas.Schema.Storage.Store.IContainer> Containers
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
                public IReadOnlyList<Mas.Schema.Storage.Store.IContainer> Containers => ctx.ReadCapList<Mas.Schema.Storage.Store.IContainer>(0);
                public bool HasContainers => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfCapsSerializer<Mas.Schema.Storage.Store.IContainer> Containers
                {
                    get => BuildPointer<ListOfCapsSerializer<Mas.Schema.Storage.Store.IContainer>>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbfbe4f9e7fb62452UL)]
        public class Params_RemoveContainer : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbfbe4f9e7fb62452UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaa0460382685000eUL)]
        public class Result_RemoveContainer : ICapnpSerializable
        {
            public const UInt64 typeId = 0xaa0460382685000eUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Success = reader.Success;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Success = Success;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Success
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
                public bool Success => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Success
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }
    }
}