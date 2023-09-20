#tool dotnet:?package=dotnet-coverage&version=17.8.6

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

Task("Clean")
.WithCriteria(c => HasArgument("rebuild"))
.Does(() => {
        CleanDirectories($"**/bin/");
        CleanDirectories($"**/obj/");
});


Task("Restore")
.IsDependentOn("Clean")
.Does(() => {
    DotNetRestore();
});

Task("Build")
.IsDependentOn("Restore")
.Does(() => {
     DotNetBuild("./Gatekeeper.sln", new DotNetBuildSettings
       {
           Configuration = configuration,
           
       });
});

var testSettings = new DotNetTestSettings {
    Configuration = configuration
};

void RunTestWithCoverage(string project, string suffixLabel)
{
    Command(
            new []{ "dotnet-coverage", "dotnet-coverage.exe"},
            settingsCustomization: settings => settings
                                                    .WithToolName(".NET coverage")
                                                    .WithExpectedExitCode(0)
                                                    .WithArgumentCustomization(args => 
                                                        args.Append("collect")                                                        
                                                            .AppendQuoted($"dotnet test -c {configuration} {project}")
                                                            .Append("-f xml")
                                                            .Append($"-o coverage.{suffixLabel}.xml") 
                                                    )
        );    

}

Task("Test/Unit")
.IsDependentOn("Build")
.Does(() => {    
    RunTestWithCoverage("./Core/Gatekeeper.Core.Test", "core");
});

Task("Test/Rest")
.IsDependentOn("Build")
.Does(() => {
    RunTestWithCoverage("./Rest/Gatekeeper.Rest.Test", "rest");           
});

Task("Coverage/Merge")  
.Does(() => {
   Command(
               new []{ "dotnet-coverage", "dotnet-coverage.exe"},
               settingsCustomization: settings => settings
                                                       .WithToolName(".NET coverage")
                                                       .WithExpectedExitCode(0)
                                                       .WithArgumentCustomization(args => 
                                                           args.Append("merge")                                                        
                                                               .Append("coverage.core.xml")
                                                               .Append("coverage.rest.xml")
                                                               .Append("-f xml")
                                                               .Append($"-o coverage.xml") 
                                                       )
           );     
});


Task("Default")
    .IsDependentOn("Build");

RunTarget(target);
