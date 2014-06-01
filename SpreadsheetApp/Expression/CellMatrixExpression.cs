using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetApp
{
    class CellMatrixExpression : Expression
    {
        // Cell matric Addr
        public CellMatrixExpression(CellMatrixAddress addr)
        {
            this.matAddr = addr;
            this.exprType = ExprType.Matrix;
        }
    }
}
