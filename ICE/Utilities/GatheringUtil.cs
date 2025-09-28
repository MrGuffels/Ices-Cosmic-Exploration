using FFXIVClientStructs.FFXIV.Client.Game;
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
        public Dictionary<uint, ClassGathInfo> ClassAction { get; set; } = new();
        /// <summary>
        /// Sheet name
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

    public class ClassGathInfo
    {
        public string SkillName { get; set; }
        public uint ActionId { get; set; }
    }

    public static Dictionary<string, GatheringActions> GathActionDict = new()
    {
        { "BoonIncrease1", new GatheringActions
        {
            ActionName = "Pioneer's Gift I",
            ClassAction = new()
            {
                [16] = new() { ActionId = 21177, SkillName = "", },
                [17] = new() { ActionId = 21178, SkillName = "", }
            },
            StatusId = 2666,
            StatusName = "Gift of the Land",
            RequiredGp = 50,
        }},
        { "BoonIncrease2", new GatheringActions
        {
            ActionName = "Pioneer's Gift II",
            ClassAction = new()
            {
                [16] = new() { ActionId = 25589, SkillName = "", },
                [17] = new() { ActionId = 25590, SkillName = "", }
            },
            StatusId = 759,
            StatusName = "Gift of the Land II",
            RequiredGp = 100,
        }},
        { "Tidings", new GatheringActions
        {
            ActionName = "Nophica's Tidings",
            ClassAction = new()
            {
                [16] = new() { ActionId = 21203, SkillName = "", },
                [17] = new() { ActionId = 21204, SkillName = "", }
            },
            StatusId = 2667,
            StatusName = "Gatherer's Bounty",
            RequiredGp = 200,
        }},
        { "YieldI", new GatheringActions
        {
            ActionName = "Blessed Harvest",
            ClassAction = new()
            {
                [16] = new() { ActionId = 239, SkillName = "", },
                [17] = new() { ActionId = 222, SkillName = "", }
            },
            StatusId = 219,
            StatusName = "Gathering Yield Up",
            RequiredGp = 400,
        }},
        { "YieldII", new GatheringActions
        {
            ActionName = "Blessed Harvest II",
            ClassAction = new()
            {
                [16] = new() { ActionId = 241, SkillName = "", },
                [17] = new() { ActionId = 224, SkillName = "", }
            },
            StatusId = 219,
            StatusName = "Gathering Yield Up",
            RequiredGp = 500,
        }},
        { "BonusIntegrity", new GatheringActions
        {
            ActionName = "Ageless Words",
            ClassAction = new()
            {
                [16] = new() { ActionId = 232, SkillName = "Solid Reason", },
                [17] = new() { ActionId = 215, SkillName = "Ageless Word", }
            },
            RequiredGp = 300,
        }},
        { "BonusIntegrityChance", new GatheringActions
        {
            ActionName = "Wise of the World",
            ClassAction = new()
            {
                [16] = new() { ActionId = 26521, SkillName = "", },
                [17] = new() { ActionId = 26522, SkillName = "", }
            },
            StatusId = 2765,
            StatusName = "",
            RequiredGp = 0,
        }},
        { "BountifulYieldII", new GatheringActions
        {
            ActionName = "Bountiful Yield/Harvest II",
            ClassAction = new()
            {
                [16] = new() { ActionId = 272, SkillName = "", },
                [17] = new() { ActionId = 273, SkillName = "", }
            },
            StatusId = 1286,
            StatusName = "",
            RequiredGp = 100,
        }},
    };

    public static Dictionary<string, GatheringActions> GathCollectableBuffs = new()
    {
        { "Scrutiny", new GatheringActions
        {
            ActionName = "Scrutiny",
            ClassAction = new()
            {
                [16] = new() { ActionId = 22185 },
                [17] = new() { ActionId = 22189 }
            },
            StatusId = 757,
            StatusName = "",
            RequiredGp = 200,
        }},
        { "Focus", new GatheringActions
        {
            ActionName = "Collector's Focus",
            ClassAction = new()
            {
                [16] = new() { ActionId = 21205 },
                [17] = new() { ActionId = 21206 }
            },
            StatusId = 2668,
            StatusName = "",
            RequiredGp = 100,
        }},
        { "Priming", new GatheringActions
        {
            ActionName = "Priming Touch",
            ClassAction = new()
            {
                [16] = new() { ActionId = 21205 },
                [17] = new() { ActionId = 34872 }
            },
            StatusId = 2668,
            StatusName = "",
            RequiredGp = 100,
        }},
        { "CollectorsHigh", new GatheringActions
        {
            // Only available in certain missions... *-sighs-*
            ActionName = "Collectors High Standard",
            ClassAction = new()
            {
                [16] = new() { ActionId = 27 },
                [17] = new() { ActionId = 27 }
            },
            StatusId = 3911,
            StatusName = "",
            RequiredGp = 0,
        }},
        { "BonusIntegrity", new GatheringActions
        {
            ActionName = "Ageless Words",
            ClassAction = new()
            {
                [16] = new() { ActionId = 232, SkillName = "Solid Reason", },
                [17] = new() { ActionId = 215, SkillName = "Ageless Word", }
            },
            RequiredGp = 300,
        }},
        { "BonusIntegrityChance", new GatheringActions
        {
            ActionName = "Wise of the World",
            ClassAction = new()
            {
                [16] = new() { ActionId = 26521, SkillName = "", },
                [17] = new() { ActionId = 26522, SkillName = "", }
            },
            StatusId = 2765,
            StatusName = "",
            RequiredGp = 0,
        }},
    };

    public static Dictionary<string, GatheringActions> GathCollectableActions = new()
    {
        { "Scour", new GatheringActions
        {
            // Base general use skill
            ActionName = "Scour",
            ClassAction = new()
            {
                [16] = new() { ActionId = 22182 },
                [17] = new() { ActionId = 22186 }
            },
            StatusId = 0,
            StatusName = "n/a",
            RequiredGp = 0,
        }},
        { "Brazen", new GatheringActions
        {
            // 50 - 150% buff
            ActionName = "Brazen Woodsman",
            ClassAction = new()
            {
                [16] = new() { ActionId = 22183 },
                [17] = new() { ActionId = 22187 }
            },
            StatusId = 0,
            StatusName = "n/a",
            RequiredGp = 0,
        }},
        { "Meticulous", new GatheringActions
        {
            // Chance to not use durability/integrity
            ActionName = "Meticulous Woodsman",
            ClassAction = new()
            {
                [16] = new() { ActionId = 22184 },
                [17] = new() { ActionId = 22188 }
            },
            StatusId = 0,
            StatusName = "n/a",
            RequiredGp = 0,
        }},
        { "BonusIntegrity", new GatheringActions
        {
            ActionName = "Ageless Words",
            ClassAction = new()
            {
                [16] = new() { ActionId = 232, SkillName = "Solid Reason", },
                [17] = new() { ActionId = 215, SkillName = "Ageless Word", }
            },
            RequiredGp = 300,
        }},
        { "BonusIntegrityChance", new GatheringActions
        {
            ActionName = "Wise of the World",
            ClassAction = new()
            {
                [16] = new() { ActionId = 26521 },
                [17] = new() { ActionId = 26522 }
            },
            StatusId = 2765,
            StatusName = "",
            RequiredGp = 0,
        }},
        { "Collect", new GatheringActions
        {
            ActionName = "Collect",
            ClassAction = new()
            {
                [16] = new() { ActionId = 240},
                [17] = new() { ActionId = 815},
            },
            StatusId = 0,
            StatusName = "",
            RequiredGp = 0,
        } },
    };

    public unsafe static uint CollectStandardCharges()
    {
        try
        {
            if (DutyActionManager.GetInstanceIfReady() != null)
                return (uint)(DutyActionManager.GetInstanceIfReady()->CurCharges[1] + DutyActionManager.GetInstanceIfReady()->CurCharges[0]);

            return 0;
        }
        catch (Exception e)
        {
            ECommons.GenericHelpers.Log(e);
            return 0;
        }
    }

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
        public Vector3 FacePosition { get; set; }
        public Vector3 FishingSpot { get; set; }
    }

    public static Dictionary<uint, Dictionary<Vector2, List<FisherSpotInfo>>> MoonFishingLocations = new()
    {
        [1237] = new()
        {
            [new Vector2(-673f, 497f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-681.51f, 56.73f, 452.61f),
                    FishingSpot = new Vector3(-684.29f, 57.14f, 449.44f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-679.39f, 56.64f, 406.97f),
                    FishingSpot = new Vector3(-681.86f, 56.15f, 409.36f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(0.00f, 0.00f, 0.00f),
                    FishingSpot = new Vector3(0.00f, 0.00f, 0.00f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-717.31f, 57.61f, 396.69f),
                    FishingSpot = new Vector3(-715.62f, 57.07f, 398.90f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(0.00f, 0.00f, 0.00f),
                    FishingSpot = new Vector3(0.00f, 0.00f, 0.00f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-723.78f, 57.86f, 430.71f),
                    FishingSpot = new Vector3(-720.61f, 57.15f, 429.68f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-713.74f, 57.47f, 454.12f),
                    FishingSpot = new Vector3(-711.97f, 57.13f, 451.87f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-739.04f, 70.08f, 510.99f),
                    FishingSpot = new Vector3(-742.36f, 69.99f, 511.54f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-746.67f, 70.33f, 495.85f),
                    FishingSpot = new Vector3(-750.49f, 70.21f, 496.88f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-768.05f, 72.84f, 490.57f),
                    FishingSpot = new Vector3(-766.88f, 72.81f, 493.54f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-790.76f, 76.47f, 511.84f),
                    FishingSpot = new Vector3(-787.50f, 75.94f, 511.36f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-783.67f, 73.19f, 534.13f),
                    FishingSpot = new Vector3(-780.99f, 73.10f, 532.39f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-756.48f, 69.96f, 540.26f),
                    FishingSpot = new Vector3(-754.94f, 70.47f, 536.35f),
                },
            },
            // Export for Fishing Zone 1237, Flag (-642, -631)
            [new Vector2(-642f, -631f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-704.47f, 78.86f, -624.86f),
                    FishingSpot = new Vector3(-702.74f, 78.86f, -625.20f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-706.16f, 80.15f, -655.53f),
                    FishingSpot = new Vector3(-704.44f, 80.15f, -654.71f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-719.40f, 81.29f, -674.84f),
                    FishingSpot = new Vector3(-719.62f, 81.27f, -676.77f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-748.08f, 82.86f, -653.60f),
                    FishingSpot = new Vector3(-749.59f, 82.81f, -654.64f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-734.22f, 78.50f, -622.21f),
                    FishingSpot = new Vector3(-735.90f, 78.47f, -621.42f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-756.54f, 79.55f, -589.94f),
                    FishingSpot = new Vector3(-757.96f, 79.48f, -590.99f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-740.87f, 78.75f, -550.59f),
                    FishingSpot = new Vector3(-742.23f, 78.70f, -549.42f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-718.82f, 78.06f, -533.16f),
                    FishingSpot = new Vector3(-719.09f, 78.02f, -531.37f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-693.19f, 76.95f, -546.06f),
                    FishingSpot = new Vector3(-691.86f, 76.91f, -544.77f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-683.75f, 76.02f, -565.56f),
                    FishingSpot = new Vector3(-682.70f, 75.97f, -564.08f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-587.65f, 69.48f, -650.96f),
                    FishingSpot = new Vector3(-589.30f, 69.46f, -650.02f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-588.52f, 69.70f, -672.03f),
                    FishingSpot = new Vector3(-590.26f, 69.65f, -672.61f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-580.89f, 69.61f, -696.34f),
                    FishingSpot = new Vector3(-582.38f, 69.56f, -697.39f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-551.81f, 69.04f, -697.00f),
                    FishingSpot = new Vector3(-551.48f, 68.99f, -698.79f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-536.81f, 68.99f, -673.98f),
                    FishingSpot = new Vector3(-534.98f, 68.99f, -673.93f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-548.28f, 69.07f, -654.38f),
                    FishingSpot = new Vector3(-547.26f, 69.03f, -652.89f),
                },
            },
            [new Vector2(-348f, 604f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-325.62f, 47.98f, 607.32f),
                    FishingSpot = new Vector3(-329.34f, 47.71f, 605.86f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-320.30f, 47.83f, 579.08f),
                    FishingSpot = new Vector3(-322.90f, 47.62f, 580.78f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-347.21f, 47.86f, 562.18f),
                    FishingSpot = new Vector3(-347.55f, 47.62f, 565.18f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-374.60f, 47.86f, 575.96f),
                    FishingSpot = new Vector3(-371.96f, 47.67f, 577.85f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-373.32f, 47.81f, 612.22f),
                    FishingSpot = new Vector3(-370.08f, 46.95f, 610.97f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-361.07f, 47.95f, 639.70f),
                    FishingSpot = new Vector3(-358.79f, 47.78f, 636.85f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-335.31f, 47.81f, 647.01f),
                    FishingSpot = new Vector3(-336.22f, 47.66f, 644.39f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-318.74f, 47.84f, 627.92f),
                    FishingSpot = new Vector3(-322.40f, 47.62f, 629.60f),
                },
            },
            [new Vector2(-281f, -104f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-296.67f, 23.60f, -99.04f),
                    FishingSpot = new Vector3(-298.62f, 23.61f, -98.77f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-288.99f, 22.55f, -82.12f),
                    FishingSpot = new Vector3(-290.41f, 22.56f, -80.72f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-277.21f, 22.42f, -71.16f),
                    FishingSpot = new Vector3(-276.94f, 22.39f, -69.24f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-261.90f, 22.32f, -76.85f),
                    FishingSpot = new Vector3(-260.88f, 22.31f, -75.17f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-265.77f, 22.11f, -93.86f),
                    FishingSpot = new Vector3(-263.89f, 22.11f, -94.51f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-267.32f, 25.05f, -119.22f),
                    FishingSpot = new Vector3(-265.36f, 25.07f, -119.82f),
                },
            },
            [new Vector2(-139f, -283f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-121.13f, 20.16f, -282.06f),
                    FishingSpot = new Vector3(-125.02f, 19.49f, -282.55f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-116.87f, 21.07f, -295.31f),
                    FishingSpot = new Vector3(-120.34f, 21.01f, -296.72f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-133.53f, 22.10f, -311.44f),
                    FishingSpot = new Vector3(-133.89f, 21.89f, -307.83f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-149.53f, 21.87f, -301.28f),
                    FishingSpot = new Vector3(-147.11f, 21.38f, -300.01f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-156.93f, 21.11f, -274.46f),
                    FishingSpot = new Vector3(-153.78f, 20.48f, -275.65f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-139.95f, 20.60f, -259.87f),
                    FishingSpot = new Vector3(-140.35f, 20.74f, -263.50f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-123.05f, 20.56f, -265.43f),
                    FishingSpot = new Vector3(-126.56f, 19.80f, -267.38f),
                },
            },
            [new Vector2(104f, -269f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(77.32f, 18.30f, -247.04f),
                    FishingSpot = new Vector3(79.81f, 18.34f, -249.27f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(87.04f, 17.93f, -228.42f),
                    FishingSpot = new Vector3(90.35f, 17.72f, -230.32f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(111.18f, 19.02f, -226.39f),
                    FishingSpot = new Vector3(110.04f, 18.81f, -229.60f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(152.24f, 24.84f, -257.98f),
                    FishingSpot = new Vector3(147.53f, 24.62f, -258.54f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(146.35f, 28.85f, -282.43f),
                    FishingSpot = new Vector3(143.39f, 28.23f, -279.23f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(126.45f, 28.76f, -294.49f),
                    FishingSpot = new Vector3(124.30f, 28.83f, -293.16f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(95.25f, 28.89f, -300.15f),
                    FishingSpot = new Vector3(95.97f, 29.03f, -297.06f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(62.61f, 25.80f, -293.60f),
                    FishingSpot = new Vector3(65.88f, 25.24f, -292.10f),
                },
            },
            [new Vector2(193f, 196f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(191.93f, 17.58f, 188.29f),
                    FishingSpot = new Vector3(191.46f, 17.58f, 186.35f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(184.14f, 17.50f, 193.09f),
                    FishingSpot = new Vector3(182.83f, 17.50f, 191.59f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(182.53f, 17.60f, 200.46f),
                    FishingSpot = new Vector3(181.67f, 17.55f, 201.99f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(196.90f, 19.35f, 206.23f),
                    FishingSpot = new Vector3(196.96f, 19.35f, 208.23f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(202.64f, 19.47f, 202.95f),
                    FishingSpot = new Vector3(203.70f, 19.47f, 204.47f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(208.40f, 19.45f, 196.68f),
                    FishingSpot = new Vector3(210.37f, 19.45f, 197.03f),
                },
            },
            [new Vector2(573f, 573f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(609.12f, 54.33f, 580.25f),
                    FishingSpot = new Vector3(610.75f, 54.33f, 581.42f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(622.91f, 53.68f, 534.43f),
                    FishingSpot = new Vector3(624.69f, 53.68f, 533.99f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(617.22f, 54.78f, 513.61f),
                    FishingSpot = new Vector3(619.07f, 54.78f, 512.96f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(616.89f, 54.38f, 495.02f),
                    FishingSpot = new Vector3(618.41f, 54.36f, 496.12f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(615.84f, 50.98f, 479.81f),
                    FishingSpot = new Vector3(616.91f, 50.92f, 478.42f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(603.52f, 50.28f, 480.78f),
                    FishingSpot = new Vector3(603.09f, 50.27f, 479.07f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(593.25f, 50.03f, 491.32f),
                    FishingSpot = new Vector3(591.76f, 50.03f, 490.30f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(586.13f, 50.49f, 502.14f),
                    FishingSpot = new Vector3(584.81f, 50.48f, 500.88f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(576.58f, 50.34f, 514.46f),
                    FishingSpot = new Vector3(575.03f, 50.29f, 513.57f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(562.37f, 51.04f, 534.96f),
                    FishingSpot = new Vector3(561.92f, 51.03f, 533.17f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(529.04f, 51.24f, 538.05f),
                    FishingSpot = new Vector3(528.37f, 51.21f, 536.42f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(514.62f, 52.87f, 559.11f),
                    FishingSpot = new Vector3(512.91f, 52.87f, 558.70f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(518.79f, 51.11f, 580.81f),
                    FishingSpot = new Vector3(516.99f, 51.06f, 581.01f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(527.99f, 53.66f, 597.43f),
                    FishingSpot = new Vector3(526.16f, 53.66f, 597.76f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(521.59f, 53.16f, 619.16f),
                    FishingSpot = new Vector3(519.88f, 53.14f, 618.53f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(520.15f, 51.23f, 638.33f),
                    FishingSpot = new Vector3(518.43f, 51.19f, 638.93f),
                },
            },
            [new Vector2(909f, -336f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(912.58f, -58.25f, -328.75f),
                    FishingSpot = new Vector3(914.87f, -58.35f, -328.79f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(916.20f, -58.75f, -338.51f),
                    FishingSpot = new Vector3(918.65f, -58.90f, -337.07f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(932.74f, -57.69f, -359.17f),
                    FishingSpot = new Vector3(934.37f, -57.65f, -356.92f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(933.92f, -57.13f, -362.23f),
                    FishingSpot = new Vector3(936.45f, -56.75f, -360.90f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(939.04f, -55.44f, -368.28f),
                    FishingSpot = new Vector3(940.42f, -55.71f, -366.29f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(941.76f, -54.56f, -370.87f),
                    FishingSpot = new Vector3(943.26f, -54.79f, -368.71f),
                },
            },
        },
        [1291] = new()
        {
            [new Vector2(-700f, -652f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-676.44f, -2.99f, -653.59f),
                    FishingSpot = new Vector3(-680.97f, -4.06f, -653.59f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-682.00f, -3.00f, -664.97f),
                    FishingSpot = new Vector3(-685.18f, -3.94f, -662.78f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-694.13f, -2.88f, -672.60f),
                    FishingSpot = new Vector3(-695.36f, -3.98f, -668.60f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-712.22f, -2.99f, -668.82f),
                    FishingSpot = new Vector3(-710.34f, -3.99f, -665.82f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-720.01f, -2.92f, -650.55f),
                    FishingSpot = new Vector3(-717.04f, -3.96f, -651.28f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-712.97f, -3.14f, -634.80f),
                    FishingSpot = new Vector3(-711.18f, -4.07f, -637.41f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-693.99f, -3.06f, -630.53f),
                    FishingSpot = new Vector3(-695.14f, -4.13f, -633.54f),
                },
            },
            [new Vector2(-522f, 462f)] = new()
            {
            },
            [new Vector2(-450f, -673f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-458.07f, -8.02f, -671.92f),
                    FishingSpot = new Vector3(-454.69f, -9.23f, -673.17f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-455.46f, -8.39f, -659.50f),
                    FishingSpot = new Vector3(-452.35f, -9.21f, -662.55f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-443.30f, -8.86f, -658.08f),
                    FishingSpot = new Vector3(-444.48f, -9.18f, -659.68f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-438.66f, -9.10f, -666.39f),
                    FishingSpot = new Vector3(-440.41f, -9.82f, -666.91f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-438.30f, -8.79f, -674.51f),
                    FishingSpot = new Vector3(-439.83f, -8.77f, -674.59f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-445.84f, -8.57f, -688.73f),
                    FishingSpot = new Vector3(-446.96f, -9.69f, -686.45f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-457.90f, -7.80f, -693.78f),
                    FishingSpot = new Vector3(-457.68f, -8.38f, -690.98f),
                },
            },
            [new Vector2(-252f, -74f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-248.41f, -10.09f, -79.94f),
                    FishingSpot = new Vector3(-249.49f, -10.25f, -77.68f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-240.29f, -11.00f, -81.46f),
                    FishingSpot = new Vector3(-240.33f, -11.62f, -78.31f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-229.02f, -11.08f, -79.22f),
                    FishingSpot = new Vector3(-230.43f, -11.45f, -77.20f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-228.38f, -10.86f, -70.23f),
                    FishingSpot = new Vector3(-231.23f, -11.10f, -72.50f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-239.20f, -10.76f, -65.61f),
                    FishingSpot = new Vector3(-238.98f, -11.25f, -68.43f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-241.46f, -10.73f, -65.75f),
                    FishingSpot = new Vector3(-243.54f, -11.18f, -67.34f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-252.66f, -10.25f, -60.48f),
                    FishingSpot = new Vector3(-252.52f, -10.07f, -63.13f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-262.36f, -9.73f, -63.18f),
                    FishingSpot = new Vector3(-260.30f, -9.90f, -65.25f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-273.79f, -10.23f, -74.13f),
                    FishingSpot = new Vector3(-270.98f, -10.83f, -74.62f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-268.01f, -10.07f, -81.27f),
                    FishingSpot = new Vector3(-266.49f, -10.29f, -78.99f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-257.65f, -10.91f, -85.84f),
                    FishingSpot = new Vector3(-257.52f, -11.08f, -82.96f),
                },
            },
            [new Vector2(-239f, -352f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-239.70f, -6.63f, -359.42f),
                    FishingSpot = new Vector3(-239.26f, -6.63f, -355.69f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-236.09f, -6.43f, -360.86f),
                    FishingSpot = new Vector3(-234.13f, -7.05f, -358.69f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-216.25f, -4.51f, -363.22f),
                    FishingSpot = new Vector3(-218.18f, -4.50f, -361.86f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-217.15f, -6.62f, -353.96f),
                    FishingSpot = new Vector3(-220.26f, -7.21f, -353.64f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-215.15f, -6.68f, -346.41f),
                    FishingSpot = new Vector3(-218.17f, -6.95f, -345.71f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-211.91f, -7.40f, -332.87f),
                    FishingSpot = new Vector3(-213.56f, -7.57f, -334.14f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-226.48f, -8.44f, -328.96f),
                    FishingSpot = new Vector3(-224.87f, -9.06f, -331.05f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-230.26f, -7.42f, -340.99f),
                    FishingSpot = new Vector3(-228.63f, -6.71f, -343.76f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-239.19f, -8.53f, -337.74f),
                    FishingSpot = new Vector3(-239.12f, -8.75f, -340.62f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-248.03f, -8.91f, -342.80f),
                    FishingSpot = new Vector3(-246.44f, -9.14f, -345.84f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-257.40f, -8.99f, -348.86f),
                    FishingSpot = new Vector3(-255.05f, -9.66f, -349.50f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-262.06f, -7.95f, -361.69f),
                    FishingSpot = new Vector3(-260.39f, -8.70f, -359.37f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-249.80f, -7.29f, -359.88f),
                    FishingSpot = new Vector3(-250.22f, -6.85f, -357.14f),
                },
            },
            [new Vector2(28f, 99f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(0.08f, -11.95f, 63.10f),
                    FishingSpot = new Vector3(2.85f, -12.51f, 64.48f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(12.05f, -13.19f, 62.90f),
                    FishingSpot = new Vector3(10.25f, -14.11f, 64.94f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(20.01f, -12.15f, 71.47f),
                    FishingSpot = new Vector3(18.04f, -12.19f, 73.60f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(28.99f, -11.83f, 73.88f),
                    FishingSpot = new Vector3(27.43f, -12.32f, 76.54f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(35.22f, -11.77f, 83.39f),
                    FishingSpot = new Vector3(32.59f, -12.18f, 83.39f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(43.19f, -12.01f, 97.75f),
                    FishingSpot = new Vector3(40.23f, -12.72f, 98.95f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(45.91f, -11.38f, 106.61f),
                    FishingSpot = new Vector3(43.90f, -11.44f, 108.78f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(54.43f, -11.36f, 120.65f),
                    FishingSpot = new Vector3(50.35f, -12.65f, 120.35f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(62.81f, -11.04f, 128.76f),
                    FishingSpot = new Vector3(60.76f, -12.09f, 131.02f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(66.74f, -11.19f, 139.08f),
                    FishingSpot = new Vector3(63.89f, -12.32f, 138.41f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(47.85f, -12.66f, 142.70f),
                    FishingSpot = new Vector3(48.68f, -13.80f, 138.88f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(31.86f, -12.06f, 133.57f),
                    FishingSpot = new Vector3(32.15f, -13.68f, 129.67f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(16.78f, -12.14f, 124.23f),
                    FishingSpot = new Vector3(20.85f, -13.00f, 121.99f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(12.42f, -12.02f, 113.60f),
                    FishingSpot = new Vector3(14.83f, -12.12f, 111.30f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(6.02f, -10.97f, 100.08f),
                    FishingSpot = new Vector3(9.58f, -12.16f, 101.33f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(15.25f, -10.81f, 90.49f),
                    FishingSpot = new Vector3(19.14f, -10.52f, 89.53f),
                },
            },
            [new Vector2(46f, -344f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(42.45f, 9.45f, -336.36f),
                    FishingSpot = new Vector3(41.51f, 9.79f, -339.41f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(30.07f, 8.43f, -325.01f),
                    FishingSpot = new Vector3(31.79f, 7.84f, -329.47f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(31.76f, 8.42f, -338.79f),
                    FishingSpot = new Vector3(33.50f, 8.40f, -338.15f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(37.46f, 9.62f, -348.96f),
                    FishingSpot = new Vector3(38.50f, 8.76f, -346.58f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(49.10f, 13.05f, -357.40f),
                    FishingSpot = new Vector3(50.84f, 11.86f, -355.20f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(62.91f, 12.84f, -351.41f),
                    FishingSpot = new Vector3(60.05f, 11.59f, -350.42f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(58.30f, 10.77f, -338.69f),
                    FishingSpot = new Vector3(55.84f, 10.34f, -341.09f),
                },
            },
            [new Vector2(214f, -742f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(217.07f, 134.03f, -754.25f),
                    FishingSpot = new Vector3(217.51f, 133.57f, -750.65f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(206.88f, 133.68f, -745.50f),
                    FishingSpot = new Vector3(209.77f, 133.82f, -744.00f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(205.17f, 133.64f, -741.28f),
                    FishingSpot = new Vector3(205.18f, 133.59f, -738.54f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(206.42f, 133.97f, -726.84f),
                    FishingSpot = new Vector3(206.03f, 133.68f, -729.59f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(221.92f, 134.34f, -731.40f),
                    FishingSpot = new Vector3(218.78f, 133.40f, -732.39f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(220.82f, 134.18f, -739.20f),
                    FishingSpot = new Vector3(217.94f, 133.71f, -740.18f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(227.07f, 133.59f, -746.64f),
                    FishingSpot = new Vector3(224.13f, 133.67f, -747.62f),
                },
            },
            [new Vector2(462f, -47f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(522.39f, 3.72f, 6.38f),
                    FishingSpot = new Vector3(526.37f, 1.46f, 6.78f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(514.93f, 5.06f, -5.50f),
                    FishingSpot = new Vector3(517.66f, 3.72f, -7.25f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(534.45f, 2.90f, 20.02f),
                    FishingSpot = new Vector3(532.72f, 2.09f, 17.03f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(530.05f, 4.40f, -7.54f),
                    FishingSpot = new Vector3(530.28f, 3.94f, -4.81f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(547.62f, 5.78f, -12.96f),
                    FishingSpot = new Vector3(545.12f, 4.14f, -11.22f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(543.05f, 4.01f, 6.34f),
                    FishingSpot = new Vector3(540.22f, 2.90f, 3.38f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(395.64f, 29.33f, -82.69f),
                    FishingSpot = new Vector3(392.52f, 28.75f, -82.02f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(393.66f, 26.52f, -68.54f),
                    FishingSpot = new Vector3(392.21f, 26.54f, -70.02f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(380.67f, 25.86f, -61.85f),
                    FishingSpot = new Vector3(380.79f, 25.43f, -65.96f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(372.12f, 26.73f, -73.69f),
                    FishingSpot = new Vector3(375.33f, 26.09f, -72.63f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(378.86f, 28.80f, -80.56f),
                    FishingSpot = new Vector3(381.41f, 27.31f, -78.12f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(384.36f, 31.21f, -89.83f),
                    FishingSpot = new Vector3(386.65f, 29.25f, -85.66f),
                },
            },
            [new Vector2(526f, 448f)] = new()
            {
            },
            [new Vector2(562f, 580f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(568.49f, -255.60f, 443.19f),
                    FishingSpot = new Vector3(566.70f, -255.60f, 443.12f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(563.41f, -255.53f, 423.46f),
                    FishingSpot = new Vector3(558.72f, -255.60f, 422.97f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(564.38f, -255.60f, 392.22f),
                    FishingSpot = new Vector3(561.69f, -255.60f, 394.55f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(543.42f, -255.60f, 377.75f),
                    FishingSpot = new Vector3(543.87f, -255.60f, 382.61f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(518.68f, -255.60f, 381.85f),
                    FishingSpot = new Vector3(521.64f, -255.60f, 384.93f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(495.36f, -255.60f, 394.65f),
                    FishingSpot = new Vector3(497.47f, -255.60f, 396.45f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(487.47f, -255.60f, 418.04f),
                    FishingSpot = new Vector3(491.50f, -255.60f, 418.84f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(497.92f, -255.60f, 441.74f),
                    FishingSpot = new Vector3(501.92f, -255.60f, 441.30f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(507.45f, -255.60f, 469.76f),
                    FishingSpot = new Vector3(510.07f, -255.60f, 469.08f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(523.13f, -255.60f, 492.38f),
                    FishingSpot = new Vector3(523.15f, -255.60f, 490.31f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(553.21f, -255.60f, 511.88f),
                    FishingSpot = new Vector3(554.02f, -255.60f, 510.50f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(575.75f, -255.60f, 512.97f),
                    FishingSpot = new Vector3(574.34f, -255.60f, 510.77f),
                },
            },
        },
    };

    public static Dictionary<uint, Dictionary<Vector2, List<GathNodeInfo>>> MoonGatherLocations = new()
    {
        [1237] = new()
        {
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
                    LandZone = new Vector3(-730.36f, 78.54f, -510.34f),
                    NodeId = 35089,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-727.81f, 79.13f, -503.01f),
                    LandZone = new Vector3(-726.55f, 77.83f, -503.28f),
                    NodeId = 35090,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-640.96f, 60.56f, -463.86f),
                    LandZone = new Vector3(-639.77f, 59.70f, -464.76f),
                    NodeId = 35087,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-637.53f, 59.94f, -456.50f),
                    LandZone = new Vector3(-636.71f, 58.94f, -457.73f),
                    NodeId = 35088,
                },
            },
            [new Vector2(-524f, 379f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-452.78f, 60.37f, 379.16f),
                    LandZone = new Vector3(-453.84f, 59.47f, 381.17f),
                    NodeId = 35095,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-455.10f, 59.95f, 373.16f),
                    LandZone = new Vector3(-457.01f, 59.35f, 373.97f),
                    NodeId = 35096,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-540.52f, 68.91f, 303.68f),
                    LandZone = new Vector3(-540.22f, 67.24f, 305.90f),
                    NodeId = 35094,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-554.54f, 68.53f, 308.34f),
                    LandZone = new Vector3(-552.58f, 67.35f, 309.29f),
                    NodeId = 35093,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-551.84f, 53.58f, 442.55f),
                    LandZone = new Vector3(-551.52f, 53.15f, 440.49f),
                    NodeId = 35098,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-547.85f, 53.53f, 446.89f),
                    LandZone = new Vector3(-546.35f, 52.79f, 447.71f),
                    NodeId = 35097,
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
            [new Vector2(404f, -802f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(345.89f, -118.14f, -803.14f),
                    LandZone = new Vector3(346.63f, -120.01f, -803.70f),
                    NodeId = 35104,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(349.89f, -116.71f, -798.16f),
                    LandZone = new Vector3(350.60f, -118.53f, -798.91f),
                    NodeId = 35103,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(423.53f, -115.18f, -735.87f),
                    LandZone = new Vector3(423.79f, -116.70f, -736.87f),
                    NodeId = 35101,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(436.18f, -119.70f, -746.12f),
                    LandZone = new Vector3(434.96f, -120.71f, -745.94f),
                    NodeId = 35102,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(449.16f, -117.91f, -856.33f),
                    LandZone = new Vector3(448.56f, -118.71f, -855.31f),
                    NodeId = 35100,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(439.48f, -117.91f, -859.57f),
                    LandZone = new Vector3(439.85f, -118.68f, -858.42f),
                    NodeId = 35099,
                },
            },
            [new Vector2(-706f, 564f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-684.35f, 58.86f, 503.32f),
                    LandZone = new Vector3(-683.60f, 58.23f, 504.68f),
                    NodeId = 35196,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-701.75f, 62.75f, 512.91f),
                    LandZone = new Vector3(-700.85f, 62.11f, 514.00f),
                    NodeId = 35195,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-772.70f, 69.54f, 593.97f),
                    LandZone = new Vector3(-773.29f, 68.94f, 592.76f),
                    NodeId = 35200,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-770.47f, 70.37f, 608.22f),
                    LandZone = new Vector3(-769.52f, 69.51f, 606.86f),
                    NodeId = 35199,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-651.49f, 56.65f, 620.18f),
                    LandZone = new Vector3(-653.01f, 56.47f, 619.18f),
                    NodeId = 35198,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-649.10f, 57.32f, 604.20f),
                    LandZone = new Vector3(-650.02f, 57.06f, 605.83f),
                    NodeId = 35197,
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
            [new Vector2(566f, -908f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(599.50f, -137.88f, -890.90f),
                    LandZone = new Vector3(598.19f, -138.44f, -890.75f),
                    NodeId = 35232,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(597.52f, -137.84f, -902.99f),
                    LandZone = new Vector3(596.83f, -138.30f, -902.08f),
                    NodeId = 35231,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(557.76f, -128.66f, -962.77f),
                    LandZone = new Vector3(558.95f, -129.05f, -963.30f),
                    NodeId = 35235,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(551.58f, -128.77f, -962.95f),
                    LandZone = new Vector3(550.48f, -129.23f, -962.32f),
                    NodeId = 35236,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(533.47f, -128.57f, -876.10f),
                    LandZone = new Vector3(534.80f, -129.00f, -876.09f),
                    NodeId = 35234,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(533.52f, -127.52f, -869.26f),
                    LandZone = new Vector3(535.01f, -128.37f, -869.20f),
                    NodeId = 35233,
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
            [new Vector2(874f, -771f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(852.60f, -49.63f, -714.26f),
                    LandZone = new Vector3(850.62f, -49.96f, -714.16f),
                    NodeId = 35206,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(844.02f, -49.58f, -706.32f),
                    LandZone = new Vector3(843.06f, -50.31f, -707.46f),
                    NodeId = 35205,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(780.73f, -49.59f, -793.34f),
                    LandZone = new Vector3(781.44f, -50.40f, -792.39f),
                    NodeId = 35202,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(779.68f, -49.49f, -799.95f),
                    LandZone = new Vector3(780.52f, -50.37f, -800.25f),
                    NodeId = 35201,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(909.35f, -32.17f, -828.50f),
                    LandZone = new Vector3(907.84f, -33.26f, -828.82f),
                    NodeId = 35203,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(919.15f, -31.58f, -808.37f),
                    LandZone = new Vector3(918.00f, -32.16f, -808.61f),
                    NodeId = 35204,
                },
            },
        },
        [1291] = new()
        {
            [new Vector2(-706f, -464f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-677.49f, 3.87f, -396.39f),
                    LandZone = new Vector3(-678.70f, 2.99f, -397.15f),
                    NodeId = 35300,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-670.86f, 3.37f, -399.23f),
                    LandZone = new Vector3(-670.91f, 2.42f, -400.42f),
                    NodeId = 35299,
                },
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
                    Position = new Vector3(-744.39f, 4.28f, -522.87f),
                    LandZone = new Vector3(-743.58f, 3.03f, -522.21f),
                    NodeId = 35301,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-744.95f, 3.85f, -515.26f),
                    LandZone = new Vector3(-744.90f, 3.37f, -514.68f),
                    NodeId = 35302,
                },
            },
            [new Vector2(-552f, 651f)] = new()
            {
            },
            [new Vector2(-481f, -533f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-486.94f, -4.44f, -554.26f),
                    LandZone = new Vector3(-488.14f, -5.14f, -551.95f),
                    NodeId = 35354,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-481.86f, -4.79f, -551.38f),
                    LandZone = new Vector3(-482.41f, -5.48f, -551.35f),
                    NodeId = 35353,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-477.31f, -3.85f, -530.33f),
                    LandZone = new Vector3(-478.85f, -4.78f, -530.15f),
                    NodeId = 35352,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-478.01f, -3.47f, -520.87f),
                    LandZone = new Vector3(-479.12f, -4.40f, -521.79f),
                    NodeId = 35351,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-474.70f, -3.76f, -516.29f),
                    LandZone = new Vector3(-476.00f, -4.44f, -516.77f),
                    NodeId = 35355,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-476.26f, -3.47f, -509.29f),
                    LandZone = new Vector3(-476.29f, -4.15f, -510.13f),
                    NodeId = 35356,
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
            [new Vector2(184f, 624f)] = new()
            {
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
            [new Vector2(362f, 438f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(424.83f, -254.71f, 471.70f),
                    LandZone = new Vector3(424.28f, -255.60f, 473.30f),
                    NodeId = 35341,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(419.82f, -254.78f, 467.71f),
                    LandZone = new Vector3(417.98f, -255.60f, 468.30f),
                    NodeId = 35342,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(340.18f, -254.55f, 394.51f),
                    LandZone = new Vector3(340.05f, -255.60f, 396.29f),
                    NodeId = 35343,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(333.97f, -254.57f, 392.74f),
                    LandZone = new Vector3(333.27f, -255.61f, 394.98f),
                    NodeId = 35344,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(307.02f, -254.62f, 454.27f),
                    LandZone = new Vector3(307.38f, -255.60f, 452.87f),
                    NodeId = 35339,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(309.92f, -254.68f, 459.90f),
                    LandZone = new Vector3(311.28f, -255.60f, 459.96f),
                    NodeId = 35340,
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
            [new Vector2(-561f, -658f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-583.87f, -3.61f, -665.30f),
                    LandZone = new Vector3(-582.41f, -4.29f, -665.27f),
                    NodeId = 35449,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-574.71f, -2.62f, -654.34f),
                    LandZone = new Vector3(-575.60f, -3.33f, -655.43f),
                    NodeId = 35450,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-562.63f, -2.94f, -665.23f),
                    LandZone = new Vector3(-563.36f, -3.97f, -665.80f),
                    NodeId = 35448,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-557.39f, -3.74f, -652.86f),
                    LandZone = new Vector3(-557.67f, -4.48f, -653.98f),
                    NodeId = 35451,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-543.34f, -3.81f, -660.56f),
                    LandZone = new Vector3(-543.44f, -4.94f, -661.23f),
                    NodeId = 35447,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-538.54f, -4.12f, -649.13f),
                    LandZone = new Vector3(-538.56f, -5.00f, -649.76f),
                    NodeId = 35452,
                },
            },
            [new Vector2(-333f, 560f)] = new()
            {
            },
            [new Vector2(-326f, -616f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-302.15f, 21.02f, -684.67f),
                    LandZone = new Vector3(-301.70f, 20.20f, -682.84f),
                    NodeId = 35392,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-328.16f, 14.10f, -676.55f),
                    LandZone = new Vector3(-327.71f, 13.26f, -674.97f),
                    NodeId = 35391,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-350.45f, -1.84f, -572.52f),
                    LandZone = new Vector3(-348.32f, -2.18f, -571.86f),
                    NodeId = 35393,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-354.05f, -3.17f, -565.04f),
                    LandZone = new Vector3(-352.18f, -3.42f, -566.22f),
                    NodeId = 35394,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-292.54f, 12.38f, -575.24f),
                    LandZone = new Vector3(-294.61f, 11.06f, -575.14f),
                    NodeId = 35389,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-297.00f, 12.35f, -580.00f),
                    LandZone = new Vector3(-299.07f, 10.49f, -579.03f),
                    NodeId = 35390,
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
            [new Vector2(-88f, -312f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-40.89f, 20.55f, -347.15f),
                    LandZone = new Vector3(-41.35f, 19.80f, -345.97f),
                    NodeId = 35370,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-36.83f, 19.65f, -344.87f),
                    LandZone = new Vector3(-37.50f, 18.97f, -344.46f),
                    NodeId = 35369,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-83.85f, 20.00f, -253.83f),
                    LandZone = new Vector3(-82.53f, 19.59f, -254.87f),
                    NodeId = 35374,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-83.73f, 19.92f, -263.66f),
                    LandZone = new Vector3(-83.21f, 19.42f, -265.17f),
                    NodeId = 35373,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-134.14f, 11.05f, -334.28f),
                    LandZone = new Vector3(-132.80f, 11.62f, -335.30f),
                    NodeId = 35372,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-136.79f, 11.41f, -340.57f),
                    LandZone = new Vector3(-135.64f, 11.17f, -340.40f),
                    NodeId = 35371,
                },
            },
            [new Vector2(-57f, 116f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(-74.00f, 2.70f, 167.99f),
                    LandZone = new Vector3(-76.39f, 1.79f, 167.83f),
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
                    Position = new Vector3(86.33f, 12.51f, 612.67f),
                    LandZone = new Vector3(86.06f, 12.00f, 614.50f),
                    NodeId = 35424,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(90.81f, 12.54f, 615.77f),
                    LandZone = new Vector3(89.45f, 12.00f, 617.27f),
                    NodeId = 35423,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(22.84f, 27.59f, 695.38f),
                    LandZone = new Vector3(23.88f, 26.97f, 693.00f),
                    NodeId = 35427,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(15.62f, 27.44f, 692.80f),
                    LandZone = new Vector3(15.44f, 27.05f, 690.44f),
                    NodeId = 35428,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(-1.78f, 27.72f, 618.93f),
                    LandZone = new Vector3(-0.38f, 26.96f, 620.82f),
                    NodeId = 35426,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(1.48f, 27.69f, 616.74f),
                    LandZone = new Vector3(2.72f, 26.87f, 618.28f),
                    NodeId = 35425,
                },
            },
            [new Vector2(66f, -368f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(41.48f, 12.73f, -359.44f),
                    LandZone = new Vector3(41.84f, 12.39f, -359.74f),
                    NodeId = 35377,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(51.21f, 17.04f, -376.71f),
                    LandZone = new Vector3(51.09f, 16.46f, -376.31f),
                    NodeId = 35376,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(58.82f, 19.40f, -384.76f),
                    LandZone = new Vector3(59.48f, 19.05f, -384.91f),
                    NodeId = 35378,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(77.15f, 24.94f, -394.85f),
                    LandZone = new Vector3(77.22f, 24.28f, -394.78f),
                    NodeId = 35382,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(97.24f, 25.93f, -380.47f),
                    LandZone = new Vector3(97.27f, 25.27f, -380.52f),
                    NodeId = 35375,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(87.94f, 22.49f, -378.19f),
                    LandZone = new Vector3(87.94f, 21.98f, -378.49f),
                    NodeId = 35381,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(82.01f, 18.62f, -366.24f),
                    LandZone = new Vector3(81.65f, 18.28f, -366.77f),
                    NodeId = 35380,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(70.37f, 14.61f, -343.78f),
                    LandZone = new Vector3(70.26f, 13.74f, -343.80f),
                    NodeId = 35379,
                },
            },
            [new Vector2(143f, -182f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(124.87f, 67.42f, -185.22f),
                    LandZone = new Vector3(125.30f, 66.15f, -185.51f),
                    NodeId = 35444,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(128.02f, 68.75f, -186.39f),
                    LandZone = new Vector3(128.43f, 67.53f, -186.89f),
                    NodeId = 35443,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(146.33f, 79.30f, -187.32f),
                    LandZone = new Vector3(146.67f, 78.41f, -188.78f),
                    NodeId = 35442,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(155.69f, 82.63f, -187.68f),
                    LandZone = new Vector3(155.48f, 81.90f, -188.24f),
                    NodeId = 35441,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(152.27f, 87.93f, -173.21f),
                    LandZone = new Vector3(153.36f, 87.05f, -173.02f),
                    NodeId = 35446,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(148.69f, 88.16f, -177.27f),
                    LandZone = new Vector3(148.86f, 87.49f, -175.82f),
                    NodeId = 35445,
                },
            },
            [new Vector2(152f, 287f)] = new()
            {
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
                    LandZone = new Vector3(349.24f, 16.95f, 76.19f),
                    NodeId = 35399,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(353.68f, 18.39f, 87.36f),
                    LandZone = new Vector3(353.18f, 17.45f, 85.23f),
                    NodeId = 35400,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(267.24f, 1.82f, 23.99f),
                    LandZone = new Vector3(268.72f, 0.74f, 25.29f),
                    NodeId = 35395,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(268.73f, 2.14f, 18.46f),
                    LandZone = new Vector3(270.55f, 1.41f, 18.37f),
                    NodeId = 35396,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(307.85f, 17.38f, -48.76f),
                    LandZone = new Vector3(307.70f, 15.08f, -47.02f),
                    NodeId = 35398,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(314.60f, 16.92f, -48.69f),
                    LandZone = new Vector3(314.10f, 14.64f, -47.12f),
                    NodeId = 35397,
                },
            },
            [new Vector2(648f, 434f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(714.03f, -235.90f, 435.24f),
                    LandZone = new Vector3(712.81f, -236.84f, 435.63f),
                    NodeId = 35430,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(707.92f, -237.20f, 420.86f),
                    LandZone = new Vector3(706.65f, -238.33f, 420.42f),
                    NodeId = 35429,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(653.91f, -241.41f, 380.82f),
                    LandZone = new Vector3(653.30f, -242.03f, 381.90f),
                    NodeId = 35432,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(650.98f, -241.68f, 377.91f),
                    LandZone = new Vector3(649.69f, -242.52f, 378.70f),
                    NodeId = 35431,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(595.00f, -248.66f, 462.41f),
                    LandZone = new Vector3(596.58f, -248.94f, 461.94f),
                    NodeId = 35433,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(592.07f, -249.05f, 471.72f),
                    LandZone = new Vector3(593.78f, -249.96f, 472.06f),
                    NodeId = 35434,
                },
            },
            [new Vector2(724f, -319f)] = new()
            {
                new GathNodeInfo()
                {
                    Position = new Vector3(784.18f, 53.69f, -286.61f),
                    LandZone = new Vector3(783.79f, 52.96f, -288.24f),
                    NodeId = 35388,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(786.91f, 54.43f, -287.83f),
                    LandZone = new Vector3(786.78f, 53.92f, -289.22f),
                    NodeId = 35387,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(720.00f, 50.29f, -370.76f),
                    LandZone = new Vector3(720.79f, 49.80f, -369.55f),
                    NodeId = 35385,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(717.10f, 50.20f, -372.52f),
                    LandZone = new Vector3(716.40f, 49.40f, -371.21f),
                    NodeId = 35386,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(646.58f, 54.64f, -315.07f),
                    LandZone = new Vector3(647.23f, 53.53f, -316.22f),
                    NodeId = 35384,
                },
                new GathNodeInfo()
                {
                    Position = new Vector3(649.06f, 55.04f, -314.26f),
                    LandZone = new Vector3(649.95f, 53.96f, -314.97f),
                    NodeId = 35383,
                },
            },
        },
    };

    public class FishingTools
    {
        public List<string> FishingPreset = new List<string>();
        public int AmountRequired = 0;
        public Dictionary<string, List<uint>> Baits = new();
        public Dictionary<string, List<uint>> RequiredFish = new();
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
        // Export for Mission [508] - EX: Processed Aquatic Metals
        [508] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACtVXXXObOBT9Kx49ww7CAgNvjpukmbHTTHFmHzr7IOCCNcHIkUQaN+P/viM+bMB2s+142u6budI9OudIV1d+Q9NS8RmVSs7SDAVv6LqgUQ7TPEeBEiUYSA/OWQF68C4ruIAF5/EKBSnNJRhNQtJOv0tQYHu+gR4E44KpLQqwge7k9Wuclwkkh7Cev6vxG8Q3VP2wq6U0jusZ6HazXAmQK54nKMCW1UP+PnSF4U96Gda7ZGarcn1woieMYIsMGHkDRi0Iz3OI1Xkc3M2y3yfFRcJofgYP267bs5w0aTdMrq63IFtHCbacgQDH6Qlw2y2hTxCuWKquKKtk6IBsA6Gi8ZNEgdOY7HrHuF1Uv0F9oIpBEUOHjzvMc/uG2m2qYN9gRlV9UE6dOtc7AvO8Pti4AVuuaM7ok7yhL1xovF6gVTc2+vHPEPMXECjA2rMzFEhvwdbOK5bd0nWle1pkOQjZLqL3PkHBeGKRIXu7B+Xtdga6flWC9kp1T0Dvy5KHX+nmrlAlU4wXt5QVrdUmNtC8FLAAKWkGKEDIQPcVJ3TPC0ANwnYDKNA+ncCbc6l+Gu9BgITTDJGJzozXK1bjBz7hBmIlaD4rhYBCXUjlAPViWk+yPVJ8cvVq1g0XMVRF95Vu2s2uD1Go+EaXOCuyUMGmumoPgpqDNhWX0dGFq4g9Fuy5BI2LiGUR4sXETBIHTOLB2PTiyDNh4tru2Ce+gwnaGWjOpPqU6jUkCr68VatpAXuCvn+e4TTPR3XqkOY9F2uaf+T8SQO1l8zfQKtvHZeg9uXSa1zNYNfbJlTDEzzRl1eLGSrBi+wCqNa4gzqHDIqEiu2hrM8gLNkaxKDwF6zYD+kL8y/LQLqJnZqr4/35rp5+itwHXkb5eyb2Em3X3+e9a9TZzJ4ZZ5IX9LUr+UhCjfcoYSnYpjazdaCO/JCsiWNrQ+rMHxTWyz2S9iu2s6WgL4tpqkDMaJmt1JytdWPH9cDwFqlefaWoXw76R6cn1u3K8YdPoe++rfRzrL3U20L9DM8lE5CEiqpSvyb0e+9/Xr2XAP6TK+9E8h9VUf81/bdXw+Ohe44jgiMX2yaJ49QkjhWZUWRhM3U8O47tyCZWgnb/tO2z+bf0ZR+oO6j+rvt10y+vwsXIHD0IHoOUkIymzyVVLB4tQNFcjszRR6Av29FU0VemdO/vUMIxjhI7dkyM08gkvodNL0rATCI7Sid4TFPsod2/sM5WQT8OAAA=",
            },
            AmountRequired = 0,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },

        // Export for Mission [509] - EX: Refined Moon Gel
        [509] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1X227bOBD9FYPPIqALqdub66ZpACcbxFnsAkEfKHFkE5FFl6K6zQb+9wV1sSVHttHAD8UibzI5c+bM6Ghm/IqmlZYzVupyli1R/IquCpbkMM1zFGtVgYXM5VwUsL/k3dUNR7EbRha6V0IqoV9Q7Fjoprz6meYVB74/NvbbButWynRlwOoH1zzVOH5ooevN40pBuZI5R7Fj2wPk09A1RhQMPOyzZGaran0kMeLY5AyjDkTmOaS6y4Q4ttM3c8+zkIoLlh8h4ri+P6gxad2+iHJ19QJlLzA9YEzpgLHfvQP2DIuVyPQnJmre5qDsDhaapc8limlbVT98i9tHjVrUe6YFFCn0+PiHfv6wgm7nqsS/MGO6UUYX9dDbPai/13o/rlgu2HP5hf2QygAMDrp0PGt4/gCp/AEKxY4p0pi0/dBIoBewq98nsbxm6zrRabHMQZVdEPOyOYq9wCZv2A+gwu3WQlc/tWLth2cq/ygX/7DNTaEroYUsrpkounpgx0LzSsEtlCVbAooRstBdTQLdyQKQ1SC8bADFpjAjeHNZ6nfj3SsoYZwhwujIfROxvt/zWWwg1Yrls0opKPSFsjxAvViuo2zfZDwavbZqBLLQcmO+V1EsFxo2daPcc29FNFWXodyHqzn8WYjvFRhcRG3qJdyJcJLRCBOPJJg5xMGOT93UjYgbcB9tLTQXpf4jMzFKFD818jQJ7AhG0XGG0zyfNK6HNO+kWrP8q5TPBqjrGH8Bq3+b8xL07lvMWF5C9222lybB7ittjxp44gSmE3WYC61k0Rtp591tr+c+hyUUnKmXC/CqgT/LKskPM20sXD/aGexpHzUZo9a3elRicyxSQF1vZ3Is1sDoRLTWzmh7mmlQM1YtV3ou1maoOM3FoejrdaJSzdQyD732PNKDvYBGb8fwiYlqVoGu+3Qye4DvlVDAF5rpygw2s2sc0d4ZLf2qZD4k8GsSeO8773W4LAt8z3ETnJKQYwLMx0kUZdjnJCCe65HIDtD2W9fi2n30aXfQdLmnV9Rvd4RGrnu84X2Vm40olpN5VSwz49bve86pAt1wKLRIWW6qYqI1BtO1rIqe2diCSqPDFcMbrnuhCVypjKWwyE132uVCz2xWdGuh32ZR38/Jd09H42xOZqaqdUH787KdkuaxOd6bjem3p7XIjmzikQAzygJMPEhw6BMbpxC6vgseTUmKttZbLZ1I4FbKIhd6MmNqc3EdvVs44wL80NFldORxAqGd2ZjzyMfEsTPMUkox9zh1nNCB1LXHdOScWMLmVcHU5IEJtmbDRexDSP9bIfGUE+4TH4euzzCJKODE5h72sjRMvIDzDMJ6+DW4LcUnakffJld/x5MHyEQBfGKa0OQa8uGfB5cy14syD/uBTTBxuWl3SYizEGhAw8DOsgBt/wPyisUyVRIAAA==",
            },
            AmountRequired = 0,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
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

    public static Dictionary<string, List<uint>> MoonBaits = new();
    public static Dictionary<string, List<uint>> MoonFish = new();
}
