using MediatR;

namespace Application.Discounts.GetDiscountUsagesPaged
{
    public record GetDiscountUsagesPagedQuery(
       Guid DiscountId,
       int Page = 1,
       int PageSize = 20
   ) : IRequest<GetDiscountUsagesPagedResponse>;
}
