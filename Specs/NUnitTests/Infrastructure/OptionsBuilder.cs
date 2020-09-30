using Microsoft.Extensions.Options;
using WeatherWalkingSkeleton.Config;

namespace WeatherWalkingSkeleton.NunitTests.Infrastructure
{
    public static class OptionsBuilder
    {
        public static IOptions<OpenWeather> OpenWeatherConfig()
        {
            return Options.Create<OpenWeather>(new OpenWeather { ApiKey = "00000" });
        }
    }
}
