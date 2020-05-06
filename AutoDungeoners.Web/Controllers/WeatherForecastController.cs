using System;
using System.Collections.Generic;
using System.Linq;
using AutoDungeoners.Web.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AutoDungeoners.Web.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class WeatherForecastController : AutoDungeonersController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IGenericRepository _genericRepo;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration, IGenericRepository genericRepository)
        : base(genericRepository)
        {
            _logger = logger;
            _genericRepo = genericRepository;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
