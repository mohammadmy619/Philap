using Application.Discounts.GetDiscount;

public record GetDiscountsResponse(
    IReadOnlyCollection<GetDiscountResponse> Discounts,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);