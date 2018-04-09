using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DataProtector
{
    class SecureDataProtector
    {
        public static byte[] GenerateEntropy()
        {
            var entropy = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(entropy);

            return entropy;
        }

        public static byte[] GenerateEntropy(int length)
        {
            var entropy = new byte[length];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(entropy);
            return entropy;
        }

        public static long ProtectDataToFile(byte[] Data, string FilePath, string EntropyFilePath)
        {
            byte[] entropy = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(entropy);

            var encryptedData = ProtectedData.Protect(Data, entropy, DataProtectionScope.CurrentUser);
            long Length = encryptedData.Length;      

            using (var BinWriter = new BinaryWriter(File.Create(EntropyFilePath)))
            {
                BinWriter.Write(entropy);
            }

            using (var BinWriter = new BinaryWriter(File.Open(FilePath, FileMode.Append)))
            {
                BinWriter.Write(encryptedData);
            }

            return Length;
        }

        public static long ProtectDataToFile(byte[] Data, string FilePath, byte[] Entropy)
        {

            var encryptedData = ProtectedData.Protect(Data, Entropy, DataProtectionScope.CurrentUser);
            long Length = encryptedData.Length;

            return Length;
        }

        public static byte[] UnprotectDataFromFile(byte[] Entropy, string FilePath, long Length)
        {
            var data = new byte[Length];
            using (var BinReader = new BinaryReader(File.Open(FilePath, FileMode.Open)))
            {
                BinReader.ReadInt32();
                data = BinReader.ReadBytes((int)Length);
            }

            return ProtectedData.Unprotect(data, Entropy, DataProtectionScope.CurrentUser);
        }

        public static byte[] ExtractEntropyFromFile(string FilePath, long Length) => throw new NotImplementedException();
    }
}
