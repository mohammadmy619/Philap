using Domain.DiscountAggregate;
using MediatR;

public record CreateDiscountCommand(
    string Code,
    DiscountType Type,
    decimal Value,
    DateTime? ValidFrom,
    DateTime? ValidTo,
    int? MaxUsageCount,
    Guid? ApplicableTripId = null,
    Guid? ApplicablePassengerId = null
) : IRequest<Guid>;



