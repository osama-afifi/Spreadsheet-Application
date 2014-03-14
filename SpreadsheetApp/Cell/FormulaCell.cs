using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetApp
{
    class FormulaCell : Cell
    {
        public double cacheVal;
        public List<Expression> exprList;
        public FormulaCell(List<Expression> exprList)
        {
            this.exprList = exprList;
            visited = updated = false;
        }
        override public string Show()
        {
            return cacheVal.ToString();
        }
        override public string ShowRaw()
        {
            return printExpr();
        }
        string printExpr()
        {
            string expr = "=";
            foreach (Expression e in exprList)
            {
                if (e is CellAddressExpr)
                {
                    int rowIndex = e.addr.row + 1;
                    string colIndex = CellAdresss.GetColumnAlphabet(e.addr.col);
                    expr += colIndex + rowIndex.ToString();
                }
                else if (e is LiteralExpr)
                    expr += e.value.ToString();
                else if (e is InfixOperatorExpr)
                    expr += e.operatorSign;
            
            }
            return expr;
        }
        public override double Evaluate() // Evauluate the Formula
        {
            Stack<Expression> oprStack = new Stack<Expression>();
            Stack<double> valueStack = new Stack<double>();
            try
            {

                foreach (Expression expr in exprList)
                {
                    if (expr is LiteralExpr)
                    {
                        Expression e = expr;
                        valueStack.Push(e.value);
                    }

                    else if (expr is CellAddressExpr)
                    {

                        try
                        {
                            if (Sheet.GetCell(expr.addr.row, expr.addr.col) is NumberCell)
                            {
                                double val = Sheet.GetCell(expr.addr.row, expr.addr.col).Evaluate();
                                valueStack.Push(val);
                            }
                            else if (Sheet.GetCell(expr.addr.row, expr.addr.col) is FormulaCell)
                            {
                                double val = Sheet.GetCell(expr.addr.row, expr.addr.col).Evaluate();
                                Sheet.outputMatrix[expr.addr.row, expr.addr.col] = Sheet.GetCell(expr.addr.row, expr.addr.col).Show();
                                valueStack.Push(val);
                            }
                            else if (Sheet.GetCell(expr.addr.row, expr.addr.col) is TextCell)
                            {
                                Sheet.dataMatrix[expr.addr.row, expr.addr.col] = new ErrorCell(ErrorCell.ErrorType.InvalidType);
                                Sheet.outputMatrix[expr.addr.row, expr.addr.col] = Sheet.GetCell(expr.addr.row, expr.addr.col).Show();

                                // throw new Exception();
                            }
                            else
                            {
                                valueStack.Push(0);
                            }

                        }
                        catch (Exception exc)
                        {
                            if (Sheet.inRange(expr.addr.row, expr.addr.col))
                            {
                                Cell referedCell = Sheet.GetCell(expr.addr.row, expr.addr.col);
                                Sheet.dataMatrix[expr.addr.row, expr.addr.col] = new ErrorCell(ErrorCell.ErrorType.General);
                                Sheet.outputMatrix[expr.addr.row, expr.addr.col] = Sheet.GetCell(expr.addr.row, expr.addr.col).Show();
                            }
                        }
                    }
                    else if (expr.exprType == Expression.ExprType.InfixOperator)
                    {
                        try
                        {
                            Expression opr = expr;

                            if (oprStack.Count == 0 || InfixOperatorExpr.precede(opr.operatorSign, oprStack.Peek().operatorSign))
                            {
                                oprStack.Push(opr);
                            }
                            else if (opr.operatorSign == ")")
                            {
                                while (oprStack.Peek().operatorSign != "(")
                                {
                                    Expression top = oprStack.Pop();
                                    double val2 = valueStack.Pop();
                                    double val1 = valueStack.Pop();
                                    double res = InfixOperatorExpr.binaryOperation(val1, val2, top.operatorSign);
                                    valueStack.Push(res);
                                }
                                if (oprStack.Peek().operatorSign == "(")
                                    oprStack.Pop();
                            }
                            else
                            {
                                while (oprStack.Count > 0 && !InfixOperatorExpr.precede(opr.operatorSign, oprStack.Peek().operatorSign))
                                {
                                    Expression top = oprStack.Pop();
                                    double val2 = valueStack.Pop();
                                    double val1 = valueStack.Pop();
                                    double res = InfixOperatorExpr.binaryOperation(val1, val2, top.operatorSign);
                                    valueStack.Push(res);
                                }
                                oprStack.Push(opr);
                            }
                        }
                        catch
                        {
                            int x;
                        }
                    }


                }

                while (oprStack.Count > 0)
                {
                    string opr = oprStack.Pop().operatorSign;
                    double val2 = valueStack.Pop();
                    double val1 = valueStack.Pop();
                    double res = InfixOperatorExpr.binaryOperation(val1, val2, opr);
                    valueStack.Push(res);
                }
            }


            catch
            {

                int x;
            }

            cacheVal = (valueStack.Count == 1) ? valueStack.Pop() : 0;
            return cacheVal;
        }

        public bool hasDependencyLoop()
        {
            HashSet<FormulaCell> visitedCells = new HashSet<FormulaCell>();
            visitedCells.Clear();
            return hasDependencyLoopHelper(this, visitedCells, this);
        }
        bool hasDependencyLoopHelper(FormulaCell curFormulaCell, HashSet<FormulaCell> visitedCells, FormulaCell startCell)
        {
            if (visitedCells.Contains(curFormulaCell))
                return (curFormulaCell == startCell);
            visitedCells.Add(curFormulaCell);
            bool result = false;
            foreach (Expression f in curFormulaCell.exprList)
            {
                if (f.exprType == Expression.ExprType.Address)
                {
                    CellAddressExpr e = (CellAddressExpr)f;
                    Cell refCell = Sheet.dataMatrix[e.addr.row, e.addr.col];
                    if (refCell is FormulaCell)
                    {
                        result |= hasDependencyLoopHelper((FormulaCell)refCell, visitedCells, startCell);
                    }

                }
            }
            return result;
        }

        public bool hasErrorType()
        {
            HashSet<FormulaCell> visitedCells = new HashSet<FormulaCell>();
            visitedCells.Clear();
            return hasErrorTypeHelper(this, visitedCells, this);
        }
        bool hasErrorTypeHelper(FormulaCell curFormulaCell, HashSet<FormulaCell> visitedCells, FormulaCell startCell)
        {
            if (!(curFormulaCell is FormulaCell))
                return true;
            if (visitedCells.Contains(curFormulaCell))
                return false;
            visitedCells.Add(curFormulaCell);
            bool result = false;
            foreach (Expression f in curFormulaCell.exprList)
            {
                if (f.exprType == Expression.ExprType.Address)
                {
                    CellAddressExpr e = (CellAddressExpr)f;
                    Cell refCell = Sheet.dataMatrix[e.addr.row, e.addr.col];
                    if (refCell is FormulaCell)
                    {
                        result |= hasErrorTypeHelper((FormulaCell)refCell, visitedCells, startCell);
                    }
                    else if (refCell is TextCell)
                        return true;
                }
            }
            return result;
        }

    }


}
