using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetApp
{
    class NumberCell : LiteralCell
    {
        public double val;
        public NumberCell(double val)
        {
            this.val = val;        
        }
        public override string Show()
        {
            return val.ToString();
        }
        public override string ShowRaw()
        {
            return val.ToString();
        }
        public override double Evaluate()
        {
            return val;
        }
     }
}
