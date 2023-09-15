///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////
#addin nuget:?package=Cake.Docker&version=1.2.2

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
const string connectionString =  "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres";


///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////


Task("Info")
.Does(() => {
   var result = DockerComposePs(new DockerComposePsSettings());
        
    foreach(var service in result) {
        Information(service);        
    }
});

Task("Up")  
.Does(() => {
   DockerComposeUp(new DockerComposeUpSettings() {
       Detach=true,      
   });
     
   DotNetRun("Database/Gatekeeper.Migration", connectionString); 
});

Task("Down")
.Does(() => {
   DockerComposeDown(new () {
       Volumes=true
   });
});


Task("Default")
.IsDependentOn("Info");

RunTarget(target);
