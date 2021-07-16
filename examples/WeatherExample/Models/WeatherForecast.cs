using System;

namespace Phlank.ApiModeling.WeatherExample.Models
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }
        public int TemperatureHigh { get; set; }
        public int TemperatureLow { get; set; }
        public string Summary { get; set; }
    }
}
