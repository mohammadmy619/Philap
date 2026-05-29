using BuildingBlocks.Exeptions;
using Domain.Persons;
using Domain.Persons.Passenger;
using MediatR;

namespace Application.Passenger.UpdatePassenger;
public class UpdatePassengerCommandHandler(IPassengerRepository _passengerRepository)
    : IRequestHandler<UpdatePassengerCommand, Unit>
{


    public async Task<Unit> Handle(UpdatePassengerCommand request, CancellationToken ct)
    {
        var passenger = await _passengerRepository.GetPassengerByIdAsync(request.Id, ct);

        if (passenger == null)
            throw new NotFoundException($"Passenger with ID {request.Id} not found.");

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
            isActive:request.isActive,
            passportNumber: request.PassportNumber,
            frequentFlyerNumbers: request.FrequentFlyerNumbers);

        await _passengerRepository.SaveChangesAsync(ct);

        return Unit.Value;
    }
}