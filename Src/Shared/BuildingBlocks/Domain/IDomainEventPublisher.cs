
namespace BuildingBlocks.Domain
{
    public interface IDomainEventPublisher
    {
        Task Publish<T>(T @event) where T : IDomainEvent;
    }
}