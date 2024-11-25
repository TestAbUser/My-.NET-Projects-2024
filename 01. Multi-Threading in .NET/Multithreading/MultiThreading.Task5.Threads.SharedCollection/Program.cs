using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static List<int> list = new List<int>();

        // Displays all elements of a collection as it is being added to.
        static void Main(string[] args)
        {
            const int SizeOfList = 10;
            List<int> list = new List<int>(SizeOfList);

            Task task = Task.Run(() =>
            {
                for (int i = 0; i < SizeOfList; i++)
                {
                    list.Add(i);
                    Task task2 = Task.Run(() =>
                    {
                        list.ForEach(x => Console.Write(x));
                    });
                    task2.Wait();
                    Console.WriteLine();
                }
            });
            Console.ReadLine();
        }
    }
}
