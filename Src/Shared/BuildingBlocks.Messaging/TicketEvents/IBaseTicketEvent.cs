using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.TicketEvents
{
    public interface IBaseTicketEvent: IIntegrationEvent
    {
        public Guid CorrelationId { get; }
        public Guid TicketId { get; }
        public Guid TripId { get; }
        public Guid PassengerId { get; }
        public string CurrentState { get;  }
        public DateTime TicketCreatedDate { get; }
        public DateTime TicketCancelDate { get;}
        public string? Status { get; }

  

    }
}
