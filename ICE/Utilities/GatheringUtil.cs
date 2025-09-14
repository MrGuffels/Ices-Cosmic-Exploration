using System.Collections.Generic;

namespace ICE.Utilities;

public static unsafe class GatheringUtil
{

    public class GatheringActions
    {
        /// <summary>
        /// Internal name for myself to know wtf this is
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// Sheet name
        /// </summary>
        public string BtnName { get; set; }
        /// <summary>
        /// Botanist Action ID
        /// </summary>
        public uint BtnActionId { get; set; }
        /// <summary>
        /// Sheet name
        /// </summary>
        public string MinName { get; set; }
        /// <summary>
        /// Miner Action ID
        /// </summary>
        public uint MinActionId { get; set; }
        /// <summary>
        /// If it has a status, the ID associated with it
        /// </summary>
        public uint StatusId { get; set; }
        /// <summary>
        /// The status name attached to it (personal use)
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// The amount of GP required for the skill
        /// </summary>
        public int RequiredGp { get; set; }
    }

    public static Dictionary<string, GatheringActions> GathActionDict = new()
    {
        { "BoonIncrease1", new GatheringActions
        {
            ActionName = "Pioneer's Gift I",
            BtnName = "",
            BtnActionId = 21178,
            MinName = "",
            MinActionId = 21177,
            StatusId = 2666,
            StatusName = "Gift of the Land",
            RequiredGp = 50,
        }},
        { "BoonIncrease2", new GatheringActions
        {
            ActionName = "Pioneer's Gift II",
            BtnName = "",
            BtnActionId = 25590,
            MinName = "",
            MinActionId = 25589,
            StatusId = 759,
            StatusName = "Gift of the Land II",
            RequiredGp = 100,
        }},
        { "Tidings", new GatheringActions
        {
            ActionName = "Nophica's Tidings",
            BtnName = "",
            BtnActionId = 21204,
            MinName = "",
            MinActionId = 21203,
            StatusId = 2667,
            StatusName = "Gatherer's Bounty",
            RequiredGp = 200,
        }},
        { "YieldI", new GatheringActions
        {
            ActionName = "Blessed Harvest",
            BtnName = "",
            BtnActionId = 222,
            MinName = "",
            MinActionId = 239,
            StatusId = 219,
            StatusName = "Gathering Yield Up",
            RequiredGp = 400,
        }},
        { "YieldII", new GatheringActions
        {
            ActionName = "Blessed Harvest II",
            BtnName = "",
            BtnActionId = 224,
            MinName = "",
            MinActionId = 241,
            StatusId = 219,
            StatusName = "Gathering Yield Up",
            RequiredGp = 500,
        }},
        { "IntegrityIncrease", new GatheringActions
        {
            ActionName = "Ageless Words",
            BtnName = "",
            BtnActionId = 215,
            MinName = "",
            MinActionId = 232,
            RequiredGp = 300,
        }},
        { "BonusIntegrityChance", new GatheringActions
        {
            ActionName = "Wise of the World",
            BtnName = "",
            BtnActionId = 26522,
            MinName = "",
            MinActionId = 26521,
            StatusId = 2765,
            StatusName = "",
            RequiredGp = 0,
        }},
        { "BountifulYieldII", new GatheringActions
        {
            ActionName = "Bountiful Yield/Harvest II",
            BtnName = "",
            BtnActionId = 273,
            MinName = "",
            MinActionId = 272,
            StatusId = 1286,
            StatusName = "",
            RequiredGp = 100,
        }},
    };

    /* First things first, there's several types of missions for gathering
     * 1 Quantity Limited(Gather x amount on limited amount of nodes)
     * 2 Quantity(Gather x amount, gather more for increased score)
     * 3 Timed(Gather x amount in the time limit)
     * 4 Chain(Increase score based on chain)
     * 5 Gatherer's Boon (Increase score by hitting boon % chance)
     * 6 Chain + Boon(Get score from chain nodes + boon % chance)
     * 7 Collectables(This is going to be annoying)
     * 8 Time Steller Reduction(???) (Assuming Collectables -> Reducing for score...fuck)
     */

    public class GathNodeInfo
    {
        public Vector3 Position { get; set; }
        public Vector3 LandZone { get; set; }
        public uint NodeId { get; set; }
    }

    public class FisherSpotInfo
    {
        public Vector3 NavtoSpot { get; set; }
        public Vector3 FishingSpot { get; set; }
    }

