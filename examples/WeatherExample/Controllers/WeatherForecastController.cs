using Microsoft.AspNetCore.Mvc;
using Phlank.Responder.WeatherExample.Interfaces;
using Phlank.Responder.WeatherExample.Models;
using System.Collections.Generic;

namespace Phlank.Responder.WeatherExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IResponder _resultBuilder;
        private readonly IWeatherService _weatherService;

        public WeatherForecastController(IResponder responseBuilder, IWeatherService weatherService)
        {
            _resultBuilder = responseBuilder;
            _weatherService = weatherService;
        }

        private static readonly Warning LowConfidenceForecastWarning = new Warning(
            Severity.Low,
            "Forecasts seven days or more into the future may not be accurate and are subject to change.",
            new Dictionary<string, object>()
            {
                { "Fields", new List<string>() { "DaysAhead" } }
            });

        private static readonly Warning NoConfidenceForecastWarning = new Warning(
            Severity.High,
            "Forecasts ten or more days into the future are extremely volatile and should not be relied upon for any circumstance.",
            new Dictionary<string, object>()
            {
                { "Fields", new List<string>() { "DaysAhead" } }
            });
        
        [HttpGet]
        public ResponderResult GetForecast(WeatherForecastRequest request)
        {
            if (request.DaysAhead > 10) _resultBuilder.AddWarning(NoConfidenceForecastWarning);
            else if (request.DaysAhead > 7) _resultBuilder.AddWarning(LowConfidenceForecastWarning);

            var content = _weatherService.GetRandomWeatherForecast(request.DaysAhead, request.TemperatureUnits);
            _resultBuilder.AddContent(content);
            return _resultBuilder.Build(this);
        }
    }
}
