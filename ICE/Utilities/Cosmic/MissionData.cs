using Dalamud.Interface.Textures;
using ECommons.GameHelpers;
using ICE.Enums;
using System.Collections.Generic;

namespace ICE.Utilities;

public static unsafe partial class CosmicHelper
{

    public static readonly HashSet<uint> Ranks = [1, 2, 3, 4];
    public static readonly HashSet<uint> ARankIds = [4, 5, 6];

    public static readonly HashSet<uint> CrafterJobList = [8, 9, 10, 11, 12, 13, 14, 15];
    public static readonly HashSet<uint> GatheringJobList = [16, 17, 18];

    public static readonly HashSet<int> WeatherMissionList = [30, 31, 32,];
    public static readonly HashSet<int> TimedMissionList = [40, 43,];
    public static readonly HashSet<int> CriticalMissions = [512, 513, 514,];

    /// <summary>
    /// Currently contains all the WKSMissionLotterySpecialCond values that are weather based
    /// MAKE SURE. TO UPDATE THIS. COME NEW MOON
    /// </summary>
    public static readonly HashSet<uint> WeatherSelection = new() { 13, 14, 15, 16 };

    public static List<int> GreyIconList = new List<int>() { 91031, 91032, 91033, 91034, 91035, 91036, 91037, 91038, 91039, 91040, 91041 };

    public static readonly int MinimumLevel = 10;
    public static readonly int MaximumLevel = Player.MaxLevel;

    #region Dictionaries

    /// <summary>
    /// Some things to note that I didn't realize until after I really dug into the sheet a bit more/cleaned this up. <br />
    /// Sheet is: WKSMissionUnit <br />
    /// <b>- Row 0:</b> Mission Name <br />
    /// <b>- Row 2:</b> JobId attached to the quest (so 8 is CRP, 9 is BSM, etc.) <br />
    /// <b>- Row 3:</b> 2nd Required job??? <br />
    /// <b>- Row 4:</b> 3rd Required job??? <br />
    /// <b>- Row 5:</b> Bool → Is it a critical mission? <br />
    /// <b>- Row 6:</b> Rank → D = 1 | C = 2 | B = 3 | 4 = A-1 | 5 = A-2 | 6 = A-3 <br />
    /// <b>- Row 7:</b> Mission time limit (seconds) <br />
    /// <b>- Row 18:</b> Recipe # → Corresponds to the RecipeID
    /// </summary>

    public class CosmicInfo
    {
        // - - - Fishing Specific - - - //

        /// <summary>
        /// Applies to: ScoreTimeRemaining | Score Variety <br></br>
        /// The required fish that can complete the conditions for either of these attributes
        /// </summary>
        public Dictionary<string, HashSet<uint>> RequiredFish { get; set; } = new();
        /// <summary>
        /// If a mission is a timed based mission (aka. Gather x amount of fish within a certain amount of time) <br></br>
        /// Applies to: ScoreTimeRemaining | ScoreVariety
        /// </summary>
        public uint FishCountRequired { get; set; } = 0;
        public Dictionary<string, HashSet<uint>> FishingBait { get; set; } = new();

        // - - - Crafter Specific - - - //
        public Dictionary<ushort, int> Crafts_Main { get; set; } = new();
        public Dictionary<ushort, int> Crafts_Pre { get; set; } = new();

        // - - - BTN | MIN Specific - - - //
        public Dictionary<uint, int> Gathering_Min { get; set; } = new();

        // - - - Map Related - - - // 
        public Vector2 MapPosition { get; set; } = new();
        public int Radius { get; set; } = 0;
        public uint TerritoryId { get; set; }
        public uint MarkerId { get; set; }

        // - - - Universal Info - - - //
        public string Name { get; set; }
        public HashSet<uint> Jobs { get; set; }
        public uint ToDoId { get; set; } = 0;
        public uint Rank { get; set; } = 1;
        public MissionAttributes Attributes { get; set; }
        public CosmicWeather Weather { get; set; }
        public uint StartTime { get; set; }
        public uint EndTime { get; set; }
        public uint ClassScore { get; set; } = 0;
        public uint CosmoCredit { get; set; } = 0;
        public uint LunarCredit { get; set; } = 0;
        public HashSet<uint> PreviousMissions { get; set; } = new();
        public Dictionary<int, int> RelicXpInfo { get; set; } = new();
        public uint BronzeScore { get; set; } = 0;
        public uint SilverScore { get; set; } = 0;
        public uint GoldScore { get; set; } = 0;
    }

    public static Dictionary<uint, CosmicInfo> SheetMissionDict = new();
    public class MoonRecipieInfo
    {
        public Dictionary<ushort, int> MainCraftsDict = [];
        public bool PreCrafts { get; set; } = false;
        public Dictionary<ushort, int> PreCraftDict = [];
    }

    public static Dictionary<uint, MoonRecipieInfo> MoonRecipies = [];

    public class GatheringInfo
    {
        public Dictionary<uint, int> MinGatherItems = [];
    }

    public static Dictionary<uint, GatheringInfo> GatheringItemDict = new();

    public static Dictionary<uint, ISharedImmediateTexture> GreyTexture = new Dictionary<uint, ISharedImmediateTexture>();

    public static Dictionary<uint, ISharedImmediateTexture> JobIconDict = new Dictionary<uint, ISharedImmediateTexture>();

    public static Dictionary<uint, uint> MissionScoreDict = new Dictionary<uint, uint>
    {
        [1] = 2,
        [2] = 2,
        [3] = 2,
        [4] = 2,
        [5] = 2,
        [6] = 2,
        [7] = 5,
        [8] = 6,
        [9] = 6,
        [10] = 6,
        [11] = 8,
        [12] = 8,
        [13] = 6,
        [14] = 16,
        [15] = 13,
        [16] = 9,
        [17] = 16,
        [18] = 18,
        [19] = 19,
        [20] = 17,
        [21] = 17,
        [22] = 54,
        [23] = 51,
        [24] = 52,
        [25] = 41,
        [26] = 84,
        [27] = 107,
        [28] = 82,
        [29] = 82,
        [30] = 316,
        [31] = 304,
        [32] = 323,
        [33] = 44,
        [34] = 121,
        [35] = 31,
        [36] = 91,
        [37] = 194,
        [38] = 83,
        [39] = 154,
        [40] = 182,
        [41] = 211,
        [42] = 290,
        [43] = 416,
        [44] = 0,
        [45] = 0,
        [46] = 2,
        [47] = 2,
        [48] = 2,
        [49] = 2,
        [50] = 2,
        [51] = 2,
        [52] = 5,
        [53] = 6,
        [54] = 6,
        [55] = 6,
        [56] = 8,
        [57] = 8,
        [58] = 6,
        [59] = 0,
        [60] = 0,
        [61] = 0,
        [62] = 0,
        [63] = 0,
        [64] = 0,
        [65] = 0,
        [66] = 0,
        [67] = 0,
        [68] = 0,
        [69] = 0,
        [70] = 0,
        [71] = 0,
        [72] = 0,
        [73] = 0,
        [74] = 0,
        [75] = 0,
        [76] = 0,
        [77] = 0,
        [78] = 0,
        [79] = 0,
        [80] = 0,
        [81] = 0,
        [82] = 0,
        [83] = 0,
        [84] = 0,
        [85] = 0,
        [86] = 0,
        [87] = 0,
        [88] = 0,
        [89] = 0,
        [90] = 0,
        [91] = 2,
        [92] = 2,
        [93] = 2,
        [94] = 2,
        [95] = 2,
        [96] = 2,
        [97] = 5,
        [98] = 6,
        [99] = 6,
        [100] = 6,
        [101] = 8,
        [102] = 8,
        [103] = 6,
        [104] = 0,
        [105] = 0,
        [106] = 0,
        [107] = 0,
        [108] = 0,
        [109] = 0,
        [110] = 0,
        [111] = 0,
        [112] = 0,
        [113] = 0,
        [114] = 0,
        [115] = 0,
        [116] = 0,
        [117] = 0,
        [118] = 0,
        [119] = 0,
        [120] = 0,
        [121] = 0,
        [122] = 0,
        [123] = 0,
        [124] = 0,
        [125] = 0,
        [126] = 0,
        [127] = 0,
        [128] = 0,
        [129] = 0,
        [130] = 0,
        [131] = 0,
        [132] = 0,
        [133] = 0,
        [134] = 0,
        [135] = 0,
        [136] = 0,
        [137] = 0,
        [138] = 0,
        [139] = 0,
        [140] = 0,
        [141] = 0,
        [142] = 5,
        [143] = 6,
        [144] = 6,
        [145] = 6,
        [146] = 8,
        [147] = 8,
        [148] = 6,
        [149] = 0,
        [150] = 0,
        [151] = 0,
        [152] = 0,
        [153] = 0,
        [154] = 0,
        [155] = 0,
        [156] = 0,
        [157] = 0,
        [158] = 0,
        [159] = 0,
        [160] = 0,
        [161] = 0,
        [162] = 0,
        [163] = 0,
        [164] = 0,
        [165] = 0,
        [166] = 0,
        [167] = 0,
        [168] = 0,
        [169] = 0,
        [170] = 0,
        [171] = 0,
        [172] = 0,
        [173] = 0,
        [174] = 0,
        [175] = 0,
        [176] = 0,
        [177] = 0,
        [178] = 0,
        [179] = 0,
        [180] = 0,
        [181] = 0,
        [182] = 0,
        [183] = 0,
        [184] = 0,
        [185] = 0,
        [186] = 0,
        [187] = 5,
        [188] = 6,
        [189] = 6,
        [190] = 6,
        [191] = 8,
        [192] = 8,
        [193] = 6,
        [194] = 0,
        [195] = 0,
        [196] = 0,
        [197] = 0,
        [198] = 0,
        [199] = 0,
        [200] = 0,
        [201] = 0,
        [202] = 0,
        [203] = 0,
        [204] = 0,
        [205] = 0,
        [206] = 0,
        [207] = 0,
        [208] = 0,
        [209] = 0,
        [210] = 0,
        [211] = 0,
        [212] = 0,
        [213] = 0,
        [214] = 0,
        [215] = 0,
        [216] = 0,
        [217] = 0,
        [218] = 0,
        [219] = 0,
        [220] = 0,
        [221] = 0,
        [222] = 0,
        [223] = 0,
        [224] = 0,
        [225] = 0,
        [226] = 0,
        [227] = 0,
        [228] = 0,
        [229] = 0,
        [230] = 0,
        [231] = 0,
        [232] = 5,
        [233] = 6,
        [234] = 6,
        [235] = 6,
        [236] = 8,
        [237] = 8,
        [238] = 6,
        [239] = 0,
        [240] = 0,
        [241] = 0,
        [242] = 0,
        [243] = 0,
        [244] = 0,
        [245] = 0,
        [246] = 0,
        [247] = 0,
        [248] = 0,
        [249] = 0,
        [250] = 0,
        [251] = 0,
        [252] = 0,
        [253] = 0,
        [254] = 0,
        [255] = 0,
        [256] = 0,
        [257] = 0,
        [258] = 0,
        [259] = 0,
        [260] = 0,
        [261] = 0,
        [262] = 0,
        [263] = 0,
        [264] = 0,
        [265] = 0,
        [266] = 0,
        [267] = 0,
        [268] = 0,
        [269] = 0,
        [270] = 0,
        [271] = 0,
        [272] = 0,
        [273] = 0,
        [274] = 0,
        [275] = 0,
        [276] = 0,
        [277] = 5,
        [278] = 6,
        [279] = 6,
        [280] = 6,
        [281] = 8,
        [282] = 8,
        [283] = 6,
        [284] = 0,
        [285] = 0,
        [286] = 0,
        [287] = 0,
        [288] = 0,
        [289] = 0,
        [290] = 0,
        [291] = 0,
        [292] = 0,
        [293] = 0,
        [294] = 0,
        [295] = 0,
        [296] = 0,
        [297] = 0,
        [298] = 0,
        [299] = 0,
        [300] = 0,
        [301] = 0,
        [302] = 0,
        [303] = 0,
        [304] = 0,
        [305] = 0,
        [306] = 0,
        [307] = 0,
        [308] = 0,
        [309] = 0,
        [310] = 0,
        [311] = 0,
        [312] = 0,
        [313] = 0,
        [314] = 0,
        [315] = 0,
        [316] = 0,
        [317] = 0,
        [318] = 0,
        [319] = 0,
        [320] = 0,
        [321] = 0,
        [322] = 5,
        [323] = 6,
        [324] = 6,
        [325] = 6,
        [326] = 8,
        [327] = 8,
        [328] = 6,
        [329] = 0,
        [330] = 0,
        [331] = 0,
        [332] = 0,
        [333] = 0,
        [334] = 0,
        [335] = 0,
        [336] = 0,
        [337] = 0,
        [338] = 0,
        [339] = 0,
        [340] = 0,
        [341] = 0,
        [342] = 0,
        [343] = 0,
        [344] = 0,
        [345] = 0,
        [346] = 0,
        [347] = 0,
        [348] = 0,
        [349] = 0,
        [350] = 0,
        [351] = 0,
        [352] = 0,
        [353] = 0,
        [354] = 0,
        [355] = 0,
        [356] = 0,
        [357] = 0,
        [358] = 0,
        [359] = 0,
        [360] = 0,
        [361] = 0,
        [362] = 0,
        [363] = 0,
        [364] = 0,
        [365] = 0,
        [366] = 0,
        [367] = 7,
        [368] = 3,
        [369] = 6,
        [370] = 6,
        [371] = 6,
        [372] = 6,
        [373] = 7,
        [374] = 0,
        [375] = 0,
        [376] = 0,
        [377] = 0,
        [378] = 0,
        [379] = 0,
        [380] = 0,
        [381] = 0,
        [382] = 0,
        [383] = 0,
        [384] = 0,
        [385] = 0,
        [386] = 0,
        [387] = 0,
        [388] = 0,
        [389] = 0,
        [390] = 0,
        [391] = 0,
        [392] = 0,
        [393] = 0,
        [394] = 0,
        [395] = 0,
        [396] = 0,
        [397] = 0,
        [398] = 0,
        [399] = 0,
        [400] = 0,
        [401] = 0,
        [402] = 0,
        [403] = 0,
        [404] = 0,
        [405] = 0,
        [406] = 0,
        [407] = 0,
        [408] = 0,
        [409] = 0,
        [410] = 0,
        [411] = 0,
        [412] = 0,
        [413] = 0,
        [414] = 0,
        [415] = 0,
        [416] = 0,
        [417] = 0,
        [418] = 0,
        [419] = 0,
        [420] = 0,
        [421] = 0,
        [422] = 0,
        [423] = 0,
        [424] = 0,
        [425] = 0,
        [426] = 0,
        [427] = 0,
        [428] = 0,
        [429] = 0,
        [430] = 0,
        [431] = 0,
        [432] = 0,
        [433] = 0,
        [434] = 0,
        [435] = 0,
        [436] = 0,
        [437] = 0,
        [438] = 0,
        [439] = 0,
        [440] = 0,
        [441] = 0,
        [442] = 0,
        [443] = 0,
        [444] = 0,
        [445] = 0,
        [446] = 0,
        [447] = 0,
        [448] = 0,
        [449] = 0,
        [450] = 0,
        [451] = 0,
        [452] = 0,
        [453] = 0,
        [454] = 0,
        [455] = 0,
        [456] = 0,
        [457] = 0,
        [458] = 0,
        [459] = 0,
        [460] = 0,
        [461] = 0,
        [462] = 0,
        [463] = 0,
        [464] = 0,
        [465] = 0,
        [466] = 0,
        [467] = 0,
        [468] = 0,
        [469] = 0,
        [470] = 0,
        [471] = 0,
        [472] = 0,
        [473] = 0,
        [474] = 0,
        [475] = 0,
        [476] = 0,
        [477] = 0,
        [478] = 0,
        [479] = 0,
        [480] = 0,
        [481] = 0,
        [482] = 0,
        [483] = 0,
        [484] = 0,
        [485] = 0,
        [486] = 0,
        [487] = 0,
        [488] = 0,
        [489] = 0,
        [490] = 0,
        [491] = 0,
        [492] = 0,
        [493] = 0,
        [494] = 0,
        [495] = 0,
        [496] = 134,
        [497] = 0,
        [498] = 0,
        [499] = 0,
        [500] = 0,
        [501] = 0,
        [502] = 0,
        [503] = 0,
        [504] = 0,
        [505] = 246,
        [506] = 0,
        [507] = 0,
        [508] = 0,
        [509] = 0,
        [510] = 0,
        [511] = 0,
        [512] = 383,
        [513] = 400,
        [514] = 417,
        [515] = 382,
        [516] = 0,
        [517] = 417,
        [518] = 391,
        [519] = 386,
        [520] = 0,
        [521] = 0,
        [522] = 0,
        [523] = 0,
        [524] = 0,
        [525] = 0,
        [526] = 0,
        [527] = 0,
        [528] = 0,
        [529] = 0,
        [530] = 0,
        [531] = 0,
        [532] = 0,
        [533] = 0,
        [534] = 0,
        [535] = 0,
        [536] = 0,
        [537] = 0,
        [538] = 0,
        [539] = 0,
        [540] = 0,
        [541] = 0,
        [542] = 0,
        [543] = 0,
        [544] = 0,
    };

    public class GatherItemInfo
    {
        public HashSet<uint> itemIds { get; set; } = new();
        public uint Type { get; set; } = 0;
    }
    public static Dictionary<string, GatherItemInfo> GatheringItems = new();

    public class XPType
    {
        public int CurrentXP { get; set; }
        public int NeededXP { get; set; }
    }

