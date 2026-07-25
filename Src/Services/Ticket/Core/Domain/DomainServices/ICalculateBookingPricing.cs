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
    public interface ICalculateBookingPricing
    {
       PricingResult ProcessBookingPriceDiscount(Booking booking, Discount? discount);

       //string GetCurrency(Money price);

    }
}

