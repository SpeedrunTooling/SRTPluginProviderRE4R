using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SRTPluginProviderRE4R.Structs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x8)]
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public struct EntityHealth
    {
        [FieldOffset(0x0)] private int maximumHealth;
        [FieldOffset(0x4)] private int currentHealth;

        public int MaximumHealth => maximumHealth;
        public int CurrentHealth => currentHealth;

        public bool IsTrigger => MaximumHealth == 1 && CurrentHealth == 1;
        public bool IsAlive => !IsTrigger && MaximumHealth > 0 && CurrentHealth > 0 && CurrentHealth <= MaximumHealth;
        public bool IsDamaged => MaximumHealth > 0 && CurrentHealth > 0 && CurrentHealth < MaximumHealth;
        public float Percentage => ((IsAlive) ? (float)CurrentHealth / (float)MaximumHealth : 0f);

        /// <summary>
        /// Debugger display message.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay
        {
            get
            {
                if (IsTrigger)
                    return string.Format("TRIGGER", CurrentHealth, MaximumHealth, Percentage);
                else if (IsAlive)
                    return string.Format("{0} / {1} ({2:P1})", CurrentHealth, MaximumHealth, Percentage);
                else
                    return "DEAD / DEAD (0%)";
            }
        }
    }
}
