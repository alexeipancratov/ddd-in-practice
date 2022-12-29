namespace DddInPractice.Logic.Common;

// We may not need an additional abstract interface if we won't need:
// - optimistic locking (where we would need to store version of the aggregate)
// - domain events
public abstract class AggregateRoot : Entity
{
    
}