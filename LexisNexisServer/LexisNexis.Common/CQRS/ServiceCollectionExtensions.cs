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
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection 
            
            
            AddCqrs(this IServiceCollection services, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!type.IsClass || type.IsAbstract)
                    {
                        continue;
                    }

                    foreach (var iface in type.GetInterfaces())
                    {
                        if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))
                        {
                            services.AddScoped(iface, type);
                        }

                        if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
                        {
                            services.AddScoped(iface, type);
                        }
                    }
                }
            }

            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();
            services.AddScoped<EventPlayerService>();

            return services;
        }
    }
}
