using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MultiThreading.Task3.MatrixMultiplier.Tests
{
    [TestClass]
    public class MultiplierTest
    {
        // Parallel effectiveness starts with matrices with size of 150
        long[,] matrixOne = Program.PopulateMatrix(140);
        long[,] matrixTwo = Program.PopulateMatrix(140);

        [TestMethod]
        public void ParallelEfficiencyTest()
        {
            Program p = new Program();
            Stopwatch sw = Stopwatch.StartNew();
            p.MultiplyMatrices(matrixOne, matrixTwo);
            sw.Stop();
            var sequential = sw.ElapsedMilliseconds;
            Console.WriteLine($"Time with Sequential= {sw.ElapsedMilliseconds}");
            sw.Restart();
            p.MultiplyMatricesWithParallel(matrixOne, matrixTwo);
            sw.Stop();
            var parallel = sw.ElapsedMilliseconds;
            Console.WriteLine($"Time with Parallel= {sw.ElapsedMilliseconds}");
            Assert.IsTrue(parallel < sequential);

        }
    }
}
