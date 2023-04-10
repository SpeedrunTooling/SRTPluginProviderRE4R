using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SRTPluginProviderRE4R.Structs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x88)]
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public struct GameRankSystem
    {
        [FieldOffset(0x10)] private int rank;
        [FieldOffset(0x14)] private float actionPoint;
        [FieldOffset(0x18)] private float itemPoint;

        public int Rank => rank;
        public float ActionPoint => actionPoint;
        public float ItemPoint => itemPoint;

        /// <summary>
        /// Debugger display message.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay => $"Rank {Rank} (AP {ActionPoint} / IP {ItemPoint})";
    }
}
