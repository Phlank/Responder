using Phlank.Responder.WeatherExample.Models;

namespace Phlank.Responder.WeatherExample.Services
{
    public interface IWeatherService
    {
        WeatherForecast GetRandomWeatherForecast(int daysAhead, string units);
    }
}