
var builder = DistributedApplication.CreateBuilder(args);
var centralServer = builder.AddProject<Projects.BlazeOrbital_CentralServer>("centralserver");
var manufacturing = builder.AddProject<Projects.BlazeOrbital_ManufacturingHub>("manufacturing")
        .WithReference(centralServer);

builder.AddNpmApp("missioncontrol", "/Users/achananantachot/Downloads/BlazeOrbital/MissionControl")
        .WithEndpoint(port: 44423, scheme: "https", env: "PORT")
        .WithReference(centralServer)
        .WaitFor(manufacturing)
        .WithEnvironment("BROWSER", "none") 
        .WithEnvironment("HTTPS", "true")
        .WithEnvironment("SSL_CRT_FILE", "server.cert")
        .WithEnvironment("SSL_KEY_FILE", "server.key");
        
builder.Build().Run();
