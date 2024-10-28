namespace FinHack.Core.Banking.Transactions;

public interface ITransactionSource
{
    public string Type { get; }
    public string Identifier { get; }
}