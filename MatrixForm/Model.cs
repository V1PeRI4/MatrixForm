/*TODO

матрица 4 на 4 и больше обратная.
возможно сделать остальные действия с матрицами.


где-то небольшая ошибка в вычислениях 4 на4 матрицы обратной хер его знает
 */

using System.Collections.Generic;

namespace MatrixForm
{
    public class Model
    {
        /*-------------------------ДЕЛЕГАТЫ---------------------------*/


        // Для сообщение о действии, ошибки и тд (UpdateWarningLabel)
        public delegate void ModelMsg(string message);
        public event ModelMsg ModelMsgEvent;

        // Для передачи в FillDataGridView матрицы
        public delegate void ResultDelegate(Matrix resultMatrix);
        public event ResultDelegate NotifResult;



        /*-------------------------ОСНОВНЫЕ МЕТОДЫ---------------------------*/


        /*---Метод умножения двух матриц---*/
        public void MultMatrix(Matrix matrix1, Matrix matrix2)
        {
            Matrix resultMatrix = new Matrix(matrix1.RowCount, matrix2.ColumnCount);

            if (matrix2.ColumnCount != matrix1.RowCount)
                ModelMsgEvent.Invoke("Количество столбцов первой матрицы не равно количеству строк второй матрицы.");

            else
            {
                for (int i = 0; i < matrix1.RowCount; i++)
                {
                    for (int j = 0; j < matrix2.ColumnCount; j++)
                    {
                        for (int k = 0; k < matrix1.ColumnCount; k++)
                            resultMatrix.Mass[i, j] += matrix1.Mass[i, k] * matrix2.Mass[k, j];
                    }
                }
                NotifResult.Invoke(resultMatrix);
            }

        }


        /*---Метод для вычисления обратной матрицы---*/
        public void ReverseMatrix(Matrix matrix1)
        {
            bool working= false; // Для условия остановки работы 
            double det = 0;
            det = CalculateDeterminant(matrix1, matrix1.ColumnCount, det); // Вычисление определителя матрицы

            CheckAllError(matrix1, det, ref working); // Проверка условий существования обратной матрицы
            if (working)
            {
                matrix1 = AlliedMatrix(matrix1, matrix1.ColumnCount, det); // Союзная матрица
                matrix1 = CreateTransposeMatrix(matrix1, matrix1.ColumnCount); // Транспонирование матрицы
                matrix1 = MultByNum(matrix1, det); // Умножение на число
                matrix1 = NumberRoundingMatrix(matrix1); // Округление результата

                NotifResult.Invoke(matrix1);
            }
            else return;

        }



        /*-------------------------БЛОК ВСПОМОГАТЕЛЬНЫХ МЕТОДОВ ДЛЯ ОБРАТНОЙ МАТРИЦЫ---------------------------*/


        /*---Вычисление опеределителя матрицы---*/
        public double CalculateDeterminant(Matrix matrix1, int N, double determinant)
        {
            if (N > 2)
            {
                for (int j = 0; j < N; j++)
                {
                    if (j % 2 == 0) determinant += matrix1.Mass[0, j] * CalculateDeterminant(DecreaseMatrix(matrix1, 0, j), N - 1, determinant); 
                    else determinant -= matrix1.Mass[0, j] * CalculateDeterminant(DecreaseMatrix(matrix1, 0, j), N - 1, determinant);
                }
            }
            else if (N == 2)
                determinant = matrix1.Mass[0, 0] * matrix1.Mass[1, 1] - matrix1.Mass[1, 0] * matrix1.Mass[0, 1];

            return (determinant);
        }


