// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReadTransactionsWallets.Domain.Model.Configs;
using ReadTransactionsWallets.Service;



var builder = Host.CreateApplicationBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

using IHost host = builder.Build();

host.Run();

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    #region Configs

    services.Configure<ReadTransactionWalletsConfig>(configuration.GetSection("ReadTransactionWallets"));

    #endregion

    #region Hosted Service

    services.AddHostedService<ReadTransactionWalletsService>();

    #endregion

    #region Handlers

    #endregion

    #region Repositories

    #endregion

}
