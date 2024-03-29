﻿using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Application.Commands.MainCommands.AddUpdate;
using SyncronizationBot.Application.Commands.MainCommands.Delete;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.Commands.MainCommands.Triggers;
using SyncronizationBot.Application.Response.MainCommands.AddUpdate;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Application.Response.MainCommands.Send;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transfers.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Model.Utils.Helpers;
using SyncronizationBot.Domain.Model.Utils.Transfer;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;
using SyncronizationBot.Service.Base;
using System.Data.Common;
using System.Diagnostics;
using System.Transactions;


namespace SyncronizationBot.Service
{
    public class TestService : BaseService
    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IWalletBalanceHistoryRepository _walletBalanceHistory;
        private readonly ITransfersService _transfersService;
        private readonly ITokenRepository _tokenRepository;
        private readonly ITokenAlphaRepository _tokenAlphaRepository;
        private readonly ITokenAlphaConfigurationRepository _tokenAlphaConfigurationRepository;
        private readonly ITokenAlphaWalletRepository _tokenAlphaWalletRepository;
        private readonly IPublishMessageRepository _publishMessageRepository;
        private readonly IOptions<MappedTokensConfig> _mappedTokensConfig;

        public TestService(IMediator mediator,
                                         IRunTimeControllerRepository runTimeControllerRepository,
                                         IOptions<SyncronizationBotConfig> syncronizationBotConfig,
                                         ITransfersService transfersService,
                                         ITransactionsRepository transactionsRepository,
                                         IWalletBalanceHistoryRepository walletBalanceHistory,
                                         ITokenAlphaRepository tokenAlphaRepository,
                                         ITokenAlphaConfigurationRepository tokenAlphaConfigurationRepository,
                                         ITokenAlphaWalletRepository tokenAlphaWalletRepository,
                                         IPublishMessageRepository publishMessageRepositor,
                                         IOptions<MappedTokensConfig> mappedTokensConfig) : base(mediator, runTimeControllerRepository, ETypeService.NONE, syncronizationBotConfig)
        {
            this._transfersService = transfersService;
            this._transactionsRepository = transactionsRepository;
            this._walletBalanceHistory = walletBalanceHistory;
            this._mappedTokensConfig = mappedTokensConfig;
            this._tokenAlphaRepository = tokenAlphaRepository;
            this._tokenAlphaConfigurationRepository = tokenAlphaConfigurationRepository;
            this._tokenAlphaWalletRepository = tokenAlphaWalletRepository;
            this._publishMessageRepository = publishMessageRepositor;
            base.LogMessage("Iniciando o serviço de teste de alertas");
        }

        protected override async Task DoExecute(PeriodicTimer timer, CancellationToken stoppingToken)
        {
            await this._mediator.Send(new RecoverySaveNewsTokensCommand { });
            //await this.TestePublicMessage();
            //await this.TesteAdicionarAlpha();
            //await this.RepublishTokenAlpha();
            //await this.Handle();
        }

    }
}
