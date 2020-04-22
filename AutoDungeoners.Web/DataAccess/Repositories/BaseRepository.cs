using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Linq;

namespace AutoDungeoners.Web.DataAccess.Repositories
{
    public class BaseRepository<T>
    {
        protected readonly IMongoClient client;
        protected readonly MongoCollectionSettings settings;
        protected readonly string databaseName;
        protected readonly string repositoryName;

        public BaseRepository(IConfiguration configuration, IMongoClient client)
        {
            this.client = client;
            var nameParts = typeof(T).Name.Split('.');
            this.repositoryName = nameParts[nameParts.Length - 1];
            this.settings = new MongoCollectionSettings() { AssignIdOnInsert = true };
            this.databaseName = configuration.GetSection("MongoDb:Database").Value;
        }
    }
}