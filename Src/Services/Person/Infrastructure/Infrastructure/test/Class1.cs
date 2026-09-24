using BuildingBlocks.Messaging.TicketEvents;
using BuildingBlocks.Messaging.TicketEvents.Implements;
using MassTransit;


namespace Infrastructure.test
{
    internal class Class1 : IConsumer<IAddTicketEvent>
    {
        public Task Consume(ConsumeContext<IAddTicketEvent> context)
        {
            throw new NotImplementedException();
        }
    }
    internal class Class2: IConsumer<AddTicketEvent>
    {
        public Task Consume(ConsumeContext<AddTicketEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
