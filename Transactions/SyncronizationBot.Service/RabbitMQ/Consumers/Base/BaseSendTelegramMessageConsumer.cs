using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.RabbitMQ;
using SyncronizationBot.Domain.Repository.UnitOfWork;
using SyncronizationBot.Domain.Service.InternalService.Alert;
using SyncronizationBots.RabbitMQ.Queue.Interface;
using System;
using System.Collections;
using System.Reflection;

namespace SyncronizationBot.Service.RabbitMQ.Consumers.Base
{
    public abstract class BaseSendTelegramMessageConsumer : BaseBatchMessageConsumer
    {
        private readonly IOptions<SyncronizationBotConfig> _configs;
        protected Guid? TelegramChannelId { get; private set; }
        public BaseSendTelegramMessageConsumer(IServiceProvider serviceProvider, IQueueConfiguration queueConfiguration, IOptions<SyncronizationBotConfig> configs) 
                                                                                : base(serviceProvider, queueConfiguration)
        {
            _configs = configs;
            TelegramChannelId = null!;
        }

        
        protected async Task<string?> GetMessage<T>(IAlertConfigurationService alertConfigurationService, IAlertInformationService alertInformationService, IAlertParameterService alertParameterService, MessageEvent<T> @event) where T : Entity
        {
            var configuration = await alertConfigurationService.FindFirstOrDefaultAsync(x => x.TypeOperationId == @event.TypeOperationId);
            if (configuration != null)
            {
                var informations = await alertInformationService.GetAsync(x => x.AlertConfigurationId == configuration.ID && (@event.IdSubLevelAlertConfiguration == null || x.IdSubLevel == null || x.IdSubLevel == @event.IdSubLevelAlertConfiguration));
                if (informations != null && informations.Any())
                {
                    TelegramChannelId = configuration.TelegramChannelId;
                    foreach (var information in informations)
                    {
                        var parameters = await alertParameterService.GetAsync(x => x.AlertInformationId == information.ID);
                        var messageToSend = ReplaceParametersInformation(@event.Parameters, information, parameters);
                        messageToSend = messageToSend?.Replace("{{NEWLINE}}", Environment.NewLine);
                        return messageToSend;
                    }
                }
            }
            return string.Empty;
        }

