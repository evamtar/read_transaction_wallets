using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SyncronizationBotApp.Extensions.Enum;
using System.Reflection;

namespace SyncronizationBotApp.Extensions
{
    public static class HandlerExtension
    {
        public static void AddHandlers(this IServiceCollection services, Assembly? assembly, ETypeService typeService)
        {
            var handlerTypes = assembly?.GetTypes()
                .Where(type => !type.IsAbstract && !type.IsInterface && type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)));
            if (handlerTypes?.Count() > 0)
            {
                foreach (var handlerType in handlerTypes)
                {
                    var interfaces = handlerType.GetInterfaces()
                        .Where(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                        .ToList();
                    
                    if (interfaces.Count != 1)
                        throw new InvalidOperationException($"Handler '{handlerType.Name}' must implement only one interface that implements IRequestHandler<TCommand,TResponse> where TCommand: IRequest<TResponse>.");
                    switch (typeService)
                    {
                        case ETypeService.Scoped:
                            services.AddScoped(interfaces[0], handlerType);
                            break;
                        case ETypeService.Transient:
                            services.AddTransient(interfaces[0], handlerType);
                            break;
                        case ETypeService.Singleton:
                            services.AddSingleton(interfaces[0], handlerType);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
