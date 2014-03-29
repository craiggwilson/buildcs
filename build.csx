using System.Xml.Linq;
using System.Xml.XPath;

var build = Require<Build>();

var semVersion = "0.1.0";

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

build.Target("AssemblyInfo")
    .Do(() =>
    {
        build.GenerateCSharpAssemblyInfo(assemblyInfoFile, args =>
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
        build.GitExec(baseDir, "checkout {0}".F(assemblyInfoFile));
    });

build.Target("NugetPack")
    .DependsOn("Build")
    .Do(() =>
    {
        // add dependency versions to all elements with BuildCs in them.
        nuspecFiles.Each(f => 
        {
            var doc = XDocument.Load(f);
            doc.XPathSelectElements("/package/metadata/dependencies/dependency")
                .Where(e => e.Attributes("id").Single().Value.Contains("BuildCs"))
                .Each(e => e.SetAttributeValue("version", semVersion));
            doc.Save(f);
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

build.RunTargetOrDefault("NugetPack");