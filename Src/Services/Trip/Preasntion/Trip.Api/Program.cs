using Application;
using Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Settings;
using Scalar.AspNetCore;
using System.Text;
using Trip.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddMongoDBClient(connectionName: "mongodb");
builder.AddServiceDefaults();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "کلید_پیش‌فرض_یا_ارور";
var keyBytes = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),

            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],

            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };
    });




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
        document.Components ??= new OpenApiComponents();
        //document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["BearerAuth"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "توکن JWT را بدون پیشوند Bearer وارد کنید."
        };


        return Task.CompletedTask;
    });
});

builder.Services.ConfigureCors();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(o =>
{
    o.AddPolicy("Scalar", p => p
        .WithOrigins("https://localhost:7261") // origin گیت‌وی
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();



app.MapDefaultEndpoints();

    app.MapOpenApi();

    // ✅ ۲. تنظیمات تمیز Scalar
    app.MapScalarApiReference(opt =>
    {
        opt.Title = "Trip API";
        opt.Theme = ScalarTheme.BluePlanet;
        opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
        opt.DarkMode = true;
        opt.AddPreferredSecuritySchemes(["BearerAuth"]);
        opt.EnablePersistentAuthentication();
    });


// ✅ ۳. ترتیب استاندارد خط لوله (Pipeline)
app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("Scalar");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/scalar"));

app.Run();

public partial class Program() { }
