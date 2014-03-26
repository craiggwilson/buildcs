var build = Require<Build>();

var semVersion = "0.1.0";

var baseDir = build.CurrentDirectory();
var srcDir = baseDir + "src";
var artifactsDir = baseDir + "artifacts";
var binDir = artifactsDir + "bin";

var nuspecFiles = baseDir.Glob("*.nuspec");
var slnFile = srcDir + "BuildCs.sln";

build.Target("Clean").Do(() => build.DeleteDirectory(artifactsDir));

build.Target("AssemblyInfo")
    .Do(() =>
    {
        build.GenerateCSharpAssemblyInfo(srcDir + "GlobalAssemblyInfo.cs", args =>
        {
            args.AddAssemblyCopyrightAttribute("Copyright 2014 Craig Wilson");
            args.AddAssemblyVersionAttribute(new Version(0, 1, 0, 0)); //TODO: fix this
            args.AddAssemblyInformationalVersionAttribute(semVersion);
            args.AddAssemblyFileVersionAttribute(new Version(0, 1, 0, 0)); //TODO: fix this
            args.AddAssemblyConfigurationAttribute("Release");
        });
    });

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