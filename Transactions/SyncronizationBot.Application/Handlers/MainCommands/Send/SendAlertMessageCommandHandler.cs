using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.Response.MainCommands.Send;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using System.Collections;
using System.Reflection;

namespace SyncronizationBot.Application.Handlers.MainCommands.Send
{
    public class SendAlertMessageCommandHandler : IRequestHandler<SendAlertMessageCommand, SendAlertMessageCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly IAlertConfigurationRepository _alertConfigurationRepository;
        private readonly IAlertInformationRepository _alertInformationRepository;
        private readonly IAlertParameterRepository _alertParameterRepository;
        private readonly IOptions<SyncronizationBotConfig> _syncronizationBotConfig;
        public SendAlertMessageCommandHandler(IMediator mediator,
                                              IAlertConfigurationRepository alertConfigurationRepository,
                                              IAlertInformationRepository alertInformationRepository,
                                              IAlertParameterRepository alertParameterRepository,
                                              IOptions<SyncronizationBotConfig> syncronizationBotConfig)
        {
            this._mediator = mediator;
            this._alertConfigurationRepository = alertConfigurationRepository;
            this._alertInformationRepository = alertInformationRepository;
            this._alertParameterRepository = alertParameterRepository;
            this._syncronizationBotConfig = syncronizationBotConfig;
        }

        public async Task<SendAlertMessageCommandResponse> Handle(SendAlertMessageCommand request, CancellationToken cancellationToken)
        {
            var configuration = await _alertConfigurationRepository.FindFirstOrDefault(x => x.TypeAlert == request.TypeAlert);
            if (configuration != null)
            {
                var informations = await _alertInformationRepository.Get(x => x.AlertConfigurationId == configuration.ID && (request.IdClassification == null || x.IdClassification == null || x.IdClassification == request.IdClassification));
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
                        var parameterValue = GetParameterValue(objParameter, parameter.Parameter, parameter.FixValue, parameter.DefaultValue, parameter.HasAdjustment);
                        message = message!.Replace(parameter.Name ?? string.Empty, parameterValue);
                    }
                }
            }
            return message;
        }

        private string? GetParameterValue(object objParameter, string? parameter, string? fixValue, string? defaultValue, bool? hasAdjustment)
        {
            if (fixValue != null) return fixValue;
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
                        {
                            Type? type = objectFinded?.GetType();
                            if ((type?.IsGenericType ?? false) && type?.GetGenericTypeDefinition() == typeof(List<>))
                            {
                                var genericList = ((IList?)objParameter);
                                int.TryParse(splitValue.Replace("[", string.Empty).Replace("]", string.Empty), out var index);
                                objectFinded = genericList?[index];
                            }
                            else
                            propertyFinded = objectFinded?.GetType().GetProperty(splitValue);
                        }
                            
                        else
                        {
                            objectFinded = propertyFinded.GetValue(objectFinded);
                            propertyFinded = objectFinded?.GetType().GetProperty(splitValue);
                        }

                    }
                    return propertyFinded?.GetValue(objectFinded)?.GetType() == typeof(DateTime?) || propertyFinded?.GetValue(objectFinded)?.GetType() == typeof(DateTime) ? AdjustDateTimeToPtBR((DateTime?)propertyFinded?.GetValue(objectFinded), hasAdjustment) :
                           propertyFinded?.GetValue(objectFinded)?.GetType() == typeof(bool?) || propertyFinded?.GetValue(objectFinded)?.GetType() == typeof(bool) ? RecoveryDefaultAswers((bool?)propertyFinded?.GetValue(objectFinded)):
                           !string.IsNullOrEmpty(propertyFinded?.GetValue(objectFinded)?.ToString()) ? propertyFinded?.GetValue(objectFinded)?.ToString() : defaultValue;
                }
            }
            return defaultValue;
        }

        private string? AdjustDateTimeToPtBR(DateTime? dateTime, bool? hasAdjustment)
        {
            if (hasAdjustment ?? false)
                return (dateTime?.AddHours(this._syncronizationBotConfig.Value.GTMHoursAdjust ?? 0) ?? DateTime.MinValue).ToString("dd/MM/yyyy HH:mm:ss");
            return dateTime?.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private string? RecoveryDefaultAswers(bool? boolValue) 
        {
            return boolValue == true ? "YES" : "NO";
        }

    }
}
