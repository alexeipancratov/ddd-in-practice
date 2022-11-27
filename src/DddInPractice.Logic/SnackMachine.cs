using System;
using System.Linq;
using static DddInPractice.Logic.Money;

namespace DddInPractice.Logic;

// To be able to use this entity with NHibernate we had to remove "sealed"
// and add "virtual" for all its public members. Also we had to make setters as protected.
// But it's a fair trade-off.
public class SnackMachine : Entity
{
    public virtual Money MoneyInside { get; protected set; } = None;

    public virtual Money MoneyInTransaction { get; protected set; } = None;

    public virtual void InsertMoney(Money money)
    {
        Money[] coinsAndNotes = { Cent, TenCent, Quarter, Dollar, FiveDollar, TwentyDollar };

        if (!coinsAndNotes.Contains(money))
        {
            throw new InvalidOperationException("Can insert known money only");
        }
        
        MoneyInTransaction += money;
    }

    public virtual void ReturnMoney()
    {
        MoneyInTransaction = None;
    }

    public virtual void BuySnack()
    {
        MoneyInside += MoneyInTransaction;
        MoneyInTransaction = None;
    }
}