using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class DiscountUsageEfConfig : IEntityTypeConfiguration<DiscountUsage>
    {
        public void Configure(EntityTypeBuilder<DiscountUsage> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.DiscountId)
                   .IsRequired();

            // فیلدهای اختیاری
            builder.Property(u => u.BookingId);
            builder.Property(u => u.PassengerId);
            builder.Property(u => u.TripId);

            builder.Property(u => u.AppliedAmount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(u => u.UsedAt)
                   .IsRequired();

            // ایندکس‌ها برای جستجوی سریع‌تر تاریخچه استفاده‌ها
            builder.HasIndex(u => u.DiscountId);
            builder.HasIndex(u => u.BookingId);
            builder.HasIndex(u => u.PassengerId);
            builder.HasIndex(u => u.TripId);

            // ایندکس ترکیبی برای جلوگیری از ثبت تکراری یا گزارش‌گیری سریع
            builder.HasIndex(u => new { u.DiscountId, u.BookingId });
        }
    }
}