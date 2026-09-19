using Application;
using Identity.Api;
using Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Extensions;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();


builder.Configuration.AddEnvironmentVariables();

builder.Services.ConfigurePersistenceLayer(builder.Configuration);
builder.Services.ConfigureInfrastructureLayer(builder.Configuration);
builder.Services.ConfigureApplicationLayer(builder.Configuration);

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {

        document.Servers = new List<OpenApiServer>
        {
            new OpenApiServer { Url = "https://localhost:7106" }
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

        //document.Security ??= [];
        //document.Security.Add(new OpenApiSecurityRequirement
        //{
        //    [new OpenApiSecuritySchemeReference("BearerAuth", document)] = []
        //});

        return Task.CompletedTask;
    });
});
//builder.Services.ConfigureCors();
builder.Services.AddCors(o =>
{
    o.AddPolicy("Scalar", p => p
        .WithOrigins("https://localhost:7261") // origin گیت‌وی
        .AllowAnyHeader()
        .AllowAnyMethod());
});


builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var keyBytes = Encoding.UTF8.GetBytes(
jwtSettings["SecretKey"] ?? throw new InvalidOperationException("Jwt SecretKey is missing in configuration."));

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
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

var app = builder.Build();


  app.ApplyMigrations<IdentityDbContext>();


app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
   
    app.MapOpenApi();

    app.MapScalarApiReference(opt =>
    {
        opt.Title = "Identity";
        opt.Theme = ScalarTheme.Purple;
        opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
        opt.AddPreferredSecuritySchemes(["BearerAuth"]);
        opt.EnablePersistentAuthentication();
    });

app.UseCors("Scalar");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
