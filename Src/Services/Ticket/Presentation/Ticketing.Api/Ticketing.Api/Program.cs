using Application;
using Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.ConfigurePersistenceLayer(builder.Configuration);
builder.Services.ConfigureApplicationLayer(builder.Configuration);
builder.Services.ConfigureInfrastructureLayer(builder.Configuration);


builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Servers = new List<OpenApiServer>
        {
            new OpenApiServer { Url = "https://localhost:7283" }
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
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(o =>
{
    o.AddPolicy("Scalar", p => p
        .WithOrigins("https://localhost:7261") // origin گیت‌وی
        .AllowAnyHeader()
        .AllowAnyMethod());
});

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
var app = builder.Build();

app.MapDefaultEndpoints();



    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.Title = "Ticketing";
        opt.Theme = ScalarTheme.BluePlanet;
        opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
        opt.DarkMode=true;
        opt.AddPreferredSecuritySchemes(["BearerAuth"]);
        opt.EnablePersistentAuthentication();
    });


app.UseHttpsRedirection();
app.ApplyMigrations();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/scalar"));

app.UseCors("Scalar");
app.Run();
