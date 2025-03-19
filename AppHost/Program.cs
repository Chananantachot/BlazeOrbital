using DotNetEnv;
var builder = DistributedApplication.CreateBuilder(args);
var centralServer = builder.AddProject<Projects.BlazeOrbital_CentralServer>("server");
builder.AddProject<Projects.BlazeOrbital_ManufacturingHub>("manufacturing")
        .WithReference(centralServer);

Env.Load();
builder.Build().Run();
