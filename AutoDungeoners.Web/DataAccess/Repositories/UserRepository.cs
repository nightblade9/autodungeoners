using AutoDungeoners.Web.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace AutoDungeoners.Web.DataAccess.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IConfiguration configuration, IMongoClient client) : base(configuration, client)
        {
        }

        public User FindOneByEmail(string emailAddress)
        {
            var users = this.client.GetDatabase(this.databaseName).GetCollection<User>(this.repositoryName, this.settings);
            var toReturn = users.Find(u => u.EmailAddress == emailAddress).SingleOrDefault();
            return toReturn;
        }
    }

    public interface IUserRepository
    {
        User FindOneByEmail(string emailAddress);
    }
}