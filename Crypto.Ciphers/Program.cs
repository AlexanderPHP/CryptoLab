using System;
using Crypto.Basics;

namespace Crypto.Ciphers
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                //Shamir
                var p = 98764321261;
                var m1 = 3453453454;
                var aliceKey = Shamir.GenerateKeys(p);
                var bobKey = Shamir.GenerateKeys(p);
                var x1 = Core.ModPow(m1, aliceKey.E, p);
                var x2 = Core.ModPow(x1, bobKey.E, p);
                var x3 = Core.ModPow(x2, aliceKey.D, p);
                var x4 = Core.ModPow(x3, bobKey.D, p);
                Console.WriteLine($"Shamir encryption/decryption test: {x4 == m1}");
                //ElGamal
                var pg = Core.GetPG(1000, 2000);
                // Console.WriteLine($"P:{pg.P} G:{pg.G}");
                var elGamalKey = ElGamal.GenerateKeys(pg);
                var m2 = 945;
                var elGamalEncryptedData = ElGamal.Encryptor(elGamalKey, m2);
                //Console.WriteLine($"e:{s.E} r:{s.R}");
                var elGamalDecryptedData = ElGamal.Decryptor(elGamalKey, elGamalEncryptedData);
                Console.WriteLine($"ElGamal encryption/decryption test: {elGamalDecryptedData == m2}");
                ElGamal.FileEncryptor("image.jpg", "image.elgamal", elGamalKey);
                ElGamal.FileDecryptor("image.elgamal", "image_elgamal.jpg", elGamalKey);
                //RSA
                RSAPublicKey publicKey;
                RSAPrivateKey privateKey;
                RSA.GenerateKeys(1000, 3000, out publicKey, out privateKey);
                var m3 = 945;
                var rsaEncryptedData = RSA.Encryptor(publicKey, m3);
                var rsaDecryptedData = RSA.Decryptor(publicKey, privateKey, rsaEncryptedData);
                Console.WriteLine($"RSA encryption/decryption test: {m3 == rsaDecryptedData}");
                RSA.FileEncryptor("image.jpg", "image.rsa", publicKey);
                RSA.FileDecryptor("image.rsa", "image_rsa.jpg", privateKey, publicKey);
                //Vernam
                Vernam.FileEncryptor("image.jpg", "image.vernam", "vernam.key");
                Vernam.FileDecryptor("image.vernam", "image_vernam.jpg", "vernam.key");
                Console.WriteLine($"Vernam encryption/decryption completed");
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
