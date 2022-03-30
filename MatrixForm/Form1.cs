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
        public Form1()
        {
            InitializeComponent();

            Matrix matrix = new Matrix(3, 3);
            Matrix matrix2 = new Matrix(3, 3);
            Matrix resultMatrix = new Matrix(3, 3);

            InitSizeGridView(dataGridView1, 50, 20);
            InitSizeGridView(dataGridView2, 50, 20);
            InitSizeGridView(dataGridView3, 50, 20);

            int _row = matrix.GetRowCount();
            
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
                    matrix.Mass[i, j] = (int)dataGridView1[i, j].Value;
                    //matrix[i, j] = int.Parse(dataGridView1.Rows[2]);
                }
                
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
