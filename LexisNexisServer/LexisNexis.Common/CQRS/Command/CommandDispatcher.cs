using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace LexisNexis.Common.CQRS.Command
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _provider;

        public CommandDispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }

        ///<inheritdoc cref="ICommandDispatcher.DispatchAsync{TCommand}(TCommand)"/>
        public async Task<TResult> DispatchAsync<TResult>(ICommand<TResult> query)
        {
            Type queryType = query.GetType();

            Type handlerType = typeof(ICommandHandler<,>).MakeGenericType(queryType, typeof(TResult));

            object handler = _provider.GetRequiredService(handlerType);

            MethodInfo method = handlerType.GetMethod("HandleAsync") ?? throw new InvalidOperationException($"HandleAsync not found on {handlerType.Name}");

            Task<TResult> task = (Task<TResult>)method.Invoke(handler, new object[] { query })!;
            return await task;
        }


    }
}
