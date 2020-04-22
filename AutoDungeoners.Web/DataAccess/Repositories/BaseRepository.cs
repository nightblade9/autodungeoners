using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace AutoDungeoners.Web.DataAccess.Repositories
{
    public class BaseRepository<T>
    {
        private readonly IMongoClient client;
        private readonly MongoCollectionSettings settings;
        private readonly string databaseName;
        private readonly string repositoryName;

        public BaseRepository(IConfiguration configuration, IMongoClient client)
        {
            this.client = client;
            var nameParts = typeof(T).Name.Split('.');
            this.repositoryName = nameParts[nameParts.Length - 1];
            this.settings = new MongoCollectionSettings() { AssignIdOnInsert = true };
            this.databaseName = configuration.GetSection("MongoDb:Database").Value;
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            return this.client.GetDatabase(this.databaseName).GetCollection<T>(this.repositoryName, this.settings);
        }
    }
}