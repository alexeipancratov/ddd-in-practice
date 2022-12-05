using System;
using System.Linq;
using static DddInPractice.Logic.Money;

namespace DddInPractice.Logic;

// To be able to use this entity with NHibernate we had to remove "sealed"
// and add "virtual" for all its public members. Also we had to make setters as protected.
// But it's a fair trade-off.
public class SnackMachine : AggregateRoot
{
    public virtual Money MoneyInside { get; protected set; }

    /// <summary>
    /// Holds the currently insert amount of money. Since upon return we return money
    /// using highest denomination first, it doesn't make sense to make it of Money type.
    /// </summary>
    public virtual decimal MoneyInTransaction { get; protected set; }

    protected virtual Slot[] Slots { get; set; }

    public SnackMachine()
    {
        MoneyInside = None;
        MoneyInTransaction = 0;
        Slots = new Slot[]
        {
            new(this, 1),
            new(this, 2),
            new(this, 3),
        };
    }

    public virtual SnackPile GetSnackPile(int position)
    {
        return GetSlot(position).SnackPile;
    }

    private Slot GetSlot(int position)
    {
        return Slots.Single(s => s.Position == position);
    }

    // Inserts customer's money into the machine.
    public virtual void InsertMoney(Money money)
    {
        Money[] coinsAndNotes = { Cent, TenCent, Quarter, Dollar, FiveDollar, TwentyDollar };

        if (!coinsAndNotes.Contains(money))
        {
            throw new InvalidOperationException("Can insert known money only");
        }
        
        MoneyInTransaction += money.Amount;
        MoneyInside += money;
    }

    public virtual void ReturnMoney()
    {
        Money moneyToReturn = MoneyInside.CalculateMoneyBasedOnAmount(MoneyInTransaction);
        MoneyInside -= moneyToReturn;
        MoneyInTransaction = 0;
    }

    public virtual void BuySnack(int position)
    {
        Slot slot = GetSlot(position);
        if (slot.SnackPile.Price > MoneyInTransaction)
        {
            throw new InvalidOperationException("Not enough money.");
        }
        slot.SnackPile = slot.SnackPile.SubtractOne();
        
        Money change = MoneyInside.CalculateMoneyBasedOnAmount(MoneyInTransaction - slot.SnackPile.Price);
        if (change.Amount < MoneyInTransaction - slot.SnackPile.Price)
        {
            throw new InvalidOperationException();
        }
        MoneyInside -= change;
        MoneyInTransaction = 0;
    }

    public virtual void LoadSnacks(int position, SnackPile snackPile)
    {
        Slot slot = GetSlot(position);
        slot.SnackPile = snackPile;
    }

    // A utility method to load money into the machine.
    public virtual void LoadMoney(Money money)
    {
        MoneyInside += money;
    }
}