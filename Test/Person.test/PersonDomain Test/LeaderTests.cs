using Domain.Persons;
using Domain.Persons.Leader;
using Domain.Persons.Leader.Exception;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Xunit;

namespace Domain.Tests.Persons
{
    public class LeaderTests
    {
        // داده‌های نمونه برای تست
        private readonly List<Guid> _validTripIds = new() { Guid.NewGuid() };
        private readonly Address _validAddress = new Address("Iran", "Tehran", "Main St","13878683"); // فرض بر وجود این سازنده
        private readonly List<string> _validSkills = new() { "C#", "Leadership" };

        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateLeader()
        {
            // Arrange & Act
            var leader = new Leader(
                _validTripIds,
                "Ali",
                "Alavi",
                "ali@example.com",
                "09123456789",
                new DateTime(1990, 1, 1),
                Gender.Male,
                _validAddress,
                "Iranian",
                true,
                "Senior Guide",
                "Adventure",
                DateTime.Now.AddDays(-10),
                _validSkills,
                "Experienced leader"
            );

            // Assert
            leader.Should().NotBeNull();
            leader.Name.Should().Be("Ali");
            leader.Title.Should().Be("Senior Guide");
            leader.Skills.Should().HaveCount(2);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_WhenTitleIsInvalid_ShouldThrowTitleIsNullException(string invalidTitle)
        {
            // Act
            Action act = () => new Leader(
                _validTripIds, "Ali", "Alavi", "ali@example.com", "09123456789",
                new DateTime(1990, 1, 1), Gender.Male, _validAddress, "Iranian", true,
                invalidTitle, "Department", DateTime.Now.AddDays(-1), _validSkills, "Bio"
            );

            // Assert
            act.Should().Throw<TitleIsNullException>();
        }

        [Fact]
        public void Constructor_WhenJoiningDateIsInFuture_ShouldThrowJoiningDateIsInvalidException()
        {
            // Arrange
            var futureDate = DateTime.Now.AddDays(5);

            // Act
            Action act = () => new Leader(
                _validTripIds, "Ali", "Alavi", "ali@example.com", "09123456789",
                new DateTime(1990, 1, 1), Gender.Male, _validAddress, "Iranian", true,
                "Title", "Dept", futureDate, _validSkills, "Bio"
            );

            // Assert
            act.Should().Throw<JoiningDateIsInvalidException>();
        }

        [Fact]
        public void Update_WhenValidDataProvided_ShouldUpdateAllProperties()
        {
            // Arrange
            var leader = new Leader(
                _validTripIds, "Ali", "Alavi", "ali@example.com", "09123456789",
                new DateTime(1990, 1, 1), Gender.Male, _validAddress, "Iranian", true,
                "Old Title", "Old Dept", DateTime.Now.AddDays(-1), _validSkills, "Old Bio"
            );

            var newSkills = new List<string> { "Management" };

            // Act
            leader.Update(
                _validTripIds,
                "Hassan",
                "Hassani",
                "hassan@example.com",
                "09876543210",
                new DateTime(1992, 2, 2),
                Gender.Male,
                _validAddress,
                "German",
                "New Title",
                "New Dept",
                DateTime.Now.AddDays(-2),
                newSkills,
                "New Bio"
            );

            // Assert
            leader.Name.Should().Be("Hassan");
            leader.LastName.Should().Be("Hassani");
            leader.Title.Should().Be("New Title");
            leader.Skills.Should().Contain("Management");
        }

        [Fact]
        public void GuardAgainstSkills_WhenSkillsIsEmpty_ShouldThrowSkillsAreEmptyException()
        {
            // Arrange
            var emptySkills = new List<string>();

            // Act
            Action act = () => new Leader(
                _validTripIds, "Ali", "Alavi", "ali@example.com", "09123456789",
                new DateTime(1990, 1, 1), Gender.Male, _validAddress, "Iranian", true,
                "Title", "Dept", DateTime.Now.AddDays(-1), emptySkills, "Bio"
            );

            // Assert
            act.Should().Throw<SkillsAreEmptyException>();
        }
    }
}
