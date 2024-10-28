namespace FinHack.Common.Model;

public abstract record DomainEvent(DateTime EventTime)
{
    protected DomainEvent() : this(DateTime.Now)
    {
    }
}