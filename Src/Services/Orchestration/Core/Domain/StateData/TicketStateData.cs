using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.StateData
{
    public class TicketStateData : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public string CurrentState { get; set; }
        public DateTime TicketCreatedDate { get; set; }
        public DateTime TicketCancelDate { get; set; }
        public Guid TicketId { get; set; }
        public Guid TripId { get;  set; }
        public Guid? LeaderId { get;  set; }
        public Guid PassengerId { get;  set; }
        public string? Status { get; set; }


        public decimal PriceAmount { get; set; }
        public string PriceCurrency { get; set; } = string.Empty;
        public DateTime? TripCancelDate { get; set; }
        public DateTime? PersonCancelDate { get; set; }
        public DateTime? TripSentDate { get; set; }
        public DateTime? AcceptedDate { get; set; }
        // وضعیت‌ها
        public bool IsCancelled { get; set; }
        public string? CancelReason { get; set; }


        // ✨ فیلدهای جدید برای پشتیبانی از رویداد آپدیت
        public decimal PriceAmountUpdate { get; set; }
        public string PriceCurrencyUpdate { get; set; }
        public string PreviousState { get; set; }
        public DateTime TicketUpdatedDate { get; set; }

        
        public Guid? CancellationTimeoutTokenId { get; set; }
    }
}
