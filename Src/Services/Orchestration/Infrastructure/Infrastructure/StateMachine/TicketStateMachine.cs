using BuildingBlocks.Messaging.TicketEvents;
using BuildingBlocks.Messaging.TicketEvents.CancelTicketEvents;
using BuildingBlocks.Messaging.TicketEvents.Implements;
using BuildingBlocks.Messaging.TicketEvents.UpdateBookingEvents;
using Domain.StateData;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;

namespace Infrastructure.StateMachine
{
    public class TicketStateMachine : MassTransitStateMachine<TicketStateData>
    {
        private readonly ILogger<TicketStateMachine> _logger;

        // =====================================================================
        // States
        // =====================================================================
        // Creation States
        public State AddTicket { get; set; }
        public State SendTrip { get; set; }
        public State SendPerson { get; set; }
        public State CancelTrip { get; set; }
        public State CancelPerson { get; set; }
        public State Accepted { get; set; }

        // Update States
        public State UpdatingTicket { get; set; }
        public State UpdateTripSent { get; set; }
        public State UpdatePersonSent { get; set; }
        public State UpdateTripCancelled { get; set; }
        public State UpdatePersonCancelled { get; set; }
        public State UpdateFailed { get; set; }

        // Cancel States
        public State CancelTicket { get; set; }
        public State CancellingTrip { get; set; }
        public State CancellingPerson { get; set; }
        public State TicketCancelled { get; set; }
        public State TicketCancelFailed { get; set; }

        // =====================================================================
        // Events
        // =====================================================================
        // Creation Events
        public Event<IAddTicketEvent> AddTicketEvent { get; set; }
        public Event<ISendTripEvent> SendTripEvent { get; private set; }
        public Event<ICancelTripEvent> CancelTripEvent { get; private set; }
        public Event<ISendPersonEvent> SendPersonEvent { get; private set; }
        public Event<ICancelPersonEvent> CancelPersonEvent { get; private set; }
        public Event<IAcceptTicketEvent> AcceptEvent { get; private set; }

        // Update Events
        public Event<IUpdateBookingEvent> UpdateBookingEvent { get; private set; }
        public Event<IUpdateTripEvent> UpdateTripEvent { get; private set; }
        public Event<ICancelUpdateTripEvent> CancelUpdateTripEvent { get; private set; }
        public Event<IUpdatePersonEvent> UpdatePersonEvent { get; private set; }
        public Event<ICancelUpdatePersonEvent> CancelUpdatePersonEvent { get; private set; }
        public Event<IAcceptUpdateTicketEvent> AcceptUpdateTicketEvent { get; private set; }

        // Cancel Events
        public Event<ICancellationTicketEvent> CancellationTicketEvent { get; private set; }
        public Event<ITripCancellationEvent> TripCancellationEvent { get; private set; }
        public Event<ITripCancellationFailedEvent> TripCancellationFailedEvent { get; private set; }
        public Event<IPersonCancellationEvent> PersonCancellationEvent { get; private set; }
        public Event<IPersonCancellationFailedEvent> PersonCancellationFailedEvent { get; private set; }
        public Event<ICancellationConfirmedEvent> CancellationConfirmedEvent { get; private set; }

        public TicketStateMachine(ILogger<TicketStateMachine> logger)
        {
            _logger = logger;

            InstanceState(x => x.CurrentState);

            // =====================================================================
            // Correlation Configuration
            // =====================================================================
            // Creation Correlation
            Event(() => AddTicketEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => SendTripEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => CancelTripEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => SendPersonEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => CancelPersonEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => AcceptEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));

            // Update Correlation
            Event(() => UpdateBookingEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => UpdateTripEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => CancelUpdateTripEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => UpdatePersonEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => CancelUpdatePersonEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => AcceptUpdateTicketEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));

