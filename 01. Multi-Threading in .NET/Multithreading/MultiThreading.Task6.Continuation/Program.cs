/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and
parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would 
be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {

        static void Main(string[] args)
        {
            CreateContinuationTasks();
        }

        // Executes a chain of continuations which execute conditionally.
        static void CreateContinuationTasks()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            token.ThrowIfCancellationRequested();
            var factory = new TaskFactory(TaskCreationOptions.None, TaskContinuationOptions.ExecuteSynchronously);

            Task task = Task.Run(() =>
            {
                Console.WriteLine("Parent task is running.");
                Thread.CurrentThread.Name = "Parent";
                Console.WriteLine("Current thread Name: {0}", Thread.CurrentThread.Name);
                Console.WriteLine("Current thread state: {0}", Thread.CurrentThread.ThreadState);
                Console.WriteLine("Is Thread pool thread: {0}", Thread.CurrentThread.IsThreadPoolThread);
                Console.WriteLine("\nSelect an option by pressing following keys:");
                Console.WriteLine("'1' - to fault Parent task with an exception.");
                Console.WriteLine("'2' - to cancel Parent task.");
                Console.WriteLine("Press any other key to successfully finish Parent task.");

                var option = Console.ReadKey(true);

                switch (option.Key)
                {
                    case ConsoleKey.D1:

                        throw null;
                        break;
                    case ConsoleKey.D2:

                        cts.Cancel();
                        token.ThrowIfCancellationRequested();
                        break;
                    default:
                        break;
                }

            }, token);

            Task<string> taskA = task.ContinueWith(ant =>
            {
                string a = String.Format("{0,90}", "Task A: Continues no matter how Parent task finished ");
                return a;
            });

            Task<string> taskB = task.ContinueWith(ant =>
            {
                string b = String.Format("{0,90}", "Task B: Continues only if Parent task faulted or was cancelled");
                return b;
            }, TaskContinuationOptions.NotOnRanToCompletion);

            Task<string> taskC = task.ContinueWith(ant =>
            {
                string c = String.Format("{0,90} {1,90}", "Task C: Uses the same thread and continues only if Parent task faulted",
                                         $"Thread name= {Thread.CurrentThread.Name}");
                return c;
            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            Task<string> taskD = task.ContinueWith(ant =>
            {
                string d = String.Format("{0,90} {1,90}", "Task D: Uses thread outside thread pool and continues only if Parent was cancelled",
                    $"IsThreadPoolThread= {Thread.CurrentThread.IsThreadPoolThread}");
                return d;
            }, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);

            var tasks = new Task<string>[] { taskA, taskB, taskC, taskD };
            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException)
            {
                Console.WriteLine("Status of continuation tasks:\n");
                Console.WriteLine("{0,5} {1,20} {2,40}", "Task Id", "Status", "Message");
                for (int i = 0; i < tasks.Length; i++)
                {
                    var j = i;
                    Console.WriteLine("{0,5} {1,20} {2,90}",
                                      tasks[j].Id, tasks[j].Status,
                                        tasks[j].Status != TaskStatus.Canceled ? tasks[j].Result : "n/a");
                }

                Console.ReadLine();
            }
        }
    }
}
