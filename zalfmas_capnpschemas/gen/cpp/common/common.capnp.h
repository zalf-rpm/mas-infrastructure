// Generated by Cap'n Proto compiler, DO NOT EDIT
// source: common.capnp

#pragma once

#include <capnp/generated-header-support.h>
#include <kj/windows-sanity.h>
#if !CAPNP_LITE
#include <capnp/capability.h>
#endif  // !CAPNP_LITE

#ifndef CAPNP_VERSION
#error "CAPNP_VERSION is not defined, is capnp/generated-header-support.h missing?"
#elif CAPNP_VERSION != 1000001
#error "Version mismatch between generated code and library headers.  You must use the same version of the Cap'n Proto compiler and library."
#endif


CAPNP_BEGIN_HEADER

namespace capnp {
namespace schemas {

CAPNP_DECLARE_SCHEMA(d4cb7ecbfe03dad3);
CAPNP_DECLARE_SCHEMA(b2afd1cb599c48d5);
CAPNP_DECLARE_SCHEMA(9d8aa1cf1e49deb1);
CAPNP_DECLARE_SCHEMA(ed6c098b67cad454);
CAPNP_DECLARE_SCHEMA(e8cbf552b1c262cc);
CAPNP_DECLARE_SCHEMA(b9d4864725174733);

}  // namespace schemas
}  // namespace capnp

namespace mas {
namespace schema {
namespace common {

struct IdInformation {
  IdInformation() = delete;

  class Reader;
  class Builder;
  class Pipeline;

  struct _capnpPrivate {
    CAPNP_DECLARE_STRUCT_HEADER(d4cb7ecbfe03dad3, 0, 3)
    #if !CAPNP_LITE
    static constexpr ::capnp::_::RawBrandedSchema const* brand() { return &schema->defaultBrand; }
    #endif  // !CAPNP_LITE
  };
};

struct Identifiable {
  Identifiable() = delete;

#if !CAPNP_LITE
  class Client;
  class Server;
#endif  // !CAPNP_LITE

  struct InfoParams;

  #if !CAPNP_LITE
  struct _capnpPrivate {
    CAPNP_DECLARE_INTERFACE_HEADER(b2afd1cb599c48d5)
    static constexpr ::capnp::_::RawBrandedSchema const* brand() { return &schema->defaultBrand; }
  };
  #endif  // !CAPNP_LITE
};

struct Identifiable::InfoParams {
  InfoParams() = delete;

  class Reader;
  class Builder;
  class Pipeline;

  struct _capnpPrivate {
    CAPNP_DECLARE_STRUCT_HEADER(9d8aa1cf1e49deb1, 0, 0)
    #if !CAPNP_LITE
    static constexpr ::capnp::_::RawBrandedSchema const* brand() { return &schema->defaultBrand; }
    #endif  // !CAPNP_LITE
  };
};

struct StructuredText {
  StructuredText() = delete;

  class Reader;
  class Builder;
  class Pipeline;
  struct Structure;

  struct _capnpPrivate {
    CAPNP_DECLARE_STRUCT_HEADER(ed6c098b67cad454, 1, 1)
    #if !CAPNP_LITE
    static constexpr ::capnp::_::RawBrandedSchema const* brand() { return &schema->defaultBrand; }
    #endif  // !CAPNP_LITE
  };
};

struct StructuredText::Structure {
  Structure() = delete;

  class Reader;
  class Builder;
  class Pipeline;
  enum Which: uint16_t {
    NONE,
    JSON,
    XML,
  };

  struct _capnpPrivate {
    CAPNP_DECLARE_STRUCT_HEADER(e8cbf552b1c262cc, 1, 1)
    #if !CAPNP_LITE
    static constexpr ::capnp::_::RawBrandedSchema const* brand() { return &schema->defaultBrand; }
    #endif  // !CAPNP_LITE
  };
};

template <typename F = ::capnp::AnyPointer, typename S = ::capnp::AnyPointer>
struct Pair {
  Pair() = delete;

  class Reader;
  class Builder;
  class Pipeline;

  struct _capnpPrivate {
    CAPNP_DECLARE_STRUCT_HEADER(b9d4864725174733, 0, 2)
    #if !CAPNP_LITE
    static const ::capnp::_::RawBrandedSchema::Scope brandScopes[];
    static const ::capnp::_::RawBrandedSchema::Binding brandBindings[];
    static const ::capnp::_::RawBrandedSchema specificBrand;
    static constexpr ::capnp::_::RawBrandedSchema const* brand() { return ::capnp::_::ChooseBrand<_capnpPrivate, F, S>::brand(); }
    #endif  // !CAPNP_LITE
  };
};

// =======================================================================================

class IdInformation::Reader {
public:
  typedef IdInformation Reads;

  Reader() = default;
  inline explicit Reader(::capnp::_::StructReader base): _reader(base) {}

  inline ::capnp::MessageSize totalSize() const {
    return _reader.totalSize().asPublic();
  }

#if !CAPNP_LITE
  inline ::kj::StringTree toString() const {
    return ::capnp::_::structString(_reader, *_capnpPrivate::brand());
  }
#endif  // !CAPNP_LITE

