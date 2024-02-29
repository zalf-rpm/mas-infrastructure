const std = @import("std");

pub fn build(b: *std.build.Builder) void {
    const target = b.standardTargetOptions(.{});
    const optimize = b.standardOptimizeOption(.{});

    const test_y = b.addExecutable(.{
        .name = "test_y",
        .target = target,
        .optimize = optimize,
    });
    test_y.addIncludePath(.{
        .cwd_relative = "C:/Users/berg.ZALF-AD/GitHub/vcpkg/installed/x64-windows-static/include",
    });
    test_y.addLibraryPath(.{.cwd_relative = "C:/Users/berg.ZALF-AD/GitHub/vcpkg/installed/x64-windows-static/lib",});
    test_y.linkSystemLibrary("kj");

    test_y.addCSourceFiles(&.{"test_some_main.cpp"}, &.{});

    test_y.linkLibC();
    test_y.linkLibCpp();

    b.default_step.dependOn(&test_y.step);
    b.installArtifact(test_y);
}