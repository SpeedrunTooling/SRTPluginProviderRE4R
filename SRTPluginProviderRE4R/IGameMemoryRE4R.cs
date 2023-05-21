using SRTPluginProducerRE4R.Structs;
using System;

namespace SRTPluginProducerRE4R
{
    public interface IGameMemoryRE4R
    {
        string GameName { get; }

        string VersionInfo { get; }

        bool IsInGameShopOpen { get; }

        string CurrentChapter { get; }

        GameTimer Timer { get; }

        PlayerContext PlayerContext { get; }

        PlayerContext[] PartnerContext { get; }

        int LastItem { get; }

        int InventoryCount { get; }

        CaseSize CaseSize { get; }

        InventoryEntry[] Items { get; }

        int PTAS { get; }

        int Spinel { get; }

        int KeyItemCount { get; }

        InventoryEntry[] KeyItems { get; }

        int TreasureItemsCount { get; }

        InventoryEntry[] TreasureItems { get; }

        int UniqueCount { get; }

        InventoryEntry[] UniqueItems { get; }

        PlayerContext[] Enemies { get; }

        int EnemyArraySize { get; }

        GameRankSystem Rank { get; }

        GameStatsChapterLapTimeElement GameStatsChapterLapTimeElement { get; }

        GameStatsKillCountElement GameStatsKillCountElement { get; }
    }
}
