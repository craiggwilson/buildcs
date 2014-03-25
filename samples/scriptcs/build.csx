var build = Require<Build>();

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
			args.AddProperty("Configuration", build.GetParameterOrDefault("config", "Release"));
			args.AddTarget("Build");
			args.Verbosity = MsBuildVerbosity.Quiet;
		});
	});

build.Target("Test")
	.DependsOn("Build")
	.Do(() =>
	{
		var testAssemblies = binDir.Glob("**/*Tests.dll");

		build.NUnit(testAssemblies, args =>
		{
			args.NoLogo = true;
			args.ToolPath = baseDir + "../tools/NUnit/nunit-console.exe";
			args.XmlOutputPath = artifactsDir + "nunit-test-results.xml";
		});

		build.XUnit(testAssemblies, args =>
		{
			args.ToolPath = baseDir + "../tools/xunit/xunit.console.clr4.exe";
			args.XmlOutputPath = artifactsDir + "xunit-test-results.xml";
		});
	});

build.Target("Zip")
	.DependsOn("Build")
	.Do(() =>
	{
		build.Zip(artifactsDir + "Sample-{0}.zip".F(semVersion), args =>
		{
			args.AddItem("artifacts/bin/BuildCs.Sample.dll", ".");
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

build.RunTargetOrDefault("Build");