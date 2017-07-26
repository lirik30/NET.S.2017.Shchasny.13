using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] arr = {1,2,2,4,5,8,7,6,5,5};
            var queue = new Queue<int>(new List<int>(arr));
            queue.Enqueue(111);
            queue.Dequeue();

            foreach (var x in queue)
            {
                Console.WriteLine(x);
            }
            Console.ReadKey();
        }
    }
}
