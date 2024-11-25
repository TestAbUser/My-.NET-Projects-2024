using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        static int threadNumber = 10;
        static Semaphore semaphore = new Semaphore(1, threadNumber);
        static void Main(string[] args)
        {

             RecursiveThreadCreation(10);
            RecursiveThreadCreationSemaphore(10);
            Console.ReadLine();
        }

        // Creates recursive threads using Thread and waits for them using Join.
        public static void RecursiveThreadCreation(object state)
        {
            int newState = (int)state;
            if (newState>0)
            {
                newState -= 1;
                Thread thread = new Thread(RecursiveThreadCreation);
                thread.Name =newState.ToString();
                thread.Start(newState);
                thread.Join();
                Console.WriteLine("\nCurrent thread Name: {0}", Thread.CurrentThread.Name);
                Console.WriteLine("\nCurrent thread state: {0}", Thread.CurrentThread.ThreadState);
                Console.WriteLine("Thread State: {0}", thread.ThreadState);
                Console.WriteLine("Thread Name: {0}", thread.Name);
            }
        }

        // Creates recursive threads using ThreadPool and waits for them using Semaphore. 
        public static void RecursiveThreadCreationSemaphore(object state)
        {
            int newState = (int)state;
            if (newState > 0)
            {
                semaphore.WaitOne();
                newState -= 1;
                ThreadPool.QueueUserWorkItem(RecursiveThreadCreationSemaphore, newState);
                Thread.CurrentThread.Name = newState.ToString();
                Console.WriteLine("\nCurrent thread: {0} has state {1}", Thread.CurrentThread.Name, Thread.CurrentThread.ThreadState);
                semaphore.Release();
            }

        }
    }
}
