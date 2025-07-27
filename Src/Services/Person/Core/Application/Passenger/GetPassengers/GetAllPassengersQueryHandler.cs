using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Domain.Persons.Passenger;

namespace Application.Passenger.GetPassengers
{
    public class GetAllPassengersQueryHandler(IPassengerRepository _passengerRepository)
     : IRequestHandler<GetAllPassengersQuery, IReadOnlyCollection<GetAllPassengersResponse>>
    {
        public async Task<IReadOnlyCollection<GetAllPassengersResponse>> Handle(GetAllPassengersQuery request, CancellationToken ct)
        {
            var passengers = await _passengerRepository.GetAllPassengersAsync(
                request.PageNumber,
                request.PageSize,
                ct);

            // تبدیل Passenger به GetAllPassengersResponse
            var passengerDtos = passengers.Select(p => new GetAllPassengersResponse(
                Id: p.Id,
                TripIds: p.TripIds.ToList(),
                Name: p.Name,
                LastName: p.LastName,
                Email: p.Email,
                PhoneNumber: p.PhoneNumber,
                DateOfBirth: p.DateOfBirth,
                Gender: p.Gender,
                Street: p.Address.Street,
                City: p.Address.City,
                State: p.Address.State,
                ZipCode: p.Address.ZipCode,
                Nationality: p.Nationality,
                PassportNumber: p.PassportNumber,
                FrequentFlyerNumbers: p.FrequentFlyerNumbers.ToList(),
                Count: passengers.Count
            )).ToList();

            return passengerDtos;
        }
    }
}
