using DotNetDemo.Configs;
using DotNetDemo.Models;
using DotNetDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sharpbrake.Client;

namespace DotNetDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(IOptions<DemoConfig> options, IWeatherService weatherService) : ControllerBase
    {
        private readonly DemoConfig demoConfig = options.Value;
        private readonly IWeatherService weatherService = weatherService;

        [HttpGet("GetWeatherForecast", Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> GetWeatherForecast(int forecastDays = 5)
        {
            return weatherService.WeatherForecast(forecastDays);
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

        [HttpGet("GetHealth", Name = "GetHealth")]
        public IActionResult GetHealth()
        {
            return StatusCode(200, "I'm working harder than you are!");
        }
    }
}
