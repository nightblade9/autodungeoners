using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoDungeoners.Web.DataAccess.Repositories;
using AutoDungeoners.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AutoDungeoners.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<LoginController> logger;
        private readonly IGenericRepository genericRepository;

        public LoginController(ILogger<LoginController> logger, IConfiguration configuration, IGenericRepository genericRepository)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.genericRepository = genericRepository;
        }

        /// <summary>Log in.</summary>
        [HttpPost]
        public ActionResult<string> Login(LoginRequest request)
        {
            var emailAddress = request.EmailAddress;
            var plainTextPassword = request.Password;

            var user = this.genericRepository.SingleOrDefault<User>(u => u.EmailAddress == emailAddress);
            
            if (user == null)
            {
                return Unauthorized(new ArgumentException(nameof(emailAddress)));
            }

            var userCredentials = this.genericRepository.SingleOrDefault<Auth>(a => a.UserId == user.Id);
            var hashedPassword = plainTextPassword;
            if (userCredentials == null || userCredentials.HashedPassword != hashedPassword)
            {
                return Unauthorized(new ArgumentException(nameof(emailAddress)));
            }

            return Ok(new {
                token = GenerateJsonWebToken(user),
            });
        }
        
        // https://www.c-sharpcorner.com/article/jwt-json-web-token-authentication-in-asp-net-core/
        private string GenerateJsonWebToken(User user)  
        {  
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:Key"]));  
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);  
  
            var claims = new[] {  
                new Claim(JwtRegisteredClaimNames.Sub, user.EmailAddress),  
                new Claim(JwtRegisteredClaimNames.Email, user.EmailAddress),  
                //new Claim("DateOfJoing", user.DateOfJoing.ToString("yyyy-MM-dd")),  
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };  

            var token = new JwtSecurityToken(this.configuration["Jwt:Issuer"],  
              this.configuration["Jwt:Issuer"],  
              claims,  
              expires: DateTime.Now.AddMinutes(120),  
              signingCredentials: credentials);  
  
            return new JwtSecurityTokenHandler().WriteToken(token);  
        }

        public class LoginRequest
        {
            public string EmailAddress { get; set; }
            public string Password { get; set; }
        }
    }
}
