using AutoMapper;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.SolnetRpc.Balance.Response;
using SyncronizationBot.Domain.Model.CrossCutting.SolnetRpc.Balance.Response;


namespace SyncronizationBot.Application.AutoMapper.Profiles
{
    public class ServiceMediatorProfile : Profile
    {
        public ServiceMediatorProfile()
        {
            /*  SolnetBalance  (RPC)   */
            CreateMap<SolnetBalanceResponse, SolnetBalanceReadCommandResponse>().ReverseMap();
            CreateMap<BalanceResponse, SolnetReadBalanceResponse>().ReverseMap();
            CreateMap<TokenResponse, SolnetReadTokenResponse>().ReverseMap();
        }
    }
}
