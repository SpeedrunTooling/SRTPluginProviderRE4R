using ProcessMemory;
using SRTPluginProviderRE4R.Structs;
using System;
using System.Diagnostics;

namespace SRTPluginProviderRE4R
{
    internal class GameMemoryRE4RScanner : IDisposable
    {
        // Variables
        private ProcessMemoryHandler memoryAccess;
        private GameMemoryRE4R gameMemoryValues;
        public bool HasScanned;
        public bool ProcessRunning => memoryAccess != null && memoryAccess.ProcessRunning;
        public uint ProcessExitCode => (memoryAccess != null) ? memoryAccess.ProcessExitCode : 0;

        // Pointer Address Variables
        private int pointerAddressCharacterManager;
        private int pointerAddressGameStatsManager;
        private int pointerAddressGameRankManager;
        private int pointerAddressGameClock;

        // Pointer Classes
        private IntPtr BaseAddress { get; set; }
        private MultilevelPointer PointerCharacterContext { get; set; }
        private MultilevelPointer[] PointerEnemyContext { get; set; }
        private MultilevelPointer PointerPlayerHealth { get; set; }
        private MultilevelPointer PointerEnemyCount { get; set; }
        private MultilevelPointer[] PointerEnemyHealth { get; set; }
        private MultilevelPointer PointerGameStatsManagerOngoingStatsChapterLapTime { get; set; }
        private MultilevelPointer PointerGameStatsManagerOngoingStatsKillCount { get; set; }
        private MultilevelPointer PointerGameRankManager { get; set; }
        private MultilevelPointer PointerGameClock { get; set; }
        // private MultilevelPointer PointerGameClockGameSaveData { get; set; }

        internal GameMemoryRE4RScanner(Process process = null)
        {
            gameMemoryValues = new GameMemoryRE4R();
            if (process != null)
                Initialize(process);
        }

