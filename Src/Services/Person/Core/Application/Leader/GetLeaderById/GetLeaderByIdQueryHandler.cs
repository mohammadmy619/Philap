using Application.Leader.GetLeaderById;
using Domain.Persons.Leader;
using MediatR;
using static Application.Leader.GetLeaderById.GetLeaderByIdResponse;

public class GetLeaderByIdQueryHandler(ILeaderRepository _leaderRepository) : IRequestHandler<GetLeaderByIdQuery, GetLeaderByIdResponse>
{
 

    public async Task<GetLeaderByIdResponse> Handle(GetLeaderByIdQuery request, CancellationToken ct)
    {
        var leader = await _leaderRepository.GetLeaderByIdAsync(request.Id, ct);

        if (leader == null)
            throw new KeyNotFoundException($"Leader with ID {request.Id} not found.");

        return new GetLeaderByIdResponse(
            Id: leader.Id,
            TripIds: leader.TripIds,
            Name: leader.Name,
            LastName: leader.LastName,
            Email: leader.Email,
            PhoneNumber: leader.PhoneNumber,
            DateOfBirth: leader.DateOfBirth,
            leader.Gender,
            Address: new AddressDto(
                Street: leader.Address.Street,
                City: leader.Address.City,
                State: leader.Address.State,
                ZipCode: leader.Address.ZipCode),
            Nationality: leader.Nationality,
            Title: leader.Title,
            Department: leader.Department,
            JoiningDate: leader.JoiningDate,
            Skills: leader.Skills,
            Bio: leader.Bio);
    }
}
