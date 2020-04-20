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
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IConfiguration _configuration;
        private IMongoClient _client;

        public LoginController(ILogger<LoginController> logger, IConfiguration configuration, IMongoClient client)
        {
            _logger = logger;
            _configuration = configuration;
            _client = client;
        }

        /// <summary>Log in.</summary>
        [HttpPost]
        public ActionResult<User> Login(LoginRequest request)
        {
            var emailAddress = request.EmailAddress;
            var plainTextPassword = request.Password;

            var database = _configuration.GetSection("MongoDb:Database").Value;
            var mongoSettings = new MongoCollectionSettings() { AssignIdOnInsert = true };
            var users = _client.GetDatabase(database).GetCollection<User>("User", mongoSettings);

            var user = users.Find(u => u.EmailAddress == emailAddress).SingleOrDefault();
            if (user == null)
            {
                return BadRequest(new ArgumentException(nameof(emailAddress)));
            }

            var auths = _client.GetDatabase(database).GetCollection<Auth>("Auth", mongoSettings);
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
