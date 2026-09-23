using Orchestration.Infrastructure;
using Orchestration.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();


builder.Services.ConfigurePersistenceLayer(builder.Configuration);

builder.Services.ConfigureInfrastructureLayer(builder.Configuration);


builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapOpenApi();
app.ApplyMigrations();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();
