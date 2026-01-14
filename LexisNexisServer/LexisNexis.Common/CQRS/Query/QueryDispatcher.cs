using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.Common.CQRS.Query
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _provider;

        public QueryDispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query)
        {
            Type queryType = query.GetType();

            Type handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResult));

            object handler = _provider.GetRequiredService(handlerType);

            MethodInfo method = handlerType.GetMethod("HandleAsync") ?? throw new InvalidOperationException($"HandleAsync not found on {handlerType.Name}");

            var task = (Task<TResult>)method.Invoke(handler, new object[] { query })!;

            return await task;
        }
    }
}
