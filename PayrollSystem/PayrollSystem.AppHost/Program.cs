var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.PayrollSystem_ApiService>("apiservice");

builder.AddProject<Projects.PayrollSystem_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
