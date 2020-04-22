using AutoDungeoners.Web.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace AutoDungeoners.Web.DataAccess.Repositories
{
    public class MongoRepository : IMongoRepository
    {
        private readonly IConfiguration configuration;
        private IMongoClient client;

        public MongoRepository(IConfiguration configuration, IMongoClient client)
        {
            this.configuration = configuration;
            this.client = client;
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            var typeName = typeof(T).Name.Split('.');
            var collectionName = typeName[typeName.Length - 1];
            var database = configuration.GetSection("MongoDb:Database").Value;
            var mongoSettings = new MongoCollectionSettings() { AssignIdOnInsert = true };
            var users = client.GetDatabase(database).GetCollection<T>(collectionName, mongoSettings);
            return users;
        }
    }
}