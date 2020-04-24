using System;
using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace AutoDungeoners.Web.DataAccess.Repositories
{
    public class GenericRepository
    {
        private readonly IMongoClient client;
        private readonly MongoCollectionSettings settings;
        private readonly string databaseName;

        public GenericRepository(IConfiguration configuration, IMongoClient client)
        {
            this.client = client;
            this.settings = new MongoCollectionSettings() { AssignIdOnInsert = true };
            this.databaseName = configuration.GetSection("MongoDb:Database").Value;
        }

        public T SingleOrDefault<T>(Expression<Func<T, bool>> predicate)
        {
            var nameParts = typeof(T).Name.Split('.');
            var repositoryName = nameParts[nameParts.Length - 1];
            var collection = this.client.GetDatabase(this.databaseName).GetCollection<T>(repositoryName, this.settings);
            //var expression = FuncToExpression(predicate);
            return collection.Find(o => predicate(o) == true).SingleOrDefault();
        }

        private static Expression<Func<T, bool>> FuncToExpression<T>(Func<T, bool> f)  
        {  
            return x => f(x);  
        } 
    }
}