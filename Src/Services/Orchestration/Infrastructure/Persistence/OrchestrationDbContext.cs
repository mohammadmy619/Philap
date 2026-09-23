using Domain.StateData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class OrchestrationDbContext:DbContext
    {
        public OrchestrationDbContext(DbContextOptions<OrchestrationDbContext> options) : base(options)
        {
        }

        public DbSet<TicketStateData> TicketStateData { get; set; }
    }
}
