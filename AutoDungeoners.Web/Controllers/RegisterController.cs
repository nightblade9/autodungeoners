using System;
using System.Linq;
using AutoDungeoners.Web.DataAccess.Repositories;
using AutoDungeoners.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace AutoDungeoners.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> logger;
        private IMongoRepository repository;

        public RegisterController(ILogger<RegisterController> logger, IMongoRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }
        
        /// <summary>
        /// Registers a new user. Returns the user's ID if successful.
        /// </summary>
        [HttpPost]
        public ActionResult<User> Register(RegistrationRequest request)
        {
            // TODO: validate email address
            // TODO: validate password is sufficiently long

            var emailAddress = request.EmailAddress;
            var plainTextPassword = request.Password;

            var newUser = new User() { EmailAddress = emailAddress };
            var users = repository.GetCollection<User>();

            var existingUser = users.Find(u => u.EmailAddress == emailAddress).SingleOrDefault();
            if (existingUser != null)
            {
                return BadRequest(new ArgumentException(nameof(emailAddress)));
            }

            users.InsertOne(newUser);
            newUser = users.Find(u => u.EmailAddress == emailAddress).Single();

            var auth = new Auth() { UserId = newUser.Id, HashedPassword = plainTextPassword, Salt = "TODO" };
            var auths = repository.GetCollection<Auth>();
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
