using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherWalkingSkeleton.Core;

namespace WeatherWalkingSkeleton.Services
{
    public enum Unit
    {
        Metric,
        Imperial,
        Kelvin
    }

    public interface IOpenWeatherService
    {
        Task<List<WeatherForecast>> GetFiveDayForecastAsync(string location, Unit unit = Unit.Metric);
    }
    // [...]
}