    public static Dictionary<Vector2, List<FisherSpotInfo>> FishingLocation = new()
    {
        { new Vector2(104, -269), new List<FisherSpotInfo>() {
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(67.09f, 19.13f, -246.34f),
                FishingSpot = new Vector3(72.78f, 18.07f, -251.65f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(81.34f, 18.19f, -232.67f),
                FishingSpot = new Vector3(86.95f, 18.50f, -236.47f)
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(112.55f, 19.17f, -226.95f),
                FishingSpot = new Vector3(110.03f, 18.81f, -229.61f)
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(136.14f, 22.61f, -235.72f),
                FishingSpot = new Vector3(132.63f, 22.12f, -238.54f)
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(146.02f, 28.76f, -281.85f),
                FishingSpot = new Vector3(143.39f, 28.23f, -279.23f)
            },
        } },
        { new Vector2(-139, -283), new List<FisherSpotInfo>(){
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-119.67f, 20.40f, -285.04f),
                FishingSpot = new Vector3(-124.69f, 20.12f, -286.35f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-116.36f, 21.45f, -303.67f),
                FishingSpot = new Vector3(-119.45f, 21.23f, -302.83f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-132.22f, 22.14f, -315.09f),
                FishingSpot = new Vector3(-133.92f, 21.89f, -307.77f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-149.69f, 21.98f, -303.55f),
                FishingSpot = new Vector3(-146.28f, 21.34f, -301.40f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-157.92f, 21.41f, -274.88f),
                FishingSpot = new Vector3(-153.74f, 20.53f, -275.38f),
            }
        } },
        { new Vector2(-281, -104), new List<FisherSpotInfo>()
        {
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-263.24f, 26.04f, -123.94f),
                FishingSpot = new Vector3(-266.38f, 25.28f, -122.38f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-261.11f, 24.92f, -111.89f),
                FishingSpot = new Vector3(-262.08f, 24.88f, -111.59f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-255.11f, 22.12f, -86.71f),
                FishingSpot = new Vector3(-257.40f, 21.89f, -86.17f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-257.32f, 23.20f, -73.09f),
                FishingSpot = new Vector3(-258.54f, 22.30f, -76.33f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-286.72f, 23.03f, -70.65f),
                FishingSpot = new Vector3(-285.15f, 22.29f, -72.24f),
            },
        } },
        { new Vector2(193, 196), new List<FisherSpotInfo>()
        {
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(209.92f, 19.74f, 205.66f),
                FishingSpot = new Vector3(207.89f, 19.52f, 203.26f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(201.62f, 19.74f, 210.88f),
                FishingSpot = new Vector3(200.33f, 19.44f, 208.05f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(192.16f, 19.16f, 208.58f),
                FishingSpot = new Vector3(192.16f, 19.16f, 208.58f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(174.96f, 18.57f, 194.23f),
                FishingSpot = new Vector3(177.16f, 17.96f, 194.73f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(190.70f, 18.08f, 182.53f),
                FishingSpot = new Vector3(191.45f, 17.58f, 186.35f),
            },
        } },
        { new Vector2(573, 573), new List<FisherSpotInfo>()
        {
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(512.46f, 52.25f, 568.22f),
                FishingSpot = new Vector3(516.74f, 52.35f, 568.17f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(560.55f, 51.35f, 528.83f),
                FishingSpot = new Vector3(562.74f, 50.80f, 533.07f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(588.83f, 50.18f, 488.87f),
                FishingSpot = new Vector3(591.27f, 49.92f, 491.14f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(626.51f, 54.19f, 528.09f),
                FishingSpot = new Vector3(623.40f, 54.05f, 529.16f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(614.27f, 54.74f, 583.04f),
                FishingSpot = new Vector3(611.00f, 54.41f, 580.93f),
            },
        } },
        { new Vector2(-642, -631), new List<FisherSpotInfo>()
        {
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-683.40f, 75.44f, -602.75f),
                FishingSpot = new Vector3(-684.47f, 74.90f, -599.87f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-696.28f, 78.40f, -613.17f),
                FishingSpot = new Vector3(-697.79f, 78.65f, -611.57f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-705.91f, 80.24f, -640.99f),
                FishingSpot = new Vector3(-709.01f, 80.49f, -640.24f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-717.26f, 81.34f, -680.15f),
                FishingSpot = new Vector3(-717.43f, 81.14f, -676.65f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-751.98f, 83.22f, -657.03f),
                FishingSpot = new Vector3(-749.07f, 83.04f, -655.83f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-592.44f, 69.78f, -675.48f),
                FishingSpot = new Vector3(-589.42f, 69.64f, -675.19f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-582.07f, 70.15f, -702.85f),
                FishingSpot = new Vector3(-579.83f, 69.57f, -700.20f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-562.54f, 69.07f, -706.32f),
                FishingSpot = new Vector3(-563.15f, 68.70f, -703.20f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-531.57f, 68.99f, -686.71f),
                FishingSpot = new Vector3(-534.03f, 69.17f, -685.55f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-532.27f, 68.40f, -670.41f),
                FishingSpot = new Vector3(-535.46f, 69.00f, -671.25f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-557.52f, 68.54f, -646.11f),
                FishingSpot = new Vector3(-558.69f, 68.80f, -649.56f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-586.18f, 67.78f, -637.35f),
                FishingSpot = new Vector3(-584.33f, 68.06f, -640.09f),
            },
        } },
        { new Vector2(909, -336), new List<FisherSpotInfo>()
        {
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(914.64f, -58.02f, -324.31f),
                FishingSpot = new Vector3(917.44f, -58.05f, -323.82f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(916.67f, -58.75f, -337.77f),
                FishingSpot = new Vector3(918.65f, -58.89f, -337.07f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(920.99f, -58.76f, -351.82f),
                FishingSpot = new Vector3(921.87f, -58.69f, -350.27f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(927.40f, -58.56f, -356.18f),
                FishingSpot = new Vector3(928.81f, -58.54f, -355.07f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(934.18f, -57.12f, -361.69f),
                FishingSpot = new Vector3(935.86f, -56.89f, -360.24f),
            },
        } },
        { new Vector2(-673, 497), new List<FisherSpotInfo>()
        {
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-737.19f, 69.92f, 509.45f),
                FishingSpot = new Vector3(-742.38f, 69.99f, 511.53f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-756.84f, 70.81f, 486.75f),
                FishingSpot = new Vector3(-757.83f, 70.11f, 490.82f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-717.75f, 57.43f, 438.85f),
                FishingSpot = new Vector3(-713.94f, 57.14f, 437.88f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-721.22f, 57.21f, 409.58f),
                FishingSpot = new Vector3(-716.97f, 57.09f, 410.51f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-609.42f, 50.00f, 515.78f),
                FishingSpot = new Vector3(-607.97f, 49.84f, 519.56f),
            },
        } },
        { new Vector2(-348, 604), new List<FisherSpotInfo>()
        {
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-381.03f, 47.83f, 590.70f),
                FishingSpot = new Vector3(-376.01f, 47.63f, 591.75f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-372.27f, 47.92f, 572.60f),
                FishingSpot = new Vector3(-369.26f, 47.70f, 574.86f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-341.73f, 47.75f, 563.59f),
                FishingSpot = new Vector3(-342.58f, 47.59f, 567.20f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-320.82f, 47.78f, 579.43f),
                FishingSpot = new Vector3(-322.84f, 47.60f, 580.93f),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(-324.11f, 47.83f, 604.22f),
                FishingSpot = new Vector3(-327.31f, 47.65f, 603.05f),
            },
        } },
        { new Vector2(0, 0), new List<FisherSpotInfo>()
        {
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(),
                FishingSpot = new Vector3(),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(),
                FishingSpot = new Vector3(),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(),
                FishingSpot = new Vector3(),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(),
                FishingSpot = new Vector3(),
            },
            new FisherSpotInfo
            {
                NavtoSpot = new Vector3(),
                FishingSpot = new Vector3(),
            },
        } },
    };

    public static Dictionary<uint, Dictionary<Vector2, List<GathNodeInfo>>> MoonGatherLocations = new()
    {
        [1237] = new()
        {
            [new Vector2(-119f, -175f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-50.68f, 19.41f, -208.97f),
                    LandZone = new Vector3(-50.64f, 18.41f, -209.93f),
                    NodeId = 35040,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-73.10f, 20.16f, -204.29f),
                    LandZone = new Vector3(-73.45f, 19.12f, -205.25f),
                    NodeId = 35039,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-90.95f, 22.52f, -194.11f),
                    LandZone = new Vector3(-91.03f, 21.15f, -194.67f),
                    NodeId = 35038,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-109.77f, 24.82f, -187.37f),
                    LandZone = new Vector3(-109.83f, 23.74f, -188.08f),
                    NodeId = 35037,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-129.53f, 27.94f, -170.34f),
                    LandZone = new Vector3(-129.85f, 26.59f, -170.81f),
                    NodeId = 35036,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-135.65f, 28.20f, -156.82f),
                    LandZone = new Vector3(-135.94f, 27.00f, -157.07f),
                    NodeId = 35035,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-142.32f, 27.14f, -139.70f),
                    LandZone = new Vector3(-142.57f, 25.67f, -140.04f),
                    NodeId = 35034,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-153.94f, 24.31f, -124.47f),
                    LandZone = new Vector3(-154.36f, 23.29f, -124.61f),
                    NodeId = 35033,
                },
            },
            [new Vector2(-168f, -181f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-110.86f, 20.37f, -226.34f),
                    LandZone = new Vector3(-110.73f, 19.30f, -225.31f),
                    NodeId = 35062,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-117.08f, 20.90f, -230.27f),
                    LandZone = new Vector3(-118.01f, 20.00f, -230.48f),
                    NodeId = 35061,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-226.12f, 29.29f, -176.71f),
                    LandZone = new Vector3(-225.34f, 28.75f, -176.68f),
                    NodeId = 35057,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-228.37f, 27.66f, -167.48f),
                    LandZone = new Vector3(-227.69f, 27.39f, -167.57f),
                    NodeId = 35058,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-170.40f, 20.94f, -116.41f),
                    LandZone = new Vector3(-170.99f, 20.28f, -116.77f),
                    NodeId = 35059,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-162.69f, 22.16f, -124.16f),
                    LandZone = new Vector3(-162.84f, 21.93f, -125.09f),
                    NodeId = 35060,
                },
            },
            [new Vector2(96f, 259f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(60.94f, 22.63f, 204.94f),
                    LandZone = new Vector3(60.95f, 21.53f, 205.38f),
                    NodeId = 35066,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(40.95f, 21.06f, 208.70f),
                    LandZone = new Vector3(41.39f, 19.97f, 209.17f),
                    NodeId = 35065,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(65.52f, 18.94f, 318.76f),
                    LandZone = new Vector3(65.60f, 18.21f, 318.18f),
                    NodeId = 35063,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(75.12f, 19.39f, 322.87f),
                    LandZone = new Vector3(75.45f, 18.57f, 322.06f),
                    NodeId = 35064,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(172.32f, 23.64f, 275.08f),
                    LandZone = new Vector3(172.16f, 22.95f, 274.95f),
                    NodeId = 35068,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(174.37f, 23.41f, 267.84f),
                    LandZone = new Vector3(174.24f, 22.82f, 267.61f),
                    NodeId = 35067,
                },
            },
            [new Vector2(65f, -431f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(70.23f, 35.63f, -370.83f),
                    LandZone = new Vector3(69.72f, 35.02f, -370.42f),
                    NodeId = 35041,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(56.98f, 36.75f, -385.37f),
                    LandZone = new Vector3(57.49f, 35.95f, -384.87f),
                    NodeId = 35042,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(78.47f, 39.50f, -424.33f),
                    LandZone = new Vector3(77.97f, 39.06f, -424.98f),
                    NodeId = 35043,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(56.85f, 40.02f, -444.27f),
                    LandZone = new Vector3(57.26f, 39.27f, -444.00f),
                    NodeId = 35044,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(56.59f, 40.24f, -453.96f),
                    LandZone = new Vector3(57.03f, 39.60f, -454.24f),
                    NodeId = 35045,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(45.23f, 41.43f, -473.65f),
                    LandZone = new Vector3(45.55f, 40.41f, -473.40f),
                    NodeId = 35046,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(49.70f, 41.83f, -481.26f),
                    LandZone = new Vector3(50.06f, 40.96f, -481.57f),
                    NodeId = 35047,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(60.49f, 43.10f, -499.58f),
                    LandZone = new Vector3(60.52f, 42.33f, -499.64f),
                    NodeId = 35048,
                },
            },
            [new Vector2(-475f, 135f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-430.47f, 42.53f, 96.48f),
                    LandZone = new Vector3(-430.96f, 42.04f, 96.05f),
                    NodeId = 35072,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-438.91f, 43.87f, 101.09f),
                    LandZone = new Vector3(-438.25f, 42.71f, 101.14f),
                    NodeId = 35071,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-543.83f, 41.39f, 78.79f),
                    LandZone = new Vector3(-543.35f, 40.24f, 78.70f),
                    NodeId = 35074,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-542.48f, 44.36f, 91.48f),
                    LandZone = new Vector3(-542.08f, 43.15f, 91.97f),
                    NodeId = 35073,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-400.24f, 45.00f, 191.30f),
                    LandZone = new Vector3(-400.94f, 44.26f, 190.28f),
                    NodeId = 35070,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-394.25f, 44.13f, 189.71f),
                    LandZone = new Vector3(-394.35f, 43.28f, 188.94f),
                    NodeId = 35069,
                },
            },
            [new Vector2(73f, -482f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(77.56f, 39.48f, -424.00f),
                    LandZone = new Vector3(76.97f, 38.91f, -424.55f),
                    NodeId = 35078,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(87.10f, 39.77f, -424.88f),
                    LandZone = new Vector3(87.04f, 39.23f, -425.47f),
                    NodeId = 35077,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(142.59f, 47.46f, -491.28f),
                    LandZone = new Vector3(142.78f, 46.57f, -490.84f),
                    NodeId = 35076,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(135.91f, 47.36f, -500.54f),
                    LandZone = new Vector3(135.63f, 46.70f, -500.50f),
                    NodeId = 35075,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(32.70f, 43.50f, -521.08f),
                    LandZone = new Vector3(32.70f, 43.50f, -521.08f),
                    NodeId = 35079,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(15.94f, 44.39f, -526.71f),
                    LandZone = new Vector3(15.66f, 43.36f, -526.49f),
                    NodeId = 35080,
                },
            },
            [new Vector2(-463f, -729f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-419.94f, 66.80f, -692.30f),
                    LandZone = new Vector3(-420.07f, 66.15f, -691.43f),
                    NodeId = 35056,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-428.39f, 67.89f, -704.01f),
                    LandZone = new Vector3(-428.84f, 67.15f, -703.40f),
                    NodeId = 35055,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-447.28f, 68.51f, -707.15f),
                    LandZone = new Vector3(-446.41f, 67.61f, -707.17f),
                    NodeId = 35054,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-461.66f, 69.71f, -713.83f),
                    LandZone = new Vector3(-462.18f, 68.99f, -713.69f),
                    NodeId = 35053,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-462.91f, 71.27f, -731.60f),
                    LandZone = new Vector3(-463.43f, 70.55f, -731.26f),
                    NodeId = 35052,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-467.54f, 73.48f, -747.74f),
                    LandZone = new Vector3(-468.00f, 72.64f, -747.89f),
                    NodeId = 35051,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-469.04f, 76.77f, -770.29f),
                    LandZone = new Vector3(-469.29f, 76.02f, -769.88f),
                    NodeId = 35050,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-492.64f, 78.80f, -777.00f),
                    LandZone = new Vector3(-492.64f, 78.01f, -776.50f),
                    NodeId = 35049,
                },
            },
            [new Vector2(-690f, -752f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-635.22f, 73.97f, -704.67f),
                    LandZone = new Vector3(-636.22f, 73.17f, -703.98f),
                    NodeId = 35086,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-621.59f, 75.08f, -715.89f),
                    LandZone = new Vector3(-620.05f, 74.21f, -717.71f),
                    NodeId = 35085,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-671.18f, 93.37f, -819.39f),
                    LandZone = new Vector3(-670.57f, 92.57f, -819.02f),
                    NodeId = 35084,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-679.34f, 91.67f, -804.68f),
                    LandZone = new Vector3(-678.57f, 90.89f, -804.33f),
                    NodeId = 35083,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-752.37f, 88.51f, -717.92f),
                    LandZone = new Vector3(-751.92f, 87.59f, -717.87f),
                    NodeId = 35082,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-758.07f, 88.73f, -707.39f),
                    LandZone = new Vector3(-757.45f, 87.93f, -707.14f),
                    NodeId = 35081,
                },
            },
            [new Vector2(-669f, -515f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-642.11f, 69.73f, -572.83f),
                    LandZone = new Vector3(-641.41f, 68.81f, -572.60f),
                    NodeId = 35091,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-652.40f, 71.72f, -564.69f),
                    LandZone = new Vector3(-652.33f, 70.89f, -564.22f),
                    NodeId = 35092,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-731.63f, 79.66f, -509.58f),
                    LandZone = new Vector3(-731.27f, 78.70f, -509.72f),
                    NodeId = 35089,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-727.81f, 79.13f, -503.01f),
                    LandZone = new Vector3(-727.68f, 78.05f, -502.80f),
                    NodeId = 35090,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-640.96f, 60.56f, -463.86f),
                    LandZone = new Vector3(-640.66f, 59.77f, -463.80f),
                    NodeId = 35087,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-637.53f, 59.94f, -456.50f),
                    LandZone = new Vector3(-637.10f, 58.91f, -456.96f),
                    NodeId = 35088,
                },
            },
            [new Vector2(-503f, -324f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-501.87f, 48.78f, -376.74f),
                    LandZone = new Vector3(-501.87f, 48.78f, -376.74f),
                    NodeId = 35115,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-518.88f, 48.09f, -370.48f),
                    LandZone = new Vector3(-518.88f, 48.09f, -370.48f),
                    NodeId = 35116,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-541.98f, 48.69f, -303.82f),
                    LandZone = new Vector3(-541.98f, 48.69f, -303.82f),
                    NodeId = 35112,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-544.85f, 46.39f, -288.65f),
                    LandZone = new Vector3(-544.85f, 46.39f, -288.65f),
                    NodeId = 35111,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-453.94f, 43.02f, -287.68f),
                    LandZone = new Vector3(-453.94f, 43.02f, -287.68f),
                    NodeId = 35114,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-459.90f, 44.86f, -304.86f),
                    LandZone = new Vector3(-459.90f, 44.86f, -304.86f),
                    NodeId = 35113,
                },
            },
            [new Vector2(-131f, -365f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-88.08f, 22.78f, -356.08f),
                    LandZone = new Vector3(-88.08f, 22.78f, -356.08f),
                    NodeId = 35118,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-91.32f, 22.03f, -345.19f),
                    LandZone = new Vector3(-91.32f, 22.03f, -345.19f),
                    NodeId = 35117,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-127.83f, 30.41f, -423.23f),
                    LandZone = new Vector3(-127.83f, 30.41f, -423.23f),
                    NodeId = 35119,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-146.95f, 29.89f, -412.10f),
                    LandZone = new Vector3(-146.95f, 29.89f, -412.10f),
                    NodeId = 35120,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-160.71f, 25.29f, -317.52f),
                    LandZone = new Vector3(-160.71f, 25.29f, -317.52f),
                    NodeId = 35121,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-150.44f, 23.76f, -313.23f),
                    LandZone = new Vector3(-150.44f, 23.76f, -313.23f),
                    NodeId = 35122,
                },
            },
            [new Vector2(-270f, 140f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-314.19f, 27.94f, 121.84f),
                    LandZone = new Vector3(-314.19f, 27.94f, 121.84f),
                    NodeId = 35132,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-317.83f, 28.43f, 135.26f),
                    LandZone = new Vector3(-317.83f, 28.43f, 135.26f),
                    NodeId = 35133,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-277.70f, 24.80f, 187.43f),
                    LandZone = new Vector3(-277.70f, 24.80f, 187.43f),
                    NodeId = 35134,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-245.44f, 24.63f, 188.87f),
                    LandZone = new Vector3(-245.44f, 24.63f, 188.87f),
                    NodeId = 35131,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-216.66f, 19.97f, 105.91f),
                    LandZone = new Vector3(-216.66f, 19.97f, 105.91f),
                    NodeId = 35129,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-243.67f, 20.00f, 90.38f),
                    LandZone = new Vector3(-243.67f, 20.00f, 90.38f),
                    NodeId = 35130,
                },
            },
            [new Vector2(-278f, -13f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-228.04f, 17.31f, -13.59f),
                    LandZone = new Vector3(-228.84f, 16.82f, -13.53f),
                    NodeId = 35168,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-238.16f, 17.82f, -33.92f),
                    LandZone = new Vector3(-237.94f, 17.22f, -33.02f),
                    NodeId = 35167,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-325.44f, 28.14f, -51.83f),
                    LandZone = new Vector3(-324.69f, 27.81f, -51.55f),
                    NodeId = 35166,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-330.45f, 29.18f, -28.82f),
                    LandZone = new Vector3(-330.08f, 28.69f, -29.41f),
                    NodeId = 35165,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-288.95f, 25.49f, 44.38f),
                    LandZone = new Vector3(-288.93f, 25.07f, 43.86f),
                    NodeId = 35169,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-273.81f, 23.78f, 44.75f),
                    LandZone = new Vector3(-274.15f, 23.28f, 44.57f),
                    NodeId = 35170,
                },
            },
            [new Vector2(225f, 83f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(223.44f, 20.26f, 8.42f),
                    LandZone = new Vector3(223.04f, 19.41f, 8.25f),
                    NodeId = 35159,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(223.99f, 20.31f, 15.83f),
                    LandZone = new Vector3(224.33f, 19.39f, 15.45f),
                    NodeId = 35160,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(285.30f, 18.88f, 108.14f),
                    LandZone = new Vector3(285.20f, 17.89f, 108.78f),
                    NodeId = 35163,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(286.34f, 19.03f, 114.67f),
                    LandZone = new Vector3(286.04f, 18.02f, 114.53f),
                    NodeId = 35164,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(207.68f, 18.92f, 145.46f),
                    LandZone = new Vector3(207.74f, 17.93f, 145.63f),
                    NodeId = 35162,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(186.24f, 19.58f, 152.27f),
                    LandZone = new Vector3(186.34f, 18.68f, 152.56f),
                    NodeId = 35161,
                },
            },
            [new Vector2(232f, -50f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(221.91f, 20.14f, 1.22f),
                    LandZone = new Vector3(221.63f, 19.27f, 1.76f),
                    NodeId = 35137,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(214.32f, 20.07f, -9.12f),
                    LandZone = new Vector3(214.58f, 19.17f, -9.09f),
                    NodeId = 35136,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(212.42f, 18.82f, -23.42f),
                    LandZone = new Vector3(212.86f, 18.07f, -22.78f),
                    NodeId = 35141,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(231.74f, 19.90f, -27.72f),
                    LandZone = new Vector3(231.49f, 18.96f, -27.89f),
                    NodeId = 35140,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(244.55f, 21.27f, -39.45f),
                    LandZone = new Vector3(244.63f, 20.40f, -39.56f),
                    NodeId = 35139,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(251.86f, 22.50f, -54.28f),
                    LandZone = new Vector3(252.26f, 21.65f, -54.21f),
                    NodeId = 35138,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(238.31f, 21.42f, -64.92f),
                    LandZone = new Vector3(238.52f, 20.47f, -65.07f),
                    NodeId = 35142,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(225.60f, 20.73f, -83.05f),
                    LandZone = new Vector3(225.59f, 19.73f, -82.86f),
                    NodeId = 35135,
                },
            },
            [new Vector2(456f, 221f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(421.10f, 33.10f, 189.18f),
                    LandZone = new Vector3(421.21f, 32.62f, 189.40f),
                    NodeId = 35143,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(441.48f, 33.91f, 186.57f),
                    LandZone = new Vector3(441.11f, 33.22f, 186.48f),
                    NodeId = 35150,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(439.68f, 34.32f, 211.00f),
                    LandZone = new Vector3(439.92f, 33.71f, 210.52f),
                    NodeId = 35147,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(460.41f, 34.31f, 206.39f),
                    LandZone = new Vector3(460.12f, 33.59f, 206.25f),
                    NodeId = 35148,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(468.54f, 34.92f, 216.16f),
                    LandZone = new Vector3(468.20f, 34.34f, 216.77f),
                    NodeId = 35149,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(459.23f, 34.89f, 234.63f),
                    LandZone = new Vector3(459.71f, 34.33f, 234.41f),
                    NodeId = 35144,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(454.67f, 34.75f, 254.14f),
                    LandZone = new Vector3(454.88f, 34.04f, 254.43f),
                    NodeId = 35145,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(461.14f, 35.08f, 268.28f),
                    LandZone = new Vector3(461.07f, 34.47f, 267.78f),
                    NodeId = 35146,
                },
            },
            [new Vector2(-121f, 368f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-185.84f, 32.88f, 351.45f),
                    LandZone = new Vector3(-185.52f, 32.05f, 351.60f),
                    NodeId = 35176,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-174.21f, 32.41f, 342.16f),
                    LandZone = new Vector3(-174.13f, 31.51f, 342.22f),
                    NodeId = 35175,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-83.13f, 28.26f, 312.21f),
                    LandZone = new Vector3(-83.36f, 27.38f, 311.60f),
                    NodeId = 35171,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-69.83f, 27.94f, 330.99f),
                    LandZone = new Vector3(-69.73f, 27.02f, 330.61f),
                    NodeId = 35172,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-110.16f, 34.36f, 427.53f),
                    LandZone = new Vector3(-110.27f, 33.45f, 428.06f),
                    NodeId = 35173,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-116.48f, 34.47f, 438.21f),
                    LandZone = new Vector3(-116.70f, 33.51f, 437.75f),
                    NodeId = 35174,
                },
            },
            [new Vector2(455f, 243f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(490.19f, 36.57f, 174.08f),
                    LandZone = new Vector3(489.71f, 35.70f, 174.19f),
                    NodeId = 35177,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(441.94f, 36.48f, 176.07f),
                    LandZone = new Vector3(441.74f, 35.50f, 176.29f),
                    NodeId = 35178,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(382.50f, 35.42f, 265.39f),
                    LandZone = new Vector3(383.06f, 35.01f, 264.97f),
                    NodeId = 35180,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(395.40f, 37.31f, 276.67f),
                    LandZone = new Vector3(395.65f, 36.48f, 276.62f),
                    NodeId = 35179,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(476.83f, 40.68f, 297.10f),
                    LandZone = new Vector3(477.28f, 39.93f, 297.15f),
                    NodeId = 35181,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(510.84f, 37.40f, 283.41f),
                    LandZone = new Vector3(510.55f, 36.63f, 283.30f),
                    NodeId = 35182,
                },
            },
            [new Vector2(506f, 682f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(559.91f, 55.79f, 672.96f),
                    LandZone = new Vector3(559.79f, 55.21f, 672.69f),
                    NodeId = 35151,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(536.10f, 55.63f, 679.65f),
                    LandZone = new Vector3(536.37f, 54.90f, 679.87f),
                    NodeId = 35158,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(520.14f, 56.14f, 694.22f),
                    LandZone = new Vector3(520.91f, 55.61f, 693.97f),
                    NodeId = 35154,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(502.04f, 56.63f, 680.56f),
                    LandZone = new Vector3(502.08f, 55.68f, 680.68f),
                    NodeId = 35155,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(481.21f, 56.12f, 660.01f),
                    LandZone = new Vector3(481.17f, 55.23f, 660.35f),
                    NodeId = 35157,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(489.90f, 56.31f, 671.44f),
                    LandZone = new Vector3(490.20f, 55.49f, 671.69f),
                    NodeId = 35156,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(464.35f, 55.87f, 661.57f),
                    LandZone = new Vector3(464.23f, 55.16f, 661.27f),
                    NodeId = 35152,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(452.62f, 56.02f, 663.78f),
                    LandZone = new Vector3(452.79f, 55.22f, 663.50f),
                    NodeId = 35153,
                },
            },
            [new Vector2(609f, 478f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(522.22f, 53.20f, 514.22f),
                    LandZone = new Vector3(522.16f, 52.29f, 515.22f),
                    NodeId = 35183,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(529.68f, 54.04f, 488.30f),
                    LandZone = new Vector3(529.51f, 53.32f, 488.70f),
                    NodeId = 35184,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(579.70f, 54.15f, 412.27f),
                    LandZone = new Vector3(579.61f, 53.31f, 412.66f),
                    NodeId = 35185,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(592.29f, 54.46f, 429.05f),
                    LandZone = new Vector3(592.12f, 53.71f, 428.87f),
                    NodeId = 35186,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(671.40f, 64.60f, 498.43f),
                    LandZone = new Vector3(670.72f, 63.73f, 498.07f),
                    NodeId = 35187,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(651.13f, 57.95f, 522.03f),
                    LandZone = new Vector3(651.07f, 57.13f, 521.38f),
                    NodeId = 35188,
                },
            },
            [new Vector2(527f, 630f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(595.76f, 58.28f, 648.99f),
                    LandZone = new Vector3(595.16f, 57.34f, 650.08f),
                    NodeId = 35194,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(602.08f, 59.59f, 659.02f),
                    LandZone = new Vector3(601.25f, 58.52f, 658.62f),
                    NodeId = 35193,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(484.43f, 56.59f, 676.53f),
                    LandZone = new Vector3(484.55f, 55.62f, 676.34f),
                    NodeId = 35192,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(463.90f, 55.91f, 671.28f),
                    LandZone = new Vector3(464.47f, 55.17f, 670.95f),
                    NodeId = 35191,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(468.77f, 55.87f, 591.90f),
                    LandZone = new Vector3(469.39f, 55.08f, 592.10f),
                    NodeId = 35190,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(492.08f, 54.21f, 574.35f),
                    LandZone = new Vector3(491.81f, 53.34f, 574.80f),
                    NodeId = 35189,
                },
            },
            [new Vector2(-706f, 564f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-684.35f, 58.86f, 503.32f),
                    LandZone = new Vector3(-684.05f, 58.24f, 503.64f),
                    NodeId = 35196,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-701.75f, 62.75f, 512.91f),
                    LandZone = new Vector3(-701.30f, 62.17f, 513.41f),
                    NodeId = 35195,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-772.70f, 69.54f, 593.97f),
                    LandZone = new Vector3(-773.43f, 68.96f, 593.82f),
                    NodeId = 35200,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-770.47f, 70.37f, 608.22f),
                    LandZone = new Vector3(-769.76f, 69.60f, 608.24f),
                    NodeId = 35199,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-651.49f, 56.65f, 620.18f),
                    LandZone = new Vector3(-651.85f, 56.27f, 619.56f),
                    NodeId = 35198,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-649.10f, 57.32f, 604.20f),
                    LandZone = new Vector3(-649.86f, 57.03f, 604.38f),
                    NodeId = 35197,
                },
            },
            [new Vector2(566f, -908f)] = new()
            {
            },
            [new Vector2(188f, -201f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(178.51f, 28.34f, -234.28f),
                    LandZone = new Vector3(178.51f, 28.34f, -234.28f),
                    NodeId = 35225,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(197.68f, 31.66f, -243.88f),
                    LandZone = new Vector3(197.68f, 31.66f, -243.88f),
                    NodeId = 35226,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(144.96f, 22.66f, -198.45f),
                    LandZone = new Vector3(144.96f, 22.66f, -198.45f),
                    NodeId = 35230,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(167.50f, 24.01f, -186.32f),
                    LandZone = new Vector3(167.50f, 24.01f, -186.32f),
                    NodeId = 35229,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(230.11f, 26.80f, -167.23f),
                    LandZone = new Vector3(230.11f, 26.80f, -167.23f),
                    NodeId = 35228,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(234.40f, 27.38f, -175.04f),
                    LandZone = new Vector3(234.40f, 27.38f, -175.04f),
                    NodeId = 35227,
                },
            },
            [new Vector2(748f, 101f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(778.85f, 58.51f, 90.23f),
                    LandZone = new Vector3(778.85f, 58.51f, 90.23f),
                    NodeId = 35213,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(785.67f, 59.14f, 69.10f),
                    LandZone = new Vector3(785.67f, 59.14f, 69.10f),
                    NodeId = 35214,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(724.36f, 56.97f, 149.78f),
                    LandZone = new Vector3(724.36f, 56.97f, 149.78f),
                    NodeId = 35215,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(733.38f, 57.85f, 152.23f),
                    LandZone = new Vector3(733.38f, 57.85f, 152.23f),
                    NodeId = 35216,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(710.48f, 53.41f, 49.44f),
                    LandZone = new Vector3(710.48f, 53.41f, 49.44f),
                    NodeId = 35217,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(700.02f, 52.95f, 56.65f),
                    LandZone = new Vector3(700.02f, 52.95f, 56.65f),
                    NodeId = 35218,
                },
            },
            [new Vector2(-524f, 379f)] = new()
            {

            },
            [new Vector2(404f, -802f)] = new()
            {
            },
            [new Vector2(874f, -771f)] = new()
            {
            },
        },
        [1291] = new()
        {
            [new Vector2(416f, -737f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(429.20f, 131.87f, -763.91f),
                    LandZone = new Vector3(427.61f, 132.36f, -763.25f),
                    NodeId = 35292,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(429.60f, 132.24f, -758.56f),
                    LandZone = new Vector3(428.02f, 132.25f, -759.05f),
                    NodeId = 35287,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(421.66f, 132.03f, -753.53f),
                    LandZone = new Vector3(422.50f, 131.48f, -754.78f),
                    NodeId = 35289,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(427.35f, 131.98f, -751.26f),
                    LandZone = new Vector3(426.33f, 131.95f, -752.23f),
                    NodeId = 35286,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(424.18f, 131.95f, -745.52f),
                    LandZone = new Vector3(424.40f, 131.48f, -747.27f),
                    NodeId = 35288,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(425.38f, 133.77f, -733.58f),
                    LandZone = new Vector3(425.46f, 133.31f, -735.48f),
                    NodeId = 35285,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(419.32f, 132.46f, -717.05f),
                    LandZone = new Vector3(419.74f, 131.96f, -718.69f),
                    NodeId = 35290,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(416.41f, 132.54f, -718.93f),
                    LandZone = new Vector3(417.72f, 131.96f, -719.66f),
                    NodeId = 35291,
                },
            },
            [new Vector2(414f, -755f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(378.61f, 132.46f, -705.46f),
                    LandZone = new Vector3(380.25f, 131.96f, -706.13f),
                    NodeId = 35283,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(378.45f, 132.54f, -709.68f),
                    LandZone = new Vector3(379.95f, 132.12f, -709.44f),
                    NodeId = 35284,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(424.22f, 131.93f, -810.33f),
                    LandZone = new Vector3(423.90f, 131.40f, -809.02f),
                    NodeId = 35280,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(426.68f, 131.82f, -808.77f),
                    LandZone = new Vector3(426.01f, 131.26f, -807.95f),
                    NodeId = 35279,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(449.33f, 132.29f, -723.14f),
                    LandZone = new Vector3(449.00f, 131.41f, -724.58f),
                    NodeId = 35281,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(442.13f, 132.10f, -724.95f),
                    LandZone = new Vector3(442.74f, 131.32f, -725.91f),
                    NodeId = 35282,
                },
            },
            [new Vector2(538f, -83f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(478.24f, 32.61f, -81.42f),
                    LandZone = new Vector3(478.40f, 31.25f, -80.31f),
                    NodeId = 35296,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(486.70f, 32.58f, -85.90f),
                    LandZone = new Vector3(486.79f, 31.26f, -84.64f),
                    NodeId = 35295,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(561.54f, 36.35f, -130.75f),
                    LandZone = new Vector3(560.92f, 35.19f, -130.06f),
                    NodeId = 35294,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(567.13f, 30.79f, -111.63f),
                    LandZone = new Vector3(566.05f, 29.46f, -110.47f),
                    NodeId = 35293,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(572.79f, 18.04f, -42.11f),
                    LandZone = new Vector3(571.03f, 16.56f, -42.25f),
                    NodeId = 35298,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(572.07f, 17.93f, -27.12f),
                    LandZone = new Vector3(571.14f, 16.77f, -28.68f),
                    NodeId = 35297,
                },
            },
            [new Vector2(-706f, -464f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-659.68f, 12.58f, -456.86f),
                    LandZone = new Vector3(-658.59f, 11.02f, -455.62f),
                    NodeId = 35304,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-664.22f, 12.24f, -459.15f),
                    LandZone = new Vector3(-664.57f, 11.20f, -458.01f),
                    NodeId = 35303,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-670.86f, 3.37f, -399.23f),
                    LandZone = new Vector3(-670.91f, 2.42f, -400.42f),
                    NodeId = 35299,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-677.49f, 3.87f, -396.39f),
                    LandZone = new Vector3(-678.70f, 2.99f, -397.15f),
                    NodeId = 35300,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-744.95f, 3.85f, -515.26f),
                    LandZone = new Vector3(-744.90f, 3.37f, -514.68f),
                    NodeId = 35302,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-744.39f, 4.28f, -522.87f),
                    LandZone = new Vector3(-743.58f, 3.03f, -522.21f),
                    NodeId = 35301,
                },
            },
            [new Vector2(157f, -37f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(142.12f, 3.10f, -76.40f),
                    LandZone = new Vector3(140.62f, 1.77f, -76.15f),
                    NodeId = 35311,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(147.86f, 3.05f, -62.13f),
                    LandZone = new Vector3(146.10f, 1.46f, -62.52f),
                    NodeId = 35312,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(146.25f, 2.29f, -50.91f),
                    LandZone = new Vector3(144.95f, 0.71f, -51.74f),
                    NodeId = 35313,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(143.12f, 1.18f, -44.94f),
                    LandZone = new Vector3(141.52f, -0.66f, -45.04f),
                    NodeId = 35318,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(147.06f, 1.32f, -32.19f),
                    LandZone = new Vector3(145.65f, -0.36f, -31.15f),
                    NodeId = 35316,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(150.83f, 0.90f, -23.07f),
                    LandZone = new Vector3(148.89f, -0.67f, -23.14f),
                    NodeId = 35317,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(150.51f, -0.82f, -15.70f),
                    LandZone = new Vector3(148.79f, -2.59f, -15.85f),
                    NodeId = 35314,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(148.74f, -2.28f, -7.21f),
                    LandZone = new Vector3(147.40f, -3.99f, -7.23f),
                    NodeId = 35315,
                },
            },
            [new Vector2(324f, -41f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(262.53f, 20.20f, -69.19f),
                    LandZone = new Vector3(264.05f, 19.37f, -70.35f),
                    NodeId = 35305,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(267.99f, 16.39f, -58.74f),
                    LandZone = new Vector3(268.25f, 15.60f, -60.07f),
                    NodeId = 35306,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(335.80f, 18.23f, 10.65f),
                    LandZone = new Vector3(334.63f, 17.17f, 9.59f),
                    NodeId = 35310,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(341.32f, 18.72f, 10.44f),
                    LandZone = new Vector3(340.75f, 17.83f, 9.10f),
                    NodeId = 35309,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(351.07f, 36.07f, -86.81f),
                    LandZone = new Vector3(351.39f, 34.39f, -85.82f),
                    NodeId = 35307,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(345.85f, 36.33f, -86.42f),
                    LandZone = new Vector3(345.87f, 34.54f, -85.39f),
                    NodeId = 35308,
                },
            },
            [new Vector2(-256f, -14f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-219.18f, -2.93f, -16.26f),
                    LandZone = new Vector3(-217.95f, -3.71f, -16.01f),
                    NodeId = 35328,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-244.57f, 3.65f, -5.23f),
                    LandZone = new Vector3(-243.78f, 2.08f, -5.93f),
                    NodeId = 35330,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-236.16f, -0.94f, -18.20f),
                    LandZone = new Vector3(-238.21f, -1.90f, -17.59f),
                    NodeId = 35327,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-245.83f, -3.65f, -27.76f),
                    LandZone = new Vector3(-244.86f, -4.71f, -26.35f),
                    NodeId = 35326,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-252.06f, -1.32f, -19.71f),
                    LandZone = new Vector3(-251.48f, -3.26f, -20.72f),
                    NodeId = 35331,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-261.23f, -2.60f, -21.77f),
                    LandZone = new Vector3(-260.17f, -4.12f, -22.74f),
                    NodeId = 35332,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-267.18f, -4.79f, -26.32f),
                    LandZone = new Vector3(-266.01f, -5.27f, -26.33f),
                    NodeId = 35325,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-269.73f, -6.92f, -40.61f),
                    LandZone = new Vector3(-269.25f, -7.82f, -39.31f),
                    NodeId = 35329,
                },
            },
            [new Vector2(-320f, 97f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-290.60f, 20.75f, 143.79f),
                    LandZone = new Vector3(-289.83f, 20.28f, 142.27f),
                    NodeId = 35323,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-299.46f, 21.23f, 141.52f),
                    LandZone = new Vector3(-298.33f, 20.18f, 140.25f),
                    NodeId = 35324,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-375.36f, 16.60f, 76.83f),
                    LandZone = new Vector3(-373.67f, 15.75f, 77.57f),
                    NodeId = 35321,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-374.67f, 16.60f, 67.37f),
                    LandZone = new Vector3(-372.99f, 15.53f, 67.64f),
                    NodeId = 35322,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-290.44f, 16.26f, 56.02f),
                    LandZone = new Vector3(-292.07f, 15.49f, 56.57f),
                    NodeId = 35320,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-283.03f, 16.31f, 57.60f),
                    LandZone = new Vector3(-283.49f, 15.39f, 59.10f),
                    NodeId = 35319,
                },
            },
            [new Vector2(-72f, 525f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-64.80f, 34.13f, 475.99f),
                    LandZone = new Vector3(-63.61f, 33.61f, 477.18f),
                    NodeId = 35337,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-79.20f, 34.77f, 470.77f),
                    LandZone = new Vector3(-78.73f, 33.96f, 471.71f),
                    NodeId = 35338,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-122.09f, 34.94f, 525.76f),
                    LandZone = new Vector3(-121.10f, 33.82f, 525.23f),
                    NodeId = 35335,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-123.45f, 34.98f, 544.07f),
                    LandZone = new Vector3(-122.88f, 33.82f, 542.84f),
                    NodeId = 35336,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-32.31f, 34.31f, 575.29f),
                    LandZone = new Vector3(-33.94f, 33.91f, 574.79f),
                    NodeId = 35333,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-29.12f, 34.71f, 561.02f),
                    LandZone = new Vector3(-30.50f, 34.07f, 561.93f),
                    NodeId = 35334,
                },
            },
            [new Vector2(362f, 438f)] = new()
            {
            },
            [new Vector2(-552f, 651f)] = new()
            {
            },
            [new Vector2(66f, -368f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(41.48f, 12.73f, -359.44f),
                    LandZone = new Vector3(42.59f, 12.50f, -359.94f),
                    NodeId = 35377,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(51.21f, 17.04f, -376.71f),
                    LandZone = new Vector3(52.02f, 15.83f, -373.06f),
                    NodeId = 35376,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(58.82f, 19.40f, -384.76f),
                    LandZone = new Vector3(56.35f, 18.45f, -383.27f),
                    NodeId = 35378,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(77.15f, 24.94f, -394.85f),
                    LandZone = new Vector3(76.03f, 23.98f, -394.61f),
                    NodeId = 35382,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(97.24f, 25.93f, -380.47f),
                    LandZone = new Vector3(97.07f, 25.34f, -381.58f),
                    NodeId = 35375,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(87.94f, 22.49f, -378.19f),
                    LandZone = new Vector3(88.48f, 22.42f, -379.51f),
                    NodeId = 35381,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(82.01f, 18.62f, -366.24f),
                    LandZone = new Vector3(82.51f, 18.64f, -367.87f),
                    NodeId = 35380,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(70.37f, 14.61f, -343.78f),
                    LandZone = new Vector3(70.76f, 13.98f, -344.67f),
                    NodeId = 35379,
                },
            },
            [new Vector2(-88f, -312f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-136.79f, 11.41f, -340.57f),
                    LandZone = new Vector3(-131.96f, 12.43f, -339.04f),
                    NodeId = 35371,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-134.14f, 11.05f, -334.28f),
                    LandZone = new Vector3(-131.96f, 12.43f, -339.04f),
                    NodeId = 35372,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-40.89f, 20.55f, -347.15f),
                    LandZone = new Vector3(-49.06f, 20.65f, -342.78f),
                    NodeId = 35370,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-36.83f, 19.65f, -344.87f),
                    LandZone = new Vector3(-49.06f, 20.65f, -342.78f),
                    NodeId = 35369,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-83.73f, 19.92f, -263.66f),
                    LandZone = new Vector3(-82.56f, 19.42f, -262.30f),
                    NodeId = 35373,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-83.85f, 20.00f, -253.83f),
                    LandZone = new Vector3(-82.56f, 19.42f, -262.30f),
                    NodeId = 35374,
                },
            },
            [new Vector2(724f, -319f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(784.18f, 53.69f, -286.61f),
                    LandZone = new Vector3(777.18f, 52.27f, -290.98f),
                    NodeId = 35388,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(786.91f, 54.43f, -287.83f),
                    LandZone = new Vector3(777.18f, 52.27f, -290.98f),
                    NodeId = 35387,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(720.00f, 50.29f, -370.76f),
                    LandZone = new Vector3(719.94f, 49.52f, -367.47f),
                    NodeId = 35385,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(717.10f, 50.20f, -372.52f),
                    LandZone = new Vector3(719.94f, 49.52f, -367.47f),
                    NodeId = 35386,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(649.06f, 55.04f, -314.26f),
                    LandZone = new Vector3(649.23f, 53.94f, -314.71f),
                    NodeId = 35383,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(646.58f, 54.64f, -315.07f),
                    LandZone = new Vector3(649.23f, 53.94f, -314.71f),
                    NodeId = 35384,
                },
            },
            [new Vector2(-326f, -616f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-302.15f, 21.02f, -684.67f),
                    LandZone = new Vector3(-301.97f, 20.23f, -684.10f),
                    NodeId = 35392,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-328.16f, 14.10f, -676.55f),
                    LandZone = new Vector3(-327.38f, 13.43f, -676.63f),
                    NodeId = 35391,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-350.45f, -1.84f, -572.52f),
                    LandZone = new Vector3(-350.07f, -2.36f, -573.83f),
                    NodeId = 35393,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-354.05f, -3.17f, -565.04f),
                    LandZone = new Vector3(-354.43f, -3.84f, -565.88f),
                    NodeId = 35394,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-297.00f, 12.35f, -580.00f),
                    LandZone = new Vector3(-296.70f, 11.06f, -579.78f),
                    NodeId = 35390,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-292.54f, 12.38f, -575.24f),
                    LandZone = new Vector3(-292.77f, 11.35f, -573.92f),
                    NodeId = 35389,
                },
            },
            [new Vector2(290f, -19f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(314.88f, 16.87f, -48.70f),
                    LandZone = new Vector3(314.98f, 15.19f, -48.24f),
                    NodeId = 35407,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(308.83f, 16.89f, -49.10f),
                    LandZone = new Vector3(309.22f, 15.44f, -48.08f),
                    NodeId = 35406,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(299.04f, 20.22f, -58.99f),
                    LandZone = new Vector3(299.30f, 19.32f, -58.06f),
                    NodeId = 35405,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(282.76f, 10.87f, -35.26f),
                    LandZone = new Vector3(283.93f, 10.18f, -35.23f),
                    NodeId = 35404,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(240.88f, 16.49f, -27.37f),
                    LandZone = new Vector3(242.16f, 15.10f, -27.66f),
                    NodeId = 35401,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(301.19f, 8.20f, 8.86f),
                    LandZone = new Vector3(301.80f, 7.78f, 7.06f),
                    NodeId = 35403,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(321.27f, 17.43f, 13.76f),
                    LandZone = new Vector3(321.67f, 16.02f, 12.79f),
                    NodeId = 35402,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(324.45f, 17.31f, 18.39f),
                    LandZone = new Vector3(325.65f, 16.67f, 17.94f),
                    NodeId = 35408,
                },
            },
            [new Vector2(331f, 32f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(348.23f, 17.91f, 77.35f),
                    LandZone = new Vector3(348.52f, 17.42f, 76.97f),
                    NodeId = 35399,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(353.68f, 18.39f, 87.36f),
                    LandZone = new Vector3(353.76f, 17.46f, 87.20f),
                    NodeId = 35400,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(267.24f, 1.82f, 23.99f),
                    LandZone = new Vector3(267.51f, 0.74f, 23.86f),
                    NodeId = 35395,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(268.73f, 2.14f, 18.46f),
                    LandZone = new Vector3(268.45f, 1.26f, 18.02f),
                    NodeId = 35396,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(307.85f, 17.38f, -48.76f),
                    LandZone = new Vector3(307.74f, 15.89f, -48.83f),
                    NodeId = 35398,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(314.60f, 16.92f, -48.69f),
                    LandZone = new Vector3(314.59f, 15.56f, -48.89f),
                    NodeId = 35397,
                },
            },
            [new Vector2(-237f, 187f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-224.29f, 20.86f, 179.91f),
                    LandZone = new Vector3(-223.21f, 20.31f, 177.64f),
                    NodeId = 35421,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-218.14f, 20.72f, 184.68f),
                    LandZone = new Vector3(-217.72f, 20.31f, 183.81f),
                    NodeId = 35420,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-215.21f, 20.72f, 197.59f),
                    LandZone = new Vector3(-215.35f, 20.31f, 196.97f),
                    NodeId = 35418,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-208.33f, 20.78f, 200.30f),
                    LandZone = new Vector3(-209.62f, 20.31f, 199.51f),
                    NodeId = 35417,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-243.34f, 20.85f, 211.78f),
                    LandZone = new Vector3(-242.70f, 20.31f, 211.85f),
                    NodeId = 35416,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-245.06f, 21.01f, 215.41f),
                    LandZone = new Vector3(-245.53f, 20.31f, 214.48f),
                    NodeId = 35415,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-251.37f, 21.09f, 161.46f),
                    LandZone = new Vector3(-250.98f, 20.31f, 161.63f),
                    NodeId = 35419,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-255.78f, 20.88f, 160.89f),
                    LandZone = new Vector3(-255.39f, 20.31f, 160.04f),
                    NodeId = 35422,
                },
            },
            [new Vector2(-57f, 116f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-74.00f, 2.70f, 167.99f),
                    LandZone = new Vector3(-72.76f, 1.70f, 165.23f),
                    NodeId = 35409,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-71.63f, 2.71f, 176.74f),
                    LandZone = new Vector3(-70.91f, 1.76f, 174.97f),
                    NodeId = 35410,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-122.46f, 2.85f, 120.67f),
                    LandZone = new Vector3(-121.85f, 1.96f, 121.72f),
                    NodeId = 35412,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-129.57f, 2.56f, 116.94f),
                    LandZone = new Vector3(-128.01f, 1.84f, 117.71f),
                    NodeId = 35411,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-16.47f, -9.33f, 72.52f),
                    LandZone = new Vector3(-17.87f, -10.00f, 73.21f),
                    NodeId = 35413,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-14.56f, -9.34f, 79.26f),
                    LandZone = new Vector3(-14.53f, -10.10f, 76.90f),
                    NodeId = 35414,
                },
            },
            [new Vector2(49f, 636f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(90.81f, 12.54f, 615.77f),
                    LandZone = new Vector3(87.49f, 12.00f, 617.15f),
                    NodeId = 35423,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(86.33f, 12.51f, 612.67f),
                    LandZone = new Vector3(87.49f, 12.00f, 617.15f),
                    NodeId = 35424,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(22.84f, 27.59f, 695.38f),
                    LandZone = new Vector3(23.84f, 26.98f, 691.93f),
                    NodeId = 35427,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(15.62f, 27.44f, 692.80f),
                    LandZone = new Vector3(23.84f, 26.98f, 691.93f),
                    NodeId = 35428,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-1.78f, 27.72f, 618.93f),
                    LandZone = new Vector3(1.38f, 26.92f, 620.88f),
                    NodeId = 35426,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(1.48f, 27.69f, 616.74f),
                    LandZone = new Vector3(1.38f, 26.92f, 620.88f),
                    NodeId = 35425,
                },
            },
            [new Vector2(648f, 434f)] = new()
            {
            },
            [new Vector2(-333f, 560f)] = new()
            {
            },
            [new Vector2(-481f, -533f)] = new()
            {
            },
            [new Vector2(129f, -749f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(133.10f, 133.26f, -727.37f),
                    LandZone = new Vector3(134.94f, 132.55f, -727.40f),
                    NodeId = 35362,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(134.45f, 133.09f, -733.57f),
                    LandZone = new Vector3(135.68f, 132.44f, -733.32f),
                    NodeId = 35361,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(136.71f, 133.03f, -746.63f),
                    LandZone = new Vector3(136.06f, 132.38f, -744.94f),
                    NodeId = 35358,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(125.69f, 132.76f, -753.30f),
                    LandZone = new Vector3(126.82f, 132.15f, -752.13f),
                    NodeId = 35357,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(131.80f, 132.92f, -760.74f),
                    LandZone = new Vector3(130.97f, 132.23f, -759.88f),
                    NodeId = 35360,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(128.40f, 132.86f, -767.84f),
                    LandZone = new Vector3(128.62f, 132.21f, -766.95f),
                    NodeId = 35359,
                },
            },
            [new Vector2(184f, 624f)] = new()
            {
            },
            [new Vector2(143f, -182f)] = new()
            {
            },
            [new Vector2(-561f, -658f)] = new()
            {
            },
            [new Vector2(152f, 287f)] = new()
            {
            },
        },
    };



    public class FishingTools
    {
        public List<string> FishingPreset = new List<string>();
        public HashSet<uint> Baits = new HashSet<uint>();
    }

    public static Dictionary<uint, FishingTools> FishingPreset = new()
    {
        #region D Rank

        [451] = new FishingTools
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1WS2/jOAz+K4XONuCH5NctzaadAmmnmHSxh2IOjE0nQhwrI8vTyRT57wvZVmKnSTsdFIs9zE2gyI8fKYrkMxnVSoyhUtU4X5DkmUxKmBc4KgqSKFmjRfTllJd4uMzM1U1GEi+KLXIvuZBcbUniWuSmmvxIizrD7CDW+rsW61aIdKnBmoOnTw1OEFnkevOwlFgtRZGRxHWcAfLr0A1GHA4snDfJjJf1+kxg1HXoG4wMiCgKTJWJhLqO21fz3mYhZMahMACBSwcAtFO74tVyssWq54gdMWRswDAwOYcVzpY8V5fAG55aUBnBTEG6qkjCuiwG0UvcPmrcod6D4lim2OMTHNsFw4x5xlTynzgG1VaC8Xps7R3l2++sH5ZQcFhVV/BdSA0wEJhwfGso/4Kp+I6SJK5OkvFJBx5Mwi754hrWTWSjclGgrAyqfs2MJH7o0Bd0B1DRbmeRyQ8lYfCz9jWmM/8gZk+wuSlVzRUX5TXw0uTDdi0yrSXeYlXBAklCiEXuGk7kTpRIOoTtBkmiE3MCbyoq9dt49xIrPM2Q2OTMfeuxuT/wmW0wVRKKcS0lluqDojxC/bBYT7J9EfFJ743WlZApNt/qCTbmsRthpqXNR2ExDa2usmZKbPTP5uVipnDTtNBDlF31jeTHBNeHa9j+XfJvNWpc4gQsyDygNpvHmU1p5tgxQG6nUYoUAiePPZfsLDLllfqcax8VSR6fG286gH0bYDGNznO8FaK8uISi0Fh3Qq6h+CTESlubhvIPwurwY/RthYN0dqI2RuqGuiMZ45mSoly8x9zxe+ZTXGCZgdy+G+EvUc+LPfeBhhfEe4UDv7MqAw4ntB4k35zzFDLP36uc8zVQesVbp6dLdJQrlGOoF0s15Ws9Rdz24rh2m32hlu2Y0odeP247J4tfDtZXZqQe7qa/mEr5gt9qLjGbKVC1Hl16ezgun1+rkl8uhj9v/p++ea8zBeCmIUapHQMNbOp4aM/dMLIj1w/mcUZZECPZfTWtqdswH/eCtjs9PpN+m3p3Hz2bkZsMS8VTKHQaNHyrMFqLuhyoNb3xeGvwhytbpD3VMocUZ4VuOPumyt7YjtjOIv+b5fowwX4/30+w0ZKxTmOTwf4k6+aXPrbig9qpCu1VU8giABbkNmM52NSPqB3TOdhuRCFklNIoC5tqanE7io+UuV8vpnWZLi8ma5QLLNPtcICGaRwCi3N7HrLIplmIduzGno1+4LOMprFLfbL7F1DkJiNzDQAA"
            },
            Baits = new HashSet<uint>()
        },
        [452] = new FishingTools
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACs1VyW7jMAz9lYJnG7BjSV5uaZAWBdJOMemcih4Um46FOpYryV2myL8P5NiTOMsEKHqYm0CRj49PJPUJ48bICddGT/IlJJ8wrfiixHFZQmJUgw7Yy5mocHuZ9Vc3GSSjKHbgXgmphPmAxHfgRk/f07LJMNuarf96g3UrZVpYsPYwsqcWh0UOXNcPhUJdyDKDxPe8AfK/oVuMOBxEeGfJTIpm1TMgvkfOUOijZFlianYC/V230fm0UmWClyck9UeMDUQlXdiV0MX0A/VOYrrHmNIBY9aLzp9xXojcXHLR8rYG3RvmhqfPGhLayciiQ9xd1LhDvedGYJXiDh+2H8eGCo76UCV+44SbTSsc6ysWHYCN9p4j6MAeCl4K/qyv+KtUFm9g6KsLnKH9J6byFRUkvtXsBAUySNjLeSmW13zV1j2uliUq3Sexb59BEoQeOWA/gIrWawem70bxweD9JWDf5UHO33h9U5lGGCGray6qXmrXd2DWKLxFrfkSIQFw4K7lBHeyQugQPmqExOp0BG8mtfky3r1CjccZggsn7jcZ2/stn3mNqVG8nDRKYWW+qco91G+r9Sjbg4qPZm+9rqRKsR26N173j90aM2ttx4jGJHS6zpobWdu5F9VybrBuN+y2yq77xup7ituFa9n+qsRLgxYXqMc9jj5zfQyIS7IodKOYEjeOWB57NOIhD2DtwExo8yO3OTQkj0+9oVv7W4MtCpLHT9gcug1CWcxOF3CLJa/SQpaCD+qwi9kKNc4NqglvloWZiZXddPbTyLAyIuWlnVybaOMwXsmmGri1yu9PbTBcqJHN1Kicpzgv7QMeXV6ExvTM7qJrB/6br3DbUF9uo/kbr61lYlVtBd1trK6d7HFj3roda/CdtguoH1MSLVxklLskCIgbpTFzozjnfkTRj1gK66c+XUfxkdDR08WMqyVejF8abkR6YWdSrLDSw7728yBE3ycuC3zqkiyP3cWChG4cxKM4CkLEjMH6D56DAd8pCQAA"
            } 
        },
        [453] = new FishingTools
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACtVXWW/bOBD+KwGfJUAHRR1vrtdJDbjZoMoiD0EfKGlkE5FFlaSaZgP/9wV12JKP2CmCRfsmDznffDOcy69oUis+pVLJab5E0SualTQpYFIUKFKiBgPpwwUrYXeY9UfzDEVOEBroTjAumHpBkW2guZz9TIs6g2wn1vc3LdYXztOVBms+HP3V4JDAQDfV/UqAXPEiQ5FtWSPkt6EbjNAfaVhnyUxX9bpngG0Ln6HQa/GigFSdiAi2LXuo5ZxnwUXGaHECz3YIGcUYd2rXTK5mLyAHDnh7DnjeyAHSvwF9gnjFcvWJssYNLZC9IFY0fZIo8rqokuAQd4gadqh3VDEoUxjwIft6ZBxQp1cV7F+YUtVmxrE0I8EBmLP3Om4Hdr+iBaNP8pr+4ELjjQS9d64xln+FlP8AgSJbx+wEBTwy2IfzE1ve0HXj96RcFiBkb0S/fYYi17fwAfsRVLDZGGj2Uwk6qsMtAf0u9zx+ptW8VDVTjJc3lJV9qE3bQItawBeQki4BRQgZ6LbhhG55CahDeKkARTpOR/AWXKpfxrsTIOE4Q2SiE+etxeZ8xyeuIFWCFtNaCCjVB3m5h/phvh5le+DxUevNrWsuUmiK7plW/WM3wkxLmzLyQuwbXWbFile67lm5jBVUTcPdedll30R8jHNDuIbtPyX7XoPGRbYT+mEOxIQMchNnATETQqmZ5zS1Ej/zIAjRxkALJtXfubYhUfT42ljTDmybROvdKY5TLtdcra7u6opquFsu1rT4zPmTBug7zgPQ5reWS1Db2slpIaEv5u5wGOhO1HqPbV93sh4zVoKXg1I8r265A/UFLKHMqHj5AF4N8F+8Topzno4UHRJu9XbenLxyCeMjyveCVe/k5XuOu9U8xWx06f3cOnVdL5NcgZjSerlSC7bWA89uD/YLqVl1atFOVP0xmBVtG/fCwxXhjfGu95K+2fU5+xW+10xAFiuqaj1l9eJzYSJflq+XKb87W/+HpLxY83fJ1Xfo/rkpPOj6JHQTF/LAzP3QM7FFAjOAIDHtxA3tAFPL9gFtvvVtv9v1H7eCtvM/vqLxCPD1xnx6BCS0UFczKEbDyn4rNPMMSsVSWuh4aDvthcma1+XoWjOA9lczd7w1B9pSLXKaQlzoJr2dXN6ZjdTbGOi3+b+zWxN+eTmIn2mlJVMdxiaCw3WhWxL0ZyveXTuWqoO0wiE4TmgFpuc7nolzTM0QAs90KUlyy3bClJAmrVrcjuIj9txvVw8gFYjy6oEqEFfzUupdi/FyvK5kWZBjz8VmRiA0sZdgM0l8MJ2U4twlYDkBQZv/APBzqN0PDwAA"
            }
        },
        [454] = new FishingTools
        {
            FishingPreset = new()
            {
                "AH4_H4sIAAAAAAAACu1WS2+jMBD+K5XPIEEwz1uaTbqV0m60SbWHqgcHhmCFYGqbttkq/31lwEnIo49Vtae9wcz4m2/G489+Rf1KsgERUgzSBYpe0bAg8xz6eY4iySswkHKOaQE7Z6Jd1wmKekFooAmnjFO5RpFtoGsxfInzKoFkZ1bxmwbrhrE4U2D1R0991TheYKCrcpZxEBnLExTZltVBfhu6xgj9zgrrXTKDrFqdKQzbFn6HkQZheQ6x1JVg27L3w3rvs2A8oSTXAJ6NOwC4DRtRkQ3XIPYSuQcMXbfD0NM9J0uYZjSVl4TWPJVBaMNUkngpUOS2XfSCY9x91LBFnRBJoYhhj493uM7rdqynl3L6GwZENpNwaqy84Aisd9B+pwWbZSSnZClG5Ilxhdcx6Ooco2v/CTF7Ao4iW/XsDAXcSajbeUkXV2RV190vFjlwoZOovU5Q5PgWPmLfgQo2GwMNXyQnnXO3JaD2Zcamz6S8LmRFJWXFFaGFbrVpG2hccbgBIcgCUISQgW5rTuiWFYBahHUJKFJ9OoE3ZkL+Nd6Eg4DTDJGJzvibjLV/x2daQiw5yQcV51DIL6ryAPXLaj3J9qjik9nrqBHjMdSH7pmUerNrY6Ks9TFyQ+wb7WRNJSvVuafFYiqhrAV2V2U7fX3+NcXtw9Vs7wr6WIHCRcTziOv5junPA8fEcWiZ8zl4ZooDSKzQn3tOD20MNKZC/khVDoGi+9c6mypgKxJNdec4DphYMZldTKqSKLhbxlck/87YUgFoxfkFZLk7NMoroNPR1tSUiW1fSZZePJWcFYvPLLecveVjWECREL7+NMI3Vs3zLfdORM8LtwE7fmdDOhxORM04Lc9l8t2esw05l6sT9Ea2Nk5NaT+VwAekWmRyTFfqmrEbx+H41g+Kijf3mPrYU+hGPN3w+OZ94xJVt7+WGD0pP+GxohySqSSyUnebel4cjs/HpuTDw/B/z//pnt/txClxHYC565lJgmMT23FszsMYTIKDNPEtwEGI0eZBq1P7BL3fGhqBUv+NHLZidI9d/HDRf6yIpPHFiLFEyCpNRVcZXce3wtDxTPDBMbELthmAG5i+5Qahb4ep56Ro8wdsUj8tagsAAA=="
            }
        },
        [455] = new FishingTools
        {
            FishingPreset = new()
            {
                "AH4_H4sIAAAAAAAACu1Xy27bOhD9FYNrERBlinrsHN8kDeCkQZ2LLoIuKGlkE5FFl6TSpoH//YJ62JZjJXGRRYHenTCcOTwzPByOntGkMnLKtdHTfIHiZ3Re8qSASVGg2KgKHGQXZ6KE3WLWLV1lKPbCyEG3SkglzBOKiYOu9PnPtKgyyHZm679psK6lTJcWrP7w7FeNw0IHXa7vlgr0UhYZionr9pBfh64xoqAX4b5JZrqsVgOJUeLSNxh1ILIoIDXDOGQ/ynublFSZ4MUAHiO0h0fbqAuhl+dPoLuCUuL6B/x9v8efdSfCH2C+FLk546LOwhp0Z5gbnj5oFPttjVn4EncfNWpRb7kRUKawx4cdxrF+Pb0uVIlfMOWm0ckx0bHwBZh3cDjjFuxuyQvBH/QFf5TK4vUMXXZjp2//Aql8BIViYms2QIH2NuzKeSYWl3xV5z0pFwUo3W1ijz5D8Thw6Qv2Pahws3HQ+U+jeO9WbgnYc7mT8x98fVWaShghy0suyq7UmDhoVim4Bq35AlCMkINuak7oRpaAWoSnNaDY1ukI3kxq89t4two0HGeIMBpYb3as13d85mtIjeLFtFIKSvNBWR6gfliuR9m+yPjo7rXXhVQp1JfuB193h10bM2utr5Ef0dBplTU3cm3vvSgXcwPruv3usmzVN1Efk9w+XM3231J8r8DiIkKTPKeMY0qDANM0yHFCPMDEJVEYuAxcH9DGQTOhzefc7qFRfP9c72YT2DaJJrshjtdSlqMzXhQW60aqFS8+Sflgo7t28xX4w+7G2FUNvXK2piZHSgLbr7rguVGyXJwS7o73wmewgDLj6ulkhH9klRRb7j0Pj0Vbhx2/1mXbF3Je6EPs/cgetfcH3ymxHuIV+N5463Iis17s6dzacCv/SW5ATXm1WJqZWNn3izQLh/einmMq1TyQ9mOv9Tdd2Y9ePvivPNZ26Oh6V6fCL/C9EgqyueGmso+mnWoOpfmeFAeF+b7gYVmeEH9UlO9W3J+h1RNk+HdKeK+Jh6HnjnkYYsgJYErzAEfczTELKAkzjwU0jNDmW9fF20H+fmtoGvn9M+p3dBaNhzv6RBueVno04ysojVTVqvcGkddKdJVBaUTKC1sXu1/jMFnJquy51e/K4cQ17g/Dod2pUjlPYV7YZn18mPcj/42509846I/5x9kNA789Athga5naqtYF3R8K2lHAfjbmndsxBe+pzXU9PwpSin02DjClSYhDyD3sURZ5bsgZhDnaOIdqCtxgOIFZVXI1+gQrIbNK/6+kv0NJOaM0zL0AJ4RRTHnm4SjPCQ48whI3JB7Jk7pvNbgtxXvq+99GXwHWolyMbqUsRuepLOTC6mA0r9QjPPWHXOp6fsL8CKchpJj6xMVJ4CeYkjQgLE8SD1K0+Q9F3lgEUxEAAA=="
            }
        },
        [456] = new FishingTools
        {
            FishingPreset = new()
            {
                "AH4_H4sIAAAAAAAACq2US0/jMBDHvwqacyIlqfO8haogpMKiLXtCHAZn0liEOGs7PBb1u6+cJrQpZZFW3JwZz2/+83DeIO+MnKM2el6uIXuDRYP3NeV1DZlRHTlgnUvR0M5ZjK6LArIgSR24VkIqYV4h8x240IsXXncFFTuzvb/Zsi6l5JWF9YfAnnpOlDhw3t5UinQl6wIy3/Mm5H+je0YaTyK8L8XMq+5xVMB8j30hYYySdU3c7AX6+9eCr9NKVQisP2mpH0TRpKlsCDsTulq8kt5LHB4oDsOJ4mhsOj7QqhKlOUXR67YGPRpWBvmDhiwc2hglH7n71HSgXqMR1PDPVoP5XnSIiaYNDUaSEn9ojma7GaOIw+jgYByzIfqmwlrggz7DJ6ksYGIYq5s5U/tP4vKJFGS+7dmx1Y4SuxF7Ccd2nor1OT72defNuialxyR29gVks9hjH9RPUMlm48DixSicPLx3AXYuN3L1jO1FYzphhGzOUTRje1zfgWWn6JK0xjVBBuDAVa8JrmRDMBBeW4LM9ukIbym1+W/etSJNxxWCC5/4txl7/07PqiVuFNbzTilqzDdVeUD9tlqPqv1Q8dHs/a0zqTj1j+4Z23HYvbGw1v7dhClLnGGzVka29t2LZr0y1PZ/2F2Vw/bl6nuKy/dwvdpfjfjdkeVCQkGByGK3LIi7zPfIRc65W5TePc4CVnpeABsHlkKbH6XNoSG7vRsNw29/Z7BFbb+3CgaNtyyM7k5yraUyVJzkNa/oUXCsTy7RkBJY66kuxLjgjKHre7PQZUFMbpqWsRvxMKYojsKUlbD5C4vnvqjpBgAA"
            }
        },

        #endregion

        #region C Rank

        [457] = new FishingTools
        {
            FishingPreset = new()
            {
                "AH4_H4sIAAAAAAAACq2VTW+jMBCG/0o1Z5AwAfNxS6O0qpR2q033VPXgmCFYpZi1TT+2yn9f2YEmpMlWWvUGM/Yz7zsewztMOyNnTBs9K9eQv8O8Yasap3UNuVEdemCTC9HgLlkMqasC8jDNPLhVQiph3iAnHlzp+SuvuwKLXdiu32xZ11LyysLcQ2ifHIemHly2d5VCXcm6gJwEwYj8b7RjZMloR/ClmFnVPZ0wFpEg+kLRAJF1jdwMTiISkP1l4dcqpCoEq08IISGlox5H/bYLoav5G+q9wvGB4jgeKabDGbBHXFaiNOdMON02oIfA0jD+qCGP+67S9DN3n5r11FtmBDb81KREJKCHGDpuaDiQlPiDM2a2gzKIONwdHhzHpN99V7FasEd9wZ6lsoBRYHA38cbxn8jlMyrIie3ZsUmnqZ2IvYJDO8/F+pI9Od/TZl2j0kMRe/YF5JMkiD6pH6HSzcaD+atRbHQPPwTYc7mTyxfWXjWmE0bI5pKJZmiPTzxYdAqvUWu2RsgBPLhxmuBGNgg94a1FyG2fjvAWUpv/5t0q1HhcIfhwIr+t6PI7PcsWuVGsnnVKYWO+yeUB9du8HlX7yfHR6m7VhVQc3aV7Ye1w2C5Y2Ki7N3EWZV4/WUsjW3vvRbNeGmzdB3fnsp++qfoec9M9nFP7qxG/O7RcyFZpUWLMfeQ48SNGAz9LS+6XhFPELKF0xWDjwUJo86O0NTTk9w9DoP8L7ALW1PZ9q6DXeB/FycPZuVifuQUjCUmxymJWcp+SlPlREJd+ltGJzwMSJiQiCeMFbP4C1oBtKeMGAAA="
            }
        },
        [458] = new FishingTools 
        {
            FishingPreset = new()
            {
                "AH4_H4sIAAAAAAAACq1Uy27bMBD8lWDPEiDJ1PPmGE5gwEmDOj0FOdDUxiKikCpJ5dHC/16QFmvLcRqgyI3a5c7OzC71G6a9kTOqjZ49bKD6DXNB1y1O2xYqo3oMwCaXXOA+WfvUooYqKcoAbhSXips3qOIAFnr+ytq+xnoftve3O6wrKVljwdwhsSeHkxUBXHa3jULdyLaGKo6iEfK/oR1GmY8qok/JzJr+6QNhJI7IJ4w8iGxbZMYrIXEUH15LPmchVc1p+wGROMmykcdkKLvgupm/oT5onB4xTtMR48zPgD7iquEP5pxyx9sGtA+sDGWPGqp0cDUr3uMeopYD6g01HAXDAz7ZcV02djDxpYr/whk1u83wXY+rkyP/J0P1bUNbTh/1BX2WygKMAl7OJBjHvyOTz6igiq1Jp1Y7K+wKHDT0/p3zzSV9ckKnYtOi0r6JHXYN1SSPyDv2I6hiuw1g/moUHT28vwTsIG7l6oV2C2F6brgUl5QLb08YB7DsFV6h1nSDUAEEcO04wbUUCAPCW4dQWZ9O4C2lNv+Nd6NQ42mGEMIH+V1Hl9/zWXXIjKLtrFcKhfkilUeoX6b1JNt3ik92d7cupGLoXtkL7fywXbC2Ufdu0pKUwbBZKyM7+9C52KwMdu4Pu1c5bN9UfY246QGcY/tD8J89WlyICMNJktchYZMiJDUpwzWp8zBLWUlKlqU5EtgGsOTafHuwPTRUd/c+MPz29wErave9YzBwvCNpcX82F89cSfGEwtD2bCG09ZNLcUQpy5KkjpIwTdk6JJTmIS3qJIyifJ2QtCjjNIXtH76wCNnkBgAA"
            }
        },
        [459] = new FishingTools 
        {
            FishingPreset = new()
            {
                "AH4_H4sIAAAAAAAACs1VTW/bMAz9KwXPNuBvW76lWVoUSLtg7k5FD4pMx0IdK5PkfqzIfx/k2IudJgtQ9LCbQJGPj08k9Q6TRospVVpNixWk7zCr6bLCSVVBqmWDFpjLOa9xf5n3Vzc5pF5CLFhILiTXb5C6Ftyo2Surmhzzvdn4b3dYt0Kw0oC1B8+cWpwoseB6c19KVKWockhdxxkh/xu6xSDxKMI5S2ZaNuueQeA6wRkKfZSoKmR6EOgO3bzzaYXMOa1OSOp6UTQSNejCrrgqZ2+oBonDA8ZhOGIc9aLTJ8xKXuhLylvexqB6Q6Ype1KQhp2MUfIRd4hKOtQF1Rxrdqo1AteJDmGisaBejyT5b5xSveuMnsRhtHfwHH4XfV/SitMndUWfhTQAI0NfnW+N7T+QiWeUkLpGs2OtHSWmIwYJezkv+eqartu6J/WqQqn6JObtc0j92Ak+sB9BJdutBbNXLelo8P4SMO9yL7IXurmpdcM1F/U15XUvj+1aMG8k3qJSdIWQAlhw13KCO1EjdAhvG4TU6HQEby6U/jTeQqLC4wzBhhP3u4zt/Z5PtkGmJa2mjZRY6y+q8gD1y2o9yvZDxUezt15XQjJsh+6FbvrHbo25sbZzE5KAWF1nZVpszNzzepVp3LQbdl9l130T+TXFDeFatj9r/qtBgwtB7niOF3u27zvEDkhR2Mug8O04cl0nIiSkSQhbC+Zc6e+FyaEgfXjsDd3a3xtMUZA+vMPu0G20MPa80wXMm5rKiwnqEmUlGjWuxqxnI9ek0CintFmVes7XZt+ZryPHWnNGKzO/Jt3OYbIWTT1wO7bGQnI4yv54yyYmcSMLyjCrzKv2xZDwzAYLtxb8Nx/ivq0+3UzZC90Yy9So2go6bK+uqcxxZ967HWvzQfNRwljuF77tIFnawTIkNmHL2Ma4cAjmSeh5EWwf+3QdxYcgJI8XC1FR873kF9/M3znCdf3YcZfMscmycO3AQ2ZTc0K3YAGhJIkdCts/1YWfqiYJAAA="
            }
        },
        [460] = new FishingTools 
        {
            FishingPreset = new()
            {
                "AH4_H4sIAAAAAAAACu1XTW/bOBD9KwbPJkBJpCT65nqTbAA3G1RZ9BD0QEkjmzAtuhSVNhv4vy+oD9ty7KQtclhsczOGM2/eDJ+G4yc0ra2eicpWs2KBJk/oohSpgqlSaGJNDWPkDueyhP1h3h9d52jix3yMbo3URtpHNPHG6Lq6+J6pOod8b3b+2xbro9bZ0oE1P3z3q8EJ4zG62twtDVRLrXI08QgZIL8M3WDwaBBBXiUzW9brM4VRj9BXGPUgWinIbF8J9Yh36Oa/zkKbXAp1hojnh+Ggx7QLu5TV8uIRqoPE7IgxYwPGYX8HYgXJUhb2g5ANb2eoekNiRbaq0IR1XQ3j57iHqLxDvRVWQpnBAZ/wOC4cdtDvQ438B2bCtsrosx5H+0f9D7rou6VQUqyqS/GgjQMYGPpygvHQ/gky/QAGTTzXpFPSDmMngYOEff8+yMWVWDeFTsuFAlP1Sdxl52gSRIQ+Yz+AirfbMbr4bo0YfHg7Au4i7nTyTWyuS1tLK3V5JWTZtwd7YzSvDXyEqhILQBOExuim4YRudAmoQ3jcAJq4Pp3Am+vK/jLerYEKTjNEGJ05bzM253s+yQYya4Sa1cZAad+oyiPUN6v1JNtnFZ/M3nhdapNB85V9E5v+shtj7qzNd8M45eNOWYnVG/ehy3KRWNg0E3ZfZae+qXmb4g7hGrZ/l/JrDQ4XZX4UMJ6HGLI4wJSFAeZ5FGAKkEWe8H0CBdqO0VxW9q/C5ajQ5P6pyeYK2BHk/DzDqVKjNvSY5o02a6H+1HrlgPpR8xnEav/xuNMKBp3tTC0O9SI3q/rgxBpdLn4mnAQH4XNYQJkL8/jTCH/oOlU77gMPP+Q7hz2/sy4DDie87ozcnMsUMT/YuZzLNXB6IVvn59Q6LSyYmagXSzuXa/e+eO3BsYybzaI27QPmfhxM6naIMn78BLtRffY1dWtAP2p6pXyCr7U0kCdW2No9am7POJbPj6nkh8Xwfudveucv7nHb4ZDKCASZIJjRHDD1WI7TwhO4EB7nURiS1I/R9ks/pbpd9H5naAfV/RM6nFiURcQ/P7MSK8zoVkGmB0PLe6k11zmUVmZCuX64PK3DdK3rcuDWvAbHm0Qw3Opil6k2hcggUW7ynN5nGWev7FNsO0b/mfV8/8j98tPmgp1l5rraNPTwseueOPezNe/dTin3QGUiCoFEoYcpcIppFBKcBmGI4zwVcZB5jPgUbcfPVOTT8wXcynK11roczWS2VDJ/19LvoSWacipCiLAXpRRTiAOckpzglDDwGWSU+v5JLbHzBczrUphRotWwjHcV/W9VlKY8ogUNcAE8xpSTDHMoCE4z4kVRWpDAg5MqCl9416R6ADNKbG0WoMt3Kf0eUooCj9GQAy5yn2FKfI5FzAX2RZiywksjylizQrW4HcV7GpIvo0TXdgmisqPPoNToItNKL5wSRkltHuBx+I8yZpDmMWWYRoGPKS0o5pxGOBMQFoIEnHkF2v4LFSMleKMUAAA="
            }
        },
        [461] = new FishingTools 
        {
            FishingPreset = new()
            {
                "AH4_H4sIAAAAAAAACq2V226jMBCGX6Waa5CAcL5Lo7SKlHarpXtV9cIxQ7BKbdY2PWyVd1+Z4CbksJVWvYMZ+5v/H4/hA6adFjOitJpVa8g/YM7JqsFp00CuZYcOmOSScdwlS5talJAHaebAnWRCMv0Oue/AQs3faNOVWO7CZv1my7oRgtYG1j8E5qnnxKkD1+19LVHVoikh9z1vRP43umdkyWiH96WYWd09nzEW+l74hSILEU2DVFsnoe/5+8uCr1UIWTLSnBHiB3E86nE4bLtiqp6/o9orHB0ojqKR4tieAXnComaVviSs120CygYKTeiTgjwauhqnx9x9ajZQ74hmyOm5SQl9Lz7ExOOGBpYk2R+cEb0dFCvicHdwcByTYfd9TRpGntQVeRHSAEYB627ijOM/kYoXlJD7pmenJj1OzUTsFbTtvGTra/Lc+57ydYNS2SLm7EvIJ4kXHqkfodLNxoH5m5ZkdA8/BZhzuRfFK2kXXHdMM8GvCeO2Pa7vwLKTeINKkTVCDuDAba8JbgVHGAjvLUJu+nSCtxRK/zfvTqLC0wrBhTP5bcU+v9NTtEi1JM2skxK5/iaXB9Rv83pS7ZHjk9X7VVdCUuwv3Stp7WH3wdJE+3sTZWYit5NVaNGae8/4utDY9h/cncth+qbye8xN93C92l+c/e7QcGFSeVXmU88lXhy7IU0SNw2j2M3CsEySSUnJagUbB5ZM6R+VqaEgf3i0geEvsAsYU9v3rYJB40MY+48XJnlRkI7ixYKvJZYMuVZjQbSqsjRMYtdfpUaQ57mkXFEXJ9kKsywKkqCCzV+y46n58QYAAA=="
            }
        },
        [462] = new FishingTools 
        {
            FishingPreset = new()
            {
                "AH4_H4sIAAAAAAAACq2V227iMBCGX6Wa60TK0TncUQRVJdqtNt2rqhcmGYjVNE5tp4etePeVTbwQCltpxV2YyXzz/+Mx+YRJr/iUSiWnqzXknzBr6bLBSdNArkSPDujkgrW4S1Y2dV1BHqSZA3eCccHUB+S+A9dy9l42fYXVLqzf32xZN5yXtYaZh0A/GQ5JHbjq7muBsuZNBbnveSPyv9GGkSWjCu9bMdO6f7YKIt+LvpFgq3jTYKlOTCTyPX+/KvheBRcVo80Jnh8QMppxNJTNmaxnHyj3DMQHBuJ4ZIDYM6BPWNRspS4pMzZ0QNpAoWj5JCGPh6mS9Ct3n5oN1DuqGLblqU2JfI8cYsh4voElCfYbp1RtF8WKOKwODk4nHKrva9ow+iTn9JULDRgFrLvQGcd/YslfUUDu65kd23SS6gXZa2jHecnWV/TZ+J606waFtE302VeQh4kXfVE/QqWbjQOzdyXo6B7+FaDP5Z4Xb7S7blXPFOPtFWWtHY/rO7DoBd6glHSNkAM4cGs0wS1vEQbCR4eQ6zkd4S24VP/NuxMo8bhCcOFEftvR5Hd6ig5LJWgz7YXAVp3J5QH1bF6Pqv3i+Gh389acixLNpXujnT1sE6x01NybOIsyZ9isQvFO33vWrguFnfnD3bkctm8izmNusoczan+17KVHzQVMcOl7fuDSNArdKExDl8ar0iVelSUr4q1ISmDjwIJJ9WOle0jIHx5tYPgK7ALa1Pb3VsGg8SEiwePF5KWnipUXBX3uGpRjJcuAkNJLEpemaexGAfHdNIsjN4y9qkwSEnppAps/ch9WWuoGAAA="
            }
        },
        [463] = new FishingTools 
        {
            FishingPreset = new()
            {
                "AH4_H4sIAAAAAAAACu1WTW/jNhD9KwbPIqAPSiJ187pJGsBJgzpFD0EOFDm0iciiS1GbTQP/9wVlq7b8sQEWCZAWvRFDzpv3ho8jvaJx68yEN66ZqDkqXtFFzcsKxlWFCmdbCJDfnOoadpuy37qWqIgpC9Cd1cZq94KKKEDXzcU3UbUS5C7sz683WDfGiIUH6xaxX3U4GQ3Q1ep+YaFZmEqiIgrDAfKPoTsMlg8ywjfJTBbt8owwEoXkDUY9iKkqEK5XQqIw2j8Wv83CWKl5dYZIFGfZoMdkm3apm8XFCzR7hdMDxmk6YJz1d8CfYLbQyn3huuPtA00fmDkunhpUpNuuZvQYdx+VbVHvuNNQC9jjkx3mZcMOxn2q1X/DhLuNM/qqh9nxQf+Tbfb9gleaPzWX/KuxHmAQ6OUkwTD+OwjzFSwqIt+kU9bOqLfAXsG+f1/0/IovO6Hjel6Bbfoi/rIlKpI8JEfsB1B0vQ7QxTdn+eDh/UPAX8S9mT3z1XXtWu20qa+4rvv24ChA09bCDTQNnwMqEArQbccJ3Zoa0BbhZQWo8H06gTc1jftpvDsLDZxmiDA6s7+p2O3v+MxWIJzl1aS1Fmr3TioPUN9N60m2R4pPVu9OXRoroHtlz3zVX3YXlD7avZuUERZsnTVzZuUfuq7nMwerbsLuVG7dN7bvI24frmP7R63/asHjIhpKzvIUcM7KFBPKBC4jLnHEhYpVzGjJKVoHaKob95vyNRpUPDz2ge3Y3wW8KFQ8vKLNYjsy0jxJzwu4gYrXYmEqzQc6/CT2jRorB3bC2/nCTfXSjzb/0ZBQOy145V+uL7Q5MF6ath4c6zp/+GqT4QSlvlJrFRcwq/wFnv52pCx9Y3al6wB9mk/hzlA/bSOf7CMT39WuofvG2trJLzfh3bFTBt+zXUxyKWNCMC1VikmaAKYsjbHMkzSJRZYoFqN1cGyj/LyAaVtzO/pFN6JtPo+P/o3G6W/9+Ku1ryg8fxUT0yzNs7FL9IEeoiUFkAxwJEWICSkTXBJGsIhYSAVLGU3oSQ9l54n/alYrkNjUoylwpXzip3HS/xPpIydSpLIyJILhMgkTTECEmJNI4YxESkS5THhWnnQTPS9gZipuR5cVt/C5rPSfHUr+D+tHQ0mL0cTyGkaX1ctHzqac0TQMlcKciByTPCoxDSHCiSQqVAkvOc3R+rEvt2X4QLLkcXRrrFs8Q+PA1qM/uQM7uq4b/+upTT38e0uAKCVFhlUKISZKAuZRSTGTZc5YQhSLQrT+Dk+y1dQPEAAA"
            }
        },

        #endregion

        #region B Rank

        [464] = new FishingTools 
        {
            FishingPreset = new()
            {
                "AH4_H4sIAAAAAAAACt1Xy27bOhD9FYNrCZAo6rlzffMC3DSoU2QRdEFLI5uwLLoklTQN/O8FJdGWFCVxiywuujOGM2fOjOblZzStFJ9RqeQsX6HkGZ2VdFnAtChQokQFFtKPc1bC8TEzT1cZSnAUW+hGMC6YekKJa6ErefYzLaoMsqNY6+8brM+cp2sNVv/A+leNE0QWutjdrgXINS8ylLiO00N+G7rGiMOehfMumdm62r4SGHEd8g4jA8KLAlJlIiGu43bV8PssuMgYLQxA4JIeAGnVzplcnz2B7DjyBwx9v8cwMDmnG1isWa4+UVbz1AJpBAtF041Eid9mMYhe4nZR4xb1hioGZQodPsHQLuhnDBtTwX7BjKqmEozXoTUe5NtrrW/XtGB0I8/pAxcaoCcw4XhWX/4VUv4AAiWuTpLxSXoeTMI+sdUF3daRTctVAUIaVP01M5R4oUNe0O1BRfu9hc5+KkF7nXWoMZ35W754pLurUlVMMV5eUFaafNiuheaVgM8gJV0BShCy0HXNCV3zElCL8LQDlOjEjODNuVR/jXcjQMI4Q2SjV94bj/X7kc9iB6kStJhVQkCpPijKAeqHxTrK9kXEo95rrXMuUqjb6pHuzMeuhZmW1o3ixz622spaKL7Tnc3K1ULBrh6hxyjb6puKjwmuC1ez/VayHxVoXBQtU0zS2LNDP4ptEuPAjkiI7dSPlsEShz72AO0tNGdSfcm1D4mS++famw7gQDCOX2c4LYpJYzqkec3FlhaXnG80kJktd0A3x+bRrxJ6mW1FDQ5xQz2cjPFCCV7WvddqHVowp4UE62RUx+ugzmEFZUbF00cBf5PwH69afaPYSEz4X8ri6W4N5TRV7AGuMigVS/XGGEHFgc5BY/9uBl61PCXKEeNbwXZH2n0FXUEHlRfMxpTGSPT1dPdMcwViRqvVWs3ZVi84t3kYtlV9ylSi2aD6R2dVjFwAXujHw0X45k2hzxAzCU0hf4UfFROQLRRVlV6y+s4ZVvcfFfHJRdlT7NfT6QVzWmX8EyVgvjn5w2/emaFu6EXg4MgOI5rbBOe+TbEPdhws89Qn4GZA0P67GaLtLXx/EDRz9P4ZdQcq8UPivT5SbwSTW6pYOqmNunPVfSs9hymic6J9NQrTLa/KjtrYaezHw9vH6x+ekXZciZymsCj09DORvGio4Y3n7y30v/mLcNzDf719F490pyUzndU6od193G5h/bMRH9XGqrdTaUvPz1yXeDbJ48AmnhvYUeqAHcWOkwVujP3YryutwW0p3pOAfJ9c8qLgj5NLKpZcTO6oAjG5KqU+aRgv+1cBdhy8DD3Hdr1oaZMUMptiTG28JLlHAxrnNED73xGOGoBIDgAA"
            }
        },
        [465] = new FishingTools 
        {
            FishingPreset = new()
            {
                "AH4_H4sIAAAAAAAACs1VTU/jMBD9K2jOiZTvOLmVqkVIhUVb9oQ4TJNJYxHirO3Asqj/feU02Tal3UqIw96s8fjNe+Pn8TtMWi2mqLSaFmtI32FW46qiSVVBqmVLFpjNBa9pt5kPW9c5pB5LLLiTXEiu3yB1LbhWs19Z1eaU78Imf7PFuhEiKw1Yt/DMqsOJmAVXzX0pSZWiyiF1HWeE/G/oDiOJRyecs2SmZft8QljgOsEZRgOIqCrK9KAkcB13P807z0LInGN1gojrRdGox0F/bM5VOXsjtVc4PGAchiPG0XAH+ETLkhf6EnnH2wTUEFhqzJ4UpGHf1Yh9xN1HTXrUO9Sc6uyUUwLXiQ5honFDvQFJ8t80Rb01ykDi8LR3cB1+f/q+xIrjk5rji5AGYBQY1PnWOP6dMvFCElLX9OyY0yNmHLFXcGjnJV9f4XOne1KvK5JqKGLuPofUj53gA/sRFNtsLJj90hJH7/AvAXMv92L5is11rVuuuaivkNdDe2zXgkUr6YaUwjVBCmDBbccJbkVN0CO8NQSp6dMRvIVQ+tN4d5IUHWcINpzY31bs9nd8lg1lWmI1baWkWn+RygPUL9N6lO0HxUerd1lzITPqHt0rNsNld8HcRLt3EyahZ/XOWmrRmHfP6/VSU9MN3J3K3n0T+TXi9uE6tj9q/rMlgwt54FFY+JEdx5FjBxnLbOZHhb2Ks8TJMUDPd2FjwYIr/a0wNRSkD49DoP8FdgEjCtKHd9gu+okWxgE7LWDR1igvpoIqzLDW5UiMmc6mW5NCk5xiuy71gj+bcWc+kpxqzTOszPM11bYJk2fR1ntpx6ZYmBy+ZH88ZJkp3MoCM1pW5lIHLUl4ZoCFGwv+m+9x56pPe2n5io2JTE1Xu4buu6v3lFluw7u0Yy7f857rMgfJITtZFY4dOC6zMY58G1mx8rzc8YMkgc3jUK6n+BBE4eNFF5IvlF/MhciVbotCjZ29YlGMOfPsmLmJHTBiNovj0PYwCoIQCzdOXNj8AX/IXB46CQAA"
            }
        },
        [466] = new FishingTools 
        {
            FishingPreset = new()
            {
                "AH4_H4sIAAAAAAAACu1XyW7jOBD9FYNnEZAoUtvN7XEWwEkH7QzmEDQGFFWyBcuim6KS9gT+9wEly5a8ZGkEgx4gN6FYfPWq+FgsPaNhpeWIl7ocpTMUPaNxweMchnmOIq0qsJBZnGQF7BeTduk6QREJQgvdqUyqTK9R5Fjouhz/FHmVQLI3G/9Ng3UjpZgbsPqDmK8axwssdLm6nyso5zJPUOTYdg/5ZegaI/R7O+xXyYzm1fJMYtSx6SuMWhCZ5yB0mwl1bKfrRl5nIVWS8fwMEYd4Xq/GdLvtIivn4zWUncDsgDFjPcZeewZ8AdN5luovPKt5G0PZGqaai0WJIratqhcc43ZRwy3qHdcZFAI6fLzDfV6/gqTdqrJ/YMR1o4w26uFuclB/d7v7fs7zjC/KC/4olQHoGdp0XKtv/wZCPoJCkWOKdEraXmAk0AnY1u9LNrvkyzrRYTHLQZVtEHPYCYpc36ZH7HtQwWZjofFPrXjv4u0ImIO4l9MnvroudJXpTBaXPCva8mDHQpNKwQ2UJZ8BihCy0G3NCd3KAtAWYb0CFJk6ncCbyFL/Mt6dghJOM0QYnVlvItbrez7TFQiteD6qlIJCf1CWB6gflutJtkcZn4xee11IJaC+ZU981R52bUyMtb43LGTU2iprquXKXPSsmE01rOoOu89yq76h+pjkunA12z+L7EcFBhclqcsZdV3sMMYwtUmCAyAuJoR5duinMbEdtLHQJCv119TEKFH08FxHMwnsukKT3TmOI1kuMzG454oXusq5gbyVasnzKykXBqRtM38BX+wvjlktoVfVralJlTq+6VPt5qlWspi9Z7vtdrZPYAZFwtX63Qh/yCrOd9x7HsQLdw57fmddehxOeN2rbHUuks+Iu3M5F6vn9EK0rZ9R6jDVoEa8ms31JFuat8VpFg4lXE8VlWoeL/PR6dJNA2Xh8fP7wktqRoC2zbRK+QY/qkxBMtVcV+ZBMzPGoXzeppI3i+HzzP/TM+80KOraggPEOAWfYxqzGHNIKU5c8N3A8xICCdp8bzvUdg592BmaJvXwjPrdymfkfLf6uuL5YAx5r6s6LxXmOoFCZ4LnphomSuMwXMqq6LnVnfJwhnD781xgIlUq5QKmuek7+xb7yujENhb6bSZx0zObSfRw9ulmxM4fw4Wh+sQ1qMFQ6bmSeaUAdZBHprqNMJ/4qnnyyjZc9wVEEeKFLP5+oJ6Hne+DK5nn8mlwxVUs1WAsZC5n5vQG00o9whp1ATtBTqi/o1QXEkgDD7Cb0gRT4Tk4IEJglkLgEzvxeJCijXWsRPd8CSZVwdXgLlNcVaL61OOnHt+uR9tNOQ0JwQ7jMaZuLHBAQ4IFjwXEnpOGHjmlRzc8X4IbKQslxWIw4kWy/n30eOpP/38mz2M5vnvonx7p8EB0Zvj/BSU5xHU5A4GZm6aYhk6IY9+hWDCjooTECSf1G9zgbinW4iZvEXf3f8QmPveJiwNGA0xp6uNAEBt7jLLUYzQIfAdt/gXTu+Ob4RIAAA==",

                "AH4_H4sIAAAAAAAACu1X227jNhD9FYPPIiBKpG5vXm9ugJMG6xR9CBYFRY1sIrLopahk3cD/XlCyEsm3bBd5yLZ9k8nhOTPDw5nxMxrXRk14ZapJPkfJMzoreVrAuChQYnQNDrKbU1nC62bWbV1lKPGi2EG3WiotzRolxEFX1dl3UdQZZK/L1n7TYl0rJRYWrPnw7FeDE0QOuljdLTRUC1VkKCGuO0A+Dd1gxOHghPumM5NFvTwSGCUufcOjDkQVBQjTRUKJS/pm3tteKJ1JXhxxhHhBMMgx3R47l9XibA1Vj5jteMzYwOOguwP+ALOFzM0nLhu/7ULVLcwMFw8VStg2q0G0j9tHjbeot9xIKAX0/Al2zwXDDHrdUS3/ggk3rTI61t3T3k7+/e3puwUvJH+ozvmj0hZgsNCF4zvD9S8g1CNolBCbpEPSDiIrgR5hl79Pcn7Bl02g43JegK46EnvZGUr80KV73g+gos3GQWffjeaDh/figL2IOzV74qur0tTSSFVecFl26cHEQdNawzVUFZ8DShBy0E3jE7pRJaAtwnoFKLF5OoA3VZX5abxbDRUc9hBhdGS/ZWz2X/2ZrUAYzYtJrTWU5p2i3EF9t1gPersX8UH2xupcaQHNK3viq+6ym8XMrjbvhsWMOVtlzYxa2Ycuy/nMwKqpsK9RbtU31u8TXB+u8fb3Un6rweIilntREMSAoywFTGMS4jilgIEEoeAkDVw/RRsHTWVlfsstR4WS++eGzQbwUhXa6I75eG7fyxM3oEdjbRZaFbUGi3uj9JIXl0o9WKSu1vwBvPlt120YTViUhLYmdTYzo1U5P2Dl+j2rKcyhzLheHzP8rOq0OEzoBfGLwRG2vslxqtbqTsvVMaaQef6LyTGugdEJtq2d1dg4N6AnvJ4vzFQubVcg7cau+Jp5oNZt27Efvfralj4W7zfOEz3QNu+uQHTX+wW+1VJDNjPc1LYV2eng/zv/N915r7T4OWWxTxgO88DHlHuAUz+NMBXc9/Kc8TCL0OZrV1u2E+T9y0JbXu6f0bDOhOxELZzWJdejCU/VMuWDokhOZecqg9JIwQubEkvVGoyXqi4HZk2h2x0B/OE4FlmmWudcwKyw3eC1Qr4x+bCNgz7MIG07RztI7o4u/Yjo8buYqGopxeiOa16auuCoBzqxiW2F+cRXbbOqOqZ+70IJ4qUq/7ynQYC9r6NLVRTqaXTJdar06EyoQs3txY1mtX6ENeoD9kgOqL+nVDeO/QDSHFMiAFNBCOapxzGxGynjgmY+2jh7SvTj49FfK1VqJR5GE15m648jxUP/iX4xZe4r8R+PR/s63BEd/jklsRSy2BMezqhgtuZ5OI6EwDHJXBHHURoyckhJ1D2tpLksio+joV9RNP+hchZHLoeQEOy5gYtpGsU4tZqkAY1cNwSP8bhpvC3uNr7GJfIjLvWoBKXMdSngDFiAKfV8HBGIsOsLkXu+iIXvos3fXtwqLpASAAA="
            }
        },
        [467] = new FishingTools 
        {

        },
        [468] = new FishingTools 
        {

        },
        [469] = new FishingTools 
        {

        },
        [470] = new FishingTools 
        {

        },
        [471] = new FishingTools 
        {

        },

        #endregion

        #region A Rank | Sequence | Timed | Weather

        [472] = new FishingTools 
        {

        },
        [473] = new FishingTools 
        {

        },
        [474] = new FishingTools 
        {

        },
        [475] = new FishingTools 
        {

        },
        [476] = new FishingTools 
        {

        },
        [477] = new FishingTools 
        {

        },
        [478] = new FishingTools 
        {

        },
        [479] = new FishingTools { },
        [480] = new FishingTools 
        {

        },
        [481] = new FishingTools { },
        [482] = new FishingTools { },
        [483] = new FishingTools 
        {

        },
        [484] = new FishingTools { },
        [485] = new FishingTools 
        {
        },
        [486] = new FishingTools { },
        [487] = new FishingTools { },
        [488] = new FishingTools 
        {

        },
        [489] = new FishingTools { },
        [490] = new FishingTools { },
        [491] = new FishingTools { },
        [492] = new FishingTools { },
        [493] = new FishingTools { },
        [494] = new FishingTools { },
        [495] = new FishingTools { },
        [508] = new FishingTools 
        {

        },
        [509] = new FishingTools 
        {

        },
        [510] = new FishingTools { },
        [511] = new FishingTools { },

        #endregion

        #region Critical

        [542] = new FishingTools { },
        [543] = new FishingTools { },
        [544] = new FishingTools { },

        #endregion
    };
}
