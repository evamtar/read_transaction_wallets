using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadTransactionsWallets.Domain.Model.Enum
{
    public enum ETypeOperation
    {
        None = 0,
        Buy = 1,
        Sell = 2,
        Send = 3,
        Received = 4,
        Swap = 5
    }
}
