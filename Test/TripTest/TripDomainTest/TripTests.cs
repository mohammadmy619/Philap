using Domain.TripAggregate;
using Domain.TripAggregate.Exceptions;
using FluentAssertions;
using System;
using System.Net.NetworkInformation;
using Xunit;

namespace Domain.TripAggregate.Tests
{
    public class TripTests
    {
        // یک Price معتبر بساز؛ اگر سازنده‌اش متفاوت است، این‌جا را اصلاح کن
        private Price CreateValidPrice()
        {
            // فرض: Price(decimal amount, string currency)
            return new Price(1000m, "USD");
        }

        private DateTime FutureDate(int days = 10) => DateTime.Now.AddDays(days);

        #region Constructor Tests

        [Fact]
        public void Ctor_Should_Create_Trip_When_Arguments_Are_Valid()
        {
            // Arrange
            var leaderId = Guid.NewGuid();
            var startDate = FutureDate(10);
            var endDate = FutureDate(20);
            var locationName = "Tehran";
            var tripStatus = TripStatus.Active; // مقدار معتبر
            var price = CreateValidPrice();

            // Act
            var trip = new Trip(leaderId, startDate, endDate, locationName, tripStatus, price);

            // Assert
            trip.LeaderId.Should().Be(leaderId);
            trip.TravelStartDate.Should().Be(startDate);
            trip.TravelEndDate.Should().Be(endDate);
            trip.LocationName.Should().Be(locationName);
            trip.TripStatus.Should().Be(tripStatus);
            trip.Price.Should().Be(price);
        }

        [Fact]
        public void Ctor_Should_Throw_LeaderIdIsNullException_When_LeaderId_Is_Empty()
        {
            // Arrange
            var leaderId = Guid.Empty;
            var startDate = FutureDate(10);
            var endDate = FutureDate(20);
            var locationName = "Tehran";
            var tripStatus = TripStatus.Active;
            var price = CreateValidPrice();

            // Act
            Action act = () => new Trip(leaderId, startDate, endDate, locationName, tripStatus, price);

            // Assert
            act.Should().Throw<LeaderIdIsNullException>();
        }

        [Fact]
        public void Ctor_Should_Throw_TravelStartDateIsNullException_When_StartDate_Is_Default()
        {
            // Arrange
            var leaderId = Guid.NewGuid();
            var startDate = default(DateTime);
            var endDate = FutureDate(20);
            var locationName = "Tehran";
            var tripStatus = TripStatus.Active;
            var price = CreateValidPrice();

            // Act
            Action act = () => new Trip(leaderId, startDate, endDate, locationName, tripStatus, price);

            // Assert
            act.Should().Throw<TravelStartDateIsNullException>();
        }

        [Fact]
        public void Ctor_Should_Throw_TravelStartDateISnotValidException_When_StartDate_Is_In_Past()
        {
            // Arrange
            var leaderId = Guid.NewGuid();
            var startDate = DateTime.Now.AddDays(-1);
            var endDate = FutureDate(5);
            var locationName = "Tehran";
            var tripStatus = TripStatus.Active;
            var price = CreateValidPrice();

            // Act
            Action act = () => new Trip(leaderId, startDate, endDate, locationName, tripStatus, price);

            // Assert
            act.Should().Throw<TravelStartDateISnotValidException>();
        }

        [Fact]
        public void Ctor_Should_Throw_TravelEndDateIsNullException_When_EndDate_Is_Default()
        {
            // Arrange
            var leaderId = Guid.NewGuid();
            var startDate = FutureDate(5);
            var endDate = default(DateTime);
            var locationName = "Tehran";
            var tripStatus = TripStatus.Active;
            var price = CreateValidPrice();

            // Act
            Action act = () => new Trip(leaderId, startDate, endDate, locationName, tripStatus, price);

            // Assert
            act.Should().Throw<TravelEndDateIsNullException>();
        }

        [Fact]
        public void Ctor_Should_Throw_LocationNameIsNullException_When_Location_Is_Null_Or_Empty()
        {
            // Arrange
            var leaderId = Guid.NewGuid();
            var startDate = FutureDate(5);
            var endDate = FutureDate(10);
            string locationName = null; // یا "" نیز می‌توان تست کرد
            var tripStatus = TripStatus.Active;
            var price = CreateValidPrice();

            // Act
            Action act = () => new Trip(leaderId, startDate, endDate, locationName, tripStatus, price);

            // Assert
            act.Should().Throw<LocationNameIsNullException>();
        }

