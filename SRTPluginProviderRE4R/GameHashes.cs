using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SRTPluginProviderRE4R
{
    /// <summary>
    /// SHA256 hashes for the RE4 REmake game executables.
    /// 
    /// Resident Evil 4 (WW): https://steamdb.info/app/2050650/ / https://steamdb.info/depot/2050651/
    /// Biohazard 4 (CERO Z): ??? / ???
    /// </summary>
    public static class GameHashes
    {
        private static readonly byte[] re4rWW_11025382 = new byte[32] { 0x3E, 0xC3, 0x90, 0x1D, 0xD4, 0xF7, 0x0C, 0x7E, 0x03, 0xDB, 0xE3, 0x13, 0xA4, 0x7A, 0xDB, 0xC1, 0x10, 0x94, 0xD9, 0xAE, 0x34, 0xB6, 0xF3, 0x5E, 0xF9, 0xA0, 0x8C, 0x4F, 0x57, 0x32, 0x6D, 0x1E };
        private static readonly byte[] re4rWW_20230407_1 = new byte[32] { 0x7A, 0x0A, 0x58, 0xB6, 0x00, 0x35, 0xCF, 0xBB, 0x73, 0xDD, 0x4F, 0x22, 0x16, 0x38, 0xD2, 0x6E, 0x50, 0xC8, 0xBF, 0xE6, 0xFC, 0x68, 0x77, 0x6B, 0x1F, 0x05, 0x1F, 0x33, 0x4A, 0x10, 0x00, 0x5C };
        private static readonly byte[] re4rWW_20230323_1 = new byte[32] { 0x3C, 0x81, 0x07, 0xE9, 0x35, 0x2D, 0x0C, 0x4F, 0x39, 0x3D, 0x37, 0x50, 0xF0, 0xAC, 0x86, 0x62, 0x39, 0x1D, 0x52, 0x55, 0x9E, 0x94, 0xB5, 0x86, 0x73, 0x70, 0x50, 0xE0, 0xC5, 0x5E, 0xC3, 0x18 };
        private static readonly byte[] re4rDEMO_20230309_1 = new byte[32] { 0xBA, 0x9D, 0xC7, 0x4A, 0xC8, 0x52, 0x30, 0x62, 0x12, 0xB2, 0xDD, 0x17, 0x93, 0xA8, 0xB6, 0xFD, 0x09, 0x2A, 0x62, 0xD5, 0x03, 0xFC, 0x87, 0x30, 0x59, 0xD2, 0x62, 0x50, 0xAE, 0x73, 0x0E, 0xAC };

        private static void OutputVersionString(byte[] cs)
        {
            StringBuilder sb = new StringBuilder("private static readonly byte[] re4r??_00000000 = new byte[32] { ");

            for (int i = 0; i < cs.Length; i++)
            {
                sb.AppendFormat("0x{0:X2}", cs[i]);

                if (i < cs.Length - 1)
                {
                    sb.Append(", ");
                }
            }

            sb.Append(" }");
            Console.WriteLine("Please DM VideoGameRoulette or Squirrelies with the version.log");
            // write output to file
            string filename = "version.log";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine(sb.ToString());
            }
        }

        public static GameVersion DetectVersion(string filePath)
        {
            byte[] checksum;
            using (SHA256 hashFunc = SHA256.Create())
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                checksum = hashFunc.ComputeHash(fs);

            if (checksum.SequenceEqual(re4rWW_11025382))
                return GameVersion.RE4R_WW_11025382;
            else if (checksum.SequenceEqual(re4rWW_20230407_1))
                return GameVersion.RE4R_WW_20230407_1;
            else if (checksum.SequenceEqual(re4rWW_20230323_1))
                return GameVersion.RE4R_WW_20230323_1;
            else if (checksum.SequenceEqual(re4rDEMO_20230309_1))
                return GameVersion.RE4R_Demo_20230309_1;
            else
            {
                Console.WriteLine("Unknown Version");
                OutputVersionString(checksum);
                return GameVersion.Unknown;
            }
        }
    }
}
