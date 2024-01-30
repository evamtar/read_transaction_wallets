// See https://aka.ms/new-console-template for more information
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using SyncronizationBot.Application.Commands.Birdeye;
using SyncronizationBot.Application.Commands.MainCommands.AddUpdate;
using SyncronizationBot.Application.Commands.MainCommands.Read;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.Commands.SolanaFM;
using SyncronizationBot.Application.Handlers.Birdeye;
using SyncronizationBot.Application.Handlers.MainCommands.AddUpdate;
using SyncronizationBot.Application.Handlers.MainCommands.Read;
using SyncronizationBot.Application.Handlers.MainCommands.RecoverySave;
using SyncronizationBot.Application.Handlers.MainCommands.Send;
using SyncronizationBot.Application.Handlers.SolanaFM;
using SyncronizationBot.Application.Response.Birdeye;
using SyncronizationBot.Application.Response.MainCommands.AddUpdate;
using SyncronizationBot.Application.Response.MainCommands.Read;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Application.Response.MainCommands.Send;
using SyncronizationBot.Application.Response.SolanaFM;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Birdeye;
using SyncronizationBot.Domain.Service.CrossCutting.Dexscreener;
using SyncronizationBot.Domain.Service.CrossCutting.Jupiter;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;
using SyncronizationBot.Domain.Service.CrossCutting.Telegram;
using SyncronizationBot.Infra.CrossCutting.Birdeye.TokenCreation.Configs;
using SyncronizationBot.Infra.CrossCutting.Birdeye.TokenCreation.Service;
using SyncronizationBot.Infra.CrossCutting.Birdeye.TokenOverview.Configs;
using SyncronizationBot.Infra.CrossCutting.Birdeye.TokenOverview.Service;
using SyncronizationBot.Infra.CrossCutting.Birdeye.TokenSecurity.Configs;
using SyncronizationBot.Infra.CrossCutting.Birdeye.TokenSecurity.Service;
using SyncronizationBot.Infra.CrossCutting.Birdeye.WalletPortifolio.Service;
using SyncronizationBot.Infra.CrossCutting.Birdeye.WalletPortofolio.Configs;
using SyncronizationBot.Infra.CrossCutting.Dexscreener.Token.Configs;
using SyncronizationBot.Infra.CrossCutting.Dexscreener.Token.Service;
using SyncronizationBot.Infra.CrossCutting.Jupiter.Prices.Configs;
using SyncronizationBot.Infra.CrossCutting.Jupiter.Prices.Service;
using SyncronizationBot.Infra.CrossCutting.Solanafm.AccountInfo.Configs;
using SyncronizationBot.Infra.CrossCutting.Solanafm.AccountInfo.Service;
using SyncronizationBot.Infra.CrossCutting.Solanafm.Tokens.Configs;
using SyncronizationBot.Infra.CrossCutting.Solanafm.Tokens.Service;
using SyncronizationBot.Infra.CrossCutting.Solanafm.Transactions.Configs;
using SyncronizationBot.Infra.CrossCutting.Solanafm.Transactions.Service;
using SyncronizationBot.Infra.CrossCutting.Solanafm.Transfers.Configs;
using SyncronizationBot.Infra.CrossCutting.Solanafm.Transfers.Service;
using SyncronizationBot.Infra.CrossCutting.Telegram.TelegramBot.Configs;
using SyncronizationBot.Infra.CrossCutting.Telegram.TelegramBot.Service;
using SyncronizationBot.Infra.Data.Context;
using SyncronizationBot.Infra.Data.Repository;
using SyncronizationBot.Service;
using System.Reflection;


var builder = Host.CreateApplicationBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

using IHost host = builder.Build();

