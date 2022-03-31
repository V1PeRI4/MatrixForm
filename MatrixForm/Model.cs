using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixForm
{
    public class Model
    {
        public delegate void ResultDelegate(Matrix resultMatrix);
        public event ResultDelegate NotifResult;


        /// <summary>
        /// Умножение матриц
        /// </summary>
        /// 
        public void MultMatrix(Matrix matrix1, Matrix matrix2)
        {

            Matrix resultMatrix = new Matrix(matrix1.RowCount, matrix2.ColumnCount);

            if (matrix2.ColumnCount != matrix1.RowCount)
            {
                throw new Exception("Умножение невозможно! Количество столбцов первой матрицы не равно количеству строк второй матрицы.");
            }

            for (int i = 0; i < matrix1.RowCount; i++)
            {
                for (int j = 0; j < matrix2.ColumnCount; j++)
                {
                    resultMatrix.Mass[i, j] = 0;

                    for (int k = 0; k < matrix1.ColumnCount; k++)
                    {
                        resultMatrix.Mass[i, j] += matrix1.Mass[i, k] * matrix2.Mass[k, j];
                    }
                }
            }

            NotifResult.Invoke(resultMatrix);
        }

        ///////переделанный под тест
        ///
        //public Matrix MultMatrix2(Matrix matrix1, Matrix matrix2)
        //{

        //    Matrix resultMatrix = new Matrix(matrix1.RowCount, matrix2.ColumnCount);

        //    if (matrix2.ColumnCount != matrix1.RowCount)
        //    {
        //        throw new Exception("Умножение невозможно! Количество столбцов первой матрицы не равно количеству строк второй матрицы.");
        //    }

        //    for (int i = 0; i < matrix1.RowCount; i++)
        //    {
        //        for (int j = 0; j < matrix2.ColumnCount; j++)
        //        {
        //            resultMatrix.Mass[i, j] = 0;

        //            for (int k = 0; k < matrix1.ColumnCount; k++)
        //            {
        //                resultMatrix.Mass[i, j] += matrix1.Mass[i, k] * matrix2.Mass[k, j];
        //            }
        //        }
        //    }

        //    return resultMatrix;
        //}

       

        /// <summary>
        /// Нахождение обратной матрицы
        /// </summary>
        /// 

       
        public void ReverseMatrix(Matrix matrix1)
        {
            if (matrix1.ColumnCount != matrix1.RowCount)
                throw new Exception("Нахождение невозможно! Количество столбцов не равно количеству строк.");

            int N = matrix1.ColumnCount;

            float det = 0;
            det = CalculateDeterminant(ref matrix1, N, det);

            if (det <= 0)
            {     
                Console.WriteLine("Определитель меньше нуля, программа остановлена");
                return;
            }

            Matrix resultMatrix = new Matrix(N, N);

            //ProcessFunctionOverData(  (i, j)   =>  {result[i, j] = ((i + j) % 2 == 1 ? -1 : 1) * CalculateMinor(i, j) / determinant;}  );

            resultMatrix = CreateTransposeMatrix(resultMatrix, N);

            //resultMatrix = (1 / determinant) * resultMatrix; Последнее действие


            NotifResult.Invoke(resultMatrix);
        }

        public float CalculateDeterminant(ref Matrix matrix1, int N, float determinant)
        {
            if (N != 2)
            {
                for (int i = 0; i < N; i++)
                {
                    if (i % 2 == 0) determinant += matrix1.Mass[0, i] * CalculateDeterminant(ref matrix1, N - 1, determinant);
                    else determinant -= matrix1.Mass[0, i] * CalculateDeterminant(ref matrix1, N - 1, determinant);
                }
            }
            else if (N == 2)
            {
                determinant = matrix1.Mass[0, 0] * matrix1.Mass[0, 1] - matrix1.Mass[1, 0] * matrix1.Mass[1, 1];
            }
            return (determinant);
        }

        //public void ProcessFunctionOverData(Action<int, int> func)
        //{
        //    for (var i = 0; i < this.M; i++)
        //    {
        //        for (var j = 0; j < this.N; j++)
        //        {
        //            func(i, j);
        //        }
        //    }
        //}


        public Matrix AlliedMatrix(Matrix matrix1, int N)
        {
            float algebraicAdd = 0;

            Matrix alliedMatrix = new Matrix(N, N);

            if (N != 2)
            {
                for (int i = 0; i < N; i++)
                {
                    if (i % 2 == 0) algebraicAdd += matrix1.Mass[0, i] * CalculateDeterminant(ref matrix1, N - 1, algebraicAdd);
                    else algebraicAdd -= matrix1.Mass[0, i] * CalculateDeterminant(ref matrix1, N - 1, algebraicAdd);

                    alliedMatrix.Mass[]
                }
            }
            else if (N == 2)
            {
                algebraicAdd = matrix1.Mass[0, 0] * matrix1.Mass[0, 1] - matrix1.Mass[1, 0] * matrix1.Mass[1, 1];
            }
            return (algebraicAdd);
        }

        public Matrix CreateTransposeMatrix(Matrix matrix, int N)
        {
            Matrix transp = new Matrix(N, N);
           
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                    transp.Mass[i, j] = matrix.Mass[j, i];
            }
            return transp;
        }

    }
}
