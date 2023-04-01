using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SRTPluginProviderRE4R.Structs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x0C)]
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public struct GameRank
    {
        [FieldOffset(0x0)] private int rank;
        [FieldOffset(0x4)] private float actionPoint;
        [FieldOffset(0x8)] private float itemPoint;

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
