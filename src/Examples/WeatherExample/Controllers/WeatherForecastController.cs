using Microsoft.AspNetCore.Mvc;
using Phlank.Responder.WeatherExample.Models;
using Phlank.Responder.WeatherExample.Services;
using Phlank.Responder;
using System.Collections.Generic;

namespace Phlank.Responder.WeatherExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : Controller
    {
        private readonly IResponder _responder;
        private readonly IWeatherService _weatherService;

        public WeatherForecastController(IResponder responder, IWeatherService weatherService)
        {
            _responder = responder;
            _weatherService = weatherService;
        }

        [ProducesErrorResponseType(typeof(Problem))]
        [HttpGet]
        public ResponderResult<WeatherForecast> GetForecast([FromRoute] WeatherForecastRequest request)
        {
            if (request.DaysAhead > 15) _responder.AddProblem(new Problem(System.Net.HttpStatusCode.NotAcceptable));
            else
            {
                var content = _weatherService.GetRandomWeatherForecast(request.DaysAhead, request.TemperatureUnits);
                _responder.AddContent(content);
            }
            return _responder.Build<WeatherForecast>(this);
        }
    }
}
