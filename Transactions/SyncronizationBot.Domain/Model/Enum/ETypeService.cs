

using System.ComponentModel;

namespace SyncronizationBot.Domain.Model.Enum
{
    public enum ETypeService
    {
        NONE = 0,
        [DescriptionAttribute("Alerta de Transações")]
        Transaction = 1,
        [DescriptionAttribute("Carregar balanços das wallets")]
        Balance,
        [DescriptionAttribute("Alerta de preços")]
        Price,
        [DescriptionAttribute("Excluir mensagens de log antigas")]
        DeleteOldMessages,
        [DescriptionAttribute("Alerta de Token Alpha")]
        AlertTokenAlpha,
        [DescriptionAttribute("Transacões Antigas para Mapear")]
        TransactionsOldForMapping,
        [DescriptionAttribute("Carregar Listagem de Novos Tokens")]
        NewTokensBetAwards
    }
}
