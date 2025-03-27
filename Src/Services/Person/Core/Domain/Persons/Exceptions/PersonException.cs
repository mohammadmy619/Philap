using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Persons.Exceptions
{
    public class LeaderIdIsNullException : DomainException
    {
        public LeaderIdIsNullException(string message = "Leader Id is Required", string code = "0114001") : base(message, code)
        {
        }
    }

    public class LeaderNameIsNullException : DomainException
    {
        public LeaderNameIsNullException(string message = "Leader Name is Required", string code = "0114002") : base(message, code)
        {
        }
    }

    public class LeaderLastNameIsNullException : DomainException
    {
        public LeaderLastNameIsNullException(string message = "Leader Last Name is Required", string code = "0114003") : base(message, code)
        {
        }
    }

    public class LeaderEmailIsNullException : DomainException
    {
        public LeaderEmailIsNullException(string message = "Leader Email is Required", string code = "0114004") : base(message, code)
        {
        }
    }

    public class LeaderPhoneNumberIsNullException : DomainException
    {
        public LeaderPhoneNumberIsNullException(string message = "Leader Phone Number is Required", string code = "0114005") : base(message, code)
        {
        }
    }

    public class LeaderDateOfBirthIsNullException : DomainException
    {
        public LeaderDateOfBirthIsNullException(string message = "Leader Date of Birth is Required", string code = "0114006") : base(message, code)
        {
        }
    }

    public class LeaderGenderIsNullException : DomainException
    {
        public LeaderGenderIsNullException(string message = "Leader Gender is Required", string code = "0114007") : base(message, code)
        {
        }
    }
    public class NationalityIsNullException : DomainException
    {
        public NationalityIsNullException(string message = "Nationality is Required", string code = "0114008")
            : base(message, code)
        {
        }
    }
}
