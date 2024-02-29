const std = @import("std");

pub fn build(b: *std.build.Builder) void {
    const target = b.standardTargetOptions(.{});
    const optimize = b.standardOptimizeOption(.{});

    const test_y = b.addExecutable(.{
        .name = "test_y",
        .target = target,
        .optimize = optimize,
    });
    test_y.linkLibCpp();
    test_y.addCSourceFiles(&.{
        "test_some_main.cpp"
    }, &.{
    });
    b.default_step.dependOn(&test_y.step);
    b.installArtifact(test_y);

    return;
    
    const test_x = b.addExecutable(.{
        .name = "test_x",
        .target = target,
        .optimize = optimize,
    });
    //const test_x = b.addStaticLibrary(.{
    //    .name = "test_x",
    //    .target = target,
    //    .optimize = optimize,
    //});
    test_x.linkLibC();
    test_x.linkLibCpp();
    //test_x.force_pic = true;
    test_x.addIncludePath(.{
        .cwd_relative = "C:/Users/berg.ZALF-AD/GitHub/vcpkg/installed/x64-windows-static/include",
    });
    const sourceFiles = [_][]const u8{
        "test_a_main.cpp",
        "../../../capnproto_schemas/gen/cpp/test/a.capnp.cpp",
        "../../../capnproto_schemas/gen/cpp/test/x.capnp.cpp",
        //"-Iraylib/src",
        //"-Iraylib/src/external/glfw/include",
        //"-Iraylib/src/external/glfw/deps"
    };
    const capnpFlags = [_][]const u8{
        "-I../../../capnproto_schemas/gen/cpp/test",
        //"-std=c99",
        //"-Iraylib/src",
        //"-Iraylib/src/external/glfw/include",
        //"-Iraylib/src/external/glfw/deps"
    };
    test_x.addCSourceFiles(&sourceFiles, &capnpFlags);
    test_x.addLibraryPath(.{
        .cwd_relative = "C:/Users/berg.ZALF-AD/GitHub/vcpkg/installed/x64-windows-static/lib",
    });
    test_x.linkSystemLibrary("kj");
    test_x.linkSystemLibrary("kj-async");
    test_x.linkSystemLibrary("capnp");
    //test_x.linkSystemLibrary("capnpc");
    test_x.linkSystemLibrary("capnp-rpc");
    //test_x.linkSystemLibrary("kj-http");
    //test_x.linkSystemLibrary("kj-gzip");
    //test_x.linkSystemLibrary("capnp-json");

     b.default_step.dependOn(&test_x.step);

    b.installArtifact(test_x);

    //const play = b.step("selfie", "Play Selfie Invaders");
    //const run = b.addRunArtifact(test_x);
    //play.dependOn(&run.step);
}