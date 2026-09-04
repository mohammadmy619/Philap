using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.TicketEvents.CancelTicketEvents
{
    public record CancellationTimeoutExpired(Guid TicketId);
}
