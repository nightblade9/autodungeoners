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
            var users = this.GetCollection<User>();
            var toReturn = users.Find(u => u.EmailAddress == emailAddress).SingleOrDefault();
            return toReturn;
        }

        public void Insert(User user)
        {
            var users = this.GetCollection<User>();
            users.InsertOne(user);
        }
    }

    public interface IUserRepository
    {
        User FindOneByEmail(string emailAddress);
        void Insert(User user);
    }
}