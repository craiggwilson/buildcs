var build = Require<Build>();

var semVersion = "0.1.0";

var baseDir = build.CurrentDirectory();
var srcDir = baseDir + "src";
var artifactsDir = baseDir + "artifacts";
var binDir = artifactsDir + "bin";

var nuspecFiles = baseDir.Glob("*.nuspec");
var slnFile = srcDir + "BuildCs.sln";

build.Target("Clean").Do(() => build.DeleteDirectory(artifactsDir));

build.Target("Build")
    .DependsOn("Clean")
    .Do(() =>
    {
        build.NugetRestorePackages();
        build.MsBuild(slnFile, args =>
        {
            args.AddProperty("OutputPath", binDir);
            args.AddProperty("Configuration", "Release");
            args.NoLogo = true;
            args.Verbosity = MsBuildVerbosity.Quiet;
        });
    });

build.Target("NugetPack")
    .DependsOn("Build")
    .Do(() =>
    {
        build.NugetPack(nuspecFiles, args =>
        {
            args.OutputDirectory = artifactsDir;
            args.Symbols = true;
            args.Version = semVersion;
        });
    });

build.RunTargetOrDefault("NugetPack");