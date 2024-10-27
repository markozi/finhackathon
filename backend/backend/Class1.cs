using System.Numerics;

namespace backend;

public abstract class DomainEvent
{
    protected DomainEvent()
    {
        EventTime = DateTime.Now;
    }
    
    protected DomainEvent(DateTime eventTime)
    {
        EventTime = eventTime;
    }

    public DateTime EventTime { get; }
}


public abstract class TransactionProcessedEvent : DomainEvent
{
    protected TransactionProcessedEvent(ISource source, AccountReference debitAccount, AccountReference creditAccount, Amount transactionAmount)
    {
        Source = source;
        DebitAccount = debitAccount;
        CreditAccount = creditAccount;
        TransactionAmount = transactionAmount;
    }

    public ISource Source { get; }
    public AccountReference DebitAccount { get; }
    public AccountReference CreditAccount { get; }
    public Amount TransactionAmount { get; }
    public DateTime TransactionTime => EventTime;
}

public interface ISource
{
    public string Type { get; }
    public string Identifier { get; }
}

public class BalanceComputer
{
    
    public IEnumerable<BalanceChangedEvent> ComputeBalances(Balance debitorBalance, Balance creditorBalance, PaymentProcessedEvent paymentProcessedEvent)
    {
        return new List<BalanceChangedEvent>
        {
            new BalanceChangedEvent(
                debitorBalance.Account,
                debitorBalance,
                debitorBalance.Debit(paymentProcessedEvent.TransactionAmount),
                paymentProcessedEvent.TransactionAmount),

            new BalanceChangedEvent(
                creditorBalance.Account,
                creditorBalance,
                creditorBalance.Credit(paymentProcessedEvent.TransactionAmount),
                paymentProcessedEvent.TransactionAmount),
        };
    }
    
    // public IEnumerable<DomainEvent> ComputeBalance(IEnumerable<PaymentProcessedEvent> paymentProcessedEvent)
    // {
    //     
    // }
}

public class Balance
{
    private Balance(AccountReference account, DateTime timestamp, Amount amount)
    {
        Account = account;
        Timestamp = timestamp;
        Amount = amount;
    }

    public static Balance New(AccountReference account)
    {
        return new Balance(account, DateTime.Now, new Amount(0));
    }

    public static Balance Reconstitute(AccountReference account, DateTime timestamp, Amount value)
    {
        return new Balance(account, timestamp, value);
    }
    
    public AccountReference Account { get; }
    public DateTime Timestamp { get; }
    public Amount Amount { get; }

    public Balance Debit(Amount amount)
    {
        return new Balance(Account, DateTime.Now, Amount.Add(amount.Invert()));
    }

    public Balance Credit(Amount amount)
    {
        return new Balance(Account, DateTime.Now, Amount.Add(amount));
    }
}

public class PaymentProcessedEvent : TransactionProcessedEvent
{
    public PaymentProcessedEvent(
        PaymentOrderReference order,
        AccountReference debitAccount,
        AccountReference creditAccount,
        Amount transactionAmount) : base(order, debitAccount, creditAccount, transactionAmount)
    {
        Order = order;
    }
    
    public PaymentOrderReference Order { get; }
}

public class BalanceChangedEvent : DomainEvent
{
    public BalanceChangedEvent(AccountReference account, Balance previousBalance, Balance currentBalance, Amount transactionAmount)
    {
        Account = account;
        PreviousBalance = previousBalance;
        CurrentCurrentBalance = currentBalance;
        TransactionAmount = transactionAmount;
    }
    
    public AccountReference Account { get; }
    public Balance PreviousBalance { get; }
    public Balance CurrentCurrentBalance { get; }
    public Amount TransactionAmount { get; }
}

public class PaymentOrderReference : EntityReference, ISource
{
    public PaymentOrderReference(Guid id) : base(id)
    {
    }

    public string Type => nameof(PaymentOrderReference);
    public string Identifier => Id.ToString();
}

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

public class Currency(string name)
{
    public String Name { get; } = name;
}

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

public class AccountReference : EntityReference
{
    public AccountReference(Guid id) : base(id)
    {
    }
}

public class EntityReference
{
    public EntityReference(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
    
}