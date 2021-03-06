﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetApp
{
    class CellMatrixAddress
    {
        CellAddress upperLeft;
        CellAddress lowerRight;
        public CellMatrixAddress(CellAddress upperLeft , CellAddress lowerRight)
        {
            this.upperLeft = upperLeft;
            this.lowerRight = lowerRight;
        }
        public static string GetColumnAlphabet(int columnNumber)
        {
            ++columnNumber;
            int temp = columnNumber;
            string returnedName = String.Empty;
            while (temp > 0)
            {
                int mod = (temp - 1) % 26;
                returnedName = Convert.ToChar((int)'A' + mod).ToString() + returnedName;
                temp = (int)((temp - mod) / 26);
            }
            return returnedName;
        }
        public static int GetAlphabetNum(string row) //zero based
        {
            int returnedCol = 0;
            for (int i = row.Length - 1; i >= 0; i--)
                returnedCol = returnedCol * 26 + (int)(row[i] - 'A');
            return returnedCol;
        }
    }
}
