using System;
using System.ComponentModel.DataAnnotations;

namespace Phlank.Responder.WeatherExample.Models
{
    public class WeatherForecastRequest
    {
        [Range(0, int.MaxValue, ErrorMessage = "DaysAhead must be greater than zero.")]
        public int DaysAhead { get; set; }

        [RegularExpression(@"^[CcKkFf]$", ErrorMessage = "Acceptable values for TemperatureFormat are C, F, and K.")]
        public string TemperatureUnits { get; set; } = "F";
    }
}
