﻿using MediatR;
using SyncronizationBot.Application.Commands.Base;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;

namespace SyncronizationBot.Application.Commands.MainCommands.RecoverySave
{
    public class RecoverySaveTransactionsCommand : SearchCommand, IRequest<RecoverySaveTransactionsCommandResponse>
    {
        public Guid? WalletId { get; set; }
        public string? WalletHash { get; set; }
        public int? IdClassification { get; set; }
        public bool? IsContingecyTransactions { get; set; }
    }
}
