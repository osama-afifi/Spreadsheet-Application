using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetApp
{
    class InfixOperatorExpr : Expression
    {
        //public string operatorSign;
        static public Dictionary<string, int> precedenceMap;
        public InfixOperatorExpr(string operatorSign)
        {
            precedenceMap = new Dictionary<string, int>();
            initializePrecedence();
            this.operatorSign = operatorSign;
            this.exprType = ExprType.InfixOperator;
        }

        public static void initializePrecedence()
        {
            precedenceMap = new Dictionary<string, int>();
            precedenceMap.Add("+", 100);
            precedenceMap.Add("-", 100);
            precedenceMap.Add("/", 50);
            precedenceMap.Add("*", 50);

            //brackets special case
            precedenceMap.Add(")", 1000);
            precedenceMap.Add("(", 1000);


        }
        static public bool precede(string firstOp, string secondOp)
        {
            return precedenceMap[firstOp] < precedenceMap[secondOp];        
        }
        static public double binaryOperation(double a, double b, string opr)
        {
            double res = 0;
            switch (opr)
            {
                case "+":
                    res = a + b;
                    break;
                case "-":
                    res = a - b;
                    break;
                case "*":
                    res = a * b;
                    break;
                case "/":
                    res = a / b;
                    break;
            }
            return res;
        }

    }
}
