using Microsoft.AspNetCore.Mvc;
using Phlank.ApiModeling.WeatherExample.Interfaces;
using Phlank.ApiModeling.WeatherExample.Models;
using System.Collections.Generic;

namespace Phlank.ApiModeling.WeatherExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IApiResultBuilder _resultBuilder;
        private readonly IWeatherService _weatherService;

        public WeatherForecastController(IApiResultBuilder responseBuilder, IWeatherService weatherService)
        {
            _resultBuilder = responseBuilder;
            _weatherService = weatherService;
        }

        private static readonly ApiWarning LowConfidenceForecastWarning = new ApiWarning
        {
            Code = "LowConfidenceForecast",
            Fields = new List<string> { "DaysAhead" },
            Message = "Forecasts seven days or more into the future may not be accurate and are subject to change.",
            Severity = Severity.Low
        };

        private static readonly ApiWarning NoConfidenceForecastWarning = new ApiWarning
        {
            Code = "NoConfidenceForecast",
            Fields = new List<string> { "DaysAhead" },
            Message = "Forecasts ten or more days into the future are extremely volatile and should not be relied upon for any circumstance.",
            Severity = Severity.High
        };

        [HttpGet]
        public ApiResult GetForecast(WeatherForecastRequest request)
        {
            if (request.DaysAhead > 10) _resultBuilder.WithWarning(NoConfidenceForecastWarning);
            else if (request.DaysAhead > 7) _resultBuilder.WithWarning(LowConfidenceForecastWarning);

            var content = _weatherService.GetRandomWeatherForecast(request.DaysAhead, request.TemperatureUnits);
            _resultBuilder.WithContent(content);
            return _resultBuilder.Build();
        }
    }
}
