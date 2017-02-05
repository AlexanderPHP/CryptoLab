using System;
using System.IO;

using Crypto.Basics;
using Crypto.Ciphers;

namespace Crypto.Signatures
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                // RSA
                RSAPublicKey rsaPublicKey;
                RSAPrivateKey rsaPrivateKey;
                Ciphers.RSA.GenerateKeys(1000, 3000, out rsaPublicKey, out rsaPrivateKey);
                var fileToSign = new FileInfo("text.txt");
                RSA.SignFile(fileToSign, rsaPublicKey, rsaPrivateKey);
                var fileRSASignature = new FileInfo(fileToSign.Name + "_rsasign");
                Console.WriteLine("rsa {0}", RSA.CheckFileSignature(fileToSign, fileRSASignature, rsaPublicKey) ? "vsyo okey" : "data was being corrupted");

                // ElGamal
                var pg = Core.GetPG(1000, 3000);
                var elGamalKey = Ciphers.ElGamal.GenerateKeys(pg);
                ElGamal.SignFile(fileToSign, elGamalKey);
                var fileElGamalSignature = new FileInfo(fileToSign.Name + "_elgamalsign");
                Console.WriteLine("elgamal {0}", ElGamal.CheckFileSignature(fileToSign, fileElGamalSignature, elGamalKey) ? "vsyo okey" : "data was being corrupted");

                // GOST
                GOSTPublicKey gostPublicKey;
                GOSTSecretKey gostSecretKey;
                GOST.GenerateKeys(out gostPublicKey, out gostSecretKey);
                GOST.SignFile(fileToSign, gostPublicKey, gostSecretKey);
                var fileGOSTSignature = new FileInfo(fileToSign.Name + "_gostsign");
                Console.WriteLine("gost {0}", GOST.CheckFileSignature(fileToSign, fileGOSTSignature, gostPublicKey) ? "vsyo okey" : "data was being corrupted");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
