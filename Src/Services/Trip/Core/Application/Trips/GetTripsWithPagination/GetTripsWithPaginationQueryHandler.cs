using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Pagination;
using Application.Trips.GetTrip;
using Domain.TripAggregate;
using MediatR;

namespace Application.Trips.GetTripsWithPagination
{
    public class GetTripsWithPaginationQueryHandler(ITripRepository _tripRepository)
      : IRequestHandler<GetTripsWithPaginationQuery, IReadOnlyCollection<GetTripsResponse>>
    {
        private readonly ITripRepository _tripRepository;

      
        public async Task<IReadOnlyCollection<GetTripsResponse>> Handle(
            GetTripsWithPaginationQuery request,
            CancellationToken cancellationToken)
        {
            var trips = await _tripRepository.GetTripsWithPaginationAsync(
              request.PageNumber,
                request.PageSize,
                cancellationToken);

            var responseItems = trips.Select(trip => new GetTripsResponse(
                trip.Tripid,
                trip.LeaderId,
                trip.TravelStartDate,
                trip.TravelEndDate,
                trip.LocationName,
                trip.Price.price,
                trip.TripStatus
            )).ToList();

            return responseItems;
        }
    }
}
