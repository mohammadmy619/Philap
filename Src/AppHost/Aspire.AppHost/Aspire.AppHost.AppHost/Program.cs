var builder = DistributedApplication.CreateBuilder(args);

//var cache = builder.AddRedis("cache");

//var apiService = builder.AddProject<Projects.Aspire_AppHost_ApiService>("apiservice");

//builder.AddProject<Projects.Aspire_AppHost_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithReference(cache)
//    .WithReference(apiService);

builder.Build().Run();
