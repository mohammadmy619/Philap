using BuildingBlocks.Messaging.TicketEvents;
using BuildingBlocks.Messaging.TicketEvents.Implements;
using BuildingBlocks.Messaging.TicketEvents.UpdateBookingEvents;
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


        //state update
        public State UpdatingTicket { get; set; }
        public State UpdateTripSent { get; set; }
        public State UpdatePersonSent { get; set; }
        public State UpdateAccepted { get; set; }
        public State UpdateTripCancelled { get; set; }
        public State UpdatePersonCancelled { get; set; }
        public State UpdateFailed { get; set; }

        //state Cancel
        public State CancelTicket { get; set; }


        // event
        public Event<IAddTicketEvent> AddTicketEvent { get; set; }
        public Event<ISendTripEvent> SendTripEvent { get; private set; }
        public Event<ICancelTripEvent> CancelTripEvent { get; private set; }
        public Event<ISendPersonEvent> SendPersonEvent { get; private set; }
        public Event<ICancelPersonEvent> CancelPersonEvent { get; private set; }
        public Event<IAcceptTicketEvent> AcceptEvent { get; private set; }

        //event Update
        public Event<IUpdateBookingEvent> UpdateBookingEvent { get; private set; }
        public Event<IUpdateTripEvent> UpdateTripEvent { get; private set; }
        public Event<ICancelUpdateTripEvent> CancelUpdateTripEvent { get; private set; }
        public Event<IUpdatePersonEvent> UpdatePersonEvent { get; private set; }
        public Event<ICancelUpdatePersonEvent> CancelUpdatePersonEvent { get; private set; }
        public Event<IAcceptUpdateTicketEvent> AcceptUpdateTicketEvent { get; private set; }


        //event Cancel
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


            // Correlation رویدادهای Update
            Event(() => UpdateBookingEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => UpdateTripEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => CancelUpdateTripEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => UpdatePersonEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => CancelUpdatePersonEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => AcceptUpdateTicketEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));

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
                 context.Saga.IsCancelled = true;
                 context.Saga.TripCancelDate = DateTime.UtcNow;
                 context.Saga.TicketCancelDate = DateTime.UtcNow;
             })
             .TransitionTo(CancelTrip));

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
                context.Saga.IsCancelled = true;
                context.Saga.PersonCancelDate = DateTime.UtcNow;
            })
              .TransitionTo(CancelPerson)
            .Finalize());


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
            })
             .TransitionTo(Accepted)
            .Finalize());

            /// update 
            // -----------------------------------------
            // -----------------------------------------

            During(Accepted,
         When(UpdateBookingEvent)
               .Then(context =>
               {
                   _logger.LogInformation("UpdateBookingEvent received for TicketId: {TicketId}", context.Message.TicketId);

                   context.Saga.TripId = context.Message.TripId;
                   context.Saga.PassengerId = context.Message.PassengerId;
                   context.Saga.Status = "UpdatePending";
                   context.Saga.TicketUpdatedDate = DateTime.UtcNow;
               })
             .TransitionTo(UpdatingTicket)
             // ارسال رویداد برای تغییر اطلاعات در سرویس Trip
             .Publish(context => new UpdateTripEvent(
                 TicketId: context.Saga.TicketId,
                 TripId: context.Saga.TripId,
                 PassengerId: context.Saga.PassengerId,
                 Status: context.Saga.Status
             ))
            );





            During(UpdatingTicket,
             When(UpdatePersonEvent)
                 .Then(context =>
                 {
                     context.Saga.LeaderId = context.Message.LeaderId;
                     context.Saga.Status = "PersonUpdateSent";
                 })
                 .TransitionTo(UpdatePersonSent),

             When(CancelUpdateTripEvent)
                 .Then(context =>
                 {
                     context.Saga.Status = "UpdateTripFailed";
                     context.Saga.IsCancelled = true;
                 })
                 .TransitionTo(UpdateTripCancelled)
               );


            During(UpdatePersonSent,
                When(AcceptUpdateTicketEvent)
                    .Then(context =>
                    {
                        context.Saga.Status = "UpdateAccepted";
                        context.Saga.CurrentState = "Accepted";
                    })
                    .TransitionTo(Accepted),

                When(CancelUpdatePersonEvent)
                    .Then(context =>
                    {
                        context.Saga.Status = "UpdatePersonFailed";
                    })
                    .TransitionTo(UpdatePersonCancelled)
              );











        }











    }
}
