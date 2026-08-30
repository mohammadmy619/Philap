using BuildingBlocks.Domain;
using Domain.BookingAggregate;
using Domain.DiscountAggregate;
using Domain.DomainServices;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Ticketing
{
    public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand, UpdateBookingResponse>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly ICalculateBookingPriceService _calculateBookingPricing;

        public UpdateBookingCommandHandler(
            IBookingRepository bookingRepository,
            IDiscountRepository discountRepository,
            ICalculateBookingPriceService calculateBookingPricing)
        {
            _bookingRepository = bookingRepository;
            _discountRepository = discountRepository;
            _calculateBookingPricing = calculateBookingPricing;
        }

        public async Task<UpdateBookingResponse> Handle(UpdateBookingCommand command, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(command.BookingId, cancellationToken)
                ?? throw new InvalidOperationException($"Booking with Id {command.BookingId} not found");

            if (booking.Status == BookingStatus.Cancelled)
            {
                throw new InvalidOperationException("Cancelled bookings cannot be updated");
            }

            Discount? discount = null;
            if (!string.IsNullOrEmpty(command.DiscountCode))
            {
                var discounts = await _discountRepository.FindDiscountsAsync(
                    d => d.Code == command.DiscountCode,
                    cancellationToken
                );

                discount = discounts.FirstOrDefault();

                if (command.DiscountCode != null && discount == null)
                    throw new InvalidOperationException("Invalid discount code");
            }

            var newPricing = _calculateBookingPricing.ProcessBookingPriceDiscount(
                new Money(command.PriceAmount, command.PriceCurrency), 
                discount,
                command.TripId,
                command.PassengerId
            );

            booking.Update(
                command.TripId,
                command.PassengerId,
                discount?.Id, 
                command.PurchaseDate,
                newPricing
            );

            await _bookingRepository.SaveChangesAsync(cancellationToken);

            return new UpdateBookingResponse(
                BookingId: booking.Id,
                TripId: booking.TripId,
                PassengerId: booking.PassengerId,
                PurchaseDate: booking.PurchaseDate,
                PriceAmount: booking.Price.Amount,
                Status: booking.Status,
                DiscountCode: discount?.Code 
            );
        }
    }
}