/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
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
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            // feel free to add your code
            var helper = new ContinuationHelper();
            helper.ContinueAnywayAsync();
            helper.ContinueOnFailAsync();
            helper.ContinueOnFailAndReuseParentAsync();
            helper.ContinueOnCancelAsync();

            Console.ReadLine();
        }
    }

    public class ContinuationHelper
    {
        public async Task ContinueAnywayAsync()
        {
            var task = Task.Run(() => throw new InvalidOperationException("This task fails always!"));

            await task.ContinueWith(
                antecedent =>
                {
                    Console.WriteLine("This prints in child task regardless of parent's result.");
                }, TaskContinuationOptions.None);
        }

        public async Task ContinueOnFailAsync()
        {
            var task = Task.Run(() => throw new InvalidOperationException("This task fails always!"));

            await task.ContinueWith(
                antecedent =>
                {
                    Console.WriteLine("This prints in child task if parent failed with unhandled exception.");
                }, TaskContinuationOptions.OnlyOnFaulted);
        }

        public async Task ContinueOnFailAndReuseParentAsync()
        {
            var task = Task.Run(() =>
            throw new InvalidOperationException("This task fails always!")
            //Console.WriteLine("Parent task executed successfully.")
            );

            // Error: I believe ExecuteSynchronously flag should be used here
            // Error: continues always, not on fail (hint: you can use several flags)
            await task.ContinueWith(
                antecedent =>
                {
                    Console.WriteLine("This prints in child task if parent failed with unhandled exception. Child task reuses parent's thread.");
                }, TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnFaulted);
        }

        public async Task ContinueOnCancelAsync()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            var task = Task.Run(() => DoSomeWork(token), token);

            //Let's wait little
            Thread.Sleep(2000);

            //Cancel
            tokenSource.Cancel();

            // Error: continues always, not on cancel (hint: you can use several flags)
            await task.ContinueWith(
                antecedent =>
                {
                    Console.WriteLine("This prints in child task if parent was cancelled. Child task runs outside thread pool.");
                }, TaskContinuationOptions.LongRunning | TaskContinuationOptions.OnlyOnFaulted);
        }

        private void DoSomeWork(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(500);
                Console.WriteLine("Piece of work has been done.");
            }
        }
    }
}
