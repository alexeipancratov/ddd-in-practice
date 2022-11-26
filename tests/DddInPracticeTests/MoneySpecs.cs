using DddInPractice.Logic;
using FluentAssertions;
using System;
using Xunit;

namespace DddInPracticeTests;

public class MoneySpecs
{
    [Fact]
    public void SumOfTwoMoneys_ProducesCorrectResult()
    {
        var money1 = new Money(1, 2, 3, 4, 5, 6);
        var money2 = new Money(1, 2, 3, 4, 5, 6);

        Money sum = money1 + money2;

        sum.OneCentCount.Should().Be(2);
        sum.TenCentCount.Should().Be(4);
        sum.QuarterCount.Should().Be(6);
        sum.OneDollarCount.Should().Be(8);
        sum.FiveDollarCount.Should().Be(10);
        sum.TwentyDollarCount.Should().Be(12);
    }

    [Fact]
    public void TwoMoneyInstances_AreEqual_IfContainTheSameMoneyAmount()
    {
        var money1 = new Money(1, 2, 3, 4, 5, 6);
        var money2 = new Money(1, 2, 3, 4, 5, 6);

        money1.Should().Be(money2);
        money1.GetHashCode().Should().Be(money2.GetHashCode());
    }

    [Fact]
    public void TwoMoneyInstances_AreNotEqual_IfContainDifferentMoneyAmount()
    {
        var dollar = new Money(0, 0, 0, 1, 0, 0);
        var hundredCents = new Money(100, 0, 0, 0, 0, 0);

        dollar.Should().NotBe(hundredCents);
        dollar.GetHashCode().Should().NotBe(hundredCents.GetHashCode());
    }

    [Theory]
    [InlineData(-1, 0, 0, 0, 0, 0)]
    [InlineData(0, -1, 0, 0, 0, 0)]
    [InlineData(0, 0, -1, 0, 0, 0)]
    [InlineData(0, 0, 0, -1, 0, 0)]
    [InlineData(0, 0, 0, 0, -1, 0)]
    [InlineData(0, 0, 0, 0, 0, -1)]
    public void CannotCreateMoney_WithNegativeValues(
        int oneCentCount,
        int tenCentCount,
        int quarterCount,
        int oneDollarCount,
        int fiveDollarCount,
        int twentyDollarCount)
    {
        Action action = () => new Money(
            oneCentCount,
            tenCentCount,
            quarterCount,
            oneDollarCount,
            fiveDollarCount,
            twentyDollarCount);

        action.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [InlineData(0, 0, 0, 0, 0, 0, 0)]
    [InlineData(1, 0, 0, 0, 0, 0, 0.01)]
    [InlineData(1, 2, 0, 0, 0, 0, 0.21)]
    [InlineData(1, 2, 3, 0, 0, 0, 0.96)]
    [InlineData(1, 2, 3, 4, 0, 0, 4.96)]
    [InlineData(1, 2, 3, 4, 5, 0, 29.96)]
    [InlineData(1, 2, 3, 4, 5, 6, 149.96)]
    [InlineData(11, 0, 0, 0, 0, 0, 0.11)]
    [InlineData(110, 0, 0, 0, 100, 0, 501.1)]
    public void AmountIsCalculatedCorrectly(
        int oneCentCount,
        int tenCentCount,
        int quarterCount,
        int oneDollarCount,
        int fiveDollarCount,
        int twentyDollarCount,
        double expectedAmount)
    {
        var money = new Money(
            oneCentCount,
            tenCentCount,
            quarterCount,
            oneDollarCount,
            fiveDollarCount,
            twentyDollarCount);

        money.Amount.Should().Be((decimal)expectedAmount);
    }
    
    [Fact]
    public void Subtraction_of_two_moneys_produces_correct_result()
    {
        var money1 = new Money(10, 10, 10, 10, 10, 10);
        var money2 = new Money(1, 2, 3, 4, 5, 6);

        Money result = money1 - money2;

        result.OneCentCount.Should().Be(9);
        result.TenCentCount.Should().Be(8);
        result.QuarterCount.Should().Be(7);
        result.OneDollarCount.Should().Be(6);
        result.FiveDollarCount.Should().Be(5);
        result.TwentyDollarCount.Should().Be(4);
    }
    
    [Fact]
    public void Cannot_subtract_more_than_exists()
    {
        var money1 = new Money(0, 1, 0, 0, 0, 0);
        var money2 = new Money(1, 0, 0, 0, 0, 0);

        var action = () =>
        {
            var money = money1 - money2;
        };

        action.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [InlineData(1, 0, 0, 0, 0, 0, "¢1")]
    [InlineData(0, 1, 0, 0, 0, 0, "¢10")]
    [InlineData(0, 0, 0, 1, 0, 0, "$1.00")]
    [InlineData(1, 0, 0, 1, 0, 0, "$1.01")]
    [InlineData(0, 0, 2, 1, 0, 0, "$1.50")]
    public void To_string_should_return_amount_of_money(
        int oneCentCount,
        int tenCentCount,
        int quarterCount,
        int oneDollarCount,
        int fiveDollarCount,
        int twentyDollarCount,
        string expectedString)
    {
        var money = new Money(
            oneCentCount,
            tenCentCount,
            quarterCount,
            oneDollarCount,
            fiveDollarCount,
            twentyDollarCount);

        money.ToString().Should().Be(expectedString);
    }
}