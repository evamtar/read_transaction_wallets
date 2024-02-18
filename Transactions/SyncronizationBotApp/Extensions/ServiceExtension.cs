using Microsoft.Extensions.DependencyInjection;
using SyncronizationBot.Domain.Service.RecoveryService.Base;
using SyncronizationBot.Service.InternalServices.Base;
using SyncronizationBotApp.Extensions.Enum;
using System.Reflection;

namespace SyncronizationBotApp.Extensions
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services, Assembly? assembly, ETypeService typeService)
        {
            var servicesTypes = assembly?.GetTypes()
                .Where(type => !type.IsAbstract && !type.IsInterface && type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IServiceBase<>)));
            var nonBaseRepos = servicesTypes?.Where(t => t != typeof(ServiceBase<>));
            if (nonBaseRepos?.Count() > 0)
            {
                foreach (var serviceType in nonBaseRepos)
                {
                    var interfaces = serviceType.GetInterfaces()
                        .Where(@interface => !@interface.IsGenericType && !@interface.Name.Contains("IDisposable"))
                        .ToList();

                    if (interfaces.Count != 1)
                        throw new InvalidOperationException($"Service '{serviceType.Name}' must implement only one interface that implements IServiceBase<T>.");
                    switch (typeService)
                    {
                        case ETypeService.Scoped:
                            services.AddScoped(interfaces[0], serviceType);
                            break;
                        case ETypeService.Transient:
                            services.AddTransient(interfaces[0], serviceType);
                            break;
                        case ETypeService.Singleton:
                            services.AddSingleton(interfaces[0], serviceType);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        public static void AddWorkers(this IServiceCollection services, Assembly? assembly, ETypeService typeService) 
        {
            var workersTypes = assembly?.GetTypes()
                .Where(type => !type.IsAbstract && !type.IsInterface && type.GetInterfaces().Any(x => x.IsInterface && x.GetInterface("IHostWorkService") != null));
            if (workersTypes?.Count() > 0)
            {
                foreach (var workType in workersTypes)
                {
                    var interfaces = workType.GetInterfaces()
                        .Where(@interface => !@interface.IsGenericType && !@interface.Name.Contains("IHostWorkService"))
                        .ToList();

                    if (interfaces.Count != 1)
                        throw new InvalidOperationException($"Worker '{workType.Name}' must implement only one interface that implements IHostWorkService.");
                    switch (typeService)
                    {
                        case ETypeService.Scoped:
                            services.AddScoped(interfaces[0], workType);
                            break;
                        case ETypeService.Transient:
                            services.AddTransient(interfaces[0], workType);
                            break;
                        case ETypeService.Singleton:
                            services.AddSingleton(interfaces[0], workType);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
