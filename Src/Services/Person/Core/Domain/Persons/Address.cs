using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Persons
{
   public class Address
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
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
        }
        #endregion

        #region Methods
        //Override Equals و GetHashCode برای شناسایی بر اساس مقادیر
        public override bool Equals(object obj)
        {
            return obj is Address address &&
                  Street == address.Street &&
                  City == address.City &&
                  State == address.State &&
                  ZipCode == address.ZipCode;
            //return false;
        }



        public override int GetHashCode()
        {
            return HashCode.Combine(Street, City, State, ZipCode);
        }

        public override string ToString()
        {
            return $"{Street}, {City}, {State} {ZipCode}";
        }
        #endregion
    }
}
