using System;
using System.Threading.Tasks;

namespace MultiThreading.Task1._100Tasks
{
    class Program
    {
        const int TaskAmount = 100;
        const int MaxIterationsCount = 1000;

        static void Main(string[] args)
        {
            Console.BufferHeight = Int16.MaxValue - 1;
            HundredTasks();
            Console.ReadLine();
        }

        // Launches a hundred tasks each of which iterates a thousand times
        static async void HundredTasks()
        {
            Task[] taskArray = new Task[TaskAmount];

            for (var i = 0; i < taskArray.Length; i++)
            {
                int k = i;
                taskArray[i] = new Task(() =>
                {

                    for (var j = 0; j < MaxIterationsCount; j++)
                    {
                        Output(k, j);
                    }
                });
                taskArray[i].Start();
                await taskArray[i];
            }
            Task all = Task.WhenAll(taskArray);
            try
            {
                await all;
            }
            catch
            {
                Console.WriteLine("Number of exceptions {0}", all.Exception.InnerExceptions.Count);
            }
        }

        static void Output(int taskNumber, int iterationNumber)
        {
            Console.WriteLine($"Task #{taskNumber} – {iterationNumber}");
        }
    }

}
