using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using AutoDungeoners.Web.DataAccess;
using AutoDungeoners.Web.DataAccess.Repositories;
using AutoDungeoners.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AutoDungeoners.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> logger;
        private IMongoRepository repository;

        public LoginController(ILogger<LoginController> logger, IMongoRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        /// <summary>Log in.</summary>
        [HttpPost]
        public ActionResult<User> Login(LoginRequest request)
        {
            var emailAddress = request.EmailAddress;
            var plainTextPassword = request.Password;

            var users = repository.GetCollection<User>();
            var user = users.Find(u => u.EmailAddress == emailAddress).SingleOrDefault();
            if (user == null)
            {
                return BadRequest(new ArgumentException(nameof(emailAddress)));
            }

            var auths = repository.GetCollection<Auth>();
            var userCredentials = auths.Find(a => a.UserId == user.Id).SingleOrDefault();
            
            var hashedPassword = plainTextPassword;
            if (userCredentials == null || userCredentials.HashedPassword != hashedPassword)
            {
                return BadRequest(new InvalidOperationException(nameof(plainTextPassword)));
            }

            return Ok(user);
        }

        public class LoginRequest
        {
            public string EmailAddress { get; set; }
            public string Password { get; set; }
        }
    }
}
