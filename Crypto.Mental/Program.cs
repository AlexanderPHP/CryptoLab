using System;

namespace Crypto.Mental
{
    internal class Program
    {
        private static void Main()
        {
            var poker = new Poker();
            for (var i = 0; i < poker.PlayersCount; i++)
            {
                poker.EncryptDeck(i);
                poker.ShuffleDeck();
            }

            for (var k = 0; k < 2; k++)
            {
                for (var i = 0; i < poker.PlayersCount; i++)
                {
                    var index = poker.DistributeCard();
                    for (var j = 0; j < poker.PlayersCount; j++)
                    {
                        if (i != j)
                        {
                            poker.DecryptCard(j, index);
                        }
                    }
                    poker.Players[i].Cards[k] = poker.DecryptCard(i, index);
                }
            }
            for (var i = 0; i < poker.PlayersCount; i++)
            {
                poker.PrintPlayerCards(i);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
