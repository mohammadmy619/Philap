using Domain.PermissionAgregate;
using Domain.RoleAgregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{


    public class PermissionEfConfig : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            // تنظیم Id به عنوان کلید اصلی  
            builder.HasKey(p => p.Id);

            // تنظیم نام به عنوان یک فیلد الزامی و مشخص کردن حداکثر طول  
            builder.Property(p => p.Name)
                .IsRequired() // الزامی بودن نام  
                .HasMaxLength(100); // مقدار حداکثر طول نام  


            // کانفیگ AccessControl به عنوان یک Owned Collection (Value Object)
            builder.OwnsMany(p => p.AccessControl, ac =>
            {
                // نام جدولی که در دیتابیس برای این Value Objectها ساخته می‌شود
                ac.ToTable("PermissionAccessControls");

                // نام کلید خارجی که به Permission اشاره می‌کند
                ac.WithOwner().HasForeignKey("PermissionId");

                // پراپرتی‌ها
                ac.Property(a => a.Resource)
                  .IsRequired()
                  .HasMaxLength(150);

                ac.Property(a => a.Action)
                  .IsRequired()
                  .HasMaxLength(100);

                // اگر ستون Id در کلاس AccessControl ندارید، EF به صورت Shadow Property یک Id برای جدول می‌سازد:
                ac.Property<int>("Id");
                ac.HasKey("Id");
            });

        }
    }
}
