using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Persons.Leader;
using Domain.Persons.Passenger;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class PassengerEFConfig : IEntityTypeConfiguration<Passenger>
    {
        public void Configure(EntityTypeBuilder<Passenger> builder)
        {

            // --- کلید اصلی ---
            builder.Property(p => p.Id)
                .ValueGeneratedNever(); // چون Guid دستی ساخته می‌شه

            // --- فیلدهای Person ---
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(p => p.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(p => p.DateOfBirth)
                .IsRequired();

            builder.Property(p => p.Gender)
                .HasConversion<string>() // ذخیره enum به صورت "Male", "Female"
                .HasMaxLength(20);

            builder.Property(p => p.Nationality)
                .HasMaxLength(50);

            // --- TripIds: لیست Guid به صورت JSON ---
            builder.Property(p => p.TripIds)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<List<Guid>>(v, new JsonSerializerOptions()) ?? new List<Guid>())
                .HasColumnType("nvarchar(max)");

            // --- Address: Value Object ---
            builder.OwnsOne(p => p.Address, addrBuilder =>
            {
                addrBuilder.Property(a => a.Street)
                    .HasColumnName("Street")
                    .HasMaxLength(200)
                    .IsRequired();

                addrBuilder.Property(a => a.City)
                    .HasColumnName("City")
                    .HasMaxLength(100)
                    .IsRequired();

                addrBuilder.Property(a => a.ZipCode)
                    .HasColumnName("PostalCode")
                    .HasMaxLength(20);

                addrBuilder.Property(a => a.State)
                    .HasColumnName("Country")
                    .HasMaxLength(100)
                    .IsRequired();
            });

            // --- فیلدهای خاص Passenger ---
            builder.Property(p => p.PassportNumber)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("PassportNumber");

            // --- FrequentFlyerNumbers: لیست رشته به صورت JSON ---
            builder.Property(p => p.FrequentFlyerNumbers)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()) ?? new List<string>())
                .HasColumnType("nvarchar(max)");
        }
    }
}
