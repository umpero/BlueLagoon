using BlueLagoon.Kernel.Types.Abstractions;

namespace BlueLagoon.Kernel.Types;

public abstract class AggregateRoot<T> : BaseEntity
{
    public T Id { get; protected set; }

    public int Version { get; protected set; }

    private readonly List<IDomainEvent> _events = [];

    public IEnumerable<IDomainEvent> Events => _events;

    private bool _versionIncremented;

    protected void AddEvent(IDomainEvent @event)
    {
        if (!_events.Any() && !_versionIncremented)
        {
            Version++;
            _versionIncremented = true;
        }

        _events.Add(@event); 
    }

    public void ClearEvents() => _events.Clear();

    protected void IncrementVersion()
    {
        if (_versionIncremented)
        {
            return;
        }

        Version++;
        _versionIncremented = true;
    }
}

public abstract class AggregateRoot : AggregateRoot<AggregateId>
{
}
