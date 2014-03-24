using BuildCs.MsBuild;
using BuildCs.Nuget;

var build = Require<Build>();

var config = build.GetParameterOrDefault("config", "Release");
var semVersion = "0.1.0-alpha";

var baseDir = build.CurrentDirectory();
var srcDir = baseDir + "src";
var toolsDir = baseDir + "tools";
var artifactsDir = baseDir + "artifacts";
var binDir = artifactsDir + "bin";

var nuspecFile = baseDir + "buildcs-sample.nuspec";

build.Target("Clean")
	.Do(() => build.DeleteDirectory(artifactsDir));

build.Target("Build")
	.DependsOn("Clean")
	.Do(() =>
	{
		build.CreateDirectory(binDir);
		build.NugetRestorePackages();
		build.MsBuild(srcDir.Glob("**/*.csproj"), args => 
		{
			args.AddProperty("OutputPath", binDir);
			args.AddTarget("Build");
			args.Verbosity = MsBuildVerbosity.Quiet;
		});
	});

build.Target("NugetPack")
	.DependsOn("Build")
	.Do(() =>
	{
		build.NugetPack(nuspecFile, args =>
		{
			args.Verbosity = NugetVerbosity.Quiet;
			args.OutputDirectory = artifactsDir;
			args.Symbols = true;
			args.Version = semVersion;
		});
	});

build.RunTargetOrDefault("NugetPack");