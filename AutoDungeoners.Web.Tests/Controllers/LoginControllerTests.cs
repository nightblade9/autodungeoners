using System.Linq;
using AutoDungeoners.Web.Models;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using static AutoDungeoners.Web.Controllers.LoginController;

namespace AutoDungeoners.Web.Tests
{
    [TestFixture]
    public class LoginControllerTests
    {
        [Test]
        public void LoginReturnsOkWithUserIfCredentialsMatch()
        {
            // Arrange
            const string expectedEmail = "test@test.com";
            const string expectedPassword = "password";

            var request = new LoginRequest() { EmailAddress = expectedEmail, Password = expectedPassword };

            var existingUser = new User() { EmailAddress = expectedEmail };
            var usersCollection = new Mock<IMongoCollection<User>>();
            usersCollection.Setup(e => e.Find(It.IsAny<FilterDefinition<User>>(), null));

            var usersDb = new Mock<IMongoDatabase>();
            usersDb.Setup(u => u.GetCollection<User>("User", It.IsAny<MongoCollectionSettings>())).Returns(usersCollection.Object);
            
            var mongoClient = new Mock<IMongoClient>();
            mongoClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null)).Returns(usersDb.Object);

            // Act
            var actual = mongoClient.Object.GetDatabase("hi there").GetCollection<User>("User").Find(u => true).SingleOrDefault();

            // Assert
            Assert.That(actual, Is.Not.Null);
        }

        [Test]
        public void LoginReturnsBadRequestIfUserNameAlreadyExistsInDatabase()
        {

        }

        [Test]
        public void LoginReturnsBadRequestIfPasswordDoesntMatchHash()
        {

        }
    }
}