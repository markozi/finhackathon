using FinHack.Common.Model;

namespace FinHack.Core.Banking.Accounting;

public record AccountReference(Guid Id) : EntityReference(Id);