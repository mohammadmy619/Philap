using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Persons.Leader
{
    public interface ILeaderRepository
    {
        Task AddLeaderAsync(Leader leader);  

        Task UpdateLeaderAsync(Leader leader); 

        Task DeleteLeaderAsync(Guid leaderId);  

        Task<Leader?> GetLeaderByIdAsync(Guid leaderId); 

        Task<IEnumerable<Leader>> GetAllLeadersAsync(); 

        Task<IEnumerable<Leader>> FindLeadersAsync(Func<Leader, bool> predicate);

        Task SaveChangesAsync(); 
    }
}
