using Phlank.Responder.WeatherExample.Models;

namespace Phlank.Responder.WeatherExample.Interfaces
{
    public interface IWeatherService
    {
        WeatherForecast GetRandomWeatherForecast(int daysAhead, string units);
    }
}