﻿using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.MongoDB.Base;



namespace SyncronizationBot.Domain.Repository.MongoDB
{
    public interface ITokenAlphaConfigurationRepository : ICachedRepository<TokenAlphaConfiguration>
    {
    }
}