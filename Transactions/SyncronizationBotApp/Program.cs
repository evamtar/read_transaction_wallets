// See https://aka.ms/new-console-template for more information
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Handlers;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting;
using SyncronizationBot.Infra.CrossCutting.Jupiter.Prices.Config;
using SyncronizationBot.Infra.CrossCutting.Jupiter.Prices.Service;
using SyncronizationBot.Infra.CrossCutting.Solanafm.AccountInfo.Configs;
using SyncronizationBot.Infra.CrossCutting.Solanafm.AccountInfo.Service;
using SyncronizationBot.Infra.CrossCutting.Solanafm.Accounts.Configs;
using SyncronizationBot.Infra.CrossCutting.Solanafm.Accounts.Service;
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

    services.AddDbContext<SqlContext>(options => options.UseSqlServer(configuration.GetConnectionString("Monitoring")));

    #endregion

    #region Hosted Service

    services.AddHostedService<ReadTransactionWalletsService>();
    //services.AddHostedService<LoadBalanceWalletsService>();

    #endregion

    #region Handlers

    services.AddScoped<IRequestHandler<ReadWalletsCommand, ReadWalletsCommandResponse>, ReadWalletsCommandHandler>();
    services.AddScoped<IRequestHandler<RecoverySaveTransactionsCommand, RecoverySaveTransactionsCommandResponse>, RecoverySaveTransactionsCommandHandler>();
    services.AddScoped<IRequestHandler<RecoverySaveTokenCommand, RecoverySaveTokenCommandResponse>, RecoverySaveTokenCommandHandler>();
    services.AddScoped<IRequestHandler<SendTelegramMessageCommand, SendTelegramMessageCommandResponse>, SendTelegramMessageCommandHandler>();
    services.AddScoped<IRequestHandler<ReadWalletsBalanceCommand, ReadWalletsBalanceCommandResponse>, ReadWalletsBalanceCommandHandler>();
    services.AddScoped<IRequestHandler<RecoverySaveTelegramChannel, RecoverySaveTelegramChannelResponse>, RecoverySaveTelegramChannelHandler>();

    #endregion

    #region Repositories

    services.AddScoped<IRunTimeControllerRepository, RunTimeControllerRepository>();
    services.AddScoped<IClassWalletRepository, ClassWalletRepository>();
    services.AddScoped<IWalletRepository, WalletRepository>();
    services.AddScoped<ITokenRepository, TokenRepository>();
    services.AddScoped<ITransactionsRepository, TransactionsRepository>();
    services.AddScoped<IWalletBalanceRepository, WalletBalanceRepository>();
    services.AddScoped<ITelegramChannelRepository, TelegramChannelRepository>();
    services.AddScoped<ITransactionNotMappedRepository, TransactionNotMappedRepository>();
    services.AddScoped<IAlertPriceRepository, AlertPriceRepository>();
    
    #endregion

    #region External Services

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

    services.Configure<TelegramBotConfig>(configuration.GetSection("TelegramBot"));
    services.AddHttpClient<ITelegramBotService, TelegramBotService>().AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

    services.Configure<AccountsConfig>(configuration.GetSection("Accounts"));
    services.AddHttpClient<IAccountsService, AccountsService>().AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
    
    services.Configure<AccountInfoConfig>(configuration.GetSection("AccountInfo"));
    services.AddHttpClient<IAccountInfoService, AccountInfoService>().AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

    services.Configure<JupiterPriceConfig>(configuration.GetSection("JupiterPrice"));
    services.AddHttpClient<IJupiterPriceService, JupiterPriceService>().AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))); 
    #endregion

}
