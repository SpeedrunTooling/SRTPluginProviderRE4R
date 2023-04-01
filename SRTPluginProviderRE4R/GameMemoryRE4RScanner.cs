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
        public int ProcessExitCode => (memoryAccess != null) ? memoryAccess.ProcessExitCode : 0;

        // Pointer Address Variables
        private int pointerAddressEntity;
        private int pointerAddressGameStatsManager;
        private int pointerAddressGameRankManager;
        private int pointerAddressIGT;

        // Pointer Classes
        private IntPtr BaseAddress { get; set; }
        private MultilevelPointer PointerPlayerHealth { get; set; }
        private MultilevelPointer PointerEnemyCount { get; set; }
        private MultilevelPointer[] PointerEnemyHealth { get; set; }
        private MultilevelPointer PointerGameStatsManager { get; set; }
        private MultilevelPointer PointerGameRankManager { get; set; }
        private MultilevelPointer PointerIGT { get; set; }

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
            memoryAccess = new ProcessMemoryHandler(pid);
            if (ProcessRunning)
            {
                BaseAddress = NativeWrappers.GetProcessBaseAddress(pid, PInvoke.ListModules.LIST_MODULES_64BIT); // Bypass .NET's managed solution for getting this and attempt to get this info ourselves via PInvoke since some users are getting 299 PARTIAL COPY when they seemingly shouldn't.

                // Setup the pointers.
                GenerateEntityHealthPointers();
                PointerGameStatsManager = new MultilevelPointer(memoryAccess, IntPtr.Add(BaseAddress, pointerAddressGameStatsManager), 0x20, 0x10);
                PointerGameRankManager = new MultilevelPointer(memoryAccess, IntPtr.Add(BaseAddress, pointerAddressGameRankManager));
                PointerIGT = new MultilevelPointer(memoryAccess, IntPtr.Add(BaseAddress, pointerAddressIGT), 0x20);
            }
        }

        private bool SelectPointerAddresses(GameVersion version)
        {
            switch (version)
            {
                case GameVersion.RE4R_WW_20230323_1:
                    {
                        pointerAddressEntity = 0x0D23E618;
                        pointerAddressGameStatsManager = 0x0D20FF80;
                        pointerAddressGameRankManager = 0x0D23F000;
                        pointerAddressIGT = 0x0D234048;
                        return true;
                    }
            }

            // If we made it this far... rest in pepperonis. We have failed to detect any of the correct versions we support and have no idea what pointer addresses to use. Bail out.
            return false;
        }

        private unsafe void GenerateEntityHealthPointers()
        {
            if (PointerPlayerHealth is null)
                PointerPlayerHealth = new MultilevelPointer(memoryAccess, IntPtr.Add(BaseAddress, pointerAddressEntity), 0x90, 0x40, 0x148);
            else
                PointerPlayerHealth.UpdatePointers();

            if (PointerEnemyCount is null)
                PointerEnemyCount = new MultilevelPointer(memoryAccess, IntPtr.Add(BaseAddress, pointerAddressEntity), 0xA8);
            else
                PointerEnemyCount.UpdatePointers();

            gameMemoryValues.enemyArraySize = PointerEnemyCount.DerefInt(0x3C);
            if (PointerEnemyHealth is null || PointerEnemyHealth.Length != gameMemoryValues.enemyArraySize ||
                gameMemoryValues.enemyHealth is null || gameMemoryValues.enemyHealth.Length != gameMemoryValues.enemyArraySize)
            {
                PointerEnemyHealth = new MultilevelPointer[gameMemoryValues.enemyArraySize];
                gameMemoryValues.enemyHealth = new EntityHealth[gameMemoryValues.enemyArraySize];
            }

            for (int i = 0; i < PointerEnemyHealth.Length; ++i)
            {
                if (PointerEnemyHealth[i] is null)
                    PointerEnemyHealth[i] = new MultilevelPointer(memoryAccess, IntPtr.Add(BaseAddress, pointerAddressEntity), 0xA8, 0x40 + (i * sizeof(nuint)), 0x148);
                else
                    PointerEnemyHealth[i].UpdatePointers();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void UpdatePointers()
        {
            GenerateEntityHealthPointers();
            PointerGameStatsManager.UpdatePointers();
            PointerGameRankManager.UpdatePointers();
            PointerIGT.UpdatePointers();
        }

        internal unsafe IGameMemoryRE4R Refresh()
        {
            gameMemoryValues.playerHealth = PointerPlayerHealth.Deref<EntityHealth>(0x10);
            for (int i = 0; i < gameMemoryValues.enemyArraySize; ++i)
                gameMemoryValues.enemyHealth[i] = PointerEnemyHealth[i].Deref<EntityHealth>(0x10);

            gameMemoryValues.chapterKillCount = PointerGameStatsManager.DerefInt(0x34);
            gameMemoryValues.rank = PointerGameRankManager.Deref<GameRank>(0x10);

            // IGT
            gameMemoryValues.chapterTimeStart = PointerGameStatsManager.DerefLong(0x18);
            gameMemoryValues.timer = PointerIGT.Deref<GameTimer>(0x18);

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
