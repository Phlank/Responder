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
        private readonly IApiResponseBuilder _responseBuilder;
        private readonly IWeatherService _weatherService;

        public WeatherForecastController(IApiResponseBuilder responseBuilder, IWeatherService weatherService)
        {
            _responseBuilder = responseBuilder;
            _weatherService = weatherService;
        }

        private static ApiWarning LowConfidenceForecastWarning = new ApiWarning
        {
            Code = "LowConfidenceForecast",
            Fields = new List<string> { "DaysAhead" },
            Message = "Forecasts seven days or more into the future may not be accurate and are subject to change.",
            Severity = Severity.Low
        };

        private static ApiWarning NoConfidenceForecastWarning = new ApiWarning
        {
            Code = "NoConfidenceForecast",
            Fields = new List<string> { "DaysAhead" },
            Message = "Forecasts ten or more days into the future are extremely volatile and should not be relied upon for any circumstance.",
            Severity = Severity.High
        };

        [HttpGet]
        public ApiResponse<WeatherForecast> GetForecast(WeatherForecastRequest request)
        {

            if (request.DaysAhead > 10) _responseBuilder.WithWarning(NoConfidenceForecastWarning);
            else if (request.DaysAhead > 7) _responseBuilder.WithWarning(LowConfidenceForecastWarning);

            return _responseBuilder.Build(_weatherService.GetRandomWeatherForecast(request.DaysAhead, request.TemperatureUnits));
        }
    }
}
