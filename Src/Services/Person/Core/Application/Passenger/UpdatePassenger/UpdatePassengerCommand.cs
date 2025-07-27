using Domain.Persons;
using MediatR;

namespace Application.Passenger.UpdatePassenger;
public record class UpdatePassengerCommand(
    Guid Id,
    List<Guid>? TripIds,
    string Name,
    string LastName,
    string Email,
    string PhoneNumber,
    DateTime DateOfBirth,
    Gender Gender,
    string Street,
    string City,
    string State,
    string ZipCode,
    string Nationality,
    string PassportNumber,
    List<string> FrequentFlyerNumbers) : IRequest<Unit>;