using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Schema.Persistence
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe10a5d74d58bd18dUL)]
    public class VatId : ICapnpSerializable
    {
        public const UInt64 typeId = 0xe10a5d74d58bd18dUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            PublicKey0 = reader.PublicKey0;
            PublicKey1 = reader.PublicKey1;
            PublicKey2 = reader.PublicKey2;
            PublicKey3 = reader.PublicKey3;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.PublicKey0 = PublicKey0;
            writer.PublicKey1 = PublicKey1;
            writer.PublicKey2 = PublicKey2;
            writer.PublicKey3 = PublicKey3;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public ulong PublicKey0
        {
            get;
            set;
        }

        public ulong PublicKey1
        {
            get;
            set;
        }

        public ulong PublicKey2
        {
            get;
            set;
        }

        public ulong PublicKey3
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
            public ulong PublicKey0 => ctx.ReadDataULong(0UL, 0UL);
            public ulong PublicKey1 => ctx.ReadDataULong(64UL, 0UL);
            public ulong PublicKey2 => ctx.ReadDataULong(128UL, 0UL);
            public ulong PublicKey3 => ctx.ReadDataULong(192UL, 0UL);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(4, 0);
            }

            public ulong PublicKey0
            {
                get => this.ReadDataULong(0UL, 0UL);
                set => this.WriteData(0UL, value, 0UL);
            }

            public ulong PublicKey1
            {
                get => this.ReadDataULong(64UL, 0UL);
                set => this.WriteData(64UL, value, 0UL);
            }

            public ulong PublicKey2
            {
                get => this.ReadDataULong(128UL, 0UL);
                set => this.WriteData(128UL, value, 0UL);
            }

            public ulong PublicKey3
            {
                get => this.ReadDataULong(192UL, 0UL);
                set => this.WriteData(192UL, value, 0UL);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfb47810671a05b0dUL)]
    public class Address : ICapnpSerializable
    {
        public const UInt64 typeId = 0xfb47810671a05b0dUL;
        public enum WHICH : ushort
        {
            Ip6 = 0,
            Host = 1,
            undefined = 65535
        }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            switch (reader.which)
            {
                case WHICH.Ip6:
                    Ip6 = CapnpSerializable.Create<Mas.Schema.Persistence.Address.ip6>(reader.Ip6);
                    break;
                case WHICH.Host:
                    Host = reader.Host;
                    break;
            }

            Port = reader.Port;
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
                    case WHICH.Ip6:
                        _content = null;
                        break;
                    case WHICH.Host:
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
                case WHICH.Ip6:
                    Ip6?.serialize(writer.Ip6);
                    break;
                case WHICH.Host:
                    writer.Host = Host;
                    break;
            }

            writer.Port = Port;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public Mas.Schema.Persistence.Address.ip6 Ip6
        {
            get => _which == WHICH.Ip6 ? (Mas.Schema.Persistence.Address.ip6)_content : null;
            set
            {
                _which = WHICH.Ip6;
                _content = value;
            }
        }

        public ushort Port
        {
            get;
            set;
        }

        public string Host
        {
            get => _which == WHICH.Host ? (string)_content : null;
            set
            {
                _which = WHICH.Host;
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
            public WHICH which => (WHICH)ctx.ReadDataUShort(144U, (ushort)0);
            public ip6.READER Ip6 => which == WHICH.Ip6 ? new ip6.READER(ctx) : default;
            public ushort Port => ctx.ReadDataUShort(128UL, (ushort)0);
            public string Host => which == WHICH.Host ? ctx.ReadText(0, null) : default;
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(3, 1);
            }

            public WHICH which
            {
                get => (WHICH)this.ReadDataUShort(144U, (ushort)0);
                set => this.WriteData(144U, (ushort)value, (ushort)0);
            }

            public ip6.WRITER Ip6
            {
                get => which == WHICH.Ip6 ? Rewrap<ip6.WRITER>() : default;
            }

            public ushort Port
            {
                get => this.ReadDataUShort(128UL, (ushort)0);
                set => this.WriteData(128UL, value, (ushort)0);
            }

            public string Host
            {
                get => which == WHICH.Host ? this.ReadText(0, null) : default;
                set => this.WriteText(0, value, null);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8fb25d0428898a69UL)]
        public class ip6 : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8fb25d0428898a69UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Lower64 = reader.Lower64;
                Upper64 = reader.Upper64;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Lower64 = Lower64;
                writer.Upper64 = Upper64;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public ulong Lower64
            {
                get;
                set;
            }

            public ulong Upper64
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
                public ulong Lower64 => ctx.ReadDataULong(0UL, 0UL);
                public ulong Upper64 => ctx.ReadDataULong(64UL, 0UL);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                }

                public ulong Lower64
                {
                    get => this.ReadDataULong(0UL, 0UL);
                    set => this.WriteData(0UL, value, 0UL);
                }

                public ulong Upper64
                {
                    get => this.ReadDataULong(64UL, 0UL);
                    set => this.WriteData(64UL, value, 0UL);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd9eccdf2dbc48087UL)]
    public class VatPath : ICapnpSerializable
    {
        public const UInt64 typeId = 0xd9eccdf2dbc48087UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Id = CapnpSerializable.Create<Mas.Schema.Persistence.VatId>(reader.Id);
            Address = CapnpSerializable.Create<Mas.Schema.Persistence.Address>(reader.Address);
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            Id?.serialize(writer.Id);
            Address?.serialize(writer.Address);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public Mas.Schema.Persistence.VatId Id
        {
            get;
            set;
        }

        public Mas.Schema.Persistence.Address Address
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
            public Mas.Schema.Persistence.VatId.READER Id => ctx.ReadStruct(0, Mas.Schema.Persistence.VatId.READER.create);
            public bool HasId => ctx.IsStructFieldNonNull(0);
            public Mas.Schema.Persistence.Address.READER Address => ctx.ReadStruct(1, Mas.Schema.Persistence.Address.READER.create);
            public bool HasAddress => ctx.IsStructFieldNonNull(1);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 2);
            }

            public Mas.Schema.Persistence.VatId.WRITER Id
            {
                get => BuildPointer<Mas.Schema.Persistence.VatId.WRITER>(0);
                set => Link(0, value);
            }

            public Mas.Schema.Persistence.Address.WRITER Address
            {
                get => BuildPointer<Mas.Schema.Persistence.Address.WRITER>(1);
                set => Link(1, value);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x886d68271d83de4dUL)]
    public class SturdyRef : ICapnpSerializable
    {
        public const UInt64 typeId = 0x886d68271d83de4dUL;
        public enum WHICH : ushort
        {
            TheTransient = 0,
            TheStored = 1,
            undefined = 65535
        }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            switch (reader.which)
            {
                case WHICH.TheTransient:
                    TheTransient = CapnpSerializable.Create<Mas.Schema.Persistence.SturdyRef.Transient>(reader.TheTransient);
                    break;
                case WHICH.TheStored:
                    TheStored = CapnpSerializable.Create<Mas.Schema.Persistence.SturdyRef.Stored>(reader.TheStored);
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
                    case WHICH.TheTransient:
                        _content = null;
                        break;
                    case WHICH.TheStored:
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
                case WHICH.TheTransient:
                    TheTransient?.serialize(writer.TheTransient);
                    break;
                case WHICH.TheStored:
                    TheStored?.serialize(writer.TheStored);
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

        public Mas.Schema.Persistence.SturdyRef.Transient TheTransient
        {
            get => _which == WHICH.TheTransient ? (Mas.Schema.Persistence.SturdyRef.Transient)_content : null;
            set
            {
                _which = WHICH.TheTransient;
                _content = value;
            }
        }

        public Mas.Schema.Persistence.SturdyRef.Stored TheStored
        {
            get => _which == WHICH.TheStored ? (Mas.Schema.Persistence.SturdyRef.Stored)_content : null;
            set
            {
                _which = WHICH.TheStored;
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
            public WHICH which => (WHICH)ctx.ReadDataUShort(0U, (ushort)0);
            public Mas.Schema.Persistence.SturdyRef.Transient.READER TheTransient => which == WHICH.TheTransient ? ctx.ReadStruct(0, Mas.Schema.Persistence.SturdyRef.Transient.READER.create) : default;
            public bool HasTheTransient => ctx.IsStructFieldNonNull(0);
            public Mas.Schema.Persistence.SturdyRef.Stored.READER TheStored => which == WHICH.TheStored ? ctx.ReadStruct(0, Mas.Schema.Persistence.SturdyRef.Stored.READER.create) : default;
            public bool HasTheStored => ctx.IsStructFieldNonNull(0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 1);
            }

            public WHICH which
            {
                get => (WHICH)this.ReadDataUShort(0U, (ushort)0);
                set => this.WriteData(0U, (ushort)value, (ushort)0);
            }

            public Mas.Schema.Persistence.SturdyRef.Transient.WRITER TheTransient
            {
                get => which == WHICH.TheTransient ? BuildPointer<Mas.Schema.Persistence.SturdyRef.Transient.WRITER>(0) : default;
                set => Link(0, value);
            }

            public Mas.Schema.Persistence.SturdyRef.Stored.WRITER TheStored
            {
                get => which == WHICH.TheStored ? BuildPointer<Mas.Schema.Persistence.SturdyRef.Stored.WRITER>(0) : default;
                set => Link(0, value);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfdd799ed60c87723UL)]
        public class Owner : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfdd799ed60c87723UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Guid = reader.Guid;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Guid = Guid;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Guid
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
                public string Guid => ctx.ReadText(0, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string Guid
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa42bd461f2a8a3c8UL)]
        public class Transient : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa42bd461f2a8a3c8UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Vat = CapnpSerializable.Create<Mas.Schema.Persistence.VatPath>(reader.Vat);
                LocalRef = CapnpSerializable.Create<object>(reader.LocalRef);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Vat?.serialize(writer.Vat);
                writer.LocalRef.SetObject(LocalRef);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Persistence.VatPath Vat
            {
                get;
                set;
            }

            public object LocalRef
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
                public Mas.Schema.Persistence.VatPath.READER Vat => ctx.ReadStruct(0, Mas.Schema.Persistence.VatPath.READER.create);
                public bool HasVat => ctx.IsStructFieldNonNull(0);
                public DeserializerState LocalRef => ctx.StructReadPointer(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public Mas.Schema.Persistence.VatPath.WRITER Vat
                {
                    get => BuildPointer<Mas.Schema.Persistence.VatPath.WRITER>(0);
                    set => Link(0, value);
                }

                public DynamicSerializerState LocalRef
                {
                    get => BuildPointer<DynamicSerializerState>(1);
                    set => Link(1, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcbe679a401315eb8UL)]
        public class Stored : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcbe679a401315eb8UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Key0 = reader.Key0;
                Key1 = reader.Key1;
                Key2 = reader.Key2;
                Key3 = reader.Key3;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Key0 = Key0;
                writer.Key1 = Key1;
                writer.Key2 = Key2;
                writer.Key3 = Key3;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public ulong Key0
            {
                get;
                set;
            }

            public ulong Key1
            {
                get;
                set;
            }

            public ulong Key2
            {
                get;
                set;
            }

            public ulong Key3
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
                public ulong Key0 => ctx.ReadDataULong(0UL, 0UL);
                public ulong Key1 => ctx.ReadDataULong(64UL, 0UL);
                public ulong Key2 => ctx.ReadDataULong(128UL, 0UL);
                public ulong Key3 => ctx.ReadDataULong(192UL, 0UL);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(4, 0);
                }

                public ulong Key0
                {
                    get => this.ReadDataULong(0UL, 0UL);
                    set => this.WriteData(0UL, value, 0UL);
                }

                public ulong Key1
                {
                    get => this.ReadDataULong(64UL, 0UL);
                    set => this.WriteData(64UL, value, 0UL);
                }

                public ulong Key2
                {
                    get => this.ReadDataULong(128UL, 0UL);
                    set => this.WriteData(128UL, value, 0UL);
                }

                public ulong Key3
                {
                    get => this.ReadDataULong(192UL, 0UL);
                    set => this.WriteData(192UL, value, 0UL);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc1a7daa0dc36cb65UL), Proxy(typeof(Persistent_Proxy)), Skeleton(typeof(Persistent_Skeleton))]
    public interface IPersistent : IDisposable
    {
        Task<(string, string)> Save(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc1a7daa0dc36cb65UL)]
    public class Persistent_Proxy : Proxy, IPersistent
    {
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
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc1a7daa0dc36cb65UL)]
    public class Persistent_Skeleton : Skeleton<IPersistent>
    {
        public Persistent_Skeleton()
        {
            SetMethodTable(Save);
        }

        public override ulong InterfaceId => 13954362354854972261UL;
        Task<AnswerOrCounterquestion> Save(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Save(cancellationToken_), (sturdyRef, unsaveSR) =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.Persistent.Result_Save.WRITER>();
                    var r_ = new Mas.Schema.Persistence.Persistent.Result_Save{SturdyRef = sturdyRef, UnsaveSR = unsaveSR};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class Persistent
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xffc59cb2d8a71502UL)]
        public class Params_Save : ICapnpSerializable
        {
            public const UInt64 typeId = 0xffc59cb2d8a71502UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xef80b08e84155cc2UL)]
        public class Result_Save : ICapnpSerializable
        {
            public const UInt64 typeId = 0xef80b08e84155cc2UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                SturdyRef = reader.SturdyRef;
                UnsaveSR = reader.UnsaveSR;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.SturdyRef = SturdyRef;
                writer.UnsaveSR = UnsaveSR;
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

            public string UnsaveSR
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
                public string UnsaveSR => ctx.ReadText(1, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public string SturdyRef
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public string UnsaveSR
                {
                    get => this.ReadText(1, null);
                    set => this.WriteText(1, value, null);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9fb6218427d92e3cUL), Proxy(typeof(Restorer_Proxy)), Skeleton(typeof(Restorer_Skeleton))]
    public interface IRestorer : IDisposable
    {
        Task<BareProxy> Restore(string srToken, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9fb6218427d92e3cUL)]
    public class Restorer_Proxy : Proxy, IRestorer
    {
        public Task<BareProxy> Restore(string srToken, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.Restorer.Params_Restore.WRITER>();
            var arg_ = new Mas.Schema.Persistence.Restorer.Params_Restore()
            {SrToken = srToken};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(11508422749279825468UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Persistence.Restorer.Result_Restore>(d_);
                    return (r_.Cap);
                }
            }

            );
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9fb6218427d92e3cUL)]
    public class Restorer_Skeleton : Skeleton<IRestorer>
    {
        public Restorer_Skeleton()
        {
            SetMethodTable(Restore);
        }

        public override ulong InterfaceId => 11508422749279825468UL;
        Task<AnswerOrCounterquestion> Restore(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Persistence.Restorer.Params_Restore>(d_);
                return Impatient.MaybeTailCall(Impl.Restore(in_.SrToken, cancellationToken_), cap =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.Restorer.Result_Restore.WRITER>();
                    var r_ = new Mas.Schema.Persistence.Restorer.Result_Restore{Cap = cap};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class Restorer
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8071b2eb61aac3f0UL)]
        public class Params_Restore : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8071b2eb61aac3f0UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                SrToken = reader.SrToken;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.SrToken = SrToken;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string SrToken
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
                public string SrToken => ctx.ReadText(0, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string SrToken
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xda966d1d252e4d25UL)]
        public class Result_Restore : ICapnpSerializable
        {
            public const UInt64 typeId = 0xda966d1d252e4d25UL;
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

            public BareProxy Cap
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
                public BareProxy Cap => ctx.ReadCap(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public BareProxy Cap
                {
                    get => ReadCap<BareProxy>(0);
                    set => LinkObject(0, value);
                }
            }
        }
    }
}