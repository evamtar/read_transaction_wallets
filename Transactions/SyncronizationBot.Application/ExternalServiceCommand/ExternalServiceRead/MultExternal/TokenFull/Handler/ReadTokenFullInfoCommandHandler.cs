using MediatR;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.Info.Command;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.Price.Command;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.TokenFull.Command;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.TokenFull.Response;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.TokenFull.Handler
{
    public class ReadTokenFullInfoCommandHandler : IRequestHandler<ReadTokenFullInfoCommand, ReadTokenFullInfoCommandResponse>
    {
        private readonly IMediator _mediator;
        public ReadTokenFullInfoCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<ReadTokenFullInfoCommandResponse> Handle(ReadTokenFullInfoCommand request, CancellationToken cancellationToken)
        {
            var taskPrice = Task.Factory.StartNew(async () =>
            {
                return await _mediator.Send(new ReadTokenPriceCommand { TokenHash = request.TokenHash });
            }).Unwrap();
            var taskInfo = Task.Factory.StartNew(async () =>
            {
                return await _mediator.Send(new ReadTokenInfoCommand { TokenHash = request.TokenHash });
            }).Unwrap();
            var waitProcess = new Task[] { taskPrice, taskInfo };
            await Task.WhenAll(waitProcess);
            var resultPrice = taskPrice.Result;
            var resultInfo = taskInfo.Result;
            return new ReadTokenFullInfoCommandResponse 
            {
                Name = resultInfo.Name ?? resultPrice.Name,
                Symbol = resultInfo.Symbol ?? resultPrice.Symbol,
                NumberOfMarkets = resultPrice.NumberOfMarkets,
                PriceUsd = resultPrice.PriceUsd,
                PriceChange5m = resultPrice.PriceChange30m,
                PriceChange1h = resultPrice.PriceChange1h,
                PriceChange4h = resultPrice.PriceChange4h,
                PriceChange6h = resultPrice.PriceChange6h,
                PriceChange24 = resultPrice.PriceChange24,
                Liquidity = resultPrice.Liquidity,
                Marketcap = resultPrice.Marketcap,
                Supply = resultInfo.Supply,
                Decimals = resultInfo.Decimals,
                MintAuthority = resultInfo.MintAuthority,
                FreezeAuthority = resultInfo.FreezeAuthority,
                UniqueWallet24H = resultPrice.UniqueWallet24H,
                UniqueWalletHistory24H = resultPrice.UniqueWalletHistory24H,
                CreateDate = GetDateTime(resultPrice.CreateDate)
            };
        }

        private DateTime? GetDateTime(DateTime? dateTime) 
        {
            var returnValue = dateTime ?? DateTime.Now.AddHours(-5);
            return returnValue > new DateTime(1970, 1, 1) ? returnValue : DateTime.Now.AddHours(-5);
        }
    }
}
