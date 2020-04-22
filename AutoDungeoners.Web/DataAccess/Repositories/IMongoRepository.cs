using MongoDB.Driver;

namespace AutoDungeoners.Web.DataAccess.Repositories
{
    public interface IMongoRepository
    {
        IMongoCollection<T> GetCollection<T>();
    }
}