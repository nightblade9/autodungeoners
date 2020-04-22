using System;
using AutoDungeoners.Web.Controllers;
using AutoDungeoners.Web.DataAccess.Repositories;
using AutoDungeoners.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using static AutoDungeoners.Web.Controllers.RegisterController;

namespace AutoDungeoners.Web.Tests
{
    [TestFixture]
    public class RegisterControllerTests
    {

        [Test]
        public void RegisterSucceeds()
        {
            // Arrange
            const string expectedEmail = "test@test.com";
            const string expectedPassword = "password";

            var request = new RegistrationRequest() { EmailAddress = expectedEmail, Password = expectedPassword };

            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(u => u.Insert(It.IsAny<User>())).Callback<User>((user) =>
            {
                Assert.That(user, Is.Not.Null);
                Assert.That(user.EmailAddress, Is.EqualTo(expectedEmail));
                // Keep track of the ID so the Auth gets linked correctly
                userRepo.Setup(u => u.FindOneByEmail(expectedEmail)).Returns(user);
                user.Id = ObjectId.GenerateNewId();
            });

            var authRepo = new Mock<IAuthRepository>();
            authRepo.Setup(a => a.Insert(It.IsAny<Auth>())).Callback<Auth>((auth) =>
            {
                Assert.That(auth, Is.Not.Null);
                Assert.That(auth.HashedPassword, Is.EqualTo(expectedPassword));
            });

            var controller = new RegisterController(new Mock<ILogger<RegisterController>>().Object, userRepo.Object, authRepo.Object);

            // Act
            var response = controller.Register(request).Result;

            // Assert: response is OK and records were inserted (callbacks invoked)
            Assert.That(response, Is.TypeOf(typeof(OkObjectResult)));
            userRepo.VerifyAll();
            authRepo.VerifyAll();
        }

        [Test]
        public void RegisterThrowsIfUserNameAlreadyExistsInDb()
        {
            // Arrange
            const string expectedEmail = "test@test.com";
            const string expectedPassword = "password";

            var request = new RegistrationRequest() { EmailAddress = expectedEmail, Password = expectedPassword };

            var existingUser = new User() { EmailAddress = expectedEmail, Id = new MongoDB.Bson.ObjectId() };
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(u => u.FindOneByEmail(expectedEmail)).Returns(existingUser);

            var controller = new RegisterController(new Mock<ILogger<RegisterController>>().Object, userRepo.Object, new Mock<IAuthRepository>().Object);

            // Act
            var response = controller.Register(request).Result;

            // Assert
            Assert.That(response, Is.TypeOf(typeof(BadRequestObjectResult)));
            var obj = ((BadRequestObjectResult)response).Value;
            Assert.That(obj, Is.TypeOf(typeof(ArgumentException)));
        }
    }
}