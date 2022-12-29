using DddInPractice.Logic;
using DddInPractice.Logic.SnackMachines;
using DddInPractice.Logic.Utils;
using Xunit;
using static DddInPractice.Logic.SharedKernel.Money;

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