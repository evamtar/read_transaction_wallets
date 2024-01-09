// See https://aka.ms/new-console-template for more information
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using ReadTransactionsWallets.Application.Commands;
using ReadTransactionsWallets.Application.Handlers;
using ReadTransactionsWallets.Application.Response;
using ReadTransactionsWallets.Domain.Model.Configs;
using ReadTransactionsWallets.Domain.Repository;
using ReadTransactionsWallets.Domain.Service.CrossCutting;
using ReadTransactionsWallets.Infra.CrossCutting.Solanafm.AccountInfo.Configs;
using ReadTransactionsWallets.Infra.CrossCutting.Solanafm.AccountInfo.Service;
using ReadTransactionsWallets.Infra.CrossCutting.Solanafm.Accounts.Configs;
using ReadTransactionsWallets.Infra.CrossCutting.Solanafm.Accounts.Service;
using ReadTransactionsWallets.Infra.CrossCutting.Solanafm.Tokens.Configs;
using ReadTransactionsWallets.Infra.CrossCutting.Solanafm.Tokens.Service;
using ReadTransactionsWallets.Infra.CrossCutting.Solanafm.Transactions.Configs;
using ReadTransactionsWallets.Infra.CrossCutting.Solanafm.Transactions.Service;
using ReadTransactionsWallets.Infra.CrossCutting.Solanafm.Transfers.Configs;
using ReadTransactionsWallets.Infra.CrossCutting.Solanafm.Transfers.Service;
using ReadTransactionsWallets.Infra.CrossCutting.Telegram.TelegramBot.Configs;
using ReadTransactionsWallets.Infra.CrossCutting.Telegram.TelegramBot.Service;
using ReadTransactionsWallets.Infra.Data.Context;
using ReadTransactionsWallets.Infra.Data.Repository;
using ReadTransactionsWallets.Service;
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

    services.Configure<ReadTransactionWalletsConfig>(configuration.GetSection("ReadTransactionWallets"));
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
    #endregion

    #region Repositories

    services.AddScoped<IRunTimeControllerRepository, RunTimeControllerRepository>();
    services.AddScoped<IClassWalletRepository, ClassWalletRepository>();
    services.AddScoped<IWalletRepository, WalletRepository>();
    services.AddScoped<ITokenRepository, TokenRepository>();
    services.AddScoped<ITransactionsRepository, TransactionsRepository>();
    services.AddScoped<IWalletBalanceRepository, WalletBalanceRepository>();

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
    #endregion

}
