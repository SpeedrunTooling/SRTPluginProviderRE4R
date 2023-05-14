using SRTPluginProviderRE4R.Structs;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace SRTPluginProviderRE4R
{
    public struct GameMemoryRE4R : IGameMemoryRE4R
    {
        private const string IGT_TIMESPAN_STRING_FORMAT = @"hh\:mm\:ss";

        public string GameName => "RE4R";

        public string VersionInfo => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        public GameTimer Timer { get => _timer; set => _timer = value; }
        internal GameTimer _timer;

        public PlayerContext PlayerContext { get => _playerContext; }
        internal PlayerContext _playerContext;
        public PlayerContext[] Enemies { get => _enemies; }
        internal PlayerContext[] _enemies;

        public HitPoint PlayerHealth { get => playerHealth; }
        internal HitPoint playerHealth;

        public int EnemyArraySize { get => enemyArraySize; }
        internal int enemyArraySize;

        public HitPoint[] EnemyHealth { get => enemyHealth; }
        internal HitPoint[] enemyHealth;

        public GameRankSystem Rank { get => rank; }
        internal GameRankSystem rank;

        public GameStatsChapterLapTimeElement GameStatsChapterLapTimeElement { get => gameStatsChapterLapTimeElement; }
        internal GameStatsChapterLapTimeElement gameStatsChapterLapTimeElement;

        public GameStatsKillCountElement GameStatsKillCountElement { get => gameStatsKillCountElement; }
        internal GameStatsKillCountElement gameStatsKillCountElement;
    }
}
