using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Schema.Common
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x97e6feac0322118dUL)]
    public class Date : ICapnpSerializable
    {
        public const UInt64 typeId = 0x97e6feac0322118dUL;
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
}