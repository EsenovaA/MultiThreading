/*
 * 1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.
 * Each Task should iterate from 1 to 1000 and print into the console the following string:
 * “Task #0 – {iteration number}”.
 */
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
            Console.WriteLine(".Net Mentoring Program. Multi threading V1.");
            Console.WriteLine("1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.");
            Console.WriteLine("Each Task should iterate from 1 to 1000 and print into the console the following string:");
            Console.WriteLine("“Task #0 – {iteration number}”.");
            Console.WriteLine();
            
            HundredTasks();
        }

        static void HundredTasks()
        {
            var tasks = new Task[TaskAmount];
            for (int i = 0; i < TaskAmount; i++)
            {
                // Warning: Better to use Task.Factory
                tasks[i] = new Task((object param) =>
                {
                    // Warning: move to delegate method
                    var t = (int)param;
                    Output(t);
                }, i);
            }

            // Error: no need to use Parallel to start already parallel tasks
            Parallel.ForEach<Task>(tasks, (t) => { t.Start(); });
            Task.WaitAll(tasks);
        }

        static void Output(int taskNumber)
        {
            for(int i = 0; i< MaxIterationsCount; i++)
            {
                Console.WriteLine($"Task #{taskNumber} – {i}");
            }
        }
    }
}