  inline bool hasId() const;
  inline  ::capnp::Text::Reader getId() const;

  inline bool hasName() const;
  inline  ::capnp::Text::Reader getName() const;

  inline bool hasDescription() const;
  inline  ::capnp::Text::Reader getDescription() const;

private:
  ::capnp::_::StructReader _reader;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::ToDynamic_;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::_::PointerHelpers;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::List;
  friend class ::capnp::MessageBuilder;
  friend class ::capnp::Orphanage;
};

class IdInformation::Builder {
public:
  typedef IdInformation Builds;

  Builder() = delete;  // Deleted to discourage incorrect usage.
                       // You can explicitly initialize to nullptr instead.
  inline Builder(decltype(nullptr)) {}
  inline explicit Builder(::capnp::_::StructBuilder base): _builder(base) {}
  inline operator Reader() const { return Reader(_builder.asReader()); }
  inline Reader asReader() const { return *this; }

  inline ::capnp::MessageSize totalSize() const { return asReader().totalSize(); }
#if !CAPNP_LITE
  inline ::kj::StringTree toString() const { return asReader().toString(); }
#endif  // !CAPNP_LITE

  inline bool hasId();
  inline  ::capnp::Text::Builder getId();
  inline void setId( ::capnp::Text::Reader value);
  inline  ::capnp::Text::Builder initId(unsigned int size);
  inline void adoptId(::capnp::Orphan< ::capnp::Text>&& value);
  inline ::capnp::Orphan< ::capnp::Text> disownId();

  inline bool hasName();
  inline  ::capnp::Text::Builder getName();
  inline void setName( ::capnp::Text::Reader value);
  inline  ::capnp::Text::Builder initName(unsigned int size);
  inline void adoptName(::capnp::Orphan< ::capnp::Text>&& value);
  inline ::capnp::Orphan< ::capnp::Text> disownName();

  inline bool hasDescription();
  inline  ::capnp::Text::Builder getDescription();
  inline void setDescription( ::capnp::Text::Reader value);
  inline  ::capnp::Text::Builder initDescription(unsigned int size);
  inline void adoptDescription(::capnp::Orphan< ::capnp::Text>&& value);
  inline ::capnp::Orphan< ::capnp::Text> disownDescription();

private:
  ::capnp::_::StructBuilder _builder;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::ToDynamic_;
  friend class ::capnp::Orphanage;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::_::PointerHelpers;
};

#if !CAPNP_LITE
class IdInformation::Pipeline {
public:
  typedef IdInformation Pipelines;

  inline Pipeline(decltype(nullptr)): _typeless(nullptr) {}
  inline explicit Pipeline(::capnp::AnyPointer::Pipeline&& typeless)
      : _typeless(kj::mv(typeless)) {}

private:
  ::capnp::AnyPointer::Pipeline _typeless;
  friend class ::capnp::PipelineHook;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::ToDynamic_;
};
#endif  // !CAPNP_LITE

#if !CAPNP_LITE
class Identifiable::Client
    : public virtual ::capnp::Capability::Client {
public:
  typedef Identifiable Calls;
  typedef Identifiable Reads;

  Client(decltype(nullptr));
  explicit Client(::kj::Own< ::capnp::ClientHook>&& hook);
  template <typename _t, typename = ::kj::EnableIf< ::kj::canConvert<_t*, Server*>()>>
  Client(::kj::Own<_t>&& server);
  template <typename _t, typename = ::kj::EnableIf< ::kj::canConvert<_t*, Client*>()>>
  Client(::kj::Promise<_t>&& promise);
  Client(::kj::Exception&& exception);
  Client(Client&) = default;
  Client(Client&&) = default;
  Client& operator=(Client& other);
  Client& operator=(Client&& other);

  ::capnp::Request< ::mas::schema::common::Identifiable::InfoParams,  ::mas::schema::common::IdInformation> infoRequest(
      ::kj::Maybe< ::capnp::MessageSize> sizeHint = nullptr);

protected:
  Client() = default;
};

class Identifiable::Server
    : public virtual ::capnp::Capability::Server {
public:
  typedef Identifiable Serves;

  ::capnp::Capability::Server::DispatchCallResult dispatchCall(
      uint64_t interfaceId, uint16_t methodId,
      ::capnp::CallContext< ::capnp::AnyPointer, ::capnp::AnyPointer> context)
      override;

protected:
  typedef  ::mas::schema::common::Identifiable::InfoParams InfoParams;
  typedef ::capnp::CallContext<InfoParams,  ::mas::schema::common::IdInformation> InfoContext;
  virtual ::kj::Promise<void> info(InfoContext context);

  inline  ::mas::schema::common::Identifiable::Client thisCap() {
    return ::capnp::Capability::Server::thisCap()
        .template castAs< ::mas::schema::common::Identifiable>();
  }

  ::capnp::Capability::Server::DispatchCallResult dispatchCallInternal(
      uint16_t methodId,
      ::capnp::CallContext< ::capnp::AnyPointer, ::capnp::AnyPointer> context);
};
#endif  // !CAPNP_LITE

class Identifiable::InfoParams::Reader {
public:
  typedef InfoParams Reads;

  Reader() = default;
  inline explicit Reader(::capnp::_::StructReader base): _reader(base) {}

