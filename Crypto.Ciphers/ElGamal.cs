using System;
using System.IO;
using System.Numerics;

using Crypto.Basics;

namespace Crypto.Ciphers
{
    public class ElGamal
    {
        public static ElGamalKey GenerateKeys(PG pg)
        {
            var x = Core.GetRandomBigInt(1, pg.P - 1);
            var y = Core.ModPow(pg.G, x, pg.P);
            return new ElGamalKey
            {
                G = pg.G,
                X = x,
                P = pg.P,
                Y = y
            };
        }
        public static ElGamalEncryptedData Encryptor(ElGamalKey key, BigInteger data)
        {
            if (data > key.P)
            {
                throw new ArgumentException(nameof(data));
            }
            var k = Core.GetRandomBigInt(2, key.P - 2);
            var r = Core.ModPow(key.G, k, key.P);
            var e = Core.ModPow(key.Y, k, key.P) * data % key.P;
            return new ElGamalEncryptedData { R = r, E = e };
        }
        public static BigInteger Decryptor(ElGamalKey key, ElGamalEncryptedData encryptedData)
        {
            return Core.ModPow(encryptedData.R, key.P - 1 - key.X, key.P) * encryptedData.E % key.P;
        }

        public static void FileEncryptor(string inputFile, string outputFile, ElGamalKey key)
        {
            using (var binaryReader = new BinaryReader(File.Open(inputFile, FileMode.Open)))
            {
                using (var binaryWriter = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
                {
                    while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                    {
                        var encryptedData = Encryptor(key, binaryReader.ReadByte());
                        binaryWriter.Write(encryptedData.R.ToString());
                        binaryWriter.Write(encryptedData.E.ToString());
                    }
                }
            }
        }
        public static void FileDecryptor(string inputFile, string outputFile, ElGamalKey key)
        {
            using (var binaryReader = new BinaryReader(File.Open(inputFile, FileMode.Open)))
            {
                using (var binaryWriter = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
                {
                    while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                    {
                        var r = BigInteger.Parse(binaryReader.ReadString());
                        var e = BigInteger.Parse(binaryReader.ReadString());
                        var encryptedData = new ElGamalEncryptedData { R = r, E = e };
                        var decryptedData = Decryptor(key, encryptedData).ToByteArray()[0];
                        binaryWriter.Write(decryptedData);
                    }
                }
            }
        }
    }

    public struct ElGamalKey
    {
        public BigInteger P;
        public BigInteger G;
        public BigInteger X;
        public BigInteger Y;
    }

    public struct ElGamalEncryptedData
    {
        public BigInteger R;
        public BigInteger E;
    }

}
