using BuildingBlocks.Exeptions;
using Domain.TripAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Trips.GetTrip
{
    public class GetTripHandler(ITripRepository _TripRepository) : IRequestHandler<GetTripQuery, GetTripResponse>
    {
         async Task<GetTripResponse> IRequestHandler<GetTripQuery, GetTripResponse>.Handle(GetTripQuery request, CancellationToken cancellationToken)
        {
           

            
            var trip = await _TripRepository.GetTripByIdAsync(request.TripID, cancellationToken);

            if (trip == null)
                throw new NotFoundException("Trip Not Found", "0814023");

           
            return new GetTripResponse(
                         trip.Id,
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
