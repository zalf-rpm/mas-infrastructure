using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Schema.Jobs
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa05b60b71ca38848UL)]
    public class Job<TPayload> : ICapnpSerializable where TPayload : class
    {
        public const UInt64 typeId = 0xa05b60b71ca38848UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Data = CapnpSerializable.Create<TPayload>(reader.Data);
            NoFurtherJobs = reader.NoFurtherJobs;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Data.SetObject(Data);
            writer.NoFurtherJobs = NoFurtherJobs;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public TPayload Data
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
            public DeserializerState Data => ctx.StructReadPointer(0);
            public bool NoFurtherJobs => ctx.ReadDataBool(0UL, false);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 1);
            }

            public DynamicSerializerState Data
            {
                get => BuildPointer<DynamicSerializerState>(0);
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
    public interface IService : Mas.Schema.Common.IIdentifiable, Mas.Schema.Persistence.IPersistent
    {
        Task<Mas.Schema.Jobs.Job<object>> NextJob(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb8745454d013cbf0UL)]
    public class Service_Proxy : Proxy, IService
    {
        public Task<Mas.Schema.Jobs.Job<object>> NextJob(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Jobs.Service.Params_NextJob.WRITER>();
            var arg_ = new Mas.Schema.Jobs.Service.Params_NextJob()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(13291341123522120688UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Jobs.Service.Result_NextJob>(d_);
                    return (r_.Job);
                }
            }

            );
        }

        public async Task<(string, string)> Save(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.Persistent.Params_Save.WRITER>();
            var arg_ = new Mas.Schema.Persistence.Persistent.Params_Save()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(13954362354854972261UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Persistence.Persistent.Result_Save>(d_);
                return (r_.SturdyRef, r_.UnsaveSR);
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
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Jobs.Service.Result_NextJob.WRITER>();
                    var r_ = new Mas.Schema.Jobs.Service.Result_NextJob{Job = job};
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
                Job = CapnpSerializable.Create<Mas.Schema.Jobs.Job<object>>(reader.Job);
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

            public Mas.Schema.Jobs.Job<object> Job
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
                public Mas.Schema.Jobs.Job<object>.READER Job => ctx.ReadStruct(0, Mas.Schema.Jobs.Job<object>.READER.create);
                public bool HasJob => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Jobs.Job<object>.WRITER Job
                {
                    get => BuildPointer<Mas.Schema.Jobs.Job<object>.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }
    }
}