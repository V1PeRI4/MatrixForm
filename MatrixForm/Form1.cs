﻿/*TODO
 Авторазмер окон GridView +
 Реализация MVC -
 Дописать кнопки

Доделать метод вычисление обратной матрицы - выдает нули
 */

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
        Matrix _matrix;
        Matrix _matrix2;
        Matrix _resultMatrix;

        Controller _controller;

        private int _matrixColumns, _matrixRows;
        private bool _safe; 
        public Form1()
        {
            InitializeComponent();

            Model model = new Model();

            _controller = new Controller(model);

            model.ModelMsgEvent += UpdateWarningLabel;
            model.NotifResult += FillDataGridView;
        }

        /*-----------------------ТЕХНИЧЕСКИЙ БЛОК--------------------------*/

        //Метод, заполняющий некую матрицу данными из DataGridView
        private void FillMatrix(Matrix matrix, DataGridView dataGridView)
        {
            int x = matrix.Mass.GetLength(0);
            int y = matrix.Mass.GetLength(1);

            if (_matrix2.ColumnCount != _matrix.RowCount)
            {
                x--;
                y--;
            }

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    matrix.Mass[i, j] = Convert.ToInt32(dataGridView[i, j].Value);
                }
            }
        }

        //Заполнение DataGridView массивом
        private void FillDataGridView(Matrix matrix)
        {
            for (int i = 0; i < matrix.Mass.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.Mass.GetLength(1); j++)
                {
                    dataGridView3[i, j].Value = matrix.Mass[i, j];
                }
            }
        }

        /*------------------БЛОК ДЛЯ СТИЛЕЙ------------------------*/

        //Метод, обьявляющий размер строк и столбцов, а так же размер и стили окна DataGridView
        private void InitFullGridView()
        {
            //Обьявление количества столбов и строк
            SetColumnRowDataGrid(dataGridView1);
            SetColumnRowDataGrid(dataGridView2);
            SetColumnRowDataGrid(dataGridView3);

            //Обьявление размера столбов и строк
            InitSizeGridView(dataGridView1, 50, 20);
            InitSizeGridView(dataGridView2, 50, 20);
            InitSizeGridView(dataGridView3, 50, 20);

            //Обьявление ширины и высоты окна DataGridView
            SetDataGridViewSize(dataGridView1);
            SetDataGridViewSize(dataGridView2);
            SetDataGridViewSize(dataGridView3);

            //Обьявление стилей окна DataGridView
            SetDefaultStyleDataGridView(dataGridView1);
            SetDefaultStyleDataGridView(dataGridView2);
            SetDefaultStyleDataGridView(dataGridView3);
        }

        //Метод определяющий размер окна DataGridView по количеству строк и размеру строк и столбцов
        private void SetDataGridViewSize(DataGridView dataGridView)
        {
            dataGridView.Width = dataGridView.Columns[0].Width * dataGridView.Columns.Count + 4;
            dataGridView.Height = dataGridView.Rows[0].Height * dataGridView.Rows.Count + 4;
        }

        //Метод, определяющий количество строк и столбцов по данным с кнопки Button3
        private void SetColumnRowDataGrid(DataGridView dataGridView)
        {
            dataGridView.RowCount = _matrixRows;
            dataGridView.ColumnCount = _matrixColumns;
        }

        //Метод, обьявляющий размер строк и столбцов DataGridView
        private void InitSizeGridView(DataGridView dataGrid, int width, int height)
        {
            for (int i = 0; i < dataGrid.RowCount; i++)
            {
                for (int j = 0; j < dataGrid.ColumnCount; j++)
                {

                    dataGrid.Columns[j].Width = width;
                    dataGrid.Rows[i].Height = height;
                }
            }
        }

        //Обьявление стилей окна
        private void SetDefaultStyleDataGridView(DataGridView dataGridView)
        {
            //Делает всё прозрачным
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.BackgroundColor = Form1.DefaultBackColor;
        }

        /*------------------------БЛОК ДЛЯ TEXT BOX------------------------------*/

        private void initColumnsTextBox_TextChanged(object sender, EventArgs e)
        {
            InitMatrixSize(initColumnsTextBox, ref _matrixColumns);
        }

        private void initRowsTextBox_TextChanged(object sender, EventArgs e)
        {
            InitMatrixSize(initRowsTextBox, ref _matrixRows);
        }

        //Метод для обьявления размера матрицы. Возвращает булево _safe
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

        /*-------------------------МЕСТО ДЛЯ КНОПОК---------------------------*/

        //Кнопка "Создать"
        private void button3_Click(object sender, EventArgs e)
        {
            if (_safe)
            {
                _matrix = new Matrix(_matrixRows, _matrixColumns);
                _matrix2 = new Matrix(_matrixRows, _matrixColumns);
                _resultMatrix = new Matrix(_matrixRows, _matrixColumns);

                label3.Text = "Матрица создана";

                InitFullGridView();

                dataGridView1.Visible = true;
                dataGridView2.Visible = true;
                matrixInverseButton.Visible = true;
                matrixMultyButton.Visible = true;
                labelFillMatrixPls.Visible = true;
            }
            else
            {
                label3.Text = "Ошибка";
            }
        }

        //Кнопка для вычисления умножения матриц
        private void matrixMultyButton_Click(object sender, EventArgs e)
        {
                FillMatrix(_matrix, dataGridView1);
            FillMatrix(_matrix2, dataGridView2);

            if (_matrix != null && _matrix2 != null)
            {
                dataGridView3.Visible       = true;

                _controller.Calculate(ActionEnum.MultMatrix, _matrix, _matrix2);
            }
        }

        //Кнопка для вычисления обратной матрицы
        private void button1_Click(object sender, EventArgs e)
        {
            if (_matrix != null)
            {
                FillMatrix(_matrix, dataGridView1);

                dataGridView3.Visible       = true;
                label4.Visible              = true;

                _controller.Calculate(ActionEnum.ReverseMatrix, _matrix, _matrix2);
            }
        }


        /*------------------------БЛОК ДЛЯ ДЕЛЕГАТОВ-------------------------*/
        private void UpdateWarningLabel(string message)
        {
            labelFillMatrixPls.Text = message;
        }

        /*------------------------МЕСТО ДЛЯ МУСОРА-------------------------*/
        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
/*            if ((sender as DataGridView).Rows.Count > 0)
            {
                SetDataGridViewSize(sender as DataGridView);
            }*/
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
