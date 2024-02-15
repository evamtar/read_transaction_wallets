using MediatR;
using Moq;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.Handlers.MainCommands.Send;
using SyncronizationBot.Application.Response.MainCommands.AddUpdate;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Model.Utils.Transfer;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SyncronizationBot.Test
{
    [TestClass]
    public class AlertTest
    {
        [TestMethod]
        public async Task AlertForBuy() 
        {
            //var mediator = new Mock<IMediator>().Object;
            //var alertConfiguration = new Mock<IAlertConfigurationRepository>();
            //alertConfiguration.Setup(x => x.FindFirstOrDefault(It.IsAny<Expression<Func<AlertConfiguration, bool>>>(), It.IsAny<Expression<Func<AlertConfiguration, object>>>())).ReturnsAsync(new AlertConfiguration { ID = Guid.NewGuid() });
            //var alertInformation = new Mock<IAlertInformationRepository>();
            //alertInformation.Setup(x => x.Get(It.IsAny<Expression<Func<AlertInformation, bool>>>())).ReturnsAsync(new List<AlertInformation> { new AlertInformation { ID = Guid.NewGuid(), Message = "<b>*** NEW BUY ALERT ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>🟢🟢🟢🟢🟢🟢🟢🟢🟢🟢🟢</tg-emoji>{{NEWLINE}}<s>Signature:</s> {{Signature}}{{NEWLINE}}<s>WalletHash:</s> {{WalletHash}{{NEWLINE}}<s>ClassWallet:</s> {{ClassWallet}} {{NEWLINE}}<s>Token:</s> {{Token}}{{NEWLINE}}<s>Ca:</s> {{Ca}}<pre>{{NEWLINE}}</pre><s>Minth Authority:</s>{{MinthAuthority}}{{NEWLINE}}<s>Freeze Authority:</s> {{FreezeAuthority}}{{NEWLINE}}<s>Is Mutable:</s>{{IsMutable}}<s>Quantity:</s> {{Quantity}}{{NEWLINE}}<s>Value Spent:</s> {{ValueSpent}}{{NEWLINE}}<s>Date:</s> {{Date}}{{NEWLINE}}<s>Position Increase</s> {{PositionIncrease}}{{NEWLINE}}<a href=''https://birdeye.so/token/{{Ca}}?chain=solana''>Chart</a>" } });
            //var alertParameters = new Mock<IAlertParameterRepository>();
            //alertParameters.Setup(x => x.Get(It.IsAny<Expression<Func<AlertParameter, bool>>>())).ReturnsAsync(new List<AlertParameter> { new AlertParameter { ID = Guid.NewGuid(), Class= "System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]", Parameter="[0].Name" } });
            //SendAlertMessageCommand command = new SendAlertMessageCommand
            //{
            //    Parameters = SendAlertMessageCommand.GetParameters(new object[]
            //                                                    {
            //                                             new Transactions{ Signature = "1", TypeOperation = ETypeOperation.BUY },
            //                                             new TransferInfo(new MappedTokensConfig{ }){ },
            //                                             new List<RecoverySaveTokenCommandResponse?> {
            //                                                                                           new RecoverySaveTokenCommandResponse
            //                                                                                           {
            //                                                                                               Name = "TokenSend"
            //                                                                                           },
            //                                                                                           new RecoverySaveTokenCommandResponse
            //                                                                                           {
            //                                                                                               Name = "TokenSendPool"
            //                                                                                           },
            //                                                                                           new RecoverySaveTokenCommandResponse
            //                                                                                           {
            //                                                                                               Name = "TokenReceived"
            //                                                                                           },
            //                                                                                           new RecoverySaveTokenCommandResponse
            //                                                                                           {
            //                                                                                               Name = "TokenReceivedPool"
            //                                                                                           }
            //                                             } ,
            //                                             new RecoveryAddUpdateBalanceItemCommandResponse{ }
            //                                                    }),
            //    TypeAlert = ETypeAlert.BUY,
            //    IdClassification = 1
            //};
            //SendAlertMessageCommandHandler handler = new SendAlertMessageCommandHandler(mediator, alertConfiguration.Object, alertInformation.Object, alertParameters.Object);

            //Act
            //var response = await handler.Handle(command, new System.Threading.CancellationToken());
        }
    }
}
