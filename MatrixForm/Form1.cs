/*TODO
 Авторазмер окон GridView +
 Реализация MVC +
 Перенести SetDataGridViewSize в другое место +
 Сделать TextAlign В DGV +
 Ограничить DGV до 6х6 +
 Дописать кнопки +
 Пофиксить баг с ошибкой со второй матрицей +
 Решить проблему с вводом дробных чисел в textbox +
 Решить проблему с вводом нуля в textbox +
 Дописать кнопку для вычисления обратной матрицы

 */

/*
 Жизненный цикл ввода чисел в TextBox
    TextBox ->  KeyPress    ->  InitMatrixSize
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

        Controller _controller;

        private int _matrixColumns, _matrixRows;
        private int _matrix2Columns, _matrix2Rows;
        private bool _matrix2Created = false; 

        public Form1()
        {
            InitializeComponent();

            Model model = new Model();

            _controller = new Controller(model);

            model.ModelMsgEvent += UpdateWarningLabel;
            model.NotifResult += FillDataGridView;
        }

        /*-------------------------МЕСТО ДЛЯ КНОПОК---------------------------*/

        //Кнопка "Создать"
        private void button3_Click(object sender, EventArgs e)
        {
            if (initColumnsTextBox.Text != "" && initRowsTextBox.Text != ""
                && initColumnsTextBox.Text != "0" && initRowsTextBox.Text != "0")
            {
                //Проверяет введенный текст на ограничение по размеру
                if (Convert.ToInt32(initColumnsTextBox.Text) <= 6 && Convert.ToInt32(initRowsTextBox.Text) <= 6)
                {
                    _matrix = new Matrix(_matrixRows, _matrixColumns);

                    SetColumnRowDataGrid(dataGridView1, _matrixRows, _matrixColumns);
                    InitFullGridView(dataGridView1);

                    dataGridView1.Visible = true;
                    matrixInverseButton.Visible = true;
                    matrixMultyButton.Visible = true;

                    label3.Text = "Первая матрица создана";
                }
                else
                {
                    FillLabelWithBorders();
                }
            }
            else
            {
                label3.Text = "Ошибка ввода";
            }
        }

        //Кнопка "Создать" для второй матрицы
        private void createSecondMatrixTextBox_Click(object sender, EventArgs e)
        {
            if (initColumns2TextBox.Text != "" && initRows2TextBox.Text != ""
                && initColumns2TextBox.Text != "0" && initRows2TextBox.Text != "0")
            {
                if (Convert.ToInt32(initColumns2TextBox.Text) <= 6 && Convert.ToInt32(initRows2TextBox.Text) <= 6)
                {
                    _matrix2 = new Matrix(_matrix2Rows, _matrix2Columns);

                    SetColumnRowDataGrid(dataGridView2, _matrix2Rows, _matrix2Columns);
                    InitFullGridView(dataGridView2);

                    _matrix2Created = true;

                    dataGridView2.Visible = true;
                    label3.Text = "Вторая матрица создана";
                }
                else
                {
                    FillLabelWithBorders();
                }
            }
            else
            {
                label3.Text = "Ошибка ввода";
            }
        }

        //Кнопка для вычисления умножения матриц
        private void matrixMultyButton_Click(object sender, EventArgs e)
        {
            if (_matrix2Created)
            {
                FillMatrix(_matrix, dataGridView1);
                FillMatrix(_matrix2, dataGridView2);

                SetColumnRowDataGrid(dataGridView3, _matrixRows, _matrix2Columns);
                InitFullGridView(dataGridView3);

                _controller.Calculate(ActionEnum.MultMatrix, _matrix, _matrix2);

                dataGridView3.Visible = true;
            }
            else
            {
                label3.Text = "Вторая матрица не создана";
            }
        }

        //Кнопка для вычисления обратной матрицы
        private void button1_Click(object sender, EventArgs e)
        {
            FillMatrix(_matrix, dataGridView1);

            SetColumnRowDataGrid(dataGridView3, _matrixRows, _matrixColumns);
            dataGridView3.Visible = true;
            label4.Visible = true;

            _controller.Calculate(ActionEnum.ReverseMatrix, _matrix, _matrix2);
        }


        /*-----------------------ТЕХНИЧЕСКИЙ БЛОК--------------------------*/

        //Метод, заполняющий некую матрицу данными из DataGridView
        private void FillMatrix(Matrix matrix, DataGridView dataGridView)
        {
            for (int j = 0; j < matrix.Mass.GetLength(0); j++)
            {
                for (int i = 0; i < matrix.Mass.GetLength(1); i++)
                {
                    try
                    {
                        matrix.Mass[j, i] = (float)Convert.ToDouble(dataGridView[i, j].Value); //индексация массива происходит по j, i, а не по i, j
                    }
                    catch (FormatException)
                    {
                        label3.Text = "Неправильный ввод данных; Дробные числа вводятся через запятую";
                    }
                }
            }
        }

        //Заполнение DataGridView массивом
        private void FillDataGridView(Matrix matrix)
        {
            int temp = matrix.Mass.GetLength(0); 
            int temp2 = matrix.Mass.GetLength(1);

            for (int i = 0; i < matrix.Mass.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.Mass.GetLength(1); j++)
                {
                    dataGridView3[i, j].Value = matrix.Mass[j, i];
                }
            }
        }

        private void FillLabelWithBorders()
        {
            label3.Text = "Нельзя создать матрицу больше чем 6х6";
        }

        /*------------------БЛОК ДЛЯ СТИЛЕЙ------------------------*/

        //Метод, обьявляющий размер строк и столбцов, а так же размер и стили окна DataGridView
        private void InitFullGridView(DataGridView dataGridView)
        {
            //Обьявление размера столбов и строк
            InitSizeGridView(dataGridView, 50, 20);

            //Обьявление стилей окна DataGridView
            SetDefaultStyleDataGridView(dataGridView);

            //Задает размер для окна
            dataGridView.Width = dataGridView.Columns[0].Width * dataGridView.Columns.Count + 4;
            dataGridView.Height = dataGridView.Rows[0].Height * dataGridView.Rows.Count + 4;

            //Текст по центру
            dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        //Метод определяющий размер окна DataGridView по количеству строк и размеру строк и столбцов
        private void SetDataGridViewSize(DataGridView dataGridView)
        {
            dataGridView.Width = dataGridView.Columns[0].Width * dataGridView.Columns.Count + 4;
            dataGridView.Height = dataGridView.Rows[0].Height * dataGridView.Rows.Count + 4;
        }

        //Метод, определяющий количество строк и столбцов по данным с кнопки Button3
        private void SetColumnRowDataGrid(DataGridView dataGridView, int rows, int columns)
        {
            dataGridView.RowCount = rows;
            dataGridView.ColumnCount = columns;
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
            //Делает окно прозрачным
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.BackgroundColor = Form1.DefaultBackColor;
        }

        /*------------------------БЛОК ДЛЯ TEXT BOX------------------------------*/

        private void initColumnsTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckInitMatrixSize(initColumnsTextBox, ref _matrixColumns);
        }

        private void initRowsTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckInitMatrixSize(initRowsTextBox, ref _matrixRows);
        }

        private void initColumns2TextBox_TextChanged(object sender, EventArgs e)
        {
            CheckInitMatrixSize(initColumns2TextBox, ref _matrix2Columns);
        }

        private void initRows2TextBox_TextChanged(object sender, EventArgs e)
        {
            CheckInitMatrixSize(initRows2TextBox, ref _matrix2Rows);
        }

        //Метод для обьявления размера матрицы. Возвращает булево _safe
        //Проблемный метод. Too bad!
        private void CheckInitMatrixSize(TextBox textBox, ref int value)
        {
            try
            {
                value = int.Parse(textBox.Text);
            }
            catch (FormatException) { }
        }

        /*------------------------БЛОК ДЛЯ ДЕЛЕГАТОВ-------------------------*/
        private void UpdateWarningLabel(string message)
        {
            label3.Text = message;
        }

        private void initColumnsTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && !(number == '\b'))
            {
                e.Handled = true;
            }
        }

        /*------------------------МЕСТО ДЛЯ МУСОРА-------------------------*/
        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
