
var builder = DistributedApplication.CreateBuilder(args);
var centralServer = builder.AddProject<Projects.BlazeOrbital_CentralServer>("server");
builder.AddProject<Projects.BlazeOrbital_ManufacturingHub>("manufacturing")
        .WithReference(centralServer);

builder.Build().Run();
