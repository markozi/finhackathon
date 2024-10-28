using FinHack.Common.Knowledge;
using FinHack.Common.Model;

namespace FinHack.Core.Banking.Accounting;

public abstract record BalanceChangedEvent(
    AccountReference Account,
    BalanceSnapshot PreviousBalance,
    BalanceSnapshot NewBalance,
    Amount TransactionAmount)
    : DomainEvent;