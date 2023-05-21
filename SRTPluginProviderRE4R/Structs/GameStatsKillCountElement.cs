using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SRTPluginProducerRE4R.Structs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x18)]
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public struct GameStatsKillCountElement
    {
        [FieldOffset(0x14)] private int count;

        public int Count => count;

        /// <summary>
        /// Debugger display message.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay => $"{Count}";
    }
}
