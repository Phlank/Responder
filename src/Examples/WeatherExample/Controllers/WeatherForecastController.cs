using Microsoft.AspNetCore.Mvc;
using Phlank.Responder.WeatherExample.Models;
using Phlank.Responder.WeatherExample.Services;
using Phlank.Responder;
using System.Collections.Generic;

namespace Phlank.Responder.WeatherExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IResponder _responder;
        private readonly IWeatherService _weatherService;

        public WeatherForecastController(IResponder responder, IWeatherService weatherService)
        {
            _responder = responder;
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
        public ResponderResult<WeatherForecast> GetForecast([FromRoute] WeatherForecastRequest request)
        {
            if (request.DaysAhead > 15) _responder.AddProblem(new Problem(System.Net.HttpStatusCode.NotAcceptable));
            else
            {
                if (request.DaysAhead > 10) _responder.AddWarning(NoConfidenceForecastWarning);
                else if (request.DaysAhead > 7) _responder.AddWarning(LowConfidenceForecastWarning);

                var content = _weatherService.GetRandomWeatherForecast(request.DaysAhead, request.TemperatureUnits);
                _responder.AddContent(content);
            }
            return _responder.Build<WeatherForecast>(this);
        }
    }
}
