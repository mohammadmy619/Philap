using Domain.TripAggregate;
using MediatR;

public record UpdateTripCommand(
    Guid TripId,
    Guid LeaderId,
    DateTime TravelStartDate,
    DateTime TravelEndDate,
    string LocationName,
    decimal PriceAmount,
    string PriceCurrency,
    TripStatus TripStatus) : IRequest;
