﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;

namespace SRTPluginProviderRE4R.Structs
{
    public class PlayerContext
    {
        private long address;
        private int kindID;
        private Vec3 position;
        private Quat rotation;
        private HitPoint health;

        public long Address { get => address; set => address = value; }
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
        public bool IsLoaded => KindID != CharacterKindID.None && Health.MaxHP != 0;
        public bool IsIgnored => CharacterUtils.IgnoreList.Contains(KindID);
        public bool IsAnimal => CharacterUtils.AnimalList.Contains(KindID);
        public bool IsBoss => CharacterUtils.BossList.Contains(KindID);

        public PlayerContext()
        {
            kindID = -1;
            Position = new Vec3(0, 0, 0);
            Rotation = new Quat(0, 0, 0, 0);
        }

        public void SetValues(CharacterContext cc, HitPoint hp, long address)
        {
            Address = address;
			KindID = cc.KindID;
            Position.Update(cc.X, cc.Y, cc.Z);
            Rotation.Update(cc.RW, cc.RX, cc.RY, cc.RZ);
            Health = hp;
        }
    }

    public class CharacterUtils
    {
        public static List<CharacterKindID> IgnoreList = new List<CharacterKindID>() {
            CharacterKindID.Sadler3,
        };

        public static List<CharacterKindID> AnimalList = new List<CharacterKindID>() {
            CharacterKindID.Cow,
            CharacterKindID.Dog,
            CharacterKindID.Pig,
            CharacterKindID.Black_Bass,
        };

        public static List<CharacterKindID> BossList = new List<CharacterKindID>()
        {
            CharacterKindID.El_Gigante,
            CharacterKindID.DelLago,
            CharacterKindID.Krauser,
            CharacterKindID.Krauser2,
            CharacterKindID.Salazar,
            CharacterKindID.Sadler,
            CharacterKindID.Méndez,
            CharacterKindID.Méndez_Mutate,
            CharacterKindID.Méndez_Mutate_2,
            CharacterKindID.Sadler2,
        };
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

}