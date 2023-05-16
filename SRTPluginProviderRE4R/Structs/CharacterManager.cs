using System;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Runtime.InteropServices;

namespace SRTPluginProviderRE4R.Structs
{
    public class PlayerContext
    {
        private int kindID;
        private Vec3 position;
        private Quat rotation;
        private HitPoint health;

        public CharacterKindID KindID { get => (CharacterKindID)kindID; set => kindID = (int)value; }
        public string SurvivorTypeString => KindID.ToString();
        public Vec3 Position { get => position; set => position = value; }
        public Quat Rotation { get => rotation; set => rotation = value; }
        public HitPoint Health { get => health; set => health = value; }
        public PlayerState HealthState
        {
            get =>
                !Health.IsAlive ? PlayerState.Dead :
                Health.Percentage >= 0.66f ? PlayerState.Fine :
                Health.Percentage >= 0.33f ? PlayerState.Caution :
                PlayerState.Danger;
        }
        public string CurrentHealthState => HealthState.ToString();
        public bool IsLoaded => KindID != CharacterKindID.None;

        public PlayerContext()
        {
            kindID = -1;
            Position = new Vec3(0, 0, 0);
            Rotation = new Quat(0, 0, 0, 0);
        }

        public void SetValues(CharacterContext cc, HitPoint hp)
        {
            KindID = cc.KindID;
            Position.Update(cc.X, cc.Y, cc.Z);
            Rotation.Update(cc.RW, cc.RX, cc.RY, cc.RZ);
            Health = hp;
        }
    }

    public class Vec3
    {
        // Initialize Fields
        private float x;
        private float y;
        private float z;

        // Initialize Properties (Getters and Setters)
        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }
        public float Z { get => z; set => z = value; }

        // Initialize Class Object
        public Vec3(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        // Update Object Method
        public void Update(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
    }

    public class Quat
    {
        private float w;
        private float x;
        private float y;
        private float z;
        public float W { get => w; set => w = value; }
        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }
        public float Z { get => z; set => z = value; }

        public Quat(float _w, float _x, float _y, float _z)
        {
            w = _w;
            x = _x;
            y = _y;
            z = _z;
        }

        public void Update(float _w, float _x, float _y, float _z)
        {
            w = _w;
            x = _x;
            y = _y;
            z = _z;
        }
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x420)]
    public struct CharacterContext
    {
        [FieldOffset(0x38)] private int kindID;
        [FieldOffset(0x60)] private Vector3 position;
        [FieldOffset(0x70)] private Quaternion rotation;
        [FieldOffset(0x148)] private nint hitPoints;

        public CharacterKindID KindID => (CharacterKindID)kindID;
        public float X => position.X;
        public float Y => position.Y;
        public float Z => position.Z;
        public float RW => rotation.W;
        public float RX => rotation.X;
        public float RY => rotation.Y;
        public float RZ => rotation.Z;
        public IntPtr HitPoints => IntPtr.Add(hitPoints, 0x0);
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x20)]
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public struct HitPoint
    {
        [FieldOffset(0x10)] private int defaultHitPoint;
        [FieldOffset(0x14)] private int currentHitPoint;
        [FieldOffset(0x18)] private byte invincible;
        [FieldOffset(0x19)] private byte noDamage;
        [FieldOffset(0x1A)] private byte noDeath;
        [FieldOffset(0x1B)] private byte immortal;
        [FieldOffset(0x1C)] private byte isHPChanged;

        public int MaxHP => defaultHitPoint;
        public int CurrentHP => currentHitPoint;
        public bool Invincible => invincible != 0;
        public bool NoDamage => noDamage != 0;
        public bool NoDeath => noDeath != 0;
        public bool Immortal => immortal != 0;
        public bool IsHPChanged => isHPChanged != 0;
        public bool IsTrigger => MaxHP == 1 && CurrentHP == 1;
        public bool IsAlive => !IsTrigger && MaxHP > 0 && CurrentHP > 0 && CurrentHP <= MaxHP;
        public bool IsDamaged => MaxHP > 0 && CurrentHP > 0 && CurrentHP < MaxHP;
        public float Percentage => ((IsAlive) ? (float)CurrentHP / (float)MaxHP : 0f);

        /// <summary>
        /// Debugger display message.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay
        {
            get
            {
                if (IsTrigger)
                    return string.Format("TRIGGER", CurrentHP, MaxHP, Percentage);
                else if (IsAlive)
                    return string.Format("{0} / {1} ({2:P1})", CurrentHP, MaxHP, Percentage);
                else
                    return "DEAD / DEAD (0%)";
            }
        }
    }

    public enum CharacterKindID : int
    {
        None = 0,
        Leon = 100000,
        ch0_a1z0 = 110000,
        ch0_zzzz = 199999,
        Village_Ganado = 200000,
        Cult_Ganado = 200001,
        Island_Ganado = 200002,
        Salvador = 200003,
        Colmillos = 200004,
        Novistador = 200005,
        El_Gigante = 200006,
        DelLago = 200007,
        Garrador = 200008,
        Las_Plagas = 200009,
        JJ = 200010,
        Krauser = 200011,
        Regenerador = 200012,
        Krauser2 = 200013,
        ch1_d5z0 = 200014,
        Verdugo = 200015,
        Soldier_Ganado = 200016,
        Salazar = 200017,
        Sadler = 200018,
        Méndez = 200019,
        Méndez2 = 200020,
        Méndez3 = 200021,
        ch4_d7z0 = 200022,
        ch4_f9z0 = 200023,
        ch4_faz0 = 200024,
        ch4_faz1 = 200025,
        Sadler2 = 200026,
        Sadler3 = 200027,
        ch4_fez0 = 200028,
        ch4_fbz0 = 200029,
        Ashley = 200030,
        ch2_a200 = 200031,
        ch2_a3z0 = 200032,
        ch2_a3z1 = 200033,
        ch2_a600 = 200034,
        ch2_a7z0 = 200035,
        ch2_b0z0 = 200036,
        ch2_b1z0 = 200037,
        ch2_b200 = 200038,
        ch2_b300 = 200039,
        ch2_b400 = 200040,
        ch2_b600 = 200041,
        ch2_b8z0 = 200042,
        ch2_b900 = 200043,
        ch2_ba00 = 200044,
        ch2_bbz0 = 200045,
        ch2_bc00 = 200046,
        ch2_bd00 = 200047,
        ch3_a8z0 = 380000,
        ch6_i0z0 = 600000,
        ch6_i1z0 = 600001,
        ch6_i2z0 = 600002,
        ch6_i3z0 = 600003,
        ch6_i4z0 = 600004,
        ch6_i5z0 = 600005,
        ch8_0000 = 80000,
        ch8_1000 = 81000,
        ch8_1100 = 81100,
        ch8_g1z0 = 81101,
        ch8_g5z0 = 81102,
        ch8_g9z0 = 81103,
        Pig = 81104,
        Cow = 81105,
        Dog = 81106,
        Black_Bass = 81107,
        ch8_g4z0 = 81108,
        ch7_k0z0 = 81109,
        ch5_j1z0 = 500000,
        Invalid = -1,
    }

    public enum PlayerState : int
    {
        Dead,
        Fine,
        Caution,
        Danger,
        Poisoned
    }
}