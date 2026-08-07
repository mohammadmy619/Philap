using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.AccountingAggregate;
using Domain.BookingAggregate;
using Domain.DiscountAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigurePersistenceLayer(
            this IServiceCollection services,
            IConfiguration configuration)
        {


            services.AddDbContext<TicketingDbContext>(options =>
           options.UseSqlServer(
            configuration.GetConnectionString("TicketingConnection")));

            // ثبت DbContext با SQL Server
            //services.AddDbContext<TicketingDbContext>(options =>
            //    options.UseSqlServer(
            //        configuration.GetConnectionString("TicketingConnection"),
            //        sqlOptions =>
            //        {
            //            sqlOptions.MigrationsAssembly(typeof(TicketingDbContext).Assembly.FullName);
            //            sqlOptions.EnableRetryOnFailure(
            //                maxRetryCount: 3,
            //                maxRetryDelay: TimeSpan.FromSeconds(30),
            //                errorNumbersToAdd: null);
            //            sqlOptions.CommandTimeout(60);
            //        }));

            // ثبت Repository ها
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IAccountingRepository, AccountingRepository>();

            return services;
        }
    }
}