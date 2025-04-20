using Domain.PermissionAgregate;
using Domain.RoleAgregate;
using Domain.UserAgregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Persistence
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Permission> Permission { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Pars_db2; Integrated Security=True; TrustServerCertificate=Yes");
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
