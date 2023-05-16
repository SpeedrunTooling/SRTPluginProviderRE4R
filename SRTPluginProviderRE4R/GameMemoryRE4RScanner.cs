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
        private int pointerAddressInventoryManager;

        // Pointer Classes
        private IntPtr BaseAddress { get; set; }
        private MultilevelPointer PointerCharacterContext { get; set; }
        private MultilevelPointer[] PointerEnemyContext { get; set; }
        private MultilevelPointer PointerEnemyCount { get; set; }
        private MultilevelPointer PointerGameStatsManagerOngoingStatsChapterLapTime { get; set; }
        private MultilevelPointer PointerGameStatsManagerOngoingStatsKillCount { get; set; }
        private MultilevelPointer PointerGameRankManager { get; set; }
        private MultilevelPointer PointerGameClock { get; set; }
        private MultilevelPointer PointerInventoryManager { get; set; }
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
                gameMemoryValues._enemies = new PlayerContext[32];
                PointerEnemyContext = new MultilevelPointer[32];

                for (int i = 0; i < 32; ++i)
                {
                    gameMemoryValues._enemies[i] = new PlayerContext();
                    PointerEnemyContext[i] = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressCharacterManager), 0xA8, 0x40 + (i * sizeof(nuint)));
                }

                // Setup the pointers.
                PointerCharacterContext = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressCharacterManager), 0x90, 0x40);
                PointerGameStatsManagerOngoingStatsChapterLapTime = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressGameStatsManager), 0x20, 0x10);
                PointerGameStatsManagerOngoingStatsKillCount = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressGameStatsManager), 0x20, 0x18);
                PointerGameRankManager = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressGameRankManager));
                PointerGameClock = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressGameClock));
                PointerInventoryManager = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressInventoryManager));
                PointerEnemyCount = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressCharacterManager), 0xA8);
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
                        pointerAddressInventoryManager = 0x0D249848;
                        Console.WriteLine("Version: RE4R_WW_11025382");
                        return true;
                    }
                case GameVersion.RE4R_WW_20230407_1:
                    {
                        pointerAddressCharacterManager = 0x0D22B0A0;
                        pointerAddressGameStatsManager = 0x0D217780;
                        pointerAddressGameRankManager = 0x0D22B1A0;
                        pointerAddressGameClock = 0x0D22D7D0;
                        pointerAddressInventoryManager = 0x0D219D08;
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
            PointerEnemyCount.UpdatePointers();
            PointerCharacterContext.UpdatePointers();
            PointerGameStatsManagerOngoingStatsChapterLapTime.UpdatePointers();
            PointerGameStatsManagerOngoingStatsKillCount.UpdatePointers();
            PointerGameRankManager.UpdatePointers();
            PointerInventoryManager.UpdatePointers();

            for (int i = 0; i < PointerEnemyContext.Length; ++i)
                PointerEnemyContext[i].UpdatePointers();
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
            gameMemoryValues.enemyArraySize = PointerEnemyCount.DerefInt(0x3C);
            // EnemyContext
            for (int i = 0; i < gameMemoryValues.enemyArraySize; ++i)
            {
                var cc = PointerEnemyContext[i].Deref<CharacterContext>(0x0);
                var hp = memoryAccess.GetAt<HitPoint>((nuint*)cc.HitPoints);
                gameMemoryValues._enemies[i].SetValues(cc, hp);
            }
        }

        private unsafe void UpdateInventory(Group group, CharacterKindID kindId, long* Controller)
        {
            if (group == Group.Case)
            {
                var csic = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add((IntPtr)Controller, 0xA0));
                var csi = memoryAccess.GetAt<CsInventory>(csic);
                var ii = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add(csi.InventoryItems, 0x10));
                
                if (kindId == CharacterKindID.Leon)
                {
                    gameMemoryValues._caseSize.SetValues(csi.CurrRowSize, csi.CurrColumnSize);
                    gameMemoryValues._inventoryCount = memoryAccess.GetIntAt((nuint*)IntPtr.Add(csi.InventoryItems, 0x18));
                    gameMemoryValues._items = new InventoryEntry[gameMemoryValues._inventoryCount > 0 ? gameMemoryValues._inventoryCount : 0];
                    for (var i = 0; i < gameMemoryValues._inventoryCount; i++)
                    {
                        var position = (i * 0x8) + 0x20;
                        var csii = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add((IntPtr)ii, position));
                        var iib = memoryAccess.GetAt<InventoryItemBase>(csii);
                        var _item = memoryAccess.GetAt<Item>((nuint*)iib.Item);
                        var itemDef = memoryAccess.GetAt<ItemDefinition>((nuint*)_item.ItemDefine);
                        gameMemoryValues._items[i].SetValues(_item, iib, itemDef);
                    }
                    return;
                }
                else if (kindId == CharacterKindID.Ashley)
                {
                    gameMemoryValues._caseSizeAshley.SetValues(csi.CurrRowSize, csi.CurrColumnSize);
                    gameMemoryValues._inventoryCountAshley = memoryAccess.GetIntAt((nuint*)IntPtr.Add(csi.InventoryItems, 0x18));
                    gameMemoryValues._itemsAshley = new InventoryEntry[gameMemoryValues._inventoryCount > 0 ? gameMemoryValues._inventoryCount : 0];
                    for (var i = 0; i < gameMemoryValues._inventoryCount; i++)
                    {
                        var position = (i * 0x8) + 0x20;
                        var csii = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add((IntPtr)ii, position));
                        var iib = memoryAccess.GetAt<InventoryItemBase>(csii);
                        var _item = memoryAccess.GetAt<Item>((nuint*)iib.Item);
                        var itemDef = memoryAccess.GetAt<ItemDefinition>((nuint*)_item.ItemDefine);
                        gameMemoryValues._itemsAshley[i].SetValues(_item, iib, itemDef);
                    }
                    return;
                }
                return;
            }
            else if (group == Group.KeyItems)
            {
                // Console.WriteLine($"KeyItems: {Controller.ToString("X8")}");
                var kiic = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add((IntPtr)Controller, 0x98));
                return;
            }
            else if (group == Group.Treasure)
            {
                // Console.WriteLine($"Treasure: {Controller.ToString("X8")}");
                var tic = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add((IntPtr)Controller, 0x98));
                if (kindId == CharacterKindID.Leon)
                {
                    return;
                }
                else if (kindId == CharacterKindID.Ashley)
                {
                    return;
                }
                    
                return;
            }
            else if (group == Group.Unique)
            {
                // Console.WriteLine($"Unique: {Controller.ToString("X8")}");
                var uic = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add((IntPtr)Controller, 0xA0));
                return;
            }
        }

        private unsafe void UpdateInventoryManager()
        {
            var im = PointerInventoryManager.Deref<InventoryManager>(0x0);
            var ct = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add(im.ControllerTable, 0x18));
            var entriesCount = memoryAccess.GetIntAt((nuint*)IntPtr.Add(im.ControllerTable, 0x20));
            for (var i = 0; i < entriesCount; i++)
            {
                var positionKey = (i * 0x18) + 0x28;
                var positionValue = (i * 0x18) + 0x30;
                var cidAddress = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add((IntPtr)ct, positionKey));
                var cid = memoryAccess.GetAt<ContextID>(cidAddress);
                var icbAddress = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add((IntPtr)ct, positionValue));
                var ccAddress = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add((IntPtr)icbAddress, 0x58));
                var cc = memoryAccess.GetAt<CharacterContext>(ccAddress);
                UpdateInventory((Group)cid.Group, cc.KindID, icbAddress);
                // Console.WriteLine($"Test Log InventoryControllerBase {i}: {ccAddress.ToString("X8")}");
            }
        }

        internal unsafe IGameMemoryRE4R Refresh()
        {
            UpdateInventoryManager();
            UpdateGameClock();
            UpdatePlayerContext();
            UpdateEnemyContext();
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
