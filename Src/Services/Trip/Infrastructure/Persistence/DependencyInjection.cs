using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.TripAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Persistence.Repositories;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigurePersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
               var applicationAssembly = typeof(IAssemblyMarker).Assembly;


            services.AddDbContext<TripDbContext>(options =>
                options.UseMongoDB(
                    configuration.GetConnectionString("TripConnection"),
                    "Trips"));

            services.AddScoped<ITripRepository, TripRepository>();

            return services;

           

      


        }
    }
}
