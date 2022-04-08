namespace MatrixForm
{
    public class Matrix
    {
        
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        public double[,] Mass { get; set; }


        public Matrix(int row, int column)
        {
            RowCount = row;
            ColumnCount = column;
            Mass = new double[row, column];
        }


    }
}
