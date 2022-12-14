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
                // Error: variable i is modifying outside of the task, that's why most of the tasks have i == 100
                tasks[i] = new Task((object param) => Print(i), i);
                tasks[i].Start();
                //Task.Factory.StartNew(() => Print(i));
            }
            
            Task.WhenAll(tasks);
        }

        static void Print(int param)
        {
            for (int i = 0; i < MaxIterationsCount; i++)
            {
                Console.WriteLine($"Task #{param} – {i}");
            }
        }
        
    }
}
