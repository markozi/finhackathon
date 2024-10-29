namespace Common.Domain.Knowledge;

public class Money
{
    public Money(Currency currency, Amount amount)
    {
        Currency = currency;
        Amount = amount;
    }
    
    public Currency Currency { get; }
    public Amount Amount { get; }
}