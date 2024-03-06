using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Extensions;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.RabbitMQ;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Domain.Repository.UnitOfWork;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.TokenInfoQueue;
using SyncronizationBot.Service.RabbitMQ.Consumers.Base;
using SyncronizationBot.Service.RabbitMQ.Queue.UpdateQueue.Configs;
using SyncronizationBot.Utils;
using SyncronizationBots.RabbitMQ.Exceptions;
using System.Reflection;


namespace SyncronizationBot.Service.RabbitMQ.Consumers
{
    public class UpdateQueueConsumerService : BaseBatchMessageConsumer
    {
        private IUnitOfWorkSqlServer UnitOfWorkSqlServer { get; set; } 

        public UpdateQueueConsumerService(IServiceProvider serviceProvider, 
                                          IOptions<UpdateQueueConfiguration> configuration) : base(serviceProvider, configuration.Value)
        {
            this.UnitOfWorkSqlServer = null!;
        }

        public override async Task HandlerAsync(IServiceScope scope, string? message, CancellationToken stoppingToken)
        {
            this.InitServices(scope);
            var eventEventName = JsonConvert.DeserializeObject<MessageEvent<Entity>>(message ?? string.Empty)?.EventName;
            var data = eventEventName?.Split("_");

            if (data?.Count() != 2)
                throw new ArgumentException("Problem in process message");
            var classType = data?[0];
            var instruction = data?[1];
            switch (instruction)
            {
                case Constants.INSTRUCTION_INSERT:
                    await this.DoProcessInsertAsync(scope, message, classType);
                    break;
                case Constants.INSTRUCTION_UPDATE:
                    this.DoProcessUpdate(scope, message, classType);
                    break;
                case Constants.INSTRUCTION_UPDATE_RANGE_WB:
                    this.DoProcessUpdateSpecial(scope, message, classType);
                    break;
                case Constants.INSTRUCTION_DELETE:
                default:
                    throw new ArgumentException($"Instruction {instruction} is not valid");
            }
        }

        private void InitServices(IServiceScope scope)
        {
            this.UnitOfWorkSqlServer = scope.ServiceProvider.GetRequiredService<IUnitOfWorkSqlServer>();
        }

        private async Task DoProcessInsertAsync(IServiceScope scope, string? message, string? classType)
        {
            try
            {
                switch (classType?.ToUpper())
                {
                    case Constants.TELEGRAM_MESSAGE_INSTRUCTION:
                        await this.DoInsertAsync(message, this.UnitOfWorkSqlServer.TelegramMessageRepository);
                        break;
                    case Constants.TOKEN_INSTRUCTION:
                        var tokenInserted = await this.DoInsertAsync(message, this.UnitOfWorkSqlServer.TokenRepository);
                        if (tokenInserted.IsLazyLoad ?? false)
                            await base.TransferQueue(tokenInserted, string.Empty, scope.ServiceProvider.GetRequiredService<IPublishTokenInfoService>());
                        break;
                    case Constants.TOKEN_PRICE_HISTORY_INSTRUCTION:
                        await this.DoInsertAsync(message, this.UnitOfWorkSqlServer.TokenPriceHistoryRepository);
                        break;
                    case Constants.TOKEN_SECURITY_INSTRUCTION:
                        await this.DoInsertAsync(message, this.UnitOfWorkSqlServer.TokenSecurityRepository);
                        break;
                    case Constants.WALLET_BALANCE_INSTRUCTION:
                        await this.DoInsertAsync(message, this.UnitOfWorkSqlServer.WalletBalanceRepository);
                        break;
                    case Constants.WALLET_BALANCE_HISTORY_INSTRUCTION:
                        await this.DoInsertAsync(message, this.UnitOfWorkSqlServer.WalletBalanceHistoryRepository);
                        break;

                    default:
                        throw new ArgumentException($"Instruction INSERT is not valid for classType {classType}");
                }
            }
            catch (Exception ex)
            {
                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                    throw new RelationShipInsertException(ex.Source, ex);
            }
        }

        private void DoProcessUpdate(IServiceScope scope, string? message, string? classType)
        {
            this.UnitOfWorkSqlServer = UnitOfWorkSqlServer ?? scope.ServiceProvider.GetRequiredService<IUnitOfWorkSqlServer>();
            switch (classType?.ToUpper())
            {
                case Constants.ALERT_PRICE_INSTRUCTION:
                    this.DoUpdate(message, this.UnitOfWorkSqlServer.AlertPriceRepository);
                    break;
                case Constants.RUN_TIME_CONTROLLER_INSTRUCTION:
                    this.DoUpdate(message, this.UnitOfWorkSqlServer.RunTimeControllerRepository);
                    break;
                case Constants.TELEGRAM_MESSAGE_INSTRUCTION:
                    this.DoUpdate(message, this.UnitOfWorkSqlServer.TelegramMessageRepository);
                    break;
                case Constants.TOKEN_INSTRUCTION:
                    this.DoUpdate(message, this.UnitOfWorkSqlServer.TokenRepository);
                    break;
                case Constants.WALLET_BALANCE_INSTRUCTION:
                    this.DoUpdate(message, this.UnitOfWorkSqlServer.WalletBalanceRepository);
                    break;
                case Constants.WALLET_INSTRUCTION:
                    this.DoUpdate(message, this.UnitOfWorkSqlServer.WalletRepository);
                    break;
                default:
                    throw new ArgumentException($"Instruction UPDATE is not valid for classType {classType}");
            }
        }

        private void DoProcessUpdateSpecial(IServiceScope scope, string? message, string? classType)
        {
            try
            {
                switch (classType?.ToUpper())
                {
                    case Constants.WALLET_BALANCE_INSTRUCTION:
                        this.DoInvokeMethod<TokenPriceHistory, WalletBalance>(message, this.UnitOfWorkSqlServer.WalletBalanceRepository, "UpdateAllBalancesWithToken");
                        break;
                    default:
                        throw new ArgumentException($"Instruction INSERT is not valid for classType {classType}");
                }
            }
            catch (Exception ex)
            {
                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                    throw new RelationShipInsertException(ex.Source, ex);
            }
        }

        private async Task<T> DoInsertAsync<T>(string? message, IRepository<T> repository) where T : Entity
        {
            var messageEvent = message?.ToMessageEvent<T>();
            var entity = await repository!.AddAsync(messageEvent!.Entity!);
            await UnitOfWorkSqlServer.SaveChangesAsync();
            return entity;
        }

        private T DoUpdate<T>(string? message, IRepository<T> repository) where T : Entity
        {
            var messageEvent = message?.ToMessageEvent<T>();
            var entity = repository.Update(messageEvent!.Entity!);
            this.UnitOfWorkSqlServer.SaveChanges();
            return entity;
        }
        private void DoInvokeMethod<TParameter, WTarget>(string? message, IRepository<WTarget> repository, string method) where TParameter : Entity
                                                                                                                          where WTarget: Entity
        {
            var messageEvent = message?.ToMessageEvent<TParameter>();
            _ = repository.GetType().GetMethod(method)?.Invoke(repository, [messageEvent!.Entity!]);
            this.UnitOfWorkSqlServer.SaveChanges();
        }

        public override void Dispose()
        {
            try
            {
                this.UnitOfWorkSqlServer.Dispose();
            }
            finally 
            {
                base.Dispose();
            }
        }
    }
}
