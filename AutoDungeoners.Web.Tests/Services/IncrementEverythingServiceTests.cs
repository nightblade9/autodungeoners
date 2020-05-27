using System.Threading.Tasks;
using AutoDungeoners.Web.DataAccess.Repositories;
using AutoDungeoners.Web.Models;
using AutoDungeoners.Web.Services;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;

namespace AutoDungeoners.Web.Tests.Services
{
    [TestFixture]
    public class IncrementEverythingServiceTestss
    {
        [Test]
        public async Task OnTickUpdatesGoldAndSavesUser()
        {
            // Arrange
            var repo = new Mock<IGenericRepository>();
            var user1 = new User() { EmailAddress = "a", Gold = 1023, Id = ObjectId.GenerateNewId() };
            var user2 = new User() { EmailAddress = "b", Gold = 0, Id = ObjectId.GenerateNewId() };
            repo.Setup(r => r.All<User>()).Returns(new User[] { user1, user2 });

            // Updating "DB" should update our user references
            repo.Setup(r => r.Update(It.IsAny<User>())).Callback<User>((updated) => {
                if (updated.Id == user1.Id)
                {
                    user1 = updated;
                } else if (updated.Id == user2.Id)
                {
                    user2 = updated;
                }
            });

            var service = new IncrementEverythingService(repo.Object);

            // Act
            await service.OnTick(1);
            
            // Assert
            Assert.That(user1.Gold, Is.EqualTo(1024));
            Assert.That(user2.Gold, Is.EqualTo(1));

            return;
        }
    }
}