        protected DateTime? AdjustDateTimeToPtBR(DateTime? dateTime) 
        {
            return dateTime?.AddHours(_configs.Value.GTMHoursAdjust ?? 0);
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
                        var parameterValue = GetParameterValue(objParameter, parameter.Parameter, parameter.FixValue, parameter.DefaultValue, parameter.FormatValue, parameter.HasAdjustment);
                        message = message!.Replace(parameter.Name ?? string.Empty, parameterValue);
                    }
                }
            }
            return message;
        }

        private string? GetParameterValue(object objParameter, string? parameter, string? fixValue, string? defaultValue, string? formatValue, bool? hasAdjustment)
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
                                if (splitValue.StartsWith("Invoke"))
                                {
                                    if (splitValue.Contains("|"))
                                    {
                                        var separetedInstructionAndParameters = splitValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                                        var subInstruction = separetedInstructionAndParameters[0].Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (subInstruction[subInstruction.Length - 1] == "Sum")
                                        {
                                            var sum = (decimal?)0;
                                            foreach (var item in genericList)
                                            {
                                                var objPropertyInfo = item.GetType().GetProperty(separetedInstructionAndParameters[separetedInstructionAndParameters.Length - 1]);
                                                var value = objPropertyInfo?.GetValue(item);
                                                if (value?.GetType() == typeof(decimal) || value?.GetType() == typeof(decimal?) ||
                                                    value?.GetType() == typeof(double) || value?.GetType() == typeof(double?) ||
                                                    value?.GetType() == typeof(int) || value?.GetType() == typeof(int?))
                                                    sum += (decimal?)objPropertyInfo?.GetValue(item);
                                            }
                                            return formatValue != null ? sum?.ToString(formatValue) : sum?.ToString();
                                        }
                                    }
                                    else
                                    {
                                        var subInstruction = splitValue.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (subInstruction[subInstruction.Length - 1] == "Count")
                                            return genericList?.Count.ToString();
                                    }
                                }
                                else if (splitValue.StartsWith("RANGE"))
                                {
                                    if (splitValue.Contains("|"))
                                    {
                                        var separetedInstructionAndParameters = splitValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                                        var subInstruction = separetedInstructionAndParameters[0].Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (subInstruction[subInstruction.Length - 1] == "ALL")
                                        {
                                            var aggregateValue = string.Empty;
                                            foreach (var item in genericList)
                                            {
                                                var objPropertyInfo = item.GetType().GetProperty(separetedInstructionAndParameters[separetedInstructionAndParameters.Length - 1]);
                                                aggregateValue += " - " + objPropertyInfo?.GetValue(item)?.ToString() + "{{NEWLINE}}";
                                            }
                                            return aggregateValue;
                                        }
                                        else
                                        {
                                            //TODO
                                        }
                                    }
                                    else
                                    {
                                        //TODO
                                    }
                                }
                                else if (splitValue.StartsWith("AGGREGATE"))
                                {
                                    var separetedInstructionAndParameters = splitValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                                    var aggregateResult = new Dictionary<string, int>();
                                    var resultAggregated = string.Empty;
                                    foreach (var item in genericList)
                                    {
                                        var objPropertyInfo = item.GetType().GetProperty(separetedInstructionAndParameters[separetedInstructionAndParameters.Length - 1]);
                                        var aggregateValue = objPropertyInfo?.GetValue(item)?.ToString();
                                        if (!string.IsNullOrEmpty(aggregateValue))
                                        {
                                            if (!aggregateResult.ContainsKey(aggregateValue))
                                                aggregateResult.Add(aggregateValue, 1);
                                            else
                                                aggregateResult[aggregateValue] += 1;
                                        }
                                    }
                                    foreach (var aggregateItem in aggregateResult)
                                        resultAggregated += " - " + aggregateItem.Key + "(" + aggregateItem.Value.ToString() + ") {{NEWLINE}}";
                                    return resultAggregated;
                                }
                                else
                                {
                                    int.TryParse(splitValue.Replace("[", string.Empty).Replace("]", string.Empty), out var index);
                                    objectFinded = genericList?[index];
                                }
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
                           propertyFinded?.GetValue(objectFinded)?.GetType() == typeof(bool?) || propertyFinded?.GetValue(objectFinded)?.GetType() == typeof(bool) ? RecoveryDefaultAswers((bool?)propertyFinded?.GetValue(objectFinded)) :
                           !string.IsNullOrEmpty(propertyFinded?.GetValue(objectFinded)?.ToString()) ? RecoveryFormatedValue(propertyFinded?.GetValue(objectFinded), formatValue) : defaultValue;
                }
            }
            return defaultValue;
        }

        private string? RecoveryFormatedValue(object? objToFormat, string? formatValue)
        {
            if (formatValue == null || formatValue == string.Empty || objToFormat == null) return objToFormat?.ToString();
            else
            {
                if (objToFormat?.GetType() == typeof(decimal))
                    return ((decimal)objToFormat).ToString(formatValue);
                else if (objToFormat?.GetType() == typeof(int))
                    return ((int)objToFormat).ToString(formatValue);
                else if (objToFormat?.GetType() == typeof(double))
                    return ((double)objToFormat).ToString(formatValue);
                else if (objToFormat?.GetType() == typeof(long))
                    return ((long)objToFormat).ToString(formatValue);
                return string.Empty;
            }
        }

        private string? AdjustDateTimeToPtBR(DateTime? dateTime, bool? hasAdjustment)
        {
            if (hasAdjustment ?? false)
                return (dateTime?.AddHours(_configs.Value.GTMHoursAdjust ?? 0) ?? DateTime.MinValue).ToString("dd/MM/yyyy HH:mm:ss");
            return dateTime?.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private string? RecoveryDefaultAswers(bool? boolValue)
        {
            return boolValue == true ? "YES" : "NO";
        }

        
    }
}
