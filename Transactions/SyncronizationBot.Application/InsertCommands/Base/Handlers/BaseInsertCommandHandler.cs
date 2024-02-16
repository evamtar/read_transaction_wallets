﻿using MediatR;
using SyncronizationBot.Application.InsertCommands.Base.Commands;
using SyncronizationBot.Application.InsertCommands.Base.Response;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.Base;

namespace SyncronizationBot.Application.InsertCommands.Base.Handlers
{
    public class BaseInsertCommandHandler<X, W, T> : IRequestHandler<X, W>
                                           where X : BaseInsertCommand<W, T>
                                           where W : BaseInsertCommandResponse<T>
                                           where T : Entity
    {
        private readonly IWriteCommandRepository<T> _repository;

        public BaseInsertCommandHandler(IRepository<T> repository)
        {
            _repository = repository ?? throw new ArgumentException($"IRepository<T> --> {typeof(T)} is null here.");
        }

        public async Task<W> Handle(X request, CancellationToken cancellationToken)
        {
            if (request.Entity != null)
            {
                var entity = await _repository.Add(request.Entity);
                var response = Activator.CreateInstance<W>();
                response.Entity = entity;
                return response;
            }
            throw new ArgumentException($"Entity --> {typeof(T)} is null here.");
        }
    }
}