            // Cancel Correlation
            Event(() => CancellationTicketEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => TripCancellationEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => TripCancellationFailedEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => PersonCancellationEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => PersonCancellationFailedEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));
            Event(() => CancellationConfirmedEvent, e => e.CorrelateById(ctx => ctx.Message.TicketId));

            // =====================================================================
            // 1. Ticket Creation Flow
            // =====================================================================
            During(Initial,
                When(AddTicketEvent)
                    .Then(context =>
                    {
                        _logger.LogInformation(
                            "AddTicketEvent received. TicketId: {TicketId}, TripId: {TripId}, PassengerId: {PassengerId}",
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
                            "Transitioned to AddTicket state. TicketId: {TicketId}",
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
                            "SendTripEvent published. TicketId: {TicketId}",
                            context.Saga.TicketId);
                    })
            );

            During(AddTicket,
                When(SendPersonEvent)
                    .Then(context =>
                    {
                        context.Saga.TicketId = context.Message.TicketId;
                        context.Saga.TripId = context.Message.TripId;
                        context.Saga.PassengerId = context.Message.PassengerId;
                        context.Saga.Status = "PersonSent";
                        context.Saga.TicketCreatedDate = context.Message.TicketCreatedDate;
                        context.Saga.LeaderId = context.Message.LeaderId;
                    })
                    .TransitionTo(SendPerson),

                When(CancelTripEvent)
                    .Then(context =>
                    {
                        context.Saga.TicketId = context.Message.TicketId;
                        context.Saga.TripId = context.Message.TripId;
                        context.Saga.PassengerId = context.Message.PassengerId;
                        context.Saga.Status = "TripCancelled";
                        context.Saga.TicketCreatedDate = context.Message.TicketCreatedDate;
                        context.Saga.IsCancelled = true;
                        context.Saga.TripCancelDate = DateTime.UtcNow;
                        context.Saga.TicketCancelDate = DateTime.UtcNow;
                    })
                    .TransitionTo(CancelTrip)
                    .Finalize()
            );

            During(SendPerson,
                When(AcceptEvent)
                    .Then(context =>
                    {
                        context.Saga.TicketId = context.Message.TicketId;
                        context.Saga.TripId = context.Message.TripId;
                        context.Saga.PassengerId = context.Message.PassengerId;
                        context.Saga.Status = "Accepted";
                        context.Saga.TicketCreatedDate = context.Message.TicketCreatedDate;
                        _logger.LogInformation("Ticket {TicketId} has been successfully accepted.", context.Saga.TicketId);
                    })
                    .TransitionTo(Accepted),

                When(CancelTripEvent)
                    .Then(context =>
                    {
                        context.Saga.TicketId = context.Message.TicketId;
                        context.Saga.TripId = context.Message.TripId;
                        context.Saga.PassengerId = context.Message.PassengerId;
                        context.Saga.Status = "TripCancelled";
                        context.Saga.TicketCreatedDate = context.Message.TicketCreatedDate;
                        context.Saga.TicketCancelDate = DateTime.UtcNow;
                        context.Saga.IsCancelled = true;
                        context.Saga.PersonCancelDate = DateTime.UtcNow;
                    })
                    .TransitionTo(CancelPerson)
                    .Finalize()
            );

            // =====================================================================
            // 2. Ticket Update Flow
            // =====================================================================
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
                    .Publish(context => new UpdateTripEvent(
                        ticketId: context.Saga.TicketId,
                        tripId: context.Saga.TripId,
                        passengerId: context.Saga.PassengerId,
                        currentState: "UpdatingTicket",
                        ticketCreatedDate: context.Saga.TicketCreatedDate,
                        ticketCancelDate: context.Saga.TicketCancelDate,
                        status: context.Saga.Status,
                        ticketUpdatedDate: DateTime.UtcNow
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
                        context.Saga.Status = "Accepted";
                    })
                    .TransitionTo(Accepted),

                When(CancelUpdatePersonEvent)
                    .Then(context =>
                    {
                        context.Saga.Status = "UpdatePersonFailed";
                    })
                    .TransitionTo(UpdatePersonCancelled)
            );

            // =====================================================================
            // 3. Ticket Cancellation Flow
            // =====================================================================
            During(Accepted,
                When(CancellationTicketEvent)
                    .Then(context =>
                    {
                        _logger.LogInformation("CancelTicketEvent initiated for TicketId: {TicketId}", context.Message.TicketId);
                        context.Saga.Status = "Cancelling";
                        context.Saga.TicketCancelDate = DateTime.UtcNow;
                        context.Saga.IsCancelled = false;
                    })
                    .TransitionTo(CancellingTrip)
                    .Publish(context => new TripCancellationEvent(
                        ticketId: context.Saga.TicketId,
                        tripId: context.Saga.TripId,
                        passengerId: context.Saga.PassengerId,
                        currentState: "CancellingTrip",
                        ticketCreatedDate: context.Saga.TicketCreatedDate,
                        status: "Cancelling",
                        cancelledAt: context.Saga.TicketCancelDate
                    ))
            );

            During(CancellingTrip,
                When(TripCancellationFailedEvent)
                    .Then(context =>
                    {
                        _logger.LogError("Trip cancellation failed | TicketId: {TicketId}, TripId: {TripId}, Reason: {Reason}",
                            context.Message.TicketId, context.Message.TripId, context.Message.Reason);

                        context.Saga.Status = "TripCancellationFailed";
                        context.Saga.IsCancelled = false;
                    })
                    .TransitionTo(TicketCancelFailed)
                   ,

                When(PersonCancellationEvent)
                    .Then(context =>
                    {
                        _logger.LogInformation("Trip cancelled successfully, initiating Person cancellation | TicketId: {TicketId}", context.Message.TicketId);
                        context.Saga.Status = "CancellingPerson";
                        context.Saga.TripCancelDate = DateTime.UtcNow;
                    })
                    .TransitionTo(CancellingPerson)
            );

            During(CancellingPerson,
                When(CancellationConfirmedEvent)
                    .Then(context =>
                    {
                        _logger.LogInformation("Ticket {TicketId} fully cancelled successfully.", context.Saga.TicketId);
                        context.Saga.Status = "Cancelled";
                        context.Saga.IsCancelled = true;
                        context.Saga.PersonCancelDate = DateTime.UtcNow;
                    })
                    .TransitionTo(TicketCancelled)
                    .Finalize(),

                When(PersonCancellationFailedEvent)
                    .Then(context =>
                    {
                        _logger.LogError("Person cancellation failed | TicketId: {TicketId}, Reason: {Reason}",
                            context.Message.TicketId, context.Message.Reason);

                        context.Saga.Status = "PersonCancellationFailed";
                        context.Saga.IsCancelled = false;
                    })
                    .TransitionTo(TicketCancelFailed)
                    
            );
        }
    }
}
