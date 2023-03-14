using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace SRTPluginProviderRE4R
{
    /// <summary>
    /// SHA256 hashes for the RE4 REmake game executables.
    /// 
    /// Resident Evil 4 (WW): https://steamdb.info/app/2050650/ / ???
    /// Biohazard 4 (CERO Z): ??? / ???
    /// </summary>
    public static class GameHashes
    {
        private static readonly byte[] re4rDEMO_20230309_1 = new byte[32] { 0xBA, 0x9D, 0xC7, 0x4A, 0xC8, 0x52, 0x30, 0x62, 0x12, 0xB2, 0xDD, 0x17, 0x93, 0xA8, 0xB6, 0xFD, 0x09, 0x2A, 0x62, 0xD5, 0x03, 0xFC, 0x87, 0x30, 0x59, 0xD2, 0x62, 0x50, 0xAE, 0x73, 0x0E, 0xAC };
        
        public static GameVersion DetectVersion(string filePath)
        {
            byte[] checksum;
            using (SHA256 hashFunc = SHA256.Create())
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                checksum = hashFunc.ComputeHash(fs);

            if (checksum.SequenceEqual(re4rDEMO_20230309_1))
                return GameVersion.RE4R_Demo_20230309_1;
            else
                return GameVersion.Unknown;
        }
    }
}
