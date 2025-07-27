using Domain.Persons.Passenger;
using Domain.Persons;
using MediatR;

namespace Application.Passenger.UpdatePassenger;
public class UpdatePassengerCommandHandler(IPassengerRepository _passengerRepository)
    : IRequestHandler<UpdatePassengerCommand, Unit>
{


    public async Task<Unit> Handle(UpdatePassengerCommand request, CancellationToken ct)
    {
        var passenger = await _passengerRepository.GetPassengerByIdAsync(request.Id, ct);

        if (passenger == null)
            throw new KeyNotFoundException($"Passenger with ID {request.Id} not found.");

        var address = new Address(
            request.Street,
            request.City,
            request.State,
            request.ZipCode);

        passenger.Update(
            tripIds: request.TripIds,
            name: request.Name,
            lastName: request.LastName,
            email: request.Email,
            phoneNumber: request.PhoneNumber,
            dateOfBirth: request.DateOfBirth,
            gender: request.Gender,
            address: address,
            nationality: request.Nationality,
            passportNumber: request.PassportNumber,
            frequentFlyerNumbers: request.FrequentFlyerNumbers);

        await _passengerRepository.SaveChangesAsync(ct);

        return Unit.Value;
    }
}