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

builder.Services.ConfigureInfrastructureLayer(builder.Configuration);
builder.Services.ConfigureApplicationLayer(builder.Configuration);

builder.Services.AddOpenApi();
builder.Services.ConfigureCors();


builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

builder.Services.AddAuthentication()
    .AddJwtBearer("Bearer", options =>
    {
        // ??? Authority ??? Identity Server ???? ???
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),

            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,


            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,

            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = true,
        };
        options.RequireHttpsMetadata = false; // ??? ???? ?????
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
