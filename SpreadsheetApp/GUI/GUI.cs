using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mommo;
using Mommo.Data;

namespace SpreadsheetApp
{
    public partial class GUI : Form
    {
 
        Mommo.Data.ArrayDataView MatrixBind;
        
        public GUI()
        {
            InitializeComponent();
        }
        private void GUI_Load(object sender, EventArgs e)
        {
            
          //  SheetDSWorker.RunWorkerAsync(currentSheet);
            Sheet.Intialize();
            MatrixBind = new Mommo.Data.ArrayDataView(Sheet.outputMatrix);
            dataGridView1.DataSource = MatrixBind;
            UpdateGrid();
            MessageBox.Show("Edit in the Spreadsheet by clicking on a Cell\n-Formula Example :    \"=B2+(C3/D4)+3\"\nCell Referring is Absolute", "Hint!");              
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void SheetDSWorker_DoWork(object sender, DoWorkEventArgs e)
        {
        }

        private void SheetDSWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void SheetDSWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show("Error: " + e.Error.Message);
             UpdateGrid();
        }

        private void UpdateGrid()
        {
           // MatrixBind = new Mommo.Data.ArrayDataView(currentSheet.outputMatrix);
           // dataGridView1.DataSource = MatrixBind;
            MatrixBind.Reset();
        }


        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {           
            UpdateGrid();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            Sheet.Clear();
            MatrixBind = new Mommo.Data.ArrayDataView(Sheet.outputMatrix);
            dataGridView1.DataSource = MatrixBind;
            UpdateGrid();
            MessageBox.Show("Sheet Cleared Successfully!" , "Cleared!"); 
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            CellAdresss addr = new CellAdresss(e.RowIndex,e.ColumnIndex);
            string cellText = dataGridView1[addr.col, addr.row].Value.ToString();
            FormulaBar.Text = cellText;
            Cell c = Sheet.insertCell(cellText, addr);
            FormulaBar.Text = c.ShowRaw();
            UpdateGrid();
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!Sheet.inRange(e.ColumnIndex, e.RowIndex))
                return;
            if (dataGridView1[e.ColumnIndex, e.RowIndex].Value != null)
            {
                if (Sheet.GetCell(e.RowIndex, e.ColumnIndex) is FormulaCell)
                    FormulaBar.Text = Sheet.GetCell(e.RowIndex, e.ColumnIndex).ShowRaw();
                else
                    FormulaBar.Text = (dataGridView1[e.ColumnIndex, e.RowIndex].Value).ToString();
            }
            else
                FormulaBar.Text = "";
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }



    }
}

