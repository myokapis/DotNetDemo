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

        private readonly DemoConfig demoConfig;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IOptions<DemoConfig> options, ILogger<WeatherForecastController> logger)
        {
            demoConfig = options.Value;
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

        /// <summary>
        /// An endpoint to see what application configuration has been loaded.
        /// </summary>
        /// <returns>The application configuration hash</returns>
        [HttpGet("GetConfigs", Name = "GetConfigs")]
        public IActionResult GetConfigs()
        {
 
            return Ok(demoConfig);
        }

        /// <summary>
        /// Raises an error to simulate how run time errors will get redirected to the errors
        /// controller to be logged and a safe response returned.
        /// To use this feature, <code>app.UseExceptionHandler("/Error");</code> must be in your pipeline.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("GetAnError", Name = "GetAnError")]
        public IActionResult GetAnError()
        {
            throw new Exception("Boink!");
        }
    }
}
