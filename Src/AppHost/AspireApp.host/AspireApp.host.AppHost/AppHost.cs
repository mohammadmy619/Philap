using k8s.Models;

var builder = DistributedApplication.CreateBuilder(args);





var Identity_Api = builder.AddProject<Projects.Identity_Api>("identity-api")
    .WithHttpHealthCheck("/health");


var Person_Api = builder.AddProject<Projects.Person_Api>("person-api")
    .WithHttpHealthCheck("/health");

var Trip_Api = builder.AddProject<Projects.Trip_Api>("trip-api").WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health").WithReference(Person_Api)
    .WaitFor(Person_Api);

var Ticketing_Api = builder.AddProject<Projects.Ticketing_Api>("ticketing-api")
    .WithHttpHealthCheck("/health");

var Ocelot_ApiGateways = builder.AddProject<Projects.Ocelot_ApiGateways>("ocelot-apigateways");


builder.Configuration["DcpPublisher:RandomizePorts"] = "false";

builder.Build().Run();
//.WithHttpEndpoint(name: "main", port: 7082, isProxied: false)

//var apiService = builder.AddProject<Projects.AspireApp_host_ApiService>("apiservice")
//    .WithHttpHealthCheck("/health");

//builder.AddProject<Projects.AspireApp_host_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithHttpHealthCheck("/health")
//    .WithReference(apiService)
//    .WaitFor(apiService);


