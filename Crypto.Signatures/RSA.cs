using System.IO;
using System.Numerics;
using System.Security.Cryptography;

using Crypto.Basics;
using Crypto.Ciphers;

namespace Crypto.Signatures
{
    public class RSA
    {
        public static void SignFile(FileInfo file, RSAPublicKey publicKey, RSAPrivateKey privateKey)
        {
            using (var fileStream = file.OpenRead())
            {
                using (var signatureStream = new BinaryWriter(File.Open(file.Name + "_rsasign", FileMode.Create)))
                {
                    using (var md5 = MD5.Create())
                    {
                        foreach (var hashByte in md5.ComputeHash(fileStream))
                        {
                            var s = Core.ModPow(hashByte, privateKey.C, publicKey.N);
                            signatureStream.Write(s.ToString());
                        }
                    }
                }
            }
        }
        public static bool CheckFileSignature(FileInfo file, FileInfo sign, RSAPublicKey publicKey)
        {
            using (var fileStream = file.OpenRead())
            {
                using (var signatureStream = new BinaryReader(sign.OpenRead()))
                {
                    using (var md5 = MD5.Create())
                    {
                        var any = true;
                        foreach (var hashByte in md5.ComputeHash(fileStream))
                        {
                            var encryptedHashByte = BigInteger.Parse(signatureStream.ReadString());
                            var w = Core.ModPow(encryptedHashByte, publicKey.D, publicKey.N);
                            if (hashByte != w.ToByteArray()[0])
                            {
                                any = false;
                                break;
                            }
                        }
                        return any;
                    }
                }
            }
        }
    }
}
