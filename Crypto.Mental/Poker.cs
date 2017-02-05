using System;
using System.Numerics;
using Crypto.Basics;

namespace Crypto.Mental
{
    public class Poker
    {
        public const int CardsCount = 52;
        public Card[] Deck = new Card[CardsCount];
        public int PlayersCount = 2;
        public Player[] Players;
        public BigInteger CommonPrime;

        public Poker()
        {
            for (var i = 0; i < CardsCount; i++)
            {
                Deck[i].Id = i + 2;
                Deck[i].IsUsed = false;
            }

            CommonPrime = Core.GetPrime(100, 10000);

            int result;
            Console.WriteLine("Enter number of players: ");
            if (!int.TryParse(Console.ReadLine(), out result)) return;

            if (1 < result || result < 10)
                PlayersCount = result;

            Players = new Player[PlayersCount];

            GeneratePlayersKeys();
        }

        public void GeneratePlayersKeys()
        {
            for (var i = 0; i < PlayersCount; i++)
            {
                var c = Core.GetRandomBigInt(0, CommonPrime - 1);
                if (c.IsEven)
                    c += 1;

                while (Core.Gcd(c, CommonPrime - 1) != 1)
                    c += 2;

                var d = Core.ExtendedEuclid(c, CommonPrime - 1)[1];
                if (d < 0)
                    d += CommonPrime - 1;

                Players[i].Crypt = c;
                Players[i].Decrypt = d;
                Players[i].Cards = new BigInteger[2];
            }
        }

        public void EncryptDeck(int pid)
        {
            for (var i = 0; i < CardsCount; i++)
            {
                Deck[i].Id = Core.ModPow(Deck[i].Id, Players[pid].Crypt, CommonPrime);
            }
        }

        public void ShuffleDeck()
        {
            var rng = new Random();
            var n = Deck.Length;
            while (n > 1)
            {
                var k = rng.Next(n--);
                var temp = Deck[n];
                Deck[n] = Deck[k];
                Deck[k] = temp;
            }
        }

        public int DistributeCard()
        {
            int i;
            var rng = new Random();

            do
            {
                i = rng.Next() % CardsCount;
            } while (Deck[i].IsUsed);

            Deck[i].IsUsed = true;
            return i;
        }

        public BigInteger DecryptCard(int pid, int idx)
        {
            Deck[idx].Id = Core.ModPow(Deck[idx].Id, Players[pid].Decrypt, CommonPrime);
            return Deck[idx].Id;
        }

        public void PrintPlayerCards(int pid)
        {
            string[] suits = { "Spades", "Hearts", "Clubs", "Diamonds" };
            string[] faces =
            {
                "Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack",
                "Queen", "King"
            };
            Console.WriteLine($"Player #{pid + 1}:");
            foreach (var card in Players[pid].Cards)
            {
                var suit = (int)((card - 2) / 13);
                var face = (int)((card - 2) % 13);
                Console.WriteLine($"{faces[face]} {suits[suit]}");
            }
            Console.WriteLine(new string('-', 8));
        }
    }

    public struct Player
    {
        public BigInteger[] Cards;
        public BigInteger Crypt;
        public BigInteger Decrypt;
    }

    public struct Card
    {
        public BigInteger Id;
        public bool IsUsed;
    }
}
