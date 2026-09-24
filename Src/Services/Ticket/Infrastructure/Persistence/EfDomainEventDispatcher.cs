using BuildingBlocks.Domain;
using MediatR;

namespace Persistence.Services;

public class EfDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly TicketingDbContext _dbContext;
    private readonly IPublisher _publisher;

    public EfDomainEventDispatcher(TicketingDbContext dbContext, IPublisher publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
    }

    public async Task DispatchEventsAsync(CancellationToken cancellationToken = default)
    {
        var aggregates = _dbContext.ChangeTracker
           .Entries<IAggregateRoot>()
           .Select(x => x.Entity)
           .Where(x => x.Events.Any())
           .ToList();


        foreach (var aggregate in aggregates)
        {
            var events = aggregate.Events.ToList();
            aggregate.ClearEvents();
            foreach (var domainEvent in events)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }
        }
    }
}
