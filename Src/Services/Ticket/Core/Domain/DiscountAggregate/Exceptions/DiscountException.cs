using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Domain;

namespace Domain.DiscountAggregate.Exceptions
{
    // Exceptions
    public class DiscountNotApplicableException : DomainException
    {
        public DiscountNotApplicableException(string message = "Discount is not applicable.", string code = "0517005")
            : base(message, code) { }
    }

    public class DiscountExpiredException : DomainException
    {
        public DiscountExpiredException(string message = "Discount has expired.", string code = "0517006")
            : base(message, code) { }
    }

    public class DiscountMaxUsageExceededException : DomainException
    {
        public DiscountMaxUsageExceededException(string message = "Discount maximum usage count has been exceeded.", string code = "0517007")
            : base(message, code) { }
    }

    public class DiscountValueIsInvalidException : DomainException
    {
        public DiscountValueIsInvalidException(string message = "Discount value is invalid.", string code = "0517008")
            : base(message, code) { }
    }

    public class DiscountValidityPeriodIsInvalidException : DomainException
    {
        public DiscountValidityPeriodIsInvalidException(string message = "Discount validity period is invalid.", string code = "0517009")
            : base(message, code) { }
    }

    public class DiscountMaxUsageCountIsInvalidException : DomainException
    {
        public DiscountMaxUsageCountIsInvalidException(string message = "Discount maximum usage count is invalid.", string code = "0517010")
            : base(message, code) { }
    }
}
