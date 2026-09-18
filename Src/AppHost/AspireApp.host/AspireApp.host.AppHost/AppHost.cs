var builder = DistributedApplication.CreateBuilder(args);


//Aspire.Hosting.MongoDB

var postgres = builder.AddPostgres("postgres").WithPgWeb(pgWeb => pgWeb.WithHostPort(5050));
var postgresdb = postgres.AddDatabase("postgresdb");



//var username = builder.AddParameter("Admin");

//var password = builder.AddParameter("Admin", secret: true);

//var mongo = builder.AddMongoDB("mongo").WithLifetime(ContainerLifetime.Persistent);


//var mongodb = mongo.AddDatabase("Trip");


var Identity_Api = builder.AddProject<Projects.Identity_Api>("identity-api")
    .WaitFor(postgresdb)
    .WithReference(postgresdb)
    .WithHttpHealthCheck("/health");


//var Person_Api = builder.AddProject<Projects.Person_Api>("person-api")
//    .WithHttpHealthCheck("/health");



//var Trip_Api = builder.AddProject<Projects.Trip_Api>("trip-api").WithExternalHttpEndpoints()
//    .WithHttpHealthCheck("/health")
//    //.WithReference(Person_Api)
//    //.WaitFor(Person_Api)
//    .WithReference(mongodb)
//    .WaitFor(mongodb);



//var Ticketing_Api = builder.AddProject<Projects.Ticketing_Api>("ticketing-api")
//    .WithHttpHealthCheck("/health");

var Ocelot_ApiGateways = builder.AddProject<Projects.Ocelot_ApiGateways>("ocelot-apigateways")
        .WaitFor(Identity_Api)
    .WithReference(Identity_Api);


//builder.Configuration["DcpPublisher:RandomizePorts"] = "false";

//builder.AddProject<Projects.Orchestration_Api>("orchestration-api");

builder.Build().Run();


