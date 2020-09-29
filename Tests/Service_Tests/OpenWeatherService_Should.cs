using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tests.Infrastructure;
using WeatherWalkingSkeleton.Config;
using WeatherWalkingSkeleton.Infrastructure;
using WeatherWalkingSkeleton.Models;
using WeatherWalkingSkeleton.Tests.Infrastructure;


namespace WeatherWalkingSkeleton.Services
{
    public class OpenWeatherService_Should
    {
        private IOptions<OpenWeather> _openWeatherConfiguration;

        [SetUp]
        public void Setup()
        {
            _openWeatherConfiguration = OptionsBuilder.OpenWeatherConfig();
        }

        [Test]
        public async Task return_a_weatherforecast()
        {
            var handler = new HttpMessageHandlerMock(OpenWeatherResponses.OkResponse);

            var client = new HttpClient(handler);
            var clientFactory = Substitute.For<IHttpClientFactory>();
            clientFactory.CreateClient().Returns<HttpClient>(client);

            var sut = new OpenWeatherService(_openWeatherConfiguration, clientFactory);

            var weatherForecasts = await sut.GetFiveDayForecastAsync("Chicago");

            weatherForecasts.Should().BeOfType<List<WeatherForecast>>();
        }

        [Test]
        public async Task Returns_expected_forecasts_from_the_api()
        {
            var handler = new HttpMessageHandlerMock(OpenWeatherResponses.OkResponse);
            
            var client = new HttpClient(handler);
            var clientFactory = Substitute.For<IHttpClientFactory>();

            clientFactory.CreateClient().Returns<HttpClient>(client);

            var sut = new OpenWeatherService(_openWeatherConfiguration, clientFactory);

            var weatherForecastResponse = await sut.GetFiveDayForecastAsync("Chicago");

            weatherForecastResponse[0].Date.Should().Be(new DateTime(1594155600));
            weatherForecastResponse[0].Temp.Should().BeApproximately((decimal)32.93,2);
            //Assert.Equal(new DateTime(1594155600), result[0].Date);
            //assert.equal((decimal)32.93, result[0].temp);
        }

        [Test]
        public void Return_openweatherexception_when_called_with_bad_argument()
        {
            var handler = new HttpMessageHandlerMock(OpenWeatherResponses.OkResponse, HttpStatusCode.NotFound);

            var client = new HttpClient(handler);
            var clientFactory = Substitute.For<IHttpClientFactory>();
            clientFactory.CreateClient().Returns<HttpClient>(client);
          
       
            var sut = new OpenWeatherService(_openWeatherConfiguration, clientFactory);

            var response = Assert.ThrowsAsync<OpenWeatherException>(async () => await sut.GetFiveDayForecastAsync("Chicago"));
           
            var weatherForecastResponse = Assert.ThrowsAsync<OpenWeatherException>(async () => await sut.GetFiveDayForecastAsync("Westeros"));
            weatherForecastResponse.StatusCode.Should().Be(404);
    
        }

        [Test]
        public void Return_openweatherexception_when_unauthorized()
        {
            var handler = new HttpMessageHandlerMock(OpenWeatherResponses.UnauthorizedResponse,
                HttpStatusCode.Unauthorized);

            var client = new HttpClient(handler);
            var clientFactory = Substitute.For<IHttpClientFactory>();
            clientFactory.CreateClient().Returns<HttpClient>(client);


            var sut = new OpenWeatherService(_openWeatherConfiguration, clientFactory);

            var weatherForecastResponse =  Assert.ThrowsAsync<OpenWeatherException>(async () => await sut.GetFiveDayForecastAsync("Chicago"));
            weatherForecastResponse.StatusCode.Should().Be(401);
        }

        [Test]
        public  void Return_openweatherexception_on_openweatherinternalerror()
        {
            var handler = new HttpMessageHandlerMock(OpenWeatherResponses.InternalErrorResponse,
                HttpStatusCode.InternalServerError);

            var client = new HttpClient(handler);
            var clientFactory = Substitute.For<IHttpClientFactory>();
            clientFactory.CreateClient().Returns<HttpClient>(client);


    
            var sut = new OpenWeatherService(_openWeatherConfiguration, clientFactory);

            var weatherForecastResponse = Assert.ThrowsAsync<OpenWeatherException>(async () => await sut.GetFiveDayForecastAsync("New York"));
            weatherForecastResponse.StatusCode.Should().Be(500);
        }

        

    }

    
}
