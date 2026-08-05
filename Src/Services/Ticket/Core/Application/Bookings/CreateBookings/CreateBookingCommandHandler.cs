using BuildingBlocks.Domain;
using Domain.BookingAggregate;
using Domain.DiscountAggregate;
using Domain.DomainServices;
using MediatR;

namespace Application.Ticketing
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, CreateBookingResponse>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICalculateBookingPriceService _calculateBookingPricing;
        private readonly IDiscountRepository _DiscountRepository;

        public CreateBookingCommandHandler(IBookingRepository bookingRepository, IDiscountRepository discountRepository, ICalculateBookingPriceService calculateBookingPricing)
        {
            _bookingRepository = bookingRepository;
            _calculateBookingPricing = calculateBookingPricing;
            _DiscountRepository = discountRepository;
        }

        public async Task<CreateBookingResponse> Handle(CreateBookingCommand command, CancellationToken cancellationToken)
        {




            // 1) تخفیف را پیدا کن (بسته به طراحی شما)
            Discount? discount = null;
            if (command.DiscountCode is not null)
            {
                // استفاده از متد جنریک موجود با استفاده از Expression
                var discounts = await _DiscountRepository.FindDiscountsAsync(
                    d => d.Code == command.DiscountCode,
                    cancellationToken
                );

                // گرفتن اولین نتیجه یا null اگر پیدا نشد
                discount = discounts.FirstOrDefault();
            }

            // 2) محاسبه قیمت‌ها
            var pricing = _calculateBookingPricing.ProcessBookingPriceDiscount(new Money(command.PriceAmount, command.PriceCurrency), discount, command.TripId, command.PassengerId);



            var booking = new Booking(
            tripId: command.TripId,
            passengerId: command.PassengerId,
            purchaseDate: command.PurchaseDate,
            price: pricing
        );

            await _bookingRepository.AddBookingAsync(booking, cancellationToken);
            await _bookingRepository.SaveChangesAsync(cancellationToken);

            return new CreateBookingResponse(
                BookingId: booking.Id,
                TripId: booking.TripId,
                PassengerId: booking.PassengerId,
                PurchaseDate: booking.PurchaseDate,
                PriceAmount: booking.Price.Amount);

        }
    }


}
