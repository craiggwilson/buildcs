using BuildCs.MsBuild;
using BuildCs.Nuget;

var build = Require<Build>();

var config = build.GetParameterOrDefault("config", "Release");

var baseDir = build.CurrentDirectory();
var srcDir = baseDir + "src";
var toolsDir = baseDir + "tools";
var artifactsDir = baseDir + "artifacts";
var binDir = artifactsDir + "bin";

build.Target("Clean")
	.Do(() => build.DeleteDirectory(artifactsDir));

build.Target("Build")
	.DependsOn("Clean")
	.Do(() =>
	{
		build.CreateDirectory(binDir);
		build.NugetRestorePackages();
		build.MsBuild(srcDir.Glob("**/*.csproj"), c => 
		{
			c.AddProperty("OutputPath", binDir);
			c.AddTarget("Build");
			c.Verbosity = MsBuildVerbosity.Quiet;
		});
	});

build.RunTargetOrDefault("Build");