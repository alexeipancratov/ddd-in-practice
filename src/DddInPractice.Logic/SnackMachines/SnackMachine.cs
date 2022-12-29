using System;
using System.Collections.Generic;
using System.Linq;
using DddInPractice.Logic.Common;
using DddInPractice.Logic.SharedKernel;
using static DddInPractice.Logic.SharedKernel.Money;

namespace DddInPractice.Logic.SnackMachines;

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

    protected virtual ICollection<Slot> Slots { get; set; }

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

    // NOTE: Just as in GetSnackPile, we return a collection of value objects,
    // and not a collection of internal entities. So we still maintain aggregate's encapsulation.
    public virtual IReadOnlyList<SnackPile> GetAllSnackPiles()
    {
        return Slots
            .OrderBy(s => s.Position)
            .Select(s => s.SnackPile)
            .ToList();
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
        Money moneyToReturn = MoneyInside.CalculateMoneyUsingHighestBillsOrCoinsBasedOnAmount(MoneyInTransaction);
        MoneyInside -= moneyToReturn;
        MoneyInTransaction = 0;
    }

    public virtual string CanBuySnack(int position)
    {
        SnackPile snackPile = GetSnackPile(position);

        if (snackPile.Quantity == 0)
        {
            return "The snack pile is empty";
        }

        if (MoneyInTransaction < snackPile.Price)
        {
            return "Not enough money";
        }

        if (!MoneyInside.CanCalculateMoneyUsingHighestBillsOrCoinsBasedOnAmount(MoneyInTransaction - snackPile.Price))
        {
            return "Not enough change";
        }

        return string.Empty;
    }

    public virtual void BuySnack(int position)
    {
        if (CanBuySnack(position) != string.Empty)
        {
            throw new InvalidOperationException();
        }
        
        Slot slot = GetSlot(position);
        slot.SnackPile = slot.SnackPile.SubtractOne();
        
        Money change = MoneyInside.CalculateMoneyUsingHighestBillsOrCoinsBasedOnAmount(MoneyInTransaction - slot.SnackPile.Price);
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