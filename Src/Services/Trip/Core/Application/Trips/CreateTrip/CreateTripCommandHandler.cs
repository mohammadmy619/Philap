using ACL.PersonACL;
using BuildingBlocks.Exeptions;
using Domain.TripAggregate;
using MediatR;

namespace Application.Trips.CreateTrip
{
    public class CreateTripCommandHandler(ITripRepository _tripRepository, IPersonACL _personACL) : IRequestHandler<CreateTripCommand, CreateTripResponse>
    {


        public async Task<CreateTripResponse> Handle(CreateTripCommand request, CancellationToken cancellationToken)
        {
            var checkValidet = await _personACL.IsLeaderValidAsync(request.LeaderId, cancellationToken);

            if (!checkValidet)
            {
                throw new NotFoundException("LeaderId is not valid");
            }

            var trip = new Trip(
                leaderId: request.LeaderId,
                travelStartDate: request.TravelStartDate,
                travelEndDate: request.TravelEndDate,
                locationName: request.LocationName,
                tripStatus: request.TripStatus,
                price: new Price(request.PriceAmount, request.PriceCurrency));

            await _tripRepository.AddTripAsync(trip, cancellationToken);
            await _tripRepository.SaveChangesAsync(cancellationToken);

            return new CreateTripResponse(trip.Id);
        }


    }
}
