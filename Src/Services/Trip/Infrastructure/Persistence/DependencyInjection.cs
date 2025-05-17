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
        public static IServiceCollection ConfigureInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {

            var applicationAssembly = typeof(IAssemblyMarker).Assembly;

            var mongoClient = new MongoClient(configuration.GetConnectionString("TripConnection"));
            var db = TripDbContext.Create(mongoClient.GetDatabase("Product"));
            db.Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;

            db.Database.EnsureCreated();
            services.AddScoped<ITripRepository, TripRepository>();





            return services;
        }
    }
}
