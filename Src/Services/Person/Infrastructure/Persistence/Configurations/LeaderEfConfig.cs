using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Persons.Leader;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class LeaderEfConfig : IEntityTypeConfiguration<Leader>
    {
        public void Configure(EntityTypeBuilder<Leader> modelBuilder)
        {

            // --- تنظیم کلید اصلی ---
            modelBuilder
                .Property(p => p.Id)
                .ValueGeneratedNever(); // چون Guid.NewGuid() در کد می‌زنیم

            modelBuilder
            .Property(p => p.Name)
           .IsRequired()
            .HasMaxLength(100);

            modelBuilder
                .Property(p => p.LastName)
                .IsRequired()
            .HasMaxLength(100);

            modelBuilder
                .Property(p => p.Email)
                .IsRequired()
            .HasMaxLength(256);



            // --- تنظیم فیلدهای Leader ---
            modelBuilder
                .Property(l => l.Title)
                .IsRequired()
            .HasMaxLength(100);

            modelBuilder
                .Property(l => l.Department)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder
                .Property(l => l.JoiningDate)
            .IsRequired();

            modelBuilder
                .Property(l => l.Bio)
                .HasMaxLength(1000);

            modelBuilder.OwnsOne(p => p.Address, addrBuilder =>
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


        }
    }
}
