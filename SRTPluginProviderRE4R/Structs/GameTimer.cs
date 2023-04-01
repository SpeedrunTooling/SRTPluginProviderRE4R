using System.Runtime.InteropServices;

namespace SRTPluginProviderRE4R.Structs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x20)]

    public struct GameTimer
    {
        [FieldOffset(0x0)] private long igtRunningTimer; // GameElapsedTime
        [FieldOffset(0x8)] private long igtCutsceneTimer; // DemoSpendingTime
        [FieldOffset(0x10)] private long igtMenuTimer; // InventorySpendingTime
        [FieldOffset(0x18)] private long igtPausedTimer; // PauseSpendingTime

        public long IGTRunningTimer => igtRunningTimer;
        public long IGTCutsceneTimer => igtCutsceneTimer;
        public long IGTMenuTimer => igtMenuTimer;
        public long IGTPausedTimer => igtPausedTimer;
    }
}