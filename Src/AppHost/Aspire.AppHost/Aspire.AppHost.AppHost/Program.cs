var builder = DistributedApplication.CreateBuilder(args);



builder.AddProject<Projects.Ocelot_ApiGateways>("ocelot-apigateways");

//var cache = builder.AddRedis("cache");

//var apiService = builder.AddProject<Projects.Aspire_AppHost_ApiService>("apiservice");

//builder.AddProject<Projects.Aspire_AppHost_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithReference(cache)
//    .WithReference(apiService);

builder.AddProject<Projects.Person_Api>("person-api");




builder.AddProject<Projects.Ticketing_Api>("ticketing-api");



builder.AddProject<Projects.Identity_Api>("Identity_Api-api");


builder.AddProject<Projects.Trip_Api>("trip-api");


builder.Build().Run();
