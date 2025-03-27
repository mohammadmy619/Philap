using BuildingBlocks.Domain;
using Domain.Persons.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Persons
{
    public class Address : ValueObject<Address>
    {


        #region propertys
  

        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string ZipCode { get; private set; }
        #endregion
        #region Constractor
        public Address(string street, string city, string state, string zipCode)
        {
            GuardAgainstStreet(street);
            GuardAgainstCity(city);
            GuardAgainstState(state);
            GuardAgainstZipCode(zipCode);
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
        }
        #endregion

        #region Methods


        public override IEnumerable<object> GetEqualityComponents()
        {
  
            yield return Street;
            yield return City;
            yield return State;
            yield return ZipCode;
        }
      


 

        public override string ToString()
        {
            return $"{Street}, {City}, {State} {ZipCode}";
        }
        #endregion

        #region Guard Methods  
        private static void GuardAgainstStreet(string street)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new StreetIsNullException();
        }

        private static void GuardAgainstCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new CityIsNullException();
        }

        private static void GuardAgainstState(string state)
        {
            if (string.IsNullOrWhiteSpace(state) || state.Length < 2 || state.Length > 3)
                throw new StateIsNullException();
        }

        private static void GuardAgainstZipCode(string zipCode)
        {
            if (string.IsNullOrWhiteSpace(zipCode) || zipCode.Length != 5)
                throw new ZipCodeIsNullException();
        }

   
        #endregion
    }
}
