using AutoDungeoners.Web.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AutoDungeoners.Web.DataAccess.Repositories
{
    public class AuthRepository : BaseRepository<Auth>, IAuthRepository
    {
        public AuthRepository(IConfiguration configuration, IMongoClient client) : base(configuration, client)
        {
        }

        public Auth FindOneById(ObjectId id)
        {
            var auths = this.GetCollection<Auth>();
            var toReturn = auths.Find(a => a.UserId == id).SingleOrDefault();
            return toReturn;
        }

        public void Insert(Auth auth)
        {
            var auths = this.GetCollection<Auth>();
            auths.InsertOne(auth);
        }
    }

    public interface IAuthRepository
    {
        Auth FindOneById(ObjectId id);
        void Insert(Auth auth);
    }
}