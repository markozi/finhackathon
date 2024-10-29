using Common.Domain.Knowledge;
using Common.Domain.Model;

namespace FinHack.Core.Banking.Accounting;

public abstract record BalanceChangedEvent(
    AccountReference Account,
    BalanceSnapshot PreviousBalance,
    BalanceSnapshot NewBalance,
    Amount TransactionAmount)
    : DomainEvent;