using System;

namespace Crypto
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var pg = Lab1.GetPG(100000, 1000000);
            Console.WriteLine($" P:{pg[0]}, G:{pg[1]}");

            var alicePrivateKey = Lab1.GetRandomBigInt(100000, 1000000);
            var alicePublicKey = Lab1.ModPow(pg[1], alicePrivateKey, pg[0]);

            var bobPrivateKey = Lab1.GetRandomBigInt(100000, 1000000);
            var bobPublicKey = Lab1.ModPow(pg[1], bobPrivateKey, pg[0]);

            var aliceKey = Lab1.ModPow(bobPublicKey, alicePrivateKey, pg[0]);
            var bobKey = Lab1.ModPow(alicePublicKey, bobPrivateKey, pg[0]);

            Console.WriteLine($"AliceKey: {aliceKey}  BobKey: {bobKey}");

            var y = Lab1.ModPow(8, 3, 27);
            var x = Lab1.BabyStepGiantStep(y, 8, 27);
            Console.WriteLine($" X:{x}");
            Console.ReadKey();
        }
    }
}
