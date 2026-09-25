//using BuildingBlocks.Messaging.TicketEvents;
//using MassTransit;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure
//{
//    public class AddTicketEventTestConsumer : IConsumer<IAddTicketEvent>
//    {
//        private readonly ILogger<AddTicketEventTestConsumer> _logger;

//        public AddTicketEventTestConsumer(ILogger<AddTicketEventTestConsumer> logger)
//        {
//            _logger = logger;
//        }

//        public Task Consume(ConsumeContext<IAddTicketEvent> context)
//        {
//            _logger.LogWarning(">>> RECEIVED AddTicketEvent | TicketId: {TicketId}", context.Message.TicketId);
//            return Task.CompletedTask;
//        }
//    }
//}
