using SRTPluginProviderRE4R.Structs;
using System;

namespace SRTPluginProviderRE4R
{
    public interface IGameMemoryRE4R
    {
        string GameName { get; }

        string VersionInfo { get; }

        HitPoint PlayerHealth { get; }

        int EnemyArraySize { get; }

        HitPoint[] EnemyHealth { get; }

        GameRankSystem Rank { get; }

        GameStatsChapterLapTimeElement GameStatsChapterLapTimeElement { get; }

        GameStatsKillCountElement GameStatsKillCountElement { get; }

        SystemSaveData SystemSaveData { get; }

        GameSaveData GameSaveData { get; }

        long IGTCalculated { get; }

        long IGTCalculatedTicks { get; }

        TimeSpan IGTTimeSpan { get; }

        string IGTFormattedString { get; }
    }
}
