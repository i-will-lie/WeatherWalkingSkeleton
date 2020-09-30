using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using WeatherWalkingSkeleton.Config;
using WeatherWalkingSkeleton.Infrastructure;
using WeatherWalkingSkeleton.Core;

namespace WeatherWalkingSkeleton.Services

{
    public class OpenWeatherService : IOpenWeatherService
    {

        private readonly OpenWeather _openWeatherConfig;
        private readonly IHttpClientFactory _httpFactory;
        public OpenWeatherService(IOptions<OpenWeather> opts, IHttpClientFactory httpFactory)
        {
            _openWeatherConfig = opts.Value;
            _httpFactory = httpFactory;
        }
        public async Task<List<WeatherForecast>> GetFiveDayForecastAsync(string location, Unit unit = Unit.Metric)
        {
            string url = BuildOpenWeatherUrl("forecast", location, unit);
            var client = _httpFactory.CreateClient();
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            // 2. Deserialize the response.
            var openWeatherResponse = JsonSerializer.Deserialize<OpenWeatherResponse>(json);
            // var openWeatherResponse = await GetWeatherResponse(location, unit);
            if (response.IsSuccessStatusCode)
            {
                // 3. Build the list of forecasts
                var forecasts = new List<WeatherForecast>();
                foreach (var forecast in openWeatherResponse.Forecasts)
                {
                    forecasts.Add(new WeatherForecast
                    {
                        Date = new DateTime(forecast.Dt),
                        Temp = forecast.Temps.Temp,
                        FeelsLike = forecast.Temps.FeelsLike,
                        TempMin = forecast.Temps.TempMin,
                        TempMax = forecast.Temps.TempMax,
                    });
                }
                return forecasts;
            }
            // build an exception with information from the third-party API
            throw new OpenWeatherException(response.StatusCode, 
                "Error response from OpenWeatherApi: " + response.ReasonPhrase);
        }
        /*
        public async Task<OpenWeatherResponse> GetWeatherResponse(string location, Unit unit = Unit.Metric)
        {
            string url = BuildOpenWeatherUrl("forecast", location, unit);
            var client = _httpFactory.CreateClient();
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            // 2. Deserialize the response.
            return JsonSerializer.Deserialize<OpenWeatherResponse>(json);
        }*/
        private string BuildOpenWeatherUrl(string resource, string location, Unit unit)
        {
            return $"https://api.openweathermap.org/data/2.5/{resource}" +
                   $"?appid={_openWeatherConfig.ApiKey}" +
                   $"&q={location}" +
                   $"&units={unit}";
        }

    }
}