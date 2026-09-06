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


            var connectionString = configuration.GetConnectionString("Trip")
    ?? throw new InvalidOperationException("ConnectionStrings:Trip is missing.");

            // پارس کردن Connection String برای استخراج DatabaseName
            var mongoUrl = new MongoUrl(connectionString);
            var databaseName = mongoUrl.DatabaseName ?? "Trip";

            Console.WriteLine($"MongoDB Connecting to: {mongoUrl.Server} | Database: {databaseName}");

            services.AddDbContext<TripDbContext>((serviceProvider, options) =>
            {
                // دریافت تنظیمات از IOptions که در Program.cs ثبت شده است
                //var settings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;

                options.UseMongoDB(
                    mongoUrl.Url,
                    databaseName);
            });

            services.AddScoped<ITripRepository, TripRepository>();

            return services;
        }
 
    }
}
