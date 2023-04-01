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

        public EntityHealth PlayerHealth { get => playerHealth; }
        internal EntityHealth playerHealth;

        public int EnemyArraySize { get => enemyArraySize; }
        internal int enemyArraySize;

        public EntityHealth[] EnemyHealth { get => enemyHealth; }
        internal EntityHealth[] enemyHealth;

        public int ChapterKillCount { get => chapterKillCount; }
        internal int chapterKillCount;

        public GameRank Rank { get => rank; }
        internal GameRank rank;

        public long ChapterTimeStart { get => chapterTimeStart; }
        internal long chapterTimeStart;

        public GameTimer Timer { get => timer; }
        internal GameTimer timer;

        // Public Properties - Calculated
        public long IGTCalculated => 0L;//unchecked(ChapterTimeStart < 0 ? 0L : (ChapterTimeStart - Timer.IGTRunningTimer - Timer.IGTCutsceneTimer - Timer.IGTPausedTimer));

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
