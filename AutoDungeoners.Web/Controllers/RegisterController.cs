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
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> logger;
        private readonly IUserRepository userRepository;
        private readonly IAuthRepository authRepository;

        public RegisterController(ILogger<RegisterController> logger, IUserRepository userRepository, IAuthRepository authRepository)
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.authRepository = authRepository;
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
            var existingUser = this.userRepository.FindOneByEmail(emailAddress);
            if (existingUser != null)
            {
                return BadRequest(new ArgumentException(nameof(emailAddress)));
            }

            this.userRepository.Insert(newUser);
            newUser = this.userRepository.FindOneByEmail(emailAddress); // Load back with ID

            var auth = new Auth() { UserId = newUser.Id, HashedPassword = plainTextPassword, Salt = "TODO" };
            this.authRepository.Insert(auth);

            return Ok(newUser);
        }

        public class RegistrationRequest
        {
            public string EmailAddress { get; set; }
            public string Password { get; set; }
        }
    }
}
