using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class IdentityDbContextContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            var optionBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
            optionBuilder.UseNpgsql("");

            return new IdentityDbContext(optionBuilder.Options);
        }
    }
}
