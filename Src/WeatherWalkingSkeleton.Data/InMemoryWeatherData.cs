using System;
using System.Collections.Generic;
using System.Linq;
using WeatherWalkingSkeleton.Core;

namespace WeatherWalkingSkeleton.Data
{
    public class InMemoryWeatherData : IWeatherData
    {
        private readonly List<CityTemperature> _cityTemperatures = new List<CityTemperature>()
        {
            new CityTemperature(){ Id = 1, CityName = "Wellington", DateTime = DateTime.Parse("0001-01-01T00:02:40.14564"), Temperature = 20.0M},
            new CityTemperature() { Id = 2, CityName = "Wellington", DateTime = DateTime.Parse("0001-01-01T00:02:40.14564"), Temperature = 15M},
            new CityTemperature() { Id = 3, CityName = "New York", DateTime = DateTime.Parse("0001-01-01T00:02:40.14564"), Temperature = 20.0M}
        };

        public IEnumerable<CityTemperature> GetAllCityTemperatures()
        {
            return from city in _cityTemperatures
                   orderby city.DateTime
                   select city;
        }
    
        public IEnumerable<CityTemperature> GetCityTemperatures(string cityName)
        {
            return from city in _cityTemperatures
                   where String.Compare(city.CityName, cityName, true) == 0
                   select city;
        }

        public void AddCityTemperature(CityTemperature cityTemperature)
        {
            _cityTemperatures.Add(cityTemperature);
        }

        public void UpdateTemperature(int cityTemperatureId, DateTime? dateTime, decimal? temperature)
        {
            var cityTemperatureToUpdate = from city in _cityTemperatures
                                          where city.Id == cityTemperatureId
                                          select city;

            var cityTemperatureResult = cityTemperatureToUpdate.ToList<CityTemperature>();
            if (cityTemperatureResult.Count == 1) 
            {
            
                if(dateTime != null)
                {
                    cityTemperatureResult[0].DateTime = (DateTime)dateTime;
                }

                if(temperature != null)
                {
                    cityTemperatureResult[0].Temperature = (decimal)temperature;
                }
            }

        }

        public void RemoveCityTemperature(int cityId)
        {
            var targetCityTemperature = _cityTemperatures.SingleOrDefault(c => c.Id == cityId);


            _cityTemperatures.Remove(targetCityTemperature);

        }
    }
}
