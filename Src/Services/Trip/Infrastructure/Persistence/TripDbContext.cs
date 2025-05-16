using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.TripAggregate;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Persistence
{
    public class TripDbContext : DbContext
    {
        public TripDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Trip> Trip { get; set; }

        public static TripDbContext Create(IMongoDatabase database) =>
                new(new DbContextOptionsBuilder<TripDbContext>()
        .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
        .Options);
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //modelBuilder
            //    .ApplyConfigurationsFromAssembly(typeof(TripDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Trip>().ToCollection("Trip");
        }




    }
}