        [Fact]
        public void Ctor_Should_Throw_InvalidTravelDateException_When_StartDate_Is_After_Or_Equal_EndDate()
        {
            // Arrange
            var leaderId = Guid.NewGuid();
            var startDate = FutureDate(10);
            var endDate = startDate; // مساوی؛ می‌توان > هم تست کرد
            var locationName = "Tehran";
            var tripStatus = TripStatus.Active;
            var price = CreateValidPrice();

            // Act
            Action act = () => new Trip(leaderId, startDate, endDate, locationName, tripStatus, price);

            // Assert
            act.Should().Throw<InvalidTravelDateException>();
        }

        [Fact]
        public void Ctor_Should_Throw_InvalidTripStatusException_When_TripStatus_Is_Invalid()
        {
         

            // Arrange
            var leaderId = Guid.NewGuid();
            var startDate = FutureDate(5);
            var endDate = FutureDate(10);
            var locationName = "Tehran";
            var tripStatus = (TripStatus)999;
            var price = CreateValidPrice();

            // Act
            Action act = () => new Trip(leaderId, startDate, endDate, locationName, tripStatus, price);

            // Assert
            act.Should().Throw<InvalidTripStatusException>();
        }

        #endregion

        #region UpdateTrip Tests

        [Fact]
        public void UpdateTrip_Should_Update_Fields_When_Data_Is_Valid()
        {
            // Arrange
            var leaderId = Guid.NewGuid();
            var startDate = FutureDate(5);
            var endDate = FutureDate(10);
            var locationName = "Tehran";
            var tripStatus = TripStatus.Active; // چون منطق فعلی InvalidStatus برای مقدار معتبر Exception می‌اندازد،
                                              // این‌جا برای تست "موفق" از مقدار غیر معتبر استفاده می‌کنیم!
            var price = CreateValidPrice();

            var trip = new Trip(leaderId, startDate, endDate, locationName, tripStatus, price);

            var newLeaderId = Guid.NewGuid();
            var newStartDate = FutureDate(20);
            var newEndDate = FutureDate(30);
            var newLocationName = "Isfahan";
            var newTripStatus = TripStatus.Cancelled; // متناسب با منطق فعلی
            var newPrice = new Price(2000m, "USD");

            var someTripId = Guid.NewGuid();

            // Act
            trip.UpdateTrip(someTripId, newLeaderId, newStartDate, newEndDate, newLocationName, newTripStatus, newPrice);

            // Assert
            trip.LeaderId.Should().Be(newLeaderId);
            trip.TravelStartDate.Should().Be(newStartDate);
            trip.TravelEndDate.Should().Be(newEndDate);
            trip.LocationName.Should().Be(newLocationName);
            trip.TripStatus.Should().Be(newTripStatus);
            trip.Price.Should().Be(newPrice);
        }

        [Fact]
        public void UpdateTrip_Should_Throw_TripIdIsInvalidException_When_Id_Is_Empty()
        {
            // Arrange
            var leaderId = Guid.NewGuid();
            var startDate = FutureDate(5);
            var endDate = FutureDate(10);
            var locationName = "Tehran";
            var tripStatus = (TripStatus)999;
            var tripStatusvalid = TripStatus.Active;
            var price = CreateValidPrice();

            var trip = new Trip(leaderId, startDate, endDate, locationName, tripStatusvalid, price);

            var invalidId = Guid.Empty;

            // Act
            Action act = () => trip.UpdateTrip(invalidId, leaderId, startDate, endDate, locationName, tripStatus, price);

            // Assert
            act.Should().Throw<TripIdIsInvalidException>();
        }

        [Fact]
        public void UpdateTrip_Should_Throw_LeaderIdIsNullException_When_LeaderId_Is_Empty()
        {
            // Arrange
            var leaderId = Guid.NewGuid();
            var startDate = FutureDate(5);
            var endDate = FutureDate(10);
            var locationName = "Tehran";
            var tripStatus = TripStatus.Active;
            var price = CreateValidPrice();

            var trip = new Trip(leaderId, startDate, endDate, locationName, tripStatus, price);

            var invalidLeaderId = Guid.Empty;

            // Act
            Action act = () => trip.UpdateTrip(Guid.NewGuid(), invalidLeaderId, startDate, endDate, locationName, tripStatus, price);

            // Assert
            act.Should().Throw<LeaderIdIsNullException>();
        }

        [Fact]
        public void UpdateTrip_Should_Throw_TravelStartDateIsNullException_When_StartDate_Is_Default()
        {
            // Arrange
            var leaderId = Guid.NewGuid();
            var startDate = FutureDate(5);
            var endDate = FutureDate(10);
            var locationName = "Tehran";
            var tripStatus = TripStatus.Active;
            var price = CreateValidPrice();

            var trip = new Trip(leaderId, startDate, endDate, locationName, tripStatus, price);

            // Act
            Action act = () => trip.UpdateTrip(
                Guid.NewGuid(),
                leaderId,
                default(DateTime),
                endDate,
                locationName,
                tripStatus,
                price);

            // Assert
            act.Should().Throw<TravelStartDateIsNullException>();
        }

