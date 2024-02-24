using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Extensions;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.RabbitMQ;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Domain.Repository.UnitOfWork;
using SyncronizationBot.Service.RabbitMQ.Consumers.Base;
using SyncronizationBot.Service.RabbitMQ.Queue.UpdateQueue.Configs;
using SyncronizationBot.Utils;
using SyncronizationBots.RabbitMQ.Exceptions;

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

        public override async Task HandlerAsync(IServiceScope _scope, string? message, CancellationToken stoppingToken)
        {
            var eventEventName = JsonConvert.DeserializeObject<MessageEvent<Entity>>(message ?? string.Empty)?.EventName;
            var data = eventEventName?.Split("_");

            if (data?.Count() != 2)
                throw new ArgumentException("Problem in process message");
            var classType = data?[0];
            var instruction = data?[1];
            switch (instruction)
            {
                case Constants.INSTRUCTION_INSERT:
                    await this.DoProcessInsert(_scope, message, classType);
                    break;
                case Constants.INSTRUCTION_UPDATE:
                    this.DoProcessUpdate(_scope, message, classType);
                    break;
                case Constants.INSTRUCTION_DELETE:
                default:
                    throw new ArgumentException($"Instruction {instruction} is not valid");
            }
        }

        private void DoProcessUpdate(IServiceScope scope, string? message, string? classType)
        {
            this.UnitOfWorkSqlServer = UnitOfWorkSqlServer ?? scope.ServiceProvider.GetRequiredService<IUnitOfWorkSqlServer>();
            switch (classType?.ToUpper())
            {
                case Constants.RUN_TIME_CONTROLLER_INSTRUCTION:
                    this.DoUpdate(message, this.UnitOfWorkSqlServer.RunTimeControllerRepository);
                    break;
                case Constants.WALLET_INSTRUCTION:
                    this.DoUpdate(message, this.UnitOfWorkSqlServer.WalletRepository);
                    break;
                default:
                    throw new ArgumentException($"Instruction UPDATE is not valid for classType {classType}");
            }
        }

        private async Task DoProcessInsert(IServiceScope scope, string? message, string? classType)
        {
            try 
            {
                switch (classType?.ToUpper())
                {
                    case Constants.WALLET_BALANCE_INSTRUCTION:
                        
                        await this.DoInsert(message, this.UnitOfWorkSqlServer.WalletBalanceRepository);
                        break;
                    case Constants.TOKEN_INSTRUCTION:
                        await this.DoInsert(message, this.UnitOfWorkSqlServer.TokenRepository);
                        break;
                    case Constants.WALLET_BALANCE_HISTORY_INSTRUCTION:
                        await this.DoInsert(message, this.UnitOfWorkSqlServer.WalletBalanceHistoryRepository);
                        break;
                    default:
                        throw new ArgumentException($"Instruction INSERT is not valid for classType {classType}");
                }
            }
            catch(Exception ex) 
            {
                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                    throw new RelationShipInsertException(ex.Source, ex);
            }
        }

        private async Task DoInsert<T>(string? message, IRepository<T> repository) where T : Entity
        {
            var messageEvent = message?.ToMessageEvent<T>();
            await repository!.AddAsync(messageEvent!.Entity!);
            await UnitOfWorkSqlServer.SaveChangesAsync();
        }

        private void DoUpdate<T>(string? message, IRepository<T> repository) where T : Entity
        {
            var messageEvent = message?.ToMessageEvent<T>();
            repository.Update(messageEvent!.Entity!);
            UnitOfWorkSqlServer.SaveChanges();
        }
    }
}
