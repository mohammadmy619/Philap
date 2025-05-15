using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.TripAggregate;

namespace Application.Trips.GetTrip
{
    public record class GetTripResponse(  Guid TripId ,Guid LeaderId, DateTime TravelStartDa, DateTime TravelEndDate , string LocationName, decimal PriceAmount, TripStatus TripStatus) ;

}
