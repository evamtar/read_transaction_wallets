

using System.ComponentModel;

namespace SyncronizationBot.Domain.Model.Enum
{
    public enum ETypeService
    {
        [DescriptionAttribute("Alerta de Transações")]
        Transaction = 1,
        [DescriptionAttribute("Carregar balanços da wallet")]
        Balance,
        [DescriptionAttribute("Alerta de preços")]
        Price,
        [DescriptionAttribute("Excluir mensagens de log antigas")]
        DeleteOldMessages,
    }
}
