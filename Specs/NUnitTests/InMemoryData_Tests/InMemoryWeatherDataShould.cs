using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using WeatherWalkingSkeleton.Core;
using WeatherWalkingSkeleton.Data;
using System.Linq;

namespace NunitTests.InMemoryData_Tests
{
    public class InMemoryWeatherDataShould
    {
        private IWeatherData _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new InMemoryWeatherData();
        }

        [Test]
        public void Get_specific_city_weather_temperatures(){
            var expectedCityTemperatures = new List<CityTemperature>()
            {
                new CityTemperature(){ Id = 1, CityName = "Wellington", DateTime = DateTime.Parse("0001-01-01T00:02:40.14564"), Temperature = 20.0M},
                new CityTemperature() { Id = 2, CityName = "Wellington", DateTime = DateTime.Parse("0001-01-01T00:02:40.14564"), Temperature = 15M}
                
            };

            var allCityTemperatures = _sut.GetCityTemperatures("wellington");

            allCityTemperatures.Should().BeEquivalentTo(expectedCityTemperatures);
        }

        [Test]
        public void Get_all_city_weather_temperatures()
        {
            var expectedCityTemperatures = new List<CityTemperature>()
            {
                new CityTemperature(){ Id = 1, CityName = "Wellington", DateTime = DateTime.Parse("0001-01-01T00:02:40.14564"), Temperature = 20.0M},
                new CityTemperature() { Id = 2, CityName = "Wellington", DateTime = DateTime.Parse("0001-01-01T00:02:40.14564"), Temperature = 15M},
                new CityTemperature() { Id = 3, CityName = "New York", DateTime = DateTime.Parse("0001-01-01T00:02:40.14564"), Temperature = 20.0M}
            };

            var allCityTemperatures = _sut.GetAllCityTemperatures();

            allCityTemperatures.Should().BeEquivalentTo(expectedCityTemperatures); 
        }

        [Test]
        public void Add_new_weather_temperature()
        {
            var expectedCityTemperatures = new List<CityTemperature>()
            {
                new CityTemperature(){ Id = 1, CityName = "Wellington", DateTime = DateTime.Parse("0001-01-01T00:02:40.14564"), Temperature = 20.0M },
                new CityTemperature() { Id = 2, CityName = "Wellington", DateTime = DateTime.Parse("0001-01-01T00:02:40.14564"), Temperature = 15M },
                new CityTemperature() { Id = 3, CityName = "New York", DateTime = DateTime.Parse("0001-01-01T00:02:40.14564"), Temperature = 20.0M },
                new CityTemperature() { Id = 4, CityName = "Taupo", DateTime = DateTime.Parse("0001-01-01T02:02:40.14564"), Temperature = 20.0M }

            };
            _sut.AddCityTemperature(new CityTemperature()
            {
                Id = 3,
                CityName = "Taupo",
                DateTime = DateTime.Parse("0001-01-01T02:02:40.14564"),
                Temperature = 20.0M
            });
            var allCityTemperatures = _sut.GetAllCityTemperatures();

            allCityTemperatures.Should().BeEquivalentTo(expectedCityTemperatures);
        }

        [Test]
        public void Update_city_temperature()
        {
            var expectedCityTemperatureTemperature = 20.5M;

            _sut.UpdateTemperature(1, null, expectedCityTemperatureTemperature);
            var updatedCityTemperature = _sut
                .GetAllCityTemperatures()
                .SingleOrDefault(t => t.Id == 1);

            updatedCityTemperature.Temperature.Should().Be(expectedCityTemperatureTemperature);
        }

        [Test]
        public void Update_city_date_time()
        {
            var expectedCityTemperatureDateTime = DateTime.Parse("0001-01-01T00:02:40.20000");

            _sut.UpdateTemperature(1, expectedCityTemperatureDateTime, null );
            var updatedCityTemperature = _sut
                .GetAllCityTemperatures()
                .SingleOrDefault(t => t.Id == 1);

            updatedCityTemperature.DateTime.Should().Be(expectedCityTemperatureDateTime);
        }

        [Test]
        public void Update_city_datetime_and_temperature()
        {
            var expectedCityTemperatureDateTime = DateTime.Parse("0001-01-01T00:02:40.20000");
            var expectedCityTemperatureTemperature = 20.5M;

            _sut.UpdateTemperature(1, expectedCityTemperatureDateTime, expectedCityTemperatureTemperature);
            var updatedCityTemperature = _sut
                .GetAllCityTemperatures()
                .SingleOrDefault(t => t.Id == 1);

            updatedCityTemperature.DateTime.Should().Be(expectedCityTemperatureDateTime);
            updatedCityTemperature.Temperature.Should().Be(expectedCityTemperatureTemperature);
        }

        [Test]
        public void Remove_citye_temperature()
        {
            var expectedCityTemperatures = new List<CityTemperature>()
            {
                new CityTemperature(){ Id = 1, CityName = "Wellington", DateTime = DateTime.Parse("0001-01-01T00:02:40.14564"), Temperature = 20.0M},
                new CityTemperature() { Id = 3, CityName = "New York", DateTime = DateTime.Parse("0001-01-01T00:02:40.14564"), Temperature = 20.0M}
            };

            var remainingCityTemperatures = _sut.GetAllCityTemperatures();

            _sut.RemoveCityTemperature(2);

            remainingCityTemperatures.Should().BeEquivalentTo(expectedCityTemperatures);

        }
    }
}
