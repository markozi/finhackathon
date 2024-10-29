using Common.Domain.Knowledge;
using FinHack.Core.Banking.Accounting;
using FluentAssertions;

namespace FinHack.Tests.Core.Banking.Accounting;

public class BalanceSnapshotTest
{
    [Test]
    public void BalanceSnapshot_0_Debit_100_NewBalance_Minus100()
    {
        // Arrange
        var account = Account();
        var originalAmount = new Amount(0);
        var debitAmount = new Amount(100);

        var originalBalance = new BalanceSnapshot(account, DateTime.Today, originalAmount);

        // Act
        var newBalance = originalBalance.Debit(debitAmount);

        // Assert
        var expectedBalance = -100;
        newBalance.Amount.Value.Should().Be(expectedBalance);
        newBalance.Account.Should().Be(account);
        newBalance.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(10));
    }

    [Test]
    public void BalanceSnapshot_0_Credit_100_NewBalance_100()
    {
        // Arrange
        var account = Account();
        var originalAmount = new Amount(0);
        var debitAmount = new Amount(100);

        var originalBalance = new BalanceSnapshot(account, DateTime.Today, originalAmount);

        // Act
        var newBalance = originalBalance.Credit(debitAmount);

        // Assert
        var expectedBalance = 100;
        newBalance.Amount.Value.Should().Be(expectedBalance);
        newBalance.Account.Should().Be(account);
        newBalance.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(10));
    }

    [Test]
    public void BalanceSnapshot_100_Debit_100_NewBalance_0()
    {
        // Arrange
        var account = Account();
        var originalAmount = new Amount(100);
        var debitAmount = new Amount(100);

        var originalBalance = new BalanceSnapshot(account, DateTime.Today, originalAmount);

        // Act
        var newBalance = originalBalance.Debit(debitAmount);

        // Assert
        var expectedBalance = 0;
        newBalance.Amount.Value.Should().Be(expectedBalance);
        newBalance.Account.Should().Be(account);
        newBalance.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(10));
    }

    [Test]
    public void BalanceSnapshot_100_Credit_100_NewBalance_200()
    {
        // Arrange
        var account = Account();
        var originalAmount = new Amount(100);
        var debitAmount = new Amount(100);

        var originalBalance = new BalanceSnapshot(account, DateTime.Today, originalAmount);

        // Act
        var newBalance = originalBalance.Credit(debitAmount);

        // Assert
        var expectedBalance = 200;
        newBalance.Amount.Value.Should().Be(expectedBalance);
        newBalance.Account.Should().Be(account);
        newBalance.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(10));
    }

    private static AccountReference Account()
    {
        return new AccountReference(Guid.NewGuid());
    }
}