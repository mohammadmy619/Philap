//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using System;

//namespace Persistence
//{
//    public class IdentityDbContextContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
//    {
//        public IdentityDbContext CreateDbContext(string[] args)
//        {
//             ۱. خواندن مقدار مستقیم از Environment Variable
//            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__postgresdb")
//                                   ?? Environment.GetEnvironmentVariable("ConnectionStrings:postgresdb")
//                                   ?? Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");

//             ۲. اگر پیدا نشد، پرتاب Exception شفاف
//            if (string.IsNullOrWhiteSpace(connectionString))
//            {
//                throw new InvalidOperationException(
//                    "Connection string was not found in environment variable 'ConnectionStrings__postgresdb'.");
//            }

//            var optionBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
//            optionBuilder.UseNpgsql(connectionString);

//            return new IdentityDbContext(optionBuilder.Options);
//        }
//    }
//}
