using FinHack.Common.Knowledge;
using FinHack.Common.Model;
using FinHack.Core.Banking.Accounting;
using FluentAssertions;

namespace FinHack.Tests.Core.Banking.Accounting;

public class BalanceTest
{
    [Test]
    public void Balance_0_Debit_100_NewBalance_Minus100()
    {
        TestDebit(0, 100, -100);
    }

    [Test]
    public void Balance_100_Debit_100_NewBalance_0()
    {
        TestDebit(100, 100, 0);
    }

    [Test]
    public void Balance_200_Debit_100_NewBalance_100()
    {
        TestDebit(200, 100, 100);
    }

    [Test]
    public void Balance_0_0001_Debit_0_0000001_NewBalance_0_0000999m()
    {
        TestDebit(0.0001m, 0.0000001m, 0.0000999m);
    }

    [Ignore(
        "This test fails currently as decimal is not suitable when subtracting such small numbers. Need a different type.")]
    [Test]
    public void Balance_0_0000000000000000001m_Debit_0_00000000000000000001m_NewBalance_0_000000000000000000009m()
    {
        TestDebit(0.0000000000000000001m, 0.00000000000000000001m, 0.000000000000000000009m);
    }

    [Test]
    public void Balance_0_1_Debit_0_00000000000000000001m_NewBalance_0_09999999999999999999m()
    {
        TestDebit(0.1m, 0.00000000000000000001m, 0.09999999999999999999m);
    }

    private static void TestDebit(decimal originalAmount, decimal debitAmount, decimal expectedAmount)
    {
        // Arrange
        var balance = ReconstitutedBalance(new Amount(originalAmount));

        // Act
        balance.Debit(new Amount(debitAmount));

        // Assert
        balance.Amount.Value.Should().Be(expectedAmount);
        balance.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(10));
    }

    [Test]
    public void Balance_0_Credit_100_NewBalance_Minus100()
    {
        TestCredit(0, 100, 100);
    }

    [Test]
    public void Balance_100_Credit_100_NewBalance_200()
    {
        TestCredit(100, 100, 200);
    }

    [Test]
    public void Balance_Minus100_Credit_100_NewBalance_0()
    {
        TestCredit(-100, 100, 0);
    }

    [Test]
    public void Balance_0001_Credit_0000001_NewBalance_0()
    {
        TestCredit(0.0001m, 0.0000001m, 0.0001001m);
    }

    [Test]
    public void Balance_0000000000000000001m_Credit_00000000000000000001m_NewBalance_00000000000000000011m()
    {
        TestCredit(0.0000000000000000001m, 0.00000000000000000001m, 0.00000000000000000011m);
    }

    private static void TestCredit(decimal originalAmount, decimal debitAmount, decimal expectedAmount)
    {
        // Arrange
        var balance = ReconstitutedBalance(new Amount(originalAmount));

        // Act
        balance.Credit(new Amount(debitAmount));

        // Assert
        balance.Amount.Value.Should().Be(expectedAmount);
        balance.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(10));
    }

    [Test]
    public void Balance_Rehydrate_HistoricBalanceEvents_BalanceEqualToLastEvent()
    {
        // Arrange
        var balanceId = Guid.NewGuid();
        var account = Account();
        var historicBalanceEvents = HistoricBalanceEvents(account);

        // Act
        var balance = Balance.Rehydrate(balanceId, account, historicBalanceEvents);

        // Assert
        balance.Amount.Should().Be(historicBalanceEvents.Last().NewBalance.Amount);
        balance.Version.Should().Be(historicBalanceEvents.Count);
        balance.Account.Should().Be(account);
        balance.Id.Should().Be(balanceId);
    }

    [Test]
    public void Balance_New_WithAccount()
    {
        // Arrange
        var account = Account();

        // Act
        var balance = Balance.New(account);

        // Assert
        balance.Account.Should().Be(account);
    }

    [Test]
    public void Balance_New_AmountIs0()
    {
        // Arrange
        var account = Account();
       
        // Act
        var balance = Balance.New(account);

        // Assert
        balance.Amount.Value.Should().Be(0);
        balance.Version.Should().Be(0);
        balance.Account.Should().Be(account);
    }

    [Test]
    public void Balance_New_VersionIs0()
    {
        // Arrange
        var account = Account();
       
        // Act
        var balance = Balance.New(account);

        // Assert
        balance.Version.Should().Be(0);
    }

    [Test]
    public void Balance_GetSnapshot()
    {
        // Arrange
        var originalAmount = new Amount(199.001m);
        var balance = ReconstitutedBalance(originalAmount);
       
        // Act
        var snapshot = balance.GetSnapshot();
        
        // Assert
        snapshot.Account.Should().Be(balance.Account);
        snapshot.Timestamp.Should().Be(balance.Timestamp);
        snapshot.Amount.Should().Be(balance.Amount);
    }

    private static List<BalanceChangedEvent> HistoricBalanceEvents(AccountReference account)
    {
        return new List<BalanceChangedEvent>
        {
            new BalanceCreditedEvent(
                account,
                new BalanceSnapshot(account, DateTime.Today.AddDays(-10), new Amount(0)),
                new BalanceSnapshot(account, DateTime.Today.AddDays(-9), new Amount(100)),
                new Amount(100)),
            new BalanceCreditedEvent(
                account,
                new BalanceSnapshot(account, DateTime.Today.AddDays(-9), new Amount(100)),
                new BalanceSnapshot(account, DateTime.Today.AddDays(-8), new Amount(150)),
                new Amount(50)),
            new BalanceDebitedEvent(
                account,
                new BalanceSnapshot(account, DateTime.Today.AddDays(-8), new Amount(150)),
                new BalanceSnapshot(account, DateTime.Today.AddDays(-7), new Amount(80)),
                new Amount(70)),
            new BalanceCreditedEvent(
                account,
                new BalanceSnapshot(account, DateTime.Today.AddDays(-7), new Amount(80)),
                new BalanceSnapshot(account, DateTime.Today.AddDays(-6), new Amount(20)),
                new Amount(60)),
        };
    }

    private static Balance ReconstitutedBalance(Amount originalAmount)
    {
        var account = Account();
        var balance = Balance.Reconstitute(Guid.NewGuid(), 1, account, DateTime.Today, originalAmount);
        return balance;
    }

    private static AccountReference Account()
    {
        return new AccountReference(Guid.NewGuid());
    }
}