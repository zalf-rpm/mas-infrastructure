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
    public interface IStore : Mas.Schema.Common.IIdentifiable, Mas.Schema.Persistence.IPersistent
    {
        Task<Mas.Schema.Storage.Store.IContainer> NewContainer(string name, string description, CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Storage.Store.IContainer> ContainerWithId(string id, CancellationToken cancellationToken_ = default);
        Task<IReadOnlyList<Mas.Schema.Storage.Store.IContainer>> ListContainers(CancellationToken cancellationToken_ = default);
        Task<bool> RemoveContainer(string id, CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Storage.Store.IContainer> ImportContainer(string json, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe69f958aa2386f06UL)]
    public class Store_Proxy : Proxy, IStore
    {
        public Task<Mas.Schema.Storage.Store.IContainer> NewContainer(string name, string description, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Params_NewContainer.WRITER>();
            var arg_ = new Mas.Schema.Storage.Store.Params_NewContainer()
            {Name = name, Description = description};
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

        public Task<Mas.Schema.Storage.Store.IContainer> ImportContainer(string json, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Params_ImportContainer.WRITER>();
            var arg_ = new Mas.Schema.Storage.Store.Params_ImportContainer()
            {Json = json};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(16618165572680052486UL, 4, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Result_ImportContainer>(d_);
                    return (r_.Container);
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe69f958aa2386f06UL)]
    public class Store_Skeleton : Skeleton<IStore>
    {
        public Store_Skeleton()
        {
            SetMethodTable(NewContainer, ContainerWithId, ListContainers, RemoveContainer, ImportContainer);
        }

        public override ulong InterfaceId => 16618165572680052486UL;
        Task<AnswerOrCounterquestion> NewContainer(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Params_NewContainer>(d_);
                return Impatient.MaybeTailCall(Impl.NewContainer(in_.Name, in_.Description, cancellationToken_), container =>
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

        Task<AnswerOrCounterquestion> ImportContainer(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Params_ImportContainer>(d_);
                return Impatient.MaybeTailCall(Impl.ImportContainer(in_.Json, cancellationToken_), container =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Result_ImportContainer.WRITER>();
                    var r_ = new Mas.Schema.Storage.Store.Result_ImportContainer{Container = container};
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
            Task<string> Export(CancellationToken cancellationToken_ = default);
            Task<IReadOnlyList<Mas.Schema.Common.Pair<string, Mas.Schema.Storage.Store.Container.Entry.Value>>> DownloadEntries(CancellationToken cancellationToken_ = default);
            Task<IReadOnlyList<Mas.Schema.Storage.Store.Container.IEntry>> ListEntries(CancellationToken cancellationToken_ = default);
            Task<Mas.Schema.Storage.Store.Container.IEntry> GetEntry(string key, CancellationToken cancellationToken_ = default);
            Task<bool> RemoveEntry(string key, CancellationToken cancellationToken_ = default);
            Task<bool> Clear(CancellationToken cancellationToken_ = default);
            Task<(Mas.Schema.Storage.Store.Container.IEntry, bool)> AddEntry(string key, Mas.Schema.Storage.Store.Container.Entry.Value value, bool replaceExisting, CancellationToken cancellationToken_ = default);
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x878131f45567ae62UL)]
        public class Container_Proxy : Proxy, IContainer
        {
            public async Task<string> Export(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Params_Export.WRITER>();
                var arg_ = new Mas.Schema.Storage.Store.Container.Params_Export()
                {};
                arg_?.serialize(in_);
                using (var d_ = await Call(9764140392590585442UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Result_Export>(d_);
                    return (r_.Json);
                }
            }

            public async Task<IReadOnlyList<Mas.Schema.Common.Pair<string, Mas.Schema.Storage.Store.Container.Entry.Value>>> DownloadEntries(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Params_DownloadEntries.WRITER>();
                var arg_ = new Mas.Schema.Storage.Store.Container.Params_DownloadEntries()
                {};
                arg_?.serialize(in_);
                using (var d_ = await Call(9764140392590585442UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Result_DownloadEntries>(d_);
                    return (r_.Entries);
                }
            }

            public Task<IReadOnlyList<Mas.Schema.Storage.Store.Container.IEntry>> ListEntries(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Params_ListEntries.WRITER>();
                var arg_ = new Mas.Schema.Storage.Store.Container.Params_ListEntries()
                {};
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(9764140392590585442UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Result_ListEntries>(d_);
                        return (r_.Entries);
                    }
                }

                );
            }

            public Task<Mas.Schema.Storage.Store.Container.IEntry> GetEntry(string key, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Params_GetEntry.WRITER>();
                var arg_ = new Mas.Schema.Storage.Store.Container.Params_GetEntry()
                {Key = key};
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(9764140392590585442UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Result_GetEntry>(d_);
                        return (r_.Entry);
                    }
                }

                );
            }

            public async Task<bool> RemoveEntry(string key, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Params_RemoveEntry.WRITER>();
                var arg_ = new Mas.Schema.Storage.Store.Container.Params_RemoveEntry()
                {Key = key};
                arg_?.serialize(in_);
                using (var d_ = await Call(9764140392590585442UL, 4, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Result_RemoveEntry>(d_);
                    return (r_.Success);
                }
            }

            public async Task<bool> Clear(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Params_Clear.WRITER>();
                var arg_ = new Mas.Schema.Storage.Store.Container.Params_Clear()
                {};
                arg_?.serialize(in_);
                using (var d_ = await Call(9764140392590585442UL, 5, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Result_Clear>(d_);
                    return (r_.Success);
                }
            }

            public Task<(Mas.Schema.Storage.Store.Container.IEntry, bool)> AddEntry(string key, Mas.Schema.Storage.Store.Container.Entry.Value value, bool replaceExisting, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Params_AddEntry.WRITER>();
                var arg_ = new Mas.Schema.Storage.Store.Container.Params_AddEntry()
                {Key = key, Value = value, ReplaceExisting = replaceExisting};
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(9764140392590585442UL, 6, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Result_AddEntry>(d_);
                        return (r_.Entry, r_.Success);
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x878131f45567ae62UL)]
        public class Container_Skeleton : Skeleton<IContainer>
        {
            public Container_Skeleton()
            {
                SetMethodTable(Export, DownloadEntries, ListEntries, GetEntry, RemoveEntry, Clear, AddEntry);
            }

            public override ulong InterfaceId => 9764140392590585442UL;
            Task<AnswerOrCounterquestion> Export(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    return Impatient.MaybeTailCall(Impl.Export(cancellationToken_), json =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Result_Export.WRITER>();
                        var r_ = new Mas.Schema.Storage.Store.Container.Result_Export{Json = json};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> DownloadEntries(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    return Impatient.MaybeTailCall(Impl.DownloadEntries(cancellationToken_), entries =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Result_DownloadEntries.WRITER>();
                        var r_ = new Mas.Schema.Storage.Store.Container.Result_DownloadEntries{Entries = entries};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> ListEntries(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    return Impatient.MaybeTailCall(Impl.ListEntries(cancellationToken_), entries =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Result_ListEntries.WRITER>();
                        var r_ = new Mas.Schema.Storage.Store.Container.Result_ListEntries{Entries = entries};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> GetEntry(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Params_GetEntry>(d_);
                    return Impatient.MaybeTailCall(Impl.GetEntry(in_.Key, cancellationToken_), entry =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Result_GetEntry.WRITER>();
                        var r_ = new Mas.Schema.Storage.Store.Container.Result_GetEntry{Entry = entry};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }

            Task<AnswerOrCounterquestion> RemoveEntry(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Params_RemoveEntry>(d_);
                    return Impatient.MaybeTailCall(Impl.RemoveEntry(in_.Key, cancellationToken_), success =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Result_RemoveEntry.WRITER>();
                        var r_ = new Mas.Schema.Storage.Store.Container.Result_RemoveEntry{Success = success};
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

            Task<AnswerOrCounterquestion> AddEntry(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Params_AddEntry>(d_);
                    return Impatient.MaybeTailCall(Impl.AddEntry(in_.Key, in_.Value, in_.ReplaceExisting, cancellationToken_), (entry, success) =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Result_AddEntry.WRITER>();
                        var r_ = new Mas.Schema.Storage.Store.Container.Result_AddEntry{Entry = entry, Success = success};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }
        }

        public static class Container
        {
            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfa1a243e7bf478c0UL), Proxy(typeof(Entry_Proxy)), Skeleton(typeof(Entry_Skeleton))]
            public interface IEntry : IDisposable
            {
                Task<string> GetKey(CancellationToken cancellationToken_ = default);
                Task<(Mas.Schema.Storage.Store.Container.Entry.Value, bool)> GetValue(CancellationToken cancellationToken_ = default);
                Task<bool> SetValue(Mas.Schema.Storage.Store.Container.Entry.Value value, CancellationToken cancellationToken_ = default);
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfa1a243e7bf478c0UL)]
            public class Entry_Proxy : Proxy, IEntry
            {
                public async Task<string> GetKey(CancellationToken cancellationToken_ = default)
                {
                    var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Entry.Params_GetKey.WRITER>();
                    var arg_ = new Mas.Schema.Storage.Store.Container.Entry.Params_GetKey()
                    {};
                    arg_?.serialize(in_);
                    using (var d_ = await Call(18021756709662652608UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Entry.Result_GetKey>(d_);
                        return (r_.Key);
                    }
                }

                public async Task<(Mas.Schema.Storage.Store.Container.Entry.Value, bool)> GetValue(CancellationToken cancellationToken_ = default)
                {
                    var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Entry.Params_GetValue.WRITER>();
                    var arg_ = new Mas.Schema.Storage.Store.Container.Entry.Params_GetValue()
                    {};
                    arg_?.serialize(in_);
                    using (var d_ = await Call(18021756709662652608UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Entry.Result_GetValue>(d_);
                        return (r_.Value, r_.IsUnset);
                    }
                }

                public async Task<bool> SetValue(Mas.Schema.Storage.Store.Container.Entry.Value value, CancellationToken cancellationToken_ = default)
                {
                    var in_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Entry.Params_SetValue.WRITER>();
                    var arg_ = new Mas.Schema.Storage.Store.Container.Entry.Params_SetValue()
                    {Value = value};
                    arg_?.serialize(in_);
                    using (var d_ = await Call(18021756709662652608UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Entry.Result_SetValue>(d_);
                        return (r_.Success);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfa1a243e7bf478c0UL)]
            public class Entry_Skeleton : Skeleton<IEntry>
            {
                public Entry_Skeleton()
                {
                    SetMethodTable(GetKey, GetValue, SetValue);
                }

                public override ulong InterfaceId => 18021756709662652608UL;
                Task<AnswerOrCounterquestion> GetKey(DeserializerState d_, CancellationToken cancellationToken_)
                {
                    using (d_)
                    {
                        return Impatient.MaybeTailCall(Impl.GetKey(cancellationToken_), key =>
                        {
                            var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Entry.Result_GetKey.WRITER>();
                            var r_ = new Mas.Schema.Storage.Store.Container.Entry.Result_GetKey{Key = key};
                            r_.serialize(s_);
                            return s_;
                        }

                        );
                    }
                }

                Task<AnswerOrCounterquestion> GetValue(DeserializerState d_, CancellationToken cancellationToken_)
                {
                    using (d_)
                    {
                        return Impatient.MaybeTailCall(Impl.GetValue(cancellationToken_), (value, isUnset) =>
                        {
                            var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Entry.Result_GetValue.WRITER>();
                            var r_ = new Mas.Schema.Storage.Store.Container.Entry.Result_GetValue{Value = value, IsUnset = isUnset};
                            r_.serialize(s_);
                            return s_;
                        }

                        );
                    }
                }

                Task<AnswerOrCounterquestion> SetValue(DeserializerState d_, CancellationToken cancellationToken_)
                {
                    using (d_)
                    {
                        var in_ = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Entry.Params_SetValue>(d_);
                        return Impatient.MaybeTailCall(Impl.SetValue(in_.Value, cancellationToken_), success =>
                        {
                            var s_ = SerializerState.CreateForRpc<Mas.Schema.Storage.Store.Container.Entry.Result_SetValue.WRITER>();
                            var r_ = new Mas.Schema.Storage.Store.Container.Entry.Result_SetValue{Success = success};
                            r_.serialize(s_);
                            return s_;
                        }

                        );
                    }
                }
            }

            public static class Entry
            {
                [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe2185cc449928f5cUL)]
                public class Value : ICapnpSerializable
                {
                    public const UInt64 typeId = 0xe2185cc449928f5cUL;
                    public enum WHICH : ushort
                    {
                        BoolValue = 0,
                        BoolListValue = 1,
                        Int8Value = 2,
                        Int8ListValue = 3,
                        Int16Value = 4,
                        Int16ListValue = 5,
                        Int32Value = 6,
                        Int32ListValue = 7,
                        Int64Value = 8,
                        Int64ListValue = 9,
                        Uint8Value = 10,
                        Uint8ListValue = 11,
                        Uint16Value = 12,
                        Uint16ListValue = 13,
                        Uint32Value = 14,
                        Uint32ListValue = 15,
                        Uint64Value = 16,
                        Uint64ListValue = 17,
                        Float32Value = 18,
                        Float32ListValue = 19,
                        Float64Value = 20,
                        Float64ListValue = 21,
                        TextValue = 22,
                        TextListValue = 23,
                        DataValue = 24,
                        DataListValue = 25,
                        AnyValue = 26,
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
                            case WHICH.BoolListValue:
                                BoolListValue = reader.BoolListValue;
                                break;
                            case WHICH.Int8Value:
                                Int8Value = reader.Int8Value;
                                break;
                            case WHICH.Int8ListValue:
                                Int8ListValue = reader.Int8ListValue;
                                break;
                            case WHICH.Int16Value:
                                Int16Value = reader.Int16Value;
                                break;
                            case WHICH.Int16ListValue:
                                Int16ListValue = reader.Int16ListValue;
                                break;
                            case WHICH.Int32Value:
                                Int32Value = reader.Int32Value;
                                break;
                            case WHICH.Int32ListValue:
                                Int32ListValue = reader.Int32ListValue;
                                break;
                            case WHICH.Int64Value:
                                Int64Value = reader.Int64Value;
                                break;
                            case WHICH.Int64ListValue:
                                Int64ListValue = reader.Int64ListValue;
                                break;
                            case WHICH.Uint8Value:
                                Uint8Value = reader.Uint8Value;
                                break;
                            case WHICH.Uint8ListValue:
                                Uint8ListValue = reader.Uint8ListValue;
                                break;
                            case WHICH.Uint16Value:
                                Uint16Value = reader.Uint16Value;
                                break;
                            case WHICH.Uint16ListValue:
                                Uint16ListValue = reader.Uint16ListValue;
                                break;
                            case WHICH.Uint32Value:
                                Uint32Value = reader.Uint32Value;
                                break;
                            case WHICH.Uint32ListValue:
                                Uint32ListValue = reader.Uint32ListValue;
                                break;
                            case WHICH.Uint64Value:
                                Uint64Value = reader.Uint64Value;
                                break;
                            case WHICH.Uint64ListValue:
                                Uint64ListValue = reader.Uint64ListValue;
                                break;
                            case WHICH.Float32Value:
                                Float32Value = reader.Float32Value;
                                break;
                            case WHICH.Float32ListValue:
                                Float32ListValue = reader.Float32ListValue;
                                break;
                            case WHICH.Float64Value:
                                Float64Value = reader.Float64Value;
                                break;
                            case WHICH.Float64ListValue:
                                Float64ListValue = reader.Float64ListValue;
                                break;
                            case WHICH.TextValue:
                                TextValue = reader.TextValue;
                                break;
                            case WHICH.TextListValue:
                                TextListValue = reader.TextListValue;
                                break;
                            case WHICH.DataValue:
                                DataValue = reader.DataValue;
                                break;
                            case WHICH.DataListValue:
                                DataListValue = reader.DataListValue;
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
                                case WHICH.BoolListValue:
                                    _content = null;
                                    break;
                                case WHICH.Int8Value:
                                    _content = 0;
                                    break;
                                case WHICH.Int8ListValue:
                                    _content = null;
                                    break;
                                case WHICH.Int16Value:
                                    _content = 0;
                                    break;
                                case WHICH.Int16ListValue:
                                    _content = null;
                                    break;
                                case WHICH.Int32Value:
                                    _content = 0;
                                    break;
                                case WHICH.Int32ListValue:
                                    _content = null;
                                    break;
                                case WHICH.Int64Value:
                                    _content = 0;
                                    break;
                                case WHICH.Int64ListValue:
                                    _content = null;
                                    break;
                                case WHICH.Uint8Value:
                                    _content = 0;
                                    break;
                                case WHICH.Uint8ListValue:
                                    _content = null;
                                    break;
                                case WHICH.Uint16Value:
                                    _content = 0;
                                    break;
                                case WHICH.Uint16ListValue:
                                    _content = null;
                                    break;
                                case WHICH.Uint32Value:
                                    _content = 0;
                                    break;
                                case WHICH.Uint32ListValue:
                                    _content = null;
                                    break;
                                case WHICH.Uint64Value:
                                    _content = 0;
                                    break;
                                case WHICH.Uint64ListValue:
                                    _content = null;
                                    break;
                                case WHICH.Float32Value:
                                    _content = 0F;
                                    break;
                                case WHICH.Float32ListValue:
                                    _content = null;
                                    break;
                                case WHICH.Float64Value:
                                    _content = 0;
                                    break;
                                case WHICH.Float64ListValue:
                                    _content = null;
                                    break;
                                case WHICH.TextValue:
                                    _content = null;
                                    break;
                                case WHICH.TextListValue:
                                    _content = null;
                                    break;
                                case WHICH.DataValue:
                                    _content = null;
                                    break;
                                case WHICH.DataListValue:
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
                            case WHICH.BoolListValue:
                                writer.BoolListValue.Init(BoolListValue);
                                break;
                            case WHICH.Int8Value:
                                writer.Int8Value = Int8Value.Value;
                                break;
                            case WHICH.Int8ListValue:
                                writer.Int8ListValue.Init(Int8ListValue);
                                break;
                            case WHICH.Int16Value:
                                writer.Int16Value = Int16Value.Value;
                                break;
                            case WHICH.Int16ListValue:
                                writer.Int16ListValue.Init(Int16ListValue);
                                break;
                            case WHICH.Int32Value:
                                writer.Int32Value = Int32Value.Value;
                                break;
                            case WHICH.Int32ListValue:
                                writer.Int32ListValue.Init(Int32ListValue);
                                break;
                            case WHICH.Int64Value:
                                writer.Int64Value = Int64Value.Value;
                                break;
                            case WHICH.Int64ListValue:
                                writer.Int64ListValue.Init(Int64ListValue);
                                break;
                            case WHICH.Uint8Value:
                                writer.Uint8Value = Uint8Value.Value;
                                break;
                            case WHICH.Uint8ListValue:
                                writer.Uint8ListValue.Init(Uint8ListValue);
                                break;
                            case WHICH.Uint16Value:
                                writer.Uint16Value = Uint16Value.Value;
                                break;
                            case WHICH.Uint16ListValue:
                                writer.Uint16ListValue.Init(Uint16ListValue);
                                break;
                            case WHICH.Uint32Value:
                                writer.Uint32Value = Uint32Value.Value;
                                break;
                            case WHICH.Uint32ListValue:
                                writer.Uint32ListValue.Init(Uint32ListValue);
                                break;
                            case WHICH.Uint64Value:
                                writer.Uint64Value = Uint64Value.Value;
                                break;
                            case WHICH.Uint64ListValue:
                                writer.Uint64ListValue.Init(Uint64ListValue);
                                break;
                            case WHICH.Float32Value:
                                writer.Float32Value = Float32Value.Value;
                                break;
                            case WHICH.Float32ListValue:
                                writer.Float32ListValue.Init(Float32ListValue);
                                break;
                            case WHICH.Float64Value:
                                writer.Float64Value = Float64Value.Value;
                                break;
                            case WHICH.Float64ListValue:
                                writer.Float64ListValue.Init(Float64ListValue);
                                break;
                            case WHICH.TextValue:
                                writer.TextValue = TextValue;
                                break;
                            case WHICH.TextListValue:
                                writer.TextListValue.Init(TextListValue);
                                break;
                            case WHICH.DataValue:
                                writer.DataValue.Init(DataValue);
                                break;
                            case WHICH.DataListValue:
                                writer.DataListValue.Init(DataListValue, (_s1, _v1) => _s1.Init(_v1));
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

                    public IReadOnlyList<bool> BoolListValue
                    {
                        get => _which == WHICH.BoolListValue ? (IReadOnlyList<bool>)_content : null;
                        set
                        {
                            _which = WHICH.BoolListValue;
                            _content = value;
                        }
                    }

                    public sbyte? Int8Value
                    {
                        get => _which == WHICH.Int8Value ? (sbyte? )_content : null;
                        set
                        {
                            _which = WHICH.Int8Value;
                            _content = value;
                        }
                    }

                    public IReadOnlyList<sbyte> Int8ListValue
                    {
                        get => _which == WHICH.Int8ListValue ? (IReadOnlyList<sbyte>)_content : null;
                        set
                        {
                            _which = WHICH.Int8ListValue;
                            _content = value;
                        }
                    }

                    public short? Int16Value
                    {
                        get => _which == WHICH.Int16Value ? (short? )_content : null;
                        set
                        {
                            _which = WHICH.Int16Value;
                            _content = value;
                        }
                    }

                    public IReadOnlyList<short> Int16ListValue
                    {
                        get => _which == WHICH.Int16ListValue ? (IReadOnlyList<short>)_content : null;
                        set
                        {
                            _which = WHICH.Int16ListValue;
                            _content = value;
                        }
                    }

                    public int? Int32Value
                    {
                        get => _which == WHICH.Int32Value ? (int? )_content : null;
                        set
                        {
                            _which = WHICH.Int32Value;
                            _content = value;
                        }
                    }

                    public IReadOnlyList<int> Int32ListValue
                    {
                        get => _which == WHICH.Int32ListValue ? (IReadOnlyList<int>)_content : null;
                        set
                        {
                            _which = WHICH.Int32ListValue;
                            _content = value;
                        }
                    }

                    public long? Int64Value
                    {
                        get => _which == WHICH.Int64Value ? (long? )_content : null;
                        set
                        {
                            _which = WHICH.Int64Value;
                            _content = value;
                        }
                    }

                    public IReadOnlyList<long> Int64ListValue
                    {
                        get => _which == WHICH.Int64ListValue ? (IReadOnlyList<long>)_content : null;
                        set
                        {
                            _which = WHICH.Int64ListValue;
                            _content = value;
                        }
                    }

                    public byte? Uint8Value
                    {
                        get => _which == WHICH.Uint8Value ? (byte? )_content : null;
                        set
                        {
                            _which = WHICH.Uint8Value;
                            _content = value;
                        }
                    }

                    public IReadOnlyList<byte> Uint8ListValue
                    {
                        get => _which == WHICH.Uint8ListValue ? (IReadOnlyList<byte>)_content : null;
                        set
                        {
                            _which = WHICH.Uint8ListValue;
                            _content = value;
                        }
                    }

                    public ushort? Uint16Value
                    {
                        get => _which == WHICH.Uint16Value ? (ushort? )_content : null;
                        set
                        {
                            _which = WHICH.Uint16Value;
                            _content = value;
                        }
                    }

                    public IReadOnlyList<ushort> Uint16ListValue
                    {
                        get => _which == WHICH.Uint16ListValue ? (IReadOnlyList<ushort>)_content : null;
                        set
                        {
                            _which = WHICH.Uint16ListValue;
                            _content = value;
                        }
                    }

                    public uint? Uint32Value
                    {
                        get => _which == WHICH.Uint32Value ? (uint? )_content : null;
                        set
                        {
                            _which = WHICH.Uint32Value;
                            _content = value;
                        }
                    }

                    public IReadOnlyList<uint> Uint32ListValue
                    {
                        get => _which == WHICH.Uint32ListValue ? (IReadOnlyList<uint>)_content : null;
                        set
                        {
                            _which = WHICH.Uint32ListValue;
                            _content = value;
                        }
                    }

                    public ulong? Uint64Value
                    {
                        get => _which == WHICH.Uint64Value ? (ulong? )_content : null;
                        set
                        {
                            _which = WHICH.Uint64Value;
                            _content = value;
                        }
                    }

                    public IReadOnlyList<ulong> Uint64ListValue
                    {
                        get => _which == WHICH.Uint64ListValue ? (IReadOnlyList<ulong>)_content : null;
                        set
                        {
                            _which = WHICH.Uint64ListValue;
                            _content = value;
                        }
                    }

                    public float? Float32Value
                    {
                        get => _which == WHICH.Float32Value ? (float? )_content : null;
                        set
                        {
                            _which = WHICH.Float32Value;
                            _content = value;
                        }
                    }

                    public IReadOnlyList<float> Float32ListValue
                    {
                        get => _which == WHICH.Float32ListValue ? (IReadOnlyList<float>)_content : null;
                        set
                        {
                            _which = WHICH.Float32ListValue;
                            _content = value;
                        }
                    }

                    public double? Float64Value
                    {
                        get => _which == WHICH.Float64Value ? (double? )_content : null;
                        set
                        {
                            _which = WHICH.Float64Value;
                            _content = value;
                        }
                    }

                    public IReadOnlyList<double> Float64ListValue
                    {
                        get => _which == WHICH.Float64ListValue ? (IReadOnlyList<double>)_content : null;
                        set
                        {
                            _which = WHICH.Float64ListValue;
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

                    public IReadOnlyList<string> TextListValue
                    {
                        get => _which == WHICH.TextListValue ? (IReadOnlyList<string>)_content : null;
                        set
                        {
                            _which = WHICH.TextListValue;
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

                    public IReadOnlyList<IReadOnlyList<byte>> DataListValue
                    {
                        get => _which == WHICH.DataListValue ? (IReadOnlyList<IReadOnlyList<byte>>)_content : null;
                        set
                        {
                            _which = WHICH.DataListValue;
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
                        public IReadOnlyList<bool> BoolListValue => which == WHICH.BoolListValue ? ctx.ReadList(0).CastBool() : default;
                        public bool HasBoolListValue => ctx.IsStructFieldNonNull(0);
                        public sbyte Int8Value => which == WHICH.Int8Value ? ctx.ReadDataSByte(0UL, (sbyte)0) : default;
                        public IReadOnlyList<sbyte> Int8ListValue => which == WHICH.Int8ListValue ? ctx.ReadList(0).CastSByte() : default;
                        public bool HasInt8ListValue => ctx.IsStructFieldNonNull(0);
                        public short Int16Value => which == WHICH.Int16Value ? ctx.ReadDataShort(0UL, (short)0) : default;
                        public IReadOnlyList<short> Int16ListValue => which == WHICH.Int16ListValue ? ctx.ReadList(0).CastShort() : default;
                        public bool HasInt16ListValue => ctx.IsStructFieldNonNull(0);
                        public int Int32Value => which == WHICH.Int32Value ? ctx.ReadDataInt(32UL, 0) : default;
                        public IReadOnlyList<int> Int32ListValue => which == WHICH.Int32ListValue ? ctx.ReadList(0).CastInt() : default;
                        public bool HasInt32ListValue => ctx.IsStructFieldNonNull(0);
                        public long Int64Value => which == WHICH.Int64Value ? ctx.ReadDataLong(64UL, 0L) : default;
                        public IReadOnlyList<long> Int64ListValue => which == WHICH.Int64ListValue ? ctx.ReadList(0).CastLong() : default;
                        public bool HasInt64ListValue => ctx.IsStructFieldNonNull(0);
                        public byte Uint8Value => which == WHICH.Uint8Value ? ctx.ReadDataByte(0UL, (byte)0) : default;
                        public IReadOnlyList<byte> Uint8ListValue => which == WHICH.Uint8ListValue ? ctx.ReadList(0).CastByte() : default;
                        public bool HasUint8ListValue => ctx.IsStructFieldNonNull(0);
                        public ushort Uint16Value => which == WHICH.Uint16Value ? ctx.ReadDataUShort(0UL, (ushort)0) : default;
                        public IReadOnlyList<ushort> Uint16ListValue => which == WHICH.Uint16ListValue ? ctx.ReadList(0).CastUShort() : default;
                        public bool HasUint16ListValue => ctx.IsStructFieldNonNull(0);
                        public uint Uint32Value => which == WHICH.Uint32Value ? ctx.ReadDataUInt(32UL, 0U) : default;
                        public IReadOnlyList<uint> Uint32ListValue => which == WHICH.Uint32ListValue ? ctx.ReadList(0).CastUInt() : default;
                        public bool HasUint32ListValue => ctx.IsStructFieldNonNull(0);
                        public ulong Uint64Value => which == WHICH.Uint64Value ? ctx.ReadDataULong(64UL, 0UL) : default;
                        public IReadOnlyList<ulong> Uint64ListValue => which == WHICH.Uint64ListValue ? ctx.ReadList(0).CastULong() : default;
                        public bool HasUint64ListValue => ctx.IsStructFieldNonNull(0);
                        public float Float32Value => which == WHICH.Float32Value ? ctx.ReadDataFloat(32UL, 0F) : default;
                        public IReadOnlyList<float> Float32ListValue => which == WHICH.Float32ListValue ? ctx.ReadList(0).CastFloat() : default;
                        public bool HasFloat32ListValue => ctx.IsStructFieldNonNull(0);
                        public double Float64Value => which == WHICH.Float64Value ? ctx.ReadDataDouble(64UL, 0) : default;
                        public IReadOnlyList<double> Float64ListValue => which == WHICH.Float64ListValue ? ctx.ReadList(0).CastDouble() : default;
                        public bool HasFloat64ListValue => ctx.IsStructFieldNonNull(0);
                        public string TextValue => which == WHICH.TextValue ? ctx.ReadText(0, null) : default;
                        public IReadOnlyList<string> TextListValue => which == WHICH.TextListValue ? ctx.ReadList(0).CastText2() : default;
                        public bool HasTextListValue => ctx.IsStructFieldNonNull(0);
                        public IReadOnlyList<byte> DataValue => which == WHICH.DataValue ? ctx.ReadList(0).CastByte() : default;
                        public IReadOnlyList<IReadOnlyList<byte>> DataListValue => which == WHICH.DataListValue ? ctx.ReadList(0).CastData() : default;
                        public bool HasDataListValue => ctx.IsStructFieldNonNull(0);
                        public DeserializerState AnyValue => which == WHICH.AnyValue ? ctx.StructReadPointer(0) : default;
                    }

                    public class WRITER : SerializerState
                    {
                        public WRITER()
                        {
                            this.SetStruct(2, 1);
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

                        public ListOfBitsSerializer BoolListValue
                        {
                            get => which == WHICH.BoolListValue ? BuildPointer<ListOfBitsSerializer>(0) : default;
                            set => Link(0, value);
                        }

                        public sbyte Int8Value
                        {
                            get => which == WHICH.Int8Value ? this.ReadDataSByte(0UL, (sbyte)0) : default;
                            set => this.WriteData(0UL, value, (sbyte)0);
                        }

                        public ListOfPrimitivesSerializer<sbyte> Int8ListValue
                        {
                            get => which == WHICH.Int8ListValue ? BuildPointer<ListOfPrimitivesSerializer<sbyte>>(0) : default;
                            set => Link(0, value);
                        }

                        public short Int16Value
                        {
                            get => which == WHICH.Int16Value ? this.ReadDataShort(0UL, (short)0) : default;
                            set => this.WriteData(0UL, value, (short)0);
                        }

                        public ListOfPrimitivesSerializer<short> Int16ListValue
                        {
                            get => which == WHICH.Int16ListValue ? BuildPointer<ListOfPrimitivesSerializer<short>>(0) : default;
                            set => Link(0, value);
                        }

                        public int Int32Value
                        {
                            get => which == WHICH.Int32Value ? this.ReadDataInt(32UL, 0) : default;
                            set => this.WriteData(32UL, value, 0);
                        }

                        public ListOfPrimitivesSerializer<int> Int32ListValue
                        {
                            get => which == WHICH.Int32ListValue ? BuildPointer<ListOfPrimitivesSerializer<int>>(0) : default;
                            set => Link(0, value);
                        }

                        public long Int64Value
                        {
                            get => which == WHICH.Int64Value ? this.ReadDataLong(64UL, 0L) : default;
                            set => this.WriteData(64UL, value, 0L);
                        }

                        public ListOfPrimitivesSerializer<long> Int64ListValue
                        {
                            get => which == WHICH.Int64ListValue ? BuildPointer<ListOfPrimitivesSerializer<long>>(0) : default;
                            set => Link(0, value);
                        }

                        public byte Uint8Value
                        {
                            get => which == WHICH.Uint8Value ? this.ReadDataByte(0UL, (byte)0) : default;
                            set => this.WriteData(0UL, value, (byte)0);
                        }

                        public ListOfPrimitivesSerializer<byte> Uint8ListValue
                        {
                            get => which == WHICH.Uint8ListValue ? BuildPointer<ListOfPrimitivesSerializer<byte>>(0) : default;
                            set => Link(0, value);
                        }

                        public ushort Uint16Value
                        {
                            get => which == WHICH.Uint16Value ? this.ReadDataUShort(0UL, (ushort)0) : default;
                            set => this.WriteData(0UL, value, (ushort)0);
                        }

                        public ListOfPrimitivesSerializer<ushort> Uint16ListValue
                        {
                            get => which == WHICH.Uint16ListValue ? BuildPointer<ListOfPrimitivesSerializer<ushort>>(0) : default;
                            set => Link(0, value);
                        }

                        public uint Uint32Value
                        {
                            get => which == WHICH.Uint32Value ? this.ReadDataUInt(32UL, 0U) : default;
                            set => this.WriteData(32UL, value, 0U);
                        }

                        public ListOfPrimitivesSerializer<uint> Uint32ListValue
                        {
                            get => which == WHICH.Uint32ListValue ? BuildPointer<ListOfPrimitivesSerializer<uint>>(0) : default;
                            set => Link(0, value);
                        }

                        public ulong Uint64Value
                        {
                            get => which == WHICH.Uint64Value ? this.ReadDataULong(64UL, 0UL) : default;
                            set => this.WriteData(64UL, value, 0UL);
                        }

                        public ListOfPrimitivesSerializer<ulong> Uint64ListValue
                        {
                            get => which == WHICH.Uint64ListValue ? BuildPointer<ListOfPrimitivesSerializer<ulong>>(0) : default;
                            set => Link(0, value);
                        }

                        public float Float32Value
                        {
                            get => which == WHICH.Float32Value ? this.ReadDataFloat(32UL, 0F) : default;
                            set => this.WriteData(32UL, value, 0F);
                        }

                        public ListOfPrimitivesSerializer<float> Float32ListValue
                        {
                            get => which == WHICH.Float32ListValue ? BuildPointer<ListOfPrimitivesSerializer<float>>(0) : default;
                            set => Link(0, value);
                        }

                        public double Float64Value
                        {
                            get => which == WHICH.Float64Value ? this.ReadDataDouble(64UL, 0) : default;
                            set => this.WriteData(64UL, value, 0);
                        }

                        public ListOfPrimitivesSerializer<double> Float64ListValue
                        {
                            get => which == WHICH.Float64ListValue ? BuildPointer<ListOfPrimitivesSerializer<double>>(0) : default;
                            set => Link(0, value);
                        }

                        public string TextValue
                        {
                            get => which == WHICH.TextValue ? this.ReadText(0, null) : default;
                            set => this.WriteText(0, value, null);
                        }

                        public ListOfTextSerializer TextListValue
                        {
                            get => which == WHICH.TextListValue ? BuildPointer<ListOfTextSerializer>(0) : default;
                            set => Link(0, value);
                        }

                        public ListOfPrimitivesSerializer<byte> DataValue
                        {
                            get => which == WHICH.DataValue ? BuildPointer<ListOfPrimitivesSerializer<byte>>(0) : default;
                            set => Link(0, value);
                        }

                        public ListOfPointersSerializer<ListOfPrimitivesSerializer<byte>> DataListValue
                        {
                            get => which == WHICH.DataListValue ? BuildPointer<ListOfPointersSerializer<ListOfPrimitivesSerializer<byte>>>(0) : default;
                            set => Link(0, value);
                        }

                        public DynamicSerializerState AnyValue
                        {
                            get => which == WHICH.AnyValue ? BuildPointer<DynamicSerializerState>(0) : default;
                            set => Link(0, value);
                        }
                    }
                }

                [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe0647ffea942d00aUL)]
                public class Params_GetKey : ICapnpSerializable
                {
                    public const UInt64 typeId = 0xe0647ffea942d00aUL;
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

                [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdbfb7e9990643f87UL)]
                public class Result_GetKey : ICapnpSerializable
                {
                    public const UInt64 typeId = 0xdbfb7e9990643f87UL;
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

                [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x923c06d58238b290UL)]
                public class Params_GetValue : ICapnpSerializable
                {
                    public const UInt64 typeId = 0x923c06d58238b290UL;
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

                [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc5e6024b9f05560eUL)]
                public class Result_GetValue : ICapnpSerializable
                {
                    public const UInt64 typeId = 0xc5e6024b9f05560eUL;
                    void ICapnpSerializable.Deserialize(DeserializerState arg_)
                    {
                        var reader = READER.create(arg_);
                        Value = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Entry.Value>(reader.Value);
                        IsUnset = reader.IsUnset;
                        applyDefaults();
                    }

                    public void serialize(WRITER writer)
                    {
                        Value?.serialize(writer.Value);
                        writer.IsUnset = IsUnset;
                    }

                    void ICapnpSerializable.Serialize(SerializerState arg_)
                    {
                        serialize(arg_.Rewrap<WRITER>());
                    }

                    public void applyDefaults()
                    {
                    }

                    public Mas.Schema.Storage.Store.Container.Entry.Value Value
                    {
                        get;
                        set;
                    }

                    public bool IsUnset
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
                        public Mas.Schema.Storage.Store.Container.Entry.Value.READER Value => ctx.ReadStruct(0, Mas.Schema.Storage.Store.Container.Entry.Value.READER.create);
                        public bool HasValue => ctx.IsStructFieldNonNull(0);
                        public bool IsUnset => ctx.ReadDataBool(0UL, false);
                    }

                    public class WRITER : SerializerState
                    {
                        public WRITER()
                        {
                            this.SetStruct(1, 1);
                        }

                        public Mas.Schema.Storage.Store.Container.Entry.Value.WRITER Value
                        {
                            get => BuildPointer<Mas.Schema.Storage.Store.Container.Entry.Value.WRITER>(0);
                            set => Link(0, value);
                        }

                        public bool IsUnset
                        {
                            get => this.ReadDataBool(0UL, false);
                            set => this.WriteData(0UL, value, false);
                        }
                    }
                }

                [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa4ff24aa7f0debafUL)]
                public class Params_SetValue : ICapnpSerializable
                {
                    public const UInt64 typeId = 0xa4ff24aa7f0debafUL;
                    void ICapnpSerializable.Deserialize(DeserializerState arg_)
                    {
                        var reader = READER.create(arg_);
                        Value = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Entry.Value>(reader.Value);
                        applyDefaults();
                    }

                    public void serialize(WRITER writer)
                    {
                        Value?.serialize(writer.Value);
                    }

                    void ICapnpSerializable.Serialize(SerializerState arg_)
                    {
                        serialize(arg_.Rewrap<WRITER>());
                    }

                    public void applyDefaults()
                    {
                    }

                    public Mas.Schema.Storage.Store.Container.Entry.Value Value
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
                        public Mas.Schema.Storage.Store.Container.Entry.Value.READER Value => ctx.ReadStruct(0, Mas.Schema.Storage.Store.Container.Entry.Value.READER.create);
                        public bool HasValue => ctx.IsStructFieldNonNull(0);
                    }

                    public class WRITER : SerializerState
                    {
                        public WRITER()
                        {
                            this.SetStruct(0, 1);
                        }

                        public Mas.Schema.Storage.Store.Container.Entry.Value.WRITER Value
                        {
                            get => BuildPointer<Mas.Schema.Storage.Store.Container.Entry.Value.WRITER>(0);
                            set => Link(0, value);
                        }
                    }
                }

                [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd667b97e089bae01UL)]
                public class Result_SetValue : ICapnpSerializable
                {
                    public const UInt64 typeId = 0xd667b97e089bae01UL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9e138889be22cc5eUL)]
            public class Params_Export : ICapnpSerializable
            {
                public const UInt64 typeId = 0x9e138889be22cc5eUL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa914844d7351c9eeUL)]
            public class Result_Export : ICapnpSerializable
            {
                public const UInt64 typeId = 0xa914844d7351c9eeUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Json = reader.Json;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Json = Json;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public string Json
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
                    public string Json => ctx.ReadText(0, null);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public string Json
                    {
                        get => this.ReadText(0, null);
                        set => this.WriteText(0, value, null);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x93fc14178e630994UL)]
            public class Params_DownloadEntries : ICapnpSerializable
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
            public class Result_DownloadEntries : ICapnpSerializable
            {
                public const UInt64 typeId = 0xffe4319ac401d166UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Entries = reader.Entries?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Common.Pair<string, Mas.Schema.Storage.Store.Container.Entry.Value>>(_));
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Entries.Init(Entries, (_s1, _v1) => _v1?.serialize(_s1));
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<Mas.Schema.Common.Pair<string, Mas.Schema.Storage.Store.Container.Entry.Value>> Entries
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
                    public IReadOnlyList<Mas.Schema.Common.Pair<string, Mas.Schema.Storage.Store.Container.Entry.Value>.READER> Entries => ctx.ReadList(0).Cast(Mas.Schema.Common.Pair<string, Mas.Schema.Storage.Store.Container.Entry.Value>.READER.create);
                    public bool HasEntries => ctx.IsStructFieldNonNull(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfStructsSerializer<Mas.Schema.Common.Pair<string, Mas.Schema.Storage.Store.Container.Entry.Value>.WRITER> Entries
                    {
                        get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Common.Pair<string, Mas.Schema.Storage.Store.Container.Entry.Value>.WRITER>>(0);
                        set => Link(0, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdbf70a288c6933b1UL)]
            public class Params_ListEntries : ICapnpSerializable
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
            public class Result_ListEntries : ICapnpSerializable
            {
                public const UInt64 typeId = 0xa028d3ba03083872UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Entries = reader.Entries;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Entries.Init(Entries);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<Mas.Schema.Storage.Store.Container.IEntry> Entries
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
                    public IReadOnlyList<Mas.Schema.Storage.Store.Container.IEntry> Entries => ctx.ReadCapList<Mas.Schema.Storage.Store.Container.IEntry>(0);
                    public bool HasEntries => ctx.IsStructFieldNonNull(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfCapsSerializer<Mas.Schema.Storage.Store.Container.IEntry> Entries
                    {
                        get => BuildPointer<ListOfCapsSerializer<Mas.Schema.Storage.Store.Container.IEntry>>(0);
                        set => Link(0, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc4161d5db43ad669UL)]
            public class Params_GetEntry : ICapnpSerializable
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
            public class Result_GetEntry : ICapnpSerializable
            {
                public const UInt64 typeId = 0x9bc1d764a970b846UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Entry = reader.Entry;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Entry = Entry;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Schema.Storage.Store.Container.IEntry Entry
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
                    public Mas.Schema.Storage.Store.Container.IEntry Entry => ctx.ReadCap<Mas.Schema.Storage.Store.Container.IEntry>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Schema.Storage.Store.Container.IEntry Entry
                    {
                        get => ReadCap<Mas.Schema.Storage.Store.Container.IEntry>(0);
                        set => LinkObject(0, value);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfde99170b27ac5ceUL)]
            public class Params_RemoveEntry : ICapnpSerializable
            {
                public const UInt64 typeId = 0xfde99170b27ac5ceUL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfbd938c95f64b7bfUL)]
            public class Result_RemoveEntry : ICapnpSerializable
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
            public class Params_Clear : ICapnpSerializable
            {
                public const UInt64 typeId = 0xc31c71f8d67b827bUL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xeb6f27dfc29bffadUL)]
            public class Result_Clear : ICapnpSerializable
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
            public class Params_AddEntry : ICapnpSerializable
            {
                public const UInt64 typeId = 0xfbef00fded9c8312UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Key = reader.Key;
                    Value = CapnpSerializable.Create<Mas.Schema.Storage.Store.Container.Entry.Value>(reader.Value);
                    ReplaceExisting = reader.ReplaceExisting;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Key = Key;
                    Value?.serialize(writer.Value);
                    writer.ReplaceExisting = ReplaceExisting;
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

                public Mas.Schema.Storage.Store.Container.Entry.Value Value
                {
                    get;
                    set;
                }

                public bool ReplaceExisting
                {
                    get;
                    set;
                }

                = false;
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
                    public Mas.Schema.Storage.Store.Container.Entry.Value.READER Value => ctx.ReadStruct(1, Mas.Schema.Storage.Store.Container.Entry.Value.READER.create);
                    public bool HasValue => ctx.IsStructFieldNonNull(1);
                    public bool ReplaceExisting => ctx.ReadDataBool(0UL, false);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 2);
                    }

                    public string Key
                    {
                        get => this.ReadText(0, null);
                        set => this.WriteText(0, value, null);
                    }

                    public Mas.Schema.Storage.Store.Container.Entry.Value.WRITER Value
                    {
                        get => BuildPointer<Mas.Schema.Storage.Store.Container.Entry.Value.WRITER>(1);
                        set => Link(1, value);
                    }

                    public bool ReplaceExisting
                    {
                        get => this.ReadDataBool(0UL, false);
                        set => this.WriteData(0UL, value, false);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x883b57737fba9e54UL)]
            public class Result_AddEntry : ICapnpSerializable
            {
                public const UInt64 typeId = 0x883b57737fba9e54UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Entry = reader.Entry;
                    Success = reader.Success;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Entry = Entry;
                    writer.Success = Success;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Schema.Storage.Store.Container.IEntry Entry
                {
                    get;
                    set;
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
                    public Mas.Schema.Storage.Store.Container.IEntry Entry => ctx.ReadCap<Mas.Schema.Storage.Store.Container.IEntry>(0);
                    public bool Success => ctx.ReadDataBool(0UL, false);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 1);
                    }

                    public Mas.Schema.Storage.Store.Container.IEntry Entry
                    {
                        get => ReadCap<Mas.Schema.Storage.Store.Container.IEntry>(0);
                        set => LinkObject(0, value);
                    }

                    public bool Success
                    {
                        get => this.ReadDataBool(0UL, false);
                        set => this.WriteData(0UL, value, false);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x847d262cefd2f142UL)]
        public class ImportExportData : ICapnpSerializable
        {
            public const UInt64 typeId = 0x847d262cefd2f142UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Info = CapnpSerializable.Create<Mas.Schema.Common.IdInformation>(reader.Info);
                Entries = reader.Entries?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Common.Pair<string, Mas.Schema.Storage.Store.Container.Entry.Value>>(_));
                IsAnyValue = reader.IsAnyValue;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Info?.serialize(writer.Info);
                writer.Entries.Init(Entries, (_s1, _v1) => _v1?.serialize(_s1));
                writer.IsAnyValue.Init(IsAnyValue);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Common.IdInformation Info
            {
                get;
                set;
            }

            public IReadOnlyList<Mas.Schema.Common.Pair<string, Mas.Schema.Storage.Store.Container.Entry.Value>> Entries
            {
                get;
                set;
            }

            public IReadOnlyList<bool> IsAnyValue
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
                public Mas.Schema.Common.IdInformation.READER Info => ctx.ReadStruct(0, Mas.Schema.Common.IdInformation.READER.create);
                public bool HasInfo => ctx.IsStructFieldNonNull(0);
                public IReadOnlyList<Mas.Schema.Common.Pair<string, Mas.Schema.Storage.Store.Container.Entry.Value>.READER> Entries => ctx.ReadList(1).Cast(Mas.Schema.Common.Pair<string, Mas.Schema.Storage.Store.Container.Entry.Value>.READER.create);
                public bool HasEntries => ctx.IsStructFieldNonNull(1);
                public IReadOnlyList<bool> IsAnyValue => ctx.ReadList(2).CastBool();
                public bool HasIsAnyValue => ctx.IsStructFieldNonNull(2);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 3);
                }

                public Mas.Schema.Common.IdInformation.WRITER Info
                {
                    get => BuildPointer<Mas.Schema.Common.IdInformation.WRITER>(0);
                    set => Link(0, value);
                }

                public ListOfStructsSerializer<Mas.Schema.Common.Pair<string, Mas.Schema.Storage.Store.Container.Entry.Value>.WRITER> Entries
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Common.Pair<string, Mas.Schema.Storage.Store.Container.Entry.Value>.WRITER>>(1);
                    set => Link(1, value);
                }

                public ListOfBitsSerializer IsAnyValue
                {
                    get => BuildPointer<ListOfBitsSerializer>(2);
                    set => Link(2, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbc4cb84d672b9bf6UL)]
        public class Params_NewContainer : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbc4cb84d672b9bf6UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Name = reader.Name;
                Description = reader.Description;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
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
                public string Name => ctx.ReadText(0, null);
                public string Description => ctx.ReadText(1, null);
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

                public string Description
                {
                    get => this.ReadText(1, null);
                    set => this.WriteText(1, value, null);
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xba3e5ec40217ab32UL)]
        public class Params_ImportContainer : ICapnpSerializable
        {
            public const UInt64 typeId = 0xba3e5ec40217ab32UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Json = reader.Json;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Json = Json;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Json
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
                public string Json => ctx.ReadText(0, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string Json
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x898f1a2675ac89cfUL)]
        public class Result_ImportContainer : ICapnpSerializable
        {
            public const UInt64 typeId = 0x898f1a2675ac89cfUL;
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
    }
}