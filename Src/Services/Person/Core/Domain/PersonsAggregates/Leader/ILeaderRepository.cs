using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Persons.Leader
{
    public interface ILeaderRepository
    {
        Task AddLeaderAsync(Leader leader,CancellationToken cancellationToken);  

        Task UpdateLeaderAsync(Leader leader, CancellationToken cancellationToken); 

        Task DeleteLeaderAsync(Guid leaderId, CancellationToken cancellationToken);  

        Task<Leader?> GetLeaderByIdAsync(Guid leaderId, CancellationToken cancellationToken); 
        Task<IReadOnlyCollection<Leader>> GetLeaders(int Page,int PageSize, CancellationToken cancellationToken); 

        Task<IEnumerable<Leader>> FindLeadersAsync(Expression<Func<Leader, bool>> predicate, CancellationToken cancellationToken);

        Task SaveChangesAsync(CancellationToken cancellationToken ); 
    }
}
