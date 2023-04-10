using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SRTPluginProviderRE4R.Structs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x20)]
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public struct HitPoint
    {
        [FieldOffset(0x10)] private int defaultHitPoint;
        [FieldOffset(0x14)] private int currentHitPoint;

        public int DefaultHitPoint => defaultHitPoint;
        public int CurrentHitPoint => currentHitPoint;

        public bool IsTrigger => DefaultHitPoint == 1 && CurrentHitPoint == 1;
        public bool IsAlive => !IsTrigger && DefaultHitPoint > 0 && CurrentHitPoint > 0 && CurrentHitPoint <= DefaultHitPoint;
        public bool IsDamaged => DefaultHitPoint > 0 && CurrentHitPoint > 0 && CurrentHitPoint < DefaultHitPoint;
        public float Percentage => ((IsAlive) ? (float)CurrentHitPoint / (float)DefaultHitPoint : 0f);

        /// <summary>
        /// Debugger display message.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay
        {
            get
            {
                if (IsTrigger)
                    return string.Format("TRIGGER", CurrentHitPoint, DefaultHitPoint, Percentage);
                else if (IsAlive)
                    return string.Format("{0} / {1} ({2:P1})", CurrentHitPoint, DefaultHitPoint, Percentage);
                else
                    return "DEAD / DEAD (0%)";
            }
        }
    }
}
