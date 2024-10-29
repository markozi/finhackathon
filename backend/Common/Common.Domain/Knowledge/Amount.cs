namespace Common.Domain.Knowledge;

public class Amount
{
    public Amount(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }

    public Amount Add(Amount amount)
    {
        return new Amount(Value + amount.Value);
    }

    public Amount Invert()
    {
        return new Amount(-Value);
    }
}