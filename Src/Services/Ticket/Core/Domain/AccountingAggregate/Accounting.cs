using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Domain;

namespace Domain.AccountingAggregate
{
    public class Accounting:AggregateRoot<Guid>
    {

        #region Properties
        public Guid BookingId { get; private set; }
        public Guid TripId { get; private set; }
        public Guid PassengerId { get; private set; }

        public DateTime EntryDate { get; private set; }
        public DateTime PurchaseDate { get; private set; }

        public Money BaseAmount { get; private set; }
        public Money DiscountAmount { get; private set; }
        public Money FinalAmount { get; private set; }
        public PaymentStatus PaymentStatus { get; private set; }

        public string Description { get; private set; }
        #endregion


    }
}
