using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Domain;

namespace Domain.DiscountAggregate.Exceptions
{
   
    public class DiscountUsageIdIsInvalidException : DomainException
    {
        public DiscountUsageIdIsInvalidException(string message = "Discount usage ID is invalid.", string code = "0517011")
            : base(message, code) { }
    }
}
