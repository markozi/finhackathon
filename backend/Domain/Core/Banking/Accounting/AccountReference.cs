using Common.Domain.Model;

namespace FinHack.Core.Banking.Accounting;

public record AccountReference(Guid Id) : EntityReference(Id);