using System;

namespace Modern.Processor
{
    internal class Program
    {
        private static void Main()
        {
            var frac1 = new Frac("5/7");
            var frac2 = new Frac(9, 5);

            Console.WriteLine(frac1 * frac2);

            var comp1 = new Complex(3, 4);
            var comp2 = new Complex("0.5+6i");
            Console.WriteLine(comp2 != comp1);
            Console.WriteLine(comp1 + comp2);
            Console.WriteLine(comp1 - comp2);
            Console.WriteLine(comp1 * comp2);

            Console.WriteLine(comp1 / comp2);
            Console.WriteLine(comp1.Radians());
            Console.WriteLine(comp1.Degrees());
            Console.WriteLine(comp1.Square());
            Console.WriteLine(comp1.Pow(2));
            Console.WriteLine(comp1.Reverse());
            //  var t = "a.bc".Split('.');

            var t = new Pnumber("A.FF", 16, 5);
            var t1 = new Pnumber("A.01", 16, 5);
            Console.WriteLine(t + t1 + " asd");

            var mem = new Memory<Frac>(new Frac(1, 2));
            mem.Add(new Frac(3, 2));
            Console.WriteLine(mem.Read());
            var t2 = new Complex(3, 4);
            t2 = t2.Add(new Complex(3, 4));
            Console.WriteLine(t2);
            Console.ReadKey();
        }
    }
}
