using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;

namespace SyncronizationBot.Domain.Service.InternalService.HostedWork.Base
{
    public interface IHostWorkService
    {
        IOptions<SyncronizationBotConfig>? Options { get; }

        ETypeService? TypeService { get; }
        Task DoExecute(CancellationToken cancellationToken);
    }
}
