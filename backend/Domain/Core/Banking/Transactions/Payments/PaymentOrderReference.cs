using Common.Domain.Model;

namespace FinHack.Core.Banking.Transactions.Payments;

public record PaymentOrderReference(Guid Id) : EntityReference(Id), ITransactionSource
{
    public string Type => nameof(PaymentOrderReference);
    public string Identifier => Id.ToString();
}