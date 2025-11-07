using Scalar.AspNetCore;
using Application;
using Persistence;
using Identity.Api;
using System.Text;
using Microsoft.IdentityModel.Tokens;



var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Configuration.AddEnvironmentVariables();

builder.Services.ConfigureInfrastructureLayer(builder.Configuration);
builder.Services.ConfigureApplicationLayer(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.ConfigureCors();
var key = Encoding.ASCII.GetBytes("THIS_IS_A_VERY_LONG_AND_SECURE_SECRET_KEY_1234567890"); 


builder.Services.AddAuthentication()
    .AddJwtBearer("Bearer", options =>
    {
        // ??? Authority ??? Identity Server ???? ???
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
        options.RequireHttpsMetadata = false; // ??? ???? ?????
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
