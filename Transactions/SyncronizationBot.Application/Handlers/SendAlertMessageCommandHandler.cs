using MediatR;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Utils;
using System.Reflection;
using System.Reflection.Metadata;

namespace SyncronizationBot.Application.Handlers
{
    public class SendAlertMessageCommandHandler : IRequestHandler<SendAlertMessageCommand, SendAlertMessageCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly IAlertConfigurationRepository _alertConfigurationRepository;
        private readonly IAlertInformationRepository _alertInformationRepository;
        private readonly IAlertParameterRepository _alertParameterRepository;
        public SendAlertMessageCommandHandler(IMediator mediator,
                                              IAlertConfigurationRepository alertConfigurationRepository,
                                              IAlertInformationRepository alertInformationRepository,
                                              IAlertParameterRepository alertParameterRepository)
        {
            this._mediator = mediator;
            this._alertConfigurationRepository = alertConfigurationRepository;
            this._alertInformationRepository = alertInformationRepository;
            this._alertParameterRepository = alertParameterRepository;
        }

        public async Task<SendAlertMessageCommandResponse> Handle(SendAlertMessageCommand request, CancellationToken cancellationToken)
        {
            var configuration = await this._alertConfigurationRepository.FindFirstOrDefault(x => x.TypeAlert == request.TypeAlert);
            if (configuration != null) 
            {
                var informations = await this._alertInformationRepository.Get(x => x.AlertConfigurationId == configuration.ID);
                if(informations != null && informations.Any()) 
                {
                    foreach (var information in informations)
                    {
                        var parameters = await this._alertParameterRepository.Get(x => x.AlertInformationId == information.ID);
                        var message = this.ReplaceParametersInformation(request.Parameters, information, parameters);
                        message = message?.Replace("{{NEWLINE}}", Environment.NewLine);
                        await this._mediator.Send(new SendTelegramMessageCommand
                        {
                            TelegramChannelId = configuration.TelegramChannelId,
                            Channel = ETelegramChannel.None,
                            Message = message
                        });
                    }
                }
            }
            return new SendAlertMessageCommandResponse{ };
        }

        private string? ReplaceParametersInformation(Dictionary<string, object>? parametersObjects, AlertInformation information, IEnumerable<AlertParameter> listOfParameters)
        {
            var message = information.Message;
            if (listOfParameters != null) 
            {
                foreach (var parameter in listOfParameters)
                {
                    if (parametersObjects != null) 
                    {
                        var objParameter = parametersObjects[parameter.Class!];
                        var parameterValue = this.GetParameterValue(objParameter, parameter.Parameter);
                        message = message!.Replace(parameter.Name ?? string.Empty, parameterValue);
                    }
                }
            }
            return message;
        }

        private string? GetParameterValue(object objParameter, string? parameter)
        {
            if (parameter != null) 
            {
                var splitData = parameter.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if(splitData.Length > 0) 
                {
                    var propertyFinded = (PropertyInfo?)null;
                    var objectFinded = objParameter;
                    foreach (var splitValue in splitData) 
                    {
                        if (propertyFinded == null)
                            propertyFinded = objectFinded?.GetType().GetProperty(splitValue);
                        else 
                        {
                            objectFinded = propertyFinded.GetValue(objectFinded);
                            propertyFinded = objectFinded?.GetType().GetProperty(splitValue);
                        }
                            
                    }
                    return propertyFinded?.GetValue(objectFinded)?.ToString();
                }
            }
            return string.Empty;
        }
    }
}
