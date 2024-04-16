using Xunit;
using FluentAssertions;
using DotNetDemo.Services;

namespace DotNetDemo.Tests
{
    public class WeatherServiceTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(6)]
        public void TestWeatherForecast(int forecastDays)
        {
            var service = new WeatherService();
            var result = service.WeatherForecast(forecastDays);

            result.Count().Should().Be(forecastDays);
        }
    }
}
