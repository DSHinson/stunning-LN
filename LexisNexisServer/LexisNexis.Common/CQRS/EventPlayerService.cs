using LexisNexis.Common.CQRS.Command;
using LexisNexis.Common.CQRS.Query;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LexisNexis.Common.CQRS
{
    public class EventPlayerService
    {
        public sealed record ReplayEntry
        (
            Type CommandType,
            Type ResultType,
            IEvent Command,
            DateTime Timestamp
        );

        private readonly IServiceProvider _serviceProvider;
        private readonly List<ReplayEntry> _eventLog = new();
        private readonly ILogger<EventPlayerService> _logger;

        public EventPlayerService(IServiceProvider serviceProvider, ILogger<EventPlayerService> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TResult> EmitAsync<TResult>(IQuery<TResult> query)
        {
            _logger.LogInformation("Emitting query {QueryType} at {Timestamp}", query.GetType().Name, DateTime.UtcNow);

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var queryDispatcher = scope.ServiceProvider.GetRequiredService<IQueryDispatcher>();
                TResult result = await queryDispatcher.DispatchAsync(query);

                _logger.LogInformation("Query {QueryType} completed at {Timestamp}", query.GetType().Name, DateTime.UtcNow);

                return result;
            }
        }

        public async Task<TResult> EmitAsync<TResult>(ICommand<TResult> command)
        {
            var timestamp = DateTime.UtcNow;
            _eventLog.Add(new ReplayEntry(command.GetType(), typeof(TResult), command, timestamp));

            _logger.LogInformation("Emitting command {CommandType} at {Timestamp}", command.GetType().Name, timestamp);

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var commandDispatcher = scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();
                TResult result = await commandDispatcher.DispatchAsync(command);

                _logger.LogInformation("Command {CommandType} completed at {Timestamp}", command.GetType().Name, DateTime.UtcNow);

                return result;
            }
        }

        public async Task ReplayAsync()
        {
            _logger.LogInformation("Starting replay of {Count} events at {Timestamp}", _eventLog.Count, DateTime.UtcNow);

            using var scope = _serviceProvider.CreateScope();
            var dispatcher = scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();

            foreach (ReplayEntry entry in _eventLog)
            {
                EventReplayBehaviorAttribute? attr = entry.Command.GetType()
                    .GetCustomAttributes(typeof(EventReplayBehaviorAttribute), false)
                    .FirstOrDefault() as EventReplayBehaviorAttribute;

                if (attr == null || !attr.Options.HasFlag(EventReplayOptions.Replayable))
                {
                    continue;
                }

                _logger.LogInformation("Replaying command {CommandType} originally emitted at {Timestamp}",
                    entry.CommandType.Name, entry.Timestamp);

                MethodInfo method = typeof(ICommandDispatcher).GetMethod(nameof(ICommandDispatcher.DispatchAsync))!
                    .MakeGenericMethod(entry.ResultType);

                var task = (Task)method.Invoke(dispatcher, new object[] { entry.Command })!;
                await task.ConfigureAwait(false);

                _logger.LogInformation("Finished replaying command {CommandType} at {Timestamp}",
                    entry.CommandType.Name, DateTime.UtcNow);
            }

            _logger.LogInformation("Replay finished at {Timestamp}", DateTime.UtcNow);
        }
    }
}
