using BuildingBlocks.Domain;
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
        protected Guid Id { get; private set; }
        protected List<Guid> TripIds { get; private set; }
        protected string Name { get; private set; }
        protected string LastName { get; private set; }
        protected string Email { get; private set; }
        protected string PhoneNumber { get; private set; }
        protected DateTime DateOfBirth { get; private set; }
        protected Gender Gender { get; private set; }
        protected Address Address { get; private set; }
        #endregion
        #region Constractor
        protected Person(Guid Id, string name, string lastName, string email, string phoneNumber, DateTime dateOfBirth, Gender gender, Address address, string nationality) : base(Id)
        {

            Id = Guid.NewGuid();
            Name = name;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address;
        }
        #endregion
        #region Guards
        #endregion
    }
}
