using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.TripAggregate;

namespace Application.Trips.GetTripsWithPagination
{
    public record GetTripsResponse(
      Guid TripId,
      Guid LeaderId,
      DateTime TravelStartDate,
      DateTime TravelEndDate,
      string LocationName,
      decimal PriceAmount,
      TripStatus TripStatus
      );
}
