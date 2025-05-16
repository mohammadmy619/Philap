using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.TripAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class TripEfConfig : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
                   .ValueGeneratedOnAdd(); // Guid جدید در حین insert در دیتابیس تولید شود

            builder.Property(t => t.LocationName)
                   .IsRequired()
                   .HasMaxLength(100);

            // LeaderId یک foreign key است، اگر رابطه داشته باشد
            builder.Property(t => t.LeaderId)
                   .IsRequired();

            builder.Property(t => t.TravelStartDate)
                   .IsRequired();

            builder.Property(t => t.TravelEndDate)
                   .IsRequired();

            // Price یک Value Object است، بنابراین باید با Owned Types کانفیگ شود
            builder.OwnsOne(t => t.Price, pb =>
            {
                pb.Property(p => p.Amount).IsRequired();
                pb.Property(p => p.Currency).IsRequired().HasMaxLength(3);
            });

            // TripStatus یک enum است، می‌توان آن را بصورت زیر کانفیگ کرد
            builder.Property(t => t.TripStatus)
                   .HasConversion(
                       v => v.ToString(),
                       v => (TripStatus)Enum.Parse(typeof(TripStatus), v))
                   .HasMaxLength(50)
                   .IsRequired();
        }
    }
}
