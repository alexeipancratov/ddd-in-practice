namespace DddInPractice.Logic;

public class Snack : AggregateRoot
{
    public virtual string Name { get; }

    // For NHibernate
    protected Snack()
    {
    }

    public Snack(string name)
        : this()
    {
        Name = name;
    }
}