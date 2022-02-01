using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Rpc.Jobs
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa05b60b71ca38848UL)]
    public class Job : ICapnpSerializable
    {
        public const UInt64 typeId = 0xa05b60b71ca38848UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            LatLngCoords = reader.LatLngCoords?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Rpc.Geo.LatLonCoord>(_));
            NoFurtherJobs = reader.NoFurtherJobs;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.LatLngCoords.Init(LatLngCoords, (_s1, _v1) => _v1?.serialize(_s1));
            writer.NoFurtherJobs = NoFurtherJobs;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public IReadOnlyList<Mas.Rpc.Geo.LatLonCoord> LatLngCoords
        {
            get;
            set;
        }

        public bool NoFurtherJobs
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
            public IReadOnlyList<Mas.Rpc.Geo.LatLonCoord.READER> LatLngCoords => ctx.ReadList(0).Cast(Mas.Rpc.Geo.LatLonCoord.READER.create);
            public bool HasLatLngCoords => ctx.IsStructFieldNonNull(0);
            public bool NoFurtherJobs => ctx.ReadDataBool(0UL, false);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 1);
            }

            public ListOfStructsSerializer<Mas.Rpc.Geo.LatLonCoord.WRITER> LatLngCoords
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Rpc.Geo.LatLonCoord.WRITER>>(0);
                set => Link(0, value);
            }

            public bool NoFurtherJobs
            {
                get => this.ReadDataBool(0UL, false);
                set => this.WriteData(0UL, value, false);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb8745454d013cbf0UL), Proxy(typeof(Service_Proxy)), Skeleton(typeof(Service_Skeleton))]
    public interface IService : IDisposable
    {
        Task<Mas.Rpc.Jobs.Job> NextJob(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb8745454d013cbf0UL)]
    public class Service_Proxy : Proxy, IService
    {
        public async Task<Mas.Rpc.Jobs.Job> NextJob(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Jobs.Service.Params_NextJob.WRITER>();
            var arg_ = new Mas.Rpc.Jobs.Service.Params_NextJob()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(13291341123522120688UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.Jobs.Service.Result_NextJob>(d_);
                return (r_.Job);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb8745454d013cbf0UL)]
    public class Service_Skeleton : Skeleton<IService>
    {
        public Service_Skeleton()
        {
            SetMethodTable(NextJob);
        }

        public override ulong InterfaceId => 13291341123522120688UL;
        Task<AnswerOrCounterquestion> NextJob(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.NextJob(cancellationToken_), job =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Jobs.Service.Result_NextJob.WRITER>();
                    var r_ = new Mas.Rpc.Jobs.Service.Result_NextJob{Job = job};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class Service
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xea3ba97e764a031cUL)]
        public class Params_NextJob : ICapnpSerializable
        {
            public const UInt64 typeId = 0xea3ba97e764a031cUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe067ec22521ebebbUL)]
        public class Result_NextJob : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe067ec22521ebebbUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Job = CapnpSerializable.Create<Mas.Rpc.Jobs.Job>(reader.Job);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Job?.serialize(writer.Job);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Rpc.Jobs.Job Job
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
                public Mas.Rpc.Jobs.Job.READER Job => ctx.ReadStruct(0, Mas.Rpc.Jobs.Job.READER.create);
                public bool HasJob => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Rpc.Jobs.Job.WRITER Job
                {
                    get => BuildPointer<Mas.Rpc.Jobs.Job.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }
    }
}