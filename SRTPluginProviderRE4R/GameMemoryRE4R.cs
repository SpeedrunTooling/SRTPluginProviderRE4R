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

        public SystemSaveData SystemSaveData { get => systemSaveData; }
        internal SystemSaveData systemSaveData;

        public GameSaveData GameSaveData { get => gameSaveData; }
        internal GameSaveData gameSaveData;

        // Public Properties - Calculated
        public long IGTCalculated => 0L;//unchecked((long)(GameStatsChapterLapTimeElement.Time < 0UL ? 0UL : (GameStatsChapterLapTimeElement.Time - GameSaveData.GameElapsedTime - GameSaveData.DemoSpendingTime - GameSaveData.PauseSpendingTime)));

        public long IGTCalculatedTicks => unchecked(IGTCalculated * 10L);

        public TimeSpan IGTTimeSpan
        {
            get
            {
                TimeSpan timespanIGT;

                if (IGTCalculatedTicks <= TimeSpan.MaxValue.Ticks)
                    timespanIGT = new TimeSpan(IGTCalculatedTicks);
                else
                    timespanIGT = new TimeSpan();

                return timespanIGT;
            }
        }

        public string IGTFormattedString => IGTTimeSpan.ToString(IGT_TIMESPAN_STRING_FORMAT, CultureInfo.InvariantCulture);
    }
}
