using SRTPluginBase;
using System;

namespace SRTPluginProducerRE4R
{
    internal class PluginInfo : IPluginInfo
    {
        public string Name => "Game Memory Producer (Resident Evil 4 (2023))";

        public string Description => "A game memory producer plugin for Resident Evil 4 (2023).";

        public string Author => "TheDementedSalad, Squirrelies, VideoGameRoulette";

        public Uri MoreInfoURL => new Uri("https://github.com/SpeedrunTooling/SRTPluginProducerRE4R");

        public int VersionMajor => assemblyVersion?.Major ?? 0;

        public int VersionMinor => assemblyVersion?.Minor ?? 0;

        public int VersionBuild => assemblyVersion?.Build ?? 0;

        public int VersionRevision => assemblyVersion?.Revision ?? 0;

        private readonly Version? assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        public bool Equals(IPluginInfo? other) => Equals(this, other);
    }
}
