using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Extensions;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.RabbitMQ;
using SyncronizationBot.Domain.Repository.SQLServer;
using SyncronizationBot.Domain.Repository.SQLServer.Base;
using SyncronizationBot.Service.RabbitMQ.Consumers.Base;
using SyncronizationBot.Service.RabbitMQ.Queue.UpdateQueue.Configs;
using SyncronizationBot.Utils;

namespace SyncronizationBot.Service.RabbitMQ.Consumers
{
    public class UpdateQueueConsumerService : BaseBatchMessageConsumer
    {
        

        public UpdateQueueConsumerService(IServiceProvider serviceProvider, 
                                          IOptions<UpdateQueueConfiguration> configuration) : base(serviceProvider, configuration.Value)
        {

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
            
            switch (classType?.ToUpper())
            {
                case Constants.RUN_TIME_CONTROLLER_INSTRUCTION:
                    this.DoUpdate<RunTimeController, IRunTimeControllerRepository>(scope, message);
                    break;
                case Constants.WALLET_INSTRUCTION:
                    this.DoUpdate<Wallet, IWalletRepository>(scope, message);
                    break;
                default:
                    throw new ArgumentException($"Instruction UPDATE is not valid for classType {classType}");
            }
        }

        private async Task DoProcessInsert(IServiceScope scope, string? message, string? classType)
        {
            switch (classType?.ToUpper())
            {
                case Constants.WALLET_BALANCE_INSTRUCTION:
                    await this.DoInsert<WalletBalance, IWalletBalanceRepository>(scope, message);
                    break;
                case Constants.TOKEN_INSTRUCTION:
                    await this.DoInsert<Token, ITokenRepository>(scope, message);
                    break;
                default:
                    throw new ArgumentException($"Instruction INSERT is not valid for classType {classType}");
            }
        }

        private async Task DoInsert<T, W>(IServiceScope scope, string? message) where T : Entity
                                                                                where W : IRepository<T>
        {
            var messageEvent = message?.ToMessageEvent<T>();
            var repository = scope.ServiceProvider.GetRequiredService<W>();
            await repository.AddAsync(messageEvent!.Entity!);
        }
        private void DoUpdate<T, W>(IServiceScope scope, string? message) where T : Entity
                                                                          where W : IRepository<T>
        {
            var messageEvent = message?.ToMessageEvent<T>();
            var repository = scope.ServiceProvider.GetRequiredService<W>();
            repository.Update(messageEvent!.Entity!);
        }
    }
}
