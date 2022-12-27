using System;
using DddInPractice.Logic;
using FluentAssertions;
using Xunit;
using static DddInPractice.Logic.Money;
using static DddInPractice.Logic.Snack;

namespace DddInPracticeTests;

public class SnackMachineSpecs
{
    [Fact]
    public void ReturnMoney_EmptiesMoneyInTransaction()
    {
        // Arrange
        var snackMachine = new SnackMachine();
        snackMachine.InsertMoney(Dollar);
        
        // Act
        snackMachine.ReturnMoney();
        
        // Assert
        snackMachine.MoneyInTransaction.Should().Be(0m);
    }

    [Fact]
    public void Inserted_money_goes_to_money_in_transaction()
    {
        var snackMachine = new SnackMachine();
        
        snackMachine.InsertMoney(Cent);
        snackMachine.InsertMoney(Dollar);

        snackMachine.MoneyInTransaction.Should().Be(1.01m);
    }

    [Fact]
    public void Cannot_insert_more_than_one_coin_or_note_at_a_time()
    {
        var snackMachine = new SnackMachine();
        var twoCent = Cent + Cent;

        var action = () => snackMachine.InsertMoney(twoCent);

        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void BuySnack_trades_inserted_money_for_a_snack()
    {
        // Arrange
        const int slotPosition = 1;
        var snackMachine = new SnackMachine();
        snackMachine.LoadSnacks(slotPosition, new SnackPile(Chocolate, 10, 1m));
        snackMachine.InsertMoney(Dollar);
        
        // Act
        snackMachine.BuySnack(slotPosition);

        // Assert
        snackMachine.MoneyInTransaction.Should().Be(0);
        snackMachine.MoneyInside.Should().Be(Dollar);
        snackMachine.GetSnackPile(1).Quantity.Should().Be(9); // we could've retrieved the same from the .Slots
    }

    [Fact]
    public void Snack_machine_returns_money_with_highest_denomination_first()
    {
        var snackMachine = new SnackMachine();
        snackMachine.LoadMoney(Dollar);
        
        snackMachine.InsertMoney(Quarter);
        snackMachine.InsertMoney(Quarter);
        snackMachine.InsertMoney(Quarter);
        snackMachine.InsertMoney(Quarter);
        snackMachine.ReturnMoney();

        snackMachine.MoneyInside.QuarterCount.Should().Be(4);
        snackMachine.MoneyInside.OneDollarCount.Should().Be(0);
    }

    [Fact]
    public void After_purchase_change_is_returned()
    {
        // Arrange
        var snackMachine = new SnackMachine();
        snackMachine.LoadSnacks(1, new SnackPile(Chocolate, 1, price: 0.5m));
        snackMachine.LoadMoney(TenCent * 10);
        
        // Act
        snackMachine.InsertMoney(Dollar);
        snackMachine.BuySnack(1);

        // Assert
        snackMachine.MoneyInside.Amount.Should().Be(1.5m); // 0.5 was returned to the customer
        snackMachine.MoneyInTransaction.Should().Be(0m);
    }

    [Fact]
    public void Cannot_buy_snack_if_not_enough_change()
    {
        // Arrange
        var snackMachine = new SnackMachine();
        snackMachine.LoadSnacks(1, new SnackPile(Chocolate, 1, price: 0.5m));
        snackMachine.InsertMoney(Dollar);
        
        // Act
        var action = () => snackMachine.BuySnack(1);
        
        // Assert
        action.Should().Throw<InvalidOperationException>();
    }
}