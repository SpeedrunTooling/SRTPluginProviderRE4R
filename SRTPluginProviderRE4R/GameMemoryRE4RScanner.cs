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
        private int pointerAddressIGT;

        // Pointer Classes
        private IntPtr BaseAddress { get; set; }
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
                PointerIGT = new MultilevelPointer(memoryAccess, IntPtr.Add(BaseAddress, pointerAddressIGT), 0x28);
            }
        }

        private bool SelectPointerAddresses(GameVersion version)
        {
            switch (version)
            {
                case GameVersion.RE4R_Demo_20230309_1:
                    {
                        // pointerAddress
                        pointerAddressIGT = 0x0DD08088;
                        return true;
                    }
            }

            // If we made it this far... rest in pepperonis. We have failed to detect any of the correct versions we support and have no idea what pointer addresses to use. Bail out.
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        internal void UpdatePointers()
        {
            PointerIGT.UpdatePointers();
        }

        internal unsafe IGameMemoryRE4R Refresh()
        {
            // IGT
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
