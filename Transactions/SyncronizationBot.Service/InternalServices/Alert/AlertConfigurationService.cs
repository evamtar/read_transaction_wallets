﻿using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.UnitOfWork;
using SyncronizationBot.Domain.Service.InternalService.Alert;
using SyncronizationBot.Service.InternalServices.Base;


namespace SyncronizationBot.Service.InternalServices.Alert
{
    public class AlertConfigurationService : CachedServiceBase<AlertConfiguration>, IAlertConfigurationService
    {
        public AlertConfigurationService(IUnitOfWorkSqlServerReadyOnly unitOfWorkSqlServerReadyOnly, IUnitOfWorkMongo unitOfWorkMongo) : base(unitOfWorkSqlServerReadyOnly, unitOfWorkMongo)
        {
        }
    }
}