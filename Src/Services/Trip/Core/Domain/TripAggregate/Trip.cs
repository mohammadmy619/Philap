using BuildingBlocks.Domain;
using Domain.TripAggregate.Exceptions;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.TripAggregate
{
    public class Trip : AggregateRoot<Guid>
    {
        #region Constractor
        public Trip(Guid tripId, Guid leaderId, DateTime travelStartDate, DateTime travelEndDate, string locationName, TripStatus tripStatus, Price price) : base(tripId)
        {
            GuardAgainstTripId(tripId);
            GuardAgainstLeaderId(leaderId);
            GuardAgainstTravelStartDate(travelStartDate);
            GuardAgainstTravelEndDate(travelEndDate);
            GuardAgainstLocationName(locationName);
            GuardAgainstTravelDates(travelStartDate, travelEndDate);
            GuardAgainstTripStatus(tripStatus);
            Tripid = tripId;
            LeaderId = leaderId;
            TravelStartDate = travelStartDate;
            TravelEndDate = travelEndDate;
            LocationName = locationName;
            Price = price;
            TripStatus = tripStatus;
        }
        protected Trip(Guid tripId) : base(tripId)
        {

        }
        #endregion
        #region propertys
        public Guid Tripid { get; private set; }
        public Guid LeaderId { get; private set; }
        public DateTime TravelStartDate { get; private set; }
        public DateTime TravelEndDate { get; private set; }
        public string LocationName { get; set; }
        public Price Price { get; private set; }
        public TripStatus TripStatus { get; private set; }

        #endregion
        #region GuardAgainst 
        private void GuardAgainstTripId(Guid tripId)
        {
            if (tripId == Guid.Empty)
            {
                throw new TripIdIsInvalidException();  
            }
        }


        private static void GuardAgainstLeaderId(Guid leaderId)
        {
            if (leaderId == Guid.Empty)
                throw new LeaderIdIsNullException();
        }

        private static void GuardAgainstTravelStartDate(DateTime travelStartDate)
        {
            if (travelStartDate == default)
                throw new TravelStartDateIsNullException();
            if (travelStartDate < DateTime.Now)
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


        private static void GuardAgainstTravelDates(DateTime travelStartDate, DateTime travelEndDate)
        {
            if (travelStartDate >= travelEndDate)
                throw new InvalidTravelDateException();
        }


        private static void GuardAgainstTripStatus(TripStatus tripStatus)
        {


            if (Enum.IsDefined(typeof(TripStatus), tripStatus))
                throw new InvalidTripStatusException();
        }


        #endregion

    }

}