host.Run();

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    #region MediatR


    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

    #endregion

    #region Configs

    services.Configure<SyncronizationBotConfig>(configuration.GetSection("SyncronizationBot"));
    services.Configure<MappedTokensConfig> (configuration.GetSection("MappedTokens"));

    #endregion

    #region Context

    services.AddDbContext<SqlContext>(options => options.UseSqlServer(configuration.GetConnectionString("Monitoring")), ServiceLifetime.Transient);

    #endregion

    #region Hosted Service

    services.AddHostedService<ReadTransactionWalletsService>();
    //services.AddHostedService<AlertPriceService>();
    services.AddHostedService<LoadBalanceWalletsService>();

    #endregion

    #region Handlers

    #region Birdeye

    services.AddTransient<IRequestHandler<RecoverySaveBalanceBirdeyeCommand, RecoverySaveBalanceBirdeyeCommandResponse>, RecoverySaveBalanceBirdeyeCommandHandler>();

    #endregion

    #region SolanaFM

    services.AddTransient<IRequestHandler<RecoverySaveBalanceSFMCommand, RecoverySaveBalanceSFMCommandResponse>, RecoverySaveBalanceSFMCommandHandler>();

    services.AddTransient<IRequestHandler<RecoverySaveTransactionsCommand, RecoverySaveTransactionsCommandResponse>, RecoverySaveTransactionsCommandHandler>();
    services.AddTransient<IRequestHandler<RecoveryTransactionsCommand, RecoveryTransactionsCommandResponse>, RecoveryTransactionsCommandHandler>();
    services.AddTransient<IRequestHandler<RecoveryTransactionsSignatureForAddressCommand, RecoveryTransactionsSignatureForAddressCommandResponse>, RecoveryTransactionsSignatureForAddressCommandHandler>();
    
    #endregion

    #region Telegram

    services.AddTransient<IRequestHandler<SendTelegramMessageCommand, SendTelegramMessageCommandResponse>, SendTelegramMessageCommandHandler>();
    services.AddTransient<IRequestHandler<RecoverySaveTelegramChannel, RecoverySaveTelegramChannelResponse>, RecoverySaveTelegramChannelHandler>();

    #endregion

    #region Globais

    services.AddTransient<IRequestHandler<ReadWalletsCommand, ReadWalletsCommandResponse>, ReadWalletsCommandHandler>();
    services.AddTransient<IRequestHandler<ReadWalletsBalanceCommand, ReadWalletsBalanceCommandResponse>, ReadWalletsBalanceCommandHandler>();
    services.AddTransient<IRequestHandler<RecoverySaveTokenCommand, RecoverySaveTokenCommandResponse>, RecoverySaveTokenCommandHandler>();
    services.AddTransient<IRequestHandler<SendAlertPriceCommand, SendAlertPriceCommandResponse>, SendAlertPriceCommandHandler>();
    services.AddTransient<IRequestHandler<RecoveryPriceCommand, RecoveryPriceCommandResponse>, RecoveryPriceCommandHandler>();
    services.AddTransient<IRequestHandler<UpdateWalletsBalanceCommand, UpdateWalletsBalanceCommandResponse>, UpdateWalletsBalanceCommandHandler>();
    services.AddTransient<IRequestHandler<RecoveryAddUpdateBalanceItemCommand, RecoveryAddUpdateBalanceItemCommandResponse>, RecoveryAddUpdateBalanceItemCommandHandler>();
    services.AddTransient<IRequestHandler<SendAlertMessageCommand, SendAlertMessageCommandResponse>, SendAlertMessageCommandHandler>();
    
    #endregion

    #endregion

    #region Repositories

    services.AddTransient<IRunTimeControllerRepository, RunTimeControllerRepository>();
    services.AddTransient<IClassWalletRepository, ClassWalletRepository>();
    services.AddTransient<IWalletRepository, WalletRepository>();
    services.AddTransient<ITokenRepository, TokenRepository>();
    services.AddTransient<ITokenSecurityRepository, TokenSecurityRepository>();
    services.AddTransient<ITransactionsRepository, TransactionsRepository>();
    services.AddTransient<ITransactionNotMappedRepository, TransactionNotMappedRepository>();
    services.AddTransient<ITransactionsOldForMappingRepository, TransactionsOldForMappingRepository>();
    services.AddTransient<IWalletBalanceRepository, WalletBalanceRepository>();
    services.AddTransient<WalletBalanceSFMCompareRepository, WalletBalanceSFMCompareRepository>();
    services.AddTransient<IWalletBalanceHistoryRepository, WalletBalanceHistoryRepository>();
    services.AddTransient<ITelegramChannelRepository, TelegramChannelRepository>();
    services.AddTransient<IAlertPriceRepository, AlertPriceRepository>();
    services.AddTransient<IAlertConfigurationRepository, AlertConfigurationRepository>();
    services.AddTransient<IAlertInformationRepository, AlertInformationRepository>();
    services.AddTransient<IAlertParameterRepository, AlertParameterRepository>();

    #endregion

    #region External Services

    #region Dexscreener

    services.Configure<DexScreenerTokenConfig>(configuration.GetSection("DexScreenerToken"));
    services.AddHttpClient<IDexScreenerTokenService, DexScreenerTokenService>().AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
    #endregion

    #region Birdeye

    services.Configure<TokenOverviewConfig>(configuration.GetSection("TokenOverview"));
    services.AddHttpClient<ITokenOverviewService, TokenOverviewService>().AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

    services.Configure<TokenSecurityConfig>(configuration.GetSection("TokenSecurity"));
    services.AddHttpClient<ITokenSecurityService, TokenSecurityService>().AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

    services.Configure<TokenCreationConfig>(configuration.GetSection("TokenCreation"));
    services.AddHttpClient<ITokenCreationService, TokenCreationService>().AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

    services.Configure<WalletPortifolioConfig>(configuration.GetSection("WalletPortifolio"));
    services.AddHttpClient<IWalletPortifolioService, WalletPortifolioService>().AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

    #endregion

    #region SolanaFM

    #region Contingency

    services.Configure<TransactionsSignatureForAddressConfig>(configuration.GetSection("TransactionsSignatureForAddress"));
    services.AddHttpClient<ITransactionsSignatureForAddressService, TransactionsSignatureForAddressService>().AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

    #endregion

    services.Configure<TransactionsConfig>(configuration.GetSection("Transactions"));
    services.AddHttpClient<ITransactionsService, TransactionsService>().AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

    services.Configure<TransfersConfig>(configuration.GetSection("Transfers"));
    services.AddHttpClient<ITransfersService, TransfersService>().AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

    services.Configure<TokensConfig>(configuration.GetSection("Tokens"));
    services.AddHttpClient<ITokensService, TokensService>().AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
    
    services.Configure<TokensAccountsByOwnerConfig>(configuration.GetSection("TokensAccountByOwner"));
    services.AddHttpClient<ITokensAccountsByOwnerService, TokensAccountsByOwnerService>().AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

    services.Configure<AccountInfoConfig>(configuration.GetSection("AccountInfo"));
    services.AddHttpClient<IAccountInfoService, AccountInfoService>().AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
    #endregion

    #region Jupiter

    services.Configure<JupiterPriceConfig>(configuration.GetSection("JupiterPrice"));
    services.AddHttpClient<IJupiterPriceService, JupiterPriceService>().AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

    #endregion

    #region Telegram

    services.Configure<TelegramBotConfig>(configuration.GetSection("TelegramBot"));
    services.AddHttpClient<ITelegramBotService, TelegramBotService>().AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

    #endregion

    #endregion

}
