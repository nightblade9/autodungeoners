using System;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _configuration;
        private IMongoClient _client;

        public UserController(ILogger<UserController> logger, IConfiguration configuration, IMongoClient client)
        {
            _logger = logger;
            _configuration = configuration;
            _client = client;
        }

        [HttpPost]
        /// <summary>
        /// Registers a new user. Returns the user's ID if successful.
        /// </summary>
        public ActionResult<User> Register(RegistrationRequest request)
        {
            // TODO: validate email address
            // TODO: validate password is sufficiently long

            var emailAddress = request.EmailAddress;
            var plainTextPassword = request.Password;

            var newUser = new User() { EmailAddress = emailAddress };
            var database = _configuration.GetSection("MongoDb:Database").Value;
            var mongoSettings = new MongoCollectionSettings() { AssignIdOnInsert = true };
            var users = _client.GetDatabase(database).GetCollection<User>("User", mongoSettings);

            var existingUser = users.Find(u => u.EmailAddress == emailAddress).SingleOrDefault();
            if (existingUser != null)
            {
                return BadRequest(new ArgumentException(nameof(emailAddress)));
            }

            users.InsertOne(newUser);
            newUser = users.Find(u => u.EmailAddress == emailAddress).Single();

            var auth = new Auth() { UserId = newUser.Id, HashedPassword = plainTextPassword, Salt = "TODO" };
            var auths = _client.GetDatabase(database).GetCollection<Auth>("Auth", mongoSettings);
            auths.InsertOne(auth);

            return Ok(newUser);
        }

        public class RegistrationRequest
        {
            public string EmailAddress { get; set; }
            public string Password { get; set; }
        }
    }
}
