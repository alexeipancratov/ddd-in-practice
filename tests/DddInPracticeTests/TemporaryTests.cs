using DddInPractice.Logic;
using NHibernate;
using Xunit;

namespace DddInPracticeTests;

public class TemporaryTests
{
    [Fact]
    public void Test()
    {
        SessionFactory.Init("Server=.;Database=DddInPractice;Trusted_Connection=true");

        using (ISession session = SessionFactory.OpenSession())
        {
            long id = 1;
            var snackMachine = session.Get<SnackMachine>(id);
        }
    }
}