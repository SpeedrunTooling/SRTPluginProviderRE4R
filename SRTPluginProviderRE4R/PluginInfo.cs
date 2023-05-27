using SRTPluginBase;
using System;
using System.Diagnostics;
using System.IO;

namespace SRTPluginProducerRE4R
{
    internal class PluginInfo : PluginInfoBase, IPluginInfo
    {
        public override string Name => "Game Memory Producer (Resident Evil 4 (2023))";

        public override string Description => "A game memory producer plugin for Resident Evil 4 (2023).";

        public override string Author => "TheDementedSalad, Squirrelies, VideoGameRoulette";

        public override Uri MoreInfoURL => new Uri("https://github.com/SpeedrunTooling/SRTPluginProducerRE4R");

        public override int VersionMajor => GetProductVersion().Major;

        public override int VersionMinor => GetProductVersion().Minor;

        public override int VersionBuild => GetProductVersion().Build;

        public override int VersionRevision => GetProductVersion().Revision;
    }
}
