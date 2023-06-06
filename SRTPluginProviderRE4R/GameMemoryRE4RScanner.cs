using Microsoft.Extensions.Logging;
using ProcessMemory;
using SRTPluginProducerRE4R.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SRTPluginHelper;
using System.Security.Cryptography;
using SRTPluginBase;

namespace SRTPluginProducerRE4R
{
    internal class GameMemoryRE4RScanner : IDisposable
    {
        // Variables
        private readonly ILogger logger;
        private ProcessMemoryHandler? memoryAccess;
		private GameMemoryRE4R gameMemoryValues;
        private GameVersion gv = GameVersion.Unknown;
        public bool HasScanned;
        public DateTimeOffset LastPointerUpdate;
		private const int MAX_ENEMIES = 32;
		private const int MAX_PARTNERS = 2;
        // private const int MAX_INVENTORIES = 8;
        // private const int MAX_ITEMS = 50;
        // private const int MAX_KEY_ITEMS = 50;
        // private const int MAX_TREASURE_ITEMS = 204;
        // private const int MAX_UNIQUE_ITEMS = 32;
        // private int previousCount = 0;
        public bool ProcessRunning => memoryAccess != null && (memoryAccess?.ProcessRunning ?? default);
        public uint ProcessExitCode => (memoryAccess != null) ? memoryAccess?.ProcessExitCode ?? default : 0;

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
        private MultilevelPointer? PointerCharacterContext { get; set; }
        private MultilevelPointer? PointerPartnerContext { get; set; }
        private MultilevelPointer?[]? PointerEnemyContext { get; set; }
        private MultilevelPointer? PointerEnemyCount { get; set; }
        private MultilevelPointer? PointerGameStatsManagerOngoingStatsChapterLapTime { get; set; }
        private MultilevelPointer? PointerGameStatsManagerOngoingStatsKillCount { get; set; }
        private MultilevelPointer? PointerGameRankManager { get; set; }
        private MultilevelPointer? PointerGameClock { get; set; }
        private MultilevelPointer? PointerInventoryManager { get; set; }
        private MultilevelPointer? InGameShopFlowController { get; set; }
        private MultilevelPointer? PointerLastItem { get; set; }
        private MultilevelPointer? PointerSpinel { get; set; }
        private MultilevelPointer? PointerCampaignManager { get; set; }

		private static readonly Dictionary<byte[], GameVersion> supportedVersions = new()
		{
			{
				new byte[32] { 0x3E, 0xC3, 0x90, 0x1D, 0xD4, 0xF7, 0x0C, 0x7E, 0x03, 0xDB, 0xE3, 0x13, 0xA4, 0x7A, 0xDB, 0xC1, 0x10, 0x94, 0xD9, 0xAE, 0x34, 0xB6, 0xF3, 0x5E, 0xF9, 0xA0, 0x8C, 0x4F, 0x57, 0x32, 0x6D, 0x1E },
				GameVersion.RE4R_WW_11025382
			},
			{
				new byte[32] { 0x7A, 0x0A, 0x58, 0xB6, 0x00, 0x35, 0xCF, 0xBB, 0x73, 0xDD, 0x4F, 0x22, 0x16, 0x38, 0xD2, 0x6E, 0x50, 0xC8, 0xBF, 0xE6, 0xFC, 0x68, 0x77, 0x6B, 0x1F, 0x05, 0x1F, 0x33, 0x4A, 0x10, 0x00, 0x5C },
				GameVersion.RE4R_WW_20230407_1
			},
			{
				new byte[32] { 0x3C, 0x81, 0x07, 0xE9, 0x35, 0x2D, 0x0C, 0x4F, 0x39, 0x3D, 0x37, 0x50, 0xF0, 0xAC, 0x86, 0x62, 0x39, 0x1D, 0x52, 0x55, 0x9E, 0x94, 0xB5, 0x86, 0x73, 0x70, 0x50, 0xE0, 0xC5, 0x5E, 0xC3, 0x18 },
				GameVersion.RE4R_WW_20230323_1
			}
		};

