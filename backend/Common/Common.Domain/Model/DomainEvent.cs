namespace Common.Domain.Model;

public abstract record DomainEvent(DateTime EventTime)
{
    protected DomainEvent() : this(DateTime.Now)
    {
    }
}