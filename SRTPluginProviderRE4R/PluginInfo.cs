using SRTPluginBase;
using System;

namespace SRTPluginProducerRE4R
{
	public class PluginInfo : PluginInfoBase, IPluginInfo
    {
        public override string Name => "Game Memory Producer (Resident Evil 4 (2023))";

        public override string Description => "A game memory producer plugin for Resident Evil 4 (2023).";

        public override string Author => "TheDementedSalad, Squirrelies, VideoGameRoulette";

        public override Uri MoreInfoURL => new Uri("https://github.com/SpeedrunTooling/SRTPluginProducerRE4R");

        public override int VersionMajor => Version.Major;

        public override int VersionMinor => Version.Minor;

        public override int VersionBuild => Version.Build;

        public override int VersionRevision => Version.Revision;
    }
}
