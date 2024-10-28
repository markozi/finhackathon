using FinHack.Common.Knowledge;
using FinHack.Common.Model;
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