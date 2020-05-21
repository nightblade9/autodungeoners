using System;
using System.Linq.Expressions;
using AutoDungeoners.Web.Controllers;
using AutoDungeoners.Web.DataAccess.Repositories;
using AutoDungeoners.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using static AutoDungeoners.Web.Controllers.RegisterController;

namespace AutoDungeoners.Web.Tests.Controllers
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

            User expectedUser = null;
            var request = new RegistrationRequest() { EmailAddress = expectedEmail, Password = expectedPassword };

            var repository = new Mock<IGenericRepository>();
            repository.Setup(u => u.Insert(It.IsAny<User>())).Callback<User>((user) =>
            {
                Assert.That(user, Is.Not.Null);
                Assert.That(user.EmailAddress, Is.EqualTo(expectedEmail));
                repository.Setup(u => u.SingleOrDefault(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);
                user.Id = ObjectId.GenerateNewId();
                expectedUser = user;
            });

            repository.Setup(a => a.Insert(It.IsAny<Auth>())).Callback<Auth>((auth) =>
            {
                Assert.That(auth, Is.Not.Null);
                Assert.That(auth.HashedPassword, Is.EqualTo(expectedPassword));
            });

            var controller = new RegisterController(new Mock<ILogger<RegisterController>>().Object, repository.Object);

            // Act
            var response = controller.Register(request).Result;

            // Assert: response is OK and records were inserted (callbacks invoked)
            Assert.That(response, Is.TypeOf(typeof(OkObjectResult)));
            var obj = ((OkObjectResult)response).Value;
            Assert.That(obj, Is.EqualTo(expectedUser));
            repository.VerifyAll();
        }

        [Test]
        public void RegisterThrowsIfUserNameAlreadyExistsInDb()
        {
            // Arrange
            const string expectedEmail = "test@test.com";
            const string expectedPassword = "password";

            var request = new RegistrationRequest() { EmailAddress = expectedEmail, Password = expectedPassword };

            var existingUser = new User() { EmailAddress = expectedEmail, Id = new MongoDB.Bson.ObjectId() };
            var repository = new Mock<IGenericRepository>();
            repository.Setup(u => u.SingleOrDefault(It.IsAny<Expression<Func<User, bool>>>())).Returns(existingUser);

            var controller = new RegisterController(new Mock<ILogger<RegisterController>>().Object, repository.Object);

            // Act
            var response = controller.Register(request).Result;

            // Assert
            Assert.That(response, Is.TypeOf(typeof(BadRequestObjectResult)));
            var obj = ((BadRequestObjectResult)response).Value;
            Assert.That(obj, Is.TypeOf(typeof(ArgumentException)));
        }
    }
}