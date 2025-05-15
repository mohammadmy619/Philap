using Domain.TripAggregate;
using MediatR;

namespace Application.Trips.CreateTrip
{
    public class CreateTripCommandHandler(ITripRepository _tripRepository) : IRequestHandler<CreateTripCommand, CreateTripResponse>
    {
       
  
        public async Task<CreateTripResponse> Handle(CreateTripCommand request, CancellationToken cancellationToken)
        {

            var trip = new Trip(
                leaderId: request.LeaderId,
                travelStartDate: request.TravelStartDate,
                travelEndDate: request.TravelEndDate,
                locationName: request.LocationName,
                tripStatus: request.TripStatus,
                price: new Price(request.PriceAmount));

            // ذخیره در Repository
            await _tripRepository.AddTripAsync(trip, cancellationToken);
            await _tripRepository.SaveChangesAsync(cancellationToken);

            return new CreateTripResponse(trip.Id); 
        }

  
    }
}
