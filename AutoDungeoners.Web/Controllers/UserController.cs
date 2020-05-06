using System;
using AutoDungeoners.Web.DataAccess.Repositories;
using AutoDungeoners.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoDungeoners.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : AutoDungeonersController
    {
        private readonly ILogger<UserController> logger;
        private readonly IGenericRepository genericRepository;

        public UserController(ILogger<UserController> logger, IGenericRepository genericRepository)
        : base(genericRepository)
        {
            this.logger = logger;
            this.genericRepository = genericRepository;
        }

        /// <summary>Log in.</summary>
        [HttpGet]
        public ActionResult<string> WhoAmI()
        {
            var currentUser = this.CurrentUser;
            if (currentUser != null)
            {
                return Ok(new {
                    currentUser
                });
            }
            else
            {
                // shold be impossible; [Authorize] amirite?
                return BadRequest(new InvalidOperationException("User is not logged in!"));
            }
        }
    }
}
