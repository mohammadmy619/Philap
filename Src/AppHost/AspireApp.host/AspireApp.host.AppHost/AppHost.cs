var builder = DistributedApplication.CreateBuilder(args);



var rabbitmq = builder.AddRabbitMQ("messaging").WithManagementPlugin();


var postgres = builder.AddPostgres("postgres").WithPgWeb(pgWeb => pgWeb.WithHostPort(5050));
var postgresdb = postgres.AddDatabase("postgresdb");

var mongo = builder.AddMongoDB("mongo").WithLifetime(ContainerLifetime.Persistent);


var mongodb = mongo.AddDatabase("Trip");





var Identity_Api = builder.AddProject<Projects.Identity_Api>("identity-api")
    .WaitFor(postgresdb)
    .WithReference(postgresdb)
    .WithHttpHealthCheck("/health");


var Person_Api = builder.AddProject<Projects.Person_Api>("person-api")
    .WaitFor(Identity_Api)
    .WithReference(Identity_Api)
    .WaitFor(rabbitmq)
    .WithReference(rabbitmq)
    .WithHttpHealthCheck("/health");



var Trip_Api = builder.AddProject<Projects.Trip_Api>("trip-api").WithExternalHttpEndpoints()
    .WaitFor(Person_Api)
    .WithReference(Person_Api)
    .WaitFor(mongodb)
    .WithReference(mongodb)
    .WaitFor(rabbitmq)
    .WithReference(rabbitmq)
    .WithHttpHealthCheck("/health");



var Ticketing_Api = builder.AddProject<Projects.Ticketing_Api>("ticketing-api")
    .WaitFor(Identity_Api)
    .WithReference(Identity_Api)
    .WaitFor(Person_Api)
    .WithReference(Person_Api)
    .WaitFor(Trip_Api)
    .WithReference(Trip_Api)
    .WaitFor(rabbitmq)
    .WithReference(rabbitmq)
    .WithHttpHealthCheck("/health");


var Orchestration_Api = builder.AddProject<Projects.Orchestration_Api>("orchestration-api")
           .WaitFor(Identity_Api)
           .WithReference(Identity_Api)
           .WaitFor(Person_Api)
           .WithReference(Person_Api)
           .WaitFor(Trip_Api)
           .WithReference(Trip_Api)
           .WaitFor(Ticketing_Api)
           .WithReference(Ticketing_Api)
           .WithHttpHealthCheck("/health");
    
var Ocelot_ApiGateways = builder.AddProject<Projects.Ocelot_ApiGateway>("ocelot-apigateways")
    .WaitFor(Identity_Api)
     .WithReference(Identity_Api)
     .WaitFor(Person_Api)
     .WithReference(Person_Api)
    .WaitFor(Trip_Api)
     .WithReference(Trip_Api)
     .WaitFor(Ticketing_Api)
    .WithReference(Ticketing_Api)
    .WaitFor(Orchestration_Api)
    .WithReference(Orchestration_Api)
    .WithHttpHealthCheck("/health");





builder.Build().Run();