  inline ::capnp::MessageSize totalSize() const {
    return _reader.totalSize().asPublic();
  }

#if !CAPNP_LITE
  inline ::kj::StringTree toString() const {
    return ::capnp::_::structString(_reader, *_capnpPrivate::brand());
  }
#endif  // !CAPNP_LITE

private:
  ::capnp::_::StructReader _reader;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::ToDynamic_;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::_::PointerHelpers;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::List;
  friend class ::capnp::MessageBuilder;
  friend class ::capnp::Orphanage;
};

class Identifiable::InfoParams::Builder {
public:
  typedef InfoParams Builds;

  Builder() = delete;  // Deleted to discourage incorrect usage.
                       // You can explicitly initialize to nullptr instead.
  inline Builder(decltype(nullptr)) {}
  inline explicit Builder(::capnp::_::StructBuilder base): _builder(base) {}
  inline operator Reader() const { return Reader(_builder.asReader()); }
  inline Reader asReader() const { return *this; }

  inline ::capnp::MessageSize totalSize() const { return asReader().totalSize(); }
#if !CAPNP_LITE
  inline ::kj::StringTree toString() const { return asReader().toString(); }
#endif  // !CAPNP_LITE

private:
  ::capnp::_::StructBuilder _builder;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::ToDynamic_;
  friend class ::capnp::Orphanage;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::_::PointerHelpers;
};

#if !CAPNP_LITE
class Identifiable::InfoParams::Pipeline {
public:
  typedef InfoParams Pipelines;

  inline Pipeline(decltype(nullptr)): _typeless(nullptr) {}
  inline explicit Pipeline(::capnp::AnyPointer::Pipeline&& typeless)
      : _typeless(kj::mv(typeless)) {}

private:
  ::capnp::AnyPointer::Pipeline _typeless;
  friend class ::capnp::PipelineHook;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::ToDynamic_;
};
#endif  // !CAPNP_LITE

class StructuredText::Reader {
public:
  typedef StructuredText Reads;

  Reader() = default;
  inline explicit Reader(::capnp::_::StructReader base): _reader(base) {}

  inline ::capnp::MessageSize totalSize() const {
    return _reader.totalSize().asPublic();
  }

#if !CAPNP_LITE
  inline ::kj::StringTree toString() const {
    return ::capnp::_::structString(_reader, *_capnpPrivate::brand());
  }
#endif  // !CAPNP_LITE

  inline bool hasValue() const;
  inline  ::capnp::Text::Reader getValue() const;

  inline typename Structure::Reader getStructure() const;

private:
  ::capnp::_::StructReader _reader;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::ToDynamic_;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::_::PointerHelpers;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::List;
  friend class ::capnp::MessageBuilder;
  friend class ::capnp::Orphanage;
};

class StructuredText::Builder {
public:
  typedef StructuredText Builds;

  Builder() = delete;  // Deleted to discourage incorrect usage.
                       // You can explicitly initialize to nullptr instead.
  inline Builder(decltype(nullptr)) {}
  inline explicit Builder(::capnp::_::StructBuilder base): _builder(base) {}
  inline operator Reader() const { return Reader(_builder.asReader()); }
  inline Reader asReader() const { return *this; }

  inline ::capnp::MessageSize totalSize() const { return asReader().totalSize(); }
#if !CAPNP_LITE
  inline ::kj::StringTree toString() const { return asReader().toString(); }
#endif  // !CAPNP_LITE

  inline bool hasValue();
  inline  ::capnp::Text::Builder getValue();
  inline void setValue( ::capnp::Text::Reader value);
  inline  ::capnp::Text::Builder initValue(unsigned int size);
  inline void adoptValue(::capnp::Orphan< ::capnp::Text>&& value);
  inline ::capnp::Orphan< ::capnp::Text> disownValue();

  inline typename Structure::Builder getStructure();
  inline typename Structure::Builder initStructure();

private:
  ::capnp::_::StructBuilder _builder;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::ToDynamic_;
  friend class ::capnp::Orphanage;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::_::PointerHelpers;
};

#if !CAPNP_LITE
class StructuredText::Pipeline {
public:
  typedef StructuredText Pipelines;

  inline Pipeline(decltype(nullptr)): _typeless(nullptr) {}
  inline explicit Pipeline(::capnp::AnyPointer::Pipeline&& typeless)
      : _typeless(kj::mv(typeless)) {}

  inline typename Structure::Pipeline getStructure();
private:
  ::capnp::AnyPointer::Pipeline _typeless;
  friend class ::capnp::PipelineHook;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::ToDynamic_;
};
#endif  // !CAPNP_LITE

class StructuredText::Structure::Reader {
public:
  typedef Structure Reads;

  Reader() = default;
  inline explicit Reader(::capnp::_::StructReader base): _reader(base) {}

  inline ::capnp::MessageSize totalSize() const {
    return _reader.totalSize().asPublic();
  }

#if !CAPNP_LITE
  inline ::kj::StringTree toString() const {
    return ::capnp::_::structString(_reader, *_capnpPrivate::brand());
  }
#endif  // !CAPNP_LITE

