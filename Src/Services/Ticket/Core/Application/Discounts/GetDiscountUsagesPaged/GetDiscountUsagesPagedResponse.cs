using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Discounts.GetDiscountUsagesPaged
{

    public record GetDiscountUsagesPagedResponse(
        IReadOnlyCollection<GetDiscountUsageResponse> Usages,
        int TotalCount,
        int Page,
        int PageSize,
        int TotalPages
    );
}
