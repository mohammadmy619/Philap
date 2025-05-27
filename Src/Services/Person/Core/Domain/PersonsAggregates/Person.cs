using BuildingBlocks.Domain;
using Domain.Persons.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Persons
{
    public class Person : AggregateRoot<Guid>
    {
        #region property
        public Guid Id { get; private set; }
        public List<Guid> TripIds { get; private set; }
        public string Name { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public Gender Gender { get; private set; }
        public string Nationality { get; private set; }
        public Address Address { get; private set; }

        #endregion
        #region Constractor
        protected Person(List<Guid> TripIds, string name, string lastName, string email, string phoneNumber, DateTime dateOfBirth, Gender gender, Address address, string nationality)
        {
            GuardAgainstLeaderId(Id);
            GuardAgainstTripIds(TripIds);
            GuardAgainstName(name);
            GuardAgainstLastName(lastName);
            GuardAgainstEmail(email);
            GuardAgainstPhoneNumber(phoneNumber);
            GuardAgainstDateOfBirth(dateOfBirth);
            GuardAgainstGender(gender);
            GuardAgainstNationality(nationality);

            Name = name;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address;
            Nationality = nationality;
        }

        private Person()
        {

        }
        public void UpdatePerson(
         List<Guid> tripIds,
         string name,
         string lastName,
         string email,
         string phoneNumber,
         DateTime dateOfBirth,
         Gender gender,
         Address address,
         string nationality)
        {
            // اعتبارسنجی تمام فیلدها
            GuardAgainstTripIds(tripIds);
            GuardAgainstName(name);
            GuardAgainstLastName(lastName);
            GuardAgainstEmail(email);
            GuardAgainstPhoneNumber(phoneNumber);
            GuardAgainstDateOfBirth(dateOfBirth);
            GuardAgainstGender(gender);
            GuardAgainstNationality(nationality);

            // آپدیت مقادیر
            TripIds = tripIds;
            Name = name;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address;
            Nationality = nationality;
        }
        #endregion
        #region Guards
        private static void GuardAgainstLeaderId(Guid leaderId)
        {
            if (leaderId == Guid.Empty)
                throw new LeaderIdIsNullException();
        }
        private static void GuardAgainstTripIds(List<Guid> TripIds)
        {
            //to do check TripId with acl
            if (TripIds.Count > 1) { }
            //throw new TripIds();
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
        private static void GuardAgainstGender(Gender Gender)
        {


            if (Enum.IsDefined(typeof(Gender), Gender))
                throw new LeaderGenderIsNullException();
        }
        private static void GuardAgainstNationality(string Nationality)
        {


            if (string.IsNullOrEmpty(Nationality))
                throw new NationalityIsNullException();
        }


        #endregion
    }
}
