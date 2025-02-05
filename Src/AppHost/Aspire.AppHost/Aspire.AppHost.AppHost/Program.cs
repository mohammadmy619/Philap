var builder = DistributedApplication.CreateBuilder(args);

//var cache = builder.AddRedis("cache");

//var apiService = builder.AddProject<Projects.Aspire_AppHost_ApiService>("apiservice");

//builder.AddProject<Projects.Aspire_AppHost_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithReference(cache)
//    .WithReference(apiService);

builder.AddProject<Projects.Ocelot_ApiGateways>("ocelot-apigateways");

//var cache = builder.AddRedis("cache");

//var apiService = builder.AddProject<Projects.Aspire_AppHost_ApiService>("apiservice");

//builder.AddProject<Projects.Aspire_AppHost_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithReference(cache)
//    .WithReference(apiService);

builder.AddProject<Projects.Shipment_Api>("shipment-api");

//var cache = builder.AddRedis("cache");

//var apiService = builder.AddProject<Projects.Aspire_AppHost_ApiService>("apiservice");

//builder.AddProject<Projects.Aspire_AppHost_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithReference(cache)
//    .WithReference(apiService);

builder.AddProject<Projects.Person_Api>("person-api");

//var cache = builder.AddRedis("cache");

//var apiService = builder.AddProject<Projects.Aspire_AppHost_ApiService>("apiservice");

//builder.AddProject<Projects.Aspire_AppHost_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithReference(cache)
//    .WithReference(apiService);

builder.AddProject<Projects.Accommadation_Api>("accommadation-api");

//var cache = builder.AddRedis("cache");

//var apiService = builder.AddProject<Projects.Aspire_AppHost_ApiService>("apiservice");

//builder.AddProject<Projects.Aspire_AppHost_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithReference(cache)
//    .WithReference(apiService);

builder.AddProject<Projects.Ticketing_Api>("ticketing-api");

//var cache = builder.AddRedis("cache");

//var apiService = builder.AddProject<Projects.Aspire_AppHost_ApiService>("apiservice");

//builder.AddProject<Projects.Aspire_AppHost_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithReference(cache)
//    .WithReference(apiService);

builder.AddProject<Projects.Authentication_Api>("authentication-api");

//var cache = builder.AddRedis("cache");

//var apiService = builder.AddProject<Projects.Aspire_AppHost_ApiService>("apiservice");

//builder.AddProject<Projects.Aspire_AppHost_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithReference(cache)
//    .WithReference(apiService);

builder.AddProject<Projects.Infrastructure>("infrastructure");

//var cache = builder.AddRedis("cache");

//var apiService = builder.AddProject<Projects.Aspire_AppHost_ApiService>("apiservice");

//builder.AddProject<Projects.Aspire_AppHost_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithReference(cache)
//    .WithReference(apiService);

builder.AddProject<Projects.Trip_Api>("trip-api");

//var cache = builder.AddRedis("cache");

//var apiService = builder.AddProject<Projects.Aspire_AppHost_ApiService>("apiservice");

//builder.AddProject<Projects.Aspire_AppHost_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithReference(cache)
//    .WithReference(apiService);

builder.Build().Run();
