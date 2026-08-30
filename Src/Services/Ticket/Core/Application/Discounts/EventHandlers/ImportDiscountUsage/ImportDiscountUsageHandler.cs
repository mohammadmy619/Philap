using Domain.DiscountAggregate;
using Domain.TripAggregate.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence.Repositories;

namespace Application.Handlers.Events
{
    public class ImportDiscountUsageHandler : INotificationHandler<BookingCreatedEvent>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<ImportDiscountUsageHandler> _logger;

        public ImportDiscountUsageHandler(
            IDiscountRepository discountRepository,
            ILogger<ImportDiscountUsageHandler> logger)
        {
            _discountRepository = discountRepository;
            _logger = logger;
        }

        public async Task Handle(BookingCreatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(
                    "Processing discount usage for BookingId: {BookingId}, TripId: {TripId}, PassengerId: {PassengerId}",
                    notification.Id,
                    notification.TripId,
                    notification.PassengerId);

                // اگر DiscountId در Event وجود دارد
                if (notification.DiscountId.HasValue && notification.DiscountId.Value != Guid.Empty)
                {
                    // بررسی تکراری نبودن (Idempotency)
                    var alreadyUsed = await _discountRepository.HasUsageForBookingAsync(
                        notification.DiscountId.Value,
                        notification.Id,
                        cancellationToken);

                    if (alreadyUsed)
                    {
                        _logger.LogWarning(
                            "Discount {DiscountId} already used for BookingId: {BookingId}. Skipping.",
                            notification.DiscountId.Value,
                            notification.Id);
                        return;
                    }

                    var discount = await _discountRepository.GetDiscountByIdAsync(
                        notification.DiscountId.Value,
                        cancellationToken);

                    if (discount is null)
                    {
                        _logger.LogWarning(
                            "Discount with Id {DiscountId} not found for BookingId: {BookingId}",
                            notification.DiscountId.Value,
                            notification.Id);
                        return;
                    }

                    // محاسبه مبلغ تخفیف اعمال‌شده
                    var appliedAmount = discount.ApplyDiscount(
                        notification.Price.Amount, // فرض بر این است که در Event مقدار قیمت وجود دارد
                        notification.TripId,
                        notification.PassengerId);

                    var originalAmount = notification.Price.Amount;
                    var discountAmount = originalAmount - appliedAmount;

                    // ایجاد رکورد DiscountUsage
                    var usage = new DiscountUsage(
                        discountId: discount.Id,
                        bookingId: notification.Id,
                        passengerId: notification.PassengerId,
                        tripId: notification.TripId,
                        appliedAmount: discountAmount);

                    // ثبت استفاده در Discount
                    discount.RecordUsage();

                    // ذخیره
                    await _discountRepository.AddDiscountUsageAsync(usage, cancellationToken);
                    await _discountRepository.UpdateDiscountAsync(discount, cancellationToken);
                    await _discountRepository.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation(
                        "Discount usage recorded. DiscountId: {DiscountId}, BookingId: {BookingId}, AppliedAmount: {AppliedAmount}, UsedCount: {UsedCount}",
                        discount.Id,
                        notification.Id,
                        discountAmount,
                        discount.UsedCount);
                }
                else
                {
                    // اگر DiscountId نداریم، تخفیف‌های قابل اعمال را پیدا می‌کنیم
                    var applicableDiscounts = await _discountRepository.FindDiscountsAsync(
                        d => d.IsActive &&
                             (d.ApplicableTripId == null || d.ApplicableTripId == notification.TripId) &&
                             (d.ApplicablePassengerId == null || d.ApplicablePassengerId == notification.PassengerId) &&
                             (d.ValidFrom == null || d.ValidFrom <= DateTime.UtcNow) &&
                             (d.ValidTo == null || d.ValidTo >= DateTime.UtcNow) &&
                             (d.MaxUsageCount == null || d.UsedCount < d.MaxUsageCount.Value),
                        cancellationToken);

                    foreach (var discount in applicableDiscounts)
                    {
                        discount.RecordUsage();
                        await _discountRepository.UpdateDiscountAsync(discount, cancellationToken);
                    }

                    if (applicableDiscounts.Any())
                    {
                        await _discountRepository.SaveChangesAsync(cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error processing discount usage for BookingId: {BookingId}",
                    notification.Id);
                throw;
            }
        }
    }
}