
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixForm
{
    public class Model
    {
        Matrix resultMatrix = new Matrix(3, 3);

        public delegate void ResultDelegate(Matrix resultMatrix);
        public event ResultDelegate NotifResult;
        public void MultMatrix(Matrix matrix1, Matrix matrix2)
        {
            int _row = matrix1.GetColumnCount();

            int _col = matrix1.GetColumnCount();


            int _row = matrix2.GetColumnCount();

            int _col = matrix2.GetColumnCount();


            for (int i = 0; i < 3; i++)
            resultMatrix.Mass[0, i] = matrix1.Mass[0, 0] * matrix2.Mass[0, i] + matrix1.Mass[0, 1] * matrix2.Mass[1, i] + matrix1.Mass[0, 2] * matrix2.Mass[2, i];

            for (int i = 0; i < 3; i++)
                resultMatrix.Mass[1, i] = matrix1.Mass[1, 0] * matrix2.Mass[0, i] + matrix1.Mass[1, 1] * matrix2.Mass[1, i] + matrix1.Mass[1, 2] * matrix2.Mass[2, i];

            for (int i = 0; i < 3; i++)
                resultMatrix.Mass[2, i] = matrix1.Mass[2, 0] * matrix2.Mass[0, i] + matrix1.Mass[2, 1] * matrix2.Mass[1, i] + matrix1.Mass[2, 2] * matrix2.Mass[2, i];

            NotifResult.Invoke(resultMatrix);
        }

        public void ReverseMatrix(Matrix matrix1)
        {
            int detMatrix = 0;

            NotifResult.Invoke(resultMatrix);
        }

        
    }
}
