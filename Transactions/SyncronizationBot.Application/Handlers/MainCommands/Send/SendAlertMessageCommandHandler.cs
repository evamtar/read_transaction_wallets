using MediatR;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.Response.MainCommands.Send;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Utils;
using System.Reflection;
using System.Reflection.Metadata;

namespace SyncronizationBot.Application.Handlers.MainCommands.Send
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
            _mediator = mediator;
            _alertConfigurationRepository = alertConfigurationRepository;
            _alertInformationRepository = alertInformationRepository;
            _alertParameterRepository = alertParameterRepository;
        }

        public async Task<SendAlertMessageCommandResponse> Handle(SendAlertMessageCommand request, CancellationToken cancellationToken)
        {
            var configuration = await _alertConfigurationRepository.FindFirstOrDefault(x => x.TypeAlert == request.TypeAlert);
            if (configuration != null)
            {
                var informations = await _alertInformationRepository.Get(x => x.AlertConfigurationId == configuration.ID && (request.IdClassification == null || x.IdClassification == request.IdClassification));
                if (informations != null && informations.Any())
                {
                    foreach (var information in informations)
                    {
                        var parameters = await _alertParameterRepository.Get(x => x.AlertInformationId == information.ID);
                        var message = ReplaceParametersInformation(request.Parameters, information, parameters);
                        message = message?.Replace("{{NEWLINE}}", Environment.NewLine);
                        await _mediator.Send(new SendTelegramMessageCommand
                        {
                            TelegramChannelId = configuration.TelegramChannelId,
                            Channel = ETelegramChannel.None,
                            Message = message
                        });
                    }
                }
            }
            return new SendAlertMessageCommandResponse { };
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
                        var parameterValue = GetParameterValue(objParameter, parameter.Parameter);
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
                if (splitData.Length > 0)
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
