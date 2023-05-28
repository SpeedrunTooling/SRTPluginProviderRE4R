using SRTPluginProducerRE4R.Structs;

namespace SRTPluginProducerRE4R
{
    public struct GameMemoryRE4R : IGameMemoryRE4R
    {
        private const string IGT_TIMESPAN_STRING_FORMAT = @"hh\:mm\:ss";

        public string GameName => "RE4R";

        public string VersionInfo => new PluginInfo().Version.ToString();

        public bool IsInGameShopOpen { get => isInGameShopOpen; set => isInGameShopOpen = value; }
        internal bool isInGameShopOpen;

		public bool IsNewGame { get => isNewGame; set => isNewGame = value; }
		internal bool isNewGame;

		private ChapterID ChapterId { get => (ChapterID)chapterId; set => chapterId = (int)value; }
        internal int chapterId;

        public string CurrentChapter => ChapterId.ToString();

		public long TimerOffset { get => timerOffset; set => timerOffset = value; }
		internal long timerOffset;
		
        public GameTimer? Timer { get => timer; set => timer = value; }
        internal GameTimer? timer;

        public PlayerContext? PlayerContext { get => playerContext; }
        internal PlayerContext? playerContext;

        public PlayerContext?[]? PartnerContext { get => partnerContext; }
        internal PlayerContext?[]? partnerContext;

        public int PTAS { get => ptas; }
        internal int ptas;

        public int Spinel { get => spinel; }
        internal int spinel;

        public int LastItem { get => lastItem; }
        internal int lastItem;

        public string LastItemName { get => ((ItemID)lastItem).ToString(); }

        public int InventoryCount { get => inventoryCount; }
        internal int inventoryCount;

        public CaseSize CaseSize { get => caseSize; }
        internal CaseSize caseSize;

        public InventoryEntry[]? Items { get => items; set => items = value; }
        internal InventoryEntry[]? items;

        public int KeyItemCount { get => keyItemCount; }
        internal int keyItemCount;

        public InventoryEntry[]? KeyItems { get => keyItems; set => keyItems = value; }
        internal InventoryEntry[]? keyItems;

        public int TreasureItemsCount { get => treasureItemsCount; }
        internal int treasureItemsCount;

        public InventoryEntry[]? TreasureItems { get => treasureItems; set => treasureItems = value; }
        internal InventoryEntry[]? treasureItems;

        public int UniqueCount { get => uniqueCount; }
        internal int uniqueCount;

        public InventoryEntry[]? UniqueItems { get => uniqueItems; set => uniqueItems = value; }
        internal InventoryEntry[]? uniqueItems;

        public PlayerContext?[]? Enemies { get => enemies; }
        internal PlayerContext?[]? enemies;

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
