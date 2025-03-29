using Domain.RoleAgregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{
    public class RoleEfConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // تنظیم Id به عنوان کلید اصلی  
            builder.HasKey(r => r.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();



            // تنظیم نام به عنوان یک فیلد الزامی و مشخص کردن حداکثر طول  
            builder.Property(r => r.Name)
                .IsRequired() // الزامی بودن نام  
                .HasMaxLength(100); // مقدار حداکثر طول  

        }
    }
}
