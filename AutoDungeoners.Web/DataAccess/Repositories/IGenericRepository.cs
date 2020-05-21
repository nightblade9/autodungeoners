using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoDungeoners.Web.Models;

namespace AutoDungeoners.Web.DataAccess.Repositories
{
    public interface IGenericRepository
    {
        T SingleOrDefault<T>(Expression<Func<T, bool>> predicate);
        void Insert<T>(T obj);
        IEnumerable<T> All<T>();
        void Update<T>(T target) where T : HasId;
    }
}