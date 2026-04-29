
using MediatR;


    public record UpdateDiscountCommand(
      Guid Id,
      string Code,
      decimal Value,
      DateTime? ValidFrom,
      DateTime? ValidTo,
      int? MaxUsageCount,
      Guid? ApplicableTripId = null,
      Guid? ApplicablePassengerId = null
  ) : IRequest<DiscountUpdatedResponse>;
