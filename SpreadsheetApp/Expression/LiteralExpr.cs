using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetApp
{
    class LiteralExpr : Expression
    {
        //public double value;
        public LiteralExpr(double value)
        {
            this.value = value;
            this.exprType = ExprType.Literal;
        }
    }
}
