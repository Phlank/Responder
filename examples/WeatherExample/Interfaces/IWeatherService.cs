using Phlank.ApiModeling.WeatherExample.Models;

namespace Phlank.ApiModeling.WeatherExample.Interfaces
{
    public interface IWeatherService
    {
        WeatherForecast GetRandomWeatherForecast(int daysAhead, string units);
    }
}