using System;
using System.Collections.Generic;
using WeatherWalkingSkeleton.Core;

namespace WeatherWalkingSkeleton.Data
{
    public interface IWeatherData
    {
        IEnumerable<CityTemperature> GetAllCityTemperatures();

        IEnumerable<CityTemperature> GetCityTemperatures(string city);

        void AddCityTemperature(CityTemperature cityTemperature);

        void UpdateTemperature(int cityTemperatureId, DateTime? dateTime, decimal? temperature);
        void RemoveCityTemperature(int cityId);
    }
}
