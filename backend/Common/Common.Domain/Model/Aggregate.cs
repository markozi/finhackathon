namespace Common.Domain.Model;

public abstract class Aggregate : Entity
{
    private readonly List<DomainEvent> _uncommittedEvents = new List<DomainEvent>();

    protected Aggregate(Guid id)
    {
        Id = id;
        Version = 0;
    }
    
    protected Aggregate(Guid id, int version)
    {
        Id = id;
        Version = version;
    }

    public Guid Id { get; }
    public int Version { get; private set; }

    public IEnumerable<DomainEvent> DomainEvents => _uncommittedEvents;

    protected void RaiseEvent(DomainEvent domainEvent)
    {
        _uncommittedEvents.Add(domainEvent);
        ApplyEvent(domainEvent, isFromHistory: false);
    }
    
    public void Rehydrate(IEnumerable<DomainEvent> historyEvents)
    {
        foreach (var domainEvent in historyEvents)
        {
            ApplyEvent(domainEvent, isFromHistory: true);
            Version++;
        }
    }
    
    private void ApplyEvent(DomainEvent domainEvent, bool isFromHistory)
    {
        ApplyEvent(domainEvent);

        if (!isFromHistory)
        {
            _uncommittedEvents.Add(domainEvent);
            Version++;
        }
    }

    /// <summary>
    /// The implementors should implement additional Apply(ConcreteDomainEvent) methods
    /// for each concrete type of DomainEvent that it raises.
    /// And call these methods using Apply((dynamic)domainEvent). 
    /// </summary>
    /// <param name="domainEvent"></param>
    protected abstract void ApplyEvent(DomainEvent domainEvent);

    /// <summary>
    ///  Clears uncommitted events after they have been saved
    /// </summary>
    public void ClearUncommittedEvents()
    {
        _uncommittedEvents.Clear();
    }
    
    /// <summary>
    /// This is a fallback method for event sourced entities where the developer forgot to implement a specific Apply()-method.
    /// Normally each event that can be produced by an entity must also be applied.
    /// </summary>
    /// <param name="event"></param>
    /// <exception cref="NotImplementedException"></exception>
    protected void Apply(object @event)
    {
        throw new NotImplementedException($"Aggregate {GetType().FullName} does not implement an Apply()-method for {@event.GetType().FullName}.");
    }
}