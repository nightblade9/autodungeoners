using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace AutoDungeoners.Web.DataAccess.Repositories
{
    public class GenericRepository : IGenericRepository
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
            var repositoryName = this.GetRepositoryName<T>();
            var collection = this.client.GetDatabase(this.databaseName).GetCollection<T>(repositoryName, this.settings);
            return collection.Find(predicate).SingleOrDefault();
        }

        public void Insert<T>(T obj)
        {
            var repositoryName = this.GetRepositoryName<T>();
            var collection = this.client.GetDatabase(this.databaseName).GetCollection<T>(repositoryName, this.settings);
            collection.InsertOne(obj);
        }

        public IEnumerable<T> All<T>()
        {
            var repositoryName = this.GetRepositoryName<T>();
            var collection = this.client.GetDatabase(this.databaseName).GetCollection<T>(repositoryName, this.settings);
            return collection.AsQueryable();
        }

        private string GetRepositoryName<T>()
        {
            var nameParts = typeof(T).Name.Split('.');
            var repositoryName = nameParts[nameParts.Length - 1];
            return repositoryName;
        }
    }
}