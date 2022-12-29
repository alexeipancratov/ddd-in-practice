using System;
using DddInPractice.Logic.Common;

namespace DddInPractice.Logic.SharedKernel;

// DB: Value objects should be stored inside of the parent entity object.
public sealed class Money : ValueObject<Money>
{
    public static readonly Money None = new Money(0, 0, 0, 0, 0, 0);
    public static readonly Money Cent = new Money(1, 0, 0, 0, 0, 0);
    public static readonly Money TenCent = new Money(0, 1, 0, 0, 0, 0);
    public static readonly Money Quarter = new Money(0, 0, 1, 0, 0, 0);
    public static readonly Money Dollar = new Money(0, 0, 0, 1, 0, 0);
    public static readonly Money FiveDollar = new Money(0, 0, 0, 0, 1, 0);
    public static readonly Money TwentyDollar = new Money(0, 0, 0, 0, 0, 1);
    
    public int OneCentCount { get; }

    public int TenCentCount { get; }

    public int QuarterCount { get; }

    public int OneDollarCount { get; }

    public int FiveDollarCount { get; }

    public int TwentyDollarCount { get; }

    public decimal Amount =>
        OneCentCount * 0.01m +
        TenCentCount * 0.10m +
        QuarterCount * 0.25m +
        OneDollarCount +
        FiveDollarCount * 5 +
        TwentyDollarCount * 20;

    private Money()
    {
    }

    public Money(
        int oneCentCount,
        int tenCentCount,
        int quarterCount,
        int oneDollarCount,
        int fiveDollarCount,
        int twentyDollarCount)
    : this()
    {
        if (oneCentCount < 0)
            throw new InvalidOperationException();
        if (tenCentCount < 0)
            throw new InvalidOperationException();
        if (quarterCount < 0)
            throw new InvalidOperationException();
        if (oneDollarCount < 0)
            throw new InvalidOperationException();
        if (fiveDollarCount < 0)
            throw new InvalidOperationException();
        if (twentyDollarCount < 0)
            throw new InvalidOperationException();

        OneCentCount = oneCentCount;
        TenCentCount = tenCentCount;
        QuarterCount = quarterCount;
        OneDollarCount = oneDollarCount;
        FiveDollarCount = fiveDollarCount;
        TwentyDollarCount = twentyDollarCount;
    }

    public static Money operator +(Money money1, Money money2) => new(
        money1.OneCentCount + money2.OneCentCount,
        money1.TenCentCount + money2.TenCentCount,
        money1.QuarterCount + money2.QuarterCount,
        money1.OneDollarCount + money2.OneDollarCount,
        money1.FiveDollarCount + money2.FiveDollarCount,
        money1.TwentyDollarCount + money2.TwentyDollarCount);

    public static Money operator -(Money money1, Money money2) =>
        new(
            money1.OneCentCount - money2.OneCentCount,
            money1.TenCentCount - money2.TenCentCount,
            money1.QuarterCount - money2.QuarterCount,
            money1.OneDollarCount - money2.OneDollarCount,
            money1.FiveDollarCount - money2.FiveDollarCount,
            money1.TwentyDollarCount - money2.TwentyDollarCount);

    public static Money operator *(Money money1, int multiplier) =>
        new(
            money1.OneCentCount * multiplier,
            money1.TenCentCount * multiplier,
            money1.QuarterCount * multiplier,
            money1.OneDollarCount * multiplier,
            money1.FiveDollarCount * multiplier,
            money1.TwentyDollarCount * multiplier);

    protected override bool EqualsCore(Money other)
    {
        return OneCentCount == other.OneCentCount
               && TenCentCount == other.TenCentCount
               && QuarterCount == other.QuarterCount
               && OneDollarCount == other.OneDollarCount
               && FiveDollarCount == other.FiveDollarCount
               && TwentyDollarCount == other.TwentyDollarCount;
    }

    protected override int GetHashCodeCore()
    {
        unchecked
        {
            int hashCode = OneCentCount;
            hashCode = (hashCode * 397) ^ TenCentCount;
            hashCode = (hashCode * 397) ^ QuarterCount;
            hashCode = (hashCode * 397) ^ OneDollarCount;
            hashCode = (hashCode * 397) ^ FiveDollarCount;
            hashCode = (hashCode * 397) ^ TwentyDollarCount;

            return hashCode;
        }
    }

    public override string ToString()
    {
        if (Amount < 1)
        {
            return "¢" + (Amount * 100).ToString("0");
        }

        return "$" + Amount.ToString("0.00");
    }
    
    /// <summary>
    /// Verifies if it's possible to calculate a new <see cref="Money"/> instance which corresponds to the <paramref name="amount"/>
    /// using the highest possible bills/coins ($20 down to 1 cent).
    /// </summary>
    /// <param name="amount">Amount to be checked against.</param>
    /// <returns>True, if there's enough bills/coins to match <paramref name="amount"/>. False - otherwise.</returns>
    public bool CanCalculateMoneyUsingHighestBillsOrCoinsBasedOnAmount(decimal amount)
    {
        Money money = CalculateMoneyUsingHighestBillsOrCoinsBasedOnAmountCore(amount);
        return money.Amount == amount;
    }

    /// <summary>
    /// Calculates a new <see cref="Money"/> instance which corresponds to the <paramref name="amount"/>
    /// using the highest possible bills/coins ($20 down to 1 cent).
    /// </summary>
    /// <remarks>Is a useful logic because it's preferable to have lower-denominated bills
    /// in the machine for change.</remarks>
    /// <param name="amount">The target amount of money.</param>
    /// <returns>A new <see cref="Money"/> instance which corresponds to the <paramref name="amount"/></returns>
    public Money CalculateMoneyUsingHighestBillsOrCoinsBasedOnAmount(decimal amount)
    {
        if (!CanCalculateMoneyUsingHighestBillsOrCoinsBasedOnAmount(amount))
        {
            throw new InvalidOperationException();
        }

        return CalculateMoneyUsingHighestBillsOrCoinsBasedOnAmountCore(amount);
    }
    
    private Money CalculateMoneyUsingHighestBillsOrCoinsBasedOnAmountCore(decimal amount)
    {
        int twentyDollarCount = Math.Min((int)(amount / 20), TwentyDollarCount);
        amount -= twentyDollarCount * 20;
        
        int fiveDollarCount = Math.Min((int)(amount / 5), FiveDollarCount);
        amount -= fiveDollarCount * 5;
        
        int oneDollarCount = Math.Min((int)amount, OneDollarCount);
        amount -= oneDollarCount;
        
        int quarterCount = Math.Min((int)(amount / 0.25m), QuarterCount);
        amount -= quarterCount * 0.25m;
        
        int tenCentCount = Math.Min((int)(amount / 0.1m), TenCentCount);
        amount -= tenCentCount * 0.1m;

        int oneCentCount = Math.Min((int)(amount / 0.01m), OneCentCount);

        return new Money(
            oneCentCount,
            tenCentCount,
            quarterCount,
            oneDollarCount,
            fiveDollarCount,
            twentyDollarCount);
    }
}