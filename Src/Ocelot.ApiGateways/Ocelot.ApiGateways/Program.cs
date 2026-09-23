using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.ApiGateway.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.ServiceDiscovery;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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

ServiceDiscoveryFinderDelegate finder = (provider, config, route) =>
    new AspireServiceDiscoveryProvider(provider, config, route);

builder.Services.AddSingleton(finder);




// 1. تنظیمات Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddControllers();


//builder.Services.AddOpenApi();
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



//builder.Services.AddOpenApi(options =>
//{
//    options.AddDocumentTransformer((document, context, cancellationToken) =>
//    {
//        // 1. تنظیم URL سرور گیت‌وی
//        document.Servers = new List<OpenApiServer>
//        {
//            new OpenApiServer { Url = "https://localhost:7261" }
//        };

//        // 2. ساخت کامپوننت‌ها در صورت خالی بودن
//        document.Components ??= new OpenApiComponents();
//        document.Components.SecuritySchemes ??= new Dictionary<string, OpenApiSecurityScheme>();

//        // 3. تعریف SecurityScheme برای Bearer JWT
//        var bearerScheme = new OpenApiSecurityScheme
//        {
//            Name = "Authorization",
//            Type = SecuritySchemeType.Http,
//            Scheme = "bearer",
//            BearerFormat = "JWT",
//            In = ParameterLocation.Header,
//            Description = "توکن JWT را وارد کنید (پیشوند Bearer به صورت خودکار اعمال می‌شود)."
//        };

//        document.Components.SecuritySchemes["BearerAuth"] = bearerScheme;

//        // 4. اعمال الزام احراز هویت برای تمام اندپوینت‌ها در اسناد (Security Requirement)
//        document.SecurityRequirements ??= new List<OpenApiSecurityRequirement>();
//        document.SecurityRequirements.Add(new OpenApiSecurityRequirement
//        {
//            {
//                new OpenApiSecurityScheme
//                {
//                    Reference = new OpenApiReference
//                    {
//                        Type = ReferenceType.SecurityScheme,
//                        Id = "BearerAuth"
//                    }
//                },
//                Array.Empty<string>()
//            }
//        });

//        return Task.CompletedTask;
//    });
//});

builder.Services.AddHttpClient();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApi(); // مسیر پیش‌فرض: /openapi/v1.json


//app.MapGet(
//    "/openapi/identity/v1.json",
//    async (IHttpClientFactory httpClientFactory, CancellationToken cancellationToken) =>
//    {
//        var client = httpClientFactory.CreateClient();

//        using var response = await client.GetAsync(
//            "https://localhost:7106/openapi/v1.json",
//            cancellationToken);

//        var document = await response.Content.ReadAsStringAsync(cancellationToken);

//        return Results.Content(
//            document,
//            response.Content.Headers.ContentType?.ToString()
//                ?? "application/json",
//            statusCode: (int)response.StatusCode);
//    });


app.MapScalarApiReference("/scalar", options =>
{
    options.AddDocument("Identity", "Identity API", "https://localhost:7106/openapi/v1.json");
    options.AddDocument("trip", "trip API", "https://localhost:7082/openapi/v1.json");
    options.AddDocument("Person", "Person API", "https://localhost:7211/openapi/v1.json");
    options.AddDocument("Ticketing", "Ticketing API", "https://localhost:7283/openapi/v1.json");

    options.Title = "ApiGateWay";
    options.Theme = ScalarTheme.DeepSpace;
    options.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
    options.AddPreferredSecuritySchemes(["BearerAuth"]);
    options.EnablePersistentAuthentication();

});

app.MapGet("/", () => Results.Redirect("/scalar"));

// Ocelot فقط برای مسیرهایی غیر از اینها اجرا شود
app.UseWhen(
    ctx =>
        !ctx.Request.Path.StartsWithSegments("/scalar") &&
        !ctx.Request.Path.StartsWithSegments("/openapi") &&
        !ctx.Request.Path.StartsWithSegments("/health") &&
        ctx.Request.Path != "/",
    branch =>
    {
        branch.UseOcelot().Wait();
    });
//app.MapControllers();

app.Run();
