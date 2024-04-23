using Courier_lockers.Data;
using Courier_lockers.Entities;
using Courier_lockers.Services;
using Microsoft.AspNetCore.Mvc;

namespace Courier_lockers.Controllers
{
    [ApiController]
    [Route("api/Device/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
         };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ITest _test;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, ITest test)
        {
            _logger = logger;
            _test = test ?? throw new ArgumentNullException(nameof(test));
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task<ActionResult<List<edpmain>>> GetTest()
        {
            var st= _test.getAllTest();
            return Ok(await _test.getAllTest());
        }
    }
}