  inline Which which() const;
  inline bool isNone() const;
  inline  ::capnp::Void getNone() const;

  inline bool isJson() const;
  inline  ::capnp::Void getJson() const;

  inline bool isXml() const;
  inline  ::capnp::Void getXml() const;

private:
  ::capnp::_::StructReader _reader;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::ToDynamic_;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::_::PointerHelpers;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::List;
  friend class ::capnp::MessageBuilder;
  friend class ::capnp::Orphanage;
};

class StructuredText::Structure::Builder {
public:
  typedef Structure Builds;

  Builder() = delete;  // Deleted to discourage incorrect usage.
                       // You can explicitly initialize to nullptr instead.
  inline Builder(decltype(nullptr)) {}
  inline explicit Builder(::capnp::_::StructBuilder base): _builder(base) {}
  inline operator Reader() const { return Reader(_builder.asReader()); }
  inline Reader asReader() const { return *this; }

  inline ::capnp::MessageSize totalSize() const { return asReader().totalSize(); }
#if !CAPNP_LITE
  inline ::kj::StringTree toString() const { return asReader().toString(); }
#endif  // !CAPNP_LITE

  inline Which which();
  inline bool isNone();
  inline  ::capnp::Void getNone();
  inline void setNone( ::capnp::Void value = ::capnp::VOID);

  inline bool isJson();
  inline  ::capnp::Void getJson();
  inline void setJson( ::capnp::Void value = ::capnp::VOID);

  inline bool isXml();
  inline  ::capnp::Void getXml();
  inline void setXml( ::capnp::Void value = ::capnp::VOID);

private:
  ::capnp::_::StructBuilder _builder;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::ToDynamic_;
  friend class ::capnp::Orphanage;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::_::PointerHelpers;
};

#if !CAPNP_LITE
class StructuredText::Structure::Pipeline {
public:
  typedef Structure Pipelines;

  inline Pipeline(decltype(nullptr)): _typeless(nullptr) {}
  inline explicit Pipeline(::capnp::AnyPointer::Pipeline&& typeless)
      : _typeless(kj::mv(typeless)) {}

private:
  ::capnp::AnyPointer::Pipeline _typeless;
  friend class ::capnp::PipelineHook;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::ToDynamic_;
};
#endif  // !CAPNP_LITE

template <typename F, typename S>
class Pair<F, S>::Reader {
public:
  typedef Pair Reads;

  Reader() = default;
  inline explicit Reader(::capnp::_::StructReader base): _reader(base) {}

  inline ::capnp::MessageSize totalSize() const {
    return _reader.totalSize().asPublic();
  }

#if !CAPNP_LITE
  inline ::kj::StringTree toString() const {
    return ::capnp::_::structString(_reader, *_capnpPrivate::brand());
  }
#endif  // !CAPNP_LITE

  template <typename F2 = ::capnp::AnyPointer, typename S2 = ::capnp::AnyPointer>
  typename Pair<F2, S2>::Reader asGeneric() {
    return typename Pair<F2, S2>::Reader(_reader);
  }

  inline bool hasFst() const;
  inline  ::capnp::ReaderFor<F> getFst() const;

  inline bool hasSnd() const;
  inline  ::capnp::ReaderFor<S> getSnd() const;

private:
  ::capnp::_::StructReader _reader;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::ToDynamic_;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::_::PointerHelpers;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::List;
  friend class ::capnp::MessageBuilder;
  friend class ::capnp::Orphanage;
};

template <typename F, typename S>
class Pair<F, S>::Builder {
public:
  typedef Pair Builds;

  Builder() = delete;  // Deleted to discourage incorrect usage.
                       // You can explicitly initialize to nullptr instead.
  inline Builder(decltype(nullptr)) {}
  inline explicit Builder(::capnp::_::StructBuilder base): _builder(base) {}
  inline operator Reader() const { return Reader(_builder.asReader()); }
  inline Reader asReader() const { return *this; }

  inline ::capnp::MessageSize totalSize() const { return asReader().totalSize(); }
#if !CAPNP_LITE
  inline ::kj::StringTree toString() const { return asReader().toString(); }
#endif  // !CAPNP_LITE

  template <typename F2 = ::capnp::AnyPointer, typename S2 = ::capnp::AnyPointer>
  typename Pair<F2, S2>::Builder asGeneric() {
    return typename Pair<F2, S2>::Builder(_builder);
  }

  inline bool hasFst();
  inline  ::capnp::BuilderFor<F> getFst();
  inline void setFst( ::capnp::ReaderFor<F> value);
  inline  ::capnp::BuilderFor<F> initFst();
  inline  ::capnp::BuilderFor<F> initFst(unsigned int size);
  inline void adoptFst(::capnp::Orphan<F>&& value);
  inline ::capnp::Orphan<F> disownFst();

  inline bool hasSnd();
  inline  ::capnp::BuilderFor<S> getSnd();
  inline void setSnd( ::capnp::ReaderFor<S> value);
  inline  ::capnp::BuilderFor<S> initSnd();
  inline  ::capnp::BuilderFor<S> initSnd(unsigned int size);
  inline void adoptSnd(::capnp::Orphan<S>&& value);
  inline ::capnp::Orphan<S> disownSnd();

private:
  ::capnp::_::StructBuilder _builder;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::ToDynamic_;
  friend class ::capnp::Orphanage;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::_::PointerHelpers;
};

#if !CAPNP_LITE
template <typename F, typename S>
class Pair<F, S>::Pipeline {
public:
  typedef Pair Pipelines;

