using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
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






// 1. تنظیمات Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddControllers();


builder.Services.AddOpenApi();
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

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApi(); // مسیر پیش‌فرض: /openapi/v1.json

//app.MapScalarApiReference(options =>
//{
//    options.Title = "API Gateway Documentation";
//    options.Theme = ScalarTheme.Purple;


//    options.WithOpenApiRoutePattern("/docs/{documentName}/openapi.json");

//    // مسیر OpenAPI خودِ Gateway یا روت‌های پروکسی شده
//    //options.WithOpenApiRoutePattern("/docs/{service}/openapi.json");

//    options.AddPreferredSecuritySchemes(["BearerAuth"]);
//    options.EnablePersistentAuthentication();
//});
app.MapControllers();

await app.UseOcelot();

app.Run();
