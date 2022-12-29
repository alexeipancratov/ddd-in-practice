using FluentNHibernate.Mapping;

namespace DddInPractice.Logic.SnackMachines;

public class SlotMap : ClassMap<Slot>
{
    public SlotMap()
    {
        Id(x => x.Id);
        Map(x => x.Position);

        Component(x => x.SnackPile, y =>
        {
            y.Map(x => x.Quantity);
            y.Map(x => x.Price);
            // Since we cannot use Lazy Loading when our object is detached from its session.
            // In a web app object wouldn't be detached.
            y.References(x => x.Snack).Not.LazyLoad();
        });

        References(x => x.SnackMachine);
    }
}