using ACL.PersonACL;
using Infrastructure.ExternalServices.ACLImplementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CheckLeaderValided;

namespace Infrastructure
{
    public static class DependencyInjection
    {

        public static IServiceCollection ConfigureInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationAssembly = typeof(IAssemblyMarker).Assembly;


            //Console.WriteLine("https+http://Person.Api");

            services.AddGrpcClient<CheckLeaderValid.CheckLeaderValidClient> (o =>
            {
           
                o.Address = new Uri("https://localhost:7211"); 
            });

            services.AddScoped<IPersonACL, PersonGrpcAcl>();

            return services;

        }
    }
}
