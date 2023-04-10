using System.Runtime.InteropServices;

namespace SRTPluginProviderRE4R.Structs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x20)]

    public struct SystemSaveData
    {
        [FieldOffset(0x18)] private ulong systemElapsedTime; // SystemElapsedTime

        public ulong SystemElapsedTime => systemElapsedTime;
    }
}