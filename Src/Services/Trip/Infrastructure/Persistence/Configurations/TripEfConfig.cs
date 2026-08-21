using System;
using System.Collections.Generic;
using System.Text.Json;
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


            builder.Property(t => t.LocationName)
                   .IsRequired();

            builder.Property(t => t.LeaderId)
                   .IsRequired();

            builder.Property(t => t.TravelStartDate)
                   .IsRequired();

            builder.Property(t => t.TravelEndDate)
                   .IsRequired();

            // Price - در MongoDB به صورت Nested Document ذخیره می‌شود
            builder.OwnsOne(t => t.Price, pb =>
            {
                pb.Property(p => p.Amount).IsRequired();
                pb.Property(p => p.Currency).IsRequired();
            });

            // TripStatus - به صورت string در MongoDB ذخیره می‌شود
            builder.Property(t => t.TripStatus)
                   .HasConversion<string>()
                   .IsRequired();

            // ✅ لیست TripIds - فقط HasConversion لازم است
            builder.Property<List<Guid>>("_ticketIds")
                   .HasConversion(
                       v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                       v => JsonSerializer.Deserialize<List<Guid>>(v, new JsonSerializerOptions()) ?? new List<Guid>());
        }
    }
}