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

    public class CraftingInfo
    {
        public uint ItemId { get; set; } 
        public int RequiredAmount { get; set; }
        public Dictionary<uint, int> RequiredItems { get; set; } = new();
    }

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
        public HashSet<uint> FishingBaits { get; set; } = new();

        // - - - Crafter Specific - - - //
        public Dictionary<ushort, CraftingInfo> Crafts_Main { get; set; } = new();
        public Dictionary<ushort, CraftingInfo> Crafts_Pre { get; set; } = new();

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
        public Dictionary<ushort, CosmicHelper.CraftingInfo> MainCraftsDict = [];
        public Dictionary<ushort, CosmicHelper.CraftingInfo> PreCraftDict = [];
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

    };

    #endregion
}