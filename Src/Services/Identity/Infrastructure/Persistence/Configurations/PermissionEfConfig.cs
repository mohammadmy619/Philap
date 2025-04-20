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

            //// تنظیم RoleIds به‌عنوان یک مجموعه، اگر نیاز به ارتباط داشته باشد  
            //builder.HasMany<Role>() // فرض بر این است که RoleIds به یک موجودیت Role مرتبط است  
            //    .WithMany() // یک رابطه چند به چند  
            //    .UsingEntity(j => j.ToTable("RolePermissions")); // نام جدول میانجی برای رابطه  
        }
    }
}
