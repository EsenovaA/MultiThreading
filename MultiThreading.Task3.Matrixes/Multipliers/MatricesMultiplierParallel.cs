using System.Diagnostics;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using System.Threading.Tasks;

namespace MultiThreading.Task3.MatrixMultiplier.Multipliers
{
    public class MatricesMultiplierParallel : IMatricesMultiplier, IMatrixElapser
    {
        public IMatrix Multiply(IMatrix m1, IMatrix m2)
        {
            var resultMatrix = new Matrix(m1.RowCount, m2.ColCount);
            // Error: this should be paralled too
            for (long i = 0; i < m1.RowCount; i++)
            {
                Parallel.For(0, m2.ColCount, j => resultMatrix.SetMultipliedElement(m1, m2, i, (int)j)); // Warning: use whether int or long everywhere
            };

            return resultMatrix;
        }

        public long GetMultiplyElapsed(IMatrix m1, IMatrix m2)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            Multiply(m1, m2);
            stopwatch.Stop();

            return stopwatch.ElapsedMilliseconds;
        }
    }

    public static class MatrixHelper
    {
        public static void SetMultipliedElement(this Matrix matrix, IMatrix m1, IMatrix m2, long i, int j)
        {
            long sum = 0;
            for (byte k = 0; k < m1.ColCount; k++)
            {
                sum += m1.GetElement(i, k) * m2.GetElement(k, j);
            }

            matrix.SetElement(i, j, sum);
        }
    }
}
