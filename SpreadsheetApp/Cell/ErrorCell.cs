using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetApp
{
    class ErrorCell : Cell
    {
        public enum ErrorType
        {
            General,
            InvalidType,
            DependencyLoop            
        }
        ErrorType errorType;
        public ErrorCell(ErrorType errorType = ErrorType.General)
        {
            this.errorType = errorType;
        }
        public override string Show()
        {
            return ShowRaw();
        }
        public override string ShowRaw()
        {
            if (errorType == ErrorType.DependencyLoop)
                return "ERROR!: DEPENDENCY LOOP";
            else if (errorType == ErrorType.InvalidType)
                return "ERROR!: DEPEND ON INVALID TYPE";
            else
                return "ERROR!:";
        }
        public override double Evaluate()
        {
            return -1;
        }
    }
}
