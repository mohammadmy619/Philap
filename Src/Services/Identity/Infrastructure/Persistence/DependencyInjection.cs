using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.services;
using Domain.Domain_Services;
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
        public static IServiceCollection ConfigureInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {

            var applicationAssembly = typeof(IAssemblyMarker).Assembly;

            services.Configure<EmailSettings>(options =>
            {
                options.From = configuration.GetSection("EmailSettings:UserId").Value;
                options.SmtpPort =int.Parse(configuration.GetSection("EmailSettings:SmtpPort").Value);

            });

            // 3. Register IdentityDbContext with Npgsql (PostgreSQL)
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });


            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPermissonValidationService, PermissonValidationService>();
            services.AddScoped<IRoleValidationService, RoleValidationService>();


            return services;
        }
    }
}
