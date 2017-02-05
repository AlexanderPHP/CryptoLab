using System;

namespace Modern.Polynomial
{
    internal class Program
    {
        private static void Main()
        {
            var a = new Polynomial(3, 3) + new Polynomial(2, 4) + new Polynomial(5);
            var b = new Polynomial(4, 3) + new Polynomial(2, 2) + new Polynomial(6, 1);
            Test(a, b);
            Console.ReadLine();
        }

        public static void Test(Polynomial a, Polynomial b)
        {
            Console.WriteLine($"a: {a}");
            Console.WriteLine($"b: {b}");
            Console.WriteLine($"a+b: {a+b}");
            Console.WriteLine($"a-b: {a-b}");
            Console.WriteLine($"a*b: {a*b}");
            Console.WriteLine($"a*b exp: {(a*b).MaxExponent()}");
            Console.WriteLine($"(a*b)': {(a*b).Differentiate()}");
            Console.WriteLine($"((a*b)')': {(a * b).Differentiate().Differentiate()}");
            Console.WriteLine($"a(2): {a.Calculate(2)}");
        }

    }
}
