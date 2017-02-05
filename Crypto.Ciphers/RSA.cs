using System;
using System.IO;
using System.Numerics;

using Crypto.Basics;

namespace Crypto.Ciphers
{
    public class RSA
    {
        public static void GenerateKeys(BigInteger min, BigInteger max, out RSAPublicKey publicKey, out RSAPrivateKey privateKey)
        {
            var p = Core.GetPrime(min, max);
            var q = Core.GetPrime(min, max);
            var n = p * q;
            var phi = (p - 1) * (q - 1);
            var d = Core.GetRandomBigInt(0, phi);
            if (d.IsEven)
            {
                d++;
            }

            while (Core.Gcd(d, phi) != 1)
            {
                d += 2;
            }

            var dInv = Core.ExtendedEuclid(d, phi)[1];
            var c = dInv < 0 ? phi + dInv : dInv;
            publicKey = new RSAPublicKey { D = d, N = n };
            privateKey = new RSAPrivateKey { C = c, P = p, Phi = phi, Q = q };
        }
        public static BigInteger Encryptor(RSAPublicKey publicKey, BigInteger data)
        {
            if (publicKey.N < data)
            {
                throw new ArgumentException(nameof(data));
            }

            return Core.ModPow(data, publicKey.D, publicKey.N);
        }
        public static BigInteger Decryptor(RSAPublicKey publicKey, RSAPrivateKey privateKey, BigInteger data)
        {
            if (publicKey.N < data)
            {
                throw new ArgumentException(nameof(data));
            }

            return Core.ModPow(data, privateKey.C, publicKey.N);
        }
        public static void FileEncryptor(string inputFile, string outputFile, RSAPublicKey publicKey)
        {
            using (var binaryReader = new BinaryReader(File.Open(inputFile, FileMode.Open)))
            {
                using (var binaryWriter = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
                {
                    while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                    {
                        var encryptedData = Encryptor(publicKey, binaryReader.ReadByte());
                        binaryWriter.Write(encryptedData.ToString());
                    }
                }
            }
        }
        public static void FileDecryptor(string inputFile, string outputFile, RSAPrivateKey privateKey, RSAPublicKey publicKey)
        {
            using (var binaryReader = new BinaryReader(File.Open(inputFile, FileMode.Open)))
            {
                using (var binaryWriter = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
                {
                    while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                    {
                        var encryptedData = BigInteger.Parse(binaryReader.ReadString());
                        var decryptedData = Decryptor(publicKey, privateKey, encryptedData).ToByteArray()[0];
                        binaryWriter.Write(decryptedData);
                    }
                }
            }
        }
    }

    public struct RSAPrivateKey
    {
        public BigInteger C;
        public BigInteger Q;
        public BigInteger P;
        public BigInteger Phi;
    }

    public struct RSAPublicKey
    {
        public BigInteger N;
        public BigInteger D;
    }
}
