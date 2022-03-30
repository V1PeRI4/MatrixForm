using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatrixForm
{
    public partial class Form1 : Form
    {
        private int _matrixColumns, _matrixRows;
        private bool _safe;
        public Form1()
        {
            InitializeComponent();

            InitSizeGridView(dataGridView1, 50, 20);
            InitSizeGridView(dataGridView2, 50, 20);
            InitSizeGridView(dataGridView3, 50, 20);
        }

        private void InitSizeGridView(DataGridView dataGrid, int width, int height)
        {
            for (int i = 0; i < dataGrid.RowCount; i++)
            {
                for (int j = 0; j < dataGrid.ColumnCount; j++)
                {
                    dataGrid.Columns[j].Width = width;
                }
            }
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        public void FillMatrix(Matrix matrix)
        {
            for (int i = 0; i < matrix.Mass.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.Mass.GetLength(1); j++)
                {
                    matrix.Mass[i, j] = Convert.ToInt32(dataGridView1[i,j].Value);
                }
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_safe)
            {
                Matrix matrix = new Matrix(_matrixRows, _matrixColumns);
                Matrix matrix2 = new Matrix(_matrixRows, _matrixColumns);
                Matrix resultMatrix = new Matrix(_matrixRows, _matrixColumns);
            }
        }

        private void initColumnsTextBox_TextChanged(object sender, EventArgs e)
        {
            InitMatrixSize(initColumnsTextBox, ref _matrixColumns);
        }

        private void initRowsTextBox_TextChanged(object sender, EventArgs e)
        {
            InitMatrixSize(initRowsTextBox, ref _matrixRows);
        }

        private bool InitMatrixSize(TextBox textBox, ref int value)
        {
            _safe = true;

            try
            {
                value = int.Parse(textBox.Text);
            }
            catch (FormatException)
            {
                _safe = false;
            }

            return _safe;
        }
    }
}
