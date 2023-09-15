///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");


///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////
Task("Clean")
.WithCriteria(c => HasArgument("rebuild"))
.Does(() => {
        CleanDirectories($"**/bin/{configuration}");
});


Task("Build")
.IsDependentOn("Clean")
.Does(() => {
     DotNetBuild("./Gatekeeper.sln", new DotNetBuildSettings
       {
           Configuration = configuration,
       });
});

Task("Test")
.IsDependentOn("Clean")
.Does(() => {
    DotNetTest("./Gatekeeper.sln", new DotNetTestSettings
    {
        Configuration = configuration,
    });
});

Task("Default")
    .IsDependentOn("Build");

RunTarget(target);
