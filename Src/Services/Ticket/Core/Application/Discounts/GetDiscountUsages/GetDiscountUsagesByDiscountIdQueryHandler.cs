using MediatR;

namespace Application.Discounts.GetDiscountUsages
{
    public class GetDiscountUsagesByDiscountIdQueryHandler
        : IRequestHandler<GetDiscountUsagesByDiscountIdQuery, IEnumerable<GetDiscountUsageResponse>>
    {
        private readonly IDiscountRepository _discountRepository;

        public GetDiscountUsagesByDiscountIdQueryHandler(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<IEnumerable<GetDiscountUsageResponse>> Handle(
            GetDiscountUsagesByDiscountIdQuery request,
            CancellationToken cancellationToken)
        {
            var usages = await _discountRepository.GetUsagesByDiscountIdAsync(
                request.DiscountId,
                cancellationToken);

            return usages.Select(u => new GetDiscountUsageResponse(
                UsageId: u.Id,
                DiscountId: u.DiscountId,
                BookingId: u.BookingId,
                PassengerId: u.PassengerId,
                TripId: u.TripId,
                AppliedAmount: u.AppliedAmount,
                UsedAt: u.UsedAt
            ));
        }
    }
}