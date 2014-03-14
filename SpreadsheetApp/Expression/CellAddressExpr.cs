using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetApp
{
    class CellAddressExpr : Expression
    {
        //public CellAdresss addr;
        public CellAddressExpr(CellAdresss addr)
        {
            this.addr = addr;
            this.exprType = ExprType.Address;
        }
        public CellAddressExpr(string alphaAddr)
        {
            addr = new CellAdresss(alphaAddr);
        }
    }
}
