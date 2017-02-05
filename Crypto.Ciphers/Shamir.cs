using System.Numerics;

using Crypto.Basics;

namespace Crypto.Ciphers
{
    public class Shamir
    {
        public static ShamirKey GenerateKeys(BigInteger p)
        {
            var e = Core.GetRandomBigInt(0, p - 1);
            if (e.IsEven)
            {
                e++;
            }

            while (Core.Gcd(e, p - 1) != 1)
            {
                e += 2;
            }

            var d = Core.ExtendedEuclid(e, p - 1)[1];
            if (d < 0)
                d += p - 1;

            return new ShamirKey
            {
                E = e,
                D = d
            };
        }
    }

    public class ShamirKey
    {
        public BigInteger E;
        public BigInteger D;
    }
}
