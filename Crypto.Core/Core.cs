using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;

namespace Crypto.Basics
{
    public class Core
    {
        public static BigInteger ModPow(BigInteger a, BigInteger b, BigInteger p)
        {
            if (b < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(b));
            }

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

        public static BigInteger Gcd(BigInteger a, BigInteger b)
        {
            while (b != 0)
            {
                b = a % (a = b);
            }

            return a;
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
            var rng = new RNGCryptoServiceProvider();
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
            {
                return false;
            }
            if (n != 2 && n % 2 == 0)
            {
                return false;
            }
            var s = n - 1;
            while (s % 2 == 0)
            {
                s >>= 1;
            }
            for (var i = 0; i < k; i++)
            {
                var a = GetRandomBigInt(2, n - 1);
                var temp = s;
                var mod = ModPow(a, temp, n);
                while (temp != n - 1 && mod != 1 && mod != n - 1)
                {
                    mod = (mod * mod) % n;
                    temp = temp * 2;
                }
                if (mod != n - 1 && temp % 2 == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static BigInteger GetPrime(BigInteger minValue, BigInteger maxValue)
        {
            var r = GetRandomBigInt(minValue, maxValue);
            var k = (BigInteger)BigInteger.Log(r, 2);
            if ((r & 1).IsZero)
            {
                r++;
            }

            while (!MillerRabinTest(r, k))
            {
                r += 2;
            }
            return r;
        }

        public static PG GetPG(BigInteger minValue, BigInteger maxValue)
        {
            BigInteger p, g, q;
            do
            {
                q = GetPrime((minValue - 1) / 2, (maxValue - 1) / 2);
                p = 2 * q + 1;
            } while (!MillerRabinTest(p, (BigInteger)BigInteger.Log(p, 2)));
            do
            {
                g = GetRandomBigInt(2, p - 1);
            } while (ModPow(g, q, p).IsOne);

            return new PG
            {
                P = p,
                G = g
            };
        }

        public static BigInteger BabyStepGiantStep(BigInteger y, BigInteger a, BigInteger p)
        {
            BigInteger i, j;
            var bs = new Dictionary<BigInteger, BigInteger>();
            var m = (BigInteger)(Math.Sqrt((double)p) + 1);
            y %= p;

            for (j = 0; j < m; j++)
            {
                bs.Add(ModPow(a, j, p) * y % p, j);
            }

            for (i = 1; i <= m; i++)
            {
                BigInteger value;
                if (!bs.TryGetValue(ModPow(a, i * m, p), out value))
                {
                    continue;
                }

                j = value;
                break;
            }

            return i * m - j;
        }
    }

    public class PG
    {
        public BigInteger P;
        public BigInteger G;
    }
}
