using SRTPluginProducerRE4R.Structs;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace SRTPluginProducerRE4R
{
    public struct GameMemoryRE4R : IGameMemoryRE4R
    {
        private const string IGT_TIMESPAN_STRING_FORMAT = @"hh\:mm\:ss";

        public string GameName => "RE4R";

        public string VersionInfo => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        public bool IsInGameShopOpen { get => _isInGameShopOpen; set => _isInGameShopOpen = value; }
        internal bool _isInGameShopOpen;

        private ChapterID ChapterId { get => (ChapterID)_chapterId; set => _chapterId = (int)value; }
        internal int _chapterId;

        public string CurrentChapter => ChapterId.ToString();

        public GameTimer Timer { get => _timer; set => _timer = value; }
        internal GameTimer _timer;

        public PlayerContext PlayerContext { get => _playerContext; }
        internal PlayerContext _playerContext;

        public PlayerContext[] PartnerContext { get => _partnerContext; }
        internal PlayerContext[] _partnerContext;

        public int PTAS { get => _ptas; }
        internal int _ptas;

        public int Spinel { get => _spinel; }
        internal int _spinel;

        public int LastItem { get => _lastItem; }
        internal int _lastItem;

        public string LastItemName { get => ((ItemID)_lastItem).ToString(); }

        public int InventoryCount { get => _inventoryCount; }
        internal int _inventoryCount;

        public CaseSize CaseSize { get => _caseSize; }
        internal CaseSize _caseSize;

        public InventoryEntry[] Items { get => _items; set => _items = value; }
        internal InventoryEntry[] _items;

        public int KeyItemCount { get => _keyItemCount; }
        internal int _keyItemCount;

        public InventoryEntry[] KeyItems { get => _keyItems; set => _keyItems = value; }
        internal InventoryEntry[] _keyItems;

        public int TreasureItemsCount { get => _treasureItemsCount; }
        internal int _treasureItemsCount;

        public InventoryEntry[] TreasureItems { get => _treasureItems; set => _treasureItems = value; }
        internal InventoryEntry[] _treasureItems;

        public int UniqueCount { get => _uniqueCount; }
        internal int _uniqueCount;

        public InventoryEntry[] UniqueItems { get => _uniqueItems; set => _uniqueItems = value; }
        internal InventoryEntry[] _uniqueItems;

        public PlayerContext[] Enemies { get => _enemies; }
        internal PlayerContext[] _enemies;

        public int EnemyArraySize { get => enemyArraySize; }
        internal int enemyArraySize;

        public GameRankSystem Rank { get => rank; }
        internal GameRankSystem rank;

        public GameStatsChapterLapTimeElement GameStatsChapterLapTimeElement { get => gameStatsChapterLapTimeElement; }
        internal GameStatsChapterLapTimeElement gameStatsChapterLapTimeElement;

        public GameStatsKillCountElement GameStatsKillCountElement { get => gameStatsKillCountElement; }
        internal GameStatsKillCountElement gameStatsKillCountElement;
    }
}
