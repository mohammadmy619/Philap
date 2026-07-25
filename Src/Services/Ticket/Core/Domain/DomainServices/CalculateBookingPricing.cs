using BuildingBlocks.Domain;
using Domain.BookingAggregate;
using Domain.DiscountAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainServices
{
    public class CalculateBookingPricing : ICalculateBookingPricing
    {
        /// <summary>
        /// محاسبه مبالغ پایه، تخفیف و نهایی
        /// </summary>
        /// <param name="booking">رزرو مورد نظر</param>
        /// <param name="discount">تخفیف اعمال شده (می‌تواند null باشد)</param>
        /// <returns>نتیجه محاسبات شامل هر سه مبلغ</returns>
        public PricingResult ProcessBookingPriceDiscount(Booking booking, Discount? discount)
        {
            // 1. دریافت مبلغ پایه از رزرو
            decimal baseAmountValue = booking.Price.Amount;
            decimal finalAmountValue = baseAmountValue;

            // 2. اعمال منطق تخفیف در صورت وجود (فقط محاسبه، بدون تغییر وضعیت تخفیف)
            if (discount != null)
            {
                // متد ApplyDiscount مقدار نهایی پس از کسر تخفیف را برمی‌گرداند
                finalAmountValue = discount.ApplyDiscount(baseAmountValue, booking.TripId, booking.PassengerId);
            }

            // 3. محاسبه مبلغ تخفیف (اختلاف مبلغ پایه و نهایی)
            decimal discountAmountValue = baseAmountValue - finalAmountValue;

            // 4. تبدیل به Value Object های Money 
            // نکته: اگر سازنده کلاس Money شما فقط یک پارامتر decimal می‌گیرد، 
            // آن را به new Money(baseAmountValue) تغییر دهید.
            string currency = GetCurrency(booking.Price);

            var baseAmount = new Money(baseAmountValue, currency);
            var discountAmount = new Money(discountAmountValue, currency);
            var finalAmount = new Money(finalAmountValue, currency);

            return new PricingResult(baseAmount, discountAmount, finalAmount);
        }

        private string GetCurrency(Money price)
        {
            // اگر کلاس Money پراپرتی Currency دارد: return price.Currency;
            return "IRR"; // مقدار پیش‌فرض
        }

       
    }
}
