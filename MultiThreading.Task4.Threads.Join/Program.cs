/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            // feel free to add your code

            var threadFromPool = new ThreadFromPool();
            var thread1 = new Thread(threadFromPool.CreateThreadsRecursively);
            thread1.Name = "Initial thread with pool";
            thread1.Start(new[] { 38 });

            Console.ReadLine();
        }
    }

    public class ThreadJoin
    {
        public int ThreadsCount = 10;

        public void CreateThreadsRecursively(object state)
        {
            if (ThreadsCount == 0)
            {
                Console.WriteLine("All threads have been created.");
                return;
            }

            var intState = (int)state;
            intState--;
            ThreadsCount--;

            Thread thread1 = new Thread(CreateThreadsRecursively);
            thread1.Name = $"Thread{ThreadsCount}";
            thread1.Start(intState);

            thread1.Join();
            
            Console.WriteLine($"State: {intState}, Current thread: {Thread.CurrentThread.Name}.");
            
        }
    }

    public class ThreadFromPool
    {
        private static Semaphore _pool = new Semaphore(initialCount: 1, maximumCount: 1);

        public int ThreadsCount = 10;

        public void CreateThreadsRecursively(object state)
        {
            var intState = ((int[])state)[0];
            intState--;
            ThreadPool.QueueUserWorkItem(CreateThreadsRecursively, new[] { intState });

            _pool.WaitOne();

            if (ThreadsCount == 0)
            {
                Console.WriteLine("All threads have been created.");
                return;
            }
            
            ThreadsCount--;
            Console.WriteLine($"State: {intState}, Current count: {ThreadsCount}.");
            
            _pool.Release();
        }
    }
}
