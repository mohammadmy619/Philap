using Application.Leader.GetLeaders;
using Domain.Persons.Leader;
using MediatR;

public class GetLeadersQueryHandler(ILeaderRepository _leaderRepository) : IRequestHandler<GetLeadersQuery, GetLeadersResponse>
{
  

    public async Task<GetLeadersResponse> Handle(GetLeadersQuery request, CancellationToken ct)
    {
        var result = await _leaderRepository.GetLeaders(request.Page, request.PageSize, ct);

        var leaders = result.Select(l => new LeaderDto(
            Id: l.Id,
            Name: l.Name,
            LastName: l.LastName,
            Email: l.Email,
            DateOfBirth: l.DateOfBirth,
            Gender: l.Gender,
            Nationality: l.Nationality,
            Title: l.Title,
            Department: l.Department,
            JoiningDate: l.JoiningDate,
            SkillsCount: l.Skills.Count
           
        
        )).ToList();

        return new GetLeadersResponse(leaders, result.Count);
    }
}