using BuildingBlocks.Domain;
using Domain.TripAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.TripAggregate
{
    public class Trip : AggregateRoot<Guid>
    {
        #region Fields

        private readonly List<Guid> _ticketIds = new();

        #endregion

        #region Constructors

 



        public Trip(
            Guid leaderId,
            DateTime travelStartDate,
            DateTime travelEndDate,
            string locationName,
            TripStatus tripStatus,
            Price price,
            List<Guid>? ticketIds = null)
        {
            GuardAgainstLeaderId(leaderId);
            GuardAgainstTravelStartDate(travelStartDate);
            GuardAgainstTravelEndDate(travelEndDate);
            GuardAgainstLocationName(locationName);
            GuardAgainstTravelDates(travelStartDate, travelEndDate);
            GuardAgainstTripStatus(tripStatus);

            LeaderId = leaderId;
            TravelStartDate = travelStartDate;
            TravelEndDate = travelEndDate;
            LocationName = locationName;
            Price = price;
            TripStatus = tripStatus;

            if (ticketIds != null && ticketIds.Count > 0)
            {
                SetTicketIds(ticketIds);
            }
        }

        // برای EF Core و ORMها
        public Trip()
        {
        }


        #endregion

        #region Properties

        public Guid LeaderId { get; private set; }
        public DateTime TravelStartDate { get; private set; }
        public DateTime TravelEndDate { get; private set; }
        public string LocationName { get; private set; } // تغییر به private set جهت حفظ کپسوله‌سازی
        public Price Price { get; private set; }
        public TripStatus TripStatus { get; private set; }

        public IReadOnlyCollection<Guid> TicketIds => _ticketIds.AsReadOnly();

        #endregion

        #region Methods

        public void UpdateTrip(
            Guid id,
            Guid leaderId,
            DateTime travelStartDate,
            DateTime travelEndDate,
            string locationName,
            TripStatus tripStatus,
            Price price,
            List<Guid>? ticketIds = null)
        {
            GuardAgainstTripId(id);
            GuardAgainstLeaderId(leaderId);
            GuardAgainstTravelStartDate(travelStartDate);
            GuardAgainstTravelEndDate(travelEndDate);
            GuardAgainstLocationName(locationName);
            GuardAgainstTravelDates(travelStartDate, travelEndDate);
            GuardAgainstTripStatus(tripStatus);

            LeaderId = leaderId;
            TravelStartDate = travelStartDate;
            TravelEndDate = travelEndDate;
            LocationName = locationName;
            Price = price;
            TripStatus = tripStatus;

            if (ticketIds != null)
            {
                SetTicketIds(ticketIds);
            }
        }

        public void AddTicketId(Guid ticketId)
        {
            GuardAgainstTicketId(ticketId);
            GuardAgainstDuplicateTicketId(ticketId);

            _ticketIds.Add(ticketId);
        }

        public void RemoveTicketId(Guid ticketId)
        {
            GuardAgainstTicketId(ticketId);

            var removed = _ticketIds.Remove(ticketId);

            if (!removed)
                throw new TicketNotFoundException();
        }
        /// <summary>
        /// بازگرداندن بلیط به سفر در صورت شکست فرآیند لغو در سرویس شخص (Compensation)
        /// </summary>
        public void RevertTicketCancellation(Guid ticketId)
        {
            GuardAgainstTicketId(ticketId);

            // بررسی Idempotency: اگر بلیط از قبل در لیست وجود دارد، یعنی قبلاً بازگردانی شده است
            // در این حالت هیچ کاری انجام نمی‌دهیم تا از خطای تکراری جلوگیری شود
            if (_ticketIds.Contains(ticketId))
            {
                return;
            }

            _ticketIds.Add(ticketId);
        }
        public bool HasTicket(Guid ticketId)
        {
            GuardAgainstTicketId(ticketId);

            return _ticketIds.Contains(ticketId);
        }

        private void SetTicketIds(IEnumerable<Guid> ticketIds)
        {
            _ticketIds.Clear();
            foreach (var ticketId in ticketIds)
            {
                AddTicketId(ticketId);
            }
        }

        #endregion

        #region Guard Against 

        private void GuardAgainstTripId(Guid tripId)
        {
            if (tripId == Guid.Empty)
                throw new TripIdIsInvalidException();
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

            if (travelStartDate < DateTime.UtcNow)
                throw new TravelStartDateISnotValidException();
        }

        private static void GuardAgainstTravelEndDate(DateTime travelEndDate)
        {
            if (travelEndDate == default)
                throw new TravelEndDateIsNullException();
        }

        private static void GuardAgainstLocationName(string locationName)
        {
            if (string.IsNullOrWhiteSpace(locationName))
                throw new LocationNameIsNullException();
        }

        private static void GuardAgainstTravelDates(DateTime travelStartDate, DateTime travelEndDate)
        {
            if (travelStartDate >= travelEndDate)
                throw new InvalidTravelDateException();
        }

        private static void GuardAgainstTripStatus(TripStatus tripStatus)
        {
            if (!Enum.IsDefined(typeof(TripStatus), tripStatus))
                throw new InvalidTripStatusException();
        }

        private static void GuardAgainstTicketId(Guid ticketId)
        {
            if (ticketId == Guid.Empty)
                throw new ArgumentException("TicketId cannot be empty.", nameof(ticketId));
        }

        private void GuardAgainstDuplicateTicketId(Guid ticketId)
        {
            if (_ticketIds.Contains(ticketId))
                throw new InvalidOperationException($"TicketId '{ticketId}' already exists in this trip.");
        }

        #endregion
    }
}
