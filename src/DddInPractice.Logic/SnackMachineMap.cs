using FluentNHibernate.Mapping;

namespace DddInPractice.Logic;

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
    }
}