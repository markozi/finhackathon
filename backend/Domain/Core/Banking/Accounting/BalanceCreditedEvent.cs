using Common.Domain.Knowledge;

namespace FinHack.Core.Banking.Accounting;

public record BalanceCreditedEvent(
    AccountReference Account,
    BalanceSnapshot PreviousBalance,
    BalanceSnapshot NewBalance,
    Amount TransactionAmount)
    : BalanceChangedEvent(Account,
        PreviousBalance,
        NewBalance,
        TransactionAmount);