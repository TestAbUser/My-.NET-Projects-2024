using System;
using System.Threading.Tasks;

namespace MultiThreading.Task3.MatrixMultiplier
{
    public class Program
    {
        static void Main(string[] args)
        {
            const byte matrixSize = 3; // todo: use any number you like or enter from console
            CreateAndProcessMatrices(matrixSize);
            Console.ReadLine();
        }

        public static void CreateAndProcessMatrices(byte matrixSize)
        {
            Program p = new Program();
            var matrixOne = PopulateMatrix(matrixSize);
            var matrixTwo = PopulateMatrix(matrixSize);
            DisplayMatrix(matrixOne, "matrixOne");
            DisplayMatrix(matrixTwo, "matrixTwo");
            var matrixThree = p.MultiplyMatrices(matrixOne, matrixTwo);
            DisplayMatrix(matrixThree, "multipliedSequentially");
            var matrixFour = p.MultiplyMatricesWithParallel(matrixOne, matrixTwo);
            DisplayMatrix(matrixFour, "multipliedWithParallel");
        }

        // Displays a two-dimensional array in Console.
        static void DisplayMatrix(long[,] matrix, string matrixName = default)
        {
            Console.WriteLine(matrixName);
            for (int i = 0; i < Math.Sqrt(matrix.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(matrix.Length); j++)
                {
                    Console.Write($"{matrix[i, j],5}");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        // Creates and assigns values to a two-dimensional array.
        public static long[,] PopulateMatrix(byte matrixSize)
        {
            Random random = new Random();
            var matrix = new long[matrixSize, matrixSize];
            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    matrix[i, j] = random.Next(10);
                }
            }
            return matrix;
        }

        // Multiplies two matrices using Parallel.For.
        public long[,] MultiplyMatricesWithParallel(long[,] matrixOne, long[,] matrixTwo)
        {
            var matrixSize = (long)Math.Sqrt(matrixOne.Length);
            long[,] multiplied = new long[matrixSize, matrixSize];
            Parallel.For(0, matrixSize, i =>
            {
                // Changing counters from int to byte since matrixSize is in byte.
                for (byte j = 0; j < matrixSize; j++) 
                {
                    long temp = 0;// I nearly offset all the advantage of Parallel by placing "temp" outside of the loops
                    for (byte k = 0; k < matrixSize; k++)
                    {
                        temp += matrixOne[i, k] * matrixTwo[k, j];
                    }
                    multiplied[i, j] = temp;
                    // temp = 0;
                }
            });
            return multiplied;
        }

        public long[,] MultiplyMatrices(long[,] matrixOne, long[,] matrixTwo)
        {
            var matrixSize = (long)Math.Sqrt(matrixOne.Length);
            long[,] multiplied = new long[matrixSize, matrixSize];
            for (long i = 0; i < matrixSize; i++)
            {
                // Changing counters from int to byte since matrixSize is in byte.
                for (byte j = 0; j < matrixSize; j++) 
                {
                    long temp = 0;
                    for (byte k = 0; k < matrixSize; k++)
                    {
                        temp += matrixOne[i, k] * matrixTwo[k, j];
                    }
                    multiplied[i, j] = temp;
                }
            };
            return multiplied;
        }
    }
}
