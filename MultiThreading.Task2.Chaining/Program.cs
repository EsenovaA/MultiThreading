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
        private static Random _rnd = new Random();
        private const int _arrayLength = 10;
        private const int _maxRandomValue = 10;
        private const int _minRandomValue = 5;

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
            // Warning: use shorter "syntax sugar" where it's possible
            Task<int[]> taskRandom = Task.Run(() => GenerateArray());

            var taskMultiply = taskRandom.ContinueWith(antecedent => MultiplyArray((int[])antecedent.Result));

            var taskSort = taskMultiply.ContinueWith(antecedent => SortArray((int[])antecedent.Result));

            var taskAverage = taskSort.ContinueWith(antecedent =>
            {
                return antecedent.Result.Average(x => x);
            });

            Console.WriteLine($"Average of sorted = {taskAverage.Result}");//this line waits for taskAverage.Result, and of course for all chaining tasks

        }
        
        public static int[] GenerateArray()
        {
            var intArray = new int[_arrayLength];
            for (int i = 0; i < _arrayLength; i++)
            {
                intArray[i] = _rnd.Next(0, RandomMaxValue);
                Console.WriteLine($"Source array element[{i}] = {intArray[i]}");
            }
            return intArray;
        }

        public static int[] MultiplyArray(int[] inArray)
        {
            var outArray = new int[10];
            inArray.CopyTo(outArray, 0);

            var multipleOn = _rnd.Next(_minRandomValue, _maxRandomValue);
            Console.WriteLine($"Multiple array on = {multipleOn}");
            for (int i = 0; i < _arrayLength; i++)
            {
                outArray[i] = inArray[i] * multipleOn;
                Console.WriteLine($"Multiplied array element[{i}] = {inArray[i]}"); // Error: input array instead of output:)
            }
            return outArray;
        }

        public static int[] SortArray(int[] inArray)
        {
            var outArray = inArray.OrderBy(x => x).ToArray();
            for (int i = 0; i < _arrayLength; i++)
            {
                Console.WriteLine($"Sorted array element[{i}] = {outArray[i]}");
            }

            return outArray;
        }
        
    }
    
}
