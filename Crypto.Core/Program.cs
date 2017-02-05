using System;

namespace Crypto.Basics
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                var pg = Core.GetPG(100000, 1000000);
                Console.WriteLine($" P:{pg.P}, G:{pg.G}");

                var alicePrivateKey = Core.GetRandomBigInt(100000, 1000000);
                var alicePublicKey = Core.ModPow(pg.G, alicePrivateKey, pg.P);

                var bobPrivateKey = Core.GetRandomBigInt(100000, 1000000);
                var bobPublicKey = Core.ModPow(pg.G, bobPrivateKey, pg.P);

                var aliceKey = Core.ModPow(bobPublicKey, alicePrivateKey, pg.P);
                var bobKey = Core.ModPow(alicePublicKey, bobPrivateKey, pg.P);

                Console.WriteLine($"AliceKey: {aliceKey}  BobKey: {bobKey}");

                var y = Core.ModPow(8, 3, 27);
                var x = Core.BabyStepGiantStep(y, 8, 27);
                Console.WriteLine($" X:{x}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
