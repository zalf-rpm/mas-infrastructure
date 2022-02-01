using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Rpc.Fbp
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9f6bf783c59ae53fUL), Proxy(typeof(Input_Proxy)), Skeleton(typeof(Input_Skeleton))]
    public interface IInput : IDisposable
    {
        Task Input(string data, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9f6bf783c59ae53fUL)]
    public class Input_Proxy : Proxy, IInput
    {
        public async Task Input(string data, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Fbp.Input.Params_Input.WRITER>();
            var arg_ = new Mas.Rpc.Fbp.Input.Params_Input()
            {Data = data};
            arg_?.serialize(in_);
            using (var d_ = await Call(11487547419866621247UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.Fbp.Input.Result_Input>(d_);
                return;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9f6bf783c59ae53fUL)]
    public class Input_Skeleton : Skeleton<IInput>
    {
        public Input_Skeleton()
        {
            SetMethodTable(Input);
        }

        public override ulong InterfaceId => 11487547419866621247UL;
        async Task<AnswerOrCounterquestion> Input(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.Fbp.Input.Params_Input>(d_);
                await Impl.Input(in_.Data, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Rpc.Fbp.Input.Result_Input.WRITER>();
                return s_;
            }
        }
    }

    public static class Input
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdf66370e686c9b09UL)]
        public class Params_Input : ICapnpSerializable
        {
            public const UInt64 typeId = 0xdf66370e686c9b09UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Data = reader.Data;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Data = Data;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Data
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
                public string Data => ctx.ReadText(0, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string Data
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9d2556ab64354282UL)]
        public class Result_Input : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9d2556ab64354282UL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf92ba4cc25099ed7UL), Proxy(typeof(Output_Proxy)), Skeleton(typeof(Output_Skeleton))]
    public interface IOutput : IDisposable
    {
        Task Output(string data, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf92ba4cc25099ed7UL)]
    public class Output_Proxy : Proxy, IOutput
    {
        public async Task Output(string data, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Fbp.Output.Params_Output.WRITER>();
            var arg_ = new Mas.Rpc.Fbp.Output.Params_Output()
            {Data = data};
            arg_?.serialize(in_);
            using (var d_ = await Call(17954625536144285399UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.Fbp.Output.Result_Output>(d_);
                return;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf92ba4cc25099ed7UL)]
    public class Output_Skeleton : Skeleton<IOutput>
    {
        public Output_Skeleton()
        {
            SetMethodTable(Output);
        }

        public override ulong InterfaceId => 17954625536144285399UL;
        async Task<AnswerOrCounterquestion> Output(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.Fbp.Output.Params_Output>(d_);
                await Impl.Output(in_.Data, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<Mas.Rpc.Fbp.Output.Result_Output.WRITER>();
                return s_;
            }
        }
    }

    public static class Output
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc041f7b2c1f05e54UL)]
        public class Params_Output : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc041f7b2c1f05e54UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Data = reader.Data;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Data = Data;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Data
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
                public string Data => ctx.ReadText(0, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string Data
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb1ffe14d66f53639UL)]
        public class Result_Output : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb1ffe14d66f53639UL;
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