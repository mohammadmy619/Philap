using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.TripAggregate.Exceptions
{



    public class LeaderIdIsNullException : DomainException
    {
        public LeaderIdIsNullException(string message = "Leader Id is Required", string code = "0314001") : base(message, code)
        {
        }
    }
    public class LeaderIdISNotValidException : DomainException
    {
        public LeaderIdISNotValidException(string message = "Leader Id is Not Valid", string code = "0314001") : base(message, code)
        {
        }
    }

    public class TravelStartDateIsNullException : DomainException
    {
        public TravelStartDateIsNullException(string message = "Travel Start Date is Required", string code = "0314002") : base(message, code)
        {
        }
    }
    public class TravelStartDateISnotValidException : DomainException
    {
        public TravelStartDateISnotValidException(string message = "Travel Start Date IS not Valid", string code = "0314008") : base(message, code)
        {
        }
    }

    public class TravelEndDateIsNullException : DomainException
    {
        public TravelEndDateIsNullException(string message = "Travel End Date is Required", string code = "0314003") : base(message, code)
        {
        }
    }
    public class TravelEndDateISnotValidException : DomainException
    {
        public TravelEndDateISnotValidException(string message = "Travel End Date IS not Valid", string code = "0314007") : base(message, code)
        {
        }
    }

    public class LocationNameIsNullException : DomainException
    {
        public LocationNameIsNullException(string message = "Location Name is Required", string code = "0314004") : base(message, code)
        {
        }
    }

    public class InvalidPriceException : DomainException
    {
        public InvalidPriceException(string message = "Price must be greater than zero", string code = "0314005") : base(message, code)
        {
        }
    }

    public class InvalidTravelDateException : DomainException
    {
        public InvalidTravelDateException(string message = "Travel Start Date must be before Travel End Date", string code = "0314006") : base(message, code)
        {
        }
    }
    public class InvalidTripStatusException : DomainException
    {
        public InvalidTripStatusException(string message = "Travel Trip Status must be Not Null", string code = "0314010") : base(message, code)
        {
        }
    }


}
