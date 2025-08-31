using Dalamud.Interface.Textures;
using ECommons.GameHelpers;
using ICE.Enums;
using System.Collections.Generic;

namespace ICE.Utilities;

public static unsafe partial class CosmicHelper
{

    public static readonly HashSet<uint> Ranks = [1, 2, 3, 4];
    public static readonly HashSet<uint> ARankIds = [4, 5, 6];

    public static readonly HashSet<int> CrafterJobList = [8, 9, 10, 11, 12, 13, 14, 15];
    public static readonly HashSet<int> GatheringJobList = [16, 17, 18];

    public static readonly HashSet<int> WeatherMissionList = [30, 31, 32,];
    public static readonly HashSet<int> TimedMissionList = [40, 43,];
    public static readonly HashSet<int> CriticalMissions = [512, 513, 514,];

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
    public class MissionListInfo
    {
        public string Name { get; set; }
        public uint JobId { get; set; }
        public uint JobId2 { get; set; } = 0;
        public uint JobId3 { get; set; } = 0;
        public uint ToDoSlot { get; set; }
        public uint Rank { get; set; }
        public MissionAttributes Attributes { get; set; }
        public uint TimeLimit { get; set; }
        public uint Time { get; set; }
        public CosmicWeather Weather { get; set; }
        public uint RecipeId { get; set; } = 0;
        public uint BronzeRequirement { get; set; }
        public uint SilverRequirement { get; set; }
        public uint GoldRequirement { get; set; }
        public uint CosmoCredit { get; set; }
        public uint LunarCredit { get; set; }
        public uint missionScore { get; set; } = 0;
        public uint PreviousMissionID { get; set; }
        public uint MarkerId { get; set; }
        public uint TerritoryId { get; set; }
        public Vector2 MapPosition { get; set; } = new Vector2();
        public int Radius { get; set; }
        public uint NodeSet { get; set; }
        public List<(int Type, int Amount)> ExperienceRewards { get; set; }
        public HashSet<uint> StartingItems { get; set; } = new HashSet<uint>();
    }

    public static Dictionary<uint, MissionListInfo> MissionInfoDict = [];
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

    public class FishingMissionInfo
    {
        public Dictionary<string, HashSet<uint>> fishingItems { get; set; } = new();
        public int amount { get; set; } = 0;
    }

    /// <summary>
    /// Key: MissionId
    /// Value: 
    /// </summary>
    public static Dictionary<uint, FishingMissionInfo> CustomFishDict = new()
    {

    };

    public class XPType
    {
        public int CurrentXP { get; set; }
        public int NeededXP { get; set; }
    }

    public class FishingInfo
    {
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
        /// <summary>
        /// Minimim score that is required to turnin
        /// </summary>
        public uint BronzeScore { get; set; } = 0;
        /// <summary>
        /// Silver score requirement
        /// </summary>
        public uint SilverScore { get; set; } = 0;
        /// <summary>
        /// Gold score requirement
        /// </summary>
        public uint GoldScore { get; set; } = 0;
    }

