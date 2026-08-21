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


        #region Fields
        private readonly List<Guid> _ticketIds = new();
        #endregion

        #region Properties  

        public string PassportNumber { get; private set; }
        public List<string> FrequentFlyerNumbers { get; private set; }

        public IReadOnlyCollection<Guid> TicketIds => _ticketIds.AsReadOnly();
        #endregion

        #region Constructor  

        public Passenger(
       List<Guid>? tripIds,
       string name,
       string lastName,
       string email,
       string phoneNumber,
       DateTime dateOfBirth,
       Gender gender,
       Address address,
       string nationality,
       bool isActive,              // Fixed: removed duplicate "bool Address" typo
       string passportNumber,
       List<string> frequentFlyerNumbers)
       : base(tripIds, name, lastName, email, phoneNumber, dateOfBirth, gender, address, nationality, isActive)
        {
            GuardAgainstPassportNumber(passportNumber);
            GuardAgainstFrequentFlyerNumbers(frequentFlyerNumbers);

            PassportNumber = passportNumber;
            FrequentFlyerNumbers = frequentFlyerNumbers;
        }

        #endregion
        #region
        // --- متود Update ---
        public void Update(
            List<Guid>? tripIds,
            string name,
            string lastName,
            string email,
            string phoneNumber,
            DateTime dateOfBirth,
            Gender gender,
            Address address,
            string nationality,
            bool isActive,
            string passportNumber,
            List<string> frequentFlyerNumbers)
        {
            // 1. آپدیت فیلدهای Person
            UpdatePerson(
                tripIds: tripIds,
                name: name,
                lastName: lastName,
                email: email,
                phoneNumber: phoneNumber,
                dateOfBirth: dateOfBirth,
                gender: gender,
                address: address,
                nationality: nationality,
                isActive: isActive
                );

            // 2. اعتبارسنجی پاسپورت و شماره‌های مسافر تکراری
            GuardAgainstPassportNumber(passportNumber);
            GuardAgainstFrequentFlyerNumbers(frequentFlyerNumbers);

            // 3. به‌روزرسانی فیلدهای خاص Passenger
            PassportNumber = passportNumber;
            FrequentFlyerNumbers = frequentFlyerNumbers;
        }

        #endregion


        #region method
        public void AddTicketId(Guid ticketId)
        {
            GuardAgainstTicketId(ticketId);
            GuardAgainstDuplicateTicketId(ticketId);

            _ticketIds.Add(ticketId);
        }

        public void RemoveTicketId(Guid ticketId)
        {
            GuardAgainstTicketId(ticketId);

            var removed = _ticketIds.Remove(ticketId);

            if (!removed)
                throw new TicketNotFoundException();
        }

        public bool HasTicket(Guid ticketId)
        {
            GuardAgainstTicketId(ticketId);

            return _ticketIds.Contains(ticketId);
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


        private static void GuardAgainstTicketId(Guid ticketId)
        {
            if (ticketId == Guid.Empty)
                throw new ArgumentException("TicketId cannot be empty.");
        }

        private void GuardAgainstDuplicateTicketId(Guid ticketId)
        {
            if (_ticketIds.Contains(ticketId))
                throw new InvalidOperationException($"TicketId '{ticketId}' already exists in trip.");
        }

        #endregion

    }


}
