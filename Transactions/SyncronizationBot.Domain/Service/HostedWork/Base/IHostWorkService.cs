﻿using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;

namespace SyncronizationBot.Domain.Service.HostedWork.Base
{
    public interface IHostWorkService : IDisposable
    {
        IOptions<SyncronizationBotConfig>? Options { get; }

        ETypeService? TypeService { get; }
        Task DoExecute(CancellationToken cancellationToken);
    }
}
