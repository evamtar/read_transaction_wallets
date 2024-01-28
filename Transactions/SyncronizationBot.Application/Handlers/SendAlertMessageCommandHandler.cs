using MediatR;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Repository;

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

        public Task<SendAlertMessageCommandResponse> Handle(SendAlertMessageCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
