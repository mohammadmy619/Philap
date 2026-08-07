using Domain.DiscountAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Discounts.GetDiscount
{
    public record GetDiscountResponse(
         Guid DiscountId,
         string Code,
         DiscountType Type,
         decimal Value,
         DateTime? ValidFrom,
         DateTime? ValidTo,
         int? MaxUsageCount,
         int UsedCount,
         Guid? ApplicableTripId,
         Guid? ApplicablePassengerId,
         bool IsActive,
         DateTime CreatedAt,
         DateTime? LastUsedAt
     );
}
