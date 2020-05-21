using Mongo.Migration.Documents;
using MongoDB.Bson;

namespace AutoDungeoners.Web.Models
{
    /// <summary>Authentication instance (user ID, password hash, salt, etc.)</summary>
    public class Auth : HasId
    {
        public DocumentVersion Version { get; set; }
        
        public ObjectId UserId { get; set; }
        public string Salt { get; set; }
        public string HashedPassword { get; set; }
    }
}