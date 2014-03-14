using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetApp
{
    abstract class LiteralCell : Cell
    {

        public LiteralCell()
        {
            visited = updated = false;
        }
        abstract override public double Evaluate();
        abstract override public string Show();
        abstract override public string ShowRaw();

    }
}
