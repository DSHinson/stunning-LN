using LexisNexis.API;
using LexisNexis.Common.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.BLL.Weather
{
    public class GetWeatherForecastEventHandler : IQueryHandler<GetWeatherForecastEvent, IEnumerable<WeatherForecast>>
    {
        private static readonly string[] Summaries =
       [
           "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
       ];
        public async Task<IEnumerable<WeatherForecast>> HandleAsync(GetWeatherForecastEvent command)
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();
        }
    }
}
