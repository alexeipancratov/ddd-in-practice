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

    public virtual Money MoneyInTransaction { get; protected set; }

    protected virtual Slot[] Slots { get; set; }

    public SnackMachine()
    {
        MoneyInside = None;
        MoneyInTransaction = None;
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

    public virtual void BuySnack(int position)
    {
        Slot slot = GetSlot(position);
        slot.SnackPile = slot.SnackPile.SubtractOne();
        
        MoneyInside += MoneyInTransaction;
        MoneyInTransaction = None;
    }

    public virtual void LoadSnacks(int position, SnackPile snackPile)
    {
        Slot slot = GetSlot(position);
        slot.SnackPile = snackPile;
    }
}