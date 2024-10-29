using Common.Domain.Knowledge;
using Common.Domain.Model;
using FinHack.Core.Banking.Accounting;

namespace FinHack.Core.Banking.Transactions;

public abstract record TransactionProcessedEvent(
    ITransactionSource TransactionSource,
    AccountReference DebitAccount,
    AccountReference CreditAccount,
    Amount TransactionAmount)
    : DomainEvent
{
    public DateTime TransactionTime => EventTime;
}