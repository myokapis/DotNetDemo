using DotNetDemo.Models;

namespace DotNetDemo.Services
{
    public interface IWeatherService
    {
        public IEnumerable<WeatherForecast> WeatherForecast(int forecastDays);
    }

    public class WeatherService : IWeatherService
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        public IEnumerable<WeatherForecast> WeatherForecast(int forecastDays)
        {
            return Enumerable.Range(1, forecastDays).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
