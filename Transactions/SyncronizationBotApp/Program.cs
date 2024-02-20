﻿// See https://aka.ms/new-console-template for more information
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Service.CrossCutting.Birdeye;
using SyncronizationBot.Domain.Service.CrossCutting.Dexscreener;
using SyncronizationBot.Domain.Service.CrossCutting.Jupiter;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;
using SyncronizationBot.Domain.Service.CrossCutting.SolnetRpc.Balance;
using SyncronizationBot.Domain.Service.CrossCutting.SolnetRpc.Transactions;
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
using SyncronizationBot.Infra.CrossCutting.SolnetRpc.Balance.Service;
using SyncronizationBot.Infra.CrossCutting.SolnetRpc.Transactions.Service;
using SyncronizationBot.Infra.CrossCutting.Telegram.TelegramBot.Configs;
using SyncronizationBot.Infra.CrossCutting.Telegram.TelegramBot.Service;
using System.Reflection;
using SyncronizationBotApp.Extensions;
using SyncronizationBot.Service.HostedServices;
using SyncronizationBot.Application.AutoMapper.Profiles;
using SyncronizationBot.Infra.Data.MongoDB.Context;
using SyncronizationBot.Infra.Data.SQLServer.Context;
using MongoDB.Driver;
using SyncronizationBots.RabbitMQ.Extension;
using SyncronizationBot.Service.RabbitMQ.UpdatesQueue.Configs;
using SyncronizationBot.Domain.Service.RabbitMQ.UpdatesQueue;
using SyncronizationBot.Service.RabbitMQ.UpdatesQueue;


var builder = Host.CreateApplicationBuilder(args);

await ConfigureServices(builder.Services, builder.Configuration);

using IHost host = builder.Build();
host.Run();

static async Task ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    await CleanUpCacheDB(configuration);

    #region MediatR

    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

    #endregion

    #region Configs

    services.Configure<SyncronizationBotConfig>(configuration.GetSection("SyncronizationBot"));
    services.Configure<MappedTokensConfig>(configuration.GetSection("MappedTokens"));

    #endregion

    #region Context / Repositories / Handlers / Services / HostedService (NOT NOW THE EXTENSION)

    services.AddDbContext<SqlContext>(options => options.UseSqlServer(configuration.GetConnectionString("Monitoring")), ServiceLifetime.Transient);
    services.AddDbContext<MongoDbContext>(options => options.UseMongoDB(configuration.GetConnectionString("CacheMongoDB")?? string.Empty, configuration.GetSection("Mongo:Database").Value ?? string.Empty), ServiceLifetime.Scoped);
    services.AddRepositories(Assembly.Load("SyncronizationBot.Infra.Data"), SyncronizationBotApp.Extensions.Enum.ETypeService.Scoped);
    services.AddHandlers(Assembly.Load("SyncronizationBot.Application"), SyncronizationBotApp.Extensions.Enum.ETypeService.Scoped);
    services.AddServices(Assembly.Load("SyncronizationBot.Service"), SyncronizationBotApp.Extensions.Enum.ETypeService.Scoped);
    services.AddWorkers(Assembly.Load("SyncronizationBot.Service"), SyncronizationBotApp.Extensions.Enum.ETypeService.Scoped);
    services.AddAutoMapper(typeof(ServiceMediatorProfile));

    #endregion

    #region rabbitMQ
    
    services.AddRabitMqConnection(configuration);
    services.Configure<UpdateQueueConfiguration>(configuration.GetSection("UpdateQueue"));
    services.AddScoped<IPublishUpdateService, PublishUpdateService>();
    #endregion

    #region Hosted Service
    services.AddHostedService<BalanceWalletsHostedService>();

        //services.AddHostedService<ReadTransactionWalletsService>();
        //services.AddHostedService<AlertPriceService>();
        //services.AddHostedService<DeleteOldsMessagesLogService>();
        //services.AddHostedService<AlertTokenAlphaService>();
        //services.AddHostedService<ReadTransactionsOldForMapping>();
        //services.AddHostedService<LoadNewTokensForBetAwardsService>();

        #region Only For Test

        //services.AddHostedService<TestService>();

        #endregion

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

    #region Solnet

    services.AddTransient<ISolnetBalanceService, SolnetBalanceService>();
    services.AddTransient<ISolnetTransactionService, SolnetTransactionService>();

    #endregion

    #endregion

}

static async Task CleanUpCacheDB(IConfiguration configuration) 
{
    var client = new MongoClient(configuration.GetConnectionString("CacheMongoDB"));
    await client.DropDatabaseAsync(configuration.GetSection("Mongo:Database").Value);
}
