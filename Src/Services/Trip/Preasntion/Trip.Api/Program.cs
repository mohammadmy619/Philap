using Application;
using Infrastructure;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using Persistence;
using Persistence.Settings;
using Scalar.AspNetCore;
using Trip.Api;

var builder = WebApplication.CreateBuilder(args);

builder.AddMongoDBClient(connectionName: "mongodb");
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddServiceDiscoveryCore();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.ConfigureApplicationLayer(builder.Configuration);
builder.Services.ConfigurePersistenceLayer(builder.Configuration);
builder.Services.ConfigureInfrastructureLayer(builder.Configuration);

// ✅ ۱. اصلاح تنظیمات OpenAPI با DocumentTransformer
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        // آدرس را بر اساس پورتی که با آن صفحه را باز می‌کنید یا مستقیماً 7082 ست می‌کنیم
        document.Servers = new List<OpenApiServer>
        {
            new OpenApiServer { Url = "https://localhost:7082" }
        };
        return Task.CompletedTask;
    });
});

builder.Services.ConfigureCors();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // ✅ ۲. تنظیمات تمیز Scalar
    app.MapScalarApiReference(opt =>
    {
        opt.Title = "Trip API";
        opt.Theme = ScalarTheme.BluePlanet;
        opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
    });
}

// ✅ ۳. ترتیب استاندارد خط لوله (Pipeline)
app.UseHttpsRedirection();
app.UseRouting();

// اگر در ConfigureCors سیاستی تعریف کردید اینجا آن را اعمال کنید:
// app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program() { }
