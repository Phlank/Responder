using System;

namespace Phlank.Responder.WeatherExample.Models
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }
        public int TemperatureHigh { get; set; }
        public int TemperatureLow { get; set; }
        public string Summary { get; set; }
    }
}
