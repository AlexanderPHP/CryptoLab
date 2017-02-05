using System;
using Modern.Processor;

namespace Modern.Set
{
    internal class Program
    {
        private static void Main()
        {
            var set1 = new Set<Frac>();
            var set2 = new Set<Frac>();
            for (var i = 0; i < 7; i++)
                set1.Add(new Frac(i, i + 2));
            for (var i = 0; i < 10; i++)
                set2.Add(i);
            for (var i = 0; i < set1.Size(); i++)
                Console.WriteLine(set1.At(i));

            Console.WriteLine();
            for (var i = 0; i < set2.Size(); i++)
                Console.WriteLine(set2.At(i));

            var result = set1.Union(set2);
            Console.WriteLine();
            for (var i = 0; i < result.Size(); i++)
                Console.WriteLine(result.At(i));

            Console.ReadLine();
        }
    }
}
