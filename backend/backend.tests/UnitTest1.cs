using FluentAssertions;

namespace backend.tests;

public class BalanceTest
{
    [Fact]
    public void Balance_0_Debit_100_NewBalance_Minus100()
    {
        // Arrange
        var account = Account();
        var originalAmount = new Amount(0);
        var debitAmount = new Amount(100);
        
        var originalBalance = Balance.Reconstitute(account, DateTime.Today, originalAmount);
        
        // Act
        var newBalance = originalBalance.Debit(debitAmount);
        
        // Assert
        var expectedBalance = -100;
        newBalance.Amount.Value.Should().Be(expectedBalance);
        newBalance.Account.Should().Be(account);
        newBalance.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(10));
    }
    
    [Fact]
    public void Balance_0_Credit_100_NewBalance_100()
    {
        // Arrange
        var account = Account();
        var originalAmount = new Amount(0);
        var debitAmount = new Amount(100);
        
        var originalBalance = Balance.Reconstitute(account, DateTime.Today, originalAmount);
        
        // Act
        var newBalance = originalBalance.Credit(debitAmount);
        
        // Assert
        var expectedBalance = 100;
        newBalance.Amount.Value.Should().Be(expectedBalance);
        newBalance.Account.Should().Be(account);
        newBalance.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(10));
    }
    
    [Fact]
    public void Balance_100_Debit_100_NewBalance_0()
    {
        // Arrange
        var account = Account();
        var originalAmount = new Amount(100);
        var debitAmount = new Amount(100);
        
        var originalBalance = Balance.Reconstitute(account, DateTime.Today, originalAmount);
        
        // Act
        var newBalance = originalBalance.Debit(debitAmount);
        
        // Assert
        var expectedBalance = 0;
        newBalance.Amount.Value.Should().Be(expectedBalance);
        newBalance.Account.Should().Be(account);
        newBalance.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(10));
    }
    
    [Fact]
    public void Balance_100_Credit_100_NewBalance_200()
    {
        // Arrange
        var account = Account();
        var originalAmount = new Amount(100);
        var debitAmount = new Amount(100);
        
        var originalBalance = Balance.Reconstitute(account, DateTime.Today, originalAmount);
        
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