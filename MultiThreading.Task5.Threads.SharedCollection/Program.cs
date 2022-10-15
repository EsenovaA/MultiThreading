/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            // feel free to add your code

            var threadsWithSharedCollection = new ThreadsWithSharedCollection();
            threadsWithSharedCollection.ProcessCollection();

            Console.ReadLine();
        }
    }

    public class ThreadsWithSharedCollection
    {
        private List<EditableElement> _sharedCollection = new List<EditableElement>();
        private const int _collectionLength = 10;
        
        private Semaphore _semaphore = new Semaphore(1, 1);
        
        public void ProcessCollection()
        {
            var taskFill = new Task(FillCollection);
            var taskUpdate = new Task(UpdateOne);
            taskFill.Start();
            taskUpdate.Start();
        }

        private void FillCollection()
        {
            var elementValue = 1;
            bool added;

            while (elementValue <= _collectionLength)
            {
                added = AddOne(elementValue);
                if (added)
                {
                    elementValue++;
                }
            }
        }

        private bool AddOne(int i)
        {
            _semaphore.WaitOne();

            var added = false;

            if (_sharedCollection.Count != _collectionLength && (_sharedCollection.Count == 0 || _sharedCollection.All(c => c.Edited)))
            {
                _sharedCollection.Add(new EditableElement()
                {
                    Value = i
                });

                added = true;
            }
            
            _semaphore.Release();

            return added;
        }

        private void UpdateOne()
        {
            var shouldTryUpdate = true;

            while (shouldTryUpdate)
            {
                _semaphore.WaitOne();

                if (_sharedCollection.Count == _collectionLength && _sharedCollection.All(e=>e.Edited))
                {
                    shouldTryUpdate = false;
                }

                if (_sharedCollection.Count > 0 && _sharedCollection.Any(e => !e.Edited))
                {
                    var firstNotEdited = _sharedCollection.First(e => !e.Edited);
                    firstNotEdited.PrintableValue = String.Join(',',
                        _sharedCollection.OrderBy(el => el.Value).ToArray().Select(e => e.Value));
                    firstNotEdited.Edited = true;
                    Console.WriteLine($"Elements of collection: {string.Join(',', _sharedCollection.Select(e=>$"[{e.PrintableValue}]"))}");
                }

                _semaphore.Release();
            }
        }
    }

    public class EditableElement
    {
        public int Value { get; set; }
        public string PrintableValue { get; set; }
        public bool Edited { get; set; }
    }
}
