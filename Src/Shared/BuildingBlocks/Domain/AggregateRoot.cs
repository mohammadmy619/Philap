namespace BuildingBlocks.Domain;
public abstract class AggregateRoot<TId> :  IAggregateRoot
    where TId : notnull
{
    public TId Id { get; protected set; }
    public IReadOnlyCollection<IDomainEvent> Events => [.. _events];

    private readonly List<IDomainEvent> _events;

    protected AggregateRoot() 
    {
        _events = [];
    }

    public void ClearEvents() => _events.Clear();

    protected void AddEvent<TDomainEvent>(TDomainEvent @event)
        where TDomainEvent : IDomainEvent => _events.Add(@event);
}
