using System.IO;
using System.Numerics;
using System.Security.Cryptography;

using Crypto.Basics;

namespace Crypto.Signatures
{
    public class GOST
    {
        public static void GenerateKeys(out GOSTPublicKey publicKey, out GOSTSecretKey secretKey)
        {
            BigInteger g, a, p;
            var q = Core.GetPrime(16384, 32767);

            do
            {
                var b = Core.GetRandomBigInt(10, 100);
                p = b * q + 1;
            } while (!Core.MillerRabinTest(p, 10));

            do
            {
                g = Core.GetRandomBigInt(2, p - 1);
                a = Core.ModPow(g, (p - 1) / q, p);
            } while (Core.ModPow(a, q, p) != 1);

            var x = Core.GetRandomBigInt(1, q);
            var y = Core.ModPow(a, x, p);
            publicKey = new GOSTPublicKey { P = p, Q = q, Y = y, A = a };
            secretKey = new GOSTSecretKey { X = x };
            //publicKey = new GOSTPublicKey { P = 60037, Q = 5003, Y = 45615, A = 24977 };
            //secretKey = new GOSTSecretKey { X = 2168 };
        }

        public static void SignFile(FileInfo file, GOSTPublicKey publicKey, GOSTSecretKey secretKey)
        {
            using (var fileStream = file.OpenRead())
            {
                using (var signatureStream = new BinaryWriter(File.Open(file.Name + "_gostsign", FileMode.Create)))
                {
                    using (var md5 = MD5.Create())
                    {
                        foreach (var hashByte in md5.ComputeHash(fileStream))
                        {
                            BigInteger k, r, s;
                            do
                            {
                                do
                                {
                                    //k = 2438;
                                    k = Core.GetRandomBigInt(1, publicKey.Q);
                                    r = Core.ModPow(publicKey.A, k, publicKey.P) % publicKey.Q;
                                } while (r == 0);
                                s = (k * hashByte + secretKey.X * r) % publicKey.Q;
                                // s = (k * 38605 + secretKey.X * r) % publicKey.Q;
                            } while (s == 0);
                            signatureStream.Write(r.ToString());
                            signatureStream.Write(s.ToString());
                            //break;
                        }
                    }
                }
            }
        }

        public static bool CheckFileSignature(FileInfo file, FileInfo sign, GOSTPublicKey publicKey)
        {
            using (var fileStream = file.OpenRead())
            {
                using (var signatureStream = new BinaryReader(sign.OpenRead()))
                {
                    using (var md5 = MD5.Create())
                    {
                        foreach (var hashByte in md5.ComputeHash(fileStream))
                        {
                            var r = BigInteger.Parse(signatureStream.ReadString());
                            var s = BigInteger.Parse(signatureStream.ReadString());
                            if (0 > r || r > publicKey.Q) return false;
                            if (0 > s || s > publicKey.Q) return false;
                            //var hashByteInv = Lab1.ExtendedEuclid(38605, publicKey.Q)[1];
                            var hashByteInv = Core.ExtendedEuclid(hashByte, publicKey.Q)[1];
                            if (hashByteInv < 0) hashByteInv += publicKey.Q;
                            var u1 = s * hashByteInv % publicKey.Q;
                            var u2 = -r * hashByteInv % publicKey.Q;
                            if (u2 < 0) u2 += publicKey.Q;
                            var v = Core.ModPow(publicKey.A, u1, publicKey.P) *
                                    Core.ModPow(publicKey.Y, u2, publicKey.P) % publicKey.P % publicKey.Q;
                            if (r != v) return false;

                            //var lhs = Lab1.ModPow(publicKey.Y, r, publicKey.P) * Lab1.ModPow(r, s, publicKey.P) % publicKey.P;
                            //var rhs = Lab1.ModPow(publicKey.G, hashByte, publicKey.P);
                            //if (lhs != rhs) return false;
                        }
                    }
                }
            }
            return true;
        }
    }


    public struct GOSTSecretKey
    {
        public BigInteger X;
    }

    public struct GOSTPublicKey
    {
        public BigInteger Y;
        public BigInteger P;
        public BigInteger Q;
        public BigInteger A;
    }
}
