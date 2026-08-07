using Domain.DiscountAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class DiscountEfConfig : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.HasKey(d => d.Id);

            // کانفیگ کد تخفیف
            builder.Property(d => d.Code)
                   .HasMaxLength(100)
                   .IsRequired(); // اگر در دامین اختیاری است، می‌توانید IsRequired(false) بگذارید

            // ایندکس یکتا برای کد تخفیف تا کد تکراری ثبت نشود
            builder.HasIndex(d => d.Code).IsUnique();

            // تبدیل Enum به String
            builder.Property(d => d.Type)
                   .HasConversion(
                       v => v.ToString(),
                       v => (DiscountType)Enum.Parse(typeof(DiscountType), v))
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(d => d.Value)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            // فیلدهای اختیاری (Nullable)
            builder.Property(d => d.ValidFrom);
            builder.Property(d => d.ValidTo);
            builder.Property(d => d.MaxUsageCount);
            builder.Property(d => d.ApplicableTripId);
            builder.Property(d => d.ApplicablePassengerId);
            builder.Property(d => d.LastUsedAt);

            builder.Property(d => d.UsedCount)
                   .IsRequired();

            builder.Property(d => d.IsActive)
                   .IsRequired();

            builder.Property(d => d.CreatedAt)
                   .IsRequired();

            // ارتباط یک به چند با DiscountUsage
            builder.HasMany(d => d.Usages)
                   .WithOne() // چون در DiscountUsage navigation property به سمت Discount نداریم
                   .HasForeignKey(u => u.DiscountId)
                   .OnDelete(DeleteBehavior.Cascade); // با حذف Discount، کاربردهای آن هم حذف می‌شوند

            // ایندکس برای بهبود کوئری‌ها
            builder.HasIndex(d => d.ApplicableTripId);
            builder.HasIndex(d => d.ApplicablePassengerId);
            builder.HasIndex(d => d.IsActive);
        }
    }
}