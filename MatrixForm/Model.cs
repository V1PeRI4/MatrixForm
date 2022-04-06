namespace MatrixForm
{
    public class Model
    {
        public delegate void ModelMsg(string message);
        public event ModelMsg ModelMsgEvent;

        public delegate void ResultDelegate(Matrix resultMatrix);
        public event ResultDelegate NotifResult;


        /// <summary>
        /// Умножение матриц
        /// </summary>
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
                        {
                            resultMatrix.Mass[i, j] += matrix1.Mass[i, k] * matrix2.Mass[k, j];
                        }
                    }
                }
                NotifResult.Invoke(resultMatrix);
            }
        }


        /// <summary>
        /// Нахождение обратной матрицы
        /// </summary>
        public void ReverseMatrix(Matrix matrix1)
        {

            if (matrix1.ColumnCount != matrix1.RowCount)
                ModelMsgEvent.Invoke("Количество столбцов не равно количеству строк");

            int N = matrix1.ColumnCount;

            float det = 0;
            det = CalculateDeterminant(matrix1, N, det);

            if (det == 0)
                ModelMsgEvent.Invoke("Определитель - ноль. Обратной матрицы не существует");

            matrix1 = AlliedMatrix(matrix1, N, det); ////// доделывай

            matrix1 = CreateTransposeMatrix(matrix1, N);

            float firstAction = 1 / det;

            matrix1 = MultByNum(matrix1, firstAction);

            NotifResult.Invoke(matrix1);
        }



        public float CalculateDeterminant(Matrix matrix1, int N, float determinant)
        {

            if (N != 2)
            {
                for (int i = 0; i < N; i++)
                {
                    //if (i % 2 == 0) determinant += matrix1.Mass[0, i] * CalculateDeterminant(matrix1, N - 1, determinant);
                    //else determinant -= matrix1.Mass[0, i] * CalculateDeterminant(matrix1, N - 1, determinant);
                }
            }
            else if (N == 2)
                determinant = matrix1.Mass[0, 0] * matrix1.Mass[1, 1] - matrix1.Mass[1, 0] * matrix1.Mass[0, 1];

            return (determinant);
        }

        public Matrix AlliedMatrix(Matrix matrix1, int N, float det)
        {
            float algebraicAdd = 0;

            Matrix tempMatrix = matrix1;
            Matrix alliedMatrix = new Matrix(N, N);

            if (N > 2)
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        algebraicAdd = 0;
                        if (i % 2 == 0) algebraicAdd += matrix1.Mass[i, j] * CalculateDeterminant(DecreaseMatrix(matrix1, i, j), N - 1, det);
                        else algebraicAdd -= matrix1.Mass[i, j] * CalculateDeterminant(DecreaseMatrix(matrix1, i, j), N - 1, det); //заметь как уменньшаютсчя матрицы

                        alliedMatrix.Mass[i, j] = algebraicAdd;
                    }
                }
                /////доделать перебрать логику когда большая матрица а когда маленькая
                
            }
            else if (N == 2)
            {
                    for (int i = 0; i < N; i++)
                    {
                        for (int j = 0; j < N; j++)
                        {
                        if (tempMatrix.Mass[i, j] != 0)
                            if ((j % 2 == 0) || (i % 2 == 0)) alliedMatrix.Mass[i, j] += tempMatrix.Mass[i, j] ;
                            else alliedMatrix.Mass[i, j] -= tempMatrix.Mass[i, j] ; 
                        }
                    }

                }
                
            return alliedMatrix;
        }

        public Matrix MultByNum(Matrix matrix1, float num)
        {
            Matrix resultMatrix = matrix1;

            for (int i = 0; i < matrix1.RowCount; i++)
            {
                for (int j = 0; j < matrix1.ColumnCount; j++)
                    resultMatrix.Mass[i, j] *= num;
            }

            return resultMatrix;
        }

        public Matrix CreateTransposeMatrix(Matrix matrix, int N)
        {
            Matrix transp = matrix;

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                    transp.Mass[i, j] = matrix.Mass[j, i];
            }

            return transp;
        }


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


        public Matrix DelColumnMatrix(ref Matrix matrix, int DelColumn)
        {
            Matrix delColumnMatrix = new Matrix(matrix.RowCount - 1, matrix.ColumnCount);

            for (int i = 0; i < DelColumn; i++)
            {
                for (int j = 0; j < delColumnMatrix.ColumnCount; j++)
                    delColumnMatrix.Mass[i, j] = matrix.Mass[i, j];
            }

            for (int i = DelColumn; i < delColumnMatrix.RowCount; i++)
            {
                for (int j = 0; j < delColumnMatrix.ColumnCount; j++)
                    delColumnMatrix.Mass[i, j] = matrix.Mass[i + 1, j];
            }

            return delColumnMatrix;
        }

        public Matrix DecreaseMatrix(Matrix matrix, int DelRow, int DelColumn)
        {
            Matrix decreaseMatrix = matrix;
            decreaseMatrix = DelRowMatrix(ref decreaseMatrix, DelRow);
            decreaseMatrix = DelColumnMatrix(ref decreaseMatrix, DelColumn);
            return decreaseMatrix;

        }





    }
}
