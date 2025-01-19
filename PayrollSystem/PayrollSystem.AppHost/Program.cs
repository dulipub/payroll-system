var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("Username", "postgres", secret: true);
var password = builder.AddParameter("Password", "Admin123", secret: true);

var postgres = builder.AddPostgres("postgres", username, password).WithPgAdmin();
var postgresdb = postgres.AddDatabase("postgresdb");

var apiService = builder.AddProject<Projects.PayrollSystem_ApiService>("apiservice").WithReference(postgresdb);

builder.AddProject<Projects.PayrollSystem_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
