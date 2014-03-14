using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpreadsheetApp
{
    static class Sheet
    {
        #region attributes
        public static Cell[,] dataMatrix;
        public static string[,] outputMatrix;
        public static List<CellAdresss> formulaList;
        //static Sheet instance;

        #endregion

        #region methods

        public static void Intialize(int initalrows = 200, int intialCols = 100)
        {
            // Each Sheet created with following sizes initially
            dataMatrix = new Cell[initalrows, intialCols];
            outputMatrix = new string[initalrows, intialCols];
            formulaList = new List<CellAdresss>();
            InfixOperatorExpr.initializePrecedence();
            //   Expand();
        }

        static void Expand(bool rows = true) // Expand If Rows/Columns become near limits
        {
            int newRows = getNumRows();
            int newCols = getNumCols();
            // The Dimension to expand
            if (rows)
                newRows *= 2;
            else
                newCols *= 2;

            Cell[,] tempMatrix = new Cell[newRows, newCols];
            for (int i = 0; i < getNumRows(); i++)
                for (int j = 0; j < getNumCols(); j++)
                    tempMatrix[i, j] = dataMatrix[i, j];
            dataMatrix = tempMatrix;
        }
        public static int getNumRows()
        {
            return dataMatrix.GetLength(0);
        }
        public static int getNumCols()
        {
            return dataMatrix.GetLength(1);
        }

        public static bool inRange(int maxRow, int maxCol)
        {
            return (maxRow >= 0 && maxRow < getNumRows() && maxCol >= 0 && maxCol < getNumCols());
        }
        public static void Clear()
        {
            Intialize();
        }
        public static Cell GetCell(int row, int col)
        {
            return dataMatrix[row, col];
        }
        public static Cell SetCell(int row, int col, Cell c)
        {
            return dataMatrix[row, col] = c;
        }
        public static Cell insertCell(string rawData, CellAdresss addr)
        {
            Parser p = new Parser();
            List<Expression> exprList = p.ParseFormula(rawData);
            double parsedDouble = 0;
            if (p.ValidFormula)
            {
                FormulaCell Cell = new FormulaCell(exprList);
                dataMatrix[addr.row, addr.col] = Cell;
                if (Cell.hasDependencyLoop()) // has dependency loop
                    dataMatrix[addr.row, addr.col] = new ErrorCell(ErrorCell.ErrorType.DependencyLoop);
                else if (Cell.hasErrorType()) // depends on an Invalid Type
                    dataMatrix[addr.row, addr.col] = new ErrorCell(ErrorCell.ErrorType.InvalidType);
                else
                {
                    dataMatrix[addr.row, addr.col] = new FormulaCell(exprList);
                    dataMatrix[addr.row, addr.col].Evaluate();
                    formulaList.Add(addr);
                }
            }
            else if (double.TryParse(rawData, out parsedDouble))
            {
                dataMatrix[addr.row, addr.col] = new NumberCell(parsedDouble);
            }
            else
            {
                dataMatrix[addr.row, addr.col] = new TextCell(rawData);
            }
            RefreshDependancies();
            dataMatrix[addr.row, addr.col].Evaluate();
            outputMatrix[addr.row, addr.col] = dataMatrix[addr.row, addr.col].Show();
            return dataMatrix[addr.row, addr.col];
        }

        public static void RefreshDependancies()
        {
            HashSet<CellAdresss> visitedCells = new HashSet<CellAdresss>();
            for (int i = 0; i < formulaList.Count; i++)
            {
                Cell c = Sheet.GetCell(formulaList[i].row, formulaList[i].col);
                if (c is FormulaCell)
                {
                    RefreshDependanciesHelper(formulaList[i], ref  visitedCells);
                }
                else
                {
                    // Cleaning Up Automatically Non Formula Cells in List
                    formulaList.Remove(formulaList[i]);
                }
            }

        }
        static void RefreshDependanciesHelper(CellAdresss curFormulaAddr, ref HashSet<CellAdresss> visitedCells)
        {
            if (visitedCells.Contains(curFormulaAddr))
                return;
            visitedCells.Add(curFormulaAddr);
            Cell c = Sheet.GetCell(curFormulaAddr.row, curFormulaAddr.col);
            if (c is FormulaCell)
            {
                FormulaCell curFormulaCell = (FormulaCell)c;
                foreach (Expression f in curFormulaCell.exprList)
                {
                    if (f is CellAddressExpr)
                    {
                        CellAddressExpr e = (CellAddressExpr)f;
                        Cell refCell = Sheet.dataMatrix[e.addr.row, e.addr.col];
                        if (refCell is FormulaCell)
                        {
                            FormulaCell fc = (FormulaCell)refCell;
                            RefreshDependanciesHelper(e.addr, ref visitedCells);

                        }
                        else if (refCell is TextCell)
                        {
                            dataMatrix[e.addr.row, e.addr.col] = new ErrorCell(ErrorCell.ErrorType.InvalidType);
                            outputMatrix[e.addr.row, e.addr.col] = dataMatrix[e.addr.row, e.addr.col].Show();
                        }
                    }
                }
                curFormulaCell.Evaluate();
                dataMatrix[curFormulaAddr.row, curFormulaAddr.col] = curFormulaCell;
                outputMatrix[curFormulaAddr.row, curFormulaAddr.col] = dataMatrix[curFormulaAddr.row, curFormulaAddr.col].Show();
            }
        }

        #endregion

    }
}
