using Application;
using Infrastructure;
using Infrastructure.ExternalServices.ACLImplementation.GrpcServices;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        // آدرس را بر اساس پورتی که با آن صفحه را باز می‌کنید یا مستقیماً 7082 ست می‌کنیم
        document.Servers = new List<OpenApiServer>
        {
            new OpenApiServer { Url = "https://localhost:7211" }
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
builder.Services.AddControllers(options =>
{
    options.Filters.Add<NotFoundExceptionFilter>();
});
builder.Services.ConfigurePersistenceLayer(builder.Configuration);
builder.Services.ConfigureApplicationLayer(builder.Configuration);
builder.Services.ConfigureInfrastructureLayer(builder.Configuration);

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

builder.Services.AddGrpcReflection();

var app = builder.Build();

app.MapDefaultEndpoints();


    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.Title = "Person";
        opt.Theme = ScalarTheme.Moon;
        opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
        opt.DarkMode = true;
        opt.AddPreferredSecuritySchemes(["BearerAuth"]);
        opt.EnablePersistentAuthentication();
    });

app.MapGrpcService<CheckLeaderValidService>();
app.MapGrpcReflectionService();
app.UseHttpsRedirection();
app.ApplyMigrations();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors("Scalar");

app.Run();
