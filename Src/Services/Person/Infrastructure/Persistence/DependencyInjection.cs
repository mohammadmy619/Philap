using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Persons.Leader;
using Domain.Persons.Passenger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositores;
using Persistence.Repositories;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {

            var applicationAssembly = typeof(IAssemblyMarker).Assembly;


        
            services.AddDbContext<PersonDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<ILeaderRepository, LeaderRepository>();
            services.AddScoped<IPassengerRepository, PassengerRepository>();

            return services;
        }
    }
}
