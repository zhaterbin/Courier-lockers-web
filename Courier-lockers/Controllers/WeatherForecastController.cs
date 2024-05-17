using Courier_lockers.Data;
using Courier_lockers.Entities;
using Courier_lockers.Services;
using Courier_lockers.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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
        private readonly IHubContext<Myhub> _hubContext;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, ITest test, IHubContext<Myhub> hubContext)
        {
            _logger = logger;
            _test = test ?? throw new ArgumentNullException(nameof(test));
            _hubContext = hubContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _hubContext.Clients.All.SendAsync("GetWeatherForecast", Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray());
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