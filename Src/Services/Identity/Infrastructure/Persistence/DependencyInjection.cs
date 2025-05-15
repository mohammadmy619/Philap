using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public static IServiceCollection ConfigureApplicationLayer(this IServiceCollection services, IConfiguration configuration)
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


            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IPermissionRepository, PermissionRepository>();
            services.AddSingleton<IRoleRepository, RoleRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IPermissonValidationService, PermissonValidationService>();
            services.AddSingleton<IRoleValidationService, RoleValidationService>();


            return services;
        }
    }
}
