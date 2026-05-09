var builder = DistributedApplication.CreateBuilder(args);





builder.AddProject<Projects.Ocelot_ApiGateways>("ocelot-apigateways");



builder.AddProject<Projects.Person_Api>("person-api").WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health");




builder.AddProject<Projects.Ticketing_Api>("ticketing-api").WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health");



builder.AddProject<Projects.Identity_Api>("identity-api").WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health");



builder.AddProject<Projects.Trip_Api>("trip-api").WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health");



//var apiService = builder.AddProject<Projects.AspireApp_host_ApiService>("apiservice")
//    .WithHttpHealthCheck("/health");

//builder.AddProject<Projects.AspireApp_host_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithHttpHealthCheck("/health")
//    .WithReference(apiService)
//    .WaitFor(apiService);

builder.Build().Run();