    public static Dictionary<uint, FishingInfo> FishingMission = new()
    {
        [451] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Astacus Lamentorum"] = new HashSet<uint> { 45693, 45705, 45715, 45727, 45744, 45826, 45847 },
                ["Lunar Tilapia"] = new HashSet<uint> { 45694 },
                ["Lunar Blue Guppy"] = new HashSet<uint> { 45695 },
            },
            FishCountRequired = 5,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
        },
        [452] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 540,
            SilverScore = 630,
            GoldScore = 1280,
        },
        [453] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Cobalt Eel"] = new HashSet<uint> { 45701 },
            },
            FishCountRequired = 0,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
        },
        [454] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1000,
            SilverScore = 1300,
            GoldScore = 1500,
        },
        [455] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Astacus Lamentorum"] = new HashSet<uint> { 45693, 45705, 45715, 45727, 45744, 45826, 45847 },
                ["Lunar Peacock Bass"] = new HashSet<uint> { 45706 },
                ["Lunar Hemiodus"] = new HashSet<uint> { 45707 },
            },
            FishCountRequired = 2,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
        },
        [456] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2000,
            SilverScore = 5200,
            GoldScore = 5500,
        },
        [457] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1570,
            SilverScore = 2040,
            GoldScore = 3690,
        },
        [458] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Weepingeye"] = new HashSet<uint> { 45717 },
                ["Lunar Eel"] = new HashSet<uint> { 45718 },
            },
            FishCountRequired = 2, // TODO: Set based on mission requirements
            BronzeScore = 0, // TODO: Determine bronze requirement
            SilverScore = 0,
            GoldScore = 0,
        },
        [459] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1000,
            SilverScore = 1500,
            GoldScore = 2000,
        },
        [460] = new FishingInfo
        {
            FishCountRequired = 2,
            BronzeScore = 0,
            SilverScore = 3000,
            GoldScore = 4000,
        },
        [461] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 20000,
            SilverScore = 31000,
            GoldScore = 37000,
        },
        [462] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 200,
            SilverScore = 300,
            GoldScore = 1000,
        },
        [463] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Melancholia"] = new HashSet<uint> { 45696, 45708, 45735, 45754, 45778, 45853, 45938 },
                ["Hopped-on Leaffish"] = new HashSet<uint> { 45736 },
                ["Lunar Discus"] = new HashSet<uint> { 45737 },
                ["Solar Flarefish"] = new HashSet<uint> { 45738 },
            },
            FishCountRequired = 4,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
        },
        [464] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Prismatic Fish"] = new HashSet<uint> { 45743 },
            },
            FishCountRequired = 3, // TODO: Set based on mission requirements
            BronzeScore = 0, // TODO: Determine bronze requirement
            SilverScore = 0,
            GoldScore = 0,
        },
        [465] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 3000,
            SilverScore = 3500,
            GoldScore = 3700,
        },
        [466] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Moonrock Candy"] = new HashSet<uint> { 45739, 45749, 45789, 45799, 45871, 45877, 45929 },
                ["Moongill"] = new HashSet<uint> { 45740, 45750, 45790, 45800, 45878, 45930 },
                ["Lunar Cabomba"] = new HashSet<uint> { 45751 },
                ["Opal Eel"] = new HashSet<uint> { 45752 },
                ["Lunar Pirarucu"] = new HashSet<uint> { 45753 },
            },
            FishCountRequired = 3,
            BronzeScore = 0,
            SilverScore = 4000,
            GoldScore = 5000,
        },
        [467] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 3690,
            SilverScore = 5540,
            GoldScore = 6280,
        },
        [468] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2400,
            SilverScore = 3800,
            GoldScore = 4500,
        },
        [469] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Bluemoon Loach"] = new HashSet<uint> { 45699, 45719, 45731, 45764, 45784, 45810, 45918, 45935 },
                ["Leaping Loach"] = new HashSet<uint> { 45765 },
                ["Lunar Bronze Pleco"] = new HashSet<uint> { 45766 },
                ["Starry Stingray"] = new HashSet<uint> { 45767 },
                ["Lunar Lungfish"] = new HashSet<uint> { 45768 },
            },
            FishCountRequired = 4,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
        },
        [470] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Star Pleco"] = new HashSet<uint> { 45702, 45711, 45723, 45769, 45820, 45841, 45913 },
                ["Lunar Grass Carp"] = new HashSet<uint> { 45712, 45770, 45821, 45842, 45914 },
                ["Macrobrachium Lunaris"] = new HashSet<uint> { 45771 },
                ["Ataxite"] = new HashSet<uint> { 45772 },
            },
            FishCountRequired = 13,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
        },
        [471] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 35000,
            SilverScore = 44000,
            GoldScore = 50000,
        },
        [472] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Platinum Bichir"] = new HashSet<uint> { 45783 },
            },
            FishCountRequired = 1,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
        },
        [473] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1500,
            SilverScore = 3000,
            GoldScore = 3500,
        },
        [474] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Moonrock Candy"] = new HashSet<uint> { 45739, 45749, 45789, 45799, 45871, 45877, 45929 },
                ["Moongill"] = new HashSet<uint> { 45740, 45750, 45790, 45800, 45878, 45930 },
                ["Darkside Bass"] = new HashSet<uint> { 45791, 45801, 45879 },
                ["Lunar Sisterscale"] = new HashSet<uint> { 45792 },
                ["Grand Crowntail Betta"] = new HashSet<uint> { 45793 },
            },
            FishCountRequired = 18,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
        },
        [475] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 5500,
            SilverScore = 7200,
            GoldScore = 7700,
        },
        [476] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 5000,
            SilverScore = 10000,
            GoldScore = 15000,
        },
        [477] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 7730,
            SilverScore = 10240,
            GoldScore = 10260,
        },
        [478] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 30000,
            SilverScore = 34000,
            GoldScore = 36000,
        },
        [479] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 1500,
            SilverScore = 2250,
            GoldScore = 2650,
        },
        [483] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Lunar Scorpion"] = new HashSet<uint> { 45759, 45773, 45794, 45804, 45815, 45865, 45895, 45923 },
                ["Moonwhip"] = new HashSet<uint> { 45760, 45774, 45795, 45805, 45816, 45866, 45924 },
                ["Lunar Anemone"] = new HashSet<uint> { 45806 },
                ["Culter Arsenici"] = new HashSet<uint> { 45807 },
                ["Lamentorum Regotoise"] = new HashSet<uint> { 45808 },
                ["Polypus Sulfuris"] = new HashSet<uint> { 45809 },
            },
            FishCountRequired = 4,
            BronzeScore = 0,
            SilverScore = 5000,
            GoldScore = 6000,
        },
        [485] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 4510,
            SilverScore = 6500,
            GoldScore = 6540,
        },
        [488] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 11700,
            SilverScore = 12200,
            GoldScore = 12900,
        },

        // Dual Class Missions
        [508] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Heavy Ataxite"] = new HashSet<uint> { 45917 },
            },
            FishCountRequired = 7,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2700,
        },
        [509] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Hopping Lungfish"] = new HashSet<uint> { 45922 },
            },
            FishCountRequired = 1, // TODO: Set based on mission requirements
            BronzeScore = 900, // TODO: Determine bronze requirement
            SilverScore = 1800,
            GoldScore = 2700,
        },

        // Time-Restricted
        [490] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 4800,
            SilverScore = 4830,
            GoldScore = 9240,
        },
        [493] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Ctenophora Lunaris"] = new HashSet<uint> { 45831, 45889, 45907 },
                ["Protomyke #721"] = new HashSet<uint> { 45832, 45890, 45908 },
                ["Argonauta Lunaris"] = new HashSet<uint> { 45833, 45891, 45909 },
                ["Aetherial Sword"] = new HashSet<uint> { 45892, 45910 },
                ["Macropinna"] = new HashSet<uint> { 45911 },
                ["Deepmoon Seadragon"] = new HashSet<uint> { 45912 },
            },
            FishCountRequired = 18, // TODO: Set based on mission requirements
            BronzeScore = 0, // TODO: Determine bronze requirement
            SilverScore = 0,
            GoldScore = 0,
        },

        // Weather Missions
        [480] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Arsenic Axolotl"] = new HashSet<uint> { 45836, 45883 },
                ["Sunny Jellyfish"] = new HashSet<uint> { 45837, 45884 },
                ["Universal Darkfin"] = new HashSet<uint> { 45838, 45885 },
                ["Etheirys Croppie"] = new HashSet<uint> { 45839 },
                ["Moon Mora"] = new HashSet<uint> { 45840 },
            },
            FishCountRequired = 18,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
        },
        [481] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Eyeballingway"] = new HashSet<uint> { 45870 },
            },
            FishCountRequired = 1,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
        },
        [482] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 4660,
            SilverScore = 8500,
            GoldScore = 8550,
        },
        [510] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Raw Moonbright Tourmaline"] = new HashSet<uint> { 45928 },
            },
            FishCountRequired = 7, // TODO: Set based on mission requirements
            BronzeScore = 900, // TODO: Determine bronze requirement
            SilverScore = 1800,
            GoldScore = 2700,
        },
        [511] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Hollow Eel"] = new HashSet<uint> { 45934 },
            },
            FishCountRequired = 1,
            BronzeScore = 900,
            SilverScore = 1800,
            GoldScore = 2700,
        },

        // Sequence
        [484] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Lunar Sturgeon"] = new HashSet<uint> { 45846 },
            },
            FishCountRequired = 1,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
        },
        [486] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Astacus Lamentorum"] = new HashSet<uint> { 45693, 45705, 45715, 45727, 45744, 45826, 45847 },
                ["Teardrop Knifefish"] = new HashSet<uint> { 45848 },
                ["Weeping Crab"] = new HashSet<uint> { 45849 },
                ["Silvermoon Tilapia"] = new HashSet<uint> { 45850 },
                ["Weeping Minnow"] = new HashSet<uint> { 45851 },
            },
            FishCountRequired = 5,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
        },
        [487] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 7900,
            SilverScore = 9600,
            GoldScore = 12300,
        },
        [489] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 2250,
            SilverScore = 3050,
            GoldScore = 3450,
        },
        [491] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Lunar Goldfish"] = new HashSet<uint> { 45859, 45901 },
                ["Lunar Minnow"] = new HashSet<uint> { 45860, 45902 },
                ["Gleamingray"] = new HashSet<uint> { 45861 },
                ["Lunar Butterfly"] = new HashSet<uint> { 45862 },
                ["Lunar Seagrapes"] = new HashSet<uint> { 45863 },
                ["Fishingway"] = new HashSet<uint> { 45864 },
            },
            FishCountRequired = 4,
            BronzeScore = 0,
            SilverScore = 5000,
            GoldScore = 6000,
        },
        [492] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 11570,
            SilverScore = 11640,
            GoldScore = 23140,
        },
        [494] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Lunar Scorpion"] = new HashSet<uint> { 45759, 45773, 45794, 45804, 45815, 45865, 45895, 45923 },
                ["Arsenical Proto-hropken"] = new HashSet<uint> { 45896 },
                ["Lunar Oil Eel"] = new HashSet<uint> { 45897 },
                ["Galactic Noise"] = new HashSet<uint> { 45898 },
                ["Onychodictyon"] = new HashSet<uint> { 45899 },
                ["Eolactoria Arsenici"] = new HashSet<uint> { 45900 },
            },
            FishCountRequired = 6,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
        },
        [495] = new FishingInfo
        {
            FishCountRequired = 0,
            BronzeScore = 6000,
            SilverScore = 8000,
            GoldScore = 20000,
        },

        // Critical Engagements
        [542] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Moon Bluetail"] = new HashSet<uint> { 45937 },
            },
            FishCountRequired = 3,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
        },
        [543] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Sunken Drone"] = new HashSet<uint> { 45939 },
            },
            FishCountRequired = 3,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
        },
        [544] = new FishingInfo
        {
            RequiredFish = new Dictionary<string, HashSet<uint>>
            {
                ["Moonlight Pleco"] = new HashSet<uint> { 45945 },
            },
            FishCountRequired = 3,
            BronzeScore = 0,
            SilverScore = 0,
            GoldScore = 0,
        },
    };

    #endregion
}