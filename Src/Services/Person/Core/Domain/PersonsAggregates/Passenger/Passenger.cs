using Domain.Persons.Exceptions;
using Domain.Persons.Passenger.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Persons.Passenger
{
    public class Passenger : Person
    {
        #region Properties  

        public string PassportNumber { get; private set; }
        public List<string> FrequentFlyerNumbers { get; private set; }

        #endregion

        #region Constructor  

        protected Passenger(List<Guid>? TripIds, string name, string lastName, string email, string phoneNumber,
            DateTime dateOfBirth, Gender gender, Address address, string nationality, string passportNumber, List<string> frequentFlyerNumbers)
            : base( TripIds, name, lastName, email, phoneNumber, dateOfBirth, gender, address, nationality)
        {
            GuardAgainstPassportNumber(passportNumber);
            GuardAgainstFrequentFlyerNumbers(frequentFlyerNumbers);

            PassportNumber = passportNumber;
            FrequentFlyerNumbers = frequentFlyerNumbers;
        }

        #endregion

        #region Guard Methods  

        private void GuardAgainstPassportNumber(string passportNumber)
        {
            // چک شماره پاسپورت در دیتابیس (توسعه لازم)  
            if (string.IsNullOrWhiteSpace(passportNumber))
            {
                throw new PassportNumberIsNullException();
            }
        }

        private void GuardAgainstFrequentFlyerNumbers(List<string> frequentFlyerNumbers)
        {
            if (frequentFlyerNumbers == null || frequentFlyerNumbers.Count == 0)
            {
                throw new FrequentFlyerNumbersAreNullException();
            }
        }

        #endregion

    }


}