        internal unsafe void Initialize(Process process)
        {
            if (process == null)
                return; // Do not continue if this is null.

            if (!SelectPointerAddresses(GameHashes.DetectVersion(process.MainModule.FileName)))
                return; // Unknown version.

            int pid = GetProcessId(process).Value;
            memoryAccess = new ProcessMemoryHandler((uint)pid);
            if (ProcessRunning)
            {
                BaseAddress = process?.MainModule?.BaseAddress ?? IntPtr.Zero; // Bypass .NET's managed solution for getting this and attempt to get this info ourselves via PInvoke since some users are getting 299 PARTIAL COPY when they seemingly shouldn't.

                gameMemoryValues._timer = new GameTimer();
                gameMemoryValues._playerContext = new PlayerContext();

                // Setup the pointers.
                GenerateEntityHealthPointers();
                PointerCharacterContext = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressCharacterManager), 0x90, 0x40);
                PointerGameStatsManagerOngoingStatsChapterLapTime = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressGameStatsManager), 0x20, 0x10);
                PointerGameStatsManagerOngoingStatsKillCount = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressGameStatsManager), 0x20, 0x18);
                PointerGameRankManager = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressGameRankManager));
                PointerGameClock = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressGameClock));
                // PointerGameClockGameSaveData = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressGameClock), 0x20);
            }
        }

        private bool SelectPointerAddresses(GameVersion version)
        {
            switch (version)
            {
                case GameVersion.RE4R_WW_11025382:
                    {
                        pointerAddressCharacterManager = 0x0D261E60;
                        pointerAddressGameStatsManager = 0x0D2603F0;
                        pointerAddressGameRankManager = 0x0D292A68;
                        pointerAddressGameClock = 0x0D262890;
                        Console.WriteLine("Version: RE4R_WW_11025382");
                        return true;
                    }
                case GameVersion.RE4R_WW_20230407_1:
                    {
                        pointerAddressCharacterManager = 0x0D22B0A0;
                        pointerAddressGameStatsManager = 0x0D217780;
                        pointerAddressGameRankManager = 0x0D22B1A0;
                        pointerAddressGameClock = 0x0D22D7D0;
                        Console.WriteLine("Version: RE4R_WW_20230407_1");
                        return true;
                    }

                case GameVersion.RE4R_WW_20230323_1:
                    {
                        Console.WriteLine("Version: RE4R_WW_20230323_1 - No longer supported due to structure changes.");
                        return false; // No longer supported due to structure changes.
                    }
            }

            // If we made it this far... rest in pepperonis. We have failed to detect any of the correct versions we support and have no idea what pointer addresses to use. Bail out.
            Console.WriteLine("Version: Unknown");
            return false;
        }

        internal void UpdatePointers()
        {
            GenerateEntityHealthPointers();
            PointerCharacterContext.UpdatePointers();
            PointerGameStatsManagerOngoingStatsChapterLapTime.UpdatePointers();
            PointerGameStatsManagerOngoingStatsKillCount.UpdatePointers();
            PointerGameRankManager.UpdatePointers();
            // PointerGameClockSystemSaveData.UpdatePointers();
            // PointerGameClockGameSaveData.UpdatePointers();
        }

        private unsafe void GenerateEntityHealthPointers()
        {
            if (PointerPlayerHealth is null)
                PointerPlayerHealth = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressCharacterManager), 0x90, 0x40, 0x148);
            else
                PointerPlayerHealth.UpdatePointers();

            if (PointerEnemyCount is null)
                PointerEnemyCount = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressCharacterManager), 0xA8);
            else
                PointerEnemyCount.UpdatePointers();

            gameMemoryValues.enemyArraySize = PointerEnemyCount.DerefInt(0x3C);
            if (PointerEnemyHealth is null || PointerEnemyHealth.Length != gameMemoryValues.enemyArraySize ||
                gameMemoryValues.enemyHealth is null || gameMemoryValues.enemyHealth.Length != gameMemoryValues.enemyArraySize)
            {
                PointerEnemyHealth = new MultilevelPointer[gameMemoryValues.enemyArraySize];
                PointerEnemyContext = new MultilevelPointer[gameMemoryValues.enemyArraySize];
                gameMemoryValues.enemyHealth = new HitPoint[gameMemoryValues.enemyArraySize];
                gameMemoryValues._enemies = new PlayerContext[gameMemoryValues.enemyArraySize];
            }

            if (gameMemoryValues._enemies[0] == null)
                for (int i = 0; i < gameMemoryValues._enemies.Length; ++i)
                    gameMemoryValues._enemies[i] = new PlayerContext();

            for (int i = 0; i < PointerEnemyHealth.Length; ++i)
            {
                if (PointerEnemyHealth[i] is null)
                    PointerEnemyHealth[i] = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressCharacterManager), 0xA8, 0x40 + (i * sizeof(nuint)), 0x148);
                else
                    PointerEnemyHealth[i].UpdatePointers();
            }

            for (int i = 0; i < PointerEnemyContext.Length; ++i)
            {
                if (PointerEnemyContext[i] is null)
                    PointerEnemyContext[i] = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressCharacterManager), 0xA8, 0x40 + (i * sizeof(nuint)));
                else
                    PointerEnemyContext[i].UpdatePointers();
            }
        }

        private unsafe void UpdateGameClock()
        {
            // GameClock
            var gc = PointerGameClock.Deref<GameClock>(0x0);
            var gsd = memoryAccess.GetAt<GameClockGameSaveData>((nuint*)gc.GameSaveData);
            gameMemoryValues._timer.SetValues(gc, gsd);
        }

        private unsafe void UpdatePlayerContext()
        {
            // PlayerContext
            var cc = PointerCharacterContext.Deref<CharacterContext>(0x0);
            var hp = memoryAccess.GetAt<HitPoint>((nuint*)cc.HitPoints);
            gameMemoryValues._playerContext.SetValues(cc, hp);
        }

        private unsafe void UpdateEnemyContext()
        {
            // EnemyContext
            for (int i = 0; i < gameMemoryValues.enemyArraySize; ++i)
            {
                var cc = PointerEnemyContext[i].Deref<CharacterContext>(0x0);
                var hp = memoryAccess.GetAt<HitPoint>((nuint*)cc.HitPoints);
                gameMemoryValues._enemies[i].SetValues(cc, hp);
            }

        }

        internal unsafe IGameMemoryRE4R Refresh()
        {
            UpdateGameClock();
            UpdatePlayerContext();
            UpdateEnemyContext();
            gameMemoryValues.playerHealth = PointerPlayerHealth.Deref<HitPoint>(0);
            for (int i = 0; i < gameMemoryValues.enemyArraySize; ++i)
                gameMemoryValues.enemyHealth[i] = PointerEnemyHealth[i].Deref<HitPoint>(0);

            gameMemoryValues.rank = PointerGameRankManager.Deref<GameRankSystem>(0);
            gameMemoryValues.gameStatsChapterLapTimeElement = PointerGameStatsManagerOngoingStatsChapterLapTime.Deref<GameStatsChapterLapTimeElement>(0);
            gameMemoryValues.gameStatsKillCountElement = PointerGameStatsManagerOngoingStatsKillCount.Deref<GameStatsKillCountElement>(0);
            // gameMemoryValues.systemSaveData = PointerGameClockSystemSaveData.Deref<SystemSaveData>(0);
            // gameMemoryValues.gameSaveData = PointerGameClockGameSaveData.Deref<GameSaveData>(0);

            HasScanned = true;
            return gameMemoryValues;
        }

        private int? GetProcessId(Process process) => process?.Id;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (memoryAccess != null)
                        memoryAccess.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~REmake1Memory() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
