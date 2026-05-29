using Application;
using Infrastructure;
using Persistence;
using Scalar.AspNetCore;
using Trip.Api;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddServiceDiscoveryCore();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<NotFoundExceptionFilter>();
});



builder.Services.ConfigureApplicationLayer(builder.Configuration);

builder.Services.ConfigurePersistenceLayer(builder.Configuration);

builder.Services.ConfigureInfrastructureLayer(builder.Configuration);




builder.Services.AddOpenApi();


builder.Services.ConfigureCors();
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{


    app.MapOpenApi();
    //app.MapScalarApiReference();

    app.MapScalarApiReference(opt =>
        {
            
            opt.Title = "Trip";
            opt.Theme = ScalarTheme.BluePlanet;
            opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
        });
}
app.UseRouting(); 
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



public partial class Program() { }
