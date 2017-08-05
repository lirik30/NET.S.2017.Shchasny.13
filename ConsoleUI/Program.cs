using System;
using QueueLogic;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] arr = {1,2,2,4,5,8,7,6,5,5};
            var queue = new Queue<int>(arr);
            queue.Enqueue(11);
            queue.Enqueue(12);
            queue.Enqueue(13);
            queue.Dequeue();
            Console.WriteLine(queue.Peek());
            foreach (var elem in queue)
                Console.WriteLine(elem);

            var iterator = queue.GetEnumerator();
            iterator.MoveNext();  
            queue.Enqueue(14);   //changes queue while iterating
            iterator.MoveNext();//throws exception
            iterator.Dispose();
            Console.ReadKey();
        }
    }
}
