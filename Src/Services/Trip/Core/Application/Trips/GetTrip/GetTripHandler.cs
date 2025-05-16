using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.TripAggregate;
using MediatR;

namespace Application.Trips.GetTrip
{
    public class GetTripHandler(ITripRepository _TripRepository) : IRequestHandler<GetTripQuery, GetTripResponse>
    {
         async Task<GetTripResponse> IRequestHandler<GetTripQuery, GetTripResponse>.Handle(GetTripQuery request, CancellationToken cancellationToken)
        {
           

            
            var trip = await _TripRepository.GetTripByIdAsync(request.TripID, cancellationToken);

            if (trip == null)
                throw new GetTripNotFoundException();

           
            return new GetTripResponse(
                         trip.Tripid,
                         trip.LeaderId,
                         trip.TravelStartDate,
                         trip.TravelEndDate,
                         trip.LocationName,
                         trip.Price.Amount,
                         trip.Price.Currency,
                         trip.TripStatus);
        }
    }
}
