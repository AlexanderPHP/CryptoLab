using System.Numerics;
using System.Security.Cryptography;

namespace Crypto
{
    public class Lab1
    {
        public static BigInteger ModPow(BigInteger a, BigInteger b, BigInteger p)
        {
            BigInteger res = 1;

            for (; b != 0; b >>= 1, a = a * a % p)
            {
                if ((b & 1) != 0)
                {
                    res = res * a % p;
                }
            }
            return res;
        }

        public static BigInteger[] ExtendedEuclid(BigInteger a, BigInteger b)
        {
            var u = new[] { a, 1, 0 };
            var v = new[] { b, 0, 1 };
            var t = new BigInteger[3];

            while (v[0] > 0)
            {
                var q = u[0] / v[0];
                t[0] = u[0] % v[0];
                t[1] = u[1] - q * v[1];
                t[2] = u[2] - q * v[2];

                u[0] = v[0]; u[1] = v[1]; u[2] = v[2];
                v[0] = t[0]; v[1] = t[1]; v[2] = t[2];
            }
            return u;
        }

        public static BigInteger GetRandomBigInt(BigInteger minValue, BigInteger maxValue)
        {
            var rng = RandomNumberGenerator.Create();
            var bytes = new byte[maxValue.ToByteArray().Length];
            BigInteger a;

            do
            {
                rng.GetBytes(bytes);
                a = new BigInteger(bytes);
            }
            while (a < minValue || a >= maxValue);
            return a;
        }

        public static bool MillerRabinTest(BigInteger n, BigInteger k)
        {
            if (n < 2)
                return false;
            if (n == 2)
                return true;
            if (n % 2 == 0)
                return false;
            var r = 0;
            var d = n - 1;
            while (d % 2 == 0)
            {
                d /= 2;
                r++;
            }
            for (var i = 0; i < k; i++)
            {
                var a = GetRandomBigInt(2, n - 1);
                var x = ModPow(a, d, n);
                if (x == 1 || x == n - 1)
                    continue;
                for (var j = 0; j < r - 1; j++)
                {
                    x = ModPow(x, 2, n);
                    if (x == 1)
                        return false;
                    if (x == n - 1)
                        break;
                }
                if (x != n - 1)
                    return false;
            }
            return true;
        }

        public static BigInteger GetPrime(BigInteger minValue, BigInteger maxValue)
        {
            var r = GetRandomBigInt(minValue, maxValue);
            var k = (BigInteger)BigInteger.Log(r, 2);
            if ((r & 1) == 0) r++;
            while (!MillerRabinTest(r, k))
            {
                r += 2;
            }
            return r;
        }

        public static BigInteger[] GetPG(BigInteger minValue, BigInteger maxValue)
        {
            BigInteger p, g, q;
            do
            {
                do
                {
                    q = GetPrime((minValue - 1) / 2, (maxValue - 1) / 2);
                    p = 2 * q + 1;
                } while (!MillerRabinTest(p, (BigInteger)BigInteger.Log(p, 2)));

                g = GetRandomBigInt(2, p - 1);

            } while (ModPow(g, q, p) == 1);

            return new[] { p, g };
        }

      //  public static BigInteger[] 
    }
}
