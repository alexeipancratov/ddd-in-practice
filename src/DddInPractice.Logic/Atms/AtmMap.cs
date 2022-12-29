using FluentNHibernate.Mapping;

namespace DddInPractice.Logic.Atms;

public class AtmMap : ClassMap<Atm>
{
    public AtmMap()
    {
        Id(a => a.Id);

        Component(a => a.MoneyInside, cp =>
        {
            cp.Map(a => a.OneCentCount);
            cp.Map(a => a.TenCentCount);
            cp.Map(a => a.QuarterCount);
            cp.Map(a => a.OneDollarCount);
            cp.Map(a => a.FiveDollarCount);
            cp.Map(a => a.TwentyDollarCount);
        });
    }
}