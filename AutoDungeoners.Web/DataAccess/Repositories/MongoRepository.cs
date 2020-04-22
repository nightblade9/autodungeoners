using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Linq;

namespace AutoDungeoners.Web.DataAccess.Repositories
{
    public class MongoRepository<T> : IRepository<T>
    {
        private readonly IMongoClient client;
        private readonly MongoCollectionSettings settings;
        private readonly string databaseName;
        private readonly string repositoryName;

        public MongoRepository(IConfiguration configuration, IMongoClient client)
        {
            this.client = client;
            var nameParts = typeof(T).Name.Split('.');
            this.repositoryName = nameParts[nameParts.Length - 1];
            this.settings = new MongoCollectionSettings() { AssignIdOnInsert = true };
            this.databaseName = configuration.GetSection("MongoDb:Database").Value;
        }

        public T SingleOrDefault(Func<T, bool> predicate)
        {
            var objects = this.client.GetDatabase(this.databaseName).GetCollection<T>(this.repositoryName, this.settings);
            var toReturn = objects.Find(o => predicate(o) == true).SingleOrDefault();
            return toReturn;
        }
    }
}