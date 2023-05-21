using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SRTPluginProducerRE4R.Structs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x20)]
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public struct GameStatsChapterLapTimeElement
    {
        [FieldOffset(0x18)] private ulong time;

        public ulong Time => time;

        /// <summary>
        /// Debugger display message.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay => $"{Time}";
    }
}
