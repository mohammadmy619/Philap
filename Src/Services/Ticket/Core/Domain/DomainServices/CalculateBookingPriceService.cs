using BuildingBlocks.Exeptions;
using Domain.BookingAggregate;
using Domain.DiscountAggregate;
using System.ComponentModel.DataAnnotations;

namespace Domain.DomainServices
{
    public class CalculateBookingPriceService : ICalculateBookingPriceService
    {
        /// <summary>
        /// محاسبه مبالغ پایه، تخفیف و نهایی
        /// </summary>
        /// <param name="booking">رزرو مورد نظر</param>
        /// <param name="discount">تخفیف اعمال شده (می‌تواند null باشد)</param>
        /// <returns>نتیجه محاسبات شامل هر سه مبلغ</returns>
        public Money ProcessBookingPriceDiscount(Money money, Discount? discount, Guid tripId,Guid passengerId)
        {

            decimal baseAmountValue = money.Amount;
            decimal finalAmountValue = baseAmountValue;

            GuardAgainstFinalAmount(finalAmountValue, baseAmountValue);

            //decimal discountAmountValue = baseAmountValue - finalAmountValue;
            decimal discountAmount=0;
            if (discount is not null)
            {
                 discountAmount = discount.ApplyDiscount(money.Amount, tripId, passengerId);

            }

            return new Money(baseAmountValue, discountAmount, money.Currency);
        }



        private void GuardAgainstFinalAmount(decimal finalAmount, decimal baseAmount)
        {
            if (finalAmount < 0)
            {
                throw new AmountIsInvalidException("Final amount cannot be negative.");
            }

            if (finalAmount > baseAmount)
            {
                throw new AmountIsInvalidException("Final amount cannot be greater than base amount.");
            }
        }
    }
}
