using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SpreadsheetApp
{

    abstract class Expression
    {
        public enum ExprType
        {
            Address,
            Matrix,
            Literal,
            InfixOperator
        };

        public ExprType exprType;
        public double value;
        public CellAddress addr;
        public CellMatrixAddress matAddr;
        public string operatorSign;
        public Expression()
        {

        }

    }
}
