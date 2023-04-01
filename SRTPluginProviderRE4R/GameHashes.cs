using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

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
        private static readonly byte[] re4rWW_20230323_1 = new byte[32] { 0x3C, 0x81, 0x07, 0xE9, 0x35, 0x2D, 0x0C, 0x4F, 0x39, 0x3D, 0x37, 0x50, 0xF0, 0xAC, 0x86, 0x62, 0x39, 0x1D, 0x52, 0x55, 0x9E, 0x94, 0xB5, 0x86, 0x73, 0x70, 0x50, 0xE0, 0xC5, 0x5E, 0xC3, 0x18 };
        private static readonly byte[] re4rDEMO_20230309_1 = new byte[32] { 0xBA, 0x9D, 0xC7, 0x4A, 0xC8, 0x52, 0x30, 0x62, 0x12, 0xB2, 0xDD, 0x17, 0x93, 0xA8, 0xB6, 0xFD, 0x09, 0x2A, 0x62, 0xD5, 0x03, 0xFC, 0x87, 0x30, 0x59, 0xD2, 0x62, 0x50, 0xAE, 0x73, 0x0E, 0xAC };

        public static GameVersion DetectVersion(string filePath)
        {
            byte[] checksum;
            using (SHA256 hashFunc = SHA256.Create())
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                checksum = hashFunc.ComputeHash(fs);

            if (checksum.SequenceEqual(re4rWW_20230323_1))
                return GameVersion.RE4R_WW_20230323_1;
            else if (checksum.SequenceEqual(re4rDEMO_20230309_1))
                return GameVersion.RE4R_Demo_20230309_1;
            else
                return GameVersion.Unknown;
        }
    }
}