  inline Pipeline(decltype(nullptr)): _typeless(nullptr) {}
  inline explicit Pipeline(::capnp::AnyPointer::Pipeline&& typeless)
      : _typeless(kj::mv(typeless)) {}

  inline  ::capnp::PipelineFor<F> getFst();
  inline  ::capnp::PipelineFor<S> getSnd();
private:
  ::capnp::AnyPointer::Pipeline _typeless;
  friend class ::capnp::PipelineHook;
  template <typename, ::capnp::Kind>
  friend struct ::capnp::ToDynamic_;
};
#endif  // !CAPNP_LITE

// =======================================================================================

inline bool IdInformation::Reader::hasId() const {
  return !_reader.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS).isNull();
}
inline bool IdInformation::Builder::hasId() {
  return !_builder.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS).isNull();
}
inline  ::capnp::Text::Reader IdInformation::Reader::getId() const {
  return ::capnp::_::PointerHelpers< ::capnp::Text>::get(_reader.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS));
}
inline  ::capnp::Text::Builder IdInformation::Builder::getId() {
  return ::capnp::_::PointerHelpers< ::capnp::Text>::get(_builder.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS));
}
inline void IdInformation::Builder::setId( ::capnp::Text::Reader value) {
  ::capnp::_::PointerHelpers< ::capnp::Text>::set(_builder.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS), value);
}
inline  ::capnp::Text::Builder IdInformation::Builder::initId(unsigned int size) {
  return ::capnp::_::PointerHelpers< ::capnp::Text>::init(_builder.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS), size);
}
inline void IdInformation::Builder::adoptId(
    ::capnp::Orphan< ::capnp::Text>&& value) {
  ::capnp::_::PointerHelpers< ::capnp::Text>::adopt(_builder.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS), kj::mv(value));
}
inline ::capnp::Orphan< ::capnp::Text> IdInformation::Builder::disownId() {
  return ::capnp::_::PointerHelpers< ::capnp::Text>::disown(_builder.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS));
}

inline bool IdInformation::Reader::hasName() const {
  return !_reader.getPointerField(
      ::capnp::bounded<1>() * ::capnp::POINTERS).isNull();
}
inline bool IdInformation::Builder::hasName() {
  return !_builder.getPointerField(
      ::capnp::bounded<1>() * ::capnp::POINTERS).isNull();
}
inline  ::capnp::Text::Reader IdInformation::Reader::getName() const {
  return ::capnp::_::PointerHelpers< ::capnp::Text>::get(_reader.getPointerField(
      ::capnp::bounded<1>() * ::capnp::POINTERS));
}
inline  ::capnp::Text::Builder IdInformation::Builder::getName() {
  return ::capnp::_::PointerHelpers< ::capnp::Text>::get(_builder.getPointerField(
      ::capnp::bounded<1>() * ::capnp::POINTERS));
}
inline void IdInformation::Builder::setName( ::capnp::Text::Reader value) {
  ::capnp::_::PointerHelpers< ::capnp::Text>::set(_builder.getPointerField(
      ::capnp::bounded<1>() * ::capnp::POINTERS), value);
}
inline  ::capnp::Text::Builder IdInformation::Builder::initName(unsigned int size) {
  return ::capnp::_::PointerHelpers< ::capnp::Text>::init(_builder.getPointerField(
      ::capnp::bounded<1>() * ::capnp::POINTERS), size);
}
inline void IdInformation::Builder::adoptName(
    ::capnp::Orphan< ::capnp::Text>&& value) {
  ::capnp::_::PointerHelpers< ::capnp::Text>::adopt(_builder.getPointerField(
      ::capnp::bounded<1>() * ::capnp::POINTERS), kj::mv(value));
}
inline ::capnp::Orphan< ::capnp::Text> IdInformation::Builder::disownName() {
  return ::capnp::_::PointerHelpers< ::capnp::Text>::disown(_builder.getPointerField(
      ::capnp::bounded<1>() * ::capnp::POINTERS));
}

inline bool IdInformation::Reader::hasDescription() const {
  return !_reader.getPointerField(
      ::capnp::bounded<2>() * ::capnp::POINTERS).isNull();
}
inline bool IdInformation::Builder::hasDescription() {
  return !_builder.getPointerField(
      ::capnp::bounded<2>() * ::capnp::POINTERS).isNull();
}
inline  ::capnp::Text::Reader IdInformation::Reader::getDescription() const {
  return ::capnp::_::PointerHelpers< ::capnp::Text>::get(_reader.getPointerField(
      ::capnp::bounded<2>() * ::capnp::POINTERS));
}
inline  ::capnp::Text::Builder IdInformation::Builder::getDescription() {
  return ::capnp::_::PointerHelpers< ::capnp::Text>::get(_builder.getPointerField(
      ::capnp::bounded<2>() * ::capnp::POINTERS));
}
inline void IdInformation::Builder::setDescription( ::capnp::Text::Reader value) {
  ::capnp::_::PointerHelpers< ::capnp::Text>::set(_builder.getPointerField(
      ::capnp::bounded<2>() * ::capnp::POINTERS), value);
}
inline  ::capnp::Text::Builder IdInformation::Builder::initDescription(unsigned int size) {
  return ::capnp::_::PointerHelpers< ::capnp::Text>::init(_builder.getPointerField(
      ::capnp::bounded<2>() * ::capnp::POINTERS), size);
}
inline void IdInformation::Builder::adoptDescription(
    ::capnp::Orphan< ::capnp::Text>&& value) {
  ::capnp::_::PointerHelpers< ::capnp::Text>::adopt(_builder.getPointerField(
      ::capnp::bounded<2>() * ::capnp::POINTERS), kj::mv(value));
}
inline ::capnp::Orphan< ::capnp::Text> IdInformation::Builder::disownDescription() {
  return ::capnp::_::PointerHelpers< ::capnp::Text>::disown(_builder.getPointerField(
      ::capnp::bounded<2>() * ::capnp::POINTERS));
}

#if !CAPNP_LITE
inline Identifiable::Client::Client(decltype(nullptr))
    : ::capnp::Capability::Client(nullptr) {}
inline Identifiable::Client::Client(
    ::kj::Own< ::capnp::ClientHook>&& hook)
    : ::capnp::Capability::Client(::kj::mv(hook)) {}
template <typename _t, typename>
inline Identifiable::Client::Client(::kj::Own<_t>&& server)
    : ::capnp::Capability::Client(::kj::mv(server)) {}
template <typename _t, typename>
inline Identifiable::Client::Client(::kj::Promise<_t>&& promise)
    : ::capnp::Capability::Client(::kj::mv(promise)) {}
inline Identifiable::Client::Client(::kj::Exception&& exception)
    : ::capnp::Capability::Client(::kj::mv(exception)) {}
inline  ::mas::schema::common::Identifiable::Client& Identifiable::Client::operator=(Client& other) {
  ::capnp::Capability::Client::operator=(other);
  return *this;
}
inline  ::mas::schema::common::Identifiable::Client& Identifiable::Client::operator=(Client&& other) {
  ::capnp::Capability::Client::operator=(kj::mv(other));
  return *this;
}

#endif  // !CAPNP_LITE
inline bool StructuredText::Reader::hasValue() const {
  return !_reader.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS).isNull();
}
inline bool StructuredText::Builder::hasValue() {
  return !_builder.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS).isNull();
}
inline  ::capnp::Text::Reader StructuredText::Reader::getValue() const {
  return ::capnp::_::PointerHelpers< ::capnp::Text>::get(_reader.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS));
}
inline  ::capnp::Text::Builder StructuredText::Builder::getValue() {
  return ::capnp::_::PointerHelpers< ::capnp::Text>::get(_builder.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS));
}
inline void StructuredText::Builder::setValue( ::capnp::Text::Reader value) {
  ::capnp::_::PointerHelpers< ::capnp::Text>::set(_builder.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS), value);
}
inline  ::capnp::Text::Builder StructuredText::Builder::initValue(unsigned int size) {
  return ::capnp::_::PointerHelpers< ::capnp::Text>::init(_builder.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS), size);
}
inline void StructuredText::Builder::adoptValue(
    ::capnp::Orphan< ::capnp::Text>&& value) {
  ::capnp::_::PointerHelpers< ::capnp::Text>::adopt(_builder.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS), kj::mv(value));
}
inline ::capnp::Orphan< ::capnp::Text> StructuredText::Builder::disownValue() {
  return ::capnp::_::PointerHelpers< ::capnp::Text>::disown(_builder.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS));
}

inline typename StructuredText::Structure::Reader StructuredText::Reader::getStructure() const {
  return typename StructuredText::Structure::Reader(_reader);
}
inline typename StructuredText::Structure::Builder StructuredText::Builder::getStructure() {
  return typename StructuredText::Structure::Builder(_builder);
}
#if !CAPNP_LITE
inline typename StructuredText::Structure::Pipeline StructuredText::Pipeline::getStructure() {
  return typename StructuredText::Structure::Pipeline(_typeless.noop());
}
#endif  // !CAPNP_LITE
inline typename StructuredText::Structure::Builder StructuredText::Builder::initStructure() {
  _builder.setDataField< ::uint16_t>(::capnp::bounded<0>() * ::capnp::ELEMENTS, 0);
  return typename StructuredText::Structure::Builder(_builder);
}
inline  ::mas::schema::common::StructuredText::Structure::Which StructuredText::Structure::Reader::which() const {
  return _reader.getDataField<Which>(
      ::capnp::bounded<0>() * ::capnp::ELEMENTS);
}
inline  ::mas::schema::common::StructuredText::Structure::Which StructuredText::Structure::Builder::which() {
  return _builder.getDataField<Which>(
      ::capnp::bounded<0>() * ::capnp::ELEMENTS);
}

