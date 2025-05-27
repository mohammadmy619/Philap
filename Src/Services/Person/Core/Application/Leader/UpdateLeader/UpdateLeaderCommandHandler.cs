using Application.Leader.UpdateLeader;
using Domain.Persons;
using Domain.Persons.Leader;
using MediatR;

public class UpdateLeaderCommandHandler(ILeaderRepository _leaderRepository) : IRequestHandler<UpdateLeaderCommand>
{

  
   async Task  IRequestHandler<UpdateLeaderCommand>.Handle(UpdateLeaderCommand request, CancellationToken cancellationToken)
    {
        // 1. دریافت رهبر از پایگاه داده
        var leader = await _leaderRepository.GetLeaderByIdAsync(request.LeaderId, cancellationToken);


        // 2. ساخت Address از ورودی‌های Command
        var address = new Address(
            street: request.Street,
            city: request.City,
            state: request.State,
            zipCode: request.ZipCode);

        // 3. فراخوانی متود Update موجود در کلاس Leader
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
            Title: request.Title,
            Department: request.Department,
            newJoiningDate: request.JoiningDate,
            newSkills: request.Skills,
            newBio: request.Bio);

        await _leaderRepository.UpdateLeaderAsync(leader, cancellationToken);
        await _leaderRepository.SaveChangesAsync(cancellationToken);

        
    }
}
