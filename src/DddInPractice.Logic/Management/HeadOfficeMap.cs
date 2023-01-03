using FluentNHibernate.Mapping;

namespace DddInPractice.Logic.Management;

public class HeadOfficeMap : ClassMap<HeadOffice>
{
    public HeadOfficeMap()
    {
        Id(o => o.Id);

        Map(o => o.Balance);
        
        Component(o => o.Cash, cp =>
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