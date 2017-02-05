using System;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;

using Crypto.Basics;
using Crypto.Ciphers;

namespace Crypto.Signatures
{
    public class ElGamal
    {
        public static void SignFile(FileInfo file, ElGamalKey key)
        {
            using (var fileStream = file.OpenRead())
            {
                using (var signatureStream = new BinaryWriter(File.Open(file.Name + "_elgamalsign", FileMode.Create)))
                {
                    using (var md5 = MD5.Create())
                    {
                        foreach (var hashByte in md5.ComputeHash(fileStream))
                        {
                            if (1 > hashByte || hashByte > key.P) throw new ArgumentException();
                            var k = Core.GetRandomBigInt(0, key.P - 1);
                            if (k.IsEven) k++;
                            while (Core.Gcd(k, key.P - 1) != 1) k += 2;
                            var r = Core.ModPow(key.G, k, key.P);
                            var u = (hashByte - key.X * r) % (key.P - 1);
                            if (u < 0) u += key.P - 1;
                            var kInv = Core.ExtendedEuclid(k, key.P - 1)[1];
                            if (kInv < 0) kInv += key.P - 1;
                            var s = kInv * u % (key.P - 1);
                            signatureStream.Write(r.ToString());
                            signatureStream.Write(s.ToString());
                        }
                    }
                }
            }
        }

        public static bool CheckFileSignature(FileInfo file, FileInfo sign, ElGamalKey key)
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
                            if (0 > r || r > key.P) return false;
                            if (0 > s || s > key.P - 1) return false;
                            var lhs = Core.ModPow(key.Y, r, key.P) * Core.ModPow(r, s, key.P) % key.P;
                            var rhs = Core.ModPow(key.G, hashByte, key.P);
                            if (lhs != rhs) return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
