using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {

        static void Main()
        {
            ManipulateTasks();
        }

        // Creates four chained tasks which manage an array of random int values. 
        static void ManipulateTasks()
        {
            Random random = new Random();
            var tenRandomInt = Task<int[]>.Run(() =>
            {
                int[] arr = new int[10];
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = random.Next(10);
                }
               // throw null;
                DisplayInConsole(arr, "Array of random int: ");
                return arr;
            }).ContinueWith(ant =>
            {
                ant.Wait();
                var randomInt = random.Next(5);
                var newArr = ant.Result.AsEnumerable().Select(arr => arr * randomInt).ToArray();
                DisplayInConsole(newArr, $"Array multiplied by {randomInt}: ");
                return newArr;
            }, TaskContinuatio⁠nOptions.ExecuteSynchronously).ContinueWith(ant =>
            {
                ant.Wait();
                var newA = ant.Result.AsEnumerable().OrderBy(arr => arr).ToArray();
                DisplayInConsole(newA, "Ordered by ascending: ");
                return newA;
            }, TaskContinuationOptions.ExecuteSynchronously).ContinueWith(ant =>
            {
                ant.Wait();
                var average = ant.Result.Average();
                Console.WriteLine($"Average= {average}");
            });

            tenRandomInt.Wait();
        }

        static void DisplayInConsole(int[] array, string message = default)
        {
            Console.Write(message);
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(" " + array[i]);
            }
            Console.WriteLine();
        }
    }
}



