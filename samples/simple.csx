var build = Require<Build>();

var config = build.GetParameterOrDefault("config", "Release");

build.Target("Clean")
	.Do(() => 
	{
		build.Trace("Cleaning the solution.");
	});

build.Target("Build")
	.DependsOn("Clean")
	.Do(() =>
	{
		build.Trace("Building the solution in '{0}' mode.", config);
	});

build.RunTargetOrDefault("Build");