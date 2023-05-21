using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SRTPluginProducerRE4R.Structs
{
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public struct InventoryEntry
    {
        private ItemID itemId;
        private int row;
        private int column;
        private ItemSizeKind itemSize;
        private ItemDirection currDirection;
        private int count;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay
        {
            get
            {
                return string.Format("ItemID: {0} - ItemName: {1} - Row: {2} - Column: {3} - Count: {4} - Item Size: {5}:{6} - IsRotated: {7}", (int)ItemId, ItemId, Row, Column, Count, ItemSize, (int)ItemSize, IsRotated);
            }
        }

        public ItemID ItemId { get => itemId; set => itemId = value; }
        public int Row { get => row; set => row = value; }
        public int Column { get => column; set => column = value; }
        public ItemSizeKind ItemSize { get => itemSize; set => itemSize = value; }
        public bool IsRotated => currDirection == ItemDirection.Rot_090;
        public int Count { get => count; set => count = value; }

        public void SetValues(Item item, InventoryItemBase inventoryItemBase, ItemDefinition itemDefinition)
        {
            itemId = item.ItemId;
            row = inventoryItemBase.Row;
            column = inventoryItemBase.Column;
            itemSize = itemDefinition.ItemSize;
            currDirection = inventoryItemBase.CurrDirection;
            count = item.CurrentItemCount;
        }
    }

    public struct CaseSize
    {
        private int rows;
        private int columns;
        public int Rows { get => rows; set => rows = value; }
        public int Columns { get => columns; set => columns = value; }
        public void SetValues(int _rows, int _columns)
        {
            rows = _rows;
            columns = _columns;
        }
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0xA0)]
    public struct InventoryManager
    {
        [FieldOffset(0x10)] private int currPTAS;
        [FieldOffset(0x20)] private nint controllerTable;
        public int CurrPTAS => currPTAS;
        public IntPtr ControllerTable => IntPtr.Add(controllerTable, 0x0);
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x28)]
    public struct ContextID
    {
        [FieldOffset(0x18)] private byte category;
        [FieldOffset(0x19)] private byte kind;
        [FieldOffset(0x1A)] private byte requestUpdateCode;
        [FieldOffset(0x1C)] private int group;
        [FieldOffset(0x20)] private int index;
        [FieldOffset(0x24)] private uint code;

        public byte Category => category;
        public byte Kind => kind;
        public bool RequestUpdateCode => requestUpdateCode != 0;
        public int Group => group;
        public int Index => index;
        public uint Code => code;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x138)]
    public struct CsInventory
    {
        [FieldOffset(0x20)] private int currInventorySize;
        [FieldOffset(0x24)] private int currRowSize;
        [FieldOffset(0x28)] private int currColumnSize;
        [FieldOffset(0x38)] private nint inventoryItems;

        public int CurrInventorySize => currInventorySize;
        public int CurrRowSize => currRowSize;
        public int CurrColumnSize => currColumnSize;
        public IntPtr InventoryItems => IntPtr.Add(inventoryItems, 0x0);
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x20)]
    public struct InventoryItemBase
    {
        [FieldOffset(0x10)] private nint item;
        [FieldOffset(0x18)] private int row;
        [FieldOffset(0x1C)] private int column;
        [FieldOffset(0x28)] private int currDirection;

        public IntPtr Item => IntPtr.Add(item, 0x0);
        public int Row => row;
        public int Column => column;
        public ItemDirection CurrDirection => (ItemDirection)currDirection;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x38)]
    public struct Item
    {
        [FieldOffset(0x10)] private nint itemDefine;
        [FieldOffset(0x28)] private int itemId;
        [FieldOffset(0x2C)] private int currentCondition;
        [FieldOffset(0x30)] private int currentDurability;
        [FieldOffset(0x34)] private int currentItemCount;

        public IntPtr ItemDefine => IntPtr.Add(itemDefine, 0x0);
        public ItemID ItemId => (ItemID)itemId;
        public int CurrentCondition => currentCondition;
        public int CurrentDurability => currentDurability;
        public int CurrentItemCount => currentItemCount;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x38)]
    public struct ItemDefinition
    {
        [FieldOffset(0x10)] private int itemSize;
        [FieldOffset(0x14)] private int stackMax;
        [FieldOffset(0x18)] private int defaultDurabilityMax;
        public ItemSizeKind ItemSize => (ItemSizeKind)itemSize;
        public int StackMax => stackMax;
        public int DefaultDurabilityMax => defaultDurabilityMax;
    }

}