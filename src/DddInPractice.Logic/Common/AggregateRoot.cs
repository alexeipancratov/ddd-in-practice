using System.Collections.Generic;

namespace DddInPractice.Logic.Common;

// We may not need an additional abstract interface if we won't need:
// - optimistic locking (where we would need to store version of the aggregate)
// - domain events
public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public virtual IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

    protected virtual void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public virtual void ClearEvents()
    {
        _domainEvents.Clear();
    }
}