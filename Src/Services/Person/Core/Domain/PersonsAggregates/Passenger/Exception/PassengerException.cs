using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Persons.Passenger.Exception
{
    public class PassportNumberIsNullException : DomainException
    {
     

        public PassportNumberIsNullException(string message = "Passport number is required.", string code = "0124001")
            : base(message,code) { }
    }

    public class FrequentFlyerNumbersAreNullException : DomainException
    {


        public FrequentFlyerNumbersAreNullException(string message = "At least one frequent flyer number is required.", string code = "0124002")
            : base(message, code) { }
    }
}
