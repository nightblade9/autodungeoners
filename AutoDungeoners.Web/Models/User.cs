using Mongo.Migration.Documents;
using MongoDB.Bson;

namespace AutoDungeoners.Web.Models
{
    /// <summary>
    /// A registered user (sans credentials, which are in the Auth class).
    /// </summary>
    public class User : HasId
    {
        public DocumentVersion Version { get; set; }
        public string EmailAddress { get; set; }
        public int Gold { get; set; }
    }
}