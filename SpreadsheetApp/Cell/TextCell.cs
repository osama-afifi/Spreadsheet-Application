using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetApp
{
    class TextCell : LiteralCell
    {
        public string val;
        public TextCell(string val)
        {
            this.val = val;
        }
        public override string Show()
        {
            return val;
        }
        public override string ShowRaw()
        {
            return val;
        }
        public override double Evaluate()
        {
            return -1;
        }
    }
}
