using Domain.TripAggregate;
using MediatR;

namespace Application.Trips.CreateTrip
{
    public record CreateTripCommand(
    Guid LeaderId,
    DateTime TravelStartDate,
    DateTime TravelEndDate,
    string LocationName,
    decimal PriceAmount,
    string PriceCurrency,
    TripStatus TripStatus) : IRequest<CreateTripResponse>;
}
