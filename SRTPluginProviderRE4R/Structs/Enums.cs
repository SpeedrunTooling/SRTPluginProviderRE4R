﻿namespace SRTPluginProducerRE4R.Structs
{
    public enum HPType
    {
        Text,
        Bar,
    }

    public enum HPPosition
    {
        Left,
        Center,
        Right,
        Custom,
    }

    public enum ChapterID
    {
        None = 0,
        Chapter1 = 21100,
        Chapter2 = 21200,
        Chapter3 = 21300,
        Chapter4 = 22100,
        Chapter5 = 22200,
        Chapter6 = 22300,
        Chapter7 = 23100,
        Chapter8 = 23200,
        Chapter9 = 23300,
        Chapter10 = 24100,
        Chapter11 = 24200,
        Chapter12 = 24300,
        Chapter13 = 25100,
        Chapter14 = 25200,
        Chapter15 = 25300,
        Chapter16 = 25400,
        Invalid = -1,
    }

    public enum CharacterKindID : int
    {
        Invalid = -1,
        None = 0,
        Leon = 100000,
        Ashley = 110000,
        ch0_zzzz = 199999,
        Villager = 200000,
        Zealot = 200001,
        Island_Ganado = 200002,
        Salvador = 200003,
        Colmillos = 200004,
        Novistador = 200005,
        El_Gigante = 200006,
        DelLago = 200007,
        Garrador = 200008,
        Las_Plagas = 200009,
        Brute = 200010,
        Krauser = 200011,
        Regenerador = 200012,
        Krauser2 = 200013,
        ch1_d5z0 = 200014,
        Verdugo = 200015,
        Soldier = 200016,
        Salazar = 200017,
        Sadler = 200018,
        Méndez = 200019,
        Méndez_Mutate = 200020,
        Méndez_Mutate_2 = 200021,
        ch4_d7z0 = 200022,
        ch4_f9z0 = 200023,
        ch4_faz0 = 200024,
        ch4_faz1 = 200025,
        Sadler2 = 200026,
        Sadler3 = 200027,
        ch4_fez0 = 200028,
        ch4_fbz0 = 200029,
        Ashley_ = 200030,
        ch2_a200 = 200031,
        Luis = 200032,
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
    }

    public enum PlayerState : int
    {
        Dead,
        Fine,
        Caution,
        Danger,
        Poisoned
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
        MagnumAmmo = 112801600,
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
        MixedHerbsGGG = 114406400,
        MixedHerbsGR = 114408000,
        MixedHerbsGY = 114409600,
        MixedHerbsRY = 114411200,
        MixedHerbsGRY = 114412800,
        MixedHerbsGGY = 114414400,
        FirstAidSpray = 114416000,
        BlackBass = 114417600,
        BlackBass2 = 114419200,
        BlackBass3 = 114420800,
        Viper = 114422400,
        Bug = 114424000,
        Scope = 116000000,
        Red9Stock = 116001600,
        HighPoweredScope = 116003200,
        BiosensorScope = 116004800,
        TMPStock = 116006400,
        LaserSight = 116008000,
        MatildaStock = 116009600,
        Gunpowder = 117600000,
        ResourcesL = 117601600,
        AttachableMines = 117603200,
        BrokenKitchenKnife = 117604800,
        ResourcesS = 117606400,
        it_sm74_502_00_0 = 119203200,
        InsigniaKey = 119204800,
        HaloWheel = 119206400,
        Lantern = 119209600,
        Level1Keycard = 119211200,
        Level2Keycard = 119212800,
        Level3Keycard = 119214400,
        CrystalMarble = 119217600,
        Wrench = 119219200,
        Shield = 119220800,
        LionHead = 119222400,
        GoatHead = 119224000,
        SnakeHead = 119225600,
        BlasphemersHead = 119230400,
        ApostatesHead = 119232000,
        Cog = 119235200,
        Dynamite = 119236800,
        LuisKey = 119238400,
        SmallKey = 119244800,
        Crank = 119246400,
        HexagonPieceA = 119248000,
        HexagonPieceB = 119249600,
        HexagonPieceC = 119251200,
        DungeonKey = 119254400,
        OldWayshrineKey = 119256000,
        ChurchInsignia = 119257600,
        WaterScooterKey = 119259200,
        it_sm74_538_00_0 = 119260800,
        it_sm74_539_00_0 = 119262400,
        LithographicStoneA = 119265600,
        LithographicStoneB = 119267200,
        LithographicStoneC = 119268800,
        LithographicStoneD = 119270400,
        BoatFuel = 119273600,
        it_sm74_547_00_0 = 119275200,
        CubicDevice = 119276800,
        BloodSword = 119278400,
        RustSword = 119280000,
        HuntersLodgeKey = 119281600,
        Unicorn = 119283200,
        BlueDial = 119284800,
        SilverToken = 119286400,
        GoldToken = 119288000,
        WoodPlanks = 119289600,
        it_sm74_557_00_0 = 119291200,
        Gems = 118880000,
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
        ElegantHeaddress = 120812800,
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
        BirdCrank = 124000000,
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
        Punisher = 274837056,
        Red9 = 274838656,
        Blacktail = 274840256,
        it_wp4004_00_0 = 274841856,
        it_wp4005_00_0 = 274843456,
        W870 = 274995456,
        RiotGun = 274997056,
        Striker = 274998656,
        TMP = 275155456,
        ChicagoSweeper = 275157056,
        LE5 = 275158656,
        SRM1903 = 275475456,
        Stingray = 275477056,
        CQBRAssaultRifle = 275478656,
        BrokenButterfly = 275635456,
        Killer7 = 275637056,
        Handcannon = 275638656,
        BoltThrower = 275795456,
        it_wp4600_10_0 = 275795616,
        it_wp4701_00_0 = 275957056,
        it_wp4702_00_0 = 275958656,
        it_wp4800_00_0 = 276115456,
        it_wp4801_00_0 = 276117056,
        RocketLauncher = 276275456,
        RocketLauncher2 = 276277056,
        RocketLauncher3 = 276278656,
        CombatKnife = 276435456,
        FightingKnife = 276437056,
        KitchenKnife = 276438656,
        BootKnife = 276440256,
        it_wp5004_00_0 = 276441856,
        it_wp5005_00_0 = 276443456,
        it_wp5006_00_0 = 276445056,
        it_wp5300_00_0 = 276915456,
        it_wp5301_00_0 = 276917056,
        it_wp5302_00_0 = 276918656,
        HandGrenade = 277075456,
        HeavyGrenade = 277077056,
        FlashGrenade = 277078656,
        ChickenEgg = 277080256,
        BrownChickenEgg = 277081856,
        GoldChickenEgg = 277083456,
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
