using System;
using DddInPractice.Logic;
using FluentAssertions;
using Xunit;
using static DddInPractice.Logic.Money;

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

        snackMachine.MoneyInTransaction.Amount.Should().Be(1.01m);
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
    public void Money_in_transaction_goes_to_money_inside_after_purchase()
    {
        var snackMachine = new SnackMachine();
        snackMachine.InsertMoney(Dollar);
        snackMachine.InsertMoney(Dollar);
        
        snackMachine.BuySnack();

        snackMachine.MoneyInTransaction.Should().Be(None);
        snackMachine.MoneyInside.Should().Be(2m);
    }
}