using BuildingBlocks.Domain;
using Domain.TripAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.TripAggregate
{
    public class Trip: AggregateRoot<Guid>
    {
        public Trip(Guid tripId, Guid leaderId, DateTime travelStartDate, DateTime travelEndDate, string locationName, decimal price, TripStatus tripStatus): base(tripId)
        {
            Tripid = tripId;
            GuardAgainstLeaderId(leaderId);
            GuardAgainstTravelStartDate(travelStartDate);
            GuardAgainstTravelEndDate(travelEndDate);
            GuardAgainstLocationName(locationName);
            GuardAgainstPrice(price);
            GuardAgainstTravelDates(travelStartDate, travelEndDate);
            TripStatus = tripStatus;
        }


        public Guid Tripid { get; private set; }
        public Guid LeaderId { get; private set; }
        public DateTime TravelStartDate { get; private set; }
        public DateTime TravelEndDate { get; private set; }
        public string LocationName { get; set; }
        public price Price { get; private set; }
        public TripStatus TripStatus { get; private set; }


        #region GuardAgainst 

      
        private static void GuardAgainstLeaderId(Guid leaderId)
        {
            if (leaderId == Guid.Empty)
                throw new LeaderIdIsNullException();
        }

        private static void GuardAgainstTravelStartDate(DateTime travelStartDate)
        {
            if (travelStartDate == default)
                throw new TravelStartDateIsNullException();
            if(travelStartDate <  DateTime.Now)
                throw new TravelStartDateISnotValidException();


        }

        private static void GuardAgainstTravelEndDate(DateTime travelEndDate)
        {
            if (travelEndDate == default)
                throw new TravelEndDateIsNullException();

          
        }

        private static void GuardAgainstLocationName(string locationName)
        {
            if (string.IsNullOrEmpty(locationName))
                throw new LocationNameIsNullException();
        }
        ///todo:chake
        private static void GuardAgainstPrice(decimal price)
        {
            if (price <= 0)
                throw new InvalidPriceException();
        }

        private static void GuardAgainstTravelDates(DateTime travelStartDate, DateTime travelEndDate)
        {
            if (travelStartDate >= travelEndDate)
                throw new InvalidTravelDateException();
        } 
        

        #endregion

    }

}
