using FluentNHibernate;
using FluentNHibernate.Mapping;

namespace DddInPractice.Logic.SnackMachines;

public class SnackMachineMap : ClassMap<SnackMachine>
{
    public SnackMachineMap()
    {
        Id(sm => sm.Id);

        Component(sm => sm.MoneyInside, cm =>
        {
            // each one of them will have its own column in the SnackMachine table.
            cm.Map(m => m.OneCentCount);
            cm.Map(m => m.TenCentCount);
            cm.Map(m => m.QuarterCount);
            cm.Map(m => m.OneDollarCount);
            cm.Map(m => m.FiveDollarCount);
            cm.Map(m => m.TwentyDollarCount);
        });
        
        // We use Reveal because Slots in SnackMachine is protected and thus cannot be accessed directly.
        HasMany<Slot>(Reveal.Member<SnackMachine>("Slots"))
            .Cascade.SaveUpdate()
            .Not.LazyLoad()
            .Inverse();
    }
}