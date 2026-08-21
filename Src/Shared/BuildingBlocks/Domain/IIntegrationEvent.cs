namespace BuildingBlocks.Domain;

public interface IIntegrationEvent
{
    public Guid Id { get;  }
    public DateTime OccurredOn { get; }
}
