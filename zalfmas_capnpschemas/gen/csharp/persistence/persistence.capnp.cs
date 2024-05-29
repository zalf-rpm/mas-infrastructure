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
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Vat = CapnpSerializable.Create<Mas.Schema.Persistence.VatPath>(reader.Vat);
            LocalRef = CapnpSerializable.Create<Mas.Schema.Persistence.SturdyRef.Token>(reader.LocalRef);
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            Vat?.serialize(writer.Vat);
            LocalRef?.serialize(writer.LocalRef);
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

        public Mas.Schema.Persistence.SturdyRef.Token LocalRef
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
            public Mas.Schema.Persistence.SturdyRef.Token.READER LocalRef => ctx.ReadStruct(1, Mas.Schema.Persistence.SturdyRef.Token.READER.create);
            public bool HasLocalRef => ctx.IsStructFieldNonNull(1);
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

            public Mas.Schema.Persistence.SturdyRef.Token.WRITER LocalRef
            {
                get => BuildPointer<Mas.Schema.Persistence.SturdyRef.Token.WRITER>(1);
                set => Link(1, value);
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfa412bb47f11b488UL)]
        public class Token : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfa412bb47f11b488UL;
            public enum WHICH : ushort
            {
                Text = 0,
                Data = 1,
                undefined = 65535
            }

            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                switch (reader.which)
                {
                    case WHICH.Text:
                        Text = reader.Text;
                        break;
                    case WHICH.Data:
                        Data = reader.Data;
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
                        case WHICH.Text:
                            _content = null;
                            break;
                        case WHICH.Data:
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
                    case WHICH.Text:
                        writer.Text = Text;
                        break;
                    case WHICH.Data:
                        writer.Data.Init(Data);
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

            public string Text
            {
                get => _which == WHICH.Text ? (string)_content : null;
                set
                {
                    _which = WHICH.Text;
                    _content = value;
                }
            }

            public IReadOnlyList<byte> Data
            {
                get => _which == WHICH.Data ? (IReadOnlyList<byte>)_content : null;
                set
                {
                    _which = WHICH.Data;
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
                public string Text => which == WHICH.Text ? ctx.ReadText(0, null) : default;
                public IReadOnlyList<byte> Data => which == WHICH.Data ? ctx.ReadList(0).CastByte() : default;
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

                public string Text
                {
                    get => which == WHICH.Text ? this.ReadText(0, null) : default;
                    set => this.WriteText(0, value, null);
                }

                public ListOfPrimitivesSerializer<byte> Data
                {
                    get => which == WHICH.Data ? BuildPointer<ListOfPrimitivesSerializer<byte>>(0) : default;
                    set => Link(0, value);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc1a7daa0dc36cb65UL), Proxy(typeof(Persistent_Proxy)), Skeleton(typeof(Persistent_Skeleton))]
    public interface IPersistent : IDisposable
    {
        Task<Mas.Schema.Persistence.Persistent.SaveResults> Save(Mas.Schema.Persistence.Persistent.SaveParams arg_, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc1a7daa0dc36cb65UL)]
    public class Persistent_Proxy : Proxy, IPersistent
    {
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
                return Impatient.MaybeTailCall(Impl.Save(CapnpSerializable.Create<Mas.Schema.Persistence.Persistent.SaveParams>(d_), cancellationToken_), r_ =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.Persistent.SaveResults.WRITER>();
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class Persistent
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd5e0aac4225e0343UL)]
        public class SaveParams : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd5e0aac4225e0343UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                SealFor = CapnpSerializable.Create<Mas.Schema.Persistence.SturdyRef.Owner>(reader.SealFor);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                SealFor?.serialize(writer.SealFor);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Persistence.SturdyRef.Owner SealFor
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
                public Mas.Schema.Persistence.SturdyRef.Owner.READER SealFor => ctx.ReadStruct(0, Mas.Schema.Persistence.SturdyRef.Owner.READER.create);
                public bool HasSealFor => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Persistence.SturdyRef.Owner.WRITER SealFor
                {
                    get => BuildPointer<Mas.Schema.Persistence.SturdyRef.Owner.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdc5bd1ef982cec13UL)]
        public class SaveResults : ICapnpSerializable
        {
            public const UInt64 typeId = 0xdc5bd1ef982cec13UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                SturdyRef = CapnpSerializable.Create<Mas.Schema.Persistence.SturdyRef>(reader.SturdyRef);
                UnsaveSR = CapnpSerializable.Create<Mas.Schema.Persistence.SturdyRef>(reader.UnsaveSR);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                SturdyRef?.serialize(writer.SturdyRef);
                UnsaveSR?.serialize(writer.UnsaveSR);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Persistence.SturdyRef SturdyRef
            {
                get;
                set;
            }

            public Mas.Schema.Persistence.SturdyRef UnsaveSR
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
                public Mas.Schema.Persistence.SturdyRef.READER SturdyRef => ctx.ReadStruct(0, Mas.Schema.Persistence.SturdyRef.READER.create);
                public bool HasSturdyRef => ctx.IsStructFieldNonNull(0);
                public Mas.Schema.Persistence.SturdyRef.READER UnsaveSR => ctx.ReadStruct(1, Mas.Schema.Persistence.SturdyRef.READER.create);
                public bool HasUnsaveSR => ctx.IsStructFieldNonNull(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public Mas.Schema.Persistence.SturdyRef.WRITER SturdyRef
                {
                    get => BuildPointer<Mas.Schema.Persistence.SturdyRef.WRITER>(0);
                    set => Link(0, value);
                }

                public Mas.Schema.Persistence.SturdyRef.WRITER UnsaveSR
                {
                    get => BuildPointer<Mas.Schema.Persistence.SturdyRef.WRITER>(1);
                    set => Link(1, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8f700f81169f2e52UL), Proxy(typeof(ReleaseSturdyRef_Proxy)), Skeleton(typeof(ReleaseSturdyRef_Skeleton))]
        public interface IReleaseSturdyRef : IDisposable
        {
            Task<bool> Release(CancellationToken cancellationToken_ = default);
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8f700f81169f2e52UL)]
        public class ReleaseSturdyRef_Proxy : Proxy, IReleaseSturdyRef
        {
            public async Task<bool> Release(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.Persistent.ReleaseSturdyRef.Params_Release.WRITER>();
                var arg_ = new Mas.Schema.Persistence.Persistent.ReleaseSturdyRef.Params_Release()
                {};
                arg_?.serialize(in_);
                using (var d_ = await Call(10335778191920016978UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Persistence.Persistent.ReleaseSturdyRef.Result_Release>(d_);
                    return (r_.Success);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8f700f81169f2e52UL)]
        public class ReleaseSturdyRef_Skeleton : Skeleton<IReleaseSturdyRef>
        {
            public ReleaseSturdyRef_Skeleton()
            {
                SetMethodTable(Release);
            }

            public override ulong InterfaceId => 10335778191920016978UL;
            Task<AnswerOrCounterquestion> Release(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    return Impatient.MaybeTailCall(Impl.Release(cancellationToken_), success =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.Persistent.ReleaseSturdyRef.Result_Release.WRITER>();
                        var r_ = new Mas.Schema.Persistence.Persistent.ReleaseSturdyRef.Result_Release{Success = success};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }
        }

        public static class ReleaseSturdyRef
        {
            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa4db8c20d9807c15UL)]
            public class Params_Release : ICapnpSerializable
            {
                public const UInt64 typeId = 0xa4db8c20d9807c15UL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x932d6ee32410e853UL)]
            public class Result_Release : ICapnpSerializable
            {
                public const UInt64 typeId = 0x932d6ee32410e853UL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9fb6218427d92e3cUL), Proxy(typeof(Restorer_Proxy)), Skeleton(typeof(Restorer_Skeleton))]
    public interface IRestorer : IDisposable
    {
        Task<BareProxy> Restore(Mas.Schema.Persistence.Restorer.RestoreParams arg_, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9fb6218427d92e3cUL)]
    public class Restorer_Proxy : Proxy, IRestorer
    {
        public Task<BareProxy> Restore(Mas.Schema.Persistence.Restorer.RestoreParams arg_, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.Restorer.RestoreParams.WRITER>();
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
                return Impatient.MaybeTailCall(Impl.Restore(CapnpSerializable.Create<Mas.Schema.Persistence.Restorer.RestoreParams>(d_), cancellationToken_), cap =>
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
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc541e5764a37d73aUL)]
        public class RestoreParams : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc541e5764a37d73aUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                LocalRef = CapnpSerializable.Create<Mas.Schema.Persistence.SturdyRef.Token>(reader.LocalRef);
                SealedBy = CapnpSerializable.Create<Mas.Schema.Persistence.SturdyRef.Owner>(reader.SealedBy);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                LocalRef?.serialize(writer.LocalRef);
                SealedBy?.serialize(writer.SealedBy);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Persistence.SturdyRef.Token LocalRef
            {
                get;
                set;
            }

            public Mas.Schema.Persistence.SturdyRef.Owner SealedBy
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
                public Mas.Schema.Persistence.SturdyRef.Token.READER LocalRef => ctx.ReadStruct(0, Mas.Schema.Persistence.SturdyRef.Token.READER.create);
                public bool HasLocalRef => ctx.IsStructFieldNonNull(0);
                public Mas.Schema.Persistence.SturdyRef.Owner.READER SealedBy => ctx.ReadStruct(1, Mas.Schema.Persistence.SturdyRef.Owner.READER.create);
                public bool HasSealedBy => ctx.IsStructFieldNonNull(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public Mas.Schema.Persistence.SturdyRef.Token.WRITER LocalRef
                {
                    get => BuildPointer<Mas.Schema.Persistence.SturdyRef.Token.WRITER>(0);
                    set => Link(0, value);
                }

                public Mas.Schema.Persistence.SturdyRef.Owner.WRITER SealedBy
                {
                    get => BuildPointer<Mas.Schema.Persistence.SturdyRef.Owner.WRITER>(1);
                    set => Link(1, value);
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaa8d91fab6d01d9fUL), Proxy(typeof(HostPortResolver_Proxy)), Skeleton(typeof(HostPortResolver_Skeleton))]
    public interface IHostPortResolver : Mas.Schema.Common.IIdentifiable, Mas.Schema.Persistence.IRestorer
    {
        Task<(string, ushort)> Resolve(string id, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaa8d91fab6d01d9fUL)]
    public class HostPortResolver_Proxy : Proxy, IHostPortResolver
    {
        public async Task<(string, ushort)> Resolve(string id, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.HostPortResolver.Params_Resolve.WRITER>();
            var arg_ = new Mas.Schema.Persistence.HostPortResolver.Params_Resolve()
            {Id = id};
            arg_?.serialize(in_);
            using (var d_ = await Call(12289639464158895519UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Persistence.HostPortResolver.Result_Resolve>(d_);
                return (r_.Host, r_.Port);
            }
        }

        public Task<BareProxy> Restore(Mas.Schema.Persistence.Restorer.RestoreParams arg_, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.Restorer.RestoreParams.WRITER>();
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaa8d91fab6d01d9fUL)]
    public class HostPortResolver_Skeleton : Skeleton<IHostPortResolver>
    {
        public HostPortResolver_Skeleton()
        {
            SetMethodTable(Resolve);
        }

        public override ulong InterfaceId => 12289639464158895519UL;
        Task<AnswerOrCounterquestion> Resolve(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Persistence.HostPortResolver.Params_Resolve>(d_);
                return Impatient.MaybeTailCall(Impl.Resolve(in_.Id, cancellationToken_), (host, port) =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.HostPortResolver.Result_Resolve.WRITER>();
                    var r_ = new Mas.Schema.Persistence.HostPortResolver.Result_Resolve{Host = host, Port = port};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class HostPortResolver
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb0caf775704690b2UL), Proxy(typeof(Registrar_Proxy)), Skeleton(typeof(Registrar_Skeleton))]
        public interface IRegistrar : IDisposable
        {
            Task<(Mas.Schema.Persistence.HostPortResolver.Registrar.IHeartbeat, uint)> Register(Mas.Schema.Persistence.HostPortResolver.Registrar.RegisterParams arg_, CancellationToken cancellationToken_ = default);
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb0caf775704690b2UL)]
        public class Registrar_Proxy : Proxy, IRegistrar
        {
            public Task<(Mas.Schema.Persistence.HostPortResolver.Registrar.IHeartbeat, uint)> Register(Mas.Schema.Persistence.HostPortResolver.Registrar.RegisterParams arg_, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.HostPortResolver.Registrar.RegisterParams.WRITER>();
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(12739266579737776306UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Persistence.HostPortResolver.Registrar.Result_Register>(d_);
                        return (r_.Heartbeat, r_.SecsHeartbeatInterval);
                    }
                }

                );
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb0caf775704690b2UL)]
        public class Registrar_Skeleton : Skeleton<IRegistrar>
        {
            public Registrar_Skeleton()
            {
                SetMethodTable(Register);
            }

            public override ulong InterfaceId => 12739266579737776306UL;
            Task<AnswerOrCounterquestion> Register(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    return Impatient.MaybeTailCall(Impl.Register(CapnpSerializable.Create<Mas.Schema.Persistence.HostPortResolver.Registrar.RegisterParams>(d_), cancellationToken_), (heartbeat, secsHeartbeatInterval) =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.HostPortResolver.Registrar.Result_Register.WRITER>();
                        var r_ = new Mas.Schema.Persistence.HostPortResolver.Registrar.Result_Register{Heartbeat = heartbeat, SecsHeartbeatInterval = secsHeartbeatInterval};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }
        }

        public static class Registrar
        {
            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x87de92d2d68df26fUL), Proxy(typeof(Heartbeat_Proxy)), Skeleton(typeof(Heartbeat_Skeleton))]
            public interface IHeartbeat : IDisposable
            {
                Task Beat(CancellationToken cancellationToken_ = default);
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x87de92d2d68df26fUL)]
            public class Heartbeat_Proxy : Proxy, IHeartbeat
            {
                public async Task Beat(CancellationToken cancellationToken_ = default)
                {
                    var in_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.HostPortResolver.Registrar.Heartbeat.Params_Beat.WRITER>();
                    var arg_ = new Mas.Schema.Persistence.HostPortResolver.Registrar.Heartbeat.Params_Beat()
                    {};
                    arg_?.serialize(in_);
                    using (var d_ = await Call(9790424074190451311UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                    {
                        var r_ = CapnpSerializable.Create<Mas.Schema.Persistence.HostPortResolver.Registrar.Heartbeat.Result_Beat>(d_);
                        return;
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x87de92d2d68df26fUL)]
            public class Heartbeat_Skeleton : Skeleton<IHeartbeat>
            {
                public Heartbeat_Skeleton()
                {
                    SetMethodTable(Beat);
                }

                public override ulong InterfaceId => 9790424074190451311UL;
                async Task<AnswerOrCounterquestion> Beat(DeserializerState d_, CancellationToken cancellationToken_)
                {
                    using (d_)
                    {
                        await Impl.Beat(cancellationToken_);
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.HostPortResolver.Registrar.Heartbeat.Result_Beat.WRITER>();
                        return s_;
                    }
                }
            }

            public static class Heartbeat
            {
                [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xebff70497e0ba555UL)]
                public class Params_Beat : ICapnpSerializable
                {
                    public const UInt64 typeId = 0xebff70497e0ba555UL;
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

                [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9510c22fa544a65eUL)]
                public class Result_Beat : ICapnpSerializable
                {
                    public const UInt64 typeId = 0x9510c22fa544a65eUL;
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

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbf018f62ff460d0fUL)]
            public class RegisterParams : ICapnpSerializable
            {
                public const UInt64 typeId = 0xbf018f62ff460d0fUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Base64VatId = reader.Base64VatId;
                    Host = reader.Host;
                    Port = reader.Port;
                    Alias = reader.Alias;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Base64VatId = Base64VatId;
                    writer.Host = Host;
                    writer.Port = Port;
                    writer.Alias = Alias;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public string Base64VatId
                {
                    get;
                    set;
                }

                public string Host
                {
                    get;
                    set;
                }

                public ushort Port
                {
                    get;
                    set;
                }

                public string Alias
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
                    public string Base64VatId => ctx.ReadText(0, null);
                    public string Host => ctx.ReadText(1, null);
                    public ushort Port => ctx.ReadDataUShort(0UL, (ushort)0);
                    public string Alias => ctx.ReadText(2, null);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 3);
                    }

                    public string Base64VatId
                    {
                        get => this.ReadText(0, null);
                        set => this.WriteText(0, value, null);
                    }

                    public string Host
                    {
                        get => this.ReadText(1, null);
                        set => this.WriteText(1, value, null);
                    }

                    public ushort Port
                    {
                        get => this.ReadDataUShort(0UL, (ushort)0);
                        set => this.WriteData(0UL, value, (ushort)0);
                    }

                    public string Alias
                    {
                        get => this.ReadText(2, null);
                        set => this.WriteText(2, value, null);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfafc816633f98bb9UL)]
            public class Result_Register : ICapnpSerializable
            {
                public const UInt64 typeId = 0xfafc816633f98bb9UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Heartbeat = reader.Heartbeat;
                    SecsHeartbeatInterval = reader.SecsHeartbeatInterval;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Heartbeat = Heartbeat;
                    writer.SecsHeartbeatInterval = SecsHeartbeatInterval;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Schema.Persistence.HostPortResolver.Registrar.IHeartbeat Heartbeat
                {
                    get;
                    set;
                }

                public uint SecsHeartbeatInterval
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
                    public Mas.Schema.Persistence.HostPortResolver.Registrar.IHeartbeat Heartbeat => ctx.ReadCap<Mas.Schema.Persistence.HostPortResolver.Registrar.IHeartbeat>(0);
                    public uint SecsHeartbeatInterval => ctx.ReadDataUInt(0UL, 0U);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 1);
                    }

                    public Mas.Schema.Persistence.HostPortResolver.Registrar.IHeartbeat Heartbeat
                    {
                        get => ReadCap<Mas.Schema.Persistence.HostPortResolver.Registrar.IHeartbeat>(0);
                        set => LinkObject(0, value);
                    }

                    public uint SecsHeartbeatInterval
                    {
                        get => this.ReadDataUInt(0UL, 0U);
                        set => this.WriteData(0UL, value, 0U);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe6f8966f0f2cbb33UL)]
        public class Params_Resolve : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe6f8966f0f2cbb33UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfc185f518d220b8cUL)]
        public class Result_Resolve : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfc185f518d220b8cUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Host = reader.Host;
                Port = reader.Port;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Host = Host;
                writer.Port = Port;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Host
            {
                get;
                set;
            }

            public ushort Port
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
                public string Host => ctx.ReadText(0, null);
                public ushort Port => ctx.ReadDataUShort(0UL, (ushort)0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public string Host
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public ushort Port
                {
                    get => this.ReadDataUShort(0UL, (ushort)0);
                    set => this.WriteData(0UL, value, (ushort)0);
                }
            }
        }
    }
}