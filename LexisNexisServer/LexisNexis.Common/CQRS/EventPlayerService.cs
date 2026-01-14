using LexisNexis.Common.CQRS.Command;
using LexisNexis.Common.CQRS.Query;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.Common.CQRS
{
    public class EventPlayerService
    {
        public sealed record ReplayEntry
        (
            Type CommandType,
            Type ResultType,
            IEvent Command
        );

        private readonly IServiceProvider _serviceProvider;
        private readonly List<ReplayEntry> _eventLog = new();

        public EventPlayerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task<TResult> EmitAsync<TResult>(IQuery<TResult> query)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var queryDispatcher = scope.ServiceProvider.GetRequiredService<IQueryDispatcher>();
                return await queryDispatcher.DispatchAsync(query);
            }
        }

        public async Task<TResult> EmitAsync<TResult>(ICommand<TResult> command)
        {
            _eventLog.Add(new ReplayEntry(CommandType: command.GetType(), ResultType: typeof(TResult), Command: command));

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var commandDispatcher = scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();
                return await commandDispatcher.DispatchAsync(command);
            }
        }

        public async Task ReplayAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            // Resolve the dispatcher from DI
            ICommandDispatcher dispatcher = serviceProvider.GetRequiredService<ICommandDispatcher>();

            foreach (ReplayEntry entry in _eventLog)
            {
                EventReplayBehaviorAttribute? attr = entry.Command.GetType().GetCustomAttributes(typeof(EventReplayBehaviorAttribute), false).FirstOrDefault() as EventReplayBehaviorAttribute;

                if (attr == null || !attr.Options.HasFlag(EventReplayOptions.Replayable))
                {
                    continue;
                }

                MethodInfo method = typeof(ICommandDispatcher).GetMethod(nameof(ICommandDispatcher.DispatchAsync))!.MakeGenericMethod(entry.ResultType);

                var task = (Task)method.Invoke(dispatcher, new object[] { entry.Command })!;
                await task.ConfigureAwait(false);

            }
        }
    }
}