        /*---Нахождение союзной матрицы---*/
        public Matrix AlliedMatrix(Matrix matrix1, int N, double det)
        {
            double algebraicAdd = 0;

            Matrix tempMatrix = matrix1;
            Matrix alliedMatrix = new Matrix(N, N);
            Matrix reverseIndexMatrix = ReverseIndexMatrix(matrix1);

            if (N > 2)
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        algebraicAdd = 0;

                        if ((i + j) % 2 == 0) algebraicAdd += CalculateDeterminant(DecreaseMatrix(matrix1, i, j), N - 1, det);
                        else algebraicAdd -= CalculateDeterminant(DecreaseMatrix(matrix1, i, j), N - 1, det); 

                        alliedMatrix.Mass[i, j] = algebraicAdd;
                    }
                }
            }
            else if (N == 2)
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        if (tempMatrix.Mass[i, j] != 0)
                            if ((i + j) % 2 == 0) alliedMatrix.Mass[i, j] += reverseIndexMatrix.Mass[i, j];
                            else alliedMatrix.Mass[i, j] -= reverseIndexMatrix.Mass[i, j];
                        else alliedMatrix.Mass[i, j] = 0;
                    }
                }
            }

            return alliedMatrix;
        }


        /*---Методы по удалению строк и столбцов матрицы (для вычисение определителя)---*/
        // №1  Основной, содержащий в себе вызовы 2 и 3
        public Matrix DecreaseMatrix(Matrix matrix, int DelRow, int DelColumn)
        {
            Matrix decreaseMatrix = matrix;
            decreaseMatrix = DelRowMatrix(ref decreaseMatrix, DelRow);
            decreaseMatrix = DelColumnMatrix(ref decreaseMatrix, DelColumn);
            return decreaseMatrix;
        }
        // №2
        public Matrix DelRowMatrix(ref Matrix matrix, int DelRow)
        {
            Matrix delRowMatrix = new Matrix(matrix.RowCount - 1, matrix.ColumnCount);

            for (int i = 0; i < DelRow; i++)
            {
                for (int j = 0; j < delRowMatrix.ColumnCount; j++)
                    delRowMatrix.Mass[i, j] = matrix.Mass[i, j];
            }

            for (int i = DelRow; i < delRowMatrix.RowCount; i++)
            {
                for (int j = 0; j < delRowMatrix.ColumnCount; j++)
                    delRowMatrix.Mass[i, j] = matrix.Mass[i + 1, j];
            }

            return delRowMatrix;
        }
        // №3
        public Matrix DelColumnMatrix(ref Matrix matrix, int DelColumn)
        {
            Matrix delColumnMatrix = new Matrix(matrix.RowCount, matrix.ColumnCount - 1);

            for (int i = 0; i < delColumnMatrix.RowCount; i++)
            {
                for (int j = 0; j < DelColumn; j++)
                    delColumnMatrix.Mass[i, j] = matrix.Mass[i, j];
            }

            for (int i = 0; i < delColumnMatrix.RowCount; i++)
            {
                for (int j = DelColumn; j < delColumnMatrix.RowCount; j++)
                    delColumnMatrix.Mass[i, j] = matrix.Mass[i, j + 1];
            }

            return delColumnMatrix;
        }


        /*---Создание транспонированной матрицы---*/
        public Matrix CreateTransposeMatrix(Matrix matrix, int N)
        {
            Matrix transp = new Matrix(N, N);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    transp.Mass[i, j] = matrix.Mass[i, j];

                    if (i != j)
                        transp.Mass[i, j] = matrix.Mass[j, i];
                }
            }

            return transp;
        }


        /*---Переворот матицы с конца в начало, т.е обратное индексирование---*/
        public Matrix ReverseIndexMatrix(Matrix matrix)
        {
            Matrix reverseIndexMatrix = new Matrix(matrix.RowCount, matrix.ColumnCount);
            int tempNum = 0;

            List<double> tempList = new List<double>();

            for (int i = 0; i < matrix.RowCount; i++)
            {
                for (int j = 0; j < matrix.ColumnCount; j++)
                    tempList.Add(matrix.Mass[i, j]);
            }

            tempList.Reverse();

            for (int i = 0; i < matrix.RowCount; i++)
            {
                for (int j = 0; j < matrix.ColumnCount; j++)
                {
                    reverseIndexMatrix.Mass[i, j] = tempList[tempNum];
                    tempNum++;
                }
            }
            return reverseIndexMatrix;
        }


        /*---Умножение результата на число, последнее действие в методе обратной матрицы---*/
        public Matrix MultByNum(Matrix matrix1, double det)
        {
            Matrix resultMatrix = matrix1;
            double firstAction = 1 / det;

            for (int i = 0; i < matrix1.RowCount; i++)
            {
                for (int j = 0; j < matrix1.ColumnCount; j++)
                    resultMatrix.Mass[i, j] *= firstAction;
            }
            return resultMatrix;

        }


        /*---Метод для округления результа---*/
        public Matrix NumberRoundingMatrix(Matrix matrix)
        {
            for (int i = 0; i < matrix.RowCount; i++)
            {
                for (int j = 0; j < matrix.ColumnCount; j++)
                    matrix.Mass[i, j] -= (matrix.Mass[i, j] % 0.00001);
            }
            return matrix;

        }



        /*-------------------------БЛОК ПРОВЕРКИ НЕКОГО РЕЗУЛЬТАТА---------------------------*/


        /*---Обьединение всех условий работы---*/
        private void CheckAllError(Matrix matrix1, double det, ref bool working)
        {
            if (CheckColumnAndRowEquality(matrix1)) // Cтолбцы не равны
                ModelMsgEvent.Invoke("Количество столбцов и строк равны");

            else if (IsIdentityMatrix(matrix1)) // Если матрица еденичная
            {
                int one = 1;
                matrix1.Mass[0, 0] = one / matrix1.Mass[0, 0];
                NotifResult.Invoke(matrix1);
                ModelMsgEvent.Invoke("Это еденичная матрица");
            }
            else if (det == 0) // Проверка на нулевой определитель
                ModelMsgEvent.Invoke("Определитель - ноль. Обратной матрицы не существует");
            else
            {
                working = true;
                return;
            }
        }


        /*---Проверка равенства строк и столбцов матрицы для метода обратной матрицы---*/
        private bool CheckColumnAndRowEquality(Matrix matrix1)
        {
            return matrix1.ColumnCount != matrix1.RowCount;
        }


        /*---Если матрица еденичная, то выводим еденицу деленную на единственное число в матрице---*/
        private bool IsIdentityMatrix(Matrix matrix1)
        {
            return matrix1.ColumnCount == 1 && matrix1.RowCount == 1;
        }


        


    }
}
