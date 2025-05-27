using Domain.Persons;
using Domain.Persons.Leader;
using MediatR;

public class CreateLeaderCommandHandler(ILeaderRepository _leaderRepository) : IRequestHandler<CreateLeaderCommand, CreateLeaderResponse>
{

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
                address: new Address(request.Street,request.City,request.State,request.ZipCode),
                nationality: request.Nationality,
                title: request.Title,
                department: request.Department,
                joiningDate: request.JoiningDate,
                skills: request.Skills,
                bio: request.Bio
            );

            await _leaderRepository.AddLeaderAsync(leader, ct);
            await _leaderRepository.SaveChangesAsync(ct);

            return new  CreateLeaderResponse(leader.Id);
      
       
    }
}
