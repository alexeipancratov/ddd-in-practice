using DddInPractice.Logic.Common;

namespace DddInPractice.Logic.SnackMachines;

public class Snack : AggregateRoot
{
    // We leverage the reference data that we have in database.
    // I.e. data that is predefined.
    // TODO: Cover these props with integration tests to make sure IDs are the same.
    public static readonly Snack None = new Snack(0, "None"); // Null-value design pattern.
    public static readonly Snack Chocolate = new(1, "Chocolate");
    public static readonly Snack Soda = new(2, "Soda");
    public static readonly Snack Gum = new(3, "Gum");
    
    public virtual string Name { get; }

    // For NHibernate
    protected Snack()
    {
    }

    private Snack(long id, string name)
        : this()
    {
        Id = id;
        Name = name;
    }
}