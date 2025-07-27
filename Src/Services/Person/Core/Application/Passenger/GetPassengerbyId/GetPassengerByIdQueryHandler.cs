using Domain.Persons.Passenger;
using MediatR;

public class GetPassengerByIdQueryHandler(IPassengerRepository _passengerRepository)
    : IRequestHandler<GetPassengerByIdQuery, GetPassengerByIdResponse>
{
    public async Task<GetPassengerByIdResponse> Handle(GetPassengerByIdQuery request, CancellationToken ct)
    {
        var passenger = await _passengerRepository.GetPassengerByIdAsync(request.Id, ct);

        if (passenger == null)
            throw new KeyNotFoundException($"مسافری با شناسه {request.Id} یافت نشد.");

        return new GetPassengerByIdResponse(
            Id: passenger.Id,
            TripIds: passenger.TripIds,
            Name: passenger.Name,
            LastName: passenger.LastName,
            Email: passenger.Email,
            PhoneNumber: passenger.PhoneNumber,
            DateOfBirth: passenger.DateOfBirth,
            Gender: passenger.Gender,
            Street: passenger.Address.Street,
            City: passenger.Address.City,
            State: passenger.Address.State,
            ZipCode: passenger.Address.ZipCode,
            Nationality: passenger.Nationality,
            PassportNumber: passenger.PassportNumber,
            FrequentFlyerNumbers: passenger.FrequentFlyerNumbers);
    }
}