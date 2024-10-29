using Common.Domain.Knowledge;

namespace FinHack.Core.Banking.Accounting;

public record BalanceSnapshot(AccountReference Account, DateTime Timestamp, Amount Amount)
{
    public BalanceSnapshot Debit(Amount amount)
    {
        return new BalanceSnapshot(Account, DateTime.Now, Amount.Add(amount.Invert()));
    }

    public BalanceSnapshot Credit(Amount amount)
    {
        return new BalanceSnapshot(Account, DateTime.Now, Amount.Add(amount));
    }
}