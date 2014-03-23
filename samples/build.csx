var build = Require<Build>();

var config = build.GetParameterOrDefault("config", "Release");

var baseDir = build.CurrentDirectory();
var srcDir = baseDir + "src";
var toolsDir = baseDir + "tools";

build.Target("Clean")
	.Do(() => 
	{
		build.Log("Cleaning the solution.");
	});

build.Target("GenerateAssemblyInfo")
	.DependsOn("Clean")
	.PreCondition(() => false)
	.Do(() =>
	{
		build.Exec("git", "status");
	});

build.Target("Build")
	.DependsOn("GenerateAssemblyInfo", "Clean")
	.Do(() =>
	{
		build.Log("Building the solution in '{0}' mode.", config);

		var projects = srcDir.Glob("**\\*.csproj");
		projects.Each(p => build.Log("Building '{0}'.", p));
	});

build.RunTargetOrDefault("Build");