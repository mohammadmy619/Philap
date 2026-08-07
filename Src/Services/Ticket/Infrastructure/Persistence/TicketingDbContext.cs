using Domain.AccountingAggregate;
using Domain.BookingAggregate;
using Domain.DiscountAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Reflection;



namespace Persistence
{
    public class TicketingDbContext:DbContext
    {

        public DbSet<Accounting> Accounting { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Discount> Discount { get; set; }
        public DbSet<DiscountUsage> DiscountUsage { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
