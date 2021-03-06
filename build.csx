var build = Require<Build>();

var semVersion = build.SemVer("0.1.1");
var buildNumber = int.Parse(build.GetEnvironmentVariableOrDefault("BUILD_NUMBER", "0"));

var baseDir = build.CurrentDirectory();
var srcDir = baseDir + "src";
var artifactsDir = baseDir + "artifacts";
var binDir = artifactsDir + "bin";

var assemblyInfoFile = srcDir + "GlobalAssemblyInfo.cs";
var nuspecFiles = srcDir.Glob("**/*.nuspec");
var slnFile = srcDir + "BuildCs.sln";

build.Parameters[XmlListener.OutputPathParameterName] = artifactsDir + "build-results.xml";

build.Target("Clean")
    .Do(() => 
    {
        build.DeleteDirectory(artifactsDir);
    });

build.Target("Build")
    .DependsOn("Clean")
    .Do(() =>
    {
        build.NugetRestorePackages();
        
        build.GenerateCSharpAssemblyInfo(assemblyInfoFile, args =>
        {
            args.Attributes.Copyright("Copyright 2014 Craig Wilson");
            args.Attributes.Version(semVersion.ToVersion(buildNumber));
            args.Attributes.InformationalVersion(semVersion);
            args.Attributes.FileVersion(semVersion.ToVersion(buildNumber));
            args.Attributes.Configuration("Release");
        });

        build.MsBuild(slnFile, args =>
        {
            args.AddProperty("OutputPath", binDir);
            args.AddProperty("Configuration", "Release");
            args.NoLogo = true;
            args.Verbosity = MsBuildVerbosity.Quiet;
        });

        build.GitExec(baseDir, "checkout {0}".F(assemblyInfoFile));
    });

build.Target("Test")
    .DependsOn("Build")
    .Do(() =>
    {
        build.XUnit(binDir.Glob("*.Tests.dll"), args =>
        {
            args.NUnitXmlOutputPath = artifactsDir + "test-results.xml";
            args.Verbose = false;
        });
    });

build.Target("NugetPack")
    .DependsOn("Test")
    .Do(() =>
    {
        // add dependency versions to all elements with BuildCs in them.
        build.XmlUpdate(nuspecFiles, doc =>
        {
            doc.XPathSelectElements("/package/metadata/dependencies/dependency")
                .Where(e => e.Attributes("id").Single().Value.Contains("BuildCs"))
                .Each(e => e.SetAttributeValue("version", semVersion));
        });

        build.NugetPack(nuspecFiles, args =>
        {
            args.BasePath = baseDir;
            args.OutputDirectory = artifactsDir;
            args.Symbols = true;
            args.Version = semVersion;
        });

        // reset all the nuspec files...
        nuspecFiles.Each(f => build.GitExec(baseDir, "checkout {0}".F(f)));
    });

build.Target("NugetPush")
    .PreCondition(() => build.DirectoryExists(artifactsDir) && artifactsDir.Glob("*.nupkg").Count() == nuspecFiles.Count() * 2, "Nuget packages were not found.")
    .Do(() =>
    {
        build.NugetPush(artifactsDir.Glob("*.nupkg"), args =>
        {
            args.ApiKey = build.GetParameter("NugetApiKey");
        });
    });

build.RunTargetOrDefault("NugetPack");