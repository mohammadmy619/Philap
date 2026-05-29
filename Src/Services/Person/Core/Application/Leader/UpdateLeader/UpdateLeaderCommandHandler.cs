using Application.Leader.UpdateLeader;
using Domain.Persons;
using Domain.Persons.Leader;
using MediatR;

public class UpdateLeaderCommandHandler(ILeaderRepository _leaderRepository) : IRequestHandler<UpdateLeaderCommand>
{

  
   async Task  IRequestHandler<UpdateLeaderCommand>.Handle(UpdateLeaderCommand request, CancellationToken cancellationToken)
    {
        var leader = await _leaderRepository.GetLeaderByIdAsync(request.LeaderId, cancellationToken);


        var address = new Address(
            street: request.Street,
            city: request.City,
            state: request.State,
            zipCode: request.ZipCode);

        leader.Update(
            tripIds: request.TripIds,
            Name: request.Name,
            LastName: request.LastName,
            Email: request.Email,
            PhoneNumber: request.PhoneNumber,
            DateOfBirth: request.DateOfBirth,
            Gender: request.Gender,
            Address: address,
            Nationality: request.Nationality,
            isActive:request.isActive,
            Title: request.Title,
            department: request.Department,
            newJoiningDate: request.JoiningDate,
            newSkills: request.Skills,
            newBio: request.Bio);

        await _leaderRepository.UpdateLeaderAsync(leader, cancellationToken);
        await _leaderRepository.SaveChangesAsync(cancellationToken);

        
    }
}
