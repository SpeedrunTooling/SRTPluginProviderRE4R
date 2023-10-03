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
        private GameVersion gv = GameVersion.Unknown;
        public bool HasScanned;
        private readonly int MAX_PARTNERS = 2;
        private readonly int MAX_INVENTORIES = 8;
        private readonly int MAX_ITEMS = 50;
        private readonly int MAX_KEY_ITEMS = 50;
        private readonly int MAX_TREASURE_ITEMS = 204;
        private readonly int MAX_UNIQUE_ITEMS = 32;
        private int previousCount = 0;
        public bool ProcessRunning => memoryAccess != null && memoryAccess.ProcessRunning;
        public uint ProcessExitCode => (memoryAccess != null) ? memoryAccess.ProcessExitCode : 0;

        // Pointer Address Variables
        private int pointerAddressCharacterManager;
        private int pointerAddressGameStatsManager;
        private int pointerAddressGameRankManager;
        private int pointerAddressGameClock;
        private int pointerAddressInventoryManager;
        private int pointerAddressInGameShopManager;
        private int pointerAddressHighwayGuiManager;
        private int pointerAddressCampaignManager;

        // Pointer Classes
        private IntPtr BaseAddress { get; set; }
        private MultilevelPointer PointerCharacterContext { get; set; }
        private MultilevelPointer PointerPartnerContext { get; set; }
        private MultilevelPointer[] PointerEnemyContext { get; set; }
        private MultilevelPointer PointerEnemyCount { get; set; }
        private MultilevelPointer PointerGameStatsManagerOngoingStatsChapterLapTime { get; set; }
        private MultilevelPointer PointerGameStatsManagerOngoingStatsKillCount { get; set; }
        private MultilevelPointer PointerGameRankManager { get; set; }
        private MultilevelPointer PointerGameClock { get; set; }
        private MultilevelPointer PointerInventoryManager { get; set; }
        private MultilevelPointer PointerInGameShopManager { get; set; }
        private MultilevelPointer PointerLastItem { get; set; }
        private MultilevelPointer PointerSpinel { get; set; }
        private MultilevelPointer PointerCampaignManager { get; set; }

        private PlayerContext NullPlayerContext = new PlayerContext();
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

            gv = SelectPointerAddresses(GameHashes.DetectVersion(process.MainModule.FileName));
            if (gv == GameVersion.Unknown)
                return; // Unknown version.

            int pid = GetProcessId(process).Value;
            memoryAccess = new ProcessMemoryHandler((uint)pid);
            if (ProcessRunning)
            {
                BaseAddress = process?.MainModule?.BaseAddress ?? IntPtr.Zero; // Bypass .NET's managed solution for getting this and attempt to get this info ourselves via PInvoke since some users are getting 299 PARTIAL COPY when they seemingly shouldn't.

                gameMemoryValues._timer = new GameTimer();
                gameMemoryValues._playerContext = new PlayerContext();
                gameMemoryValues._partnerContext = new PlayerContext[2];
                gameMemoryValues._enemies = new PlayerContext[32];
                PointerEnemyContext = new MultilevelPointer[32];

                for (int i = 0; i < 32; ++i)
                {
                    gameMemoryValues._enemies[i] = new PlayerContext();
                    PointerEnemyContext[i] = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressCharacterManager), 0xA8, 0x40 + (i * sizeof(nuint)));
                }

                // Setup the pointers.
                PointerCharacterContext = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressCharacterManager), 0x90, 0x40);
                PointerPartnerContext = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressCharacterManager), 0x98);
                PointerGameStatsManagerOngoingStatsChapterLapTime = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressGameStatsManager), 0x20, 0x10);
                PointerGameStatsManagerOngoingStatsKillCount = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressGameStatsManager), 0x20, 0x18);
                PointerGameRankManager = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressGameRankManager));
                PointerGameClock = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressGameClock));
                PointerInventoryManager = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressInventoryManager));
                PointerEnemyCount = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressCharacterManager), 0xA8);
                PointerInGameShopManager = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressInGameShopManager), 0x18);
                PointerLastItem = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressHighwayGuiManager), 0xE0);
                PointerSpinel = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressInGameShopManager));
                PointerCampaignManager = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressCampaignManager));
                // PointerGameClockGameSaveData = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressGameClock), 0x20);
            }
        }

        private GameVersion SelectPointerAddresses(GameVersion version)
        {
            Console.WriteLine($"Version: {version}");

            switch (version)
            {
                case GameVersion.RE4R_WW_20231002_1:
                    {
                        pointerAddressCharacterManager = 0x0DBB88C0;
                        pointerAddressGameStatsManager = 0x0DBB39D0;
                        pointerAddressGameRankManager = 0x0DBB89C0;
                        pointerAddressGameClock = 0x0DBBB360;
                        pointerAddressInventoryManager = 0x0DBB8A90;
                        pointerAddressInGameShopManager = 0x0DBB8A78;
                        pointerAddressHighwayGuiManager = 0x0DBB8A60;
                        pointerAddressCampaignManager = 0x0DBB8808;
                        break;
                    }
                case GameVersion.RE4R_WW_20230921_1:
                    {
                        pointerAddressCharacterManager = 0x0DBC2900;
                        pointerAddressGameStatsManager = 0x0DBBDA10;
                        pointerAddressGameRankManager = 0x0DBC2A00;
                        pointerAddressGameClock = 0x0DBC53A0;
                        pointerAddressInventoryManager = 0x0DBC2AD0;
                        pointerAddressInGameShopManager = 0x0DBC2AB8;
                        pointerAddressHighwayGuiManager = 0x0DBC2AA0;
                        pointerAddressCampaignManager = 0x0DBC2848;
                        break;
                    }
                case GameVersion.RE4R_WW_20230424_1:
                    {
                        pointerAddressCharacterManager = 0x0D261E60;
                        pointerAddressGameStatsManager = 0x0D2603F0;
                        pointerAddressGameRankManager = 0x0D292A68;
                        pointerAddressGameClock = 0x0D262890;
                        pointerAddressInventoryManager = 0x0D249848;
                        pointerAddressInGameShopManager = 0x0D249D18;
                        pointerAddressHighwayGuiManager = 0x0D260470;
                        pointerAddressCampaignManager = 0x0D259508;
                        break;
                    }
                case GameVersion.RE4R_WW_20230407_1:
                    {
                        pointerAddressCharacterManager = 0x0D22B0A0;
                        pointerAddressGameStatsManager = 0x0D217780;
                        pointerAddressGameRankManager = 0x0D22B1A0;
                        pointerAddressGameClock = 0x0D22D7D0;
                        pointerAddressInventoryManager = 0x0D219D08;
                        pointerAddressInGameShopManager = 0x0D21A0D8;
                        pointerAddressHighwayGuiManager = 0x0D22B240;
                        pointerAddressCampaignManager = 0x0D23DCE0;
                        break;
                    }

                case GameVersion.RE4R_WW_20230323_1:
                    {
                        Console.WriteLine("Version no longer supported due to structure changes.");
                        break; // No longer supported due to structure changes.
                    }
            }

            return version;
        }

        internal void UpdatePointers()
        {
            PointerInGameShopManager.UpdatePointers();
            PointerEnemyCount.UpdatePointers();
            PointerCharacterContext.UpdatePointers();
            PointerPartnerContext.UpdatePointers();
            PointerGameStatsManagerOngoingStatsChapterLapTime.UpdatePointers();
            PointerGameStatsManagerOngoingStatsKillCount.UpdatePointers();
            PointerGameRankManager.UpdatePointers();
            PointerInventoryManager.UpdatePointers();
            PointerLastItem.UpdatePointers();
            PointerSpinel.UpdatePointers();
            PointerCampaignManager.UpdatePointers();

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
            gameMemoryValues._playerContext.SetValues(cc, hp, PointerCharacterContext.Address);
            // PartnerContext
            var li = PointerPartnerContext.Deref<ListStruct>(0x0);
            for (var i = 0; i < MAX_PARTNERS; i++)
            {
                if (gameMemoryValues._partnerContext[i] == null)
                    gameMemoryValues._partnerContext[i] = new PlayerContext();

                if (i > li.Count)
                {
                    gameMemoryValues._partnerContext[i] = NullPlayerContext;
                    continue;
                }
                var position = (i * 0x8) + 0x20;
                var characterSlot = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add(li.Items, position));
                var pc = memoryAccess.GetAt<CharacterContext>(characterSlot);
                var pchp = memoryAccess.GetAt<HitPoint>((nuint*)pc.HitPoints);
                gameMemoryValues._partnerContext[i].SetValues(pc, pchp, (long)characterSlot);
            }
            // var pc = PointerPartnerContext.Deref<CharacterContext>(0x0);
            // var pchp = memoryAccess.GetAt<HitPoint>((nuint*)pc.HitPoints);
            // gameMemoryValues._partnerContext.SetValues(pc, pchp);
        }

        private unsafe void UpdateEnemyContext()
        {
            gameMemoryValues.enemyArraySize = PointerEnemyCount.DerefInt(0x3C);
            // EnemyContext
            for (int i = 0; i < gameMemoryValues.enemyArraySize; ++i)
            {
				var cc = PointerEnemyContext[i].Deref<CharacterContext>(0x0);
                var hp = memoryAccess.GetAt<HitPoint>((nuint*)cc.HitPoints);
                gameMemoryValues._enemies[i].SetValues(cc, hp, PointerEnemyContext[i].Address);
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
                return;
            }
            else if (group == Group.KeyItems)
            {
                // Console.WriteLine($"KeyItems: {Controller.ToString("X8")}");
                var kiic = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add((IntPtr)Controller, 0x98));
                var kii = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add((IntPtr)kiic, 0x20));
                var li = memoryAccess.GetAt<ListStruct>(kii);
                gameMemoryValues._keyItemCount = li.Count;
                gameMemoryValues._keyItems = new InventoryEntry[gameMemoryValues._keyItemCount > 0 ? gameMemoryValues._keyItemCount : 0];
                for (var i = 0; i < gameMemoryValues._keyItemCount; i++)
                {
                    var position = (i * 0x8) + 0x20;
                    var itemSlot = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add(li.Items, position));
                    var iib = memoryAccess.GetAt<InventoryItemBase>(itemSlot);
                    var _item = memoryAccess.GetAt<Item>((nuint*)iib.Item);
                    var itemDef = memoryAccess.GetAt<ItemDefinition>((nuint*)_item.ItemDefine);
                    gameMemoryValues._keyItems[i].SetValues(_item, iib, itemDef);
                }
                // Console.WriteLine($"KeyItems Count: {test.Count}");
                return;
            }
            else if (group == Group.Treasure)
            {
                // Console.WriteLine($"Treasure: {Controller.ToString("X8")}");
                var tic = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add((IntPtr)Controller, 0x98));
                var ti = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add((IntPtr)tic, 0x28));
                var li = memoryAccess.GetAt<ListStruct>(ti);
                if (kindId == CharacterKindID.Leon)
                {
                    gameMemoryValues._treasureItemsCount = li.Count;
                    gameMemoryValues._treasureItems = new InventoryEntry[gameMemoryValues._treasureItemsCount > 0 ? gameMemoryValues._treasureItemsCount : 0];
                    for (var i = 0; i < gameMemoryValues._treasureItemsCount; i++)
                    {
                        var position = (i * 0x8) + 0x20;
                        var itemSlot = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add(li.Items, position));
                        var iib = memoryAccess.GetAt<InventoryItemBase>(itemSlot);
                        var _item = memoryAccess.GetAt<Item>((nuint*)iib.Item);
                        var itemDef = memoryAccess.GetAt<ItemDefinition>((nuint*)_item.ItemDefine);
                        gameMemoryValues._treasureItems[i].SetValues(_item, iib, itemDef);
                    }
                    return;
                } 
                return;
            }
            else if (group == Group.Unique)
            {
                // Console.WriteLine($"Unique: {Controller.ToString("X8")}");
                var uic = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add((IntPtr)Controller, 0xA0));
                var ui = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add((IntPtr)uic, 0x20));
                var li = memoryAccess.GetAt<ListStruct>(ui);
                gameMemoryValues._uniqueCount = li.Count;
                gameMemoryValues._uniqueItems = new InventoryEntry[gameMemoryValues._uniqueCount > 0 ? gameMemoryValues._uniqueCount : 0];
                for (var i = 0; i < gameMemoryValues._uniqueCount; i++)
                {
                    var position = (i * 0x8) + 0x20;
                    var itemSlot = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add(li.Items, position));
                    var iib = memoryAccess.GetAt<InventoryItemBase>(itemSlot);
                    var _item = memoryAccess.GetAt<Item>((nuint*)iib.Item);
                    var itemDef = memoryAccess.GetAt<ItemDefinition>((nuint*)_item.ItemDefine);
                    gameMemoryValues._uniqueItems[i].SetValues(_item, iib, itemDef);
                }
                // Console.WriteLine($"Unique Count: {test.Count}");
                return;
            }
        }

        private unsafe void UpdateInventoryManager()
        {
            var im = PointerInventoryManager.Deref<InventoryManager>(0x0);
            gameMemoryValues._ptas = im.CurrPTAS;
            var ct = (long*)memoryAccess.GetLongAt((nuint*)IntPtr.Add(im.ControllerTable, 0x18));
            var entriesCount = memoryAccess.GetIntAt((nuint*)IntPtr.Add(im.ControllerTable, 0x20));
            // var entriesFreeCount = memoryAccess.GetIntAt((nuint*)IntPtr.Add(im.ControllerTable, 0x24));
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
            gameMemoryValues._isInGameShopOpen = PointerInGameShopManager.DerefInt(0x50) != 0;
            gameMemoryValues._chapterId = PointerCampaignManager.DerefInt(0x30);
            gameMemoryValues._lastItem = PointerLastItem.DerefInt(gv == GameVersion.RE4R_WW_20230424_1 ? 0xF0 : 0xE8);
            gameMemoryValues._spinel = PointerSpinel.DerefInt(0x20);
            UpdatePlayerContext();
            UpdateInventoryManager();
            UpdateGameClock();
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
