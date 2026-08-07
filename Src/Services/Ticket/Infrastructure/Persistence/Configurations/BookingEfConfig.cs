using Domain.BookingAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class BookingEfConfig : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.TripId)
                   .IsRequired();

            builder.Property(b => b.PassengerId)
                   .IsRequired();

            builder.Property(x => x.DiscountId)
                   .IsRequired(false);

            builder.Property(b => b.PurchaseDate)
                   .IsRequired();

            // Price یک Value Object است، بنابراین باید با Owned Types کانفیگ شود
            builder.OwnsOne(b => b.Price, pb =>
            {
                pb.Property(p => p.Amount).IsRequired();
                pb.Property(p => p.DiscountAmount).IsRequired();
                pb.Property(p => p.FinalAmount).IsRequired();
                pb.Property(p => p.Currency).IsRequired().HasMaxLength(3);
            });

            // BookingStatus یک enum است، بصورت string ذخیره می‌شود
            builder.Property(b => b.Status)
                   .HasConversion(
                       v => v.ToString(),
                       v => (BookingStatus)Enum.Parse(typeof(BookingStatus), v))
                   .HasMaxLength(50)
                   .IsRequired();

            // ایندکس برای بهبود عملکرد کوئری‌ها روی TripId و PassengerId
            builder.HasIndex(b => b.TripId);
            builder.HasIndex(b => b.PassengerId);
        }
    }
}