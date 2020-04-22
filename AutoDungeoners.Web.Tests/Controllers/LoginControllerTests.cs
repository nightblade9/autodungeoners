using System.Linq;
using AutoDungeoners.Web.Controllers;
using AutoDungeoners.Web.DataAccess.Repositories;
using AutoDungeoners.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

            var existingUser = new User() { EmailAddress = expectedEmail, Id = new MongoDB.Bson.ObjectId() };
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(u => u.FindOneByEmail(expectedEmail)).Returns(existingUser);

            var authRepo = new Mock<IAuthRepository>();
            var credentials = new Auth() { UserId = existingUser.Id, HashedPassword = expectedPassword };
            authRepo.Setup(a => a.FindOneById(existingUser.Id)).Returns(credentials);
            
            var controller = new LoginController(new Mock<ILogger<LoginController>>().Object, userRepo.Object, authRepo.Object);

            // Act
            var response = controller.Login(request).Result;

            // Assert
            Assert.That(response, Is.TypeOf(typeof(OkObjectResult)));
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