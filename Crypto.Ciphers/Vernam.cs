using System;
using System.IO;

namespace Crypto.Ciphers
{
    public class Vernam
    {
        public static byte VernamXor(byte key, byte data)
        {
            return (byte)(key ^ data);
        }
        public static void FileEncryptor(string inputFile, string outputFile, string keyFile)
        {
            using (var binaryReader = new BinaryReader(File.Open(inputFile, FileMode.Open)))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(outputFile, FileMode.Create)),
                    binaryWriterKey = new BinaryWriter(File.Open(keyFile, FileMode.Create)))
                {
                    var r = new Random();
                    while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                    {
                        var key = (byte)r.Next(255);
                        var encryptedData = VernamXor(key, binaryReader.ReadByte());
                        binaryWriter.Write(encryptedData);
                        binaryWriterKey.Write(key);
                    }
                }
            }
        }
        public static void FileDecryptor(string inputFile, string outputFile, string keyFile)
        {
            using (BinaryReader binaryReader = new BinaryReader(File.Open(inputFile, FileMode.Open)),
                 binaryReaderKey = new BinaryReader(File.Open(keyFile, FileMode.Open)))
            {
                using (var binaryWriter = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
                {
                    while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                    {
                        var encryptedData = binaryReader.ReadByte();
                        var key = binaryReaderKey.ReadByte();
                        var decryptedData = VernamXor(key, encryptedData);
                        binaryWriter.Write(decryptedData);
                    }
                }
            }
        }
    }
}
