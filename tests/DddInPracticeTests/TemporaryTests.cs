using DddInPractice.Logic;
using Xunit;
using static DddInPractice.Logic.Money;

namespace DddInPracticeTests;

public class TemporaryTests
{
    [Fact]
    public void Test()
    {
        SessionFactory.Init("Server=.;Database=DddInPractice;Trusted_Connection=true");

        var repository = new SnackMachineRepository();
        var snackMachine = repository.GetById(1);
        snackMachine.InsertMoney(Dollar);
        snackMachine.InsertMoney(Dollar);
        snackMachine.InsertMoney(Dollar);
        snackMachine.BuySnack(1);
        repository.Save(snackMachine);
    }
}