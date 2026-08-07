using Domain.AccountingAggregate;
using Domain.BookingAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class AccountingEfConfig : IEntityTypeConfiguration<Accounting>
    {
        public void Configure(EntityTypeBuilder<Accounting> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.BookingId)
                   .IsRequired();

            builder.Property(a => a.TripId)
                   .IsRequired();

            builder.Property(a => a.PassengerId)
                   .IsRequired();

            builder.Property(a => a.EntryDate)
                   .IsRequired();

            builder.Property(a => a.PurchaseDate)
                   .IsRequired();

            // Description اختیاری است (nullable)
            builder.Property(a => a.Description)
                   .HasMaxLength(500)
                   .IsRequired(false);

            // Price یک Value Object است، بنابراین با Owned Types کانفیگ می‌شود
            builder.OwnsOne(a => a.Price, pb =>
            {
                pb.Property(p => p.Amount).IsRequired();
                pb.Property(p => p.DiscountAmount).IsRequired();
                pb.Property(p => p.FinalAmount).IsRequired();
                pb.Property(p => p.Currency).IsRequired().HasMaxLength(3);
            });

            // PaymentStatus یک enum است، بصورت string ذخیره می‌شود
            builder.Property(a => a.PaymentStatus)
                   .HasConversion(
                       v => v.ToString(),
                       v => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), v))
                   .HasMaxLength(50)
                   .IsRequired();

            // ایندکس‌ها برای بهبود عملکرد کوئری‌ها
            builder.HasIndex(a => a.BookingId);
            builder.HasIndex(a => a.TripId);
            builder.HasIndex(a => a.PassengerId);

            // ایندکس ترکیبی برای گزارش‌گیری (مثلاً صورتحساب یک passenger در یک trip)
            builder.HasIndex(a => new { a.PassengerId, a.TripId });
        }
    }
}