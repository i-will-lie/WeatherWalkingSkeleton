using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using WeatherWalkingSkeleton.Controllers;
using WeatherWalkingSkeleton.Infrastructure;
using WeatherWalkingSkeleton.Services;
using WeatherWalkingSkeleton.NunitTests.Infrastructure;
using NUnit.Framework;
using FluentAssertions;
using System.Net.Http;
using WeatherWalkingSkeleton.Config;
using Microsoft.Extensions.Options;
using NSubstitute;
using WeatherWalkingSkeleton.Core;

namespace WeatherWalkingSkeleton.NunitTests.Controllers_Tests
{

    public class WeatherForecastController_Should
    {

        private IOptions<OpenWeather> _openWeatherConfiguration;
        private IHttpClientFactory _clientFactory;
        private OpenWeatherService _openWeatherService;
   
        [SetUp]
        public void Setup()
        {
           _openWeatherConfiguration = OptionsBuilder.OpenWeatherConfig();
           _clientFactory = Substitute.For<IHttpClientFactory>();

        }

        [Test]
        public async Task Return_Okresult_With_Weatherforecast()
        {
           // _clientFactory = ClientBuilder.OpenWeatherClientFactory(OpenWeatherResponses.OkResponse);

            var handler = new HttpMessageHandlerMock(OpenWeatherResponses.OkResponse);
            var client = new HttpClient(handler);
            _clientFactory.CreateClient().Returns<HttpClient>(client);

            _openWeatherService = new OpenWeatherService(_openWeatherConfiguration, _clientFactory);

            var sut = new WeatherForecastController(new NullLogger<WeatherForecastController>(), _openWeatherService);

            var reponse = await sut.Get("Chicago") as OkObjectResult;

            reponse.Value.Should().BeOfType<List<WeatherForecast>>();

            reponse.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task Return_400_result_when_location_not_provided()
        {
            var handler = new HttpMessageHandlerMock(OpenWeatherResponses.OkResponse);

            var client = new HttpClient(handler);
            _clientFactory = Substitute.For<IHttpClientFactory>();
            _clientFactory.CreateClient().Returns<HttpClient>(client);
            _openWeatherService = new OpenWeatherService(_openWeatherConfiguration, _clientFactory);

            var sut = new WeatherForecastController(new NullLogger<WeatherForecastController>(), _openWeatherService);

            var response = await sut.Get(String.Empty) as ObjectResult;

            response.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task Return_badrequestresult_when_location_is_not_found()
        {
            var handler = new HttpMessageHandlerMock(OpenWeatherResponses.NotFoundResponse,
                HttpStatusCode.NotFound);

            var client = new HttpClient(handler);

            _clientFactory = Substitute.For<IHttpClientFactory>();
            _clientFactory.CreateClient().Returns<HttpClient>(client);
            _openWeatherService = new OpenWeatherService(_openWeatherConfiguration, _clientFactory);

            var sut = new WeatherForecastController(new NullLogger<WeatherForecastController>(), _openWeatherService);

            var response = await sut.Get("Westworld") as ObjectResult;

            response.Value.ToString().Should().Contain("not found");
            response.StatusCode.Should().Be(400);
        }

        [Test]
        public void Return_openweatherexception_when_unauthorized_access()
        {
            var handler = new HttpMessageHandlerMock(OpenWeatherResponses.UnauthorizedResponse,
                HttpStatusCode.Unauthorized);

            var client = new HttpClient(handler);

            _clientFactory = Substitute.For<IHttpClientFactory>();
            _clientFactory.CreateClient().Returns<HttpClient>(client);
            _openWeatherService = new OpenWeatherService(_openWeatherConfiguration, _clientFactory);

            var sut = new OpenWeatherService(_openWeatherConfiguration, _clientFactory);

               var response = Assert.ThrowsAsync<OpenWeatherException>(async () => await sut.GetFiveDayForecastAsync("Chicago"));
               response.StatusCode.Should().Be(401);

            /*Func<Task> act = async () => { await sut.GetFiveDayForecastAsync("Chicago").ThrowAsync<ArgumentException>(); };
*/ }

        [Test]
        public async Task Returns_500_when_api_returns_error()
        {
            var handler = new HttpMessageHandlerMock(OpenWeatherResponses.UnauthorizedResponse,
                HttpStatusCode.Unauthorized);
            var client = new HttpClient(handler);

            _clientFactory = Substitute.For<IHttpClientFactory>();
            _clientFactory.CreateClient().Returns<HttpClient>(client);
            _openWeatherService = new OpenWeatherService(_openWeatherConfiguration, _clientFactory);

           
            var sut = new WeatherForecastController(new NullLogger<WeatherForecastController>(), _openWeatherService);

            var response = await sut.Get("Rio de Janeiro") as ObjectResult;

            response.Value.ToString().Should().Contain("Error response from OpenWeatherApi: Unauthorized");
            response.StatusCode.Should().Be(500);
        }

    }
}