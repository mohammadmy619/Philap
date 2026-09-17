using Application;
using Application.Utils;
using Identity.Api;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Scalar.AspNetCore;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();


builder.Configuration.AddEnvironmentVariables();

builder.Services.ConfigurePersistenceLayer(builder.Configuration);
builder.Services.ConfigureApplicationLayer(builder.Configuration);

builder.Services.AddOpenApi();
builder.Services.ConfigureCors();


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

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.Title = "Identity";
        opt.Theme = ScalarTheme.Purple;
        opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
