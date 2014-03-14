using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetApp
{
    
    abstract class Cell
    {
        #region Atrributes

        public enum CellType 
        {
            None,
            NumberCell ,
            FormulaCell , 
            TextCell,
            ErrorCell
        };
        public CellType cellType;
        public bool visited;
        public bool updated;
        
        #endregion

        #region Methods
        public Cell()
        {
            cellType = CellType.None;
            visited = updated = false;
        }
        abstract public double Evaluate();

        abstract public string Show();

        abstract public string ShowRaw();


        #endregion
    }
}
