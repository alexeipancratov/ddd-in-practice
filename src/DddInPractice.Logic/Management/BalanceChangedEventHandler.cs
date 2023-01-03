using DddInPractice.Logic.Atms;
using DddInPractice.Logic.Common;

namespace DddInPractice.Logic.Management;

// NOTE: Handlers are usually simple as they delegate work to other domain classes.
// And their role is very similar to domain services.
public class BalanceChangedEventHandler : IDomainEventHandler<BalanceChangedEvent>
{
    public void Handle(BalanceChangedEvent balanceChangedEvent)
    {
        var repository = new HeadOfficeRepository();
        HeadOffice headOffice = HeadOfficeInstance.Instance;
        headOffice.ChangeBalance(balanceChangedEvent.Delta);
        repository.Save(headOffice);
    }
}