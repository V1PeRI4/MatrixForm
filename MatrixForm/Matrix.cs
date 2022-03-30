using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixForm
{
    public class Matrix
    {
        
        public int Row { get; set; }
        public int Column { get; set; }
        public int[,] Mass { get; set; }

        public Matrix(int row, int column)
        {
            Mass = new int[row, column];
        }


    }
}
