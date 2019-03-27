#load "nuget:?package=Cake.Storm.Fluent"
#load "nuget:?package=Cake.Storm.Fluent.DotNetCore"
#load "nuget:?package=Cake.Storm.Fluent.NuGet"

string version = Argument("version", "0.1.0");

Configure()
    .UseRootDirectory("..")
    .UseBuildDirectory("build")
    .UseArtifactsDirectory("artifacts")
    .AddConfiguration(c => c
        .WithSolution("Storm.Sql.sln")
        .WithTargetFrameworks("netstandard2.0")
        .WithBuildParameter("Configuration", "Release")
        .WithBuildParameter("Platform", "Any CPU")
        .UseDefaultTooling()
        .UseDotNetCoreTooling()
        .WithDotNetCoreOutputType(OutputType.Copy)
    )
    .AddPlatform("dotnet")
    .AddTarget("pack")
    .AddTarget("push", c => c
        .UseNugetPush(p => p.WithApiKeyFromEnvironment())
    )
    .AddApplication("stormsql", c => c
        .WithProjects(
            "src/ServiceStack.Common/ServiceStack.Common.csproj",
            "src/ServiceStack.Interfaces/ServiceStack.Interfaces.csproj",
            "src/ServiceStack.OrmLite/ServiceStack.OrmLite.csproj",
            "src/ServiceStack.OrmLite.MySql/ServiceStack.OrmLite.MySql.csproj",
            "src/ServiceStack.OrmLite.Sqlite/ServiceStack.OrmLite.Sqlite.csproj",
            "src/ServiceStack.OrmLite.SqlServer/ServiceStack.OrmLite.SqlServer.csproj",
            "src/ServiceStack.Text/ServiceStack.Text.csproj"
        )
        .WithVersion(version)
        .UseNugetPack(n => n
            .WithAuthor("Julien Mialon")
            .WithNuspec("misc/Storm.Sql.nuspec")
            .WithPackageId("Storm.Sql")
            .WithReleaseNotesFile("misc/Storm.Sql.md")
            .AddAllFilesFromArtifacts("lib")
        )
    )
    .Build();

RunTarget(Argument("target", "help"));