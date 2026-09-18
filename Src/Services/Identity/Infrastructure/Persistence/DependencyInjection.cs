using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.PermissionAgregate;
using Domain.RoleAgregate;
using Domain.Services;
using Domain.UserAgregate;
using Infrastructure.Services.Externals;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigurePersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationAssembly = typeof(IAssemblyMarker).Assembly;

            services.Configure<EmailSettings>(options =>
            {
                options.From = configuration.GetSection("EmailSettings:UserId").Value;
                options.SmtpPort = int.Parse(configuration.GetSection("EmailSettings:SmtpPort").Value ?? "587");
            });

            // 1. خواندن کانکشن استرینگ با اولویت Environment Variable سپس Configuration
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__postgresdb")
                                   ?? Environment.GetEnvironmentVariable("ConnectionStrings:postgresdb")
                                   ?? configuration.GetConnectionString("postgresdb")
                                   ?? configuration.GetConnectionString("DefaultConnection");

            Console.WriteLine(connectionString);

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("PostgreSQL connection string was not found in environment variables or configuration.");
            }

            // 2. Register IdentityDbContext with Npgsql (PostgreSQL)
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IPermissonValidationService, PermissonValidationService>();
            //services.AddScoped<IRoleValidationService, RoleValidationService>();

            return services;
        }
    }
}
