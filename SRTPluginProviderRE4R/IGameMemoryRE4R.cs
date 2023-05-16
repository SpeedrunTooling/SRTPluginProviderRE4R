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

        int InventoryCount { get; }

        CaseSize CaseSize { get; }

        InventoryEntry[] Items { get; }

        int InventoryCountAshley { get; }

        CaseSize CaseSizeAshley { get; }

        InventoryEntry[] ItemsAshley { get; }

        PlayerContext[] Enemies { get; }

        HitPoint PlayerHealth { get; }

        int EnemyArraySize { get; }

        GameRankSystem Rank { get; }

        GameStatsChapterLapTimeElement GameStatsChapterLapTimeElement { get; }

        GameStatsKillCountElement GameStatsKillCountElement { get; }
    }
}
