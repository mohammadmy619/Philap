using BuildingBlocks.Domain;
using MediatR;
using Persistence;

public sealed class DomainEventDispatchingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IPublisher _publisher;
    private readonly TicketingDbContext _dbContext;
    public DomainEventDispatchingBehavior(IPublisher publisher, TicketingDbContext dbContext)
    {
        _publisher = publisher;
        _dbContext = dbContext;
    }
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Execute the command handler
        var response = await next();
        // Collect events from tracked aggregates
        var aggregates = _dbContext.ChangeTracker
            .Entries<AggregateRoot<object>>()
            .Where(e => e.Entity.Events.Any())
            .Select(e => e.Entity)
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
        return response;
    }
}