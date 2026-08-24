using Domain.TripAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Persistence.Repositories;
using Persistence.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class DependencyInjection
    {



        public static IServiceCollection ConfigurePersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // استفاده از overload ای که به serviceProvider دسترسی دارد
            services.AddDbContext<TripDbContext>((serviceProvider, options) =>
            {
                // دریافت تنظیمات از IOptions که در Program.cs ثبت شده است
                var settings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;

                options.UseMongoDB(
                    settings.ConnectionString,
                    settings.DatabaseName);
            });

            services.AddScoped<ITripRepository, TripRepository>();

            return services;
        }
 
    }
}
