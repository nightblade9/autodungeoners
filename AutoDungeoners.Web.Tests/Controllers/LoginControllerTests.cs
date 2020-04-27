using System;
using System.Linq.Expressions;
using AutoDungeoners.Web.Controllers;
using AutoDungeoners.Web.DataAccess.Repositories;
using AutoDungeoners.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            var credentials = new Auth() { UserId = existingUser.Id, HashedPassword = expectedPassword };
            var repository = new Mock<IGenericRepository>();
            repository.Setup(u => u.SingleOrDefault(It.IsAny<Expression<Func<User, bool>>>())).Returns(existingUser);
            repository.Setup(a => a.SingleOrDefault(It.IsAny<Expression<Func<Auth, bool>>>())).Returns(credentials);
            
            var controller = new LoginController(new Mock<ILogger<LoginController>>().Object, repository.Object);

            // Act
            var response = controller.Login(request).Result;

            // Assert
            Assert.That(response, Is.TypeOf(typeof(OkObjectResult)));
        }

        [Test]
        public void LoginReturnsBadRequestIfUserDoesntExistsInDatabase()
        {
            // Arrange
            const string expectedEmail = "test@test.com";

            var request = new LoginRequest() { EmailAddress = expectedEmail };

            var controller = new LoginController(new Mock<ILogger<LoginController>>().Object, new Mock<IGenericRepository>().Object);

            // Act
            var response = controller.Login(request).Result;

            // Assert
            Assert.That(response, Is.TypeOf(typeof(BadRequestObjectResult)));
            var obj = ((BadRequestObjectResult)response).Value;
            Assert.That(obj, Is.TypeOf(typeof(ArgumentException)));
        }

        [Test]
        public void LoginReturnsBadRequestIfPasswordDoesntMatchHash()
        {
            // Arrange
            const string expectedEmail = "test@test.com";
            const string expectedPassword = "password";

            var request = new LoginRequest() { EmailAddress = expectedEmail, Password = "wrong password!" };

            var existingUser = new User() { EmailAddress = expectedEmail, Id = new MongoDB.Bson.ObjectId() };
            var credentials = new Auth() { UserId = existingUser.Id, HashedPassword = expectedPassword };

            var repository = new Mock<IGenericRepository>();
            repository.Setup(u => u.SingleOrDefault<User>(It.IsAny<Expression<Func<User, bool>>>())).Returns(existingUser);
            repository.Setup(u => u.SingleOrDefault<Auth>(It.IsAny<Expression<Func<Auth, bool>>>())).Returns(credentials);
            
            var controller = new LoginController(new Mock<ILogger<LoginController>>().Object, repository.Object);

            // Act
            var response = controller.Login(request).Result;

            // Assert
            Assert.That(response, Is.TypeOf(typeof(BadRequestObjectResult)));
            var obj = ((BadRequestObjectResult)response).Value;
            Assert.That(obj, Is.TypeOf(typeof(ArgumentException)));
        }
    }
}