inline bool StructuredText::Structure::Reader::isNone() const {
  return which() == StructuredText::Structure::NONE;
}
inline bool StructuredText::Structure::Builder::isNone() {
  return which() == StructuredText::Structure::NONE;
}
inline  ::capnp::Void StructuredText::Structure::Reader::getNone() const {
  KJ_IREQUIRE((which() == StructuredText::Structure::NONE),
              "Must check which() before get()ing a union member.");
  return _reader.getDataField< ::capnp::Void>(
      ::capnp::bounded<0>() * ::capnp::ELEMENTS);
}

inline  ::capnp::Void StructuredText::Structure::Builder::getNone() {
  KJ_IREQUIRE((which() == StructuredText::Structure::NONE),
              "Must check which() before get()ing a union member.");
  return _builder.getDataField< ::capnp::Void>(
      ::capnp::bounded<0>() * ::capnp::ELEMENTS);
}
inline void StructuredText::Structure::Builder::setNone( ::capnp::Void value) {
  _builder.setDataField<StructuredText::Structure::Which>(
      ::capnp::bounded<0>() * ::capnp::ELEMENTS, StructuredText::Structure::NONE);
  _builder.setDataField< ::capnp::Void>(
      ::capnp::bounded<0>() * ::capnp::ELEMENTS, value);
}

inline bool StructuredText::Structure::Reader::isJson() const {
  return which() == StructuredText::Structure::JSON;
}
inline bool StructuredText::Structure::Builder::isJson() {
  return which() == StructuredText::Structure::JSON;
}
inline  ::capnp::Void StructuredText::Structure::Reader::getJson() const {
  KJ_IREQUIRE((which() == StructuredText::Structure::JSON),
              "Must check which() before get()ing a union member.");
  return _reader.getDataField< ::capnp::Void>(
      ::capnp::bounded<0>() * ::capnp::ELEMENTS);
}

inline  ::capnp::Void StructuredText::Structure::Builder::getJson() {
  KJ_IREQUIRE((which() == StructuredText::Structure::JSON),
              "Must check which() before get()ing a union member.");
  return _builder.getDataField< ::capnp::Void>(
      ::capnp::bounded<0>() * ::capnp::ELEMENTS);
}
inline void StructuredText::Structure::Builder::setJson( ::capnp::Void value) {
  _builder.setDataField<StructuredText::Structure::Which>(
      ::capnp::bounded<0>() * ::capnp::ELEMENTS, StructuredText::Structure::JSON);
  _builder.setDataField< ::capnp::Void>(
      ::capnp::bounded<0>() * ::capnp::ELEMENTS, value);
}

inline bool StructuredText::Structure::Reader::isXml() const {
  return which() == StructuredText::Structure::XML;
}
inline bool StructuredText::Structure::Builder::isXml() {
  return which() == StructuredText::Structure::XML;
}
inline  ::capnp::Void StructuredText::Structure::Reader::getXml() const {
  KJ_IREQUIRE((which() == StructuredText::Structure::XML),
              "Must check which() before get()ing a union member.");
  return _reader.getDataField< ::capnp::Void>(
      ::capnp::bounded<0>() * ::capnp::ELEMENTS);
}

inline  ::capnp::Void StructuredText::Structure::Builder::getXml() {
  KJ_IREQUIRE((which() == StructuredText::Structure::XML),
              "Must check which() before get()ing a union member.");
  return _builder.getDataField< ::capnp::Void>(
      ::capnp::bounded<0>() * ::capnp::ELEMENTS);
}
inline void StructuredText::Structure::Builder::setXml( ::capnp::Void value) {
  _builder.setDataField<StructuredText::Structure::Which>(
      ::capnp::bounded<0>() * ::capnp::ELEMENTS, StructuredText::Structure::XML);
  _builder.setDataField< ::capnp::Void>(
      ::capnp::bounded<0>() * ::capnp::ELEMENTS, value);
}

template <typename F, typename S>
inline bool Pair<F, S>::Reader::hasFst() const {
  return !_reader.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS).isNull();
}
template <typename F, typename S>
inline bool Pair<F, S>::Builder::hasFst() {
  return !_builder.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS).isNull();
}
template <typename F, typename S>
inline  ::capnp::ReaderFor<F> Pair<F, S>::Reader::getFst() const {
  return ::capnp::_::PointerHelpers<F>::get(_reader.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS));
}
template <typename F, typename S>
inline  ::capnp::BuilderFor<F> Pair<F, S>::Builder::getFst() {
  return ::capnp::_::PointerHelpers<F>::get(_builder.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS));
}
#if !CAPNP_LITE
template <typename F, typename S>
inline  ::capnp::PipelineFor<F> Pair<F, S>::Pipeline::getFst() {
  return  ::capnp::PipelineFor<F>(_typeless.getPointerField(0));
}
#endif  // !CAPNP_LITE
template <typename F, typename S>
inline void Pair<F, S>::Builder::setFst( ::capnp::ReaderFor<F> value) {
  ::capnp::_::PointerHelpers<F>::set(_builder.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS), value);
}
template <typename F, typename S>
inline  ::capnp::BuilderFor<F> Pair<F, S>::Builder::initFst() {
  return ::capnp::_::PointerHelpers<F>::init(_builder.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS));
}
template <typename F, typename S>
inline  ::capnp::BuilderFor<F> Pair<F, S>::Builder::initFst(unsigned int size) {
  return ::capnp::_::PointerHelpers<F>::init(_builder.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS), size);
}
template <typename F, typename S>
inline void Pair<F, S>::Builder::adoptFst(
    ::capnp::Orphan<F>&& value) {
  ::capnp::_::PointerHelpers<F>::adopt(_builder.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS), kj::mv(value));
}
template <typename F, typename S>
inline ::capnp::Orphan<F> Pair<F, S>::Builder::disownFst() {
  return ::capnp::_::PointerHelpers<F>::disown(_builder.getPointerField(
      ::capnp::bounded<0>() * ::capnp::POINTERS));
}

