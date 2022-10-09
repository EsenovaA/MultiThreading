using System;
using System.Collections.Generic;
using System.Text;
using MultiThreading.Task3.MatrixMultiplier.Matrices;

namespace MultiThreading.Task3.MatrixMultiplier
{
    public interface IMatrixElapser
    {
        long GetMultiplyElapsed(IMatrix m1, IMatrix m2);
    }
}
