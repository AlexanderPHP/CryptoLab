using System;

namespace Crypto
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // var t = Lab1.ModPow(100, 45, 237);
            // var t1 = Lab1.ExtendedEuclid(28, 19);
            // Console.WriteLine("{0} {1} {2} | {3}", t1[0], t1[1], t1[2], t);
            do
            {
                var pg = Lab1.GetPG(1000, 10000);
                Console.WriteLine($" P:{pg[0]}, G:{pg[1]}");
            } while (Console.ReadKey() != new ConsoleKeyInfo());
            
           // var pg = Lab1.GetPG(1000, 10000);
           // Console.WriteLine($" P:{pg[0]}, G:{pg[1]}");
            Console.ReadKey();
        }
    }
}
