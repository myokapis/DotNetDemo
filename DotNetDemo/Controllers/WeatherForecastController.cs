using DotNetDemo.Configs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DotNetDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly DemoConfig config;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IOptions<DemoConfig> options, ILogger<WeatherForecastController> logger)
        {
            //Configs.DemoConfig config,
            config = options.Value;
            _logger = logger;
        }

        [HttpGet("GetWeatherForecast", Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetConfigs", Name = "GetConfigs")]
        public IActionResult GetConfigs()
        {
 
            return Ok(config);
        }

        [HttpGet("GetAnError", Name = "GetAnError")]
        public IActionResult GetAnError()
        {
            throw new Exception("Boink!");
        }
    }
}
