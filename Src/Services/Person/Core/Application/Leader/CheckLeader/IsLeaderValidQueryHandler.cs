using Domain.Persons.Leader;
using MediatR;

namespace Application.Features.Leaders.Handlers.Queries
{
    public class IsLeaderValidQueryHandler(ILeaderRepository leaderRepository)
        : IRequestHandler<IsLeaderValidQuery, bool>
    {
        public async Task<bool> Handle(IsLeaderValidQuery request, CancellationToken ct)
        {
           
            var leader = await leaderRepository.GetLeaderByIdAsync(request.LeaderId, ct);

            if (leader is null)
                return false;

           
            return leader.IsActive; 
        }
    }
}