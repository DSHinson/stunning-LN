using LexisNexis.BLL.Weather;
using LexisNexis.Common.CQRS;
using Microsoft.AspNetCore.Mvc;

namespace LexisNexis.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly EventPlayerService _eventPlayerService;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, EventPlayerService eventPlayerService)
        {
            _logger = logger;
            _eventPlayerService = eventPlayerService ?? throw new ArgumentNullException(nameof(eventPlayerService));
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            IEnumerable<WeatherForecast> result = await _eventPlayerService.EmitAsync(new GetWeatherForecastEvent());

            return result;
        }
    }
}
