using Domain.Persons.Leader;
using MediatR;

public class CreateLeaderCommandHandler : IRequestHandler<CreateLeaderCommand, CreateLeaderResponse>
{
    private readonly ILeaderRepository _leaderRepository;

    public CreateLeaderCommandHandler(ILeaderRepository leaderRepository)
    {
        _leaderRepository = leaderRepository;
    }

    public async Task<CreateLeaderResponse> Handle(CreateLeaderCommand request, CancellationToken ct)
    {
            var leader = new Leader(
                tripIds: request.TripIds,
                name: request.Name,
                lastName: request.LastName,
                email: request.Email,
                phoneNumber: request.PhoneNumber,
                dateOfBirth: request.DateOfBirth,
                gender: request.Gender,
                address: request.Address,
                nationality: request.Nationality,
                title: request.Title,
                department: request.Department,
                joiningDate: request.JoiningDate,
                skills: request.Skills,
                bio: request.Bio
            );

            await _leaderRepository.AddLeaderAsync(leader, ct);

            return CreateLeaderResponse.SuccessResponse(leader.Id);
      
       
    }
}
