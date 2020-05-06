using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AutoDungeoners.Web.DataAccess.Repositories
{
    public interface IGenericRepository
    {
        T SingleOrDefault<T>(Expression<Func<T, bool>> predicate);
        void Insert<T>(T obj);
        IEnumerable<T> All<T>();
    }
}