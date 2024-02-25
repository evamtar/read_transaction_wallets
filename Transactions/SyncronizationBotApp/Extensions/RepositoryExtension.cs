using Microsoft.Extensions.DependencyInjection;
using SyncronizationBot.Infra.Data.MongoDB.Repository;
using SyncronizationBot.Infra.Data.SQLServer.Repository;
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
                .Where(type => !type.IsAbstract && !type.IsInterface && type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(SyncronizationBot.Domain.Repository.Base.Interfaces.IRepository<>)));
            var nonBaseRepos = repositoryTypes?.Where(t => t != typeof(SqlServerRepository<>));
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
                .Where(type => !type.IsAbstract && !type.IsInterface && type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(SyncronizationBot.Domain.Repository.Base.Interfaces.IRepository<>)));
            nonBaseRepos = repositoryTypes?.Where(t => t != typeof(MongoRepository<>));
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