template <typename F, typename S>
inline bool Pair<F, S>::Reader::hasSnd() const {
  return !_reader.getPointerField(
      ::capnp::bounded<1>() * ::capnp::POINTERS).isNull();
}
template <typename F, typename S>
inline bool Pair<F, S>::Builder::hasSnd() {
  return !_builder.getPointerField(
      ::capnp::bounded<1>() * ::capnp::POINTERS).isNull();
}
template <typename F, typename S>
inline  ::capnp::ReaderFor<S> Pair<F, S>::Reader::getSnd() const {
  return ::capnp::_::PointerHelpers<S>::get(_reader.getPointerField(
      ::capnp::bounded<1>() * ::capnp::POINTERS));
}
template <typename F, typename S>
inline  ::capnp::BuilderFor<S> Pair<F, S>::Builder::getSnd() {
  return ::capnp::_::PointerHelpers<S>::get(_builder.getPointerField(
      ::capnp::bounded<1>() * ::capnp::POINTERS));
}
#if !CAPNP_LITE
template <typename F, typename S>
inline  ::capnp::PipelineFor<S> Pair<F, S>::Pipeline::getSnd() {
  return  ::capnp::PipelineFor<S>(_typeless.getPointerField(1));
}
#endif  // !CAPNP_LITE
template <typename F, typename S>
inline void Pair<F, S>::Builder::setSnd( ::capnp::ReaderFor<S> value) {
  ::capnp::_::PointerHelpers<S>::set(_builder.getPointerField(
      ::capnp::bounded<1>() * ::capnp::POINTERS), value);
}
template <typename F, typename S>
inline  ::capnp::BuilderFor<S> Pair<F, S>::Builder::initSnd() {
  return ::capnp::_::PointerHelpers<S>::init(_builder.getPointerField(
      ::capnp::bounded<1>() * ::capnp::POINTERS));
}
template <typename F, typename S>
inline  ::capnp::BuilderFor<S> Pair<F, S>::Builder::initSnd(unsigned int size) {
  return ::capnp::_::PointerHelpers<S>::init(_builder.getPointerField(
      ::capnp::bounded<1>() * ::capnp::POINTERS), size);
}
template <typename F, typename S>
inline void Pair<F, S>::Builder::adoptSnd(
    ::capnp::Orphan<S>&& value) {
  ::capnp::_::PointerHelpers<S>::adopt(_builder.getPointerField(
      ::capnp::bounded<1>() * ::capnp::POINTERS), kj::mv(value));
}
template <typename F, typename S>
inline ::capnp::Orphan<S> Pair<F, S>::Builder::disownSnd() {
  return ::capnp::_::PointerHelpers<S>::disown(_builder.getPointerField(
      ::capnp::bounded<1>() * ::capnp::POINTERS));
}

// Pair<F, S>
#if CAPNP_NEED_REDUNDANT_CONSTEXPR_DECL
template <typename F, typename S>
constexpr uint16_t Pair<F, S>::_capnpPrivate::dataWordSize;
template <typename F, typename S>
constexpr uint16_t Pair<F, S>::_capnpPrivate::pointerCount;
#endif  // !CAPNP_NEED_REDUNDANT_CONSTEXPR_DECL
#if !CAPNP_LITE
#if CAPNP_NEED_REDUNDANT_CONSTEXPR_DECL
template <typename F, typename S>
constexpr ::capnp::Kind Pair<F, S>::_capnpPrivate::kind;
template <typename F, typename S>
constexpr ::capnp::_::RawSchema const* Pair<F, S>::_capnpPrivate::schema;
#endif  // !CAPNP_NEED_REDUNDANT_CONSTEXPR_DECL
template <typename F, typename S>
const ::capnp::_::RawBrandedSchema::Scope Pair<F, S>::_capnpPrivate::brandScopes[] = {
  { 0xb9d4864725174733, brandBindings + 0, 2, false},
};
template <typename F, typename S>
const ::capnp::_::RawBrandedSchema::Binding Pair<F, S>::_capnpPrivate::brandBindings[] = {
  ::capnp::_::brandBindingFor<F>(),
  ::capnp::_::brandBindingFor<S>(),
};
template <typename F, typename S>
const ::capnp::_::RawBrandedSchema Pair<F, S>::_capnpPrivate::specificBrand = {
  &::capnp::schemas::s_b9d4864725174733, brandScopes, nullptr,
  1, 0, nullptr
};
#endif  // !CAPNP_LITE

}  // namespace
}  // namespace
}  // namespace

CAPNP_END_HEADER
