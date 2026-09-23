using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Orchestration.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigurePersistenceLayer(
            this IServiceCollection services,
            IConfiguration configuration)
        {


            services.AddDbContext<OrchestrationDbContext>(options =>
           options.UseSqlServer(
            configuration.GetConnectionString("OrchestrationConnection")));

      

            return services;
        }

        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<OrchestrationDbContext>();

            try
            {
                var pendingMigrations = dbContext.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                {
                    dbContext.Database.Migrate();
                    Console.WriteLine("--> [Database] Migrations applied successfully & tables created.");
                }
                else
                {
                    Console.WriteLine("--> [Database] Database is already up to date.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> [Database Error] Failed to apply migrations: {ex.Message}");
                throw;
            }

            return app;
        }


    }
}