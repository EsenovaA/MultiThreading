/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        private const int RandomMaxValue = 1000;

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            ExecuteTaskChain();

        }

        static void ExecuteTaskChain()
        {
            // Warning: Use methods as tasks bodies
            Task<int[]> taskRandom = Task.Run(() =>
            {
                var rnd = new Random(); // Warning: better to use common randomiser
                var intArray = new int[10]; // Warning: use const or extrnal variable instead of 10
                for (int i = 0; i < 10; i++) // Error: use Length property, the same in all cases bellow
                {
                    intArray[i] = rnd.Next(0, RandomMaxValue);
                    Console.WriteLine($"Source array element[{i}] = {intArray[i]}");
                }
                return intArray;
            });

            var taskMultiply = taskRandom.ContinueWith(antecedent => //Task<int[]>???
            {
                var rnd = new Random();
                var inArray = antecedent.Result;
                var multipleOn = rnd.Next(5, 10);
                Console.WriteLine($"Multiple array on = {multipleOn}");
                for (int i = 0; i < 10; i++)
                {
                    inArray[i] = inArray[i] * multipleOn; // Error: side effect, the insput is changed, better to create output array
                    Console.WriteLine($"Multiplied array element[{i}] = {inArray[i]}");
                }
                return inArray;
            });

            var taskSort = taskMultiply.ContinueWith(antecedent =>
            {
                var inArray = antecedent.Result;
                var outArray = inArray.OrderBy(x=>x).ToArray();
                for (int i = 0; i < 10; i++) // Warning: foreach
                {
                    Console.WriteLine($"Sorted array element[{i}] = {outArray[i]}");
                }

                return outArray;
            });

            var taskAverage = taskSort.ContinueWith(antecedent =>
            {
                var inArray = antecedent.Result;
                var average = inArray.Average(x=>x);
                return average;
            });

            Console.WriteLine($"Average of sorted = {taskAverage.Result}");//this line waits for taskAverage.Result, and of course for all chaining tasks

        }

        public int[] MultiplyArray(int[] inArray)
        {
            var rnd = new Random();
            var multipleOn = rnd.Next(0, 10);
            for (int i = 0; i < 10; i++)
            {
                inArray[i] = inArray[i] * multipleOn;
                Console.WriteLine($"Multiplied array element[{i}] = {inArray[i]}");
            }
            return inArray;
        }
    }
}
