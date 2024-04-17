using FluentAssertions;
using DotNetDemo.Services;
using Microsoft.AspNetCore.Mvc;
using DotNetDemo.Controllers;
using DotNetDemo.Configs;
using Microsoft.Extensions.Options;
using Moq;
using DotNetDemo.Models;
using Castle.Core.Smtp;

namespace DotNetDemo.Tests.Controllers
{
    public class WeatherForecastControllerTests
    {
        private readonly IOptions<DemoConfig> mockDemoConfig = MockDemoConfig();

        [Fact]
        public void TestGetAnError()
        {
            var controller = new WeatherForecastController(mockDemoConfig, MockWeatherService());

            var error = Assert.Throws<Exception>(() => controller.GetAnError());
            error.Message.Should().Be("Boink!");
        }

        [Fact]
        public void TestGetConfigs()
        {
            var controller = new WeatherForecastController(mockDemoConfig, MockWeatherService());
            var response = controller.GetConfigs();

            var result = Assert.IsType<OkObjectResult>(response);
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be(mockDemoConfig.Value);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(6)]
        public void TestGetWeatherForecast(int forecastCount)
        {
            var controller = new WeatherForecastController(mockDemoConfig, MockWeatherService());
            var response = controller.GetWeatherForecast(forecastCount);

            var forecasts = Assert.IsAssignableFrom<IEnumerable<WeatherForecast>>(response);
            forecasts.Should().BeEquivalentTo(weatherForecasts.Take(forecastCount));
        }

        [Fact]
        public void TestReturnStatus()
        {
            var controller = new WeatherForecastController(mockDemoConfig, MockWeatherService());
            var response = controller.GetHealth();

            var result = Assert.IsType<ObjectResult>(response);
            result.StatusCode.Should().Be(200);
        }

        #region Test Class Setup

        private static WeatherForecast GetWeatherForecast(int index)
        {
            return new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = index + 25,
                Summary = $"SummaryText{index}"
            };
        }

        private static IOptions<DemoConfig> MockDemoConfig()
        {
            var mockConfig = new Mock<IOptions<DemoConfig>>();
            mockConfig.Setup(m => m.Value).Returns(new DemoConfig());
            return mockConfig.Object;
        }

        private static IWeatherService MockWeatherService()
        {
            var mockService = new Mock<IWeatherService>();

            mockService.Setup(m => m.WeatherForecast(It.IsAny<int>()))
                .Returns((int p1) => weatherForecasts.Take(p1));

            return mockService.Object;
        }

        private static readonly IEnumerable<WeatherForecast> weatherForecasts =
            Enumerable.Range(0, 10).Select(index => GetWeatherForecast(index)).ToList();

        #endregion
    }
}
