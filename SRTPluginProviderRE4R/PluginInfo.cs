using SRTPluginBase;
using System;

namespace SRTPluginProviderRE4R
{
    internal class PluginInfo : IPluginInfo
    {
        public string Name => "Game Memory Producer (Resident Evil 4 (2023))";

        public string Description => "A game memory producer plugin for Resident Evil 4 (2023).";

        public string Author => "TheDementedSalad";

        public Uri MoreInfoURL => new Uri("https://github.com/SpeedrunTooling/SRTPluginProviderRE4R");

        public int VersionMajor => assemblyVersion.Major;

        public int VersionMinor => assemblyVersion.Minor;

        public int VersionBuild => assemblyVersion.Build;

        public int VersionRevision => assemblyVersion.Revision;

        private Version assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
    }
}
