using Phlank.ApiModeling.WeatherExample.Interfaces;
using Phlank.ApiModeling.WeatherExample.Models;
using System;

namespace Phlank.ApiModeling.WeatherExample.Services
{
    public class WeatherService : IWeatherService
    {
        public WeatherService() { }

        public WeatherForecast GetRandomWeatherForecast(int daysAhead, string units)
        {
            var random = new Random();
            var lowTempC = random.Next(-17, 35);
            var highTempC = random.Next(lowTempC, Math.Min(lowTempC + 5, 37));

            int lowTemp, highTemp;

            if (units.ToUpperInvariant() == "C")
            {
                lowTemp = lowTempC;
                highTemp = highTempC;
            }
            else if (units.ToUpperInvariant() == "K")
            {
                lowTemp = ConvertCelsiusToKelvin(lowTempC);
                highTemp = ConvertCelsiusToKelvin(highTempC);
            }
            else
            {
                lowTemp = ConvertCelsiusToFahrenheit(lowTempC);
                highTemp = ConvertCelsiusToFahrenheit(highTempC);
            }

            return new WeatherForecast
            {
                Date = DateTime.Now.Date + new TimeSpan(daysAhead, 0, 0, 0),
                TemperatureLow = lowTemp,
                TemperatureHigh = highTemp
            };
        }

        private static int ConvertCelsiusToKelvin(int celsius)
        {
            return celsius + 273;
        }

        private static int ConvertCelsiusToFahrenheit(int celsius)
        {
            return (int)Math.Round(celsius * 9.0 / 5.0 + 32);
        }
    }
}
