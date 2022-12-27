using NHibernate;

namespace DddInPractice.Logic;

public abstract class Repository<T>
    where T : AggregateRoot
{
    public T GetById(long id)
    {
        // We open a session every time since it's a Desktop app.
        // NOTE: In a web app we'd inject it via ctor.
        using ISession session = SessionFactory.OpenSession();
        
        return session.Get<T>(id);
    }

    public void Save(T aggregateRoot)
    {
        using ISession session = SessionFactory.OpenSession();
        using ITransaction transaction = session.BeginTransaction();
        
        session.SaveOrUpdate(aggregateRoot);
        transaction.Commit();
    }
}