using SRTPluginProviderRE4R.Structs;
using System;

namespace SRTPluginProviderRE4R
{
    public interface IGameMemoryRE4R
    {
        string GameName { get; }

        string VersionInfo { get; }

        GameTimer Timer { get; }

        PlayerContext PlayerContext { get; }
        PlayerContext[] Enemies { get; }

        HitPoint PlayerHealth { get; }

        int EnemyArraySize { get; }

        HitPoint[] EnemyHealth { get; }

        GameRankSystem Rank { get; }

        GameStatsChapterLapTimeElement GameStatsChapterLapTimeElement { get; }

        GameStatsKillCountElement GameStatsKillCountElement { get; }
    }
}
