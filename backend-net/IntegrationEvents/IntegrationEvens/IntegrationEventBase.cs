namespace IntegrationEvents;

public abstract class IntegrationEventBase : IIntegrationEvent
{
    public Guid EventId { get; }
    public DateTime CreationDate { get; }

    protected IntegrationEventBase()
    {
        EventId = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }
}