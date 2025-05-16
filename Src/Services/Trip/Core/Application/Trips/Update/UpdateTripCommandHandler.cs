using Domain.TripAggregate;
using MediatR;

public class UpdateTripCommandHandler(ITripRepository _tripRepository) : IRequestHandler<UpdateTripCommand>
{
 

   
   async Task<Unit> IRequestHandler<UpdateTripCommand, Unit>.Handle(UpdateTripCommand request, CancellationToken cancellationToken)
    {
      

        // دریافت سفر فعلی
        var trip = await _tripRepository.GetTripByIdAsync(request.TripId, cancellationToken);

        if (trip == null)
            throw new GetTripNotFoundException();

        trip.UpdateTrip(request.TripId,request.LeaderId,request.TravelStartDate,request.TravelEndDate,request.LocationName,request.TripStatus,new  Price(request.PriceAmount, request.PriceCurrency));
  

        await _tripRepository.UpdateTripAsync(trip, cancellationToken);
        await _tripRepository.SaveChangesAsync( cancellationToken);

        return Unit.Value;

    }


}