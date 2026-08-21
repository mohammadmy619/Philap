using BuildingBlocks.Domain;
using Domain.Persons.Exceptions;
using System;
using System.Collections.Generic;

namespace Domain.Persons
{
    public class Person : AggregateRoot<Guid>
    {
        #region Fields

        private readonly List<Guid> _tripIds = new();

        #endregion

        #region Properties

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public Gender Gender { get; private set; }
        public string Nationality { get; private set; }
        public bool IsActive { get; private set; }
        public Address Address { get; private set; }

        // کپسوله‌سازی شده - فقط خواندنی از بیرون
        public IReadOnlyCollection<Guid> TripIds => _tripIds.AsReadOnly();

        #endregion

        #region Constructor

        protected Person(
            List<Guid> tripIds,
            string name,
            string lastName,
            string email,
            string phoneNumber,
            DateTime dateOfBirth,
            Gender gender,
            Address address,
            string nationality,
            bool isActive)
        {
            this.Id = Guid.NewGuid();
            GuardAgainstTripIds(tripIds);
            GuardAgainstName(name);
            GuardAgainstLastName(lastName);
            GuardAgainstEmail(email);
            GuardAgainstPhoneNumber(phoneNumber);
            GuardAgainstDateOfBirth(dateOfBirth);
            GuardAgainstGender(gender);
            GuardAgainstNationality(nationality);

            SetTripIds(tripIds);

            Name = name;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address;
            Nationality = nationality;
            IsActive = isActive;
        }

        private Person()
        {
        }

        #endregion

        #region Methods

        public void UpdatePerson(
            List<Guid> tripIds,
            string name,
            string lastName,
            string email,
            string phoneNumber,
            DateTime dateOfBirth,
            Gender gender,
            Address address,
            string nationality,
            bool isActive)
        {
            GuardAgainstTripIds(tripIds);
            GuardAgainstName(name);
            GuardAgainstLastName(lastName);
            GuardAgainstEmail(email);
            GuardAgainstPhoneNumber(phoneNumber);
            GuardAgainstDateOfBirth(dateOfBirth);
            GuardAgainstGender(gender);
            GuardAgainstNationality(nationality);

            SetTripIds(tripIds);

            Name = name;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address;
            Nationality = nationality;
            IsActive = isActive;
        }

        public void AddTripId(Guid tripId)
        {
            GuardAgainstTripId(tripId);
            GuardAgainstDuplicateTripId(tripId);

            _tripIds.Add(tripId);
        }

        public void RemoveTripId(Guid tripId)
        {
            GuardAgainstTripId(tripId);

            var removed = _tripIds.Remove(tripId);

            if (!removed)
                throw new TripNotFoundException();
        }

        public bool HasTrip(Guid tripId)
        {
            GuardAgainstTripId(tripId);

            return _tripIds.Contains(tripId);
        }

        private void SetTripIds(IEnumerable<Guid> tripIds)
        {
            _tripIds.Clear();
            if (tripIds == null)
                return;

            foreach (var tripId in tripIds)
            {
                AddTripId(tripId);
            }
        }

        #endregion

        #region Guards

        private static void GuardAgainstTripId(Guid tripId)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("TripId cannot be empty.", nameof(tripId));
        }

        private static void GuardAgainstTripIds(List<Guid> tripIds)
        {
            // TODO: بررسی TripId از طریق ACL (پروتکل تطبیق با سیستم سفر)
            if (tripIds == null)
                return;

            if (tripIds.Any(t => t == Guid.Empty))
                throw new TripIdIsInvalidException();
        }

        private void GuardAgainstDuplicateTripId(Guid tripId)
        {
            if (_tripIds.Contains(tripId))
                throw new InvalidOperationException($"TripId '{tripId}' already exists for this person.");
        }

        private static void GuardAgainstName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new LeaderNameIsNullException();
        }

        private static void GuardAgainstLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new LeaderLastNameIsNullException();
        }

        private static void GuardAgainstEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new LeaderEmailIsNullException();
        }

        private static void GuardAgainstPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new LeaderPhoneNumberIsNullException();
        }

        private static void GuardAgainstDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth == default(DateTime))
                throw new LeaderDateOfBirthIsNullException();
        }

        private static void GuardAgainstGender(Gender gender)
        {
            if (!Enum.IsDefined(typeof(Gender), gender))
                throw new LeaderGenderIsNullException();
        }

        private static void GuardAgainstNationality(string nationality)
        {
            if (string.IsNullOrEmpty(nationality))
                throw new NationalityIsNullException();
        }

        #endregion
    }
}
