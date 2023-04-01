using SRTPluginProviderRE4R.Structs;
using System;

namespace SRTPluginProviderRE4R
{
    public interface IGameMemoryRE4R
    {
        string GameName { get; }

        string VersionInfo { get; }

        EntityHealth PlayerHealth { get; }

        int EnemyArraySize { get; }

        EntityHealth[] EnemyHealth { get; }

        int ChapterKillCount { get; }

        GameRank Rank { get; }

        long ChapterTimeStart { get; }

        GameTimer Timer { get; }

        long IGTCalculated { get; }

        long IGTCalculatedTicks { get; }

        TimeSpan IGTTimeSpan { get; }

        string IGTFormattedString { get; }
    }
}
