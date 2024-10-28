using FinHack.Common.Knowledge;
using FinHack.Common.Model;

namespace FinHack.Core.Banking.Accounting;

public class Balance : Aggregate
{
    private BalanceSnapshot _snapshot;
    
    private Balance(Guid id, AccountReference account) : base(id)
    {
        _snapshot = new BalanceSnapshot(account, DateTime.Now, new Amount(0));
    }
    
    private Balance(Guid id, int version, BalanceSnapshot snapshot) : base(id, version)
    {
        _snapshot = snapshot;
    }

    public static Balance New(AccountReference account)
    {
        return new Balance(Guid.NewGuid(), 0, new BalanceSnapshot(account, DateTime.Now, new Amount(0)));
    }
    
    public static Balance Reconstitute(Guid id, int version, AccountReference account, DateTime timestamp, Amount value)
    {
        return new Balance(id, version, new BalanceSnapshot(account, timestamp, value));
    }

    public static Balance Rehydrate(Guid id, AccountReference account, IEnumerable<DomainEvent> historicEvents)
    {
        var balance = new Balance(id, account);
        balance.Rehydrate(historicEvents);
        return balance;
    }
    
    public AccountReference Account => _snapshot.Account;
    public DateTime Timestamp => _snapshot.Timestamp;
    public Amount Amount => _snapshot.Amount;

    public void Debit(Amount amount)
    {
        var previousBalance = _snapshot;
        var newBalance = _snapshot.Debit(amount);
        RaiseEvent(new BalanceDebitedEvent(Account, previousBalance, newBalance, amount));
    }
    
    public void Credit(Amount amount)
    {
        var previousBalance = _snapshot;
        var newBalance = _snapshot.Credit(amount);
        RaiseEvent(new BalanceCreditedEvent(Account, previousBalance, newBalance, amount));
    }

    public BalanceSnapshot GetSnapshot()
    {
        return _snapshot;
    }
    
    protected override void ApplyEvent(DomainEvent domainEvent)
    {
        Apply((dynamic)domainEvent);
    }

    private void Apply(BalanceCreditedEvent @domainEvent)
    {
        _snapshot = domainEvent.NewBalance;
    }

    private void Apply(BalanceDebitedEvent @domainEvent)
    {
        _snapshot = domainEvent.NewBalance;
    }
}