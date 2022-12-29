using System;
using NHibernate.Proxy;

namespace DddInPractice.Logic.Common;

/* By having a base abstact class we can define the comparison logic in a single place
 * and to write logic of setting the ID in a single place.
 */
public abstract class Entity
{
    public virtual long Id { get; protected set; }

    public override bool Equals(object obj)
    {
        if (!(obj is Entity other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetRealType() != other.GetRealType())
            return false;

        // Identity was not yet established, they cannot be compared.
        if (Id == 0 || other.Id == 0)
            return false;

        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return (GetRealType().ToString() + Id).GetHashCode();
    }

    public static bool operator ==(Entity a, Entity b)
    {
        // Can accept null for both operands (unlike Equals).
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b) => !(a == b);

    // Using an ORM leads to DB storage leakage to the domain layer.
    // But it doesn't have a significant impact.
    private Type GetRealType()
    {
        return NHibernateProxyHelper.GetClassWithoutInitializingProxy(this);
    }
}