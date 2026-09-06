using ACL.PersonACL;
using ACL.PersonACL.Implementations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {

            var applicationAssembly = typeof(IAssemblyMarker).Assembly;

            services.AddScoped<IPersonACL, PersonACL>();

            services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));

            services.AddValidatorsFromAssemblies(new[] { applicationAssembly });


            services.AddMediatR(configure =>
            {
                configure.RegisterServicesFromAssembly(applicationAssembly);
            });


            return services;
        }
    }
}
