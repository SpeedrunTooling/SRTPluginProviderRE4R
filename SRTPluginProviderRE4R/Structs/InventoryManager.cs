using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SRTPluginProviderRE4R.Structs
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

    public enum ItemDirection
    {
        Invalid = -1,
        Default = 0,
        Rot_090 = 1,
    }

    public enum ItemID
    {
        HandgunAmmo = 112800000,
        it_sm70_501_00_0 = 112801600,
        ShotgunShells = 112803200,
        RifleAmmo = 112804800,
        SubmachineGunAmmo = 112806400,
        Bolts = 112808000,
        it_sm70_506_00_0 = 112809600,
        it_sm70_507_00_0 = 112811200,
        it_sm70_508_00_0 = 112812800,
        it_sm70_509_00_0 = 112814400,
        it_sm70_300_00_0 = 112480000,
        it_sm70_200_00_0 = 112320000,
        it_sm71_300_00_0 = 114080000,
        GreenHerb = 114400000,
        it_sm71_500_10_0 = 114400160,
        RedHerb = 114401600,
        it_sm71_501_10_0 = 114401760,
        YellowHerb = 114403200,
        MixedHerbsGG = 114404800,
        it_sm71_504_00_0 = 114406400,
        MixedHerbsGR = 114408000,
        it_sm71_506_00_0 = 114409600,
        it_sm71_507_00_0 = 114411200,
        MixedHerbsGRY = 114412800,
        it_sm71_509_00_0 = 114414400,
        FirstAidSpray = 114416000,
        it_sm71_511_00_0 = 114417600,
        it_sm71_512_00_0 = 114419200,
        it_sm71_513_00_0 = 114420800,
        it_sm71_514_00_0 = 114422400,
        it_sm71_515_00_0 = 114424000,
        Scope = 116000000,
        it_sm72_501_00_0 = 116001600,
        it_sm72_502_00_0 = 116003200,
        it_sm72_503_00_0 = 116004800,
        it_sm72_504_00_0 = 116006400,
        it_sm72_505_00_0 = 116008000,
        it_sm72_506_00_0 = 116009600,
        Gunpowder = 117600000,
        ResourcesL = 117601600,
        it_sm73_502_00_0 = 117603200,
        it_sm73_503_00_0 = 117604800,
        ResourcesS = 117606400,
        it_sm74_502_00_0 = 119203200,
        it_sm74_503_00_0 = 119204800,
        it_sm74_504_00_0 = 119206400,
        it_sm74_506_00_0 = 119209600,
        it_sm74_507_00_0 = 119211200,
        it_sm74_508_00_0 = 119212800,
        it_sm74_509_00_0 = 119214400,
        it_sm74_511_00_0 = 119217600,
        it_sm74_512_00_0 = 119219200,
        it_sm74_513_00_0 = 119220800,
        it_sm74_514_00_0 = 119222400,
        it_sm74_515_00_0 = 119224000,
        it_sm74_516_00_0 = 119225600,
        it_sm74_519_00_0 = 119230400,
        it_sm74_520_00_0 = 119232000,
        it_sm74_522_00_0 = 119235200,
        it_sm74_523_00_0 = 119236800,
        it_sm74_524_00_0 = 119238400,
        it_sm74_528_00_0 = 119244800,
        it_sm74_529_00_0 = 119246400,
        it_sm74_530_00_0 = 119248000,
        it_sm74_531_00_0 = 119249600,
        it_sm74_532_00_0 = 119251200,
        it_sm74_534_00_0 = 119254400,
        it_sm74_535_00_0 = 119256000,
        it_sm74_536_00_0 = 119257600,
        it_sm74_537_00_0 = 119259200,
        it_sm74_538_00_0 = 119260800,
        it_sm74_539_00_0 = 119262400,
        it_sm74_541_00_0 = 119265600,
        it_sm74_542_00_0 = 119267200,
        it_sm74_543_00_0 = 119268800,
        it_sm74_544_00_0 = 119270400,
        it_sm74_546_00_0 = 119273600,
        it_sm74_547_00_0 = 119275200,
        it_sm74_548_00_0 = 119276800,
        it_sm74_549_00_0 = 119278400,
        it_sm74_550_00_0 = 119280000,
        it_sm74_551_00_0 = 119281600,
        it_sm74_552_00_0 = 119283200,
        it_sm74_553_00_0 = 119284800,
        it_sm74_554_00_0 = 119286400,
        it_sm74_555_00_0 = 119288000,
        it_sm74_556_00_0 = 119289600,
        it_sm74_557_00_0 = 119291200,
        it_sm74_300_00_0 = 118880000,
        it_sm74_301_00_0 = 118881600,
        it_sm74_302_00_0 = 118883200,
        it_sm74_303_00_0 = 118884800,
        it_sm74_304_00_0 = 118886400,
        it_sm74_305_00_0 = 118888000,
        it_sm74_306_00_0 = 118889600,
        it_sm74_307_00_0 = 118891200,
        it_sm74_308_00_0 = 118892800,
        it_sm74_309_00_0 = 118894400,
        it_sm74_310_00_0 = 118896000,
        it_sm74_311_00_0 = 118897600,
        it_sm74_312_00_0 = 118899200,
        it_sm74_313_00_0 = 118900800,
        it_sm74_314_00_0 = 118902400,
        it_sm74_315_00_0 = 118904000,
        it_sm74_316_00_0 = 118905600,
        it_sm74_317_00_0 = 118907200,
        it_sm74_318_00_0 = 118908800,
        it_sm74_319_00_0 = 118910400,
        it_sm74_320_00_0 = 118912000,
        it_sm74_321_00_0 = 118913600,
        it_sm74_322_00_0 = 118915200,
        it_sm74_323_00_0 = 118916800,
        it_sm74_324_00_0 = 118918400,
        it_sm74_325_00_0 = 118920000,
        it_sm74_326_00_0 = 118921600,
        it_sm74_327_00_0 = 118923200,
        it_sm75_300_00_0 = 120480000,
        it_sm75_301_00_0 = 120481600,
        it_sm75_302_00_0 = 120483200,
        it_sm75_303_00_0 = 120484800,
        it_sm75_304_00_0 = 120486400,
        it_sm75_500_00_0 = 120800000,
        it_sm75_501_00_0 = 120801600,
        it_sm75_502_00_0 = 120803200,
        it_sm75_506_00_0 = 120809600,
        it_sm75_508_00_0 = 120812800,
        it_sm75_509_00_0 = 120814400,
        it_sm75_510_00_0 = 120816000,
        it_sm75_511_00_0 = 120817600,
        it_sm75_512_00_0 = 120819200,
        it_sm75_513_00_0 = 120820800,
        it_sm75_514_00_0 = 120822400,
        it_sm75_515_00_0 = 120824000,
        it_sm75_516_00_0 = 120825600,
        it_sm75_517_00_0 = 120827200,
        it_sm75_518_00_0 = 120828800,
        it_sm75_519_00_0 = 120830400,
        it_sm75_520_00_0 = 120832000,
        it_sm75_521_00_0 = 120833600,
        it_sm75_522_00_0 = 120835200,
        it_sm75_524_00_0 = 120838400,
        it_sm75_525_00_0 = 120840000,
        it_sm75_526_00_0 = 120841600,
        it_sm75_527_00_0 = 120843200,
        it_sm75_528_00_0 = 120844800,
        it_sm75_529_00_0 = 120846400,
        it_sm75_530_00_0 = 120848000,
        it_sm75_531_00_0 = 120849600,
        it_sm75_532_00_0 = 120851200,
        it_sm75_533_00_0 = 120852800,
        it_sm75_534_00_0 = 120854400,
        it_sm75_535_00_0 = 120856000,
        it_sm75_536_00_0 = 120857600,
        it_sm75_538_00_0 = 120860800,
        it_sm75_540_00_0 = 120864000,
        it_sm75_541_00_0 = 120865600,
        it_sm75_542_00_0 = 120867200,
        it_sm75_600_00_0 = 120960000,
        it_sm76_500_00_0 = 122400000,
        it_sm76_501_00_0 = 122401600,
        it_sm76_502_00_0 = 122403200,
        it_sm76_503_00_0 = 122404800,
        it_sm76_504_00_0 = 122406400,
        it_sm76_505_00_0 = 122408000,
        it_sm76_506_00_0 = 122409600,
        it_sm76_507_00_0 = 122411200,
        it_sm76_508_00_0 = 122412800,
        it_sm76_510_00_0 = 122416000,
        it_sm77_500_00_0 = 124000000,
        it_sm77_501_00_0 = 124001600,
        it_sm77_502_00_0 = 124003200,
        it_sm77_503_00_0 = 124004800,
        it_sm77_504_00_0 = 124006400,
        it_sm77_505_00_0 = 124008000,
        it_sm77_600_00_0 = 124160000,
        it_sm77_601_00_0 = 124161600,
        it_sm77_602_00_0 = 124163200,
        it_sm77_603_00_0 = 124164800,
        it_sm77_604_00_0 = 124166400,
        it_sm77_610_00_0 = 124176000,
        it_sm77_611_00_0 = 124177600,
        it_sm77_612_00_0 = 124179200,
        it_sm77_620_00_0 = 124192000,
        it_sm77_621_00_0 = 124193600,
        it_sm77_701_00_0 = 124321600,
        it_sm77_702_00_0 = 124323200,
        it_sm77_703_00_0 = 124324800,
        it_sm77_704_00_0 = 124326400,
        it_sm77_705_00_0 = 124328000,
        it_sm77_706_00_0 = 124329600,
        it_sm77_707_00_0 = 124331200,
        it_sm77_708_00_0 = 124332800,
        it_sm77_709_00_0 = 124334400,
        it_sm77_710_00_0 = 124336000,
        it_sm77_711_00_0 = 124337600,
        it_sm77_721_00_0 = 124353600,
        it_sm77_722_00_0 = 124355200,
        it_sm77_723_00_0 = 124356800,
        it_sm77_724_00_0 = 124358400,
        it_sm77_725_00_0 = 124360000,
        it_sm77_726_00_0 = 124361600,
        it_sm77_727_00_0 = 124363200,
        it_sm77_728_00_0 = 124364800,
        it_sm77_729_00_0 = 124366400,
        it_sm77_730_00_0 = 124368000,
        it_sm77_302_00_0 = 123683200,
        it_sm77_303_00_0 = 123684800,
        it_sm77_304_00_0 = 123686400,
        it_sm77_305_00_0 = 123688000,
        it_sm77_900_00_0 = 124640000,
        it_sm77_901_00_0 = 124641600,
        it_sm77_902_00_0 = 124643200,
        it_sm77_903_00_0 = 124644800,
        it_sm77_904_00_0 = 124646400,
        it_sm77_905_00_0 = 124648000,
        it_sm79_500_00_0 = 127200000,
        it_sm79_501_00_0 = 127201600,
        it_sm79_502_00_0 = 127203200,
        it_sm79_503_00_0 = 127204800,
        it_sm79_504_00_0 = 127206400,
        it_sm79_505_00_0 = 127208000,
        it_sm79_506_00_0 = 127209600,
        it_sm79_507_00_0 = 127211200,
        it_sm79_508_00_0 = 127212800,
        it_sm79_509_00_0 = 127214400,
        it_sm79_510_00_0 = 127216000,
        it_sm79_511_00_0 = 127217600,
        it_sm79_512_00_0 = 127219200,
        it_sm79_513_00_0 = 127220800,
        it_sm79_514_00_0 = 127222400,
        it_sm79_515_00_0 = 127224000,
        it_sm79_516_00_0 = 127225600,
        it_sm79_517_00_0 = 127227200,
        it_sm79_518_00_0 = 127228800,
        it_sm79_519_00_0 = 127230400,
        it_sm79_520_00_0 = 127232000,
        it_sm79_521_00_0 = 127233600,
        it_sm79_522_00_0 = 127235200,
        it_sm79_523_00_0 = 127236800,
        it_sm79_524_00_0 = 127238400,
        it_sm79_525_00_0 = 127240000,
        it_sm79_526_00_0 = 127241600,
        it_sm79_527_00_0 = 127243200,
        it_sm79_528_00_0 = 127244800,
        it_sm79_529_00_0 = 127246400,
        it_sm79_600_00_0 = 127360000,
        it_sm79_601_00_0 = 127361600,
        SG09R = 274835456,
        it_wp4001_00_0 = 274837056,
        Red9 = 274838656,
        it_wp4003_00_0 = 274840256,
        it_wp4004_00_0 = 274841856,
        it_wp4005_00_0 = 274843456,
        W870 = 274995456,
        it_wp4101_00_0 = 274997056,
        it_wp4102_00_0 = 274998656,
        TMP = 275155456,
        it_wp4201_00_0 = 275157056,
        it_wp4202_00_0 = 275158656,
        SRM1903 = 275475456,
        it_wp4401_00_0 = 275477056,
        it_wp4402_00_0 = 275478656,
        it_wp4500_00_0 = 275635456,
        it_wp4501_00_0 = 275637056,
        it_wp4502_00_0 = 275638656,
        BoltThrower = 275795456,
        it_wp4600_10_0 = 275795616,
        it_wp4701_00_0 = 275957056,
        it_wp4702_00_0 = 275958656,
        it_wp4800_00_0 = 276115456,
        it_wp4801_00_0 = 276117056,
        RocketLauncher = 276275456,
        it_wp4901_00_0 = 276277056,
        it_wp4902_00_0 = 276278656,
        CombatKnife = 276435456,
        it_wp5001_00_0 = 276437056,
        KitchenKnife = 276438656,
        it_wp5003_00_0 = 276440256,
        it_wp5004_00_0 = 276441856,
        it_wp5005_00_0 = 276443456,
        it_wp5006_00_0 = 276445056,
        it_wp5300_00_0 = 276915456,
        it_wp5301_00_0 = 276917056,
        it_wp5302_00_0 = 276918656,
        HandGrenade = 277075456,
        it_wp5401_00_0 = 277077056,
        FlashGrenade = 277078656,
        ChickenEgg = 277080256,
        BrownChickenEgg = 277081856,
        it_wp5405_00_0 = 277083456,
        it_wp5406_00_0 = 277085056,
        it_wp5407_00_0 = 277086656,
        it_wp5408_00_0 = 277088256,
        it_wp5500_00_0 = 277235456,
        it_wp5501_00_0 = 277237056,
        it_wp5502_00_0 = 277238656,
        SentinelNine = 278035456,
        SkullShaker = 278037056,
        it_wp6100_00_0 = 278195456,
        it_wp6101_00_0 = 278197056,
        it_wp6102_00_0 = 278198656,
        it_wp6102_10_0 = 278198816,
        it_wp6103_00_0 = 278200256,
        it_wp6104_00_0 = 278201856,
        it_wp6105_00_0 = 278203456,
        it_wp6106_00_0 = 278205056,
        it_wp6107_00_0 = 278206656,
        it_wp6108_00_0 = 278208256,
        it_wp6109_00_0 = 278209856,
        it_wp6111_00_0 = 278213056,
        it_wp6300_00_0 = 278515456,
        it_wp6301_00_0 = 278517056,
        it_wp6302_00_0 = 278518656,
        it_wp6303_00_0 = 278520256,
        it_wp6303_10_0 = 278520416,
        it_wp6304_00_0 = 278521856,
        it_wp6304_10_0 = 278522016,
        it_wp6305_00_0 = 278523456,
        Dummy = 0,
        Invalid = -1,
    }

    public enum ItemSizeKind
    {
        Invalid = -1,
        _1x1_ = 0,
        _1x2_ = 1,
        _1x3_ = 2,
        _1x4_ = 3,
        _1x5_ = 4,
        _1x9_ = 5,
        _2x1_ = 6,
        _2x2_ = 7,
        _2x3_ = 8,
        _2x4_ = 9,
        _2x5_ = 10,
        _2x6_ = 11,
        _2x7_ = 12,
        _2x8_ = 13,
        _3x1_ = 14,
        _3x5_ = 15,
        _3x7_ = 16,
        _4x1_ = 17,
        _4x2_ = 18,
        _6x2_ = 19,
    }

    public enum Group : int
    {
        Case = 0,
        KeyItems = 1,
        Treasure = 2,
        Unique = 3,
    }
}