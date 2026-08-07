using Application.Discounts.GetDiscount;
using MediatR;

public record GetDiscountByIdQuery(Guid DiscountId) : IRequest<GetDiscountResponse>;
