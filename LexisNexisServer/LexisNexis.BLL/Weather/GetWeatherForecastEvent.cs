using LexisNexis.API;
using LexisNexis.Common.CQRS;
using LexisNexis.Common.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.BLL.Weather
{
    [EventReplayBehaviorAttribute(EventReplayOptions.Replayable)]
    public class GetWeatherForecastEvent : IQuery<IEnumerable<WeatherForecast>>
    {
    }
}
