using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Application
{
    public static class DependencyInjection
    {

        public static IServiceCollection ConfigureApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {

            var applicationAssembly = typeof(IAssemblyMarker).Assembly;

            services.AddValidatorsFromAssemblies(new[] { applicationAssembly });


            services.AddMediatR(configure =>
            {
                configure.RegisterServicesFromAssembly(applicationAssembly);
            });


            return services;
        }
    }
}
