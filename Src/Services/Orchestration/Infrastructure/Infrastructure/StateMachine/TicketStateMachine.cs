using BuildingBlocks.Messaging.TicketEvents;
using BuildingBlocks.Messaging.TicketEvents.Implements;
using Domain.StateData;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.StateMachine
{
    public class TicketStateMachine : MassTransitStateMachine<TicketStateData>
    {


        private readonly ILogger<TicketStateMachine> _logger;
        // state
        public State AddTicket { get; set; }
        public State SendTrip { get; set; }
        public State SendPerson { get; set; }
        public State CancelTrip { get; set; }
        public State CancelPerson { get; set; }
        public State Accepted { get; set; }
        public State CancelTicket { get; set; }


        // event
        public Event<IAddTicketEvent> AddTicketEvent { get; set; }
        public Event<ISendTripEvent> SendTripEvent { get; private set; }
        public Event<ICancelTripEvent> CancelTripEvent { get; private set; }
        public Event<ISendPersonEvent> SendPersonEvent { get; private set; }
        public Event<ICancelPersonEvent> CancelPersonEvent { get; private set; }
        public Event<IAcceptTicketEvent> AcceptEvent { get; private set; }
        public Event<ICancelTicketEvent> CancelTicketEvent { get; private set; }

        public TicketStateMachine(ILogger<TicketStateMachine> logger)
        {

            _logger = logger;

            InstanceState(x => x.CurrentState);

            // تعریف Event ها و نحوه correlation با saga instance
            Event(() => AddTicketEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => SendTripEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => CancelTripEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => SendPersonEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => CancelPersonEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => AcceptEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));


            During(Initial,
             When(AddTicketEvent)
                 .Then(context =>
                 {
                     // ⭐ لاگ ورود Event
                     _logger.LogInformation(
                         " AddTicketEvent received. TicketId: {TicketId}, TripId: {TripId}, PassengerId: {PassengerId}",
                         context.Message.TicketId,
                         context.Message.TripId,
                         context.Message.PassengerId);

                     context.Saga.TicketId = context.Message.TicketId;
                     context.Saga.TripId = context.Message.TripId;
                     context.Saga.PassengerId = context.Message.PassengerId;
                     context.Saga.Status = "Pending";
                     context.Saga.TicketCreatedDate = DateTime.UtcNow;
                     context.Saga.TicketCancelDate = DateTime.MinValue;

                     _logger.LogInformation(
                         "Saga data saved. CorrelationId: {CorrelationId}, Status: {Status}",
                         context.Saga.CorrelationId,
                         context.Saga.Status);
                 })
                 .TransitionTo(AddTicket)
                 .Then(context =>
                 {
                     _logger.LogInformation(
                         " Transitioned to AddTicket state. TicketId: {TicketId}",
                         context.Saga.TicketId);
                 })
                 .Publish(context => new SendTripEvent(
                     TicketId: context.Saga.TicketId,
                     TripId: context.Saga.TripId,
                     PassengerId: context.Saga.PassengerId,
                     CurrentState: "AddTicket",
                     TicketCreatedDate: context.Saga.TicketCreatedDate,
                     TicketCancelDate: context.Saga.TicketCancelDate,
                     Status: context.Saga.Status
                 ))
                 .Then(context =>
                 {
                     _logger.LogInformation(
                         " SendTripEvent published. TicketId: {TicketId}",
                         context.Saga.TicketId);
                 })
             );




            //--------------------------------
            During(AddTicket,
                When(SendPersonEvent)
                .Then(context =>
                {
                    context.Saga.CorrelationId = context.Message.CorrelationId;
                    context.Saga.TicketId = context.Message.TicketId;
                    context.Saga.TripId = context.Message.TripId;
                    context.Saga.PassengerId = context.Message.PassengerId;
                    context.Saga.CurrentState = "AddTicket";
                    context.Saga.Status = "PersonSent";
                    context.Saga.TicketCreatedDate = context.Message.TicketCreatedDate;
                    context.Saga.LeaderId = context.Message.LeaderId;
                }
                ).TransitionTo(SendPerson));





            During(AddTicket,
             When(CancelTripEvent)
             .Then(context =>
             {
                 context.Saga.CorrelationId = context.Message.CorrelationId;
                 context.Saga.TicketId = context.Message.TicketId;
                 context.Saga.TripId = context.Message.TripId;
                 context.Saga.PassengerId = context.Message.PassengerId;
                 context.Saga.CurrentState = "AddTicket";
                 context.Saga.Status = "TripCancelled";
                 context.Saga.TicketCreatedDate = context.Message.TicketCreatedDate;
                 context.Saga.TicketCancelDate = DateTime.Now;
             })
             .TransitionTo(CancelTicket));

            //-------------------------SendPerson

            During(SendPerson,
            When(CancelTripEvent)
            .Then(context =>
            {
                context.Saga.CorrelationId = context.Message.CorrelationId;
                context.Saga.TicketId = context.Message.TicketId;
                context.Saga.TripId = context.Message.TripId;
                context.Saga.PassengerId = context.Message.PassengerId;
                context.Saga.CurrentState = "CancelTripEvent";
                context.Saga.Status = "TripCancelled";
                context.Saga.TicketCreatedDate = context.Message.TicketCreatedDate;
                context.Saga.TicketCancelDate = DateTime.Now;
            })
            .TransitionTo(CancelTicket));


            //Final
            During(SendPerson,
            When(AcceptEvent)
            .Then(context =>
            {
                context.Saga.CorrelationId = context.Message.CorrelationId;
                context.Saga.TicketId = context.Message.TicketId;
                context.Saga.TripId = context.Message.TripId;
                context.Saga.PassengerId = context.Message.PassengerId;
                context.Saga.CurrentState = "AcceptEvent";
                context.Saga.Status = "AcceptTriped";
                context.Saga.TicketCreatedDate = context.Message.TicketCreatedDate;
                context.Saga.TicketCancelDate = DateTime.Now;
            })
            .TransitionTo(CancelTicket));



              
              
        }
              










    }
}
