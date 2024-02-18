using Microsoft.Extensions.DependencyInjection;
using SyncronizationBot.Domain.Repository.MongoDB.Base;
using SyncronizationBot.Domain.Repository.SQLServer.Base;
using SyncronizationBot.Infra.Data.MongoDB.Repository.Base;
using SyncronizationBot.Infra.Data.SQLServer.Repository.Base;
using SyncronizationBotApp.Extensions.Enum;
using System.Reflection;

namespace SyncronizationBotApp.Extensions
{
    public static class RepositoryExtension
    {
        public static void AddRepositories(this IServiceCollection services, Assembly? assembly, ETypeService typeService) 
        {
            //Repositories (NOT CACHED)
            var repositoryTypes = assembly?.GetTypes()
                .Where(type => !type.IsAbstract && !type.IsInterface && type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IRepository<>)));
            var nonBaseRepos = repositoryTypes?.Where(t => t != typeof(Repository<>));
            if (nonBaseRepos?.Count() > 0) 
            {
                foreach (var repositoryType in nonBaseRepos)
                {
                    var interfaces = repositoryType.GetInterfaces()
                        .Where(@interface => !@interface.IsGenericType)
                        .ToList();
                    
                    if (interfaces.Count != 1)
                        throw new InvalidOperationException($"Repository '{repositoryType.Name}' must implement only one interface that implements IRepositoryBase<T>.");
                    switch (typeService)
                    {
                        case ETypeService.Scoped:
                            services.AddScoped(interfaces[0], repositoryType);
                            break;
                        case ETypeService.Transient:
                            services.AddTransient(interfaces[0], repositoryType);
                            break;
                        case ETypeService.Singleton:
                            services.AddSingleton(interfaces[0], repositoryType);
                            break;
                        default:
                            break;
                    }
                }
            }
            //Repositories (CACHED)
            repositoryTypes = assembly?.GetTypes()
                .Where(type => !type.IsAbstract && !type.IsInterface && type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICachedRepository<>)));
            nonBaseRepos = repositoryTypes?.Where(t => t != typeof(CachedRepository<>));
            if (nonBaseRepos?.Count() > 0)
            {
                foreach (var repositoryType in nonBaseRepos)
                {
                    var interfaces = repositoryType.GetInterfaces()
                        .Where(@interface => !@interface.IsGenericType)
                        .ToList();

                    if (interfaces.Count != 1)
                        throw new InvalidOperationException($"Repository '{repositoryType.Name}' must implement only one interface that implements IRepositoryBase<T>.");
                    switch (typeService)
                    {
                        case ETypeService.Scoped:
                            services.AddScoped(interfaces[0], repositoryType);
                            break;
                        case ETypeService.Transient:
                            services.AddTransient(interfaces[0], repositoryType);
                            break;
                        case ETypeService.Singleton:
                            services.AddSingleton(interfaces[0], repositoryType);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
