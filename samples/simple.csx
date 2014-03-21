var build = Require<Build>();

var config = build.GetParameterOrDefault("config", "Release");

build.Target("Clean")
	.Do(() => 
	{
		build.Log("Cleaning the solution.");
	});

build.Target("Build")
	.DependsOn("Clean")
	.Do(() =>
	{
		build.Log("Building the solution in '{0}' mode.", config);
		build.Exec("git", "status");
	});

build.RunTargetOrDefault("Build");