		internal GameMemoryRE4RScanner(SRTPluginProducerRE4R pluginProducer, Process? process = default)
        {
            this.logger = pluginProducer.Logger;
            gameMemoryValues = new GameMemoryRE4R();
            if (process is not null)
            {
                gv = SelectPointerAddresses(SRTPluginHelper.Version.DetectVersion<SRTPluginProducerRE4R, SHA256, GameVersion>(pluginProducer, process.MainModule?.FileName ?? default, supportedVersions));
                Initialize(process);
            }
        }

        internal unsafe void Initialize(Process process)
        {
            if (process is null)
                return; // Do not continue if this is null.

            //gv = SelectPointerAddresses(GameHashes.DetectVersion(logger, process.MainModule?.FileName ?? default, supportedVersions));
            
            if (gv == GameVersion.Unknown)
                return; // Unknown version.

			int? pid = GetProcessId(process);
            if (pid is null)
                return;

            memoryAccess = new ProcessMemoryHandler((uint)pid);
            if (ProcessRunning)
            {
                BaseAddress = process?.MainModule?.BaseAddress ?? default; // Bypass .NET's managed solution for getting this and attempt to get this info ourselves via PInvoke since some users are getting 299 PARTIAL COPY when they seemingly shouldn't.

                gameMemoryValues.timer = new GameTimer();
                gameMemoryValues.playerContext = new PlayerContext();
                gameMemoryValues.partnerContext = new PlayerContext[2];
                gameMemoryValues.enemies = new PlayerContext[32];
                PointerEnemyContext = new MultilevelPointer[32];

                for (int i = 0; i < MAX_ENEMIES; ++i)
                {
                    gameMemoryValues.enemies[i] = new PlayerContext();
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
                InGameShopFlowController = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressInGameShopManager), 0x18);
                PointerLastItem = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressHighwayGuiManager), 0xE0);
                PointerSpinel = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressInGameShopManager));
                PointerCampaignManager = new MultilevelPointer(memoryAccess, (nint*)IntPtr.Add(BaseAddress, pointerAddressCampaignManager));

				LastPointerUpdate = DateTimeOffset.UtcNow;
			}
        }

        private GameVersion SelectPointerAddresses(GameVersion version)
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
                        pointerAddressInGameShopManager = 0x0D249D18;
                        pointerAddressHighwayGuiManager = 0x0D260470;
                        pointerAddressCampaignManager = 0x0D259508;

                        return GameVersion.RE4R_WW_11025382;
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

                        return GameVersion.RE4R_WW_20230407_1;
                    }

                case GameVersion.RE4R_WW_20230323_1:
                    {
                        logger.LogWarning("Version: RE4R_WW_20230323_1 - No longer supported due to structure changes.");
                        return GameVersion.RE4R_WW_20230323_1; // No longer supported due to structure changes.
                    }
            }

            // If we made it this far... rest in pepperonis. We have failed to detect any of the correct versions we support and have no idea what pointer addresses to use. Bail out.
            return GameVersion.Unknown;
        }

        internal void UpdatePointers() // DONE
        {
            // Only update pointers once every n seconds.
            if ((DateTimeOffset.UtcNow - LastPointerUpdate).TotalSeconds < 5d)
                return;
            LastPointerUpdate = DateTimeOffset.UtcNow;

			InGameShopFlowController?.UpdatePointers();
            PointerEnemyCount?.UpdatePointers();
            PointerCharacterContext?.UpdatePointers();
            PointerPartnerContext?.UpdatePointers();
            PointerGameStatsManagerOngoingStatsChapterLapTime?.UpdatePointers();
            PointerGameStatsManagerOngoingStatsKillCount?.UpdatePointers();
            PointerGameRankManager?.UpdatePointers();
            PointerInventoryManager?.UpdatePointers();
            PointerLastItem?.UpdatePointers();
            PointerSpinel?.UpdatePointers();
            PointerCampaignManager?.UpdatePointers();

            for (int i = 0; i < MAX_ENEMIES; ++i)
                PointerEnemyContext?[i]?.UpdatePointers();
        }

        private unsafe void UpdateGameClock() // DONE
        {
            // GameClock
            var gc = PointerGameClock?.Deref<GameClock>(0x0);
            var gsd = memoryAccess?.GetAt<GameClockGameSaveData>((nuint*)(gc?.GameSaveData ?? default));
			if (gameMemoryValues.IsNewGame)
				gameMemoryValues.timerOffset = (gsd?.GameElapsedTime - gsd?.DemoSpendingTime - gsd?.PauseSpendingTime) ?? 0L;
            gameMemoryValues.timer?.SetValues(gc, gsd, gameMemoryValues.timerOffset);
		}

        private unsafe void UpdatePlayerContext() // DONE
        {
            // PlayerContext
            var cc = PointerCharacterContext?.Deref<CharacterContext>(0x0);
            var hp = memoryAccess?.GetAt<HitPoint>((nuint*)(cc?.HitPoints ?? default));
            gameMemoryValues.playerContext?.SetValues(cc, hp, PointerCharacterContext?.Address ?? default);
            // PartnerContext
            var li = PointerPartnerContext?.Deref<ListStruct>(0x0);
            for (var i = 0; i < MAX_PARTNERS; i++)
            {
                if (gameMemoryValues.partnerContext is not null && gameMemoryValues.partnerContext?[i] is null)
                    gameMemoryValues.partnerContext![i] = new PlayerContext();

                if (i >= (li?.Count ?? 0))
                {
                    if (gameMemoryValues.partnerContext is not null)
                        gameMemoryValues.partnerContext![i] = default;
                    continue;
                }
                var position = (i * 0x8) + 0x20;
                var characterSlot = (long*)(memoryAccess?.GetLongAt((nuint*)IntPtr.Add(li?.Items ?? default, position)) ?? 0L);
                var pc = memoryAccess?.GetAt<CharacterContext>(characterSlot);
                var pchp = memoryAccess?.GetAt<HitPoint>((nuint*)(pc?.HitPoints ?? default));
                gameMemoryValues.partnerContext?[i]?.SetValues(pc, pchp, (long)characterSlot);
            }
            // var pc = PointerPartnerContext?.Deref<CharacterContext>(0x0);
            // var pchp = memoryAccess?.GetAt<HitPoint>((nuint*)pc.HitPoints);
            // gameMemoryValues._partnerContext.SetValues(pc, pchp);
        }

        private unsafe void UpdateEnemyContext() // DONE
        {
            gameMemoryValues.enemyArraySize = PointerEnemyCount?.DerefInt(0x3C) ?? default;
            // EnemyContext
            for (int i = 0; i < MAX_ENEMIES; ++i)
            {
                if (i >= gameMemoryValues.enemyArraySize)
                {
                    gameMemoryValues.enemies?[i]?.SetValues(default, default, default);
					continue;
                }
                var cc = PointerEnemyContext?[i]?.Deref<CharacterContext>(0x0);
                var hp = memoryAccess?.GetAt<HitPoint>((nuint*)(cc?.HitPoints ?? default));
                gameMemoryValues.enemies?[i]?.SetValues(cc, hp, PointerEnemyContext?[i]?.Address ?? default);
            }
        }

        private unsafe void UpdateInventory(Group? group, CharacterKindID? kindId, long* Controller)
        {
            if (group == Group.Case)
            {
                var csic = (long*)(memoryAccess?.GetLongAt((nuint*)IntPtr.Add((IntPtr)Controller, 0xA0)) ?? default);
                var csi = memoryAccess?.GetAt<CsInventory>(csic);
                var ii = (long*)(memoryAccess?.GetLongAt((nuint*)IntPtr.Add(csi?.InventoryItems ?? default, 0x10)) ?? default);
                
                if (kindId == CharacterKindID.Leon)
                {
                    gameMemoryValues.caseSize.SetValues(csi?.CurrRowSize ?? default, csi?.CurrColumnSize ?? default);
                    gameMemoryValues.inventoryCount = memoryAccess?.GetIntAt((nuint*)IntPtr.Add(csi?.InventoryItems ?? default, 0x18)) ?? default;
                    gameMemoryValues.items = new InventoryEntry[gameMemoryValues.inventoryCount > 0 ? gameMemoryValues.inventoryCount : 0];
                    for (var i = 0; i < gameMemoryValues.inventoryCount; i++)
                    {
                        var position = (i * 0x8) + 0x20;
                        var csii = (long*)(memoryAccess?.GetLongAt((nuint*)IntPtr.Add((IntPtr)ii, position)) ?? default);
                        var iib = memoryAccess?.GetAt<InventoryItemBase>(csii);
                        var item = memoryAccess?.GetAt<Item>((nuint*)(iib?.Item ?? default));
                        var itemDef = memoryAccess?.GetAt<ItemDefinition>((nuint*)(item?.ItemDefine ?? default));
                        gameMemoryValues.items[i].SetValues(item, iib, itemDef);
                    }
                    return;
                }
                return;
            }
            else if (group == Group.KeyItems)
            {
                // Console.WriteLine($"KeyItems: {Controller.ToString("X8")}");
                var kiic = (long*)(memoryAccess?.GetLongAt((nuint*)IntPtr.Add((IntPtr)Controller, 0x98)) ?? default);
                var kii = (long*)(memoryAccess?.GetLongAt((nuint*)IntPtr.Add((IntPtr)kiic, 0x20)) ?? default);
                var li = memoryAccess?.GetAt<ListStruct>(kii);
                gameMemoryValues.keyItemCount = li?.Count ?? default;
                gameMemoryValues.keyItems = new InventoryEntry[gameMemoryValues.keyItemCount > 0 ? gameMemoryValues.keyItemCount : 0];
                for (var i = 0; i < gameMemoryValues.keyItemCount; i++)
                {
                    var position = (i * 0x8) + 0x20;
                    var itemSlot = (long*)(memoryAccess?.GetLongAt((nuint*)IntPtr.Add(li?.Items ?? default, position)) ?? default);
                    var iib = memoryAccess?.GetAt<InventoryItemBase>(itemSlot);
                    var item = memoryAccess?.GetAt<Item>((nuint*)(iib?.Item ?? default));
                    var itemDef = memoryAccess?.GetAt<ItemDefinition>((nuint*)(item?.ItemDefine ?? default));
                    gameMemoryValues.keyItems[i].SetValues(item, iib, itemDef);
                }
                // Console.WriteLine($"KeyItems Count: {test.Count}");
                return;
            }
            else if (group == Group.Treasure)
            {
                // Console.WriteLine($"Treasure: {Controller.ToString("X8")}");
                var tic = (long*)(memoryAccess?.GetLongAt((nuint*)IntPtr.Add((IntPtr)Controller, 0x98)) ?? default);
                var ti = (long*)(memoryAccess?.GetLongAt((nuint*)IntPtr.Add((IntPtr)tic, 0x28)) ?? default);
                var li = memoryAccess?.GetAt<ListStruct>(ti);
                if (kindId == CharacterKindID.Leon)
                {
                    gameMemoryValues.treasureItemsCount = li?.Count ?? default;
                    gameMemoryValues.treasureItems = new InventoryEntry[gameMemoryValues.treasureItemsCount > 0 ? gameMemoryValues.treasureItemsCount : 0];
                    for (var i = 0; i < gameMemoryValues.treasureItemsCount; i++)
                    {
                        var position = (i * 0x8) + 0x20;
                        var itemSlot = (long*)(memoryAccess?.GetLongAt((nuint*)IntPtr.Add(li?.Items ?? default, position)) ?? default);
                        var iib = memoryAccess?.GetAt<InventoryItemBase>(itemSlot);
                        var item = memoryAccess?.GetAt<Item>((nuint*)(iib?.Item ?? default));
                        var itemDef = memoryAccess?.GetAt<ItemDefinition>((nuint*)(item?.ItemDefine ?? default));
                        gameMemoryValues.treasureItems[i].SetValues(item, iib, itemDef);
                    }
                    return;
                } 
                return;
            }
            else if (group == Group.Unique)
            {
                // Console.WriteLine($"Unique: {Controller.ToString("X8")}");
                var uic = (long*)(memoryAccess?.GetLongAt((nuint*)IntPtr.Add((IntPtr)Controller, 0xA0)) ?? default);
                var ui = (long*)(memoryAccess?.GetLongAt((nuint*)IntPtr.Add((IntPtr)uic, 0x20)) ?? default);
                var li = memoryAccess?.GetAt<ListStruct>(ui);
                gameMemoryValues.uniqueCount = li?.Count ?? default;
                gameMemoryValues.uniqueItems = new InventoryEntry[gameMemoryValues.uniqueCount > 0 ? gameMemoryValues.uniqueCount : 0];
                for (var i = 0; i < gameMemoryValues.uniqueCount; i++)
                {
                    var position = (i * 0x8) + 0x20;
                    var itemSlot = (long*)(memoryAccess?.GetLongAt((nuint*)IntPtr.Add(li?.Items ?? default, position)) ?? default);
                    var iib = memoryAccess?.GetAt<InventoryItemBase>(itemSlot);
                    var item = memoryAccess?.GetAt<Item>((nuint*)(iib?.Item ?? default));
                    var itemDef = memoryAccess?.GetAt<ItemDefinition>((nuint*)(item?.ItemDefine ?? default));
                    gameMemoryValues.uniqueItems[i].SetValues(item, iib, itemDef);
                }
                // Console.WriteLine($"Unique Count: {test.Count}");
                return;
            }
        }

        private unsafe void UpdateInventoryManager()
        {
            var im = PointerInventoryManager?.Deref<InventoryManager>(0x0);
            gameMemoryValues.ptas = im?.CurrPTAS ?? default;
            var ct = (long*)(memoryAccess?.GetLongAt((nuint*)IntPtr.Add(im?.ControllerTable ?? default, 0x18)) ?? default);
            var entriesCount = memoryAccess?.GetIntAt((nuint*)IntPtr.Add(im?.ControllerTable ?? default, 0x20));
            // var entriesFreeCount = memoryAccess?.GetIntAt((nuint*)IntPtr.Add(im.ControllerTable, 0x24));
            for (var i = 0; i < (entriesCount ?? default); i++)
            {
                var positionKey = (i * 0x18) + 0x28;
                var positionValue = (i * 0x18) + 0x30;
                var cidAddress = (long*)(memoryAccess?.GetLongAt((nuint*)IntPtr.Add((IntPtr)ct, positionKey)) ?? 0L);
                var cid = memoryAccess?.GetAt<ContextID>(cidAddress);
                var icbAddress = (long*)(memoryAccess?.GetLongAt((nuint*)IntPtr.Add((IntPtr)ct, positionValue)) ?? default);
                var ccAddress = (long*)(memoryAccess?.GetLongAt((nuint*)IntPtr.Add((IntPtr)icbAddress, 0x58)) ?? default);
                var cc = memoryAccess?.GetAt<CharacterContext>(ccAddress);
                UpdateInventory((Group?)cid?.Group, cc?.KindID, icbAddress);
                // Console.WriteLine($"Test Log InventoryControllerBase {i}: {ccAddress.ToString("X8")}");
            }
        }

        internal unsafe IGameMemoryRE4R Refresh()
        {
            UpdatePointers();

			gameMemoryValues.isInGameShopOpen = InGameShopFlowController?.DerefInt(0x50) != 0;
            gameMemoryValues.chapterId = PointerCampaignManager?.DerefInt(0x30) ?? default;
			gameMemoryValues.isNewGame = (PointerCampaignManager?.DerefByte(0x48) ?? default) != 0;
			gameMemoryValues.lastItem = PointerLastItem?.DerefInt(gv == GameVersion.RE4R_WW_11025382 ? 0xF0 : 0xE8) ?? default;
            gameMemoryValues.spinel = PointerSpinel?.DerefInt(0x20) ?? default;
            UpdatePlayerContext();
            UpdateInventoryManager();
            UpdateGameClock();
            UpdateEnemyContext();
            gameMemoryValues.rank = PointerGameRankManager?.Deref<GameRankSystem>(0) ?? default;
            gameMemoryValues.gameStatsChapterLapTimeElement = PointerGameStatsManagerOngoingStatsChapterLapTime?.Deref<GameStatsChapterLapTimeElement>(0) ?? default;
            gameMemoryValues.gameStatsKillCountElement = PointerGameStatsManagerOngoingStatsKillCount?.Deref<GameStatsKillCountElement>(0) ?? default;
            // gameMemoryValues.systemSaveData = PointerGameClockSystemSaveData?.Deref<SystemSaveData>(0) ?? default;
            // gameMemoryValues.gameSaveData = PointerGameClockGameSaveData?.Deref<GameSaveData>(0) ?? default;
            HasScanned = true;
            return gameMemoryValues;
        }

        private int? GetProcessId(Process? process) => process?.Id;

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
                        memoryAccess?.Dispose();
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
