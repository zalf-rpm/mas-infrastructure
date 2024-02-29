const std = @import("std");

pub fn build(b: *std.build.Builder) void {
    const target = b.standardTargetOptions(.{});
    const mode = b.standardReleaseOptions();

    const test_x = b.addStaticLibrary("test_x", null);
    test_x.setTarget(target);
    test_x.setBuildMode(mode);
    test_x.linkLibC();
    test_x.force_pic = true;
    test_x.addCSourceFile("../../../capnproto_schemas/gen/cpp/test/x.capnp.cpp");
    //test_x.addCSourceFiles(&.{
    //    "../../../capnproto_schemas/gen/cpp/test/x.capnp.cpp",
    //}, &.{
    //    "-Wall",
        //"-W",
        //"-Wstrict-prototypes",
        //"-Wwrite-strings",
        //"-Wno-missing-field-initializers",
    //});

    const lua = b.addStaticLibrary("lua", null);
    lua.setTarget(target);
    lua.setBuildMode(mode);
    lua.linkLibC();
    lua.force_pic = true;
    lua.addCSourceFiles(&.{
        "deps/lua/src/fpconv.c",
        "deps/lua/src/lapi.c",
        "deps/lua/src/lauxlib.c",
        "deps/lua/src/lbaselib.c",
        "deps/lua/src/lcode.c",
        "deps/lua/src/ldblib.c",
        "deps/lua/src/ldebug.c",
        "deps/lua/src/ldo.c",
        "deps/lua/src/ldump.c",
        "deps/lua/src/lfunc.c",
        "deps/lua/src/lgc.c",
        "deps/lua/src/linit.c",
        "deps/lua/src/liolib.c",
        "deps/lua/src/llex.c",
        "deps/lua/src/lmathlib.c",
        "deps/lua/src/lmem.c",
        "deps/lua/src/loadlib.c",
        "deps/lua/src/lobject.c",
        "deps/lua/src/lopcodes.c",
        "deps/lua/src/loslib.c",
        "deps/lua/src/lparser.c",
        "deps/lua/src/lstate.c",
        "deps/lua/src/lstring.c",
        "deps/lua/src/lstrlib.c",
        "deps/lua/src/ltable.c",
        "deps/lua/src/ltablib.c",
        "deps/lua/src/ltm.c",
        "deps/lua/src/lua_bit.c",
        "deps/lua/src/lua_cjson.c",
        "deps/lua/src/lua_cmsgpack.c",
        "deps/lua/src/lua_struct.c",
        "deps/lua/src/lundump.c",
        "deps/lua/src/lvm.c",
        "deps/lua/src/lzio.c",
        "deps/lua/src/strbuf.c",
    }, &.{
        "-std=c99",
        "-Wall",
        "-DLUA_ANSI",
        "-DENABLE_CJSON_GLOBAL",
        "-DLUA_USE_MKSTEMP",
    });

    const redis_cli = b.addExecutable("redis-cli", null);
    redis_cli.setTarget(target);
    redis_cli.setBuildMode(mode);
    redis_cli.install();
    redis_cli.linkLibC();
    redis_cli.linkLibrary(hiredis);
    redis_cli.addIncludeDir("deps/hiredis");
    redis_cli.addIncludeDir("deps/linenoise");
    redis_cli.addIncludeDir("deps/lua/src");
    redis_cli.addCSourceFiles(&.{
        "src/adlist.c",
        "src/ae.c",
        "src/anet.c",
        "src/cli_common.c",
        "src/crc16.c",
        "src/crc64.c",
        "src/crcspeed.c",
        "src/dict.c",
        "src/monotonic.c",
        "src/mt19937-64.c",
        "src/redis-cli.c",
        "src/release.c",
        "src/redisassert.c",
        "src/siphash.c",
        "src/zmalloc.c",
        "deps/linenoise/linenoise.c",
    }, &.{
        "-std=c11",
        "-pedantic",
        "-Wall",
        "-W",
        "-Wno-missing-field-initializers",
    });

    const redis_server = b.addExecutable("redis-server", null);
    redis_server.setTarget(target);
    redis_server.setBuildMode(mode);
    redis_server.install();
    redis_server.linkLibC();
    redis_server.linkLibrary(hiredis);
    redis_server.linkLibrary(lua);
    redis_server.addIncludeDir("deps/hiredis");
    redis_server.addIncludeDir("deps/lua/src");
    redis_server.addCSourceFiles(&.{
        "src/acl.c",
        "src/adlist.c",
        "src/ae.c",
        "src/anet.c",
        "src/aof.c",
        "src/bio.c",
        "src/bitops.c",
        "src/blocked.c",
        "src/childinfo.c",
        "src/cluster.c",
        "src/config.c",
        "src/connection.c",
        "src/crc16.c",
        "src/crc64.c",
        "src/crcspeed.c",
        "src/db.c",
        "src/debug.c",
        "src/defrag.c",
        "src/dict.c",
        "src/endianconv.c",
        "src/evict.c",
        "src/expire.c",
        "src/geo.c",
        "src/geohash.c",
        "src/geohash_helper.c",
        "src/gopher.c",
        "src/hyperloglog.c",
        "src/intset.c",
        "src/latency.c",
        "src/lazyfree.c",
        "src/listpack.c",
        "src/localtime.c",
        "src/lolwut.c",
        "src/lolwut5.c",
        "src/lolwut6.c",
        "src/lzf_c.c",
        "src/lzf_d.c",
        "src/memtest.c",
        "src/module.c",
        "src/monotonic.c",
        "src/mt19937-64.c",
        "src/multi.c",
        "src/networking.c",
        "src/notify.c",
        "src/object.c",
        "src/pqsort.c",
        "src/pubsub.c",
        "src/quicklist.c",
        "src/rand.c",
        "src/rax.c",
        "src/rdb.c",
        "src/redis-check-aof.c",
        "src/redis-check-rdb.c",
        "src/release.c",
        "src/replication.c",
        "src/rio.c",
        "src/scripting.c",
        "src/sds.c",
        "src/sentinel.c",
        "src/server.c",
        "src/setcpuaffinity.c",
        "src/setproctitle.c",
        "src/sha1.c",
        "src/sha256.c",
        "src/siphash.c",
        "src/slowlog.c",
        "src/sort.c",
        "src/sparkline.c",
        "src/syncio.c",
        "src/t_hash.c",
        "src/t_list.c",
        "src/t_set.c",
        "src/t_stream.c",
        "src/t_string.c",
        "src/t_zset.c",
        "src/timeout.c",
        "src/tls.c",
        "src/tracking.c",
        "src/util.c",
        "src/ziplist.c",
        "src/zipmap.c",
        "src/zmalloc.c",
    }, &.{
        "-std=c11",
        "-pedantic",
        "-Wall",
        "-W",
        "-Wno-missing-field-initializers",
        "-fno-sanitize=undefined",
    });
}