    public static Dictionary<uint, CosmicInfo> Dict_CosmicMissions = new()
    {
        [1] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 2,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36165] = 3,
            },
        },
        [2] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 2,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36166] = 3,
            },
        },
        [3] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 2,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36167] = 3,
            },
        },
        [4] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 2,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36168] = 3,
            },
        },
        [5] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 2,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36169] = 3,
            },
        },
        [6] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 2,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36170] = 3,
            },
        },
        [7] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 5,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36171] = 3,
            },
        },
        [8] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 6,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36172] = 3,
            },
        },
        [9] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36173] = 3,
            },
        },
        [10] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36175] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36174] = 2,
            },
        },
        [11] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 8,
            CosmoCredit = 4,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 9,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36177] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36176] = 2,
            },
        },
        [12] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2250,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 8,
            CosmoCredit = 4,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36179] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36178] = 3,
            },
        },
        [13] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36181] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36180] = 2,
            },
        },
        [14] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 16,
            CosmoCredit = 7,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 9,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36182] = 3,
            },
        },
        [15] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 13,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36183] = 3,
            },
        },
        [16] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1700,
            GoldScore = 2000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 9,
            CosmoCredit = 4,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 5,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36184] = 2,
            },
        },
        [17] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 16,
            CosmoCredit = 7,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 9,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36185] = 3,
            },
        },
        [18] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 18,
            CosmoCredit = 8,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36186] = 3,
            },
        },
        [19] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 19,
            CosmoCredit = 8,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36187] = 3,
            },
        },
        [20] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 17,
            CosmoCredit = 8,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 7,
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36189] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36188] = 2,
            },
        },
        [21] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 17,
            CosmoCredit = 8,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [2] = 6,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36191] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36190] = 2,
            },
        },
        [22] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 54,
            CosmoCredit = 19,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 13,
                [1] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36192] = 3,
            },
        },
        [23] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 51,
            CosmoCredit = 18,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 18,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36193] = 3,
            },
        },
        [24] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 52,
            CosmoCredit = 19,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36194] = 1,
            },
        },
        [25] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 41,
            CosmoCredit = 15,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36195] = 1,
            },
        },
        [26] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2600,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 84,
            CosmoCredit = 30,
            LunarCredit = 28,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 30,
                [2] = 20,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36196] = 1,
                [36197] = 1,
                [36198] = 1,
            },
        },
        [27] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 107,
            CosmoCredit = 19,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 19,
                [1] = 13,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36199] = 3,
            },
        },
        [28] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 840,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 82,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36200] = 1,
            },
        },
        [29] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 840,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 82,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36201] = 1,
            },
        },
        [30] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2800,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.UmbralWind,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 316,
            CosmoCredit = 31,
            LunarCredit = 34,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 24,
                [3] = 16,
                [1] = 16,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36202] = 1,
                [36203] = 1,
                [36204] = 1,
            },
        },
        [31] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 560,
            GoldScore = 880,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 304,
            CosmoCredit = 30,
            LunarCredit = 33,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 23,
                [3] = 15,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36206] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36205] = 1,
            },
        },
        [32] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 560,
            GoldScore = 880,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.UmbralWind,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 323,
            CosmoCredit = 32,
            LunarCredit = 35,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 25,
                [2] = 16,
                [1] = 16,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36208] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36207] = 1,
            },
        },
        [33] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 44,
            CosmoCredit = 16,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 16,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36209] = 3,
            },
        },
        [34] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 121,
            CosmoCredit = 19,
            LunarCredit = 21,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 33 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 10,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36210] = 3,
            },
        },
        [35] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 31,
            CosmoCredit = 11,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36211] = 2,
            },
        },
        [36] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 91,
            CosmoCredit = 14,
            LunarCredit = 16,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 35 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 11,
                [3] = 8,
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36212] = 2,
            },
        },
        [37] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 194,
            CosmoCredit = 13,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 36 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [3] = 7,
                [2] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36213] = 2,
            },
        },
        [38] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 83,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [2] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36214] = 1,
            },
        },
        [39] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 900,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 154,
            CosmoCredit = 15,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 38 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 12,
                [3] = 8,
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36215] = 1,
            },
        },
        [40] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 2,
            ClassScore = 182,
            CosmoCredit = 18,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [3] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36216] = 3,
            },
        },
        [41] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 211,
            CosmoCredit = 19,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 40 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 11,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36217] = 3,
            },
        },
        [42] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 290,
            CosmoCredit = 17,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 41 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 14,
                [2] = 10,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36218] = 3,
            },
        },
        [43] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 12,
            EndTime = 14,
            ClassScore = 416,
            CosmoCredit = 35,
            LunarCredit = 42,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 27,
                [3] = 17,
                [1] = 17,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36220] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36219] = 1,
            },
        },
        [44] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 35,
            LunarCredit = 45,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 43 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 27,
                [3] = 19,
                [2] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36223] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36222] = 1,
            },
        },
        [45] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 84,
            LunarCredit = 117,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 44 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 84,
                [2] = 51,
                [1] = 51,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36227] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36225] = 1,
            },
        },
        [46] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 2,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36228] = 3,
            },
        },
        [47] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 2,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36229] = 3,
            },
        },
        [48] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 2,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36230] = 3,
            },
        },
        [49] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 2,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36231] = 3,
            },
        },
        [50] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 2,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36232] = 3,
            },
        },
        [51] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 2,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36233] = 3,
            },
        },
        [52] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 5,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36234] = 3,
            },
        },
        [53] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 6,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36235] = 3,
            },
        },
        [54] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36236] = 3,
            },
        },
        [55] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36238] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36237] = 2,
            },
        },
        [56] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 8,
            CosmoCredit = 4,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 9,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36240] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36239] = 2,
            },
        },
        [57] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2250,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 8,
            CosmoCredit = 4,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36242] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36241] = 3,
            },
        },
        [58] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36244] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36243] = 2,
            },
        },
        [59] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 9,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36245] = 3,
            },
        },
        [60] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36246] = 3,
            },
        },
        [61] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1700,
            GoldScore = 2000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 5,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36247] = 2,
            },
        },
        [62] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 9,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36248] = 3,
            },
        },
        [63] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36249] = 3,
            },
        },
        [64] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36250] = 3,
            },
        },
        [65] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 7,
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36252] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36251] = 2,
            },
        },
        [66] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [2] = 6,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36254] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36253] = 2,
            },
        },
        [67] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 13,
                [1] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36255] = 3,
            },
        },
        [68] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 18,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36256] = 3,
            },
        },
        [69] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36257] = 1,
            },
        },
        [70] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36258] = 1,
            },
        },
        [71] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2600,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 30,
            LunarCredit = 28,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 30,
                [2] = 20,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36259] = 1,
                [36260] = 1,
                [36261] = 1,
            },
        },
        [72] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 19,
                [1] = 13,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36262] = 3,
            },
        },
        [73] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 840,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36263] = 1,
            },
        },
        [74] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 840,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36264] = 1,
            },
        },
        [75] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2800,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 31,
            LunarCredit = 34,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 24,
                [3] = 16,
                [1] = 16,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36265] = 1,
                [36266] = 1,
                [36267] = 1,
            },
        },
        [76] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 560,
            GoldScore = 880,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.UmbralWind,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 30,
            LunarCredit = 33,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 23,
                [3] = 15,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36269] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36268] = 1,
            },
        },
        [77] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 560,
            GoldScore = 880,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 32,
            LunarCredit = 35,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 25,
                [2] = 16,
                [1] = 16,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36271] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36270] = 1,
            },
        },
        [78] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 16,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 16,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36272] = 3,
            },
        },
        [79] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 21,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 78 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 10,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36273] = 3,
            },
        },
        [80] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36274] = 2,
            },
        },
        [81] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 16,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 80 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 11,
                [3] = 8,
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36275] = 2,
            },
        },
        [82] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 13,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 81 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [3] = 7,
                [2] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36276] = 2,
            },
        },
        [83] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [2] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36277] = 1,
            },
        },
        [84] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 900,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 83 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 12,
                [3] = 8,
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36278] = 1,
            },
        },
        [85] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 4,
            EndTime = 6,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [3] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36279] = 3,
            },
        },
        [86] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 85 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 11,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36280] = 3,
            },
        },
        [87] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 86 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 14,
                [2] = 10,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36281] = 3,
            },
        },
        [88] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 16,
            EndTime = 18,
            ClassScore = 0,
            CosmoCredit = 35,
            LunarCredit = 42,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 27,
                [3] = 17,
                [1] = 17,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36283] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36282] = 1,
            },
        },
        [89] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 35,
            LunarCredit = 45,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 88 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 27,
                [3] = 19,
                [2] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36286] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36285] = 1,
            },
        },
        [90] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 84,
            LunarCredit = 117,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 89 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 84,
                [2] = 51,
                [1] = 51,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36290] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36288] = 1,
            },
        },
        [91] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 2,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36291] = 3,
            },
        },
        [92] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 2,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36292] = 3,
            },
        },
        [93] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 2,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36293] = 3,
            },
        },
        [94] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 2,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36294] = 3,
            },
        },
        [95] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 2,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36295] = 3,
            },
        },
        [96] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 2,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36296] = 3,
            },
        },
        [97] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 5,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36297] = 3,
            },
        },
        [98] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 6,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36298] = 3,
            },
        },
        [99] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36299] = 3,
            },
        },
        [100] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36301] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36300] = 2,
            },
        },
        [101] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 8,
            CosmoCredit = 4,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 9,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36303] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36302] = 2,
            },
        },
        [102] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2250,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 8,
            CosmoCredit = 4,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36305] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36304] = 3,
            },
        },
        [103] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36307] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36306] = 2,
            },
        },
        [104] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 9,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36308] = 3,
            },
        },
        [105] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36309] = 3,
            },
        },
        [106] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1700,
            GoldScore = 2000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 5,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36310] = 2,
            },
        },
        [107] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 9,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36311] = 3,
            },
        },
        [108] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36312] = 3,
            },
        },
        [109] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36313] = 3,
            },
        },
        [110] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 7,
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36315] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36314] = 2,
            },
        },
        [111] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [2] = 6,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36317] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36316] = 2,
            },
        },
        [112] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 13,
                [1] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36318] = 3,
            },
        },
        [113] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 18,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36319] = 3,
            },
        },
        [114] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36320] = 1,
            },
        },
        [115] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36321] = 1,
            },
        },
        [116] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2600,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 30,
            LunarCredit = 28,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 30,
                [2] = 20,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36322] = 1,
                [36323] = 1,
                [36324] = 1,
            },
        },
        [117] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 19,
                [1] = 13,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36325] = 3,
            },
        },
        [118] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 840,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36326] = 1,
            },
        },
        [119] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 840,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36327] = 1,
            },
        },
        [120] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2800,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.UmbralWind,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 31,
            LunarCredit = 34,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 24,
                [3] = 16,
                [1] = 16,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36328] = 1,
                [36329] = 1,
                [36330] = 1,
            },
        },
        [121] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 560,
            GoldScore = 880,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.UmbralWind,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 30,
            LunarCredit = 33,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 23,
                [3] = 15,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36332] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36331] = 1,
            },
        },
        [122] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 560,
            GoldScore = 880,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 32,
            LunarCredit = 35,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 25,
                [2] = 16,
                [1] = 16,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36334] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36333] = 1,
            },
        },
        [123] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 16,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 16,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36335] = 3,
            },
        },
        [124] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 21,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 123 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 10,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36336] = 3,
            },
        },
        [125] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36337] = 2,
            },
        },
        [126] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 16,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 125 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 11,
                [3] = 8,
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36338] = 2,
            },
        },
        [127] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 13,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 126 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [3] = 7,
                [2] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36339] = 2,
            },
        },
        [128] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [2] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36340] = 1,
            },
        },
        [129] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 900,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 128 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 12,
                [3] = 8,
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36341] = 1,
            },
        },
        [130] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 8,
            EndTime = 10,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [3] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36342] = 3,
            },
        },
        [131] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 130 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 11,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36343] = 3,
            },
        },
        [132] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 131 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 14,
                [2] = 10,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36344] = 3,
            },
        },
        [133] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 20,
            EndTime = 22,
            ClassScore = 0,
            CosmoCredit = 35,
            LunarCredit = 42,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 27,
                [3] = 17,
                [1] = 17,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36346] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36345] = 1,
            },
        },
        [134] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 35,
            LunarCredit = 45,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 133 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 27,
                [3] = 19,
                [2] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36349] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36348] = 1,
            },
        },
        [135] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 84,
            LunarCredit = 117,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 134 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 84,
                [2] = 51,
                [1] = 51,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36353] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36351] = 1,
            },
        },
        [136] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36354] = 3,
            },
        },
        [137] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36355] = 3,
            },
        },
        [138] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36356] = 3,
            },
        },
        [139] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36357] = 3,
            },
        },
        [140] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36358] = 3,
            },
        },
        [141] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36359] = 3,
            },
        },
        [142] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 5,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36360] = 3,
            },
        },
        [143] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 6,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36361] = 3,
            },
        },
        [144] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36362] = 3,
            },
        },
        [145] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36364] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36363] = 2,
            },
        },
        [146] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 8,
            CosmoCredit = 4,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 9,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36366] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36365] = 2,
            },
        },
        [147] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2250,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 8,
            CosmoCredit = 4,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36368] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36367] = 3,
            },
        },
        [148] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36370] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36369] = 2,
            },
        },
        [149] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 9,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36371] = 3,
            },
        },
        [150] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36372] = 3,
            },
        },
        [151] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1700,
            GoldScore = 2000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 5,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36373] = 2,
            },
        },
        [152] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 9,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36374] = 3,
            },
        },
        [153] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36375] = 3,
            },
        },
        [154] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36376] = 3,
            },
        },
        [155] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 7,
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36378] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36377] = 2,
            },
        },
        [156] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [2] = 6,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36380] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36379] = 2,
            },
        },
        [157] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 13,
                [1] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36381] = 3,
            },
        },
        [158] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 18,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36382] = 3,
            },
        },
        [159] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36383] = 1,
            },
        },
        [160] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36384] = 1,
            },
        },
        [161] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2600,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 30,
            LunarCredit = 28,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 30,
                [2] = 20,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36385] = 1,
                [36386] = 1,
                [36387] = 1,
            },
        },
        [162] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 19,
                [1] = 13,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36388] = 3,
            },
        },
        [163] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 840,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36389] = 1,
            },
        },
        [164] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 840,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36390] = 1,
            },
        },
        [165] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2800,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 31,
            LunarCredit = 34,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 24,
                [3] = 16,
                [1] = 16,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36391] = 1,
                [36392] = 1,
                [36393] = 1,
            },
        },
        [166] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 560,
            GoldScore = 880,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.UmbralWind,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 30,
            LunarCredit = 33,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 23,
                [3] = 15,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36395] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36394] = 1,
            },
        },
        [167] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 560,
            GoldScore = 880,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 32,
            LunarCredit = 35,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 25,
                [2] = 16,
                [1] = 16,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36397] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36396] = 1,
            },
        },
        [168] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 16,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 16,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36398] = 3,
            },
        },
        [169] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 21,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 168 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 10,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36399] = 3,
            },
        },
        [170] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36400] = 2,
            },
        },
        [171] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 16,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 170 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 11,
                [3] = 8,
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36401] = 2,
            },
        },
        [172] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 13,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 171 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [3] = 7,
                [2] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36402] = 2,
            },
        },
        [173] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [2] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36403] = 1,
            },
        },
        [174] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 900,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 173 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 12,
                [3] = 8,
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36404] = 1,
            },
        },
        [175] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 12,
            EndTime = 14,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [3] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36405] = 3,
            },
        },
        [176] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 175 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 11,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36406] = 3,
            },
        },
        [177] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 176 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 14,
                [2] = 10,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36407] = 3,
            },
        },
        [178] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 2,
            ClassScore = 0,
            CosmoCredit = 35,
            LunarCredit = 42,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 27,
                [3] = 17,
                [1] = 17,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36409] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36408] = 1,
            },
        },
        [179] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 35,
            LunarCredit = 45,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 178 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 27,
                [3] = 19,
                [2] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36412] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36411] = 1,
            },
        },
        [180] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 84,
            LunarCredit = 117,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 179 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 84,
                [2] = 51,
                [1] = 51,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36416] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36414] = 1,
            },
        },
        [181] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36417] = 3,
            },
        },
        [182] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36418] = 3,
            },
        },
        [183] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36419] = 3,
            },
        },
        [184] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36420] = 3,
            },
        },
        [185] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36421] = 3,
            },
        },
        [186] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36422] = 3,
            },
        },
        [187] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 5,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36423] = 3,
            },
        },
        [188] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 6,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36424] = 3,
            },
        },
        [189] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36425] = 3,
            },
        },
        [190] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36427] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36426] = 2,
            },
        },
        [191] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 8,
            CosmoCredit = 4,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 9,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36429] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36428] = 2,
            },
        },
        [192] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2250,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 8,
            CosmoCredit = 4,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36431] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36430] = 3,
            },
        },
        [193] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36433] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36432] = 2,
            },
        },
        [194] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 9,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36434] = 3,
            },
        },
        [195] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36435] = 3,
            },
        },
        [196] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1700,
            GoldScore = 2000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 5,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36436] = 2,
            },
        },
        [197] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 9,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36437] = 3,
            },
        },
        [198] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36438] = 3,
            },
        },
        [199] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36439] = 3,
            },
        },
        [200] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 7,
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36441] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36440] = 2,
            },
        },
        [201] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [2] = 6,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36443] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36442] = 2,
            },
        },
        [202] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 13,
                [1] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36444] = 3,
            },
        },
        [203] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 18,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36445] = 3,
            },
        },
        [204] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36446] = 1,
            },
        },
        [205] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36447] = 1,
            },
        },
        [206] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2600,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 30,
            LunarCredit = 28,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 30,
                [2] = 20,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36448] = 1,
                [36449] = 1,
                [36450] = 1,
            },
        },
        [207] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 19,
                [1] = 13,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36451] = 3,
            },
        },
        [208] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 840,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36452] = 1,
            },
        },
        [209] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 840,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36453] = 1,
            },
        },
        [210] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2800,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.UmbralWind,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 31,
            LunarCredit = 34,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 24,
                [3] = 16,
                [1] = 16,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36454] = 1,
                [36455] = 1,
                [36456] = 1,
            },
        },
        [211] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 560,
            GoldScore = 880,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 30,
            LunarCredit = 33,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 23,
                [3] = 15,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36458] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36457] = 1,
            },
        },
        [212] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 560,
            GoldScore = 880,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.UmbralWind,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 32,
            LunarCredit = 35,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 25,
                [2] = 16,
                [1] = 16,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36460] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36459] = 1,
            },
        },
        [213] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 16,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 16,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36461] = 3,
            },
        },
        [214] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 21,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 213 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 10,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36462] = 3,
            },
        },
        [215] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36463] = 2,
            },
        },
        [216] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 16,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 215 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 11,
                [3] = 8,
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36464] = 2,
            },
        },
        [217] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 13,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 216 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [3] = 7,
                [2] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36465] = 2,
            },
        },
        [218] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [2] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36466] = 1,
            },
        },
        [219] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 900,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 218 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 12,
                [3] = 8,
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36467] = 1,
            },
        },
        [220] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 16,
            EndTime = 18,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [3] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36468] = 3,
            },
        },
        [221] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 220 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 11,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36469] = 3,
            },
        },
        [222] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 221 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 14,
                [2] = 10,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36470] = 3,
            },
        },
        [223] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 4,
            EndTime = 6,
            ClassScore = 0,
            CosmoCredit = 35,
            LunarCredit = 42,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 27,
                [3] = 17,
                [1] = 17,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36472] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36471] = 1,
            },
        },
        [224] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 35,
            LunarCredit = 45,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 223 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 27,
                [3] = 19,
                [2] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36475] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36474] = 1,
            },
        },
        [225] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 84,
            LunarCredit = 117,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 224 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 84,
                [2] = 51,
                [1] = 51,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36479] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36477] = 1,
            },
        },
        [226] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36480] = 3,
            },
        },
        [227] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36481] = 3,
            },
        },
        [228] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36482] = 3,
            },
        },
        [229] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36483] = 3,
            },
        },
        [230] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36484] = 3,
            },
        },
        [231] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36485] = 3,
            },
        },
        [232] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 5,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36486] = 3,
            },
        },
        [233] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 6,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36487] = 3,
            },
        },
        [234] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36488] = 3,
            },
        },
        [235] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36490] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36489] = 2,
            },
        },
        [236] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 8,
            CosmoCredit = 4,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 9,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36492] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36491] = 2,
            },
        },
        [237] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2250,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 8,
            CosmoCredit = 4,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36494] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36493] = 3,
            },
        },
        [238] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36496] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36495] = 2,
            },
        },
        [239] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 9,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36497] = 3,
            },
        },
        [240] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36498] = 3,
            },
        },
        [241] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1700,
            GoldScore = 2000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 5,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36499] = 2,
            },
        },
        [242] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 9,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36500] = 3,
            },
        },
        [243] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36501] = 3,
            },
        },
        [244] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36502] = 3,
            },
        },
        [245] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 7,
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36504] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36503] = 2,
            },
        },
        [246] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [2] = 6,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36506] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36505] = 2,
            },
        },
        [247] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 13,
                [1] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36507] = 3,
            },
        },
        [248] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 18,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36508] = 3,
            },
        },
        [249] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36509] = 1,
            },
        },
        [250] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36510] = 1,
            },
        },
        [251] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2600,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 30,
            LunarCredit = 28,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 30,
                [2] = 20,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36511] = 1,
                [36512] = 1,
                [36513] = 1,
            },
        },
        [252] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 19,
                [1] = 13,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36514] = 3,
            },
        },
        [253] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 840,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36515] = 1,
            },
        },
        [254] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 840,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36516] = 1,
            },
        },
        [255] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2800,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 31,
            LunarCredit = 34,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 24,
                [3] = 16,
                [1] = 16,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36517] = 1,
                [36518] = 1,
                [36519] = 1,
            },
        },
        [256] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 560,
            GoldScore = 880,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 30,
            LunarCredit = 33,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 23,
                [3] = 15,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36521] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36520] = 1,
            },
        },
        [257] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 560,
            GoldScore = 880,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.UmbralWind,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 32,
            LunarCredit = 35,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 25,
                [2] = 16,
                [1] = 16,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36523] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36522] = 1,
            },
        },
        [258] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 16,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 16,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36524] = 3,
            },
        },
        [259] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 21,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 258 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 10,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36525] = 3,
            },
        },
        [260] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36526] = 2,
            },
        },
        [261] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 16,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 260 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 11,
                [3] = 8,
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36527] = 2,
            },
        },
        [262] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 13,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 261 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [3] = 7,
                [2] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36528] = 2,
            },
        },
        [263] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [2] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36529] = 1,
            },
        },
        [264] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 900,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 263 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 12,
                [3] = 8,
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36530] = 1,
            },
        },
        [265] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 20,
            EndTime = 22,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [3] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36531] = 3,
            },
        },
        [266] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 265 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 11,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36532] = 3,
            },
        },
        [267] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 266 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 14,
                [2] = 10,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36533] = 3,
            },
        },
        [268] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 8,
            EndTime = 10,
            ClassScore = 0,
            CosmoCredit = 35,
            LunarCredit = 42,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 27,
                [3] = 17,
                [1] = 17,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36535] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36534] = 1,
            },
        },
        [269] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 35,
            LunarCredit = 45,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 268 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 27,
                [3] = 19,
                [2] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36538] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36537] = 1,
            },
        },
        [270] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 84,
            LunarCredit = 117,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 269 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 84,
                [2] = 51,
                [1] = 51,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36542] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36540] = 1,
            },
        },
        [271] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36543] = 3,
            },
        },
        [272] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36544] = 3,
            },
        },
        [273] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36545] = 3,
            },
        },
        [274] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36546] = 3,
            },
        },
        [275] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36547] = 3,
            },
        },
        [276] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36548] = 3,
            },
        },
        [277] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 5,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36549] = 3,
            },
        },
        [278] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 6,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36550] = 3,
            },
        },
        [279] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36551] = 3,
            },
        },
        [280] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36553] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36552] = 2,
            },
        },
        [281] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 8,
            CosmoCredit = 4,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 9,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36555] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36554] = 2,
            },
        },
        [282] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2250,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 8,
            CosmoCredit = 4,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36557] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36556] = 3,
            },
        },
        [283] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36559] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36558] = 2,
            },
        },
        [284] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 9,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36560] = 3,
            },
        },
        [285] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36561] = 3,
            },
        },
        [286] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1700,
            GoldScore = 2000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 5,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36562] = 2,
            },
        },
        [287] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 9,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36563] = 3,
            },
        },
        [288] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36564] = 3,
            },
        },
        [289] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36565] = 3,
            },
        },
        [290] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 7,
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36567] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36566] = 2,
            },
        },
        [291] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [2] = 6,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36569] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36568] = 2,
            },
        },
        [292] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 13,
                [1] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36570] = 3,
            },
        },
        [293] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 18,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36571] = 3,
            },
        },
        [294] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36572] = 1,
            },
        },
        [295] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36573] = 1,
            },
        },
        [296] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2600,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 30,
            LunarCredit = 28,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 30,
                [2] = 20,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36574] = 1,
                [36575] = 1,
                [36576] = 1,
            },
        },
        [297] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 19,
                [1] = 13,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36577] = 3,
            },
        },
        [298] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 840,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36578] = 1,
            },
        },
        [299] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 840,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36579] = 1,
            },
        },
        [300] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2800,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.UmbralWind,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 31,
            LunarCredit = 34,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 24,
                [3] = 16,
                [1] = 16,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36580] = 1,
                [36581] = 1,
                [36582] = 1,
            },
        },
        [301] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 560,
            GoldScore = 880,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.UmbralWind,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 30,
            LunarCredit = 33,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 23,
                [3] = 15,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36584] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36583] = 1,
            },
        },
        [302] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 560,
            GoldScore = 880,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 32,
            LunarCredit = 35,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 25,
                [2] = 16,
                [1] = 16,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36586] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36585] = 1,
            },
        },
        [303] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 16,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 16,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36587] = 3,
            },
        },
        [304] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 21,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 303 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 10,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36588] = 3,
            },
        },
        [305] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36589] = 2,
            },
        },
        [306] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 16,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 305 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 11,
                [3] = 8,
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36590] = 2,
            },
        },
        [307] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 13,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 306 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [3] = 7,
                [2] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36591] = 2,
            },
        },
        [308] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [2] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36592] = 1,
            },
        },
        [309] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 900,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 308 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 12,
                [3] = 8,
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36593] = 1,
            },
        },
        [310] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 2,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [3] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36594] = 3,
            },
        },
        [311] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 310 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 11,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36595] = 3,
            },
        },
        [312] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 311 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 14,
                [2] = 10,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36596] = 3,
            },
        },
        [313] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 12,
            EndTime = 14,
            ClassScore = 0,
            CosmoCredit = 35,
            LunarCredit = 42,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 27,
                [3] = 17,
                [1] = 17,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36598] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36597] = 1,
            },
        },
        [314] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 35,
            LunarCredit = 45,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 313 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 27,
                [3] = 19,
                [2] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36601] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36600] = 1,
            },
        },
        [315] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 84,
            LunarCredit = 117,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 314 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 84,
                [2] = 51,
                [1] = 51,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36605] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36603] = 1,
            },
        },
        [316] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36606] = 3,
            },
        },
        [317] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36607] = 3,
            },
        },
        [318] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36608] = 3,
            },
        },
        [319] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36609] = 3,
            },
        },
        [320] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36610] = 3,
            },
        },
        [321] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36611] = 3,
            },
        },
        [322] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 5,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36612] = 3,
            },
        },
        [323] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 6,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36613] = 3,
            },
        },
        [324] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36614] = 3,
            },
        },
        [325] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36616] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36615] = 2,
            },
        },
        [326] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 8,
            CosmoCredit = 4,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 9,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36618] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36617] = 2,
            },
        },
        [327] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2250,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 8,
            CosmoCredit = 4,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36620] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36619] = 3,
            },
        },
        [328] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36622] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36621] = 2,
            },
        },
        [329] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 9,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36623] = 3,
            },
        },
        [330] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36624] = 3,
            },
        },
        [331] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1700,
            GoldScore = 2000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 5,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36625] = 2,
            },
        },
        [332] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 9,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36626] = 3,
            },
        },
        [333] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36627] = 3,
            },
        },
        [334] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36628] = 3,
            },
        },
        [335] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 7,
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36630] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36629] = 2,
            },
        },
        [336] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [2] = 6,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36632] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36631] = 2,
            },
        },
        [337] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 13,
                [1] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36633] = 3,
            },
        },
        [338] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 18,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36634] = 3,
            },
        },
        [339] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36635] = 1,
            },
        },
        [340] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36636] = 1,
            },
        },
        [341] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2600,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 30,
            LunarCredit = 28,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 30,
                [2] = 20,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36637] = 1,
                [36638] = 1,
                [36639] = 1,
            },
        },
        [342] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 19,
                [1] = 13,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36640] = 3,
            },
        },
        [343] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 840,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36641] = 1,
            },
        },
        [344] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 840,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36642] = 1,
            },
        },
        [345] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2800,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 31,
            LunarCredit = 34,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 24,
                [3] = 16,
                [1] = 16,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36643] = 1,
                [36644] = 1,
                [36645] = 1,
            },
        },
        [346] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 560,
            GoldScore = 880,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.UmbralWind,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 30,
            LunarCredit = 33,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 23,
                [3] = 15,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36647] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36646] = 1,
            },
        },
        [347] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 560,
            GoldScore = 880,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 32,
            LunarCredit = 35,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 25,
                [2] = 16,
                [1] = 16,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36649] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36648] = 1,
            },
        },
        [348] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 16,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 16,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36650] = 3,
            },
        },
        [349] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 21,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 348 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 10,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36651] = 3,
            },
        },
        [350] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36652] = 2,
            },
        },
        [351] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 16,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 350 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 11,
                [3] = 8,
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36653] = 2,
            },
        },
        [352] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 13,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 351 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [3] = 7,
                [2] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36654] = 2,
            },
        },
        [353] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [2] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36655] = 1,
            },
        },
        [354] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 900,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 353 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 12,
                [3] = 8,
                [1] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36656] = 1,
            },
        },
        [355] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 4,
            EndTime = 6,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [3] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36657] = 3,
            },
        },
        [356] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 355 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 11,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36658] = 3,
            },
        },
        [357] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 356 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 14,
                [2] = 10,
                [1] = 10,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36659] = 3,
            },
        },
        [358] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 16,
            EndTime = 18,
            ClassScore = 0,
            CosmoCredit = 35,
            LunarCredit = 42,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 27,
                [3] = 17,
                [1] = 17,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36661] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36660] = 1,
            },
        },
        [359] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 35,
            LunarCredit = 45,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 358 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 27,
                [3] = 19,
                [2] = 19,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36664] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36663] = 1,
            },
        },
        [360] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 84,
            LunarCredit = 117,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 359 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 84,
                [2] = 51,
                [1] = 51,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36668] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36666] = 1,
            },
        },
        [361] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-119f, -175f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 11,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48635] = 25,
            },
        },
        [362] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-119f, -175f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 11,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48635] = 25,
            },
        },
        [363] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4920,
            GoldScore = 5020,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-168f, -181f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48635] = 24,
            },
        },
        [364] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4920,
            GoldScore = 5020,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(96f, 259f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48641] = 24,
            },
        },
        [365] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-168f, -181f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 9,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48635] = 28,
            },
        },
        [366] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(96f, 259f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 9,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48641] = 28,
            },
        },
        [367] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1900,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 7,
            CosmoCredit = 3,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(65f, -431f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48649] = 10,
                [48650] = 15,
            },
        },
        [368] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4920,
            GoldScore = 5270,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 3,
            CosmoCredit = 2,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(-475f, 135f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 2,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48644] = 24,
            },
        },
        [369] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1900,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(73f, -482f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48649] = 12,
                [48650] = 16,
            },
        },
        [370] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 650,
            SilverScore = 850,
            GoldScore = 1050,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(-475f, 135f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 6,
            },
        },
        [371] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1800,
            SilverScore = 2000,
            GoldScore = 2600,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(-475f, 135f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 6,
                [1] = 4,
            },
        },
        [372] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2450,
            SilverScore = 2850,
            GoldScore = 3550,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 6,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(73f, -482f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 5,
            },
        },
        [373] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 100,
            SilverScore = 120,
            GoldScore = 140,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 7,
            CosmoCredit = 3,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(73f, -482f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 6,
                [1] = 6,
            },
        },
        [374] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1950,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-463f, -729f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48654] = 5,
                [48655] = 5,
                [48656] = 15,
            },
        },
        [375] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4920,
            GoldScore = 5320,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-690f, -752f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 2,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48654] = 24,
            },
        },
        [376] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1950,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-669f, -515f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [1] = 4,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48654] = 8,
                [48655] = 8,
                [48656] = 12,
            },
        },
        [377] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 650,
            SilverScore = 850,
            GoldScore = 1050,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-690f, -752f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [2] = 4,
            },
        },
        [378] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1800,
            SilverScore = 2000,
            GoldScore = 2600,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-669f, -515f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [1] = 4,
            },
        },
        [379] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2450,
            SilverScore = 2850,
            GoldScore = 3550,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-690f, -752f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [2] = 4,
            },
        },
        [380] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 100,
            SilverScore = 130,
            GoldScore = 150,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-690f, -752f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 5,
                [2] = 4,
                [1] = 4,
            },
        },
        [381] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4400,
            GoldScore = 4600,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 12,
            LunarCredit = 16,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-669f, -515f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 9,
                [2] = 9,
                [1] = 9,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48661] = 10,
            },
        },
        [382] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2800,
            GoldScore = 3200,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-463f, -729f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 10,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48654] = 15,
                [48655] = 15,
                [48656] = 20,
            },
        },
        [383] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4800,
            GoldScore = 5100,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(73f, -482f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 7,
                [1] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48649] = 48,
            },
        },
        [384] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 3000,
            GoldScore = 3400,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(-475f, 135f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 7,
                [2] = 10,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48644] = 15,
                [48645] = 15,
                [48646] = 20,
            },
        },
        [385] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1750,
            SilverScore = 2350,
            GoldScore = 2950,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(96f, 259f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [1] = 6,
            },
        },
        [386] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2800,
            SilverScore = 4000,
            GoldScore = 5200,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(73f, -482f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [2] = 6,
            },
        },
        [387] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2850,
            GoldScore = 3250,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(65f, -431f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [1] = 7,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48649] = 15,
                [48650] = 15,
                [48651] = 21,
            },
        },
        [388] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1750,
            GoldScore = 1950,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-168f, -181f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 9,
                [2] = 14,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48639] = 25,
            },
        },
        [389] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 990,
            SilverScore = 1160,
            GoldScore = 1680,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 16,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-524f, 379f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [1] = 10,
            },
        },
        [390] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 900,
            GoldScore = 1100,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(404f, -802f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [3] = 6,
                [1] = 6,
            },
        },
        [391] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 900,
            GoldScore = 1300,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.UmbralWind,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-690f, -752f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48661] = 35,
            },
        },
        [392] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 990,
            SilverScore = 1310,
            GoldScore = 1680,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 16,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(-475f, 135f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 12,
                [2] = 8,
                [1] = 8,
            },
        },
        [393] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2800,
            GoldScore = 3200,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-119f, -175f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [3] = 7,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48635] = 15,
                [48636] = 15,
                [48637] = 20,
            },
        },
        [394] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1750,
            SilverScore = 2350,
            GoldScore = 2950,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 21,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(73f, -482f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 393 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 5,
                [2] = 5,
                [1] = 5,
            },
        },
        [395] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4800,
            GoldScore = 5100,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(-475f, 135f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 7,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48644] = 48,
            },
        },
        [396] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2800,
            SilverScore = 4200,
            GoldScore = 5400,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 20,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-690f, -752f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 395 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [1] = 5,
            },
        },
        [397] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 900,
            GoldScore = 1200,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 22,
            LunarCredit = 25,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-669f, -515f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 396 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 6,
                [2] = 6,
            },
        },
        [398] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 3000,
            GoldScore = 3400,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(73f, -482f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [2] = 7,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48649] = 15,
                [48650] = 15,
                [48651] = 20,
            },
        },
        [399] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1750,
            GoldScore = 2100,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 27,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(404f, -802f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 398 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 11,
                [3] = 8,
                [1] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48673] = 25,
            },
        },
        [400] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 3000,
            GoldScore = 3400,
            Attributes = MissionAttributes.Gather | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 2,
            EndTime = 4,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(96f, 259f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [3] = 7,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48641] = 15,
                [48642] = 15,
                [48643] = 20,
            },
        },
        [401] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 5800,
            SilverScore = 7000,
            GoldScore = 7950,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 21,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(404f, -802f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 402 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [2] = 6,
                [1] = 6,
            },
        },
        [402] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 900,
            GoldScore = 1100,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 22,
            LunarCredit = 24,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-524f, 379f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 400 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [2] = 6,
                [1] = 6,
            },
        },
        [403] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 900,
            GoldScore = 1300,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 14,
            EndTime = 16,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-524f, 379f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [1] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48669] = 35,
            },
        },
        [404] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1010,
            SilverScore = 1330,
            GoldScore = 1730,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 27,
            LunarCredit = 32,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-524f, 379f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 403 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 13,
                [3] = 9,
                [2] = 9,
            },
        },
        [405] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1800,
            GoldScore = 2250,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 29,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-524f, 379f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 404 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 13,
                [2] = 8,
                [1] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48669] = 25,
            },
        },
        [406] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(232f, -50f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 11,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48674] = 25,
            },
        },
        [407] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(232f, -50f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 11,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48674] = 25,
            },
        },
        [408] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4920,
            GoldScore = 5020,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(225f, 83f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48675] = 24,
            },
        },
        [409] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4920,
            GoldScore = 5020,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-278f, -13f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48680] = 24,
            },
        },
        [410] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(225f, 83f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 9,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48675] = 28,
            },
        },
        [411] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-278f, -13f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 9,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48680] = 28,
            },
        },
        [412] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1900,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(456f, 221f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48686] = 10,
                [48687] = 15,
            },
        },
        [413] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4920,
            GoldScore = 5270,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-121f, 368f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 2,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48681] = 24,
            },
        },
        [414] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1900,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(455f, 243f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48689] = 12,
                [48690] = 16,
            },
        },
        [415] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 650,
            SilverScore = 850,
            GoldScore = 1050,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-121f, 368f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 6,
            },
        },
        [416] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1800,
            SilverScore = 2000,
            GoldScore = 2600,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-121f, 368f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 6,
                [1] = 4,
            },
        },
        [417] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2450,
            SilverScore = 2850,
            GoldScore = 3550,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(455f, 243f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 5,
            },
        },
        [418] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 100,
            SilverScore = 120,
            GoldScore = 140,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(455f, 243f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 6,
                [1] = 6,
            },
        },
        [419] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1950,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(506f, 682f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48702] = 5,
                [48703] = 5,
                [48704] = 15,
            },
        },
        [420] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4920,
            GoldScore = 5320,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(609f, 478f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 2,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48693] = 24,
            },
        },
        [421] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1950,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(527f, 630f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [1] = 4,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48705] = 8,
                [48706] = 8,
                [48707] = 12,
            },
        },
        [422] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 650,
            SilverScore = 850,
            GoldScore = 1050,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(609f, 478f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [2] = 4,
            },
        },
        [423] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1800,
            SilverScore = 2000,
            GoldScore = 2600,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(527f, 630f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [1] = 4,
            },
        },
        [424] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2450,
            SilverScore = 2850,
            GoldScore = 3550,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(609f, 478f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [2] = 4,
            },
        },
        [425] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 100,
            SilverScore = 130,
            GoldScore = 150,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(609f, 478f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 5,
                [2] = 4,
                [1] = 4,
            },
        },
        [426] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4400,
            GoldScore = 4600,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 12,
            LunarCredit = 16,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(527f, 630f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 9,
                [2] = 9,
                [1] = 9,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48709] = 10,
            },
        },
        [427] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2800,
            GoldScore = 3200,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(506f, 682f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 10,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48702] = 15,
                [48703] = 15,
                [48704] = 20,
            },
        },
        [428] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4800,
            GoldScore = 5100,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(455f, 243f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 7,
                [1] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48689] = 48,
            },
        },
        [429] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 3000,
            GoldScore = 3400,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-121f, 368f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 7,
                [2] = 10,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48681] = 15,
                [48682] = 15,
                [48683] = 20,
            },
        },
        [430] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1750,
            SilverScore = 2350,
            GoldScore = 2950,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-278f, -13f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [1] = 6,
            },
        },
        [431] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2800,
            SilverScore = 4000,
            GoldScore = 5200,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(455f, 243f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [2] = 6,
            },
        },
        [432] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2850,
            GoldScore = 3250,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(456f, 221f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [1] = 7,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48686] = 15,
                [48687] = 15,
                [48688] = 21,
            },
        },
        [433] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1750,
            GoldScore = 1950,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(225f, 83f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 9,
                [2] = 14,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48677] = 25,
            },
        },
        [434] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 990,
            SilverScore = 1160,
            GoldScore = 1680,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 16,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-706f, 564f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 15,
                [1] = 10,
            },
        },
        [435] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 900,
            GoldScore = 1100,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.UmbralWind,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(874f, -771f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [3] = 6,
                [1] = 6,
            },
        },
        [436] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 900,
            GoldScore = 1300,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(609f, 478f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48700] = 35,
            },
        },
        [437] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 990,
            SilverScore = 1310,
            GoldScore = 1680,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.UmbralWind,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 16,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-121f, 368f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 12,
                [2] = 8,
                [1] = 8,
            },
        },
        [438] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2800,
            GoldScore = 3200,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(456f, 221f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [3] = 7,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48686] = 15,
                [48687] = 15,
                [48688] = 20,
            },
        },
        [439] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1750,
            SilverScore = 2350,
            GoldScore = 2950,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 21,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-121f, 368f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 438 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 5,
                [2] = 5,
                [1] = 5,
            },
        },
        [440] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4800,
            GoldScore = 5100,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(225f, 83f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 7,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48679] = 48,
            },
        },
        [441] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2800,
            SilverScore = 4200,
            GoldScore = 5400,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 20,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(527f, 630f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 440 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [1] = 5,
            },
        },
        [442] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 900,
            GoldScore = 1200,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 22,
            LunarCredit = 25,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(609f, 478f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 441 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 6,
                [2] = 6,
            },
        },
        [443] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 3000,
            GoldScore = 3400,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-121f, 368f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [2] = 7,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48681] = 15,
                [48682] = 15,
                [48683] = 20,
            },
        },
        [444] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1750,
            GoldScore = 2100,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 27,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(874f, -771f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 443 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 11,
                [3] = 8,
                [1] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48722] = 25,
            },
        },
        [445] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 3000,
            GoldScore = 3400,
            Attributes = MissionAttributes.Gather | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 10,
            EndTime = 12,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(455f, 243f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [3] = 7,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48689] = 15,
                [48690] = 15,
                [48691] = 20,
            },
        },
        [446] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 5800,
            SilverScore = 7000,
            GoldScore = 7950,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 21,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(874f, -771f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 447 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [2] = 6,
                [1] = 6,
            },
        },
        [447] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 900,
            GoldScore = 1100,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 22,
            LunarCredit = 24,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-706f, 564f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 445 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [2] = 6,
                [1] = 6,
            },
        },
        [448] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 900,
            GoldScore = 1300,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 22,
            EndTime = 24,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-706f, 564f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [1] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48716] = 35,
            },
        },
        [449] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1010,
            SilverScore = 1330,
            GoldScore = 1730,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 27,
            LunarCredit = 32,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-706f, 564f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 448 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 13,
                [3] = 9,
                [2] = 9,
            },
        },
        [450] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1800,
            GoldScore = 2250,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 29,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-706f, 564f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 449 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 13,
                [2] = 8,
                [1] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48719] = 25,
            },
        },
        [451] = new CosmicInfo
        {
            FishCountRequired = 5,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(104f, -269f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 7,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Astacus Lamentorum"] = new HashSet<uint> { 45693, 45705, 45715, 45727, 45744, 45826, 45847 },
                ["Lunar Tilapia"] = new HashSet<uint> { 45694 },
                ["Lunar Blue Guppy"] = new HashSet<uint> { 45695 },
            },
        },
        [452] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 540,
            SilverScore = 630,
            GoldScore = 1280,
            Attributes = MissionAttributes.Fish | MissionAttributes.Limited | MissionAttributes.ScoreLargestSize | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-139f, -283f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 8,
            },
        },
        [453] = new CosmicInfo
        {
            FishCountRequired = 1,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-281f, -104f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 19,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Cobalt Eel"] = new HashSet<uint> { 45701 },
            },
        },
        [454] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1000,
            SilverScore = 1300,
            GoldScore = 1500,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 70,
            MapPosition = new Vector2(193f, 196f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 14,
            },
        },
        [455] = new CosmicInfo
        {
            FishCountRequired = 2,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ScoreVariety,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(104f, -269f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 12,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Astacus Lamentorum"] = new HashSet<uint> { 45693, 45705, 45715, 45727, 45744, 45826, 45847 },
                ["Lunar Peacock Bass"] = new HashSet<uint> { 45706 },
                ["Lunar Hemiodus"] = new HashSet<uint> { 45707 },
            },
        },
        [456] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2000,
            SilverScore = 5200,
            GoldScore = 5500,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-139f, -283f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [1] = 12,
            },
        },
        [457] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1570,
            SilverScore = 2040,
            GoldScore = 3690,
            Attributes = MissionAttributes.Fish | MissionAttributes.Limited | MissionAttributes.ScoreLargestSize | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 70,
            MapPosition = new Vector2(193f, 196f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 6,
            },
        },
        [458] = new CosmicInfo
        {
            FishCountRequired = 2,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 13,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(104f, -269f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 13,
                [1] = 7,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Weepingeye"] = new HashSet<uint> { 45717 },
                ["Lunar Eel"] = new HashSet<uint> { 45718 },
            },
        },
        [459] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1000,
            SilverScore = 1500,
            GoldScore = 2000,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-281f, -104f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 11,
                [1] = 11,
            },
        },
        [460] = new CosmicInfo
        {
            FishCountRequired = 2,
            BronzeScore = 0,
            SilverScore = 3000,
            GoldScore = 4000,
            Attributes = MissionAttributes.Fish | MissionAttributes.Limited | MissionAttributes.ScoreVariety,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 70,
            MapPosition = new Vector2(193f, 196f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 8,
                [1] = 14,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Lunar Sole"] = new HashSet<uint> { 45725 },
                ["Silver Sturgeon"] = new HashSet<uint> { 45726 },
                ["Pinkmoon Cichlid"] = new HashSet<uint> { 45724 },
                ["Star Pleco"] = new HashSet<uint> { 45702, 45711, 45723, 45769, 45820, 45841, 45913 },
            },
        },
        [461] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 20000,
            SilverScore = 31000,
            GoldScore = 37000,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(104f, -269f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 14,
                [1] = 8,
            },
        },
        [462] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 200,
            SilverScore = 300,
            GoldScore = 1000,
            Attributes = MissionAttributes.Fish | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 13,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-281f, -104f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 10,
                [1] = 10,
            },
        },
        [463] = new CosmicInfo
        {
            FishCountRequired = 4,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ScoreVariety,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 24,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-139f, -283f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 19,
                [1] = 19,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Melancholia"] = new HashSet<uint> { 45696, 45708, 45735, 45754, 45778, 45853, 45938 },
                ["Hopped-on Leaffish"] = new HashSet<uint> { 45736 },
                ["Lunar Discus"] = new HashSet<uint> { 45737 },
                ["Solar Flarefish"] = new HashSet<uint> { 45738 },
            },
        },
        [464] = new CosmicInfo
        {
            FishCountRequired = 3,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 300,
            MapPosition = new Vector2(-642f, -631f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 6,
                [1] = 10,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Prismatic Fish"] = new HashSet<uint> { 45743 },
            },
        },
        [465] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 3000,
            SilverScore = 3500,
            GoldScore = 3700,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(104f, -269f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 14,
                [1] = 8,
            },
        },
        [466] = new CosmicInfo
        {
            FishCountRequired = 3,
            BronzeScore = 0,
            SilverScore = 4000,
            GoldScore = 5000,
            Attributes = MissionAttributes.Fish | MissionAttributes.Limited | MissionAttributes.ScoreVariety,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 13,
            TerritoryId = 1237,
            Radius = 300,
            MapPosition = new Vector2(-642f, -631f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [1] = 7,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Moonrock Candy"] = new HashSet<uint> { 45739, 45749, 45789, 45799, 45871, 45877, 45929 },
                ["Moongill"] = new HashSet<uint> { 45740, 45750, 45790, 45800, 45878, 45930 },
                ["Lunar Cabomba"] = new HashSet<uint> { 45751 },
                ["Opal Eel"] = new HashSet<uint> { 45752 },
                ["Lunar Pirarucu"] = new HashSet<uint> { 45753 },
            },
        },
        [467] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 3690,
            SilverScore = 5540,
            GoldScore = 6280,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreLargestSize | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 12,
            LunarCredit = 16,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-139f, -283f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 15,
                [2] = 9,
            },
        },
        [468] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2400,
            SilverScore = 3800,
            GoldScore = 4500,
            Attributes = MissionAttributes.Fish | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 13,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 250,
            MapPosition = new Vector2(573f, 573f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 16,
                [1] = 9,
            },
        },
        [469] = new CosmicInfo
        {
            FishCountRequired = 4,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ScoreVariety,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 9,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-281f, -104f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 11,
                [2] = 7,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Bluemoon Loach"] = new HashSet<uint> { 45699, 45719, 45731, 45764, 45784, 45810, 45918, 45935 },
                ["Leaping Loach"] = new HashSet<uint> { 45765 },
                ["Lunar Bronze Pleco"] = new HashSet<uint> { 45766 },
                ["Starry Stingray"] = new HashSet<uint> { 45767 },
                ["Lunar Lungfish"] = new HashSet<uint> { 45768 },
            },
        },
        [470] = new CosmicInfo
        {
            FishCountRequired = 13,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 70,
            MapPosition = new Vector2(193f, 196f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 7,
                [1] = 7,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Star Pleco"] = new HashSet<uint> { 45702, 45711, 45723, 45769, 45820, 45841, 45913 },
                ["Lunar Grass Carp"] = new HashSet<uint> { 45712, 45770, 45821, 45842, 45914 },
                ["Macrobrachium Lunaris"] = new HashSet<uint> { 45771 },
                ["Ataxite"] = new HashSet<uint> { 45772 },
            },
        },
        [471] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 35000,
            SilverScore = 44000,
            GoldScore = 50000,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 12,
            LunarCredit = 16,
            TerritoryId = 1237,
            Radius = 250,
            MapPosition = new Vector2(573f, 573f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 8,
                [2] = 8,
                [1] = 8,
            },
        },
        [472] = new CosmicInfo
        {
            FishCountRequired = 1,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 16,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-139f, -283f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 11,
                [1] = 16,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Platinum Bichir"] = new HashSet<uint> { 45783 },
            },
        },
        [473] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1500,
            SilverScore = 3000,
            GoldScore = 3500,
            Attributes = MissionAttributes.Fish | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 20,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-281f, -104f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 20,
                [1] = 13,
            },
        },
        [474] = new CosmicInfo
        {
            FishCountRequired = 18,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 300,
            MapPosition = new Vector2(-642f, -631f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 13,
                [2] = 19,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Moonrock Candy"] = new HashSet<uint> { 45739, 45749, 45789, 45799, 45871, 45877, 45929 },
                ["Moongill"] = new HashSet<uint> { 45740, 45750, 45790, 45800, 45878, 45930 },
                ["Darkside Bass"] = new HashSet<uint> { 45791, 45801, 45879 },
                ["Lunar Sisterscale"] = new HashSet<uint> { 45792 },
                ["Grand Crowntail Betta"] = new HashSet<uint> { 45793 },
            },
        },
        [475] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 5500,
            SilverScore = 7200,
            GoldScore = 7700,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 21,
            LunarCredit = 19,
            TerritoryId = 1237,
            Radius = 250,
            MapPosition = new Vector2(573f, 573f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 21,
                [1] = 14,
            },
        },
        [476] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 5000,
            SilverScore = 10000,
            GoldScore = 15000,
            Attributes = MissionAttributes.Fish | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 20,
            LunarCredit = 19,
            TerritoryId = 1237,
            Radius = 300,
            MapPosition = new Vector2(-642f, -631f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 20,
                [2] = 14,
            },
        },
        [477] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 7730,
            SilverScore = 10240,
            GoldScore = 10260,
            Attributes = MissionAttributes.Fish | MissionAttributes.Limited | MissionAttributes.ScoreLargestSize | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 21,
            LunarCredit = 21,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(104f, -269f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 21,
                [1] = 14,
            },
        },
        [478] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 30000,
            SilverScore = 34000,
            GoldScore = 36000,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 70,
            MapPosition = new Vector2(193f, 196f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 9,
                [2] = 14,
            },
        },
        [479] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1500,
            SilverScore = 2250,
            GoldScore = 2650,
            Attributes = MissionAttributes.Fish | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 19,
            TerritoryId = 1237,
            Radius = 150,
            MapPosition = new Vector2(909f, -336f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [1] = 12,
            },
        },
        [480] = new CosmicInfo
        {
            FishCountRequired = 18,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 21,
            LunarCredit = 24,
            TerritoryId = 1237,
            Radius = 300,
            MapPosition = new Vector2(-673f, 497f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 16,
                [3] = 11,
                [1] = 11,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Arsenic Axolotl"] = new HashSet<uint> { 45836, 45883 },
                ["Sunny Jellyfish"] = new HashSet<uint> { 45837, 45884 },
                ["Universal Darkfin"] = new HashSet<uint> { 45838, 45885 },
                ["Etheirys Croppie"] = new HashSet<uint> { 45839 },
                ["Moon Mora"] = new HashSet<uint> { 45840 },
            },
        },
        [481] = new CosmicInfo
        {
            FishCountRequired = 1,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.UmbralWind,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 21,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 250,
            MapPosition = new Vector2(573f, 573f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 16,
                [3] = 10,
                [2] = 10,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Eyeballingway"] = new HashSet<uint> { 45870 },
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [45870] = 1,
            },
        },
        [482] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 4660,
            SilverScore = 8500,
            GoldScore = 8550,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreLargestSize | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 28,
            LunarCredit = 31,
            TerritoryId = 1237,
            Radius = 300,
            MapPosition = new Vector2(-642f, -631f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 21,
                [2] = 14,
                [1] = 14,
            },
        },
        [483] = new CosmicInfo
        {
            FishCountRequired = 4,
            BronzeScore = 0,
            SilverScore = 5000,
            GoldScore = 6000,
            Attributes = MissionAttributes.Fish | MissionAttributes.Limited | MissionAttributes.ScoreVariety,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 31,
            LunarCredit = 29,
            TerritoryId = 1237,
            Radius = 250,
            MapPosition = new Vector2(573f, 573f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 31,
                [3] = 21,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Lunar Scorpion"] = new HashSet<uint> { 45759, 45773, 45794, 45804, 45815, 45865, 45895, 45923 },
                ["Moonwhip"] = new HashSet<uint> { 45760, 45774, 45795, 45805, 45816, 45866, 45924 },
                ["Lunar Anemone"] = new HashSet<uint> { 45806 },
                ["Culter Arsenici"] = new HashSet<uint> { 45807 },
                ["Lamentorum Regotoise"] = new HashSet<uint> { 45808 },
                ["Polypus Sulfuris"] = new HashSet<uint> { 45809 },
            },
        },
        [484] = new CosmicInfo
        {
            FishCountRequired = 1,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 38,
            LunarCredit = 41,
            TerritoryId = 1237,
            Radius = 70,
            MapPosition = new Vector2(193f, 196f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 483 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 15,
                [2] = 15,
                [1] = 15,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Lunar Sturgeon"] = new HashSet<uint> { 45846 },
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [45846] = 1,
            },
        },
        [485] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 4510,
            SilverScore = 6500,
            GoldScore = 6540,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreLargestSize | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 13,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-281f, -104f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 9,
                [1] = 13,
            },
        },
        [486] = new CosmicInfo
        {
            FishCountRequired = 5,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ScoreVariety | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 28,
            LunarCredit = 30,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(104f, -269f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 485 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 13,
                [3] = 9,
                [1] = 9,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Astacus Lamentorum"] = new HashSet<uint> { 45693, 45705, 45715, 45727, 45744, 45826, 45847 },
                ["Teardrop Knifefish"] = new HashSet<uint> { 45848 },
                ["Weeping Crab"] = new HashSet<uint> { 45849 },
                ["Silvermoon Tilapia"] = new HashSet<uint> { 45850 },
                ["Weeping Minnow"] = new HashSet<uint> { 45851 },
            },
        },
        [487] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 7900,
            SilverScore = 9600,
            GoldScore = 12300,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 40,
            LunarCredit = 48,
            TerritoryId = 1237,
            Radius = 300,
            MapPosition = new Vector2(-642f, -631f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 486 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 23,
                [3] = 16,
                [2] = 16,
            },
        },
        [488] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 11700,
            SilverScore = 12200,
            GoldScore = 12900,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 25,
            LunarCredit = 25,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-139f, -283f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 25,
                [2] = 17,
            },
        },
        [489] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2250,
            SilverScore = 3050,
            GoldScore = 3450,
            Attributes = MissionAttributes.Fish | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 38,
            LunarCredit = 43,
            TerritoryId = 1237,
            Radius = 300,
            MapPosition = new Vector2(-673f, 497f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 488 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 21,
                [3] = 15,
                [1] = 15,
            },
        },
        [490] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 4800,
            SilverScore = 4830,
            GoldScore = 9240,
            Attributes = MissionAttributes.Fish | MissionAttributes.Limited | MissionAttributes.ScoreLargestSize | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 6,
            EndTime = 8,
            ClassScore = 0,
            CosmoCredit = 20,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 250,
            MapPosition = new Vector2(573f, 573f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 20,
                [3] = 13,
            },
        },
        [491] = new CosmicInfo
        {
            FishCountRequired = 4,
            BronzeScore = 0,
            SilverScore = 5000,
            GoldScore = 6000,
            Attributes = MissionAttributes.Fish | MissionAttributes.Limited | MissionAttributes.ScoreVariety | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 47,
            LunarCredit = 54,
            TerritoryId = 1237,
            Radius = 150,
            MapPosition = new Vector2(-348f, 604f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 490 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 21,
                [2] = 21,
                [1] = 21,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Lunar Goldfish"] = new HashSet<uint> { 45859, 45901 },
                ["Lunar Minnow"] = new HashSet<uint> { 45860, 45902 },
                ["Gleamingray"] = new HashSet<uint> { 45861 },
                ["Lunar Butterfly"] = new HashSet<uint> { 45862 },
                ["Lunar Seagrapes"] = new HashSet<uint> { 45863 },
                ["Fishingway"] = new HashSet<uint> { 45864 },
            },
        },
        [492] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 11570,
            SilverScore = 11640,
            GoldScore = 23140,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreLargestSize | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 44,
            LunarCredit = 54,
            TerritoryId = 1237,
            Radius = 150,
            MapPosition = new Vector2(909f, -336f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 491 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 26,
                [2] = 19,
                [1] = 19,
            },
        },
        [493] = new CosmicInfo
        {
            FishCountRequired = 18,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 18,
            EndTime = 20,
            ClassScore = 0,
            CosmoCredit = 21,
            LunarCredit = 26,
            TerritoryId = 1237,
            Radius = 150,
            MapPosition = new Vector2(909f, -336f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 17,
                [3] = 11,
                [1] = 11,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Ctenophora Lunaris"] = new HashSet<uint> { 45831, 45889, 45907 },
                ["Protomyke #721"] = new HashSet<uint> { 45832, 45890, 45908 },
                ["Argonauta Lunaris"] = new HashSet<uint> { 45833, 45891, 45909 },
                ["Aetherial Sword"] = new HashSet<uint> { 45892, 45910 },
                ["Macropinna"] = new HashSet<uint> { 45911 },
                ["Deepmoon Seadragon"] = new HashSet<uint> { 45912 },
            },
        },
        [494] = new CosmicInfo
        {
            FishCountRequired = 6,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ScoreVariety | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 56,
            LunarCredit = 69,
            TerritoryId = 1237,
            Radius = 250,
            MapPosition = new Vector2(573f, 573f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 493 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 35,
                [3] = 25,
                [2] = 25,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Lunar Scorpion"] = new HashSet<uint> { 45759, 45773, 45794, 45804, 45815, 45865, 45895, 45923 },
                ["Arsenical Proto-hropken"] = new HashSet<uint> { 45896 },
                ["Lunar Oil Eel"] = new HashSet<uint> { 45897 },
                ["Galactic Noise"] = new HashSet<uint> { 45898 },
                ["Onychodictyon"] = new HashSet<uint> { 45899 },
                ["Eolactoria Arsenici"] = new HashSet<uint> { 45900 },
            },
        },
        [495] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 6000,
            SilverScore = 8000,
            GoldScore = 20000,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 60,
            LunarCredit = 79,
            TerritoryId = 1237,
            Radius = 150,
            MapPosition = new Vector2(-348f, 604f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 494 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 50,
                [2] = 30,
                [1] = 30,
            },
        },
        [496] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2700,
            Attributes = MissionAttributes.Craft | MissionAttributes.Gather | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 134,
            CosmoCredit = 24,
            LunarCredit = 25,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(-475f, 135f),
            Jobs = new HashSet<uint> { 16, 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 21,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36669] = 1,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48648] = 9,
            },
        },
        [497] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2700,
            Attributes = MissionAttributes.Craft | MissionAttributes.Gather | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 25,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(96f, 259f),
            Jobs = new HashSet<uint> { 16, 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 21,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36673] = 1,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48641] = 9,
            },
        },
        [498] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2700,
            Attributes = MissionAttributes.Craft | MissionAttributes.Gather | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 25,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-690f, -752f),
            Jobs = new HashSet<uint> { 16, 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 21,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36675] = 1,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48655] = 9,
            },
        },
        [499] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2700,
            Attributes = MissionAttributes.Craft | MissionAttributes.Gather | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.UmbralWind,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 27,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-168f, -181f),
            Jobs = new HashSet<uint> { 16, 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 21,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36671] = 1,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48635] = 9,
            },
        },
        [500] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2700,
            Attributes = MissionAttributes.Craft | MissionAttributes.Gather | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 27,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(73f, -482f),
            Jobs = new HashSet<uint> { 16, 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 21,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36677] = 1,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48653] = 9,
            },
        },
        [501] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2700,
            Attributes = MissionAttributes.Craft | MissionAttributes.Gather | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.UmbralWind,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 27,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-669f, -515f),
            Jobs = new HashSet<uint> { 16, 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 21,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36679] = 1,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48725] = 9,
            },
        },
        [502] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2700,
            Attributes = MissionAttributes.Craft | MissionAttributes.Gather | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 25,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(225f, 83f),
            Jobs = new HashSet<uint> { 17, 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 21,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36678] = 1,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48675] = 9,
            },
        },
        [503] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2700,
            Attributes = MissionAttributes.Craft | MissionAttributes.Gather | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 25,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(527f, 630f),
            Jobs = new HashSet<uint> { 17, 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 21,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36680] = 1,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48706] = 9,
            },
        },
        [504] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2700,
            Attributes = MissionAttributes.Craft | MissionAttributes.Gather | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 25,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-121f, 368f),
            Jobs = new HashSet<uint> { 17, 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 21,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36683] = 1,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48684] = 9,
            },
        },
        [505] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2700,
            Attributes = MissionAttributes.Craft | MissionAttributes.Gather | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 246,
            CosmoCredit = 24,
            LunarCredit = 27,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(455f, 243f),
            Jobs = new HashSet<uint> { 17, 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 21,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36670] = 1,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48691] = 9,
            },
        },
        [506] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2700,
            Attributes = MissionAttributes.Craft | MissionAttributes.Gather | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.UmbralWind,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 27,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(609f, 478f),
            Jobs = new HashSet<uint> { 17, 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 21,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36674] = 1,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48697] = 9,
            },
        },
        [507] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2700,
            Attributes = MissionAttributes.Craft | MissionAttributes.Gather | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 27,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-121f, 368f),
            Jobs = new HashSet<uint> { 17, 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 21,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36681] = 1,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [48683] = 9,
            },
        },
        [508] = new CosmicInfo
        {
            FishCountRequired = 7,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2700,
            Attributes = MissionAttributes.Craft | MissionAttributes.Fish | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 41,
            LunarCredit = 42,
            TerritoryId = 1237,
            Radius = 70,
            MapPosition = new Vector2(193f, 196f),
            Jobs = new HashSet<uint> { 18, 9 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 36,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Heavy Ataxite"] = new HashSet<uint> { 45917 },
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36672] = 1,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [45917] = 7,
            },
        },
        [509] = new CosmicInfo
        {
            FishCountRequired = 1,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2700,
            Attributes = MissionAttributes.Craft | MissionAttributes.Fish | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 42,
            LunarCredit = 42,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-281f, -104f),
            Jobs = new HashSet<uint> { 18, 14 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 37,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Hopping Lungfish"] = new HashSet<uint> { 45922 },
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36682] = 1,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [45922] = 1,
            },
        },
        [510] = new CosmicInfo
        {
            FishCountRequired = 7,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2700,
            Attributes = MissionAttributes.Craft | MissionAttributes.Fish | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.UmbralWind,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 46,
            LunarCredit = 52,
            TerritoryId = 1237,
            Radius = 250,
            MapPosition = new Vector2(573f, 573f),
            Jobs = new HashSet<uint> { 18, 11 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 41,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Raw Moonbright Tourmaline"] = new HashSet<uint> { 45928 },
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36676] = 1,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [45928] = 7,
            },
        },
        [511] = new CosmicInfo
        {
            FishCountRequired = 1,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2700,
            Attributes = MissionAttributes.Craft | MissionAttributes.Fish | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.MoonDust,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 51,
            LunarCredit = 56,
            TerritoryId = 1237,
            Radius = 300,
            MapPosition = new Vector2(-642f, -631f),
            Jobs = new HashSet<uint> { 18, 15 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 45,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Hollow Eel"] = new HashSet<uint> { 45934 },
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36684] = 1,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [45934] = 1,
            },
        },
        [512] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 383,
            CosmoCredit = 33,
            LunarCredit = 138,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(-74f, 776f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36685] = 1,
            },
        },
        [513] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 400,
            CosmoCredit = 35,
            LunarCredit = 144,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(386f, 713f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 19,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36686] = 1,
            },
        },
        [514] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 417,
            CosmoCredit = 36,
            LunarCredit = 150,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(661f, 108f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 20,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36687] = 1,
            },
        },
        [515] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 382,
            CosmoCredit = 33,
            LunarCredit = 138,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(-214f, 216f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36690] = 1,
            },
        },
        [516] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 34,
            LunarCredit = 143,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(836f, -374f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 19,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36694] = 1,
            },
        },
        [517] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 417,
            CosmoCredit = 36,
            LunarCredit = 150,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(546f, 54f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 20,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36691] = 1,
            },
        },
        [518] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 391,
            CosmoCredit = 34,
            LunarCredit = 141,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(166f, 556f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 19,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36695] = 1,
            },
        },
        [519] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 386,
            CosmoCredit = 33,
            LunarCredit = 139,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(41f, -344f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 19,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36697] = 1,
            },
        },
        [520] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 36,
            LunarCredit = 151,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(836f, -374f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 20,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36699] = 1,
            },
        },
        [521] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 33,
            LunarCredit = 138,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(-74f, 776f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36700] = 1,
            },
        },
        [522] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 35,
            LunarCredit = 145,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(166f, 556f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 19,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36703] = 1,
            },
        },
        [523] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 35,
            LunarCredit = 147,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(41f, -344f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 20,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36701] = 1,
            },
        },
        [524] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 33,
            LunarCredit = 138,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(-214f, 216f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36705] = 1,
            },
        },
        [525] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 34,
            LunarCredit = 143,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(836f, -374f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 19,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36707] = 1,
            },
        },
        [526] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 36,
            LunarCredit = 152,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(386f, 713f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 20,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36708] = 1,
            },
        },
        [527] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 33,
            LunarCredit = 138,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(-74f, 776f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36711] = 1,
            },
        },
        [528] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 35,
            LunarCredit = 144,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(386f, 713f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 19,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36712] = 1,
            },
        },
        [529] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 36,
            LunarCredit = 150,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(661f, 108f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 20,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36710] = 1,
            },
        },
        [530] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 34,
            LunarCredit = 141,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(166f, 556f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 19,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36715] = 1,
            },
        },
        [531] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 34,
            LunarCredit = 143,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(496f, -849f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 19,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36719] = 1,
            },
        },
        [532] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 36,
            LunarCredit = 150,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(546f, 54f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 20,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36718] = 1,
            },
        },
        [533] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 33,
            LunarCredit = 136,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(-459f, -64f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36720] = 1,
            },
        },
        [534] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 34,
            LunarCredit = 143,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(496f, -849f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 19,
                [2] = 11,
                [1] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36721] = 1,
            },
        },
        [535] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 35,
            LunarCredit = 148,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(108f, -205f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 20,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36722] = 1,
            },
        },
        [536] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Gather | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 34,
            LunarCredit = 142,
            TerritoryId = 1237,
            Radius = 80,
            MapPosition = new Vector2(-503f, -324f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 19,
                [2] = 11,
                [1] = 11,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [46181] = 16,
            },
        },
        [537] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Gather | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 23,
            LunarCredit = 97,
            TerritoryId = 1237,
            Radius = 80,
            MapPosition = new Vector2(-131f, -365f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 13,
                [2] = 8,
                [1] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [46182] = 16,
            },
        },
        [538] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Gather | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 98,
            TerritoryId = 1237,
            Radius = 80,
            MapPosition = new Vector2(-270f, 140f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 13,
                [2] = 8,
                [1] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [46184] = 16,
            },
        },
        [539] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Gather | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 22,
            LunarCredit = 93,
            TerritoryId = 1237,
            Radius = 80,
            MapPosition = new Vector2(566f, -908f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 12,
                [2] = 7,
                [1] = 7,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [46282] = 16,
            },
        },
        [540] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Gather | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 20,
            LunarCredit = 84,
            TerritoryId = 1237,
            Radius = 80,
            MapPosition = new Vector2(188f, -201f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 11,
                [2] = 7,
                [1] = 7,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [46281] = 16,
            },
        },
        [541] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Gather | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 99,
            TerritoryId = 1237,
            Radius = 80,
            MapPosition = new Vector2(748f, 101f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 13,
                [2] = 8,
                [1] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [46279] = 16,
            },
        },
        [542] = new CosmicInfo
        {
            FishCountRequired = 3,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 221,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-281f, -104f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 29,
                [2] = 18,
                [1] = 18,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Moon Bluetail"] = new HashSet<uint> { 45937 },
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [45937] = 3,
            },
        },
        [543] = new CosmicInfo
        {
            FishCountRequired = 3,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 221,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-139f, -283f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 30,
                [2] = 18,
                [1] = 18,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Sunken Drone"] = new HashSet<uint> { 45939 },
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [45939] = 3,
            },
        },
        [544] = new CosmicInfo
        {
            FishCountRequired = 3,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 221,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(104f, -269f),
            Jobs = new HashSet<uint> { 18 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 29,
                [2] = 18,
                [1] = 18,
            },
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Moonlight Pleco"] = new HashSet<uint> { 45945 },
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [45945] = 3,
            },
        },
        [545] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36950] = 3,
            },
        },
        [546] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36951] = 3,
            },
        },
        [547] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36952] = 3,
            },
        },
        [548] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36953] = 3,
            },
        },
        [549] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36954] = 3,
            },
        },
        [550] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 2,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36955] = 3,
            },
        },
        [551] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 2,
                [2] = 2,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36956] = 3,
            },
        },
        [552] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36957] = 3,
            },
        },
        [553] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36958] = 3,
            },
        },
        [554] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36960] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36959] = 2,
            },
        },
        [555] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36962] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36961] = 2,
            },
        },
        [556] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36964] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36963] = 2,
            },
        },
        [557] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36966] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36965] = 2,
            },
        },
        [558] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [2] = 6,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36967] = 3,
            },
        },
        [559] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36968] = 3,
            },
        },
        [560] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 6,
                [3] = 4,
                [2] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36969] = 2,
            },
        },
        [561] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 7,
                [2] = 7,
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36970] = 3,
            },
        },
        [562] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36971] = 3,
            },
        },
        [563] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [3] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36972] = 3,
            },
        },
        [564] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36974] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36973] = 2,
            },
        },
        [565] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36976] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36975] = 2,
            },
        },
        [566] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36977] = 3,
            },
        },
        [567] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1200,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 12,
                [2] = 12,
                [1] = 12,
            },
        },
        [568] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36979] = 1,
            },
        },
        [569] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 300,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [1] = 11,
            },
        },
        [570] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2600,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 10,
                [3] = 15,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36981] = 1,
                [36982] = 1,
                [36983] = 1,
            },
        },
        [571] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1200,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 23,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
        },
        [572] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.None,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 40,
            LunarCredit = 24,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [2] = 18,
                [1] = 18,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36985] = 1,
                [36986] = 1,
                [36987] = 1,
            },
        },
        [573] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 800,
            GoldScore = 1050,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Rain,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 8,
                [3] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36989] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36988] = 1,
            },
        },
        [574] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 16,
            EndTime = 20,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 17,
                [4] = 25,
                [3] = 25,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36991] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [36990] = 1,
            },
        },
        [575] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Clouds,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 33,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 12,
                [2] = 17,
                [1] = 17,
            },
        },
        [576] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.None,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36993] = 3,
            },
        },
        [577] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 576 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [4] = 13,
                [3] = 13,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36994] = 3,
            },
        },
        [578] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 1700,
            GoldScore = 2000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [4] = 8,
                [2] = 8,
            },
        },
        [579] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 600,
            GoldScore = 800,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 25,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 578 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36996] = 1,
            },
        },
        [580] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 400,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 31,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 579 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 10,
                [4] = 14,
                [3] = 14,
            },
        },
        [581] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1400,
            GoldScore = 2000,
            Attributes = MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 2,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 8,
                [3] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [36998] = 2,
            },
        },
        [582] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 400,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 29,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 581 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [2] = 14,
                [1] = 14,
            },
        },
        [583] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 56,
            LunarCredit = 37,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 582 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 21,
                [4] = 31,
                [3] = 31,
            },
        },
        [584] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 8,
            EndTime = 10,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 23,
                [2] = 23,
                [1] = 23,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37002] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37001] = 1,
            },
        },
        [585] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 584 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 17,
                [2] = 26,
                [1] = 26,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37004] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37003] = 1,
            },
        },
        [586] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 98,
            LunarCredit = 65,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 585 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 34,
                [4] = 50,
                [3] = 50,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37006] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37005] = 1,
            },
        },
        [587] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37007] = 3,
            },
        },
        [588] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37008] = 3,
            },
        },
        [589] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37009] = 3,
            },
        },
        [590] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37010] = 3,
            },
        },
        [591] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37011] = 3,
            },
        },
        [592] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 2,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37012] = 3,
            },
        },
        [593] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 2,
                [2] = 2,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37013] = 3,
            },
        },
        [594] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37014] = 3,
            },
        },
        [595] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37015] = 3,
            },
        },
        [596] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37017] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37016] = 2,
            },
        },
        [597] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37019] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37018] = 2,
            },
        },
        [598] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37021] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37020] = 2,
            },
        },
        [599] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37023] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37022] = 2,
            },
        },
        [600] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [2] = 6,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37024] = 3,
            },
        },
        [601] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37025] = 3,
            },
        },
        [602] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 6,
                [3] = 4,
                [2] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37026] = 2,
            },
        },
        [603] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 7,
                [2] = 7,
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37027] = 3,
            },
        },
        [604] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37028] = 3,
            },
        },
        [605] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [3] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37029] = 3,
            },
        },
        [606] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37031] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37030] = 2,
            },
        },
        [607] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37033] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37032] = 2,
            },
        },
        [608] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37034] = 3,
            },
        },
        [609] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1200,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 12,
                [2] = 12,
                [1] = 12,
            },
        },
        [610] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37036] = 1,
            },
        },
        [611] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 300,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [1] = 11,
            },
        },
        [612] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2600,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 10,
                [3] = 15,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37038] = 1,
                [37039] = 1,
                [37040] = 1,
            },
        },
        [613] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1200,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 23,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
        },
        [614] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.None,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 40,
            LunarCredit = 24,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [2] = 18,
                [1] = 18,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37042] = 1,
                [37043] = 1,
                [37044] = 1,
            },
        },
        [615] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 800,
            GoldScore = 1050,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Clouds,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 8,
                [3] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37046] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37045] = 1,
            },
        },
        [616] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 20,
            EndTime = 24,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 17,
                [4] = 25,
                [3] = 25,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37048] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37047] = 1,
            },
        },
        [617] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Rain,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 33,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 12,
                [2] = 17,
                [1] = 17,
            },
        },
        [618] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.None,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37050] = 3,
            },
        },
        [619] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 618 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [4] = 13,
                [3] = 13,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37051] = 3,
            },
        },
        [620] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 1700,
            GoldScore = 2000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [4] = 8,
                [2] = 8,
            },
        },
        [621] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 600,
            GoldScore = 800,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 25,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 620 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37053] = 1,
            },
        },
        [622] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 400,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 31,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 621 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 10,
                [4] = 14,
                [3] = 14,
            },
        },
        [623] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1400,
            GoldScore = 2000,
            Attributes = MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 4,
            EndTime = 6,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 8,
                [3] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37055] = 2,
            },
        },
        [624] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 400,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 29,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 623 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [2] = 14,
                [1] = 14,
            },
        },
        [625] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 56,
            LunarCredit = 37,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 624 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 21,
                [4] = 31,
                [3] = 31,
            },
        },
        [626] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 12,
            EndTime = 14,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 23,
                [2] = 23,
                [1] = 23,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37059] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37058] = 1,
            },
        },
        [627] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 626 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 17,
                [2] = 26,
                [1] = 26,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37061] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37060] = 1,
            },
        },
        [628] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 98,
            LunarCredit = 65,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 627 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 34,
                [4] = 50,
                [3] = 50,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37063] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37062] = 1,
            },
        },
        [629] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37064] = 3,
            },
        },
        [630] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37065] = 3,
            },
        },
        [631] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37066] = 3,
            },
        },
        [632] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37067] = 3,
            },
        },
        [633] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37068] = 3,
            },
        },
        [634] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 2,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37069] = 3,
            },
        },
        [635] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 2,
                [2] = 2,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37070] = 3,
            },
        },
        [636] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37071] = 3,
            },
        },
        [637] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37072] = 3,
            },
        },
        [638] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37074] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37073] = 2,
            },
        },
        [639] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37076] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37075] = 2,
            },
        },
        [640] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37078] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37077] = 2,
            },
        },
        [641] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37080] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37079] = 2,
            },
        },
        [642] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [2] = 6,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37081] = 3,
            },
        },
        [643] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37082] = 3,
            },
        },
        [644] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 6,
                [3] = 4,
                [2] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37083] = 2,
            },
        },
        [645] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 7,
                [2] = 7,
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37084] = 3,
            },
        },
        [646] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37085] = 3,
            },
        },
        [647] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [3] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37086] = 3,
            },
        },
        [648] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37088] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37087] = 2,
            },
        },
        [649] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37090] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37089] = 2,
            },
        },
        [650] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37091] = 3,
            },
        },
        [651] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1200,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 12,
                [2] = 12,
                [1] = 12,
            },
        },
        [652] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37093] = 1,
            },
        },
        [653] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 300,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [1] = 11,
            },
        },
        [654] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2600,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 10,
                [3] = 15,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37095] = 1,
                [37096] = 1,
                [37097] = 1,
            },
        },
        [655] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1200,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 23,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
        },
        [656] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.None,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 40,
            LunarCredit = 24,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [2] = 18,
                [1] = 18,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37099] = 1,
                [37100] = 1,
                [37101] = 1,
            },
        },
        [657] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 800,
            GoldScore = 1050,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Rain,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 8,
                [3] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37103] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37102] = 1,
            },
        },
        [658] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 4,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 17,
                [4] = 25,
                [3] = 25,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37105] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37104] = 1,
            },
        },
        [659] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Clouds,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 33,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 12,
                [2] = 17,
                [1] = 17,
            },
        },
        [660] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.None,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37107] = 3,
            },
        },
        [661] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 660 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [4] = 13,
                [3] = 13,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37108] = 3,
            },
        },
        [662] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 1700,
            GoldScore = 2000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [4] = 8,
                [2] = 8,
            },
        },
        [663] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 600,
            GoldScore = 800,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 25,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 662 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37110] = 1,
            },
        },
        [664] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 400,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 31,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 663 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 10,
                [4] = 14,
                [3] = 14,
            },
        },
        [665] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1400,
            GoldScore = 2000,
            Attributes = MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 8,
            EndTime = 10,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 8,
                [3] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37112] = 2,
            },
        },
        [666] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 400,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 29,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 665 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [2] = 14,
                [1] = 14,
            },
        },
        [667] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 56,
            LunarCredit = 37,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 666 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 21,
                [4] = 31,
                [3] = 31,
            },
        },
        [668] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 16,
            EndTime = 18,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 23,
                [2] = 23,
                [1] = 23,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37116] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37115] = 1,
            },
        },
        [669] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 668 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 17,
                [2] = 26,
                [1] = 26,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37118] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37117] = 1,
            },
        },
        [670] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 98,
            LunarCredit = 65,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 669 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 34,
                [4] = 50,
                [3] = 50,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37120] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37119] = 1,
            },
        },
        [671] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37121] = 3,
            },
        },
        [672] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37122] = 3,
            },
        },
        [673] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37123] = 3,
            },
        },
        [674] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37124] = 3,
            },
        },
        [675] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37125] = 3,
            },
        },
        [676] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 2,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37126] = 3,
            },
        },
        [677] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 2,
                [2] = 2,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37127] = 3,
            },
        },
        [678] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37128] = 3,
            },
        },
        [679] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37129] = 3,
            },
        },
        [680] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37131] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37130] = 2,
            },
        },
        [681] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37133] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37132] = 2,
            },
        },
        [682] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37135] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37134] = 2,
            },
        },
        [683] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37137] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37136] = 2,
            },
        },
        [684] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [2] = 6,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37138] = 3,
            },
        },
        [685] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37139] = 3,
            },
        },
        [686] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 6,
                [3] = 4,
                [2] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37140] = 2,
            },
        },
        [687] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 7,
                [2] = 7,
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37141] = 3,
            },
        },
        [688] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37142] = 3,
            },
        },
        [689] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [3] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37143] = 3,
            },
        },
        [690] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37145] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37144] = 2,
            },
        },
        [691] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37147] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37146] = 2,
            },
        },
        [692] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37148] = 3,
            },
        },
        [693] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1200,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 12,
                [2] = 12,
                [1] = 12,
            },
        },
        [694] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37150] = 1,
            },
        },
        [695] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 300,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [1] = 11,
            },
        },
        [696] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2600,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 10,
                [3] = 15,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37152] = 1,
                [37153] = 1,
                [37154] = 1,
            },
        },
        [697] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1200,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 23,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
        },
        [698] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.None,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 40,
            LunarCredit = 24,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [2] = 18,
                [1] = 18,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37156] = 1,
                [37157] = 1,
                [37158] = 1,
            },
        },
        [699] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 800,
            GoldScore = 1050,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Clouds,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 8,
                [3] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37160] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37159] = 1,
            },
        },
        [700] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 4,
            EndTime = 8,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 17,
                [4] = 25,
                [3] = 25,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37162] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37161] = 1,
            },
        },
        [701] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Rain,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 33,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 12,
                [2] = 17,
                [1] = 17,
            },
        },
        [702] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.None,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37164] = 3,
            },
        },
        [703] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 702 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [4] = 13,
                [3] = 13,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37165] = 3,
            },
        },
        [704] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 1700,
            GoldScore = 2000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [4] = 8,
                [2] = 8,
            },
        },
        [705] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 600,
            GoldScore = 800,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 25,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 704 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37167] = 1,
            },
        },
        [706] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 400,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 31,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 705 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 10,
                [4] = 14,
                [3] = 14,
            },
        },
        [707] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1400,
            GoldScore = 2000,
            Attributes = MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 12,
            EndTime = 14,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 8,
                [3] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37169] = 2,
            },
        },
        [708] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 400,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 29,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 707 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [2] = 14,
                [1] = 14,
            },
        },
        [709] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 56,
            LunarCredit = 37,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 708 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 21,
                [4] = 31,
                [3] = 31,
            },
        },
        [710] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 20,
            EndTime = 22,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 23,
                [2] = 23,
                [1] = 23,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37173] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37172] = 1,
            },
        },
        [711] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 710 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 17,
                [2] = 26,
                [1] = 26,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37175] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37174] = 1,
            },
        },
        [712] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 98,
            LunarCredit = 65,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 711 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 34,
                [4] = 50,
                [3] = 50,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37177] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37176] = 1,
            },
        },
        [713] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37178] = 3,
            },
        },
        [714] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37179] = 3,
            },
        },
        [715] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37180] = 3,
            },
        },
        [716] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37181] = 3,
            },
        },
        [717] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37182] = 3,
            },
        },
        [718] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 2,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37183] = 3,
            },
        },
        [719] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 2,
                [2] = 2,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37184] = 3,
            },
        },
        [720] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37185] = 3,
            },
        },
        [721] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37186] = 3,
            },
        },
        [722] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37188] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37187] = 2,
            },
        },
        [723] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37190] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37189] = 2,
            },
        },
        [724] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37192] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37191] = 2,
            },
        },
        [725] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37194] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37193] = 2,
            },
        },
        [726] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [2] = 6,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37195] = 3,
            },
        },
        [727] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37196] = 3,
            },
        },
        [728] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 6,
                [3] = 4,
                [2] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37197] = 2,
            },
        },
        [729] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 7,
                [2] = 7,
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37198] = 3,
            },
        },
        [730] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37199] = 3,
            },
        },
        [731] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [3] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37200] = 3,
            },
        },
        [732] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37202] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37201] = 2,
            },
        },
        [733] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37204] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37203] = 2,
            },
        },
        [734] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37205] = 3,
            },
        },
        [735] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1200,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 12,
                [2] = 12,
                [1] = 12,
            },
        },
        [736] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37207] = 1,
            },
        },
        [737] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 300,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [1] = 11,
            },
        },
        [738] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2600,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 10,
                [3] = 15,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37209] = 1,
                [37210] = 1,
                [37211] = 1,
            },
        },
        [739] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1200,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 23,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
        },
        [740] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.None,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 40,
            LunarCredit = 24,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [2] = 18,
                [1] = 18,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37213] = 1,
                [37214] = 1,
                [37215] = 1,
            },
        },
        [741] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 800,
            GoldScore = 1050,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Rain,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 8,
                [3] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37217] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37216] = 1,
            },
        },
        [742] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 8,
            EndTime = 12,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 17,
                [4] = 25,
                [3] = 25,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37219] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37218] = 1,
            },
        },
        [743] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Clouds,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 33,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 12,
                [2] = 17,
                [1] = 17,
            },
        },
        [744] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.None,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37221] = 3,
            },
        },
        [745] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 744 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [4] = 13,
                [3] = 13,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37222] = 3,
            },
        },
        [746] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 1700,
            GoldScore = 2000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [4] = 8,
                [2] = 8,
            },
        },
        [747] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 600,
            GoldScore = 800,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 25,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 746 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37224] = 1,
            },
        },
        [748] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 400,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 31,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 747 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 10,
                [4] = 14,
                [3] = 14,
            },
        },
        [749] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1400,
            GoldScore = 2000,
            Attributes = MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 16,
            EndTime = 18,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 8,
                [3] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37226] = 2,
            },
        },
        [750] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 400,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 29,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 749 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [2] = 14,
                [1] = 14,
            },
        },
        [751] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 56,
            LunarCredit = 37,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 750 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 21,
                [4] = 31,
                [3] = 31,
            },
        },
        [752] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 2,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 23,
                [2] = 23,
                [1] = 23,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37230] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37229] = 1,
            },
        },
        [753] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 752 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 17,
                [2] = 26,
                [1] = 26,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37232] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37231] = 1,
            },
        },
        [754] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 98,
            LunarCredit = 65,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 753 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 34,
                [4] = 50,
                [3] = 50,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37234] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37233] = 1,
            },
        },
        [755] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37235] = 3,
            },
        },
        [756] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37236] = 3,
            },
        },
        [757] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37237] = 3,
            },
        },
        [758] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37238] = 3,
            },
        },
        [759] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37239] = 3,
            },
        },
        [760] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 2,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37240] = 3,
            },
        },
        [761] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 2,
                [2] = 2,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37241] = 3,
            },
        },
        [762] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37242] = 3,
            },
        },
        [763] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37243] = 3,
            },
        },
        [764] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37245] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37244] = 2,
            },
        },
        [765] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37247] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37246] = 2,
            },
        },
        [766] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37249] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37248] = 2,
            },
        },
        [767] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37251] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37250] = 2,
            },
        },
        [768] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [2] = 6,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37252] = 3,
            },
        },
        [769] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37253] = 3,
            },
        },
        [770] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 6,
                [3] = 4,
                [2] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37254] = 2,
            },
        },
        [771] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 7,
                [2] = 7,
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37255] = 3,
            },
        },
        [772] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37256] = 3,
            },
        },
        [773] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [3] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37257] = 3,
            },
        },
        [774] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37259] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37258] = 2,
            },
        },
        [775] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37261] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37260] = 2,
            },
        },
        [776] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37262] = 3,
            },
        },
        [777] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1200,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 12,
                [2] = 12,
                [1] = 12,
            },
        },
        [778] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37264] = 1,
            },
        },
        [779] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 300,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [1] = 11,
            },
        },
        [780] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2600,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 10,
                [3] = 15,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37266] = 1,
                [37267] = 1,
                [37268] = 1,
            },
        },
        [781] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1200,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 23,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
        },
        [782] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.None,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 40,
            LunarCredit = 24,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [2] = 18,
                [1] = 18,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37270] = 1,
                [37271] = 1,
                [37272] = 1,
            },
        },
        [783] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 800,
            GoldScore = 1050,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Clouds,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 8,
                [3] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37274] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37273] = 1,
            },
        },
        [784] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 12,
            EndTime = 16,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 17,
                [4] = 25,
                [3] = 25,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37276] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37275] = 1,
            },
        },
        [785] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Rain,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 33,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 12,
                [2] = 17,
                [1] = 17,
            },
        },
        [786] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.None,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37278] = 3,
            },
        },
        [787] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 786 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [4] = 13,
                [3] = 13,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37279] = 3,
            },
        },
        [788] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 1700,
            GoldScore = 2000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [4] = 8,
                [2] = 8,
            },
        },
        [789] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 600,
            GoldScore = 800,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 25,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 788 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37281] = 1,
            },
        },
        [790] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 400,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 31,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 789 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 10,
                [4] = 14,
                [3] = 14,
            },
        },
        [791] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1400,
            GoldScore = 2000,
            Attributes = MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 20,
            EndTime = 22,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 8,
                [3] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37283] = 2,
            },
        },
        [792] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 400,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 29,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 791 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [2] = 14,
                [1] = 14,
            },
        },
        [793] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 56,
            LunarCredit = 37,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 792 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 21,
                [4] = 31,
                [3] = 31,
            },
        },
        [794] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 4,
            EndTime = 6,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 23,
                [2] = 23,
                [1] = 23,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37287] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37286] = 1,
            },
        },
        [795] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 794 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 17,
                [2] = 26,
                [1] = 26,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37289] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37288] = 1,
            },
        },
        [796] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 98,
            LunarCredit = 65,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 795 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 34,
                [4] = 50,
                [3] = 50,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37291] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37290] = 1,
            },
        },
        [797] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37292] = 3,
            },
        },
        [798] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37293] = 3,
            },
        },
        [799] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37294] = 3,
            },
        },
        [800] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37295] = 3,
            },
        },
        [801] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37296] = 3,
            },
        },
        [802] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 2,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37297] = 3,
            },
        },
        [803] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 2,
                [2] = 2,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37298] = 3,
            },
        },
        [804] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37299] = 3,
            },
        },
        [805] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37300] = 3,
            },
        },
        [806] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37302] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37301] = 2,
            },
        },
        [807] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37304] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37303] = 2,
            },
        },
        [808] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37306] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37305] = 2,
            },
        },
        [809] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37308] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37307] = 2,
            },
        },
        [810] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [2] = 6,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37309] = 3,
            },
        },
        [811] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37310] = 3,
            },
        },
        [812] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 6,
                [3] = 4,
                [2] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37311] = 2,
            },
        },
        [813] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 7,
                [2] = 7,
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37312] = 3,
            },
        },
        [814] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37313] = 3,
            },
        },
        [815] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [3] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37314] = 3,
            },
        },
        [816] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37316] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37315] = 2,
            },
        },
        [817] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37318] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37317] = 2,
            },
        },
        [818] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37319] = 3,
            },
        },
        [819] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1200,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 12,
                [2] = 12,
                [1] = 12,
            },
        },
        [820] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37321] = 1,
            },
        },
        [821] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 300,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [1] = 11,
            },
        },
        [822] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2600,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 10,
                [3] = 15,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37323] = 1,
                [37324] = 1,
                [37325] = 1,
            },
        },
        [823] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1200,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 23,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
        },
        [824] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.None,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 40,
            LunarCredit = 24,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [2] = 18,
                [1] = 18,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37327] = 1,
                [37328] = 1,
                [37329] = 1,
            },
        },
        [825] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 800,
            GoldScore = 1050,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Rain,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 8,
                [3] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37331] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37330] = 1,
            },
        },
        [826] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 16,
            EndTime = 20,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 17,
                [4] = 25,
                [3] = 25,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37333] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37332] = 1,
            },
        },
        [827] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Clouds,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 33,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 12,
                [2] = 17,
                [1] = 17,
            },
        },
        [828] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.None,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37335] = 3,
            },
        },
        [829] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 828 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [4] = 13,
                [3] = 13,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37336] = 3,
            },
        },
        [830] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 1700,
            GoldScore = 2000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [4] = 8,
                [2] = 8,
            },
        },
        [831] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 600,
            GoldScore = 800,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 25,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 830 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37338] = 1,
            },
        },
        [832] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 400,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 31,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 831 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 10,
                [4] = 14,
                [3] = 14,
            },
        },
        [833] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1400,
            GoldScore = 2000,
            Attributes = MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 2,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 8,
                [3] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37340] = 2,
            },
        },
        [834] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 400,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 29,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 833 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [2] = 14,
                [1] = 14,
            },
        },
        [835] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 56,
            LunarCredit = 37,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 834 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 21,
                [4] = 31,
                [3] = 31,
            },
        },
        [836] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 8,
            EndTime = 10,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 23,
                [2] = 23,
                [1] = 23,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37344] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37343] = 1,
            },
        },
        [837] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 836 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 17,
                [2] = 26,
                [1] = 26,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37346] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37345] = 1,
            },
        },
        [838] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 98,
            LunarCredit = 65,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 837 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 34,
                [4] = 50,
                [3] = 50,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37348] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37347] = 1,
            },
        },
        [839] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37349] = 3,
            },
        },
        [840] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37350] = 3,
            },
        },
        [841] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37351] = 3,
            },
        },
        [842] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37352] = 3,
            },
        },
        [843] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37353] = 3,
            },
        },
        [844] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 2,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37354] = 3,
            },
        },
        [845] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 3,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 2,
                [2] = 2,
                [1] = 3,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37355] = 3,
            },
        },
        [846] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1200,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37356] = 3,
            },
        },
        [847] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1300,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37357] = 3,
            },
        },
        [848] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37359] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37358] = 2,
            },
        },
        [849] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37361] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37360] = 2,
            },
        },
        [850] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1100,
            GoldScore = 2500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37363] = 2,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37362] = 2,
            },
        },
        [851] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1500,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37365] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37364] = 2,
            },
        },
        [852] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [2] = 6,
                [1] = 6,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37366] = 3,
            },
        },
        [853] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37367] = 3,
            },
        },
        [854] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 6,
                [3] = 4,
                [2] = 4,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37368] = 2,
            },
        },
        [855] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 7,
                [2] = 7,
                [1] = 7,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37369] = 3,
            },
        },
        [856] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1600,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [2] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37370] = 3,
            },
        },
        [857] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [3] = 5,
                [1] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37371] = 3,
            },
        },
        [858] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37373] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37372] = 2,
            },
        },
        [859] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1300,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37375] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37374] = 2,
            },
        },
        [860] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37376] = 3,
            },
        },
        [861] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1200,
            SilverScore = 1800,
            GoldScore = 2400,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 12,
                [2] = 12,
                [1] = 12,
            },
        },
        [862] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37378] = 1,
            },
        },
        [863] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 300,
            SilverScore = 700,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [1] = 11,
            },
        },
        [864] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2600,
            GoldScore = 3000,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 24,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 10,
                [3] = 15,
                [2] = 15,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37380] = 1,
                [37381] = 1,
                [37382] = 1,
            },
        },
        [865] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1200,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 23,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
        },
        [866] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2500,
            GoldScore = 3000,
            Attributes = MissionAttributes.None,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 40,
            LunarCredit = 24,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 18,
                [2] = 18,
                [1] = 18,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37384] = 1,
                [37385] = 1,
                [37386] = 1,
            },
        },
        [867] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 800,
            GoldScore = 1050,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Clouds,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 8,
                [3] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37388] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37387] = 1,
            },
        },
        [868] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 20,
            EndTime = 24,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 17,
                [4] = 25,
                [3] = 25,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37390] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37389] = 1,
            },
        },
        [869] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Rain,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 33,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 12,
                [2] = 17,
                [1] = 17,
            },
        },
        [870] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.None,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 7,
                [4] = 11,
                [3] = 11,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37392] = 3,
            },
        },
        [871] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2300,
            GoldScore = 3000,
            Attributes = MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 870 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [4] = 13,
                [3] = 13,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37393] = 3,
            },
        },
        [872] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 1700,
            GoldScore = 2000,
            Attributes = MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [4] = 8,
                [2] = 8,
            },
        },
        [873] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 600,
            GoldScore = 800,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 25,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 872 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37395] = 1,
            },
        },
        [874] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 400,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 31,
            LunarCredit = 23,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 873 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 10,
                [4] = 14,
                [3] = 14,
            },
        },
        [875] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1400,
            GoldScore = 2000,
            Attributes = MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 4,
            EndTime = 6,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 8,
                [3] = 8,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37397] = 2,
            },
        },
        [876] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 400,
            SilverScore = 850,
            GoldScore = 1000,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 29,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 875 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [2] = 14,
                [1] = 14,
            },
        },
        [877] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 1000,
            GoldScore = 1600,
            Attributes = MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 56,
            LunarCredit = 37,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 876 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 21,
                [4] = 31,
                [3] = 31,
            },
        },
        [878] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 12,
            EndTime = 14,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 23,
                [2] = 23,
                [1] = 23,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37401] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37400] = 1,
            },
        },
        [879] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 980,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 53,
            LunarCredit = 36,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 878 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 17,
                [2] = 26,
                [1] = 26,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37403] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37402] = 1,
            },
        },
        [880] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1000,
            GoldScore = 1080,
            Attributes = MissionAttributes.Craft | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 98,
            LunarCredit = 65,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(0f, 0f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 879 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 34,
                [4] = 50,
                [3] = 50,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37405] = 1,
            },
            Crafts_Pre = new Dictionary<ushort, int>
            {
                [37404] = 1,
            },
        },
        [881] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 30,
            MapPosition = new Vector2(416f, -737f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 7,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47354] = 25,
            },
        },
        [882] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4920,
            GoldScore = 5020,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(414f, -755f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47354] = 24,
            },
        },
        [883] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(414f, -755f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 6,
                [1] = 9,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47354] = 28,
            },
        },
        [884] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 30,
            MapPosition = new Vector2(416f, -737f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 6,
                [2] = 4,
                [1] = 4,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47354] = 5,
                [47355] = 5,
                [47356] = 15,
            },
        },
        [885] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4920,
            GoldScore = 5020,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(414f, -755f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 5,
                [3] = 3,
                [1] = 3,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47354] = 24,
            },
        },
        [886] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(414f, -755f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47354] = 8,
                [47355] = 8,
                [47356] = 12,
            },
        },
        [887] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2050,
            SilverScore = 2350,
            GoldScore = 2850,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(414f, -755f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 5,
                [3] = 3,
                [1] = 3,
            },
        },
        [888] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 650,
            SilverScore = 750,
            GoldScore = 850,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(538f, -83f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 5,
            },
        },
        [889] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4920,
            GoldScore = 5020,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(538f, -83f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47357] = 24,
            },
        },
        [890] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(538f, -83f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 6,
                [1] = 9,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47357] = 28,
            },
        },
        [891] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 900,
            GoldScore = 1300,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 12,
            EndTime = 16,
            ClassScore = 0,
            CosmoCredit = 28,
            LunarCredit = 19,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(538f, -83f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [4] = 13,
                [3] = 13,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47380] = 45,
            },
        },
        [892] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 650,
            SilverScore = 750,
            GoldScore = 850,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 135,
            MapPosition = new Vector2(-706f, -464f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 4,
            },
        },
        [893] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1400,
            SilverScore = 1600,
            GoldScore = 2000,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 135,
            MapPosition = new Vector2(-706f, -464f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 5,
                [2] = 3,
                [1] = 3,
            },
        },
        [894] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 100,
            SilverScore = 120,
            GoldScore = 140,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 135,
            MapPosition = new Vector2(-706f, -464f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
        },
        [895] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4920,
            GoldScore = 5020,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 135,
            MapPosition = new Vector2(-706f, -464f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 5,
                [3] = 3,
                [2] = 3,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47358] = 24,
            },
        },
        [896] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 700,
            SilverScore = 1000,
            GoldScore = 1200,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Clouds,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 23,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 135,
            MapPosition = new Vector2(-706f, -464f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [2] = 12,
                [1] = 12,
            },
        },
        [897] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2350,
            GoldScore = 2500,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(157f, -37f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 4,
                [4] = 6,
                [3] = 6,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47361] = 45,
            },
        },
        [898] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3100,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 115,
            MapPosition = new Vector2(324f, -41f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 8,
                [2] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47359] = 15,
                [47360] = 15,
                [47361] = 20,
            },
        },
        [899] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 500,
            SilverScore = 600,
            GoldScore = 700,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 115,
            MapPosition = new Vector2(324f, -41f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 3,
                [4] = 5,
                [1] = 5,
            },
        },
        [900] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2800,
            SilverScore = 3400,
            GoldScore = 4000,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 115,
            MapPosition = new Vector2(324f, -41f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 4,
                [3] = 6,
                [2] = 6,
            },
        },
        [901] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2350,
            GoldScore = 2700,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 12,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(157f, -37f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [2] = 6,
                [1] = 6,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47359] = 15,
                [47360] = 15,
                [47361] = 15,
            },
        },
        [902] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 65,
            MapPosition = new Vector2(-256f, -14f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 6,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47365] = 10,
                [47366] = 15,
            },
        },
        [903] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-320f, 97f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 5,
                [2] = 5,
                [1] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47362] = 12,
                [47363] = 16,
            },
        },
        [904] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 650,
            SilverScore = 750,
            GoldScore = 850,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-320f, 97f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
        },
        [905] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1400,
            SilverScore = 1600,
            GoldScore = 2000,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-320f, 97f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
        },
        [906] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2350,
            GoldScore = 2500,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 65,
            MapPosition = new Vector2(-256f, -14f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [2] = 6,
                [1] = 6,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47365] = 15,
                [47366] = 15,
                [47367] = 15,
            },
        },
        [907] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4600,
            GoldScore = 4700,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-320f, 97f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 5,
                [2] = 5,
                [1] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47362] = 48,
            },
        },
        [908] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2600,
            GoldScore = 3000,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-320f, 97f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [4] = 8,
                [3] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47362] = 15,
                [47363] = 15,
                [47364] = 20,
            },
        },
        [909] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1350,
            GoldScore = 1600,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 21,
            LunarCredit = 13,
            TerritoryId = 1237,
            Radius = 100,
            MapPosition = new Vector2(-320f, 97f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [2] = 10,
                [1] = 10,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47381] = 25,
            },
        },
        [910] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2350,
            GoldScore = 2500,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 2,
            EndTime = 4,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 65,
            MapPosition = new Vector2(-256f, -14f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 4,
                [4] = 6,
                [3] = 6,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47365] = 45,
            },
        },
        [911] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4920,
            GoldScore = 5020,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(-72f, 525f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47368] = 24,
            },
        },
        [912] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2050,
            SilverScore = 2350,
            GoldScore = 2850,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(-72f, 525f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
        },
        [913] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 100,
            SilverScore = 120,
            GoldScore = 140,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(-72f, 525f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 5,
                [2] = 5,
                [1] = 8,
            },
        },
        [914] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4600,
            GoldScore = 4700,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(-72f, 525f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 897 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 3,
                [4] = 5,
                [3] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47368] = 48,
            },
        },
        [915] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2800,
            SilverScore = 4000,
            GoldScore = 4600,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(-72f, 525f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 898 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [2] = 7,
                [1] = 7,
            },
        },
        [916] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 700,
            SilverScore = 800,
            GoldScore = 900,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(-72f, 525f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 915 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [4] = 7,
                [3] = 7,
            },
        },
        [917] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 990,
            SilverScore = 1130,
            GoldScore = 1370,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Rain,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 33,
            LunarCredit = 24,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(362f, 438f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 11,
                [4] = 16,
                [3] = 16,
            },
        },
        [918] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 3700,
            GoldScore = 4100,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 27,
            LunarCredit = 21,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(362f, 438f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 910 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [2] = 10,
                [1] = 10,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47382] = 35,
            },
        },
        [919] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 700,
            SilverScore = 1000,
            GoldScore = 1200,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 31,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(-552f, 651f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 918 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [4] = 14,
                [3] = 14,
            },
        },
        [920] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 900,
            GoldScore = 1300,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 16,
            EndTime = 18,
            ClassScore = 0,
            CosmoCredit = 30,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(-552f, 651f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 13,
                [2] = 13,
                [1] = 13,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47383] = 45,
            },
        },
        [921] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2000,
            GoldScore = 2500,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 25,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(-552f, 651f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 920 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [2] = 13,
                [1] = 13,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47383] = 30,
            },
        },
        [922] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1000,
            SilverScore = 1400,
            GoldScore = 1750,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 40,
            LunarCredit = 27,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(-552f, 651f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 921 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 16,
                [4] = 23,
                [3] = 23,
            },
        },
        [923] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 60,
            MapPosition = new Vector2(66f, -368f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 5,
                [1] = 7,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47385] = 25,
            },
        },
        [924] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4920,
            GoldScore = 5020,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 132,
            MapPosition = new Vector2(-88f, -312f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47385] = 24,
            },
        },
        [925] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 132,
            MapPosition = new Vector2(-88f, -312f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 6,
                [1] = 9,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47385] = 28,
            },
        },
        [926] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 60,
            MapPosition = new Vector2(66f, -368f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 6,
                [2] = 4,
                [1] = 4,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47385] = 5,
                [47386] = 5,
                [47387] = 15,
            },
        },
        [927] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4920,
            GoldScore = 5020,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 132,
            MapPosition = new Vector2(-88f, -312f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 5,
                [3] = 3,
                [1] = 3,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47385] = 24,
            },
        },
        [928] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 132,
            MapPosition = new Vector2(-88f, -312f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47385] = 8,
                [47386] = 8,
                [47387] = 12,
            },
        },
        [929] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2050,
            SilverScore = 2350,
            GoldScore = 2850,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 132,
            MapPosition = new Vector2(-88f, -312f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 5,
                [3] = 3,
                [1] = 3,
            },
        },
        [930] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 650,
            SilverScore = 750,
            GoldScore = 850,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 1,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(724f, -319f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 5,
            },
        },
        [931] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4920,
            GoldScore = 5020,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(724f, -319f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47388] = 24,
            },
        },
        [932] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(724f, -319f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 6,
                [1] = 9,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47388] = 28,
            },
        },
        [933] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 900,
            GoldScore = 1300,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 20,
            EndTime = 24,
            ClassScore = 0,
            CosmoCredit = 28,
            LunarCredit = 19,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(724f, -319f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [4] = 13,
                [3] = 13,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47411] = 45,
            },
        },
        [934] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 650,
            SilverScore = 750,
            GoldScore = 850,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 115,
            MapPosition = new Vector2(-326f, -616f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 4,
            },
        },
        [935] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1400,
            SilverScore = 1600,
            GoldScore = 2000,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 115,
            MapPosition = new Vector2(-326f, -616f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 5,
                [2] = 3,
                [1] = 3,
            },
        },
        [936] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 100,
            SilverScore = 120,
            GoldScore = 140,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 115,
            MapPosition = new Vector2(-326f, -616f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [2] = 5,
            },
        },
        [937] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4920,
            GoldScore = 5020,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 115,
            MapPosition = new Vector2(-326f, -616f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 5,
                [3] = 3,
                [2] = 3,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47389] = 24,
            },
        },
        [938] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 700,
            SilverScore = 1000,
            GoldScore = 1200,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Rain,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 23,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 115,
            MapPosition = new Vector2(-326f, -616f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [2] = 12,
                [1] = 12,
            },
        },
        [939] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2350,
            GoldScore = 2500,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 90,
            MapPosition = new Vector2(290f, -19f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 4,
                [4] = 6,
                [3] = 6,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47392] = 45,
            },
        },
        [940] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2700,
            GoldScore = 3100,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 110,
            MapPosition = new Vector2(331f, 32f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 8,
                [2] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47390] = 15,
                [47391] = 15,
                [47392] = 20,
            },
        },
        [941] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 500,
            SilverScore = 600,
            GoldScore = 700,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 110,
            MapPosition = new Vector2(331f, 32f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 3,
                [4] = 5,
                [1] = 5,
            },
        },
        [942] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2800,
            SilverScore = 3400,
            GoldScore = 4000,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 110,
            MapPosition = new Vector2(331f, 32f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 4,
                [3] = 6,
                [2] = 6,
            },
        },
        [943] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2350,
            GoldScore = 2700,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 12,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 90,
            MapPosition = new Vector2(290f, -19f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [2] = 6,
                [1] = 6,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47390] = 15,
                [47391] = 15,
                [47392] = 15,
            },
        },
        [944] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 90,
            MapPosition = new Vector2(-237f, 187f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 6,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47396] = 10,
                [47397] = 15,
            },
        },
        [945] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1550,
            GoldScore = 1700,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 135,
            MapPosition = new Vector2(-57f, 116f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 5,
                [2] = 5,
                [1] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47393] = 12,
                [47394] = 16,
            },
        },
        [946] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 650,
            SilverScore = 750,
            GoldScore = 850,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 135,
            MapPosition = new Vector2(-57f, 116f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
        },
        [947] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1400,
            SilverScore = 1600,
            GoldScore = 2000,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 135,
            MapPosition = new Vector2(-57f, 116f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
        },
        [948] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2350,
            GoldScore = 2500,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 90,
            MapPosition = new Vector2(-237f, 187f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 6,
                [2] = 6,
                [1] = 6,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47396] = 15,
                [47397] = 15,
                [47398] = 15,
            },
        },
        [949] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4600,
            GoldScore = 4700,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 135,
            MapPosition = new Vector2(-57f, 116f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 5,
                [2] = 5,
                [1] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47393] = 48,
            },
        },
        [950] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2600,
            GoldScore = 3000,
            Attributes = MissionAttributes.Gather,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 135,
            MapPosition = new Vector2(-57f, 116f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [4] = 8,
                [3] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47393] = 15,
                [47394] = 15,
                [47395] = 20,
            },
        },
        [951] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 1350,
            GoldScore = 1600,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 21,
            LunarCredit = 13,
            TerritoryId = 1237,
            Radius = 135,
            MapPosition = new Vector2(-57f, 116f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [2] = 10,
                [1] = 10,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47412] = 25,
            },
        },
        [952] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2350,
            GoldScore = 2500,
            Attributes = MissionAttributes.Gather | MissionAttributes.Limited | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 10,
            EndTime = 12,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 90,
            MapPosition = new Vector2(-237f, 187f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 4,
                [4] = 6,
                [3] = 6,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47396] = 45,
            },
        },
        [953] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4920,
            GoldScore = 5020,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 105,
            MapPosition = new Vector2(49f, 636f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47399] = 24,
            },
        },
        [954] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2050,
            SilverScore = 2350,
            GoldScore = 2850,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreChains | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 4,
            TerritoryId = 1237,
            Radius = 105,
            MapPosition = new Vector2(49f, 636f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 3,
                [2] = 3,
                [1] = 5,
            },
        },
        [955] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 100,
            SilverScore = 120,
            GoldScore = 140,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 105,
            MapPosition = new Vector2(49f, 636f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 5,
                [2] = 5,
                [1] = 8,
            },
        },
        [956] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 4600,
            GoldScore = 4700,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 10,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 105,
            MapPosition = new Vector2(49f, 636f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 939 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 3,
                [4] = 5,
                [3] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47399] = 48,
            },
        },
        [957] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2800,
            SilverScore = 4000,
            GoldScore = 4600,
            Attributes = MissionAttributes.Gather | MissionAttributes.ScoreGatherersBoon | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 105,
            MapPosition = new Vector2(49f, 636f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 940 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [2] = 7,
                [1] = 7,
            },
        },
        [958] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 700,
            SilverScore = 800,
            GoldScore = 900,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 105,
            MapPosition = new Vector2(49f, 636f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 957 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [4] = 7,
                [3] = 7,
            },
        },
        [959] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 990,
            SilverScore = 1130,
            GoldScore = 1370,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Clouds,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 33,
            LunarCredit = 24,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(648f, 434f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 11,
                [4] = 16,
                [3] = 16,
            },
        },
        [960] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 3700,
            GoldScore = 4100,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 26,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 120,
            MapPosition = new Vector2(648f, 434f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 952 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [2] = 10,
                [1] = 10,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47413] = 35,
            },
        },
        [961] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 700,
            SilverScore = 1000,
            GoldScore = 1200,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 30,
            LunarCredit = 21,
            TerritoryId = 1237,
            Radius = 115,
            MapPosition = new Vector2(-333f, 560f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 960 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [4] = 14,
                [3] = 14,
            },
        },
        [962] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 900,
            GoldScore = 1300,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 2,
            ClassScore = 0,
            CosmoCredit = 30,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 115,
            MapPosition = new Vector2(-333f, 560f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 13,
                [2] = 13,
                [1] = 13,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47414] = 45,
            },
        },
        [963] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2000,
            GoldScore = 2500,
            Attributes = MissionAttributes.Gather | MissionAttributes.ReducedItems | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 26,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 115,
            MapPosition = new Vector2(-333f, 560f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 962 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [2] = 13,
                [1] = 13,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47414] = 30,
            },
        },
        [964] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1000,
            SilverScore = 1400,
            GoldScore = 1750,
            Attributes = MissionAttributes.Gather | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 40,
            LunarCredit = 27,
            TerritoryId = 1237,
            Radius = 115,
            MapPosition = new Vector2(-333f, 560f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 963 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 16,
                [4] = 23,
                [3] = 23,
            },
        },
        [965] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 16500,
            GoldScore = 16800,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 36,
            MapPosition = new Vector2(214f, -742f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 6,
            },
        },
        [966] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 470,
            SilverScore = 780,
            GoldScore = 1110,
            Attributes = MissionAttributes.Fish | MissionAttributes.Limited | MissionAttributes.ScoreLargestSize | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 2,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 60,
            MapPosition = new Vector2(46f, -344f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 4,
                [1] = 6,
            },
        },
        [967] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 14600,
            GoldScore = 15300,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 13,
            TerritoryId = 1237,
            Radius = 102,
            MapPosition = new Vector2(462f, -47f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 11,
                [1] = 17,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47430] = 2,
            },
        },
        [968] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 300,
            SilverScore = 1200,
            GoldScore = 1500,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 36,
            MapPosition = new Vector2(214f, -742f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 9,
                [1] = 14,
            },
        },
        [969] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 16100,
            GoldScore = 16500,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ScoreVariety,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 102,
            MapPosition = new Vector2(462f, -47f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 10,
            },
        },
        [970] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2000,
            SilverScore = 8000,
            GoldScore = 10000,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 60,
            MapPosition = new Vector2(46f, -344f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [2] = 7,
                [1] = 11,
            },
        },
        [971] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1570,
            SilverScore = 2040,
            GoldScore = 3690,
            Attributes = MissionAttributes.Fish | MissionAttributes.Limited | MissionAttributes.ScoreLargestSize | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 3,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 36,
            MapPosition = new Vector2(214f, -742f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 4,
                [2] = 4,
                [1] = 6,
            },
        },
        [972] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 14700,
            GoldScore = 15400,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 60,
            MapPosition = new Vector2(46f, -344f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 8,
                [2] = 8,
                [1] = 13,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47446] = 1,
                [47447] = 1,
            },
        },
        [973] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 400,
            SilverScore = 1600,
            GoldScore = 2000,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 36,
            MapPosition = new Vector2(214f, -742f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 9,
                [2] = 9,
                [1] = 14,
            },
        },
        [974] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 3000,
            GoldScore = 4000,
            Attributes = MissionAttributes.Fish | MissionAttributes.Limited | MissionAttributes.ScoreVariety,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 60,
            MapPosition = new Vector2(46f, -344f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 10,
                [1] = 16,
            },
        },
        [975] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 5200,
            SilverScore = 20800,
            GoldScore = 26000,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 5,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 36,
            MapPosition = new Vector2(214f, -742f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 8,
                [2] = 8,
                [1] = 13,
            },
        },
        [976] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1000,
            SilverScore = 1200,
            GoldScore = 2750,
            Attributes = MissionAttributes.Fish | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 102,
            MapPosition = new Vector2(462f, -47f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 10,
                [1] = 16,
            },
        },
        [977] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 13800,
            GoldScore = 14600,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ScoreVariety,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 8,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 102,
            MapPosition = new Vector2(462f, -47f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 10,
                [2] = 10,
                [1] = 17,
            },
        },
        [978] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 14400,
            GoldScore = 15600,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 60,
            MapPosition = new Vector2(-450f, -673f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 7,
                [2] = 5,
                [1] = 5,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47484] = 3,
            },
        },
        [979] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 2400,
            GoldScore = 3000,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 55,
            MapPosition = new Vector2(-239f, -352f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 10,
                [3] = 6,
                [1] = 6,
            },
        },
        [980] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 400,
            SilverScore = 1600,
            GoldScore = 2000,
            Attributes = MissionAttributes.Fish | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 6,
            TerritoryId = 1237,
            Radius = 50,
            MapPosition = new Vector2(-252f, -74f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 6,
                [3] = 3,
                [2] = 3,
            },
        },
        [981] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 3930,
            SilverScore = 9240,
            GoldScore = 11100,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreLargestSize | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 4,
            LunarCredit = 5,
            TerritoryId = 1237,
            Radius = 80,
            MapPosition = new Vector2(28f, 99f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 5,
                [2] = 5,
                [1] = 5,
            },
        },
        [982] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 1500,
            GoldScore = 2500,
            Attributes = MissionAttributes.Fish | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 7,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 45,
            MapPosition = new Vector2(-700f, -652f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 9,
                [2] = 6,
                [1] = 6,
            },
        },
        [983] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 13100,
            GoldScore = 14000,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ScoreVariety,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 6,
            LunarCredit = 9,
            TerritoryId = 1237,
            Radius = 60,
            MapPosition = new Vector2(46f, -344f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [3] = 5,
                [1] = 5,
            },
        },
        [984] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 13500,
            GoldScore = 14400,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 19,
            TerritoryId = 1237,
            Radius = 102,
            MapPosition = new Vector2(462f, -47f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 19,
                [3] = 12,
                [2] = 12,
            },
        },
        [985] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 9400,
            SilverScore = 37600,
            GoldScore = 47000,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 20,
            TerritoryId = 1237,
            Radius = 45,
            MapPosition = new Vector2(-700f, -652f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 21,
                [3] = 13,
                [2] = 13,
            },
        },
        [986] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 11900,
            GoldScore = 13100,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 16,
            LunarCredit = 14,
            TerritoryId = 1237,
            Radius = 36,
            MapPosition = new Vector2(214f, -742f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 11,
                [2] = 11,
                [1] = 11,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47524] = 2,
            },
        },
        [987] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 2400,
            GoldScore = 3000,
            Attributes = MissionAttributes.Fish | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 11,
            LunarCredit = 10,
            TerritoryId = 1237,
            Radius = 60,
            MapPosition = new Vector2(46f, -344f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 8,
                [2] = 8,
                [1] = 8,
            },
        },
        [988] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 14400,
            GoldScore = 15100,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 21,
            LunarCredit = 18,
            TerritoryId = 1237,
            Radius = 102,
            MapPosition = new Vector2(462f, -47f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 10,
                [4] = 14,
                [3] = 14,
            },
        },
        [989] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 800,
            SilverScore = 3200,
            GoldScore = 4000,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 9,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 45,
            MapPosition = new Vector2(-700f, -652f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 4,
                [4] = 7,
                [1] = 7,
            },
        },
        [990] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2000,
            SilverScore = 7000,
            GoldScore = 10000,
            Attributes = MissionAttributes.Fish | MissionAttributes.Collectables | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 9,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 55,
            MapPosition = new Vector2(-239f, -352f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [3] = 7,
                [2] = 7,
            },
        },
        [991] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2360,
            SilverScore = 5540,
            GoldScore = 14060,
            Attributes = MissionAttributes.Fish | MissionAttributes.Limited | MissionAttributes.ScoreLargestSize | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 50,
            MapPosition = new Vector2(-252f, -74f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 9,
                [2] = 9,
                [1] = 9,
            },
        },
        [992] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 5500,
            SilverScore = 22000,
            GoldScore = 27500,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 12,
            LunarCredit = 7,
            TerritoryId = 1237,
            Radius = 80,
            MapPosition = new Vector2(28f, 99f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [4] = 6,
                [2] = 6,
                [1] = 6,
            },
        },
        [993] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 400,
            SilverScore = 800,
            GoldScore = 3200,
            Attributes = MissionAttributes.Fish | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Rain,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 15,
            LunarCredit = 11,
            TerritoryId = 1237,
            Radius = 400,
            MapPosition = new Vector2(526f, 448f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [4] = 8,
                [3] = 8,
            },
        },
        [994] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 23000,
            SilverScore = 29500,
            GoldScore = 34500,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 4,
            EndTime = 8,
            ClassScore = 0,
            CosmoCredit = 23,
            LunarCredit = 16,
            TerritoryId = 1237,
            Radius = 45,
            MapPosition = new Vector2(-700f, -652f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [4] = 13,
                [3] = 13,
            },
        },
        [995] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 3680,
            SilverScore = 4880,
            GoldScore = 5680,
            Attributes = MissionAttributes.Fish | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalWeather,
            Weather = CosmicWeather.Clouds,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 33,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 60,
            MapPosition = new Vector2(-450f, -673f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 12,
                [2] = 19,
                [1] = 19,
            },
        },
        [996] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2360,
            SilverScore = 5540,
            GoldScore = 7390,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreLargestSize | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 9,
            LunarCredit = 8,
            TerritoryId = 1237,
            Radius = 60,
            MapPosition = new Vector2(-450f, -673f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 4,
                [4] = 6,
                [3] = 6,
            },
        },
        [997] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 14300,
            GoldScore = 15000,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 13,
            LunarCredit = 12,
            TerritoryId = 1237,
            Radius = 55,
            MapPosition = new Vector2(-239f, -352f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 996 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 8,
                [3] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47572] = 3,
            },
        },
        [998] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2360,
            SilverScore = 5540,
            GoldScore = 7390,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreLargestSize | MissionAttributes.ScoreScore,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 21,
            LunarCredit = 13,
            TerritoryId = 1237,
            Radius = 55,
            MapPosition = new Vector2(-239f, -352f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [4] = 11,
                [2] = 11,
            },
        },
        [999] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 12100,
            GoldScore = 13300,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ScoreVariety | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 30,
            LunarCredit = 22,
            TerritoryId = 1237,
            Radius = 50,
            MapPosition = new Vector2(-252f, -74f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 998 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 10,
                [2] = 15,
                [1] = 15,
            },
        },
        [1000] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 3000,
            SilverScore = 12000,
            GoldScore = 15000,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 16,
            LunarCredit = 13,
            TerritoryId = 1237,
            Radius = 80,
            MapPosition = new Vector2(28f, 99f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 999 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 8,
                [3] = 8,
            },
        },
        [1001] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 400,
            SilverScore = 1600,
            GoldScore = 2000,
            Attributes = MissionAttributes.Fish | MissionAttributes.Limited | MissionAttributes.Collectables | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 8,
            EndTime = 10,
            ClassScore = 0,
            CosmoCredit = 17,
            LunarCredit = 17,
            TerritoryId = 1237,
            Radius = 80,
            MapPosition = new Vector2(28f, 99f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 9,
                [4] = 13,
                [3] = 13,
            },
        },
        [1002] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 5570,
            SilverScore = 13090,
            GoldScore = 29600,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreLargestSize | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 15,
            TerritoryId = 1237,
            Radius = 105,
            MapPosition = new Vector2(562f, 580f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 1001 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [2] = 8,
                [1] = 8,
            },
        },
        [1003] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 450,
            GoldScore = 900,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 46,
            LunarCredit = 32,
            TerritoryId = 1237,
            Radius = 480,
            MapPosition = new Vector2(-522f, 462f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 1002 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 16,
                [4] = 24,
                [3] = 24,
            },
        },
        [1004] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 13000,
            SilverScore = 23000,
            GoldScore = 43000,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalTimed,
            Weather = CosmicWeather.FairSkies,
            StartTime = 18,
            EndTime = 20,
            ClassScore = 0,
            CosmoCredit = 50,
            LunarCredit = 33,
            TerritoryId = 1237,
            Radius = 45,
            MapPosition = new Vector2(-700f, -652f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 28,
                [2] = 28,
                [1] = 28,
            },
        },
        [1005] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 2800,
            GoldScore = 4200,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreTimeRemaining | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 55,
            LunarCredit = 37,
            TerritoryId = 1237,
            Radius = 400,
            MapPosition = new Vector2(526f, 448f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 1004 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 19,
                [2] = 29,
                [1] = 29,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47661] = 1,
            },
        },
        [1006] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 16600,
            SilverScore = 22200,
            GoldScore = 22590,
            Attributes = MissionAttributes.Fish | MissionAttributes.ScoreLargestSize | MissionAttributes.ScoreScore | MissionAttributes.ProvisionalSequential,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 77,
            LunarCredit = 52,
            TerritoryId = 1237,
            Radius = 480,
            MapPosition = new Vector2(-522f, 462f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 1005 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 32,
                [4] = 48,
                [3] = 48,
            },
        },
        [1007] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(426f, -439f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37406] = 1,
            },
        },
        [1008] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(6f, 339f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [4] = 12,
                [3] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37407] = 1,
            },
        },
        [1009] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(396f, 213f),
            Jobs = new HashSet<uint> { 8 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [4] = 12,
                [2] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37408] = 1,
            },
        },
        [1010] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(-617f, -514f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37409] = 1,
            },
        },
        [1011] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(-507f, -757f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [4] = 12,
                [3] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37410] = 1,
            },
        },
        [1012] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(-482f, 39f),
            Jobs = new HashSet<uint> { 9 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [4] = 12,
                [2] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37411] = 1,
            },
        },
        [1013] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(411f, 36f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37412] = 1,
            },
        },
        [1014] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(396f, 213f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [4] = 12,
                [3] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37413] = 1,
            },
        },
        [1015] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(159f, 500f),
            Jobs = new HashSet<uint> { 10 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [4] = 12,
                [2] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37414] = 1,
            },
        },
        [1016] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(436f, -164f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37415] = 1,
            },
        },
        [1017] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(173f, -202f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [4] = 12,
                [3] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37416] = 1,
            },
        },
        [1018] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(159f, 500f),
            Jobs = new HashSet<uint> { 11 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [4] = 12,
                [2] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37417] = 1,
            },
        },
        [1019] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(426f, -439f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37418] = 1,
            },
        },
        [1020] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(-507f, -757f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [4] = 12,
                [3] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37419] = 1,
            },
        },
        [1021] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(396f, 213f),
            Jobs = new HashSet<uint> { 12 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [4] = 12,
                [2] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37420] = 1,
            },
        },
        [1022] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(436f, -164f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37421] = 1,
            },
        },
        [1023] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(173f, -202f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [4] = 12,
                [3] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37422] = 1,
            },
        },
        [1024] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(-482f, 39f),
            Jobs = new HashSet<uint> { 13 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [4] = 12,
                [2] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37423] = 1,
            },
        },
        [1025] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(426f, -439f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37424] = 1,
            },
        },
        [1026] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(-617f, -514f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [4] = 12,
                [3] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37425] = 1,
            },
        },
        [1027] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(-482f, 39f),
            Jobs = new HashSet<uint> { 14 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [4] = 12,
                [2] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37426] = 1,
            },
        },
        [1028] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(246f, -697f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 12,
                [2] = 12,
                [1] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37427] = 1,
            },
        },
        [1029] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(411f, 36f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [4] = 12,
                [3] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37428] = 1,
            },
        },
        [1030] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 600,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Craft | MissionAttributes.ScoreScore | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 19,
            LunarCredit = 78,
            TerritoryId = 1237,
            Radius = 0,
            MapPosition = new Vector2(554f, 635f),
            Jobs = new HashSet<uint> { 15 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [4] = 12,
                [2] = 12,
            },
            Crafts_Main = new Dictionary<ushort, int>
            {
                [37429] = 1,
            },
        },
        [1031] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Gather | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 12,
            LunarCredit = 50,
            TerritoryId = 1237,
            Radius = 43,
            MapPosition = new Vector2(-481f, -533f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 8,
                [2] = 8,
                [1] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47416] = 16,
            },
        },
        [1032] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Gather | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 12,
            LunarCredit = 50,
            TerritoryId = 1237,
            Radius = 36,
            MapPosition = new Vector2(129f, -749f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [4] = 8,
                [3] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47417] = 16,
            },
        },
        [1033] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Gather | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 12,
            LunarCredit = 50,
            TerritoryId = 1237,
            Radius = 43,
            MapPosition = new Vector2(184f, 624f),
            Jobs = new HashSet<uint> { 16 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [4] = 8,
                [2] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47418] = 16,
            },
        },
        [1034] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Gather | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 12,
            LunarCredit = 50,
            TerritoryId = 1237,
            Radius = 33,
            MapPosition = new Vector2(143f, -182f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 8,
                [2] = 8,
                [1] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47419] = 16,
            },
        },
        [1035] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Gather | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 12,
            LunarCredit = 50,
            TerritoryId = 1237,
            Radius = 36,
            MapPosition = new Vector2(-561f, -658f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [4] = 8,
                [3] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47420] = 16,
            },
        },
        [1036] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Gather | MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 12,
            LunarCredit = 50,
            TerritoryId = 1237,
            Radius = 41,
            MapPosition = new Vector2(152f, 287f),
            Jobs = new HashSet<uint> { 17 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 5,
                [4] = 8,
                [2] = 8,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47421] = 16,
            },
        },
        [1037] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 14,
            LunarCredit = 57,
            TerritoryId = 1237,
            Radius = 36,
            MapPosition = new Vector2(214f, -742f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [3] = 9,
                [2] = 9,
                [1] = 9,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47675] = 4,
            },
        },
        [1038] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 18,
            LunarCredit = 76,
            TerritoryId = 1237,
            Radius = 102,
            MapPosition = new Vector2(462f, -47f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 8,
                [4] = 12,
                [3] = 12,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47677] = 4,
            },
        },
        [1039] = new CosmicInfo
        {
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
            Attributes = MissionAttributes.Critical,
            Weather = CosmicWeather.FairSkies,
            StartTime = 0,
            EndTime = 0,
            ClassScore = 0,
            CosmoCredit = 13,
            LunarCredit = 54,
            TerritoryId = 1237,
            Radius = 105,
            MapPosition = new Vector2(562f, 580f),
            Jobs = new HashSet<uint> { 18 },
            PreviousMissions = new HashSet<uint> { 0 },
            RelicXpInfo = new Dictionary<int, int>
            {
                [5] = 6,
                [4] = 9,
                [2] = 9,
            },
            Gathering_Min = new Dictionary<uint, int>
            {
                [47680] = 4,
            },
        },
    };

    #endregion
}