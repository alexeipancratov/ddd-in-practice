using System.Collections.Generic;
using System.Linq;
using DddInPractice.Logic.Common;
using DddInPractice.Logic.Utils;
using NHibernate;

namespace DddInPractice.Logic.SnackMachines;

public class SnackMachineRepository : Repository<SnackMachine>
{
    public IReadOnlyList<SnackMachineDto> GetSnackMachineList()
    {
        ISession session = SessionFactory.OpenSession();
        
        return session.Query<SnackMachine>()
            .ToList()
            .Select(sm => new SnackMachineDto(sm.Id, sm.MoneyInside.Amount))
            .ToList();
    }
}