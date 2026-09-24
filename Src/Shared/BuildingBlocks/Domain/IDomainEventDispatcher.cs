

namespace BuildingBlocks.Domain; // یا Domain.Events

public interface IDomainEventDispatcher
{
    Task DispatchEventsAsync(CancellationToken cancellationToken = default);
}