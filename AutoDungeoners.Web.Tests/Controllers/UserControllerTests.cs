using System;
using AutoDungeoners.Web.Controllers;
using AutoDungeoners.Web.DataAccess.Repositories;
using AutoDungeoners.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;

namespace AutoDungeoners.Web.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        [Test]
        public void WhoAmIReturnsOkWithUser()
        {
            // Arrange. Use a partial mock because we have to mock CurrentUser.
            var controller = new Mock<UserController>(new Mock<ILogger<UserController>>().Object, new Mock<IGenericRepository>().Object) { CallBase = true };
            var expectedUser = new User() { Id = ObjectId.GenerateNewId() };
            controller.Setup(c => c.CurrentUser).Returns(expectedUser);

            // Act
            var response = controller.Object.WhoAmI().Result;

            // Assert
            Assert.That(response, Is.TypeOf(typeof(OkObjectResult)));
            var obj = ((OkObjectResult)response).Value;
            Assert.That(obj, Is.EqualTo(expectedUser));
        }

        [Test]
        public void WhoAmIReturnsBadRequestIfUserIsNotLoggedIn()
        {
            // Arrange. Use a partial mock because we have to mock CurrentUser.
            var controller = new Mock<UserController>(new Mock<ILogger<UserController>>().Object, new Mock<IGenericRepository>().Object) { CallBase = true };
            controller.Setup(c => c.CurrentUser).Returns((User)null);

            // Act
            var response = controller.Object.WhoAmI().Result;

            // Assert
             Assert.That(response, Is.TypeOf(typeof(BadRequestObjectResult)));
            var obj = ((BadRequestObjectResult)response).Value;
            Assert.That(obj, Is.TypeOf(typeof(InvalidOperationException)));
        }
    }
}