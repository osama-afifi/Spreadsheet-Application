using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetApp
{
    class CellAddress
    {
        public int row;
        public int col;
        public CellAddress(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
        public CellAddress(string alphaAddr)
        {
            CellAddress addr = GetCellIndex(alphaAddr);
            this.row = addr.row;
            this.col = addr.col;
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
        public static int GetAlphabetNum(string col) //zero based
        {
            int returnedCol = 0;
            for (int i = col.Length - 1; i >= 0; i--)
                returnedCol = returnedCol * 26 + (int)(col[i] - 'A');
            return returnedCol;
        }
        public static CellAddress GetCellIndex(string alphaAddr)
        {
            string column = "";
            int i = 0;
            while (i < alphaAddr.Length && char.IsLetter(alphaAddr[i]))
            {
                column += alphaAddr[i];
                ++i;
            }
            int row = 0;
            while (i < alphaAddr.Length && char.IsDigit(alphaAddr[i]))
            {
                row *= 10;
                row += (alphaAddr[i] - '0');
                ++i;
            }
            return new CellAddress(row, GetAlphabetNum(column));
        }

    }
}
