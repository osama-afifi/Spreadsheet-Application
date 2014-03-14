using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetApp
{
    class Parser
    {
        public bool ValidFormula;
        public Parser()
        {
            ValidFormula = true;
        }


        public List<Expression> ParseFormula(string rawFormula)
        {
            List<Expression> ExprList = new List<Expression>();
            rawFormula = rawFormula.Replace(" ", ""); //strip spaces
            rawFormula = rawFormula.Replace("\n", ""); //strip newlines
            rawFormula = rawFormula.Replace("\t", "");
            rawFormula = rawFormula.Replace("\r", "");
            if (rawFormula == "" || rawFormula[0] != '=' || !validParantheses(rawFormula))
                ValidFormula = false;
            try
            {
                bool prevOperator = false;

                for (int i = 0; i < rawFormula.Length; i++)
                {
                    // Check if Digit or . or -ve sign in special cases
                    if (i == 0 && rawFormula[i] == '=') ;
                    else if (Char.IsDigit(rawFormula[i]) || rawFormula[i] == '.' || (prevOperator && rawFormula[i] == '-'))
                    {
                        string num = "";
                        if (rawFormula[i] == '-')
                        {
                            num += "-";
                            ++i;
                        }
                        while (i < rawFormula.Length && (Char.IsDigit(rawFormula[i]) || rawFormula[i] == '.'))
                        {
                            num += rawFormula[i];
                            ++i;
                        }
                        --i;

                        ExprList.Add(new LiteralExpr(double.Parse(num)));
                    }
                    else if (InfixOperatorExpr.precedenceMap.ContainsKey("" + rawFormula[i]) == true)
                    {
                        ExprList.Add(new InfixOperatorExpr("" + rawFormula[i]));
                        prevOperator = true;
                    }
                    else if (char.IsUpper(rawFormula[i]))
                    {
                        string column = "" + rawFormula[i];
                        string row = "";
                        ++i;
                        while (i < rawFormula.Length && char.IsUpper(rawFormula[i]))
                        {
                            column += rawFormula[i];
                            ++i;
                        }
                        if (i < rawFormula.Length && rawFormula[i] == '$')
                            ++i;
                        while (i < rawFormula.Length && char.IsDigit(rawFormula[i]))
                        {
                            row += rawFormula[i];
                            ++i;
                        }
                        --i;
                        int rowIndex = int.Parse(row) - 1;
                        if (rowIndex < 0)
                            ValidFormula = false;
                        CellAdresss addr = new CellAdresss(rowIndex, CellAdresss.GetAlphabetNum(column));
                        if (!Sheet.inRange(addr.row, addr.col))
                        {
                            ValidFormula = false;
                            ExprList.Clear();
                            break;
                        }
                        prevOperator = false;
                        ExprList.Add(new CellAddressExpr(addr));
                    }
                    else if (rawFormula[i] == '$')
                    {
                        prevOperator = false;
                        continue;
                    }
                    else
                    {
                        ValidFormula = false;
                        prevOperator = false;
                        ExprList.Clear();
                        break;
                    }
                }
            }
            catch
            {
                ValidFormula = false;
                ExprList.Clear();
            }
            if (rawFormula == "")
                ValidFormula = false;
            return ExprList;
        }

        bool validParantheses(string rawInput)
        {
            int count = 0;
            for (int i = 0; i < rawInput.Length; i++)
            {
                if (rawInput[i] == '(')
                    count++;
                else if (rawInput[i] == ')')
                    --count;
                if (count < 0)
                    return false;
            }
            return (count == 0);
        }

    }
}
