using Microsoft.VisualStudio.TestTools.UnitTesting;
using MatrixForm;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Matrix matrix1 = new Matrix(3, 3);
            for (int i = 0; i < matrix1.RowCount; i++)
            {
                for (int j = 0; j < matrix1.ColumnCount; j++)
                {
                    matrix1.Mass[i, j] = i;
                }
            }


            Matrix matrix2 = new Matrix(3, 3);
            for (int i = 0; i < matrix1.RowCount; i++)
            {
                for (int j = 0; j < matrix1.ColumnCount; j++)
                {
                    matrix1.Mass[i, j] = j;
                }
            }

            ///;


            Assert.AreEqual(2 + 2, 4);
        }
    }
}