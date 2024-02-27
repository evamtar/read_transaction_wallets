using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceExecute.Telegram.Message.Command;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.RabbitMQ;
using SyncronizationBot.Domain.Repository.UnitOfWork;
using SyncronizationBot.Domain.Service.InternalService.Alert;
using SyncronizationBot.Domain.Service.InternalService.Telegram;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.UpdateQueue;
using SyncronizationBot.Service.RabbitMQ.Consumers.Base;
using SyncronizationBot.Service.RabbitMQ.Queue.LogMessageQueue.Configs;
using SyncronizationBot.Utils;

namespace SyncronizationBot.Service.RabbitMQ.Consumers
{
    public class LogMessageQueueConsumerService : BaseSendTelegramMessageConsumer
    {
        private IMediator Mediator { get; set; }
        private IAlertConfigurationService AlertConfigurationService { get; set; }
        private IAlertInformationService AlertInformationService { get; set; }
        private IAlertParameterService AlertParameterService { get; set; }
        private ITelegramChannelService TelegramChannelService { get; set; }
        public LogMessageQueueConsumerService(IServiceProvider serviceProvider, IOptions<SyncronizationBotConfig> configs, IOptions<LogMessageQueueConfig> configuration) : base(serviceProvider, configuration.Value, configs)
        {
            this.Mediator = null!;
            this.TelegramChannelService = null!;
            this.AlertConfigurationService = null!;
            this.AlertInformationService = null!;
            this.AlertParameterService = null!;
        }

        public override async Task HandlerAsync(IServiceScope scope, string? message, CancellationToken stoppingToken)
        {
            this.InitServices(scope);
            var @event = JsonConvert.DeserializeObject<MessageEvent<Entity>>(message ?? string.Empty);
            if (@event != null) 
            {
                var messageToSend = await base.GetMessage(this.AlertConfigurationService, this.AlertInformationService, this.AlertParameterService, @event);
                if (!string.IsNullOrEmpty(messageToSend)) 
                {
                    var channel = await TelegramChannelService.FindFirstOrDefaultAsync(x => x.ID == TelegramChannelId);
                    if (channel != null) 
                    {
                        var response = await this.Mediator.Send(new ExecuteSendTelegramMessageCommand { ChannelId = (long)(channel.ChannelId ?? 0), Message = message });
                        var saveEntity = new TelegramMessage
                        {
                            EntityId = @event?.EntityId,
                            MessageId = response?.MessageId ?? 0,
                            DateSended = AdjustDateTimeToPtBR(response?.DateSended),
                            TelegramChannelId = base.TelegramChannelId,
                            IsDeleted = false
                        };
                        await base.TransferQueue(saveEntity, Constants.INSTRUCTION_INSERT, scope.ServiceProvider.GetRequiredService<IPublishUpdateService>());
                    }
                }
            }
        }

        private void InitServices(IServiceScope scope)
        {
            this.Mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            this.TelegramChannelService = scope.ServiceProvider.GetRequiredService<ITelegramChannelService>();
            this.AlertConfigurationService = scope.ServiceProvider.GetRequiredService<IAlertConfigurationService>();
            this.AlertInformationService = scope.ServiceProvider.GetRequiredService<IAlertInformationService>();
            this.AlertParameterService = scope.ServiceProvider.GetRequiredService<IAlertParameterService>();
        }

        public override void Dispose()
        {
            try
            {
                this.TelegramChannelService.Dispose();
                this.AlertConfigurationService.Dispose();
                this.AlertInformationService.Dispose();
                this.AlertParameterService.Dispose();
            }
            finally 
            {
                base.Dispose();
            }
        }
    }
}
