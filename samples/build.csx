var build = Require<Build>();

var config = build.GetParameterOrDefault("config", "Release");

var baseDir = build.CurrentDirectory();
var srcDir = baseDir + "src";
var toolsDir = baseDir + "tools";
var artifactsDir = baseDir + "artifacts";
var binDir = artifactsDir + "bin";

build.Target("Clean")
	.Do(() => 
	{
		build.DeleteDirectory(artifactsDir);
		build.Log("Cleaning the solution.");
	});

build.Target("GitStatus")
	.SkipIf(() => true)
	.Do(() =>
	{
		build.Exec("git", "status");
	});

build.Target("Build")
	.DependsOn("GitStatus", "Clean")
	.Before(() => build.Log("First"))
	.Do(() =>
	{
		build.CreateDirectory(binDir);
		build.Log("Building the solution in '{0}' mode.", config);
	})
	.After(() => build.Log("Fourth."));

build.RunTargetOrDefault("Build");