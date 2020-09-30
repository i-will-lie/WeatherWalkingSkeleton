using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherWalkingSkeleton.Data;

namespace WeatherWalkingSkeleton.Api.Controllers
{
    public class CityTemperatureDataController : ControllerBase
    {
        public CityTemperatureDataController(IWeatherData weatherData)
        {

        }
    }
}
