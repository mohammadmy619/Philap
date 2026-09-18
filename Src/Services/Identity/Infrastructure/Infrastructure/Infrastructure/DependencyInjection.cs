using Application.Commons.Interfaces;
using Domain.Services;
using Infrastructure.Services;
using Infrastructure.Services.Externals;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            services.AddScoped<IEmailService, EmailService>();

            services.AddHttpContextAccessor();

            // ۴. ثبت سرویس کاربر جاری
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddScoped<IJwtService, JwtTokenGenerator>();

            services.AddSingleton<ILinkGenerator, LinkGenerator>();


            return services;
        }
    }
}
