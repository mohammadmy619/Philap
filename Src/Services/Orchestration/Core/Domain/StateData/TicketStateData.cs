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
        public Guid TripId { get; private set; }
        public Guid PassengerId { get; private set; }
        public string? Status { get; set; }
    }
}
