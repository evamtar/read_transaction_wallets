﻿// See https://aka.ms/new-console-template for more information
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReadTransactionsWallets.Application.Commands;
using ReadTransactionsWallets.Application.Handlers;
using ReadTransactionsWallets.Application.Response;
using ReadTransactionsWallets.Domain.Model.Configs;
using ReadTransactionsWallets.Domain.Repository;
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

    #endregion

    #region Context

    services.AddDbContext<SqlContext>(options => options.UseSqlServer(configuration.GetConnectionString("Monitoring")));

    #endregion

    #region Hosted Service

    services.AddHostedService<ReadTransactionWalletsService>();

    #endregion

    #region Handlers

    services.AddScoped<IRequestHandler<ReadWalletsCommand, ReadWalletsCommandResponse>, ReadWalletsCommandHandler>();

    #endregion

    #region Repositories

    services.AddScoped<IRunTimeControllerRepository, RunTimeControllerRepository>();
    services.AddScoped<IClassWalletRepository, ClassWalletRepository>();
    services.AddScoped<IWalletRepository, WalletRepository>();
    services.AddScoped<ITokenRepository, TokenRepository>();
    services.AddScoped<ITransactionsRepository, TransactionsRepository>();


    #endregion

}
