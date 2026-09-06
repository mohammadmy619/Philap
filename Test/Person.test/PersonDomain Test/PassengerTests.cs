using Domain.Persons;
using Domain.Persons.Passenger;
using Domain.Persons.Passenger.Exception;
using Domain.Persons.Exceptions;
using FluentAssertions;
using Xunit;
using System;
using System.Collections.Generic;

namespace Domain.UnitTests.Persons
{
    public class PassengerTests
    {
        // داده‌های کمکی برای تست (Dummy Data)
        private readonly Address _validAddress = new Address("Street", "City", "State", "8789778675");
        private readonly List<Guid> _validTripIds = new List<Guid> { Guid.NewGuid() };
        private readonly List<string> _validFrequentFlyerNumbers = new List<string> { "FF123", "FF456" };

        [Fact]
        public void Constructor_WithValidParameters_ShouldCreatePassenger()
        {
            // Arrange & Act
            // ✅ دقیقاً منطبق با کانستراکتور دامین شما (شامل address، بدون tripIds)
            var passenger = new Passenger(
                name: "John",
                lastName: "Doe",
                email: "john.doe@email.com",
                phoneNumber: "09123456789",
                dateOfBirth: new DateTime(1990, 1, 1),
                gender: Gender.Male,
                address: _validAddress, // ✅ address اضافه شد
                nationality: "Iranian",
                isActive: true,
                passportNumber: "A12345678",
                frequentFlyerNumbers: _validFrequentFlyerNumbers
            );

            // Assert
            passenger.Should().NotBeNull();
            passenger.Name.Should().Be("John");
            passenger.PassportNumber.Should().Be("A12345678");
            passenger.FrequentFlyerNumbers.Should().HaveCount(2);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Constructor_WithInvalidPassportNumber_ShouldThrowPassportNumberIsNullException(string invalidPassport)
        {
            // Arrange & Act
            Action act = () => new Passenger(
                "John",
                "Doe",
                "john@email.com",
                "0912",
                DateTime.Now.AddYears(-20),
                Gender.Male,
                _validAddress, // ✅ address اضافه شد
                "Iranian",
                true,
                invalidPassport,
                _validFrequentFlyerNumbers
            );

            // Assert
            act.Should().Throw<PassportNumberIsNullException>();
        }

        [Fact]
        public void Constructor_WithEmptyFrequentFlyerNumbers_ShouldThrowFrequentFlyerNumbersAreNullException()
        {
            // Act
            Action act = () => new Passenger(
                "John", "Doe", "john@email.com", "0912",
                DateTime.Now.AddYears(-20), Gender.Male, _validAddress, "IR", true, // ✅ address اضافه شد
                "A12345678", new List<string>());

            // Assert
            act.Should().Throw<FrequentFlyerNumbersAreNullException>();
        }

        [Fact]
        public void Update_WithValidParameters_ShouldUpdateAllProperties()
        {
            // Arrange
            var passenger = new Passenger(
                "OldName", "OldLast", "old@email.com", "0911",
                new DateTime(1990, 1, 1), Gender.Male, _validAddress, "OldNat", true, // ✅ address اضافه شد
                "OldPass", _validFrequentFlyerNumbers);

            var newTrips = new List<Guid> { Guid.NewGuid() };
            var newFlyers = new List<string> { "NEW999" };
            var newAddress = new Address("New Street", "New City", "NS", "123123123");

            // Act
            // ✅ متد Update دقیقاً با تمام پارامترهای اصلی (شامل tripIds و address) فراخوانی می‌شود
            passenger.Update(
                newTrips,
                "NewName",
                "NewLast",
                "new@email.com",
                "09998887766",
                new DateTime(1995, 5, 5),
                Gender.Female,
                newAddress,
                "NewNat",
                true,
                "NewPass123",
                newFlyers);

            // Assert
            passenger.Name.Should().Be("NewName");
            passenger.LastName.Should().Be("NewLast");
            passenger.Email.Should().Be("new@email.com");
            passenger.Gender.Should().Be(Gender.Female);
            passenger.PassportNumber.Should().Be("NewPass123");
            passenger.FrequentFlyerNumbers.Should().Contain("NEW999");
            passenger.Address.City.Should().Be("New City");
        }

        [Fact]
        public void Update_WithInvalidEmail_ShouldThrowExceptionFromBaseClass()
        {
            // Arrange
            var passenger = new Passenger(
                "John", "Doe", "john@email.com", "0912",
                DateTime.Now.AddYears(-20), Gender.Male, _validAddress, "IR", true, // ✅ address اضافه شد
                "A12345678", _validFrequentFlyerNumbers);

            // Act
            // ✅ متد Update با تمام پارامترهای اصلی فراخوانی می‌شود
            Action act = () => passenger.Update(
                _validTripIds, "John", "Doe", "", "0912",
                DateTime.Now.AddYears(-20), Gender.Male, _validAddress, "IR", false,
                "A12345678", _validFrequentFlyerNumbers);

            // Assert
            act.Should().Throw<LeaderEmailIsNullException>();
        }

        [Fact]
        public void AddTripId_WithValidGuid_ShouldAddToCollection()
        {
            // Arrange
            var passenger = new Passenger(
                "John", "Doe", "john@email.com", "0912",
                DateTime.Now.AddYears(-20), Gender.Male, _validAddress, "IR", true, // ✅ address اضافه شد
                "A12345678", _validFrequentFlyerNumbers);

            var newTripId = Guid.NewGuid();

            // Act
            passenger.AddTripId(newTripId);

            // Assert
            passenger.TripIds.Should().Contain(newTripId);
            passenger.TripIds.Should().HaveCount(1);
        }
    }
}