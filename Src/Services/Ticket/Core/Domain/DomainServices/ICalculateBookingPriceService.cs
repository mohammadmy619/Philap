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
    public interface ICalculateBookingPriceService
    {
        Money ProcessBookingPriceDiscount(Money money, Discount? discount,Guid tripId,Guid passengerId);


    }
}

