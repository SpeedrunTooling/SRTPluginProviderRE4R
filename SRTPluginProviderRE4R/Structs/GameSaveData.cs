using System.Runtime.InteropServices;

namespace SRTPluginProducerRE4R.Structs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x38)]

    public struct GameSaveData
    {
        [FieldOffset(0x18)] private ulong gameElapsedTime; // GameElapsedTime
        [FieldOffset(0x20)] private ulong demoSpendingTime; // DemoSpendingTime
        [FieldOffset(0x28)] private ulong inventorySpendingTime; // InventorySpendingTime
        [FieldOffset(0x30)] private ulong pauseSpendingTime; // PauseSpendingTime

        public ulong GameElapsedTime => gameElapsedTime;
        public ulong DemoSpendingTime => demoSpendingTime;
        public ulong InventorySpendingTime => inventorySpendingTime;
        public ulong PauseSpendingTime => pauseSpendingTime;
    }
}