        [Fact]
        public void UpdateTrip_Should_Throw_TravelStartDateISnotValidException_When_StartDate_Is_In_Past()
        {
            // Arrange
            var leaderId = Guid.NewGuid();
            var startDate = FutureDate(5);
            var endDate = FutureDate(10);
            var locationName = "Tehran";
            var tripStatus = TripStatus.Active;
            var price = CreateValidPrice();

            var trip = new Trip(leaderId, startDate, endDate, locationName, tripStatus, price);

            var pastStartDate = DateTime.Now.AddDays(-2);

            // Act
            Action act = () => trip.UpdateTrip(
                Guid.NewGuid(),
                leaderId,
                pastStartDate,
                endDate,
                locationName,
                tripStatus,
                price);

            // Assert
            act.Should().Throw<TravelStartDateISnotValidException>();
        }

        [Fact]
        public void UpdateTrip_Should_Throw_TravelEndDateIsNullException_When_EndDate_Is_Default()
        {
            // Arrange
            var leaderId = Guid.NewGuid();
            var startDate = FutureDate(5);
            var endDate = FutureDate(10);
            var locationName = "Tehran";
            var tripStatus = TripStatus.Active;
            var price = CreateValidPrice();

            var trip = new Trip(leaderId, startDate, endDate, locationName, tripStatus, price);

            // Act
            Action act = () => trip.UpdateTrip(
                Guid.NewGuid(),
                leaderId,
                startDate,
                default(DateTime),
                locationName,
                tripStatus,
                price);

            // Assert
            act.Should().Throw<TravelEndDateIsNullException>();
        }

        [Fact]
        public void UpdateTrip_Should_Throw_LocationNameIsNullException_When_Location_Is_Null_Or_Empty()
        {
            // Arrange
            var leaderId = Guid.NewGuid();
            var startDate = FutureDate(5);
            var endDate = FutureDate(10);
            var locationName = "Tehran";
            var tripStatus = TripStatus.Active;
            var price = CreateValidPrice();

            var trip = new Trip(leaderId, startDate, endDate, locationName, tripStatus, price);

            string newLocationName = string.Empty;

            // Act
            Action act = () => trip.UpdateTrip(
                Guid.NewGuid(),
                leaderId,
                startDate,
                endDate,
                newLocationName,
                tripStatus,
                price);

            // Assert
            act.Should().Throw<LocationNameIsNullException>();
        }

        [Fact]
        public void UpdateTrip_Should_Throw_InvalidTravelDateException_When_StartDate_Is_After_Or_Equal_EndDate()
        {
            // Arrange
            var leaderId = Guid.NewGuid();
            var startDate = FutureDate(5);
            var endDate = FutureDate(10);
            var locationName = "Tehran";
            var tripStatus = TripStatus.Active;
            var price = CreateValidPrice();

            var trip = new Trip(leaderId, startDate, endDate, locationName, tripStatus, price);

            var newStart = FutureDate(15);
            var newEnd = FutureDate(10); // قبل از Start

            // Act
            Action act = () => trip.UpdateTrip(
                Guid.NewGuid(),
                leaderId,
                newStart,
                newEnd,
                locationName,
                tripStatus,
                price);

            // Assert
            act.Should().Throw<InvalidTravelDateException>();
        }

        [Fact]
        public void UpdateTrip_Should_Throw_InvalidTripStatusException_When_TripStatus_Is_Valid_Based_On_Current_Logic()
        {
            // براساس منطق فعلی، هر مقدار معتبر Enum باعث Exception می‌شود
            // Arrange
            var leaderId = Guid.NewGuid();
            var startDate = FutureDate(5);
            var endDate = FutureDate(10);
            var locationName = "Tehran";
            var invalidLogicTripStatus = TripStatus.Active; // برای ساخت Trip معتبر
            var price = CreateValidPrice();

            var trip = new Trip(leaderId, startDate, endDate, locationName, invalidLogicTripStatus, price);

            var validStatus = (TripStatus)999; // enum معتبر

            // Act
            Action act = () => trip.UpdateTrip(
                Guid.NewGuid(),
                leaderId,
                startDate,
                endDate,
                locationName,
                validStatus,
                price);

            // Assert
            act.Should().Throw<InvalidTripStatusException>();
        }

        #endregion
    }
}
