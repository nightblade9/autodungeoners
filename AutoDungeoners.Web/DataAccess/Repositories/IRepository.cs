using System;

namespace AutoDungeoners.Web.DataAccess.Repositories
{
    public interface IRepository<T>
    {
        T SingleOrDefault(Func<T, bool> predicate);
    }
}