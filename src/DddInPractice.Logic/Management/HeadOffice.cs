using DddInPractice.Logic.Atms;
using DddInPractice.Logic.Common;
using DddInPractice.Logic.SharedKernel;
using DddInPractice.Logic.SnackMachines;

namespace DddInPractice.Logic.Management;

public class HeadOffice : AggregateRoot
{
    /// <summary>
    /// All the payments made from users' cards (aka they deposited their money in the "bank").
    /// </summary>
    public virtual decimal Balance { get; set; }

    /// <summary>
    /// Money transferred from the cash machines.
    /// </summary>
    public virtual Money Cash { get; set; } = Money.None;

    public virtual void ChangeBalance(decimal delta)
    {
        Balance += delta;
    }

    public virtual void LoadCashToAtm(Atm atm)
    {
        atm.LoadMoney(Cash);
        Cash = Money.None;
    }

    public virtual void UnloadCashFromSnackMachine(SnackMachine snackMachine)
    {
        Money money = snackMachine.UnloadMoney();
        Cash += money;
    }
}