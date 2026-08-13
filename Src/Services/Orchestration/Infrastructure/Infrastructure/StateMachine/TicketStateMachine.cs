using BuildingBlocks.Messaging.TicketEvents;
using Domain.StateData;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.StateMachine
{
    public class TicketStateMachine : MassTransitStateMachine<TicketStateData>
    {
        // 5 state
        public State AddTicket { get; set; }
        public State CancelTicket { get; set; }
        public State CancelTrip { get; set; }
        public State CancelPerson { get; set; }
        public State SendTrip { get; set; }
        public State Accepted { get; set; }


        // 5 event
        public Event<IAddTicketEvent> AddTicketEvent { get; set; }
        public Event<ICancelGenerateTicketEvent> CancelGenerateTicketEvent { get; set; }
        public Event<ICancelTripEvent> CancelTripEvent { get; private set; }
        public Event<ICancelPersonEvent> CancelPersonEvent { get; private set; }
        public Event<ISendEmailEvent> SendTripEvent { get; private set; }
        public Event<IAcceptTicketEvent> AcceptEvent { get; private set; }


    }
}
