using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadTransactionsWallets.Domain.Model.Utils.Transfer
{
    public enum ETypeOfTransfer
    {
        DEBIT = 0,
        CREDIT = 1,
        PAYMENT_FEE = 2,
    }
}
