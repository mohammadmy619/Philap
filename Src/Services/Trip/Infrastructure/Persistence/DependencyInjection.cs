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


            // ✅ ثبت DbContext در DI (حیاتی برای رفع خطای AggregateException)
            services.AddDbContext<TripDbContext>(options =>
                options.UseMongoDB(
                    configuration.GetConnectionString("TripConnection"),
                    "Product"));

            // ✅ ثبت Repository (DbContext حالا خودکار Inject می‌شود)
            services.AddScoped<ITripRepository, TripRepository>();

            return services;

           

            //    var mongoClient = new MongoClient(configuration.GetConnectionString("TripConnection"));
            //    var db = TripDbContext.Create(mongoClient.GetDatabase("Product"));
            //    db.Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;

            //    db.Database.EnsureCreated();
            //    services.AddScoped<ITripRepository, TripRepository>();





            //    return services;
            //}


        }
    }
}
