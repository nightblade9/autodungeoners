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
        private readonly IUserRepository userRepository;
        private readonly IAuthRepository authRepository;
        private readonly GenericRepository genericRepository;

        public LoginController(ILogger<LoginController> logger, IUserRepository userRepository, IAuthRepository authRepository, GenericRepository genericRepository)
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.authRepository = authRepository;
            this.genericRepository = genericRepository;
        }

        /// <summary>Log in.</summary>
        [HttpPost]
        public ActionResult<User> Login(LoginRequest request)
        {
            var emailAddress = request.EmailAddress;
            var plainTextPassword = request.Password;

            var user = this.userRepository.FindOneByEmail(emailAddress);
            user = this.genericRepository.SingleOrDefault<User>(u => u.EmailAddress == emailAddress);
            
            if (user == null)
            {
                return BadRequest(new ArgumentException(nameof(emailAddress)));
            }

            var userCredentials = this.authRepository.FindOneById(user.Id);
            var hashedPassword = plainTextPassword;
            if (userCredentials == null || userCredentials.HashedPassword != hashedPassword)
            {
                return BadRequest(new ArgumentException(nameof(plainTextPassword)));
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
