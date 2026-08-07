using Domain.AccountingAggregate;
using Domain.BookingAggregate;
using Domain.TripAggregate.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence.Repositories;

namespace Application.Accountings.EventHandlers.CreateAccount
{
    public class CreateAccountHandler : INotificationHandler<BookingCreatedDomainEvent>
    {
        private readonly IAccountingRepository _accountingRepository;
        private readonly ILogger<CreateAccountHandler> _logger;

        public CreateAccountHandler(
            IAccountingRepository accountingRepository,
            ILogger<CreateAccountHandler> logger)
        {
            _accountingRepository = accountingRepository;
            _logger = logger;
        }

        public async Task Handle(BookingCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(
                    "Creating accounting record for BookingId: {BookingId}, TripId: {TripId}, PassengerId: {PassengerId}",
                    notification.BookingId,
                    notification.TripId,
                    notification.PassengerId);

                // بررسی تکراری نبودن (Idempotency) - آیا قبلاً برای این Booking، Accounting ثبت شده؟
                var existingAccountings = await _accountingRepository.FindAccountingsAsync(
                    a => a.BookingId == notification.BookingId,
                    cancellationToken);

                if (existingAccountings.Any())
                {
                    _logger.LogWarning(
                        "Accounting record already exists for BookingId: {BookingId}. Skipping creation.",
                        notification.BookingId);
                    return;
                }

                // ساخت Money از PriceAmount موجود در Event
                // اگر تخفیف اعمال شده باشد، PriceAmount در Event، مقدار نهایی است
                // برای سادگی فرض می‌کنیم PriceAmount در Event، همان Amount پایه است و DiscountAmount=0
                // اگر می‌خواهی Discount را هم لحاظ کنی، می‌توانی از DiscountId استفاده کنی
   
                // ایجاد رکورد Accounting جدید
                var accounting = new Accounting(
                    bookingId: notification.BookingId,
                    tripId: notification.TripId,
                    passengerId: notification.PassengerId,
                    entryDate: DateTime.UtcNow,
                    purchaseDate: notification.PurchaseDate,
                    price: notification.Price,
                    paymentStatus: PaymentStatus.Pending, // هنوز پرداخت انجام نشده
                    description: $"Accounting record created from BookingCreatedDomainEvent (EventId: {notification.Id})"
                );

                // ذخیره در دیتابیس
                await _accountingRepository.AddAccountingAsync(accounting, cancellationToken);
                await _accountingRepository.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Accounting record created successfully. AccountingId: {AccountingId}, BookingId: {BookingId}, Amount: {Amount} {Currency}",
                    accounting.Id,
                    accounting.BookingId,
                    accounting.Price.FinalAmount,
                    accounting.Price.Currency);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error creating accounting record for BookingId: {BookingId}",
                    notification.BookingId);
                throw;
            }
        }


    }
}