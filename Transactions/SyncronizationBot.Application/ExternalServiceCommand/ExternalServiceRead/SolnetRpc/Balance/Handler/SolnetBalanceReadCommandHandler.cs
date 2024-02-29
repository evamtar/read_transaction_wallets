using AutoMapper;
using MediatR;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.SolnetRpc.Balance.Command;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.SolnetRpc.Balance.Response;
using SyncronizationBot.Domain.Model.CrossCutting.SolnetRpc.Balance.Request;
using SyncronizationBot.Domain.Service.CrossCutting.SolnetRpc.Balance;
using System.Net.Sockets;

namespace SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.SolnetRpc.Balance.Handler
{
    public class SolnetBalanceReadCommandHandler : IRequestHandler<SolnetBalanceReadCommand, SolnetBalanceReadCommandResponse>
    {
        private readonly ISolnetBalanceService _solnetBalanceService;
        private readonly IMapper _mapper;
        public SolnetBalanceReadCommandHandler(ISolnetBalanceService solnetBalanceService, IMapper mapper)
        {
            this._solnetBalanceService = solnetBalanceService;
            this._mapper = mapper;
        }
        public async Task<SolnetBalanceReadCommandResponse> Handle(SolnetBalanceReadCommand request, CancellationToken cancellationToken)
        {
            var serviceRequest = new SolnetBalanceRequest
            {
                WalletHash = request.WalletHash,
                IgnoreAmountValueZero = request.IgnoreAmountValueZero
            };
            var serviceResponse = await this._solnetBalanceService.ExecuteRecoveryWalletBalanceAsync(serviceRequest);
            return this._mapper.Map<SolnetBalanceReadCommandResponse>(serviceResponse);
        }
    }
}
