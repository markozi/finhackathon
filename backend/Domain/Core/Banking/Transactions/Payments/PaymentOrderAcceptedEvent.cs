using FinHack.Common.Knowledge;
using FinHack.Core.Banking.Accounting;

namespace FinHack.Core.Banking.Transactions.Payments;

public record PaymentOrderAcceptedEvent : TransactionProcessedEvent
{
    public PaymentOrderAcceptedEvent(
        PaymentOrderReference order,
        AccountReference debitAccount,
        AccountReference creditAccount,
        Amount transactionAmount) : base(order, debitAccount, creditAccount, transactionAmount)
    {
        Order = order;
    }
    
    public PaymentOrderReference Order { get; }
}