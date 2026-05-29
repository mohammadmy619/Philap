using Application;
using Persistence;
using Person.Api.GrpcServer.CheckLeader;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.ConfigurePersistenceLayer(builder.Configuration);
builder.Services.ConfigureApplicationLayer(builder.Configuration);


var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.Title = "Person";
        opt.Theme = ScalarTheme.Moon;
        opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
    });
}
app.MapGrpcService<CheckLeaderValidService>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
