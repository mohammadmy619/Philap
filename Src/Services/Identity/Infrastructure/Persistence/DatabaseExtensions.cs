using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Persistence.Extensions;

public static class MigrationExtensions
{
    public static IApplicationBuilder ApplyMigrations<TContext>(this IApplicationBuilder app) where TContext : DbContext
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

        try
        {
            // مستقیماً مایگریت کنید؛ EF Core خودش دیتابیس و جدول هیستوری را می‌سازد
            dbContext.Database.Migrate();
            Console.WriteLine($"--> [Database] Migrations applied successfully for {typeof(TContext).Name}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> [Database Error] Failed to apply migrations for {typeof(TContext).Name}: {ex.Message}");
            throw;
        }

        return app;
    }
}
