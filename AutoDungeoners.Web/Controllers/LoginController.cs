using System;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        private readonly IConfiguration configuration;
        private readonly IMongoClient client;

        public LoginController(ILogger<LoginController> logger, IConfiguration configuration, IMongoClient client)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.client = client;
        }

        /// <summary>Log in.</summary>
        [HttpPost]
        public ActionResult<User> Login(LoginRequest request)
        {
            var emailAddress = request.EmailAddress;
            var plainTextPassword = request.Password;

            var usersRepo = new MongoRepository<User>(this.configuration, this.client);
            var user = usersRepo.SingleOrDefault(u => u.EmailAddress == emailAddress);
            if (user == null)
            {
                return BadRequest(new ArgumentException(nameof(emailAddress)));
            }

            var authsRepo = new MongoRepository<Auth>(this.configuration, this.client);
            var userCredentials = authsRepo.SingleOrDefault(a => a.UserId == user.Id);
            
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
