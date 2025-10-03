using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
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
        public float RotationTolerance { get; set; } = 0.1f;
    }

    public static Dictionary<uint, Dictionary<Vector2, List<FisherSpotInfo>>> MoonFishingLocations = new()
    {
        [1237] = new()
        {
            [new Vector2(-673f, 497f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-685.48f, 57.15f, 448.15f),
                    FishingSpot = new Vector3(-684.29f, 57.14f, 449.44f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-683.17f, 56.18f, 410.53f),
                    FishingSpot = new Vector3(-681.86f, 56.15f, 409.36f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-714.26f, 57.07f, 400.39f),
                    FishingSpot = new Vector3(-715.62f, 57.07f, 398.90f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-718.85f, 57.17f, 429.20f),
                    FishingSpot = new Vector3(-720.61f, 57.15f, 429.68f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-710.78f, 57.15f, 450.46f),
                    FishingSpot = new Vector3(-711.97f, 57.13f, 451.87f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-744.14f, 69.99f, 511.90f),
                    FishingSpot = new Vector3(-742.36f, 69.99f, 511.54f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-752.28f, 70.25f, 497.35f),
                    FishingSpot = new Vector3(-750.49f, 70.21f, 496.88f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-766.31f, 72.83f, 495.21f),
                    FishingSpot = new Vector3(-766.88f, 72.81f, 493.54f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-785.76f, 75.98f, 511.21f),
                    FishingSpot = new Vector3(-787.50f, 75.94f, 511.36f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-779.33f, 73.05f, 531.23f),
                    FishingSpot = new Vector3(-780.99f, 73.10f, 532.39f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-754.05f, 70.47f, 534.56f),
                    FishingSpot = new Vector3(-754.94f, 70.47f, 536.35f),
                },
            },
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
                    FacePosition = new Vector3(-331.19f, 47.71f, 605.12f),
                    FishingSpot = new Vector3(-329.34f, 47.71f, 605.86f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-324.45f, 47.63f, 581.66f),
                    FishingSpot = new Vector3(-322.90f, 47.62f, 580.78f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-347.86f, 47.65f, 566.97f),
                    FishingSpot = new Vector3(-347.55f, 47.62f, 565.18f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-370.45f, 47.68f, 579.07f),
                    FishingSpot = new Vector3(-371.96f, 47.67f, 577.85f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-368.28f, 46.99f, 610.29f),
                    FishingSpot = new Vector3(-370.08f, 46.95f, 610.97f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-357.57f, 47.82f, 635.41f),
                    FishingSpot = new Vector3(-358.79f, 47.78f, 636.85f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-336.68f, 47.69f, 642.65f),
                    FishingSpot = new Vector3(-336.22f, 47.66f, 644.39f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-324.19f, 47.63f, 630.32f),
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
                    FacePosition = new Vector3(-127.01f, 19.50f, -282.83f),
                    FishingSpot = new Vector3(-125.02f, 19.49f, -282.55f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-122.23f, 21.01f, -297.40f),
                    FishingSpot = new Vector3(-120.34f, 21.01f, -296.72f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-134.13f, 21.89f, -305.78f),
                    FishingSpot = new Vector3(-133.89f, 21.89f, -307.83f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-145.22f, 21.36f, -299.16f),
                    FishingSpot = new Vector3(-147.11f, 21.38f, -300.01f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-151.91f, 20.48f, -276.35f),
                    FishingSpot = new Vector3(-153.78f, 20.48f, -275.65f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-140.59f, 20.73f, -265.49f),
                    FishingSpot = new Vector3(-140.35f, 20.74f, -263.50f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-128.37f, 19.81f, -268.13f),
                    FishingSpot = new Vector3(-126.56f, 19.80f, -267.38f),
                },
            },
            [new Vector2(104f, -269f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(81.29f, 18.29f, -250.49f),
                    FishingSpot = new Vector3(79.81f, 18.34f, -249.27f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(92.09f, 17.72f, -231.19f),
                    FishingSpot = new Vector3(90.35f, 17.72f, -230.32f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(109.53f, 18.80f, -231.42f),
                    FishingSpot = new Vector3(110.04f, 18.81f, -229.60f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(145.64f, 24.61f, -258.82f),
                    FishingSpot = new Vector3(147.53f, 24.62f, -258.54f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(142.12f, 28.26f, -277.96f),
                    FishingSpot = new Vector3(143.39f, 28.23f, -279.23f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(122.60f, 28.83f, -292.25f),
                    FishingSpot = new Vector3(124.30f, 28.83f, -293.16f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(96.34f, 29.02f, -295.20f),
                    FishingSpot = new Vector3(95.97f, 29.03f, -297.06f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(67.54f, 25.26f, -291.20f),
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
                    FacePosition = new Vector3(916.68f, -58.34f, -328.82f),
                    FishingSpot = new Vector3(914.87f, -58.35f, -328.79f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(920.33f, -58.86f, -336.27f),
                    FishingSpot = new Vector3(918.65f, -58.90f, -337.07f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(935.54f, -57.68f, -355.58f),
                    FishingSpot = new Vector3(934.37f, -57.65f, -356.92f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(938.19f, -56.75f, -360.04f),
                    FishingSpot = new Vector3(936.45f, -56.75f, -360.90f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(941.39f, -55.70f, -364.66f),
                    FishingSpot = new Vector3(940.42f, -55.71f, -366.29f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(944.47f, -54.79f, -367.15f),
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
                    FacePosition = new Vector3(-682.75f, -3.97f, -653.59f),
                    FishingSpot = new Vector3(-680.97f, -4.06f, -653.59f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-686.68f, -3.82f, -661.86f),
                    FishingSpot = new Vector3(-685.18f, -3.94f, -662.78f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-695.94f, -3.85f, -666.93f),
                    FishingSpot = new Vector3(-695.36f, -3.98f, -668.60f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-709.43f, -3.91f, -664.23f),
                    FishingSpot = new Vector3(-710.34f, -3.99f, -665.82f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-715.24f, -3.87f, -651.65f),
                    FishingSpot = new Vector3(-717.04f, -3.96f, -651.28f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-710.10f, -3.98f, -638.81f),
                    FishingSpot = new Vector3(-711.18f, -4.07f, -637.41f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-695.71f, -4.07f, -635.30f),
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
                    FacePosition = new Vector3(-453.06f, -9.17f, -673.84f),
                    FishingSpot = new Vector3(-454.69f, -9.23f, -673.17f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-450.95f, -9.19f, -663.79f),
                    FishingSpot = new Vector3(-452.35f, -9.21f, -662.55f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-445.48f, -9.10f, -661.14f),
                    FishingSpot = new Vector3(-444.48f, -9.18f, -659.68f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-442.20f, -9.78f, -667.60f),
                    FishingSpot = new Vector3(-440.41f, -9.82f, -666.91f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-441.65f, -8.79f, -674.87f),
                    FishingSpot = new Vector3(-439.83f, -8.77f, -674.59f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-447.86f, -9.56f, -684.91f),
                    FishingSpot = new Vector3(-446.96f, -9.69f, -686.45f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-457.66f, -8.34f, -689.08f),
                    FishingSpot = new Vector3(-457.68f, -8.38f, -690.98f),
                },
            },
            // Export for Fishing Zone 1291, Flag (-252, -74)
            [new Vector2(-252f, -74f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-250.39f, -10.20f, -76.12f),
                    FishingSpot = new Vector3(-249.49f, -10.25f, -77.68f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-240.21f, -11.59f, -76.41f),
                    FishingSpot = new Vector3(-240.33f, -11.62f, -78.31f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-231.32f, -11.42f, -75.66f),
                    FishingSpot = new Vector3(-230.43f, -11.45f, -77.20f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-232.67f, -11.04f, -73.56f),
                    FishingSpot = new Vector3(-231.23f, -11.10f, -72.50f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-238.98f, -11.15f, -70.22f),
                    FishingSpot = new Vector3(-238.98f, -11.25f, -68.43f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-245.00f, -11.12f, -68.31f),
                    FishingSpot = new Vector3(-243.54f, -11.18f, -67.34f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-252.52f, -10.07f, -65.02f),
                    FishingSpot = new Vector3(-252.52f, -10.07f, -63.13f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-259.07f, -9.85f, -66.64f),
                    FishingSpot = new Vector3(-260.30f, -9.90f, -65.25f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-269.25f, -10.80f, -75.05f),
                    FishingSpot = new Vector3(-270.98f, -10.83f, -74.62f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-265.35f, -10.26f, -77.56f),
                    FishingSpot = new Vector3(-266.49f, -10.29f, -78.99f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-257.34f, -11.07f, -81.12f),
                    FishingSpot = new Vector3(-257.52f, -11.08f, -82.96f),
                },
            },
            // Export for Fishing Zone 1291, Flag (-239, -352)
            [new Vector2(-239f, -352f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-239.01f, -6.54f, -353.85f),
                    FishingSpot = new Vector3(-239.26f, -6.63f, -355.69f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-232.77f, -7.04f, -357.39f),
                    FishingSpot = new Vector3(-234.13f, -7.05f, -358.69f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-219.62f, -4.49f, -360.75f),
                    FishingSpot = new Vector3(-218.18f, -4.50f, -361.86f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-222.11f, -7.16f, -353.34f),
                    FishingSpot = new Vector3(-220.26f, -7.21f, -353.64f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-219.93f, -6.95f, -345.15f),
                    FishingSpot = new Vector3(-218.17f, -6.95f, -345.71f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-215.17f, -7.55f, -335.11f),
                    FishingSpot = new Vector3(-213.56f, -7.57f, -334.14f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-223.81f, -9.02f, -332.62f),
                    FishingSpot = new Vector3(-224.87f, -9.06f, -331.05f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-227.75f, -6.76f, -345.32f),
                    FishingSpot = new Vector3(-228.63f, -6.71f, -343.76f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-239.19f, -8.74f, -342.49f),
                    FishingSpot = new Vector3(-239.12f, -8.75f, -340.62f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-245.71f, -9.11f, -347.48f),
                    FishingSpot = new Vector3(-246.44f, -9.14f, -345.84f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-253.36f, -9.66f, -350.12f),
                    FishingSpot = new Vector3(-255.05f, -9.66f, -349.50f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-259.28f, -8.49f, -358.00f),
                    FishingSpot = new Vector3(-260.39f, -8.70f, -359.37f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(-250.44f, -6.84f, -355.39f),
                    FishingSpot = new Vector3(-250.22f, -6.85f, -357.14f),
                },
            },
            // Export for Fishing Zone 1291, Flag (28, 99)
            [new Vector2(28f, 99f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(4.38f, -12.41f, 65.35f),
                    FishingSpot = new Vector3(2.85f, -12.51f, 64.48f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(9.18f, -14.01f, 66.33f),
                    FishingSpot = new Vector3(10.25f, -14.11f, 64.94f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(16.86f, -12.19f, 75.01f),
                    FishingSpot = new Vector3(18.04f, -12.19f, 73.60f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(26.57f, -12.28f, 78.14f),
                    FishingSpot = new Vector3(27.43f, -12.32f, 76.54f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(30.68f, -12.17f, 83.49f),
                    FishingSpot = new Vector3(32.59f, -12.18f, 83.39f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(38.61f, -12.68f, 99.73f),
                    FishingSpot = new Vector3(40.23f, -12.72f, 98.95f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(42.75f, -11.42f, 110.16f),
                    FishingSpot = new Vector3(43.90f, -11.44f, 108.78f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(48.54f, -12.61f, 120.29f),
                    FishingSpot = new Vector3(50.35f, -12.65f, 120.35f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(59.60f, -12.07f, 132.49f),
                    FishingSpot = new Vector3(60.76f, -12.09f, 131.02f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(62.03f, -12.27f, 138.09f),
                    FishingSpot = new Vector3(63.89f, -12.32f, 138.41f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(49.03f, -13.73f, 137.06f),
                    FishingSpot = new Vector3(48.68f, -13.80f, 138.88f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(32.23f, -13.47f, 127.90f),
                    FishingSpot = new Vector3(32.15f, -13.68f, 129.67f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(22.37f, -12.88f, 121.08f),
                    FishingSpot = new Vector3(20.85f, -13.00f, 121.99f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(16.11f, -12.12f, 109.93f),
                    FishingSpot = new Vector3(14.83f, -12.12f, 111.30f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(11.28f, -12.08f, 101.84f),
                    FishingSpot = new Vector3(9.58f, -12.16f, 101.33f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(20.85f, -10.47f, 89.04f),
                    FishingSpot = new Vector3(19.14f, -10.52f, 89.53f),
                },
            },
            // Export for Fishing Zone 1291, Flag (46, -344)
            [new Vector2(46f, -344f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(41.02f, 9.81f, -341.12f),
                    FishingSpot = new Vector3(41.51f, 9.79f, -339.41f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(32.40f, 7.93f, -331.17f),
                    FishingSpot = new Vector3(31.79f, 7.84f, -329.47f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(35.20f, 8.40f, -337.68f),
                    FishingSpot = new Vector3(33.50f, 8.40f, -338.15f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(39.32f, 8.84f, -345.02f),
                    FishingSpot = new Vector3(38.50f, 8.76f, -346.58f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(52.08f, 12.01f, -353.86f),
                    FishingSpot = new Vector3(50.84f, 11.86f, -355.20f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(58.34f, 11.73f, -349.74f),
                    FishingSpot = new Vector3(60.05f, 11.59f, -350.42f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(54.53f, 10.39f, -342.30f),
                    FishingSpot = new Vector3(55.84f, 10.34f, -341.09f),
                },
            },
            // Export for Fishing Zone 1291, Flag (214, -742)
            [new Vector2(214f, -742f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(217.65f, 133.63f, -748.86f),
                    FishingSpot = new Vector3(217.51f, 133.57f, -750.65f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(211.36f, 133.82f, -743.07f),
                    FishingSpot = new Vector3(209.77f, 133.82f, -744.00f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(205.07f, 133.68f, -736.70f),
                    FishingSpot = new Vector3(205.18f, 133.59f, -738.54f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(205.86f, 133.71f, -731.38f),
                    FishingSpot = new Vector3(206.03f, 133.68f, -729.59f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(217.06f, 133.43f, -733.05f),
                    FishingSpot = new Vector3(218.78f, 133.40f, -732.39f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(216.22f, 133.74f, -740.91f),
                    FishingSpot = new Vector3(217.94f, 133.71f, -740.18f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(222.46f, 133.68f, -748.29f),
                    FishingSpot = new Vector3(224.13f, 133.67f, -747.62f),
                },
            },
            // Export for Fishing Zone 1291, Flag (462, -47)
            [new Vector2(462f, -47f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(528.20f, 1.61f, 7.02f),
                    FishingSpot = new Vector3(526.37f, 1.46f, 6.78f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(519.13f, 3.82f, -8.26f),
                    FishingSpot = new Vector3(517.66f, 3.72f, -7.25f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(531.90f, 2.14f, 15.45f),
                    FishingSpot = new Vector3(532.72f, 2.09f, 17.03f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(530.45f, 3.97f, -3.05f),
                    FishingSpot = new Vector3(530.28f, 3.94f, -4.81f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(543.67f, 4.18f, -10.15f),
                    FishingSpot = new Vector3(545.12f, 4.14f, -11.22f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(538.89f, 2.93f, 2.09f),
                    FishingSpot = new Vector3(540.22f, 2.90f, 3.38f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(390.65f, 28.77f, -81.59f),
                    FishingSpot = new Vector3(392.52f, 28.75f, -82.02f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(390.79f, 26.54f, -71.23f),
                    FishingSpot = new Vector3(392.21f, 26.54f, -70.02f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(380.76f, 25.43f, -67.89f),
                    FishingSpot = new Vector3(380.79f, 25.43f, -65.96f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(377.05f, 26.12f, -72.07f),
                    FishingSpot = new Vector3(375.33f, 26.09f, -72.63f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(382.83f, 27.37f, -76.90f),
                    FishingSpot = new Vector3(381.41f, 27.31f, -78.12f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(387.62f, 29.30f, -84.06f),
                    FishingSpot = new Vector3(386.65f, 29.25f, -85.66f),
                },
            },
            // Export for Fishing Zone 1291, Flag (526, 448)
            [new Vector2(526f, 448f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(522.33f, -255.60f, 453.09f),
                    FishingSpot = new Vector3(521.07f, -255.60f, 454.64f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(511.24f, -255.60f, 429.31f),
                    FishingSpot = new Vector3(509.94f, -255.60f, 430.82f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(528.40f, -255.60f, 415.81f),
                    FishingSpot = new Vector3(526.90f, -255.60f, 417.13f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(518.11f, -255.60f, 405.44f),
                    FishingSpot = new Vector3(519.26f, -255.60f, 407.08f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(534.20f, -255.60f, 397.19f),
                    FishingSpot = new Vector3(534.44f, -255.60f, 395.21f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(546.96f, -255.60f, 405.64f),
                    FishingSpot = new Vector3(548.01f, -255.60f, 403.94f),
                },
            },
            [new Vector2(562f, 580f)] = new()
            {
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(561.77f, -252.05f, 562.40f),
                    FishingSpot = new Vector3(562.51f, -252.00f, 560.31f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(555.81f, -250.92f, 559.76f),
                    FishingSpot = new Vector3(556.61f, -250.92f, 557.92f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(551.05f, -249.66f, 557.31f),
                    FishingSpot = new Vector3(552.00f, -249.62f, 555.52f),
                },
                new FisherSpotInfo()
                {
                    FacePosition = new Vector3(567.62f, -254.23f, 564.65f),
                    FishingSpot = new Vector3(568.43f, -254.23f, 562.83f),
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
        public bool UniqueFish = false;
        public Dictionary<string, List<uint>> Baits = new();
        public Dictionary<string, List<uint>> RequiredFish = new();
    }

    public static Dictionary<uint, FishingTools> FishingPreset = new()
    {
        // D Rank

        // Export for Mission [451] -  Lunch Emergency
        [451] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1WS2/jOAz+K4XONuCH5NctzaadAmmnmHSxh2IOjE0nQhwrI8vTyRT57wvZVmKnSTsdFIs9zE2gyI8fKYrkMxnVSoyhUtU4X5DkmUxKmBc4KgqSKFmjRfTllJd4uMzM1U1GEi+KLXIvuZBcbUniWuSmmvxIizrD7CDW+rsW61aIdKnBmoOnTw1OEFnkevOwlFgtRZGRxHWcAfLr0A1GHA4snDfJjJf1+kxg1HXoG4wMiCgKTJWJhLqO21fz3mYhZMahMACBSwcAtFO74tVyssWq54gdMWRswDAwOYcVzpY8V5fAG55aUBnBTEG6qkjCuiwG0UvcPmrcod6D4lim2OMTHNsFw4x5xlTynzgG1VaC8Xps7R3l2++sH5ZQcFhVV/BdSA0wEJhwfGso/4Kp+I6SJK5OkvFJBx5Mwi754hrWTWSjclGgrAyqfs2MJH7o0Bd0B1DRbmeRyQ8lYfCz9jWmM/8gZk+wuSlVzRUX5TXw0uTDdi0yrSXeYlXBAklCiEXuGk7kTpRIOoTtBkmiE3MCbyoq9dt49xIrPM2Q2OTMfeuxuT/wmW0wVRKKcS0lluqDojxC/bBYT7J9EfFJ743WlZApNt/qCTbmsRthpqXNR2ExDa2usmZKbPTP5uVipnDTtNBDlF31jeTHBNeHa9j+XfJvNWpc4gQsyDygNpvHmU1p5tgxQG6nUYoUAiePPZfsLDLllfqcax8VSR6fG286gH0bYDGNznO8FaK8uISi0Fh3Qq6h+CTESlubhvIPwurwY/RthYN0dqI2RuqGuiMZ45mSoly8x9zxe+ZTXGCZgdy+G+EvUc+LPfeBhhfEe4UDv7MqAw4ntB4k35zzFDLP36uc8zVQesVbp6dLdJQrlGOoF0s15Ws9Rdz24rh2m32hlu2Y0odeP247J4tfDtZXZqQe7qa/mEr5gt9qLjGbKVC1Hl16ezgun1+rkl8uhj9v/p++ea8zBeCmIUapHQMNbOp4aM/dMLIj1w/mcUZZECPZfTWtqdswH/eCtjs9PpN+m3p3Hz2bkZsMS8VTKHQaNHyrMFqLuhyoNb3xeGvwhytbpD3VMocUZ4VuOPumyt7YjtjOIv+b5fowwX4/30+w0ZKxTmOTwf4k6+aXPrbig9qpCu1VU8giABbkNmM52NSPqB3TOdhuRCFklNIoC5tqanE7io+UuV8vpnWZLi8ma5QLLNPtcICGaRwCi3N7HrLIplmIduzGno1+4LOMprFLfbL7F1DkJiNzDQAA",
            },
            AmountRequired = 5,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Astacus Lamentorum"] = new List<uint>()
                {
                    45693,
                    45705,
                    45715,
                    45727,
                    45744,
                    45826,
                    45847,
                },
                ["Lunar Blue Guppy"] = new List<uint>()
                {
                    45695,
                },
                ["Lunar Tilapia"] = new List<uint>()
                {
                    45694,
                },
            },
        },
        // Export for Mission [452] -  Large Aquatic Specimens
        [452] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACs1VyW7jMAz9lYJnG7BjSV5uaZAWBdJOMemcih4Um46FOpYryV2myL8P5NiTOMsEKHqYm0CRj49PJPUJ48bICddGT/IlJJ8wrfiixHFZQmJUgw7Yy5mocHuZ9Vc3GSSjKHbgXgmphPmAxHfgRk/f07LJMNuarf96g3UrZVpYsPYwsqcWh0UOXNcPhUJdyDKDxPe8AfK/oVuMOBxEeGfJTIpm1TMgvkfOUOijZFlianYC/V230fm0UmWClyck9UeMDUQlXdiV0MX0A/VOYrrHmNIBY9aLzp9xXojcXHLR8rYG3RvmhqfPGhLayciiQ9xd1LhDvedGYJXiDh+2H8eGCo76UCV+44SbTSsc6ysWHYCN9p4j6MAeCl4K/qyv+KtUFm9g6KsLnKH9J6byFRUkvtXsBAUySNjLeSmW13zV1j2uliUq3Sexb59BEoQeOWA/gIrWawem70bxweD9JWDf5UHO33h9U5lGGCGray6qXmrXd2DWKLxFrfkSIQFw4K7lBHeyQugQPmqExOp0BG8mtfky3r1CjccZggsn7jcZ2/stn3mNqVG8nDRKYWW+qco91G+r9Sjbg4qPZm+9rqRKsR26N173j90aM2ttx4jGJHS6zpobWdu5F9VybrBuN+y2yq77xup7ituFa9n+qsRLgxYXqMc9jj5zfQyIS7IodKOYEjeOWB57NOIhD2DtwExo8yO3OTQkj0+9oVv7W4MtCpLHT9gcug1CWcxOF3CLJa/SQpaCD+qwi9kKNc4NqglvloWZiZXddPbTyLAyIuWlnVybaOMwXsmmGri1yu9PbTBcqJHN1Kicpzgv7QMeXV6ExvTM7qJrB/6br3DbUF9uo/kbr61lYlVtBd1trK6d7HFj3roda/CdtguoH1MSLVxklLskCIgbpTFzozjnfkTRj1gK66c+XUfxkdDR08WMqyVejF8abkR6YWdSrLDSw7728yBE3ycuC3zqkiyP3cWChG4cxKM4CkLEjMH6D56DAd8pCQAA",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [453] -  Western Water Inspection
        [453] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACtVXWW/bOBD+KwGfJUAHRR1vrtdJDbjZoMoiD0EfKGlkE5FFlaSaZgP/9wV12JKP2CmCRfsmDznffDOcy69oUis+pVLJab5E0SualTQpYFIUKFKiBgPpwwUrYXeY9UfzDEVOEBroTjAumHpBkW2guZz9TIs6g2wn1vc3LdYXztOVBms+HP3V4JDAQDfV/UqAXPEiQ5FtWSPkt6EbjNAfaVhnyUxX9bpngG0Ln6HQa/GigFSdiAi2LXuo5ZxnwUXGaHECz3YIGcUYd2rXTK5mLyAHDnh7DnjeyAHSvwF9gnjFcvWJssYNLZC9IFY0fZIo8rqokuAQd4gadqh3VDEoUxjwIft6ZBxQp1cV7F+YUtVmxrE0I8EBmLP3Om4Hdr+iBaNP8pr+4ELjjQS9d64xln+FlP8AgSJbx+wEBTwy2IfzE1ve0HXj96RcFiBkb0S/fYYi17fwAfsRVLDZGGj2Uwk6qsMtAf0u9zx+ptW8VDVTjJc3lJV9qE3bQItawBeQki4BRQgZ6LbhhG55CahDeKkARTpOR/AWXKpfxrsTIOE4Q2SiE+etxeZ8xyeuIFWCFtNaCCjVB3m5h/phvh5le+DxUevNrWsuUmiK7plW/WM3wkxLmzLyQuwbXWbFile67lm5jBVUTcPdedll30R8jHNDuIbtPyX7XoPGRbYT+mEOxIQMchNnATETQqmZ5zS1Ej/zIAjRxkALJtXfubYhUfT42ljTDmybROvdKY5TLtdcra7u6opquFsu1rT4zPmTBug7zgPQ5reWS1Db2slpIaEv5u5wGOhO1HqPbV93sh4zVoKXg1I8r265A/UFLKHMqHj5AF4N8F+8Topzno4UHRJu9XbenLxyCeMjyveCVe/k5XuOu9U8xWx06f3cOnVdL5NcgZjSerlSC7bWA89uD/YLqVl1atFOVP0xmBVtG/fCwxXhjfGu95K+2fU5+xW+10xAFiuqaj1l9eJzYSJflq+XKb87W/+HpLxY83fJ1Xfo/rkpPOj6JHQTF/LAzP3QM7FFAjOAIDHtxA3tAFPL9gFtvvVtv9v1H7eCtvM/vqLxCPD1xnx6BCS0UFczKEbDyn4rNPMMSsVSWuh4aDvthcma1+XoWjOA9lczd7w1B9pSLXKaQlzoJr2dXN6ZjdTbGOi3+b+zWxN+eTmIn2mlJVMdxiaCw3WhWxL0ZyveXTuWqoO0wiE4TmgFpuc7nolzTM0QAs90KUlyy3bClJAmrVrcjuIj9txvVw8gFYjy6oEqEFfzUupdi/FyvK5kWZBjz8VmRiA0sZdgM0l8MJ2U4twlYDkBQZv/APBzqN0PDwAA",
            },
            AmountRequired = 2,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Cobalt Eel"] = new List<uint>()
                {
                    45701,
                },
            },
        },
        // Export for Mission [454] -  Aquatic Foodstuffs
        [454] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1WS2+jMBD+K5XPIEEwz1uaTbqV0m60SbWHqgcHhmCFYGqbttkq/31lwEnIo49Vtae9wcz4m2/G489+Rf1KsgERUgzSBYpe0bAg8xz6eY4iySswkHKOaQE7Z6Jd1wmKekFooAmnjFO5RpFtoGsxfInzKoFkZ1bxmwbrhrE4U2D1R0991TheYKCrcpZxEBnLExTZltVBfhu6xgj9zgrrXTKDrFqdKQzbFn6HkQZheQ6x1JVg27L3w3rvs2A8oSTXAJ6NOwC4DRtRkQ3XIPYSuQcMXbfD0NM9J0uYZjSVl4TWPJVBaMNUkngpUOS2XfSCY9x91LBFnRBJoYhhj493uM7rdqynl3L6GwZENpNwaqy84Aisd9B+pwWbZSSnZClG5Ilxhdcx6Ooco2v/CTF7Ao4iW/XsDAXcSajbeUkXV2RV190vFjlwoZOovU5Q5PgWPmLfgQo2GwMNXyQnnXO3JaD2Zcamz6S8LmRFJWXFFaGFbrVpG2hccbgBIcgCUISQgW5rTuiWFYBahHUJKFJ9OoE3ZkL+Nd6Eg4DTDJGJzvibjLV/x2daQiw5yQcV51DIL6ryAPXLaj3J9qjik9nrqBHjMdSH7pmUerNrY6Ks9TFyQ+wb7WRNJSvVuafFYiqhrAV2V2U7fX3+NcXtw9Vs7wr6WIHCRcTziOv5junPA8fEcWiZ8zl4ZooDSKzQn3tOD20MNKZC/khVDoGi+9c6mypgKxJNdec4DphYMZldTKqSKLhbxlck/87YUgFoxfkFZLk7NMoroNPR1tSUiW1fSZZePJWcFYvPLLecveVjWECREL7+NMI3Vs3zLfdORM8LtwE7fmdDOhxORM04Lc9l8t2esw05l6sT9Ea2Nk5NaT+VwAekWmRyTFfqmrEbx+H41g+Kijf3mPrYU+hGPN3w+OZ94xJVt7+WGD0pP+GxohySqSSyUnebel4cjs/HpuTDw/B/z//pnt/txClxHYC565lJgmMT23FszsMYTIKDNPEtwEGI0eZBq1P7BL3fGhqBUv+NHLZidI9d/HDRf6yIpPHFiLFEyCpNRVcZXce3wtDxTPDBMbELthmAG5i+5Qahb4ep56Ro8wdsUj8tagsAAA==",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [455] -  Weeping Pool Ecological Survey
        [455] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1Xy27bOhD9FYNrERBlinrsHN8kDeCkQZ2LLoIuKGlkE5FFl6TSpoH//YJ62JZjJXGRRYHenTCcOTwzPByOntGkMnLKtdHTfIHiZ3Re8qSASVGg2KgKHGQXZ6KE3WLWLV1lKPbCyEG3SkglzBOKiYOu9PnPtKgyyHZm679psK6lTJcWrP7w7FeNw0IHXa7vlgr0UhYZionr9pBfh64xoqAX4b5JZrqsVgOJUeLSNxh1ILIoIDXDOGQ/ynublFSZ4MUAHiO0h0fbqAuhl+dPoLuCUuL6B/x9v8efdSfCH2C+FLk546LOwhp0Z5gbnj5oFPttjVn4EncfNWpRb7kRUKawx4cdxrF+Pb0uVIlfMOWm0ckx0bHwBZh3cDjjFuxuyQvBH/QFf5TK4vUMXXZjp2//Aql8BIViYms2QIH2NuzKeSYWl3xV5z0pFwUo3W1ijz5D8Thw6Qv2Pahws3HQ+U+jeO9WbgnYc7mT8x98fVWaShghy0suyq7UmDhoVim4Bq35AlCMkINuak7oRpaAWoSnNaDY1ukI3kxq89t4two0HGeIMBpYb3as13d85mtIjeLFtFIKSvNBWR6gfliuR9m+yPjo7rXXhVQp1JfuB193h10bM2utr5Ef0dBplTU3cm3vvSgXcwPruv3usmzVN1Efk9w+XM3231J8r8DiIkKTPKeMY0qDANM0yHFCPMDEJVEYuAxcH9DGQTOhzefc7qFRfP9c72YT2DaJJrshjtdSlqMzXhQW60aqFS8+Sflgo7t28xX4w+7G2FUNvXK2piZHSgLbr7rguVGyXJwS7o73wmewgDLj6ulkhH9klRRb7j0Pj0Vbhx2/1mXbF3Je6EPs/cgetfcH3ymxHuIV+N5463Iis17s6dzacCv/SW5ATXm1WJqZWNn3izQLh/einmMq1TyQ9mOv9Tdd2Y9ePvivPNZ26Oh6V6fCL/C9EgqyueGmso+mnWoOpfmeFAeF+b7gYVmeEH9UlO9W3J+h1RNk+HdKeK+Jh6HnjnkYYsgJYErzAEfczTELKAkzjwU0jNDmW9fF20H+fmtoGvn9M+p3dBaNhzv6RBueVno04ysojVTVqvcGkddKdJVBaUTKC1sXu1/jMFnJquy51e/K4cQ17g/Dod2pUjlPYV7YZn18mPcj/42509846I/5x9kNA789Athga5naqtYF3R8K2lHAfjbmndsxBe+pzXU9PwpSin02DjClSYhDyD3sURZ5bsgZhDnaOIdqCtxgOIFZVXI1+gQrIbNK/6+kv0NJOaM0zL0AJ4RRTHnm4SjPCQ48whI3JB7Jk7pvNbgtxXvq+99GXwHWolyMbqUsRuepLOTC6mA0r9QjPPWHXOp6fsL8CKchpJj6xMVJ4CeYkjQgLE8SD1K0+Q9F3lgEUxEAAA==",
            },
            AmountRequired = 2,
            UniqueFish = true,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Astacus Lamentorum"] = new List<uint>()
                {
                    45693,
                    45705,
                    45715,
                    45727,
                    45744,
                    45826,
                    45847,
                },
                ["Lunar Hemiodus"] = new List<uint>()
                {
                    45707,
                },
                ["Lunar Peacock Bass"] = new List<uint>()
                {
                    45706,
                },
            },
        },
        // Export for Mission [456] -  Assorted Alchemical Materials
        [456] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACq2US0/jMBDHvwqacyIlqfO8haogpMKiLXtCHAZn0liEOGs7PBb1u6+cJrQpZZFW3JwZz2/+83DeIO+MnKM2el6uIXuDRYP3NeV1DZlRHTlgnUvR0M5ZjK6LArIgSR24VkIqYV4h8x240IsXXncFFTuzvb/Zsi6l5JWF9YfAnnpOlDhw3t5UinQl6wIy3/Mm5H+je0YaTyK8L8XMq+5xVMB8j30hYYySdU3c7AX6+9eCr9NKVQisP2mpH0TRpKlsCDsTulq8kt5LHB4oDsOJ4mhsOj7QqhKlOUXR67YGPRpWBvmDhiwc2hglH7n71HSgXqMR1PDPVoP5XnSIiaYNDUaSEn9ojma7GaOIw+jgYByzIfqmwlrggz7DJ6ksYGIYq5s5U/tP4vKJFGS+7dmx1Y4SuxF7Ccd2nor1OT72defNuialxyR29gVks9hjH9RPUMlm48DixSicPLx3AXYuN3L1jO1FYzphhGzOUTRje1zfgWWn6JK0xjVBBuDAVa8JrmRDMBBeW4LM9ukIbym1+W/etSJNxxWCC5/4txl7/07PqiVuFNbzTilqzDdVeUD9tlqPqv1Q8dHs/a0zqTj1j+4Z23HYvbGw1v7dhClLnGGzVka29t2LZr0y1PZ/2F2Vw/bl6nuKy/dwvdpfjfjdkeVCQkGByGK3LIi7zPfIRc65W5TePc4CVnpeABsHlkKbH6XNoSG7vRsNw29/Z7BFbb+3CgaNtyyM7k5yraUyVJzkNa/oUXCsTy7RkBJY66kuxLjgjKHre7PQZUFMbpqWsRvxMKYojsKUlbD5C4vnvqjpBgAA",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },

        // C Rank

        // Export for Mission [457] -  Big Fish
        [457] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACq2VTW+jMBCG/0o1Z5AwAfNxS6O0qpR2q033VPXgmCFYpZi1TT+2yn9f2YEmpMlWWvUGM/Yz7zsewztMOyNnTBs9K9eQv8O8Yasap3UNuVEdemCTC9HgLlkMqasC8jDNPLhVQiph3iAnHlzp+SuvuwKLXdiu32xZ11LyysLcQ2ifHIemHly2d5VCXcm6gJwEwYj8b7RjZMloR/ClmFnVPZ0wFpEg+kLRAJF1jdwMTiISkP1l4dcqpCoEq08IISGlox5H/bYLoav5G+q9wvGB4jgeKabDGbBHXFaiNOdMON02oIfA0jD+qCGP+67S9DN3n5r11FtmBDb81KREJKCHGDpuaDiQlPiDM2a2gzKIONwdHhzHpN99V7FasEd9wZ6lsoBRYHA38cbxn8jlMyrIie3ZsUmnqZ2IvYJDO8/F+pI9Od/TZl2j0kMRe/YF5JMkiD6pH6HSzcaD+atRbHQPPwTYc7mTyxfWXjWmE0bI5pKJZmiPTzxYdAqvUWu2RsgBPLhxmuBGNgg94a1FyG2fjvAWUpv/5t0q1HhcIfhwIr+t6PI7PcsWuVGsnnVKYWO+yeUB9du8HlX7yfHR6m7VhVQc3aV7Ye1w2C5Y2Ki7N3EWZV4/WUsjW3vvRbNeGmzdB3fnsp++qfoec9M9nFP7qxG/O7RcyFZpUWLMfeQ48SNGAz9LS+6XhFPELKF0xWDjwUJo86O0NTTk9w9DoP8L7ALW1PZ9q6DXeB/FycPZuVifuQUjCUmxymJWcp+SlPlREJd+ltGJzwMSJiQiCeMFbP4C1oBtKeMGAAA=",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [458] -  Environmental Inspection
        [458] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACq1Uy27bMBD8lWDPEiDJ1PPmGE5gwEmDOj0FOdDUxiKikCpJ5dHC/16QFmvLcRqgyI3a5c7OzC71G6a9kTOqjZ49bKD6DXNB1y1O2xYqo3oMwCaXXOA+WfvUooYqKcoAbhSXips3qOIAFnr+ytq+xnoftve3O6wrKVljwdwhsSeHkxUBXHa3jULdyLaGKo6iEfK/oR1GmY8qok/JzJr+6QNhJI7IJ4w8iGxbZMYrIXEUH15LPmchVc1p+wGROMmykcdkKLvgupm/oT5onB4xTtMR48zPgD7iquEP5pxyx9sGtA+sDGWPGqp0cDUr3uMeopYD6g01HAXDAz7ZcV02djDxpYr/whk1u83wXY+rkyP/J0P1bUNbTh/1BX2WygKMAl7OJBjHvyOTz6igiq1Jp1Y7K+wKHDT0/p3zzSV9ckKnYtOi0r6JHXYN1SSPyDv2I6hiuw1g/moUHT28vwTsIG7l6oV2C2F6brgUl5QLb08YB7DsFV6h1nSDUAEEcO04wbUUCAPCW4dQWZ9O4C2lNv+Nd6NQ42mGEMIH+V1Hl9/zWXXIjKLtrFcKhfkilUeoX6b1JNt3ik92d7cupGLoXtkL7fywXbC2Ufdu0pKUwbBZKyM7+9C52KwMdu4Pu1c5bN9UfY246QGcY/tD8J89WlyICMNJktchYZMiJDUpwzWp8zBLWUlKlqU5EtgGsOTafHuwPTRUd/c+MPz29wErave9YzBwvCNpcX82F89cSfGEwtD2bCG09ZNLcUQpy5KkjpIwTdk6JJTmIS3qJIyifJ2QtCjjNIXtH76wCNnkBgAA",
            },
            AmountRequired = 2,
            UniqueFish = true,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Lunar Eel"] = new List<uint>()
                {
                    45718,
                },
                ["Weepingeye"] = new List<uint>()
                {
                    45717,
                },
            },
        },
        // Export for Mission [459] -  Polarized Dyes
        [459] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACs1VTW/bMAz9KwXPNuBvW76lWVoUSLtg7k5FD4pMx0IdK5PkfqzIfx/k2IudJgtQ9LCbQJGPj08k9Q6TRospVVpNixWk7zCr6bLCSVVBqmWDFpjLOa9xf5n3Vzc5pF5CLFhILiTXb5C6Ftyo2Surmhzzvdn4b3dYt0Kw0oC1B8+cWpwoseB6c19KVKWockhdxxkh/xu6xSDxKMI5S2ZaNuueQeA6wRkKfZSoKmR6EOgO3bzzaYXMOa1OSOp6UTQSNejCrrgqZ2+oBonDA8ZhOGIc9aLTJ8xKXuhLylvexqB6Q6Ype1KQhp2MUfIRd4hKOtQF1Rxrdqo1AteJDmGisaBejyT5b5xSveuMnsRhtHfwHH4XfV/SitMndUWfhTQAI0NfnW+N7T+QiWeUkLpGs2OtHSWmIwYJezkv+eqartu6J/WqQqn6JObtc0j92Ak+sB9BJdutBbNXLelo8P4SMO9yL7IXurmpdcM1F/U15XUvj+1aMG8k3qJSdIWQAlhw13KCO1EjdAhvG4TU6HQEby6U/jTeQqLC4wzBhhP3u4zt/Z5PtkGmJa2mjZRY6y+q8gD1y2o9yvZDxUezt15XQjJsh+6FbvrHbo25sbZzE5KAWF1nZVpszNzzepVp3LQbdl9l130T+TXFDeFatj9r/qtBgwtB7niOF3u27zvEDkhR2Mug8O04cl0nIiSkSQhbC+Zc6e+FyaEgfXjsDd3a3xtMUZA+vMPu0G20MPa80wXMm5rKiwnqEmUlGjWuxqxnI9ek0CintFmVes7XZt+ZryPHWnNGKzO/Jt3OYbIWTT1wO7bGQnI4yv54yyYmcSMLyjCrzKv2xZDwzAYLtxb8Nx/ivq0+3UzZC90Yy9So2go6bK+uqcxxZ967HWvzQfNRwljuF77tIFnawTIkNmHL2Ma4cAjmSeh5EWwf+3QdxYcgJI8XC1FR873kF9/M3znCdf3YcZfMscmycO3AQ2ZTc0K3YAGhJIkdCts/1YWfqiYJAAA=",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [460] -  Southeast Well Ecological Survey
        [460] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XTW/bOBD9KwbPJkBJpCT65nqTbAA3G1RZ9BD0QEkjmzAtuhSVNhv4vy+oD9ty7KQtclhsczOGM2/eDJ+G4yc0ra2eicpWs2KBJk/oohSpgqlSaGJNDWPkDueyhP1h3h9d52jix3yMbo3URtpHNPHG6Lq6+J6pOod8b3b+2xbro9bZ0oE1P3z3q8EJ4zG62twtDVRLrXI08QgZIL8M3WDwaBBBXiUzW9brM4VRj9BXGPUgWinIbF8J9Yh36Oa/zkKbXAp1hojnh+Ggx7QLu5TV8uIRqoPE7IgxYwPGYX8HYgXJUhb2g5ANb2eoekNiRbaq0IR1XQ3j57iHqLxDvRVWQpnBAZ/wOC4cdtDvQ438B2bCtsrosx5H+0f9D7rou6VQUqyqS/GgjQMYGPpygvHQ/gky/QAGTTzXpFPSDmMngYOEff8+yMWVWDeFTsuFAlP1Sdxl52gSRIQ+Yz+AirfbMbr4bo0YfHg7Au4i7nTyTWyuS1tLK3V5JWTZtwd7YzSvDXyEqhILQBOExuim4YRudAmoQ3jcAJq4Pp3Am+vK/jLerYEKTjNEGJ05bzM253s+yQYya4Sa1cZAad+oyiPUN6v1JNtnFZ/M3nhdapNB85V9E5v+shtj7qzNd8M45eNOWYnVG/ehy3KRWNg0E3ZfZae+qXmb4g7hGrZ/l/JrDQ4XZX4UMJ6HGLI4wJSFAeZ5FGAKkEWe8H0CBdqO0VxW9q/C5ajQ5P6pyeYK2BHk/DzDqVKjNvSY5o02a6H+1HrlgPpR8xnEav/xuNMKBp3tTC0O9SI3q/rgxBpdLn4mnAQH4XNYQJkL8/jTCH/oOlU77gMPP+Q7hz2/sy4DDie87ozcnMsUMT/YuZzLNXB6IVvn59Q6LSyYmagXSzuXa/e+eO3BsYybzaI27QPmfhxM6naIMn78BLtRffY1dWtAP2p6pXyCr7U0kCdW2No9am7POJbPj6nkh8Xwfudveucv7nHb4ZDKCASZIJjRHDD1WI7TwhO4EB7nURiS1I/R9ks/pbpd9H5naAfV/RM6nFiURcQ/P7MSK8zoVkGmB0PLe6k11zmUVmZCuX64PK3DdK3rcuDWvAbHm0Qw3Opil6k2hcggUW7ynN5nGWev7FNsO0b/mfV8/8j98tPmgp1l5rraNPTwseueOPezNe/dTin3QGUiCoFEoYcpcIppFBKcBmGI4zwVcZB5jPgUbcfPVOTT8wXcynK11roczWS2VDJ/19LvoSWacipCiLAXpRRTiAOckpzglDDwGWSU+v5JLbHzBczrUphRotWwjHcV/W9VlKY8ogUNcAE8xpSTDHMoCE4z4kVRWpDAg5MqCl9416R6ADNKbG0WoMt3Kf0eUooCj9GQAy5yn2FKfI5FzAX2RZiywksjylizQrW4HcV7GpIvo0TXdgmisqPPoNToItNKL5wSRkltHuBx+I8yZpDmMWWYRoGPKS0o5pxGOBMQFoIEnHkF2v4LFSMleKMUAAA=",
            },
            AmountRequired = 2,
            UniqueFish = true,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Lunar Sole"] = new List<uint>()
                {
                    45725,
                },
                ["Pinkmoon Cichlid"] = new List<uint>()
                {
                    45724,
                },
                ["Silver Sturgeon"] = new List<uint>()
                {
                    45726,
                },
                ["Star Pleco"] = new List<uint>()
                {
                    45702,
                    45711,
                    45723,
                    45769,
                    45820,
                    45841,
                    45913,
                },
            },
        },
        // Export for Mission [461] -  Fish Sauce Ingredients
        [461] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACq2V226jMBCGX6Waa5CAcL5Lo7SKlHarpXtV9cIxQ7BKbdY2PWyVd1+Z4CbksJVWvYMZ+5v/H4/hA6adFjOitJpVa8g/YM7JqsFp00CuZYcOmOSScdwlS5talJAHaebAnWRCMv0Oue/AQs3faNOVWO7CZv1my7oRgtYG1j8E5qnnxKkD1+19LVHVoikh9z1vRP43umdkyWiH96WYWd09nzEW+l74hSILEU2DVFsnoe/5+8uCr1UIWTLSnBHiB3E86nE4bLtiqp6/o9orHB0ojqKR4tieAXnComaVviSs120CygYKTeiTgjwauhqnx9x9ajZQ74hmyOm5SQl9Lz7ExOOGBpYk2R+cEb0dFCvicHdwcByTYfd9TRpGntQVeRHSAEYB627ijOM/kYoXlJD7pmenJj1OzUTsFbTtvGTra/Lc+57ydYNS2SLm7EvIJ4kXHqkfodLNxoH5m5ZkdA8/BZhzuRfFK2kXXHdMM8GvCeO2Pa7vwLKTeINKkTVCDuDAba8JbgVHGAjvLUJu+nSCtxRK/zfvTqLC0wrBhTP5bcU+v9NTtEi1JM2skxK5/iaXB9Rv83pS7ZHjk9X7VVdCUuwv3Stp7WH3wdJE+3sTZWYit5NVaNGae8/4utDY9h/cncth+qbye8xN93C92l+c/e7QcGFSeVXmU88lXhy7IU0SNw2j2M3CsEySSUnJagUbB5ZM6R+VqaEgf3i0geEvsAsYU9v3rYJB40MY+48XJnlRkI7ixYKvJZYMuVZjQbSqsjRMYtdfpUaQ57mkXFEXJ9kKsywKkqCCzV+y46n58QYAAA==",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [462] -  Aquatic Samples
        [462] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACq2V227iMBCGX6Wa60TK0TncUQRVJdqtNt2rqhcmGYjVNE5tp4etePeVTbwQCltpxV2YyXzz/+Mx+YRJr/iUSiWnqzXknzBr6bLBSdNArkSPDujkgrW4S1Y2dV1BHqSZA3eCccHUB+S+A9dy9l42fYXVLqzf32xZN5yXtYaZh0A/GQ5JHbjq7muBsuZNBbnveSPyv9GGkSWjCu9bMdO6f7YKIt+LvpFgq3jTYKlOTCTyPX+/KvheBRcVo80Jnh8QMppxNJTNmaxnHyj3DMQHBuJ4ZIDYM6BPWNRspS4pMzZ0QNpAoWj5JCGPh6mS9Ct3n5oN1DuqGLblqU2JfI8cYsh4voElCfYbp1RtF8WKOKwODk4nHKrva9ow+iTn9JULDRgFrLvQGcd/YslfUUDu65kd23SS6gXZa2jHecnWV/TZ+J606waFtE302VeQh4kXfVE/QqWbjQOzdyXo6B7+FaDP5Z4Xb7S7blXPFOPtFWWtHY/rO7DoBd6glHSNkAM4cGs0wS1vEQbCR4eQ6zkd4S24VP/NuxMo8bhCcOFEftvR5Hd6ig5LJWgz7YXAVp3J5QH1bF6Pqv3i+Gh389acixLNpXujnT1sE6x01NybOIsyZ9isQvFO33vWrguFnfnD3bkctm8izmNusoczan+17KVHzQVMcOl7fuDSNArdKExDl8ar0iVelSUr4q1ISmDjwIJJ9WOle0jIHx5tYPgK7ALa1Pb3VsGg8SEiwePF5KWnipUXBX3uGpRjJcuAkNJLEpemaexGAfHdNIsjN4y9qkwSEnppAps/ch9WWuoGAAA=",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [463] -  Northwestern Water Inspection
        [463] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1WTW/jNhD9KwbPIqAPSiJ187pJGsBJgzpFD0EOFDm0iciiS1GbTQP/9wVlq7b8sQEWCZAWvRFDzpv3ho8jvaJx68yEN66ZqDkqXtFFzcsKxlWFCmdbCJDfnOoadpuy37qWqIgpC9Cd1cZq94KKKEDXzcU3UbUS5C7sz683WDfGiIUH6xaxX3U4GQ3Q1ep+YaFZmEqiIgrDAfKPoTsMlg8ywjfJTBbt8owwEoXkDUY9iKkqEK5XQqIw2j8Wv83CWKl5dYZIFGfZoMdkm3apm8XFCzR7hdMDxmk6YJz1d8CfYLbQyn3huuPtA00fmDkunhpUpNuuZvQYdx+VbVHvuNNQC9jjkx3mZcMOxn2q1X/DhLuNM/qqh9nxQf+Tbfb9gleaPzWX/KuxHmAQ6OUkwTD+OwjzFSwqIt+kU9bOqLfAXsG+f1/0/IovO6Hjel6Bbfoi/rIlKpI8JEfsB1B0vQ7QxTdn+eDh/UPAX8S9mT3z1XXtWu20qa+4rvv24ChA09bCDTQNnwMqEArQbccJ3Zoa0BbhZQWo8H06gTc1jftpvDsLDZxmiDA6s7+p2O3v+MxWIJzl1aS1Fmr3TioPUN9N60m2R4pPVu9OXRoroHtlz3zVX3YXlD7avZuUERZsnTVzZuUfuq7nMwerbsLuVG7dN7bvI24frmP7R63/asHjIhpKzvIUcM7KFBPKBC4jLnHEhYpVzGjJKVoHaKob95vyNRpUPDz2ge3Y3wW8KFQ8vKLNYjsy0jxJzwu4gYrXYmEqzQc6/CT2jRorB3bC2/nCTfXSjzb/0ZBQOy145V+uL7Q5MF6ath4c6zp/+GqT4QSlvlJrFRcwq/wFnv52pCx9Y3al6wB9mk/hzlA/bSOf7CMT39WuofvG2trJLzfh3bFTBt+zXUxyKWNCMC1VikmaAKYsjbHMkzSJRZYoFqN1cGyj/LyAaVtzO/pFN6JtPo+P/o3G6W/9+Ku1ryg8fxUT0yzNs7FL9IEeoiUFkAxwJEWICSkTXBJGsIhYSAVLGU3oSQ9l54n/alYrkNjUoylwpXzip3HS/xPpIydSpLIyJILhMgkTTECEmJNI4YxESkS5THhWnnQTPS9gZipuR5cVt/C5rPSfHUr+D+tHQ0mL0cTyGkaX1ctHzqac0TQMlcKciByTPCoxDSHCiSQqVAkvOc3R+rEvt2X4QLLkcXRrrFs8Q+PA1qM/uQM7uq4b/+upTT38e0uAKCVFhlUKISZKAuZRSTGTZc5YQhSLQrT+Dk+y1dQPEAAA",
            },
            AmountRequired = 4,
            UniqueFish = true,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Hopped-on Leaffish"] = new List<uint>()
                {
                    45736,
                },
                ["Lunar Discus"] = new List<uint>()
                {
                    45737,
                },
                ["Melancholia"] = new List<uint>()
                {
                    45696,
                    45708,
                    45735,
                    45754,
                    45778,
                    45853,
                    45938,
                },
                ["Solar Flarefish"] = new List<uint>()
                {
                    45738,
                },
            },
        },

        // B Rank

        // Export for Mission [464] -  Hollow Harbor Water Inspection
        [464] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACt1Xy27bOhD9FYNrCZAo6rlzffMC3DSoU2QRdEFLI5uwLLoklTQN/O8FJdGWFCVxiywuujOGM2fOjOblZzStFJ9RqeQsX6HkGZ2VdFnAtChQokQFFtKPc1bC8TEzT1cZSnAUW+hGMC6YekKJa6ErefYzLaoMsqNY6+8brM+cp2sNVv/A+leNE0QWutjdrgXINS8ylLiO00N+G7rGiMOehfMumdm62r4SGHEd8g4jA8KLAlJlIiGu43bV8PssuMgYLQxA4JIeAGnVzplcnz2B7DjyBwx9v8cwMDmnG1isWa4+UVbz1AJpBAtF041Eid9mMYhe4nZR4xb1hioGZQodPsHQLuhnDBtTwX7BjKqmEozXoTUe5NtrrW/XtGB0I8/pAxcaoCcw4XhWX/4VUv4AAiWuTpLxSXoeTMI+sdUF3daRTctVAUIaVP01M5R4oUNe0O1BRfu9hc5+KkF7nXWoMZ35W754pLurUlVMMV5eUFaafNiuheaVgM8gJV0BShCy0HXNCV3zElCL8LQDlOjEjODNuVR/jXcjQMI4Q2SjV94bj/X7kc9iB6kStJhVQkCpPijKAeqHxTrK9kXEo95rrXMuUqjb6pHuzMeuhZmW1o3ixz622spaKL7Tnc3K1ULBrh6hxyjb6puKjwmuC1ez/VayHxVoXBQtU0zS2LNDP4ptEuPAjkiI7dSPlsEShz72AO0tNGdSfcm1D4mS++famw7gQDCOX2c4LYpJYzqkec3FlhaXnG80kJktd0A3x+bRrxJ6mW1FDQ5xQz2cjPFCCV7WvddqHVowp4UE62RUx+ugzmEFZUbF00cBf5PwH69afaPYSEz4X8ri6W4N5TRV7AGuMigVS/XGGEHFgc5BY/9uBl61PCXKEeNbwXZH2n0FXUEHlRfMxpTGSPT1dPdMcwViRqvVWs3ZVi84t3kYtlV9ylSi2aD6R2dVjFwAXujHw0X45k2hzxAzCU0hf4UfFROQLRRVlV6y+s4ZVvcfFfHJRdlT7NfT6QVzWmX8EyVgvjn5w2/emaFu6EXg4MgOI5rbBOe+TbEPdhws89Qn4GZA0P67GaLtLXx/EDRz9P4ZdQcq8UPivT5SbwSTW6pYOqmNunPVfSs9hymic6J9NQrTLa/KjtrYaezHw9vH6x+ekXZciZymsCj09DORvGio4Y3n7y30v/mLcNzDf719F490pyUzndU6od193G5h/bMRH9XGqrdTaUvPz1yXeDbJ48AmnhvYUeqAHcWOkwVujP3YryutwW0p3pOAfJ9c8qLgj5NLKpZcTO6oAjG5KqU+aRgv+1cBdhy8DD3Hdr1oaZMUMptiTG28JLlHAxrnNED73xGOGoBIDgAA",
            },
            AmountRequired = 3,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Prismatic Fish"] = new List<uint>()
                {
                    45743,
                },
            },
        },
        // Export for Mission [465] -  Preserved Foodstuffs
        [465] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACs1VTU/jMBD9K2jOiZTvOLmVqkVIhUVb9oQ4TJNJYxHirO3Asqj/feU02Tal3UqIw96s8fjNe+Pn8TtMWi2mqLSaFmtI32FW46qiSVVBqmVLFpjNBa9pt5kPW9c5pB5LLLiTXEiu3yB1LbhWs19Z1eaU78Imf7PFuhEiKw1Yt/DMqsOJmAVXzX0pSZWiyiF1HWeE/G/oDiOJRyecs2SmZft8QljgOsEZRgOIqCrK9KAkcB13P807z0LInGN1gojrRdGox0F/bM5VOXsjtVc4PGAchiPG0XAH+ETLkhf6EnnH2wTUEFhqzJ4UpGHf1Yh9xN1HTXrUO9Sc6uyUUwLXiQ5honFDvQFJ8t80Rb01ykDi8LR3cB1+f/q+xIrjk5rji5AGYBQY1PnWOP6dMvFCElLX9OyY0yNmHLFXcGjnJV9f4XOne1KvK5JqKGLuPofUj53gA/sRFNtsLJj90hJH7/AvAXMv92L5is11rVuuuaivkNdDe2zXgkUr6YaUwjVBCmDBbccJbkVN0CO8NQSp6dMRvIVQ+tN4d5IUHWcINpzY31bs9nd8lg1lWmI1baWkWn+RygPUL9N6lO0HxUerd1lzITPqHt0rNsNld8HcRLt3EyahZ/XOWmrRmHfP6/VSU9MN3J3K3n0T+TXi9uE6tj9q/rMlgwt54FFY+JEdx5FjBxnLbOZHhb2Ks8TJMUDPd2FjwYIr/a0wNRSkD49DoP8FdgEjCtKHd9gu+okWxgE7LWDR1igvpoIqzLDW5UiMmc6mW5NCk5xiuy71gj+bcWc+kpxqzTOszPM11bYJk2fR1ntpx6ZYmBy+ZH88ZJkp3MoCM1pW5lIHLUl4ZoCFGwv+m+9x56pPe2n5io2JTE1Xu4buu6v3lFluw7u0Yy7f857rMgfJITtZFY4dOC6zMY58G1mx8rzc8YMkgc3jUK6n+BBE4eNFF5IvlF/MhciVbotCjZ29YlGMOfPsmLmJHTBiNovj0PYwCoIQCzdOXNj8AX/IXB46CQAA",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [466] -  Hollow Harbor Ecological Survey
        [466] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1YTW/bOBD9KwbPJkBKpCT65nqTNoDbBnUWewh6GJGULUQWXYpqmy383wvqI5E/lGAXwW53kZs8HL55M3yaofUDzWtnFlC5apGt0ewHuighLfS8KNDM2VpPkV9c5qV+XFT90pVCsyARU3Rtc2Nzd49mdIquqovvsqiVVo9m779vsd4bIzcerHkI/FODEyVT9HZ3s7G62phCoRkl5AD5aegGQ8QHO8izZBabejuSGKOEPcOoBzFFoaXrM2GU0KFb8DwLY1UOxQgRGkTRQY1Zt+0yrzYX97oaBOZHjDk/YBz1ZwB3erXJM/cG8oa3N1S9YeVA3lVoxruqRskp7hBVdKjX4HJdSj3gEx3viw4rGPRbbf6nXoBrldFHPd4dHNU/7HbfbKDI4a66hK/GeoADQ59OOD20f9LSfNUWzagv0jlpR4mXwCBgX783+fotbJtE5+W60Lbqg/jDVmgWxoSdsD+ASvb7Kbr47ix0L56v/I1ZfYPdVenq3OWmfAt52dcD0yla1la/11UFa41mCE3Rh4YE+mBKjaYtwv1Oo5kvzBm8panc38a7trrS5xkijEbW24jN+iOf1U5LZ6FY1Nbq0r1QlkeoL5brWbYnGZ+N3ni1Alk5s/Pva16uV07vmkb5yL0T0dy+DOUhXMPh9zL/UmuPi3QaEqYyhdNQUcxUHOEkJRSnEDIiYsXjiKD9FC3zyn3MfIwKzW5befoEHggKMc5wXhSTdusxzQ/GbqF4Z8ydB+o7xh8amt/e7rNo3BmNfWfpfVbOmnJ9xouEA6+lXutSgb0fc/zN1GlxPmAQiQeHkWhDl/FQrdeNzXdjkWIehA8uY7EOnJ6I1vl5ic0zp+0C6vXGLfOt7+20XTjWXjPVa9sOD/8w6JJtA+PidPw9Mcn8CO7f+v54P+kvdW61WjlwtR8ofsa/nvn/6cwHnQWAUBryAPMskZjRgGIhOMFhRnmgSMZikGj/uW8t3T3w9sHQdpfbH2jYZhiP+ROtcFmXYCcLSM02hYNmQ5+qzpXSpcslFL4kPlTrMN+aujxw8wTE8SAPDy9ViY9U2wykXhWwe2Qu+DP3F76fol/mOuwHR3sdbG9Aj9NpmBEbP4uFqba5nNyAhdLVBaAB6MIXthVmZ/nkx1VnbgMOJ1g3t/xjax4AnFH2QIWEJ2EoSYhZFmSYgWQY0izDJKBSBkpKEodoPz1RGSPjmb03plznRfEqsFeBIQiDlLIIcJoSgZmigJMEAhyJOOVZRrUm4pzAeDCe2ccdFJML/Sqwf0BgfPwYLj3Vb+C0ncyt21hT1Fb/OyrL4iBmIosxZTHFjGUaAyQaa65lyOMgCnh6VmXhc8PyOrdga1m/au1Va63WIuABCwXghCuCmUwoTiPFsYjCFEiapIE+PzLF0yPTGnk3WUCp7n8drZ370Pcfk96p1P7yx4LVicZeRkmKZJrFIsFCCoZZHAksJM2w5kwIJZJMd38BWtyO4i2Los+Td6YozLfJO7CpsZMLaQqz9kKYrGr7VXsJDb9iJCEkioZYUQmYaQhwEpMQhwFnStMAlGJo/xM2YS3m3hYAAA==",
            },
            AmountRequired = 3,
            UniqueFish = true,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Lunar Cabomba"] = new List<uint>()
                {
                    45751,
                },
                ["Lunar Pirarucu"] = new List<uint>()
                {
                    45753,
                },
                ["Moongill"] = new List<uint>()
                {
                    45740,
                    45750,
                    45790,
                    45800,
                    45878,
                    45930,
                },
                ["Moonrock Candy"] = new List<uint>()
                {
                    45739,
                    45749,
                    45789,
                    45799,
                    45871,
                    45877,
                    45929,
                },
                ["Opal Eel"] = new List<uint>()
                {
                    45752,
                },
            },
        },
        // Export for Mission [467] -  Absolute Specimen
        [467] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XS2/bOBD+KwbPJiBRpCT65niTbAAnDeos9hD0QIkjm4gsuhSVNhv4vy+oh235ERdBDkWRmzyc+ebBjzPjVzSurJ6I0paTbI5Gr+iyEEkO4zxHI2sqGCJ3OFUFbA9ld3Qj0YjEfIjujdJG2Rc08ofoprz8meaVBLkVO/11g3WrdbpwYPUHcV81ThgP0fXqYWGgXOhcopHveT3kt6FrDB71LLyzwUwW1fJEYtT36JmIOhCd55Da0zj+rhU5H5Q2Uon8BJ5PwrBXctqaXalycfkCZVdR6ntsLwHGegmE3ZWIJ5gtVGYvhKrTcIKyE8ysSJ9KNGJtkcP4EHcXlbeo98IqKFLYiSfctwv7BSWdqVH/wUTYhijHWBfGB2Bk73aCFuxhIXIlnsor8ayNw+sJuuyCYV/+FVL9DAaNfFezEyHQnsOunBdqfi2Wdd7jYp6DKTsn7u4lGgWRRw+i70HF6/UQXf60RvSe5SYAdy8PevZDrG4KWymrdHEtVNGVGvtDNK0M3EJZijmgEUJDdFfHhO50AahFeFkBGrk6HcGb6tK+G+/eQAnHI0QYnThvPNbn23hmK0itEfmkMgYK+0FZ7qF+WK5Hoz3I+Kj3WutKmxTqR/dDrLrLroXSSetnxDgLhi2zZlav3LtXxXxmYVX3322WLfvG5mOS24Wro/2nUN8rcLiIxiTJZCpxlGQcU8ESLEjs4YCHkERMCj/jaD1EU1XaL5nzUaLR42vtzSWwaRJNdqdinFaFMIOpMM/Cod1psxT531o/Ofuu4fwLov7t5CXYzdPJRF5C95bbw906t6ImeepHrpF1mDNrdDH/AFQv2EGdwhwKKczL9on/IsJfukry/UwbDRLyjcJB2IcqvRiOaD0YtTrlKWIk2Kic8tVTesNbq+coPc4smImo5gs7VUs3k/zmYJ/r9XJSmWbouY+ddn6kZwcR44dD/Y2B7BaLrj11NPsK3ytlQM6ssJWbi25zOcG9X+PSeW58UuBdFHjvne80tszzAiJlggNOBaaCSJyk1MNZ5ntBmskw4xKtv3Wdrd1uHzeCprk9vqJ+l4tYfK7LXah0oUyvI/tvFedGQmFVKnJXEeepURgvdVX01Oouu79/BP3VMHaeKpOJFGa5a0Xb9nxm7WLrIfptdvztLHz3BHTGTjJxZawruDsT0QiNk1LnlYWBG+tqCQVBjVWjt7U7xt4+05I4kx6mWSIxZczHCWMEe4HPIIn8iMYJWg8PmES90xndal3MVZ5/suh3ZxF+H2kYAy6In2AWiRTTLCA45jHFIXAphKScU+8YaRg5ncCXlcgHl/BJmj+VNFEig9QHgTkkHNM0oZjLFDAECYkJDwkXRzvN+c38XhlhqrT6pM4fSp2QxyEjjGAmEsA0iCPMU8GxECBIFmZ+GPF6HWpw2xAfaRh9GxwMygEeXGkt+/8kgcQipUGKI8kIpjT0cML9GGckC4UMEkZJiNb/Aw5f6V25FAAA",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [468] -  Aetherochemical Creatures
        [468] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1WS2/jOAz+K4HONmDZsvy4ZbKdboFMt2iy2EMxB9miY6GOlZHl6XSL/PeB/EjsNG52ih7msDebIj9+pCiSL2hea7lgla4W2QbFL+iqZEkB86JAsVY1WMgcLkUJx0PeH91wFLthZKE7JaQS+hnF2EI31dWPtKg58KPY6O9brC9SprkBaz5c89Xg0NBC17t1rqDKZcFRjB1nhPw2dIMRBSML5yKZRV5vewYEO+QChd5KFgWkeiIjBDt4aOVeZiEVF6yYwMMupaMck87ss6jyq2eoBgH4JwH4/igA2t8Be4RVLjL9iYkmDCOoesFKs/SxQrHfZZWGr3GHqFGHese0gDKdqhSCHXoKQ8f5dXskJf6FBdNtofQkTq3dk9vxOut1zgrBHqvP7LtUBmAk6KPzrLH8HlL5HRSKscnZuUqnoSmQgcM+nZ/E5pptm7jn5aYAVfVOzN1zFHuBQ16xH0GF+72Frn5oxbp3aC5iLVdPbHdT6lpoIctrJso+Hza20LJW8AWqim0AxQhZ6LYhgW5lCchqEZ53gGKTmDN4S1npd+PdKajgPENko4nz1mNzfuSz2kGqFSsWtVJQ6g+K8gT1w2I9y/ZVxGe9N1ptgay03JnnK8rNSsOu6ZtH7l0RzdXHUB7CNRz+LsW3GgwuiigjLAqx7WZOZpOUhnbIA88mqUOJw1lKI472FlqKSv+VGR8Vih/a8jQBHAhG0TTDeVHMWtNTmrdSbVnxp5SPBqhvIP8Aa/6NvAJ9eIsZKyro32Z3aALsX2knauEJDkxj6jFXWslyMOEumzvewHwJGyg5U8+/jPCHrJPiNKRWw6XRQeHIb1JlxOGM1lqJ3ZSnwHe9g8qUr5HSG946PVPE80yDWrB6k+ul2JphgtuD0+pu1ohatdPKfAz68Jlm6wV+9HoavzFJzQrQt5m+nu7hWy0U8JVmujYDzewYE0V2oWj+c238XwLvKoH33vmglTkh8bMA+7abRoFNiJPYjETETljmpwSo40YE7b/2vazbQx8OgradPbygYV8jfkC96c52z55my7pkaraWtWllZlEeNjn8VpJuOJRapKwwmTEeW4X5VtblQO3cLuVHp/uEN171QuO4VhlLYVWYDtXHE/kX1ih/b6HfZkk/DsV3j8LVE9sZycJktUnocDh2I9F8tuKj2rkaHtSbRzDHicvthPHAJl7m2QmPsE29gCYOdqmDw6beWtyO4gOh4dfZHHQOSqY5bM39zxYKmG6exsgF+OCRhGc2D2lkE8apHeHEs33m0IBgL8Suj/Y/AaLaHNTFDQAA",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [469] -  Westward Ecological Survey
        [469] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1YTW/bOBD9KwbP5kKkSEryLfGm3QBuN6iz6CEoFhQ5soXIoktRabOF//uCkhVb/ojTIFi03dxkcubNm9HTcOhv6Kx2ZiwrV42zGRp9QxelTAs4Kwo0craGIfKbk7yEzabuti41GtE4GaIrmxubu3s0IkN0WV18VUWtQW+Wvf2qxXpnjJp7sOaB+qcGR8RD9HZ5PbdQzU2h0YgEQQ/5cegGI4l6HsFJMuN5vTiSGCMBO8GoAzFFAcp1mTASkG0zepqFsTqXxREihArRqzFbu73Jq/nFPVRbgfkOY857jEX3DuQtTOd55s5l3vD2C1W3MHVS3VZoxNdVFfE+7jZqska9ki6HUsEWH7HrJ/oVpJ2rzf+BsXStMrqou950p/7h2vt6Lotc3lZv5J2xHqC30KUTDvvrH0CZO7BoRHyRDklbxF4CWwG7+p3ns7dy0SR6Vs4KsFUXxL9sjUZhFLA99j2oeLUaoouvzsr1h+crf22mX+TysnR17nJTvpV52dUDkyGa1BbeQVXJGaARQkP0viGB3psS0LBFuF8CGvnCHMCbmMo9G+/KQgWHGSKMjuy3EZv9DZ/pEpSzshjX1kLpXijLHdQXy/Ug272MD0ZvrFqBTJ1Z+u81L2dTB8umUW64r0V0Zl+G8jZcw+GvMv9cg8dFJGWpjpXCQgcKM84FliJNsQKZZZQTQuMErYZoklfuz8zHqNDoppWnT+Dh4+YJp8c5jk21MIW0d9KDvTd2IYs/jLn17l2f+Aiy+d1+en63Auf5dx/heqlNkpHIN5rOeeqsKWff4x6EW+4TmEGppb3/boTfTZ0WD9x7FlQkDwYbfkdNehwOWF3bfHksUsRp+GByLFbP6JFoazuv0bPMgR3LejZ3k3zhDwfSbuyKtxkLatuePv5hq822HZAn++fnI0ehP8O7ttEp5QN8rnMLeuqkq/2J5IeEXfk8TSVPFsPrO/9P3/lWa4pVymSgAsx1EGOWZClOmUhxTCnlGRNEJhSthod7UXi8F03qUtrB5GnN6FVNP7OaXjvI/7qDKEGIikiG4yiWmKWaYklJhGUaJ4KmImaUoNWnbrpZX0VvHhbapnLzDfW7SyTEqe5ybk35DwyuClCmN5uRx0p0qaF0uZKFr4uP1xqcLUxd9syaHrd7oQj7l7vYR6ptJhVMCz/DHL7W8oSfuFbx1RD9MLd0P461t9Q2h83Q/KxJtIMb+wo3xd0enNEIydKUf98wkWDyafARKvdFWj24UKYwM/+mBtPa3sE9aqFa5w3YIc1v6TNkLBEsI5iGhGMWQIolpwJLHUcqUFEWBX743tdffPJ0q8tZ5p1etffzau+43H4jzxOcJDLJ0oDhJAwVZpD5pyTAOhRKMCCh4uKg4NjxpM6LGhbGlIOJkepVcC8puH2Bfff/ACdFhp+npEiRWBEVYkF0hFmoOE7TOMKJppApIDqg9KCS+COtC+QyL2c/mpB+3Vb19EvSvoy6lQ9bsmljvYzAqEoSGQcJjsMgxIwmHCdacawh1EGU6YiS8KDAouNJTZ209n4wdXk5s9If2q8S+xWbUxqliQ5khMM4AcyopjgNCcGcCtBK8igI02bub3HXFP2Ud2LG2/53gtNMk1RjoQAwi4IYS64YJjTmQUYlialGq38BoSs69lEbAAA=",
            },
            AmountRequired = 4,
            UniqueFish = true,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Bluemoon Loach"] = new List<uint>()
                {
                    45699,
                    45719,
                    45731,
                    45764,
                    45784,
                    45810,
                    45918,
                    45935,
                },
                ["Leaping Loach"] = new List<uint>()
                {
                    45765,
                },
                ["Lunar Bronze Pleco"] = new List<uint>()
                {
                    45766,
                },
                ["Lunar Lungfish"] = new List<uint>()
                {
                    45768,
                },
                ["Starry Stingray"] = new List<uint>()
                {
                    45767,
                },
            },
        },
        // Export for Mission [470] -  Supper Emergency
        [470] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1W227bOBD9FYPPEqArJerN9SbZAE42qFPsQ9CHETWyiciSS1Jts4H/vaAutuRL0hQB9oJ9k4czZ86MD4fzTKa1rmagtJrlS5I8k4sS0gKnRUESLWu0iDmcixL3h1l/dJ2RxIuZRe6kqKTQTyRxLXKtLr7zos4w25uN/7bFuqkqvjJgzYdnvhocGlvkanO/kqhWVZGRxHWcEfLL0A0Gi0YRzqtkZqt6faawwHWCVxj1IFVRINd9JYHruEM373UWlcwEFGeIuB6lox4HXdilUKuLJ1SDxOEB4zAcMab9fwCPuFiJXH8A0fA2BtUbFhr4oyJJ2HWVxse4Q1TWod6BFlhyHPChh3F03EGvD5XiL5yBbpXRZz2M9g7673fR9ysoBDyqS/haSQMwMvTl+NbY/hF59RUlSVzTpFPSprGRwCBh378PYnkF66bQabksUKo+ifmzM5L4kRMcsR9BxdutRS6+awndxTOdv68W32BzXepaaFGVVyDKvh+2a5F5LfEGlYIlkoQQi9w2JMhtVSKxWoSnDZLENOYE3rxS+pfx7iQqPM2Q2OTMeZuxOd/zWWyQawnFrJYSS/1OVR6gvlutJ9keVXwye+PVCmShq425r6JcLjRumkG5596JaCrfh/IQruHwqRRfajS4BMKQ5a6X2z7Q0A5YFtsMXdeGgKNH48D30CNbi8yF0n/kJociyUMrT1PA7nKHLAzPc7w0sv8GGuVkKvVKVkUt0eDeVnINxe9V9WiQ+pHxJ0Lzu72F5lShNqX097EztfUGbmRmTh+80LIql28Jd/xB+ByXWGYgnwxC57ibBjkUCq23Af9W1WmxK2nk4VG2c9jTPutyitrQ65PCeyk2LbOeUms5SH9Y0BgsCj1DvI084vWG2BcId37mIkxzjXIG9XKl52JtXiC3PTi8Ic3uUcv2iTMfg1nejtmQHT/SL7y3ZlHoZ1OvwY/4pRYSs4UGXZtnz2wih8L8Of29VWZ/k2xOKuSnpPAv/c8H84+DH4DvODbFNLODGMEGLwPbj5DxPA4Y8xjZfu4HYLetPuwM7Qx8eCbjYRhFzvlhOK9LkJMrCUpNZiA3o+HtvtSg6wxLLTgUpismW+swXVd1OXJrBvLhxuGPt7/YZKplDhwXhRlh+0n+yqIVbi3yj9nb98/mLz+WJthYZqaNTQeHz2f3aJrP1rx3OyXYgbiCEF2f5mBzCDw74Dm1GdDYdmiYug5jwMKUbK1j8bxQwA1wWaUS+ErU60kjJaH+V9B/VEEs4pQzmtop454dpLlvQx47NlDq5L7P3MiHZjy1uB3FhyByPk8W9WaDcnKxRrnEkj+NFz+fUSfzqWdHGYvtIHJzOw0Daruh5+VOipRHnGx/APkQ6LAREAAA",
            },
            AmountRequired = 13,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Ataxite"] = new List<uint>()
                {
                    45772,
                },
                ["Lunar Grass Carp"] = new List<uint>()
                {
                    45712,
                    45770,
                    45821,
                    45842,
                    45914,
                },
                ["Macrobrachium Phaennense"] = new List<uint>()
                {
                    47422,
                    47431,
                    47440,
                    47448,
                    47456,
                    47519,
                },
                ["Star Pleco"] = new List<uint>()
                {
                    45702,
                    45711,
                    45723,
                    45769,
                    45820,
                    45841,
                    45913,
                },
            },
        },
        // Export for Mission [471] -  Alchemical Resources
        [471] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1WS2+jMBD+K5XPIJn345Zm026ktFuVrHqoenBgACsEp7bptrvKf1+ZRwJJaKWqhz3sDcYz33wzHr7hD5pUkk2JkGKaZij8g2YlWRUwKQoUSl6BhtThgpZwOEy6o3mCQtMPNHTHKeNUvqHQ0NBczF7jokogOZiV/67BumEszhVY/WCqpxrH9TV0vV3mHETOigSFBsYD5Peha4zAG0TgD8lM82rTMbANbH9AoYtiRQGxHOmIbWCjH2V+zILxhJJiBM8wXXfQY7sNu6Iin72B6BXgHBXgOIMC3O4OyBqinKbyktC6DGUQnSGSJF4LFDptV13/FLePGrSod0RSKOOxSbEN7B7DuMP+mh0Sp79hSmQzKB2J42jz6HasNnqZk4KStbgiL4wrgIGhq87ShvZ7iNkLcBQaqmfnJt311YD0EnbtvKTZNdnUdU/KrAAuuiTq7hMUWh62T9gPoPzdTkOzV8lJ+x2qi1iy6BfZzktZUUlZeU1o2fVDNzS0qDjcgBAkAxQipKHbmgS6ZSUgrUF42wIKVWPO4C2YkJ/Gu+Mg4DxDpKOR8yZjfX7gE20hlpwU04pzKOUXVXmE+mW1nmV7UvHZ7LVXMyCRZFv1+dIyiyRsa908cG+HaMK/hnIfrubws6TPFShclPoe+IZDdC/Brm5bgaOvAnOl4wTAsoMgwaaHdhpaUCF/pCqHQOFjM56qgL32OIHjjnO8LCq4eGB8o7BuGd+Q4jtjaxXdqcYDkPpd2RX1uhTb8JS6dD6R5KzMznhhq+e1gAzKhPC3McdvrFoV5xOabrB3GMnWdxlP1XgtOd2OZfIc09q7jOUaOL2TrfVTczVJJfApqbJcLuhG6bvRHBwPXL3ZK94sEPXQk8Yz+md5TnC6IN9Zbmord19+d9v38FxRDkkkiazUjlFr//8I/IsjMP/knffUBQKMSexg3fXSVLeJ4+u+7xM9cFM/DZzAsGxAu6dOXtpfw8e9oVEY9d7oWasmj7ZnPF1MijiHDY1JcXEPglU8BjEUN4x9w0/TWI9d39Ztw0r0lWWmumfFCeDYJwHEaPcXOhT6MwQLAAA=",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },

        #region A Rank | Sequence | Timed | Weather

        // Export for Mission [472] - Northward Ecological Survey
        [472] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XTW/bOBD9KwbPEiBKlCjp5njTbAA3DeoUOQQ9UNLIIkKLLkWl9Qb+7wX1EUv+iIMgi+1hb/Rw5vHN6HGGfkbTWssZq3Q1y5cofkaXJUsETIVAsVY1WMhsznkJu82s37rOUOyGkYVuFZeK6w2KsYWuq8tfqagzyHZm479tsT5LmRYGrFm4ZtXgBKGFrtZ3hYKqkCJDMXacEfLr0A1GREcRzlkys6JenUiMYIecYdSDSCEg1X0mBDt46OaeZyFVxpk4QQS7QTCqMenCPvGquNxANTjY32Ps+yPGQf8N2CMsCp7rC8Yb3sZQ9YaFZuljhWK/q2oQHuIOUaMO9ZZpDmUKAz7BflwwrqDbhyr+D8yYbpXRn7of7e7V3+ui7womOHusPrEnqQzAyNCn41lj+1dI5RMoFGNTpGPSDkIjgcGBff0u+PKKrZpEp+VSgKr6Q8zHzlDsUYccsB9BhduthS5/acW6i2cqfycXP9n6utQ111yWV4yXfT1sbKF5reAzVBVbAooRstBNQwLdyBKQ1SJs1oBiU5gjeHNZ6Xfj3Sqo4DhDZKMT++2Jzf6Oz2INqVZMzGqloNQflOUe6oflepTtQcZHT2+8WoEstFyb+8rL5ULDummUO+6diKbqYygP4RoO30r+owaDi3IaMuL5jg2UODYJKbGjhCS2yyAKM8ogcxjaWmjOK/0lN2dUKH5o5WkSeCEYRacZToWYtKH7NG+kWjHxt5SPBqjvGPfAmt/GXoF+uYs5ExX0d7PbNAn2t7QztfAEU9OJesyFVrJcfgCq4w1Q57CEMmNqs2vWb0T4S9aJ2M+09XCD6MXhgPahy4jDEa9vFdwpvm6Z9ZRay5lCj8Go7xribeTZcr4SOyL8pRSb+wLKG6mnqeZPsBAnCteDmLszzTWoGauXhZ7zlRlauN3Yv1TNc6VW7VQ0i0H7P9LjPepHh2P+lYltnhp9d+tl/BV+1FxBttBM12ZwmrfMCW2/TavntfffSuyomt4km7P6+Hcl8N5vPuigNMfYITiyo8TLbYKJZ0ep59vgUYgCBxInx2j7vW+h3Xv34cXQdtGHZzRsp8SnoXu6oc7rkqnJvRR5boKGXRW/Vp7rDErNUyZMTcxZrcN0Jety5GYYRPsvFm/8egzNSbXKWdrd2ePvZj/yz7zb/K2F/pi/Absp/O7Za4KNZWaq2hR0OI27GWyWrXnndky9A6WBGxLPdTLbcz0zqwPfDnEGNqWR77oOjpLQzOpDJXmnE7gVTPOyXk0ueFpw9edI6X/tlB+pHS8LApr4ge0zj9okibAdUurYDGOfhRl1c482XarF7Sg+EOp+n0zjyY1UuvjJVDa5TKWQS/PhJ4taPcFm/J5kKbh5kiQ2oYlnEydgduTl1CZOxCiDJMEQoO1v5TMqZmgQAAA=",
            },
            AmountRequired = 1,
            UniqueFish = true,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Platinum Bichir"] = new List<uint>()
                {
                    45783,
                },
            },
        },
        // Export for Mission [473] - Rare Aquatic Specimens
        [473] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XTW/bOBD9KwbPIiBKlET65nrTbAA3G8Qp9hD0QFEjm4gsORTVbTbwfy+oj9iypWQbGNgecqNHw8c3w8eZ8TOaVaaYi9KU83SFps/IrhcqB7u+yEWcQYKmRlfgoKsETT3GHXSjVaGVeUJT4qCr8uKHzKoEkr3Z+u+cGutLUci1BasXnl3VOCFz0OX2bq2hXBdZgqbEdXvIr0PXGDzq7XDfJDNfV5uRwChx6RuMOpAiy0CacRxyuMt7m1ShEyWyEbyQ0B4ebXd9VuX64gnKLqGUuMER/yDo8Q+7GxEPsFyr1HwSqo7CGsrOsDRCPpRoGrQ5Dtkp7iEqb1FvhFGQSzjgEx7vC/v59LqtWv0Lc2EanQyJLmQnYN7R5fgt2N1aZEo8lJ/F90JbvJ6hi853+vZbkMV30GhKbM5GKNDegV06P6nVpdjUcc/yVQa67A6xV5+gqR+59IR9D4rtdg66+GG0aB+hvYi7YvmP2F7lplJGFfmlUHmXW0wctKg0fIGyFCtAU4QcdF2TQNdFDshpEJ62gKY2MQN4i6I078a70VDCMEOE0cj35sT6+57PcgvSaJHNK60hN2eK8gj1bLEOsj2JePD02qsRyNIUW/t8Vb5aGtjWVXTPvRXRTJ+H8iFczeFrrh4rsLjIgxhk6gHmHqSYxp7EsSAEJ27sBh7xGUkitHPQQpXmr9SeUaLpfSNPG8ALQc7HGc6ybNJsPaZ5XeiNyP4sigcL1BWQv0HUv629BPPyFlORldC9zfajDbB7pa2pgacksoWpw1waXeT1y/qv213/YPsCVpAnQj+dgVcN/EdRxdlxpI2HF/IXhz3tUZchaoded1ptx06KAs9/cRk7q+f0ymmtn9X2LDWg56Jarc1CbWyPIc2HY9HXs0almyZmFwfleaAG+1HAT3v0K/3Vzgld9elkdguPldKQLI0wle1zdhAZ0d4bWvpVyXxI4Nck8N47P6hwIeMAlDMcyxQwjRnFgvAESxJHhLM0CHwP7b51Ja4dVu9fDE2Vu39Gh+WOBhGLxgveosqFnsykSGCjpBJ5r/CR1zJ0lUBulBSZTUszjVuH2aao8p6bJcGPhwq/P+8xe1KlUyFhmdl61LHnwRuzVLBz0G8zt+8747v7od1sLXObxjqDhx2y7Yt22Zj3bkOKPVBXSkJOeerjNEkjTGMRYe66Ars+sJizNIKAoJ1zqh42HsDyQW23Kl9Nlkbosytn6A/Lh5D+dyElXuwHlPhYBFximgYCMxJyHCcUeOTFCfHFkJBsxl4vQ7dCiY0of58SNKzADyGdR0hhzEIfvBQHtg7RiHDMokRiCCUwEnDmpjBYkYIPIX0I6eteSOAJ7sWxh3nMPEwBJOZ+IjELacgpJJFw/XpwanBbivc08r9NZtPJrdAwmT1Wwig5sf+H1QZyK57D7ulTySPGsfQJx9QXDHMQFPMgTgPhSV+yEO1+Ark5pcWiFAAA",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [474] - Foodstuff Emergency
        [474] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1WTW/bOBD9KwbPIiBRJPVxc7xJNoCbDeos9hAUC1oa2URk0qWottnA/72gZMWSYidNN4cFtjeJnHnzZvg4w0c0ra2eicpWs2KF0kd0rsSyhGlZotSaGjzkNudSwWEz77aucpSSOPHQjZHaSPuA0sBDV9X5t6ysc8gPy85+12J90DpbO7Dmg7ivBofHHrrc3q4NVGtd5igNfH+A/DJ0g5FEAw//VTKzdb05kRgNfPoKow5ElyVktsuEBn7QNyOvs9Aml6I8QSQgnA9qTPduF7Janz9A1QvMRowZGzDm3RmIe1isZWHPhGx4u4WqW1hYkd1XKGX7qvL4OW4fNdmj3ggrQWXQ48PHfnxYQdK5GvkPzIRtldFFHXuTUf3DvfftWpRS3FcX4os2DmCw0KUTesP1j5DpL2BQGrgiHZM2j50EegG7+p3J1aXYNIlO1aoEU3VB3GHnKA0jnz5jP4CKdzsPnX+zRuwvnqv8rV58FdsrZWtppVaXQqquHjjw0Lw28AGqSqwApQh56Lohga61AuS1CA9bQKkrzBG8ua7sT+PdGKjgOEOE0Yn9NmKzf+Cz2EJmjShntTGg7DtlOUJ9t1yPsn2W8dHojVUrkIXVW3dfpVotLGybRnngvhfR1LwP5T5cw+FPJT/X4HARZ4wUueA4XDLAlLMAizApcB6GOWOEFcJfop2H5rKyfxQuRoXSu1aeLoEngklymuG0LCet65jmtTYbUf6u9b0D6jrGXyCa//YSut0KrMuku477pRaHBpFrOZ3zwhqtVm9x98Oe+xxWoHJhHt6M8Juul+UT94EF4cmTwYHfSZMBhyNWt0ZuT0WKGAmfTE7FGhi9EG1v59Q6LSyYmahXazuXGzcmgnZjLOPmgVCbdg65j17DbXshS8bz48XR7KZ510A6pXyEz7U0kC+ssLWbTe65MJbPj6nkh8Xw68z/1ZnTN555r0nRwI+zgvk4AcEx5STHy4RHOBNJkfMkiCgFtPvUdan9k/LuaaFtVHePqN+xKIuS8HTPujRC5ZOZ0V+VFbKcnIG1YtC/gpeqdJWDsjITpSuNC9kaTDe6VgMzRyQZvw3C4TstdpFqU4gMFqVrQl0Gzy7S+EnEdh76z7ywDwPup8eac3YrM1fGpoL9QYdSJJRWf9/RiGLyaTJNJxda55Wti2JyvgGzApU9oBamdTwAHdN1T4MkDimhhOKlz5aYMiGwoDzHge8HLAiLhFE3KJ9rjJxOcV4rYSYLWVkwVSbKYb6/9PV/0ldWQESWUYJpVHBMA1LgmMUx5nkIWcziZSDCpse1uPskGibhC0x6EYQPBAj3cZAUMaYkITgu8gzTxKehEDynPkO7783hftMDEAAA",
                "AH4_H4sIAAAAAAAACuVW227bOBD9FYPPEqALdX1zXCdrwE2DOsE+BMWCkkY2EVl0SSppNvC/FxTFWnJkpy2CYoF9k4YzZ86Mjmb4gqaNZDMipJiVa5S+oHlNsgqmVYVSyRuwkDpc0hoOh4U5WhQo9eLEQjecMk7lM0pdCy3E/FteNQUUB7Py32usj4zlGwXWPnjqqcUJYwtd7W43HMSGVQVKXccZIJ+HbjGSaBDhvElmtmm2hgF2HfwGBRPFqgpy2Qt0+27e22kZLyipFMC0qtjTp0fgOdktZqazI412vTActBp3YJdUbObPIHp0gqM6gmBQR2g+BXmA1YaW8oLQthplEMawkiR/ECgNuuaG8WvcPmrSod4QSaHOoccnPI4Lh331TCin/8KMSC0Qk/U42jv6Kn4XfbshFSUP4pI8Mq4ABgZTjm8N7Z8hZ4/AUeqqJo0pPIyVMHoJTf8u6PqKbNtCp/W6Ai5MEiWBAqV+5OBX7AdQ8X5vofk3yUn3/6nO37LVE9ktatlQSVl9RWht+mG7Flo2HD6CEGQNKEXIQtctCXTNakCWRnjeAUpVY0bwlkzI38a74SBgnCGy0YlznbE9P/BZ7SCXnFSzhnOo5TtVeYT6brWOsn1V8Wj21ksLZCXZTv2vtF6vJOzaeXng3oloyt+Hch+u5XBX068NKFwUxlFQYkjswssdGxO/sOMwSOzY8fwMHMfL8xDtLbSkQn4qVQ6B0nstT1XAD4JJcprhtKomOvSY5jXjW1L9xdiDAjIT428g7bv+CdWpAKkqMb9jZ9I42I3UyDHBK8lZvf6VcMfvhS9hDXVB+PMvI9wJ+MCazt84aospaBDmhYq2djiQPukyIDbidSfgltPdML22jKePAk/R1i6nCAyczlDo/JSup6UEPiPNeiOXdKsWiqsPjgXf3igarjeWeuiNZj01g+T1Jj6zVNX6N6PGaOozfG0oh2IliWzUFlP3i2Oh/Zyeflo2Y45/Tgj/u2/eG2fEiUnuYdd2wfFtXGaJTRw/t0PPD0McRGWWEbT/YuZZdwe9/2HQI+3+BfVnGw6ixD893a44qYvJjLOnWhJaTS5ASjKYdO65Li0KqCXNSaVao1Jqh+mWNfXATRFJjm8R/vBGF6tMDS9JDqtKjStTQRK8cXkK9hb6z1zJD6vwtxegClaWmWqjluMT2em1KMx47G9JpZ6a1f/c4wjb/pfJNJ1cMlYI2ZTlZL4FvoY6f0Z9nB72iNR7snQyz49wFtpeWICNYw/bSeZHth/FblwmcRBh3MpS43Z1tUy8M0x6GXwnLLw8xrbjuo6NS8+1YycEG2cZZDGOiyQp0P470FuRLucNAAA=",
                "AH4_H4sIAAAAAAAACu1WTW/jNhD9KwbPEiDqW7o5rpMG8KbBOkUPwaKgxJFNRCa9FLVZN/B/LyiJtqTYzibIoYfeJHLmzZvh4wxf0LRWYkYqVc2KFUpf0JyTrIRpWaJUyRospDcXjMNxk5qtW4pSN04sdC+ZkEztUIotdFvNf+ZlTYEel7X9vsX6IkS+1mDNh6u/GpwwttDN9mEtoVqLkqIUO84A+TJ0g5FEAw/nTTKzdb0xDHzs+G9QMF6iLCFXPUfcN3PfDiskZaQ8U1LshuGgqH7nds2q9XwHVS9wMGIcBAPGoSk6eYLlmhXqirCGt16ozMJSkfypQmnQlTGMX+P2UZMO9Z4oBjw/Jw0fO+EYJhwW1DVIkv0DM6JaZRgSY293dBxe5/2wJiUjT9U1+SGkBhgsmOw8a7j+FXLxAyRKsa7ZKWmHsVZEL6Ap5xVb3ZBNk/eUr0qQlQmiz56i1Isc/xX7AVS831to/lNJ0l08fRAPYvlMtrdc1UwxwW8I46YeNrbQopbwBaqKrAClCFnoriGB7gQHZLUIuy2gVBfmBN5CVOrDePcSKjjNENnozH4bsdk/8lluIVeSlLNaSuDqk7IcoX5arifZvsr4ZPTGqhXIUomtvr6Mr5YKtk2jPHLvRDSVn0O5D9dw+JOz7zVoXBRg8LIk8mzAsW/71MV25tDM9r0cO4EX5zTHaG+hBavUH4WOUaH0sZWnTuBAMEnOM5yW5aR1HdO8E3JDyt+FeNJApoH8BaT5by+h3q1A6UzMdeyWWhwfR7oDGeelkoKv3uPueD33BayAUyJ370b4TdRZeeA+sHDD5GBw5HfWZMDhhNWDZNtzkaLA9Q4m52INjC5E6+y0WqeFAjkj9WqtFmyjpwZuN8Yybh4ItWzHkv7oNdwTXdWLgmQ8XS5Oaj3cTT8xwvkK32smgS4VUbWeXPr1MFbTr4nml7XxvwQ+JAFz5v47z7zXs6IsiUIfgx0mmWP7NA/tmOaO7ReQkTx2aZAEaP/NNK3uhfl4WGj71uML6jcwP4gS73wLu5GE08lMimeuCCsnV6AUGbQzfKlKtxS4YjkpdWl0yNZguhE175mdejUFyfjl4A0fdbEOXMuC5LAsdYsyCb26V+MHU7C30H/m/X0cfx8eetpZr8x0VVt1PpNtOworU9T+ZEQpIlzwvx/9yLfdb5NpOrkWglaqLorJfANyBTzfoT5OD/uE8nsqjYPIDQta2A4GsH0n9OwYZ7GdYew4XoQzkkWNSlvcLq+GCb7ApBcBUzcMo8Sz84T4tu+GgR1TiOwwK7LMw15BIET7fwGbARl41A0AAA==",
            },
            AmountRequired = 18,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Darkside Bass"] = new List<uint>()
                {
                    45791,
                    45801,
                    45879,
                },
                ["Grand Crowntail Betta"] = new List<uint>()
                {
                    45793,
                },
                ["Lunar Sisterscale"] = new List<uint>()
                {
                    45792,
                },
                ["Moongill"] = new List<uint>()
                {
                    45740,
                    45750,
                    45790,
                    45800,
                    45878,
                    45930,
                },
                ["Moonrock Candy"] = new List<uint>()
                {
                    45739,
                    45749,
                    45789,
                    45799,
                    45871,
                    45877,
                    45929,
                },
            },
        },
        // Export for Mission [475] - Precise Water Survey
        [475] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1Wz3ObOhD+Vzw6oxnAIBA31y9NM+OmnpI3PWR6ENJia0LAFSKtm/H//kb8iMGGZJrx4R16g9Xq07erT7v7jBaVLpas1OUy3aDoGV3lLMlgkWUo0qoCC5nFlczhuCi6pRuBIjekFlorWSip9yhyLHRTXv3iWSVAHM3G/9BgfS4KvjVg9YdrvmocElroene3VVBui0ygyLHtAfLr0DUGDQY77DfJLLfVY8fAc2zvDQrdriLLgOuJjHiO7fR3uW+zKJSQLJvAc1xCBjn22m0fZbm92kPZC8A/CcD3BwGQ7g7YA8RbmeoPTNZhGEPZGWLN+EOJIr/NKgnPcfuotEVdMy0h59DjQ073kWFC3W6rkr9hyXSjjDGZkfAMzD25nXkLdrdlmWQP5Uf2VCiDNzB00c2tof0r8OIJFIock7MJCt7gwC6dH+Tmmj3WcS/yTQaq7A4xdy9QNA9s74z9ACo8HCx09Usr1r5DcxF3RfyT7W5yXUkti/yaybzLLXYstKoUfIayZBtAEUIWuq1JoNsiB2Q1CPsdoMgkZgRvVZT63XhrBSWMM0QYTaw3J9brRz7xDrhWLFtWSkGuLxTlCerFYh1lexbx6Om1VyOQWBc783xlvok17Oq6eeTeimihLkO5D1dz+DeXPyowuChMGCSu42JGHYI9QjycCCpwEgYpCX2HOMRGBwutZKm/pOaMEkX3jTxNAC8EKZ1muMiyWbP1lOZtoR5Z9qkoHgxQV0C+Aav/jb0E/fIWU5aV0L3NdtEE2L3S1tTAe44bhCYTLWisVZH3Wtyf7l/BBnLB1P4CzGwT+z9FlWSnsTYeLqEvDkfeky5j1Pped0rupk4KfHf+4jJ11sDpldNaP6PuRapBLVm12eqVfDRdxmkWTmVfzxeVatqY+egV6JEqPA98et6mX2mxZjbo6k8ntK/wo5IKRKyZrkynM8PHhPq6OwvoqJjGbvY1yfyVwJ9J4L133qtxvkd95kCKgaeAPRAEMxI6mNkkDEM6Fy4AOnzvilw7oN6/GJo6d/+M+gXP8wMaTpe8a5YxriWffWK/h9XZeS07NwJyLTnLTErMUY3D4rGo8p7b2NTp09MJYz4c/kwxiyuVMg5xZkpTFwj13xis/IOF/jdj+7FNvrs5ms3GsjRZrRPab5dtkzSfjfnoNibentACzgn3Q4KBhSn23LmDQ09wHIJtBzwkNrVTdLDOhUSmA1gX2X5XlbOFKiGXXF5cS+8Wz7gI/2ppfREteTbzIaAUz5PEw14gXBwyEeBAMMLTRKQupXXRanBbivde4H+fLaLZWgGXJcy+MQ1qFlfqCfbDyc9PuO3ZJMXC8R3sCRcwJWmARZKEnm/TOU84OvwHPvCR4iEQAAA=",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [476] - Fine-grade Aquatic Processing Materials
        [476] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XS2/bOBD+K1pe9iIt9KApyTfHTbIB7GxQZ9FD0MNIGtmEZdGhqKZp4P++oB62JD+6LXLooTd5OPzmm+G8/EYmpRJTKFQxTZdk/Eauc4gynGQZGStZokn04YzneDhM2qO7hIzdIDTJg+RCcvVKxo5J7orrr3FWJpgcxFp/V2PNhYhXGqz6cPVXhcMCk9xuH1cSi5XIEjJ2bLuHfBm6wgj93g37u2Smq3LTMqCOTQcUwgGF9pbIMozVmYhQx3a6t9zvsxAy4ZCdwXNcxnoxps21G16srl+x6DgwGjgwGvUcYO0bwBoXK56qK+CVG1pQtIKFgnhdkPGoiSoLjnG7qGGD+gCKYx5jhw8b3mP9gLrtVcm/4RRUnRmn0owFR2Du4HW8BuxxBRmHdXEDX4TUeD1B651n9uUfMRZfUJKxo2N2hgLtGWzDecWXt7Cp/J7kywxl0RpxTfKJq9Uig20LdQLZ82165FzPUrDbmeT6q5LQlKl+p0exeIHtXa5KrrjIb4HnbegtxySzUuIciwKWSMaEmOS+4kjuRY7ErBFet0jGOm4n8GaiUD+N9yCxwNMMiUXOnNcWq/MDn8UWYyUhm5ZSYq7eycsB6rv5epLtkccnrVdadf4slNjq6ub5cqFwW7XVA/cmxybyfSh34SoO/+b8uUSNS9wgDilz0IoS8C3qpKkV2RFaHnVSN0KWQJyQnUlmvFD/pNpGQcZPdXpqB/YEw/A8w0mWGfXVIc17ITeQ/S3EWgO1/eUTQvW7riR9WqDSnrQ1Nee5lj7yja5nl/5lm2QOX7uyQMt09+/rOt5e3tN3Kv3GVM2POq4f6FA2rBZKiryqzUZtX+gpZMW+8E/QHcDaXgd1hkvME5Cv7wX8QZRRtg9hT8Nl4V7hyJtjlVPUulqPkm/PWfJHrrdXOWerp3TBWqOni2aSKpRTKJcrNeMbPduc+mBYTdVWU8p6eOqPzlg42aFH4XAGXlwv9EbStrU2fz/ic8klJgsFqtTzVa88w6QevJkfXkqx/50yv1Pgx1KgfXP6g2/eaZ3M9cMo9Bwr9m3Pon7KLGCQWm4Uh9SLvdDBiOw+t72zWYuf9oK6fT69kW4fpaPA9oaddGJsYJnzlMeYKyPluRFJhHiFhaFWaLyAQvlnYRSlTCHGPw6N9wPItTGV4iVXwDPjCpWCXg92LsX0LsFc8RgyHUhNsFaYbESZ99Q07XC433j9zVR30kVNsF6WWn+Pym649Y12Jvll/lMchvRPj2Z9WUumOoxVBLvDuhnR+rMWH9RO5XhvlEOAkAYWiwAsSmNmRaPAsTwKDvMg8Dw6IjtzmG/+pck9FyKXIl4bU8iT118ndU79K/udSeY7ZVISIfgRhFaMmFp0FPlW4HuRxZI0cRl1HYS46mw1bkPxifrsszEZGzc8R+tWQoLG5LkExWPjQYoYi4LnS2OuuxWHTC+DHZtBFMXMZp4VQZJYlNrUimLGLJuxMB2FqR3YCdn9BzzfoCjPEAAA",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [477] - EX: Large Aquatic Specimens
        [477] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1WS2/jNhD+KwZPLSAWEkU9b16vNxvASYM43W0R7IEiRzYRWXIoaps08H8vqEcs+YkNcijQvVHDmW++GQ1n5gWNK11MWKnLSbpA8Qua5izJYJxlKNaqAguZy5nMYXspuqtLgWISRha6UbJQUj+j2LHQZTl94lklQGzFRn/TYF0VBV8asPpAzKnG8UMLXazvlgrKZZEJFDu2PUA+DV1jRMHAwj5LZrKsVh0D6tj0DIXOqsgy4Lpn6PTVyHm3hRKSZUdS6hDfHySVtmafZLmcPkPZc+ztMPa8AWO/Szp7gPlSpvoDkzVvIyg7wVwz/lCi2GvT6If7uH3UqEW9YVpCzqHHx9+184cZJJ2pkv/AhOmmFA7VlR/ugZGd3+G2YHdLlkn2UH5i3wtl8AaCLjrXGspvgRffQaHYMTk7QoEOHHbp/CAXF2xVxz3OFxmosnNi/r1AsRvYdI/9ACrcbCw0fdKKtQ/P/Ii7Yv43W1/mupJaFvkFk3mXW+xYaFYpuIKyZAtAMUIWuq5JoOsiB2Q1CM9rQLFJzAG8WVHqN+PdKCjhMEOE0ZH7xmN9v+UzXwPXimWTSinI9TtFuYP6brEeZLsX8UHvtVZTIHNdrM3zlflirmFdN8ot97aIxup9KPfhag5/5PKxAoOLnDDhLhcRdsLExZQkAifMITixCfG4F0WCA9pYaCZL/XtqfJQovm/K0wTwSjCKjjMcZ9moMd2leV2oFcs+F8WDAeoayFdg9beRl6Bf32LKshK6t9lemgC7V9qKGnjqBKYxdZhzrYq8N9IOmF/J3Ejv5Mq0Adf+zbbQFXvqywIj23Fjuz03M1hALph6fgf+NfDHokqy3Yw0GsSPXhW24R1VOUStr3Wn5PqYp8Aj7qvKMV8DpRPeWj3zBsapBjVh1WKpZ3JlZpHTXOw+jnrtqFQz7Myh18YP9Go38KL96X1iEJuVoetSXTnewmMlFYi5Zroy89DsJEdq9EzN/WjJ/CyBHyuBt/7zXidMXZumzAmx7UYBpqEvcMQJwTbnwmcRScME0OZb1wrbvfX+VdB0w/sX1G+L1AvJicb4kamH0YSp9aAxOqcycykg15KzzKTDuGkUxquiygdqxnm0u3S4w30wNJ4qlTIO88z0oYO7F/Ui78zq5W0s9J/Z5LeD9M3j0xgbycRktU5of6C2Y9QcG/FW7VDh9sctDXwhfA97zE4wdX2BmU19zFMe+pQ5vhc5aGPtF1FwPIC5ZoorCepnEf0/iojzJCLgBtiHhGPKicCJlwps88T3vYjwJE3qTtXgthTvaRB8G03/jEczphYwGj9WTEs+MquqXEFejn75PB1/+Wt0e33x63BJJG4KwFOBwU4opgl4OPQiF4eR54aE0sj2Q7T5F4QLwRM9EAAA",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [478] - EX: Assorted Alchemical Materials
        [478] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1WS2/bOBD+KwZPu4AE6EFJlG6q100DONkgyr4Q9EBJI5uILLok1TQN/N8X1MOWFNtpgxz2sDeKHH7zzfDTzDyjuFZ8TqWS82KFome0qGhaQlyWKFKiBgPpwyWr4HCY90eXOYocEhroRjAumHpCkW2gS7n4lpV1DvlhW9vvWqwrzrO1BmsWjl41OD4x0MX2bi1ArnmZo8i2rBHyeegGIwxGN6xXyczX9eZEYNi28ISRM2HUg/CyhEz1kWDbsodmzussuMgZLU8QsR3fH+UYd9c+MrlePIEcOPYmjD1vxNjv34A+QLJmhfpAWcNbb8h+I1E0e5Ao8rqs+uQl7hA17FBvqGJQZaeUgm3Ln8L444Q6PZJg32FOVSuUnoT/ynO43e27NS0ZfZAf6VcuNMBoo4/ONcb7t5DxryBQZOucHVO6T7QiBg77dH5gqwu6aeKOq1UJQvZO9NvnKHIDC79gP4Iiu52BFt+UoN1/qB/ijiePdHtZqZopxqsLyqo+H6ZtoGUt4AqkpCtAEUIGum5IoGteATJahKctoEgn5gjekkv1ZrwbARKOM0QmOnHeemzOD3ySLWRK0HJeCwGVeqcoJ6jvFutRti8iPuq9sWoFkii+1b8vq1aJgm1TNw/cOxHF4n0oD+EaDn9U7EsNGhdlaWDTkPqmR1PPxJRmJnED2wQo0iL3fS/NLbQz0JJJ9XuhfUgU3bfy1AHsCYbhaYZxWc7aq1Oa11xsaPmJ8wcN1BeQv4A23+1PqE8lKB1J/zt2Wy0OtgNdgfrLiRK8Wv3MdcsdXF/CCqqciqefRviN12m55z6ycPxwb3Dgd9JkxOGI1Z1g21OeAs9x9yanfI2Mznjr7LRa40KBmNN6tVZLttFdw24PpjJu5oVatG1JLwYF90hVdQMvnPbZ8Fzj1r2+rye9cG7hS80E5ImiqtadSw8TUzX9mGh+WBv/S+BNEjj15meHtd2oZnmZhVMny0zwiG9i2yNmaGWh6eOcZB7FjgUB2n3ui1Y3cN7vN9q6df+MhgUMe8TBp0vYsq6okI9c5LOk0qobVjL7XIIuc6gUy2ips6K9tQbxhtfVwOzYwOSF06HBHc9zRDuuRUEzSEpdnfpYQu+VWcnbGeg/M4kfOt+b+13ySLd6Z66z2iR02AG7vqeX7fbB7Jh+B1qjxHedwMOmC0Vu4gICMwxS3ySFR/wisFOaFo3WWtyO4j0OyOfZ4u9oFkvJhYJ8FpfZGjZaB7MrqkAwWsrZL58W8Z//zG6vL34dt2VCCmz7hWUWAVATh8Q2Cckcs3DSIsDEczKw0O5fW+14Ib4NAAA=",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        [479] = new FishingTools
        { 
            // Need Info on this one
        },
        // Export for Mission [480] - EX: Foodstuff Emergency
        [480] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACs1X247iOBD9FeTnZJWLc3HeGJbubQl6WwOjXak1DyapgEWIGduZabbFv4+cCySBpmHUI+2bKbtOnarUjVc0LBQfUankKF2i6BWNc7rIYJhlKFKiAAPpywnL4XiZNFcPCYqckBjoSTAumNqhyDbQgxy/xFmRQHIU6/f7CmvKebzSYOXB0acSxw8NdL+drwTIFc8SFNmW1UG+DF1ikKCjYb1LZrQqNg0DbFv4HQqNFs8yiFVL0W4/c943y0XCaPZGSG3H9ztBxbXaHZOr8Q5ky7DXY+x5HcZ+E3S6htmKpeoTZSVvLZCNYKZovJYo8uow+uEpbhuV1KhPVDHIY2jx8ft6fjeCTqMq2H8woqpKhcZqX9vpxd+ttecrmjG6lnf0OxcaoCNo3HGNrvwzxPw7CBTZOkjnctkPdQq0DDbx+8SW93RTOjrMlxkI2RjRHztBkRtY+IR9Byrc7w00flGC1pWmIz/nsx90+5CrginG83vK8iYepm2gSSFgClLSJaAIIQM9liTQI88BGRXCbgso0oE5gzfhUv0y3pMACecZIhO9cV9ZLO+PfGZbiJWg2agQAnL1QV72UD/M17NsTzw+a718VSXITPGtrleWL2cKtmVnPHKvk2goPoZyG67k8CVn3wrQuCiObd+3PWJalheaGDA1aRjHZhJQxyY49T0CaG+gCZPq71TbkCh6rtJTO3AgSMjbDIdZNqhU+zQfudjQ7C/O1xqo6Rj/AC1/V0WobyUo7UlTjrWowsF2oFtOozxTgufLW9Qtt6U+gSXkCRW7mxH+5MUiO3DXL+ZsA6LXSaYsP1zpuv/DMtCUvrRkrq1lHQuOTw4Gjv7VTw4GUppJuKDZce165S8S5oJtqzg0blSS3+Ns4Dk6npWJG93t6N7ucK2uy3OYKhAjWixXasI2ei7a1UW/bssVqBDV4NWH1oQ5M0bcwCP9+XlxF9HrS9NAm0r5DN8KJiCZKaoKPZv1ftQvn+uq5OpiuC7nr8vZ65Kz/eo04a5NmGsz4zelQPPN8Y3fvNWkrYUV4sTFJo7BNnHsBiYhbmim4Ftx6i88qpv016ZL1zv080FQNernV9Tu2NgLwgs9e8p5Lni8Hoxonuw6jdu+FJ6HBHLFYprpmGhb1YPhhhd555lmQPpLkdtdUENtqRApjWGW6e57diPGpwXVXw29vYH+N38tjoP+l8f77AfdaslIR7UMaHvg12NeHyvx8dm57G1lWuASTDycms7CJyamLjFpbKUm9iwvxC4mgReXmVbh1hSfcWh9HYz/jQZ3nCdSFWk6GG9ALCGPdeq0DNgJ8VxMXdPCVmLihQ1muCDYdCzb9lwSegkO0P4n1PWsUnkOAAA=",
            },
            AmountRequired = 18,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Arsenic Axolotl"] = new List<uint>()
                {
                    45836,
                    45883,
                },
                ["Etheirys Croppie"] = new List<uint>()
                {
                    45839,
                },
                ["Moon Mora"] = new List<uint>()
                {
                    45840,
                },
                ["Sunny Jellyfish"] = new List<uint>()
                {
                    45837,
                    45884,
                },
                ["Universal Darkfin"] = new List<uint>()
                {
                    45838,
                    45885,
                },
            },
        },
        [481] = new FishingTools
        {
            // Need Info on this one
        },
        [482] = new FishingTools
        {
            // Need Info on this one
        },
        // Export for Mission [483] - Aquatic Inspection I
        [483] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XTW/bOBD9KwbPJiBKpET65nrTrAEnG9RZ9BAUC0oa2URk0aGottnA/72gZMWSv9IGAXYXm5tMzrx5M3qaGT+hcWX1RJa2nGQLNHpCF4WMcxjnORpZU8EQucuZKmB3mbZX0xSNfC6G6MYobZR9RCMyRNPy4nuSVymku2Nnv2mwrrROlg6sfvDdU40T8iG6XN8uDZRLnadoRDyvh3weusYQUc/De5HMZFmtTiRGiUdfYNSC6DyHxLaZUOKRrpn/MgttUiXzE0SIH4a9GtOt20dVLi8eoewEZnuMGesxDtt3IO9hvlSZ/SBVzdsdlO3B3MrkvkQjtq1qyA9xu6hii3ojrYIigQ6fcN8v7FfQb12N+hsm0jbKaKPue/t79Q+23rdLmSt5X36UX7VxAL2DNp1g2D//BIn+CgaNiCvSMWmH3EmgE7Ct3we1uJSrOtFxscjBlG0Q97JTNAoijx6w70HxzWaILr5bI7cfnqv8rZ5/k+tpYStllS4upSraemAyRLPKwBWUpVwAGiE0RNc1CXStC0DDBuFxDWjkCnMEb6ZL+2q8GwMlHGeIMDpx30Ss73d85mtIrJH5pDIGCvtGWe6hvlmuR9keZHw0em3VCGRu9dp9r6pYzC2s60a5474V0di8DeUuXM3hz0I9VOBwUeZJ4UcEMAAnmPIgwZwHAeaMhZCxEDwKaDNEM1XaPzIXo0Sju0aeLoFngkKcZjjO80Hjuk/zWpuVzH/X+t4BtR3jM8j6d/MRutsSrMuk/Ry3Rw0OJZFrOa3z3BpdLH7F3Qs67jNYQJFK8/jLCL/pKs6fufcs/FA8G+z4nTTpcThidWvU+lSkiPnBs8mpWD2jM9G2dk6t48yCmchqsbQztXJjgjQX+zKuF4TKNHPIPXQa7pGuGkRMHA7WMzPSDfe2n7TC+QQPlTKQzq20lRtVbnvYV9PPieantfEugVdJ4LXvvNOzIkFTmQqOPS+IMI1ogmOPZ5iKzONcstijEdp8aZvWdsO8ez5o+tbdE+o2MMoit4ycamFXWhfflmrd62DkXGGmKRRWJTJ31XBRGoPxSldFz8zFFvvbQdDf1LiLVJlMJjDPXRs6vqMywV7YkdhmiP41K/du4r16zjlndzJxVa0L2p1823nnHpvjndkx3XY0BgEnkDGGSRbHmDIZ4lgQhlkALGIyECxO0GZ4oCF2ZgzOqkKawTzRZq108a6k/4eSJJOM0DDBEY1DTCHiWLAwxSISCQ88xhLqHVFS/XfnvJLGBaz2M/lHhfRfVE772lv1HyrJlcI//S7mFvJcmsFnbVboqI52oinbKHvSkoUu/rqjPMDky2A8GowfKmlVMpgWpfsjoXQxmL5OfYQIwZI0wYxkKaaMRTgGn2AGaeLL1PfiWB5V35k+dqPzx3VVDuZVnlVG9bf6dwG+CxD1BmmWCc4llmlGMU0yhmUoGY5JxgNCgfNM1Mtag7vNqabin6PS7bCCM0ZYgn1CE0wFz7DkAJiIKBUhAZ/GEdr8AA7htI3cFAAA",
                "AH4_H4sIAAAAAAAACu1XTW/jNhD9KwbPJiCJ1OfN62ZTA04axCl6CBYFSY1sIrLoUNTupoH/e0HJiiVbdnaDHLZobjI58+bN8HGGfkaTyqgpK005zZYoeUYXBeM5TPIcJUZXMEZ2cy4L2G+m7dYsRYkXxWN0o6XS0jyhxB2jWXnxXeRVCul+2dpvG6wrpcTKgtUfnv2qcYJojC43dysN5UrlKUpcx+khn4euMeKw5+G8Sma6qtYnEqOuQ19h1IKoPAdh2kyo67hdM+91FkqnkuUniLheEPRqTHdun2W5uniCshPYP2Ds+z3GQXsG7AEWK5mZT0zWvO1C2S4sDBMPJUr8XVWD6Bi3ixrvUG+YkVAI6PAJDv2CfgW91lXLf2DKTKOMNuqht3dQf7LzvluxXLKH8jP7qrQF6C206ZBxf/0WhPoKGiWuLdKQtIPISqATsK3fJ7m8ZOs60UmxzEGXbRB72ClKSOjQI/Y9qGi7HaOL70az3cWzlb9Ti29sMytMJY1UxSWTRVsP7I7RvNJwBWXJloAShMbouiaBrlUBaNwgPG0AJbYwA3hzVZo3491oKGGYIcLoxH4Tsd7f81lsQBjN8mmlNRTmnbI8QH23XAfZHmU8GL22agSyMGpj76sslgsDm7pR7rnvRDTR70O5C1dz+LOQjxVYXETCkJOURthJBcMUgOHISTkOY9/lWRwR4jloO0ZzWZo/MhujRMl9I0+bwAvBOD7NcJLno8b1kOa10muW/67UgwVqO8ZfwOrfzSW0uyUYm0l7HXdLDQ51Q9tyWueF0apY/oy7Qzruc1hCkTL99NMIv6mK5y/cexZeEL8Y7PmdNOlxGLC603JzKlLoe+TF5FSsntGZaDs7q9ZJZkBPWbVcmblc2zHhNhuHMq4fCJVu5pD96DTcga5KQj8+HqxnZqQd7m0/aYVzC4+V1JAuDDOVHVX29XCoph8TzQ9r40MCb5LAW8+807PCIGNeCB6OiRthChHHUcZTzDlljGcBESFD2y9t09q9MO9fFpq+df+Mug2M+qF9jJxqYVdKFd9WctPrYO65wsxSKIwULLfVsFEag8laVUXPzMaOD18HpP9Si2ykSmdMwCK3bWj4jerH/itvJH87Rr/Mk3s/8d4856yzXZnaqtYF7U6+3byzn83y3mxItx2N+VQQwoIAE0o4pq5HcJSmgP1QpL4TCQHMRdvxkYb8M2NwXhVMjxZC6Y1UxYeS/h9Kin3iCN+JMSPgY8qcELMgDDAn4IjAc1nq8gElRU54OoFplRvQo4kuoZBC/jpS+i9qpz34Vv/HWrKloGdOQ5VrKUZzALFCg0ray6ZsoxyIixWq+PueRgR7X0aTZDR5rJiRYjQrSvtXQqpiNHub/nhEwU5M7BPuYZpRjiPKHRxHns8dQokb0EH9RWc6GVtDYZSu1qNbWCqjZNm/UB8i/BAh6oiQAfjC5Sn2WSgwFVmAY8h8HFBOmRMLEUZu/WRrcHc51VTcc1S6EzvmnHCPYCBRjCkXHo49AJwSIUSa8Yz6Htr+C4GLjjLiFAAA",
            },
            AmountRequired = 4,
            UniqueFish = true,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Culter Arsenici"] = new List<uint>()
                {
                    45807,
                },
                ["Lamentorum Regotoise"] = new List<uint>()
                {
                    45808,
                },
                ["Lunar Anemone"] = new List<uint>()
                {
                    45806,
                },
                ["Lunar Scorpion"] = new List<uint>()
                {
                    45759,
                    45773,
                    45794,
                    45804,
                    45815,
                    45865,
                    45895,
                    45923,
                },
                ["Moonwhip"] = new List<uint>()
                {
                    45760,
                    45774,
                    45795,
                    45805,
                    45816,
                    45866,
                    45924,
                },
                ["Polypus Sulfuris"] = new List<uint>()
                {
                    45809,
                },
            },
        },
        [484] = new FishingTools
        {
            // Need Info on this one
        },
        // Export for Mission [485] - Fine-grade Water Filter Materials I
        [485] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1WTW/bOBD9KwbPEiDqW7q53iQ14KRBnUUPQQ9jaSQTYUSXpLrNFv7vC0qibfkjKYIc9yRqZvjmDTl85G8ybbWYgdJqVtUk/02uGlhxnHJOci1bdIhxLliDe2dpXfOS5H6aOeReMiGZfiE5dchcXf0qeFtiuTeb+G2PdStEsTZg3cA3ow4nTh1ys3lYS1RrwUuSU88bIb8O3WFkyWiG9yaZ2bp9vlBYSL3wDUYWRHCOhbaVhNSjh2H+2yyELBnwC0SoH8ejNQ6HaddMra9eUB0kjo4YR9GIcWz3AJ5wuWaV/gSs420MyhqWGoonRfJoWNU4PcU9RM0G1HvQDJsCD/jEx/Pi8Qr6dqpk/+IMdN8Z59osTk/A/KPtCAawhzVwBk/qGn4KafBGBltd4IztX7EQP1GSnJo1u0AhHCW0y/mJ1Tfw3NU9bWqOUtkkZu9LkgeJF56wH0Gl261Drn5pCcM5NBvxIJb/wGbe6JZpJpobYI1dW5c6ZNFKvEWloEaSE+KQu44EuRMNEqdHeNkgyc3CnMFbCKXfjXcvUeF5hsQlF/x9xs6/57PcYKEl8FkrJTb6g6o8Qv2wWs+yPan4bPYuqm+QpRYbc3xZUy81bjrd3HMfmmgqP4byIVzH4e+G/WjR4JKy8gB9im4R08INYVW4K0jRharK4sz30ioJyNYhC6b0l8rkUCR/7NvTFLAjmGWXGU45n/RTj2neCfkM/LMQTwbICsg3hO7f2BXq3VmsgCu0Z3NwmgLtKR1MPXxIEyNMFnOppWjqD0D1ggPUBdbYlCBf9rL1hwh/iXbFjyvtI/w42wWc0D4NGXE4E/Ug2eZSpiTyg13IpVyjoFeyDXGmt6eVRjmDtl7rBXs2dwztHcdN370uWtlfYmZwIM9nNDhIouz0Vn7lgjUvA6s+ts2+4o+WSSyXGnRr7jnz9LjQe3/WS2/3xv8t8K4WmL9zzw8UjhYxLYuocjNagBuuMHRTHzPX9yEokjgJE5qR7XcrccPz9HFn6FXO/PeaOmjaY5hG3yfTfHLNGnRrCSVOvoFGOblm3HxuzQ8DribzseSmUeAX2SpwKa4CN/So50IZlG4aVckqDMMyrYBs/wPTujovmgsAAA==",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        [486] = new FishingTools { },
        [487] = new FishingTools { },
        // Export for Mission [488] - EX: Coexisting Species I
        [488] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XbW+jRhD+K9Z+aiU4sbALLN98rpOL5KTROb1rFd2HBQZ7FQy+Zbkmjfzfq+XFBgyJekqlSr1veHb2mWdm583PaF6qfMELVSySDQqe0TLjYQrzNEWBkiUYSB+uRAanw7g9uopRYPvMQLdS5FKoJxRgA10Vy8coLWOIT2Ktf6ixrvM82mqw6sPWXxWO6xvocn+3lVBs8zRGAbasHvLL0BUG83o3rFfJLLblbsIxgi0yYOTSPqMWJE9TiFTrCcEW7qrZr7PIZSx4OkEE267bizFprl2IYrt8gqJjmA4Y0z5jt30D/gDrrUjUey4q3lpQtIK14tFDgQLaRNX1z3G7qKxBveVKQBZBh487vOf2I2i3V6X4CxZc1ZkxlmaufwZmDxLEacDutjwV/KG44N9yqfF6gtY7x+jLP0KUfwOJAqxjNkGB9Ay24XwvNpd8V/k9zzYpyKI1ot8+RoHjWeSMfQ/KPxwMtHxUkjd1qB/iLl//yfdXmSqFEnl2yUXWxtbEBlqVEq6hKPgGUICQgW4qEugmzwAZNcLTHlCgAzOCt8oL9d14txIKGGeITDRxXluszk981nuIlOTpopQSMvVGXg5Q38zXUbZnHo9ar7TqBFmrfK/LV2SbtYJ91TdP3Jskmsu3odyFqzj8lomvJWhcxGhCkxA7ZhRGxCQec02OOZi2w3CSELCSOEYHA61EoX5NtI0CBfd1emoHjgQZm2Y4T9NZfXVI8yaXO55+yPMHDdQ2kM/Aq991EerTApT2pC1HLboTO5CDMr0W2fFI1+c7y0DX/LEjsy0t021/7L6W9zHYO7cR92CwrWEaZrU7BNueryPfOLFWMs82b+BGTXngBv1nbtR8R/ygr/qxgg1kMZdPr7pyBvFLXobp8TV7KrbLjgqnSE2q9EiMaN1JsZ+y5FHbOapM2eopvWCt0dP1O08UyAUvN1u1Ejs9R3F9MCzsaoMqZT2o9UdnBNXTgbLhqvHi7qLXnbaltrXzEb6WQkK8VlyVenjrfWpYUINX8thovg60LGcqG8YUf7z5v/bmnbYdxZELmLmmSwg3SYKZGZLQMhm2fJvaLlg4RIcvbd9udu77o6Bu3ffPqNvDCfWpP93FF3nIUzWrbnT7OH4pNlcxZEpEPNUB0YZqhfkuL7OO2tj+TdlwZXL626xuMetSJjyCdapbUesGo69sivRgoP/M/5DT3P/uaa8va8lCR7UKaHf+N1Nff9bik9pY6nbSDEPoEtvxTDv2mUmAWSbziGd6DvMJtyNwsYUOxnkaudMOXPKUR0pEs4uUS0h+ZNP/Jpsii3oWppbpQxibhNHEDD2KTQ7EtbDj+B71qqZV4zYU74nvf5ktfw9mixweRaFEtpnp/VpAMbua/fRhOf/0x0xmm5/7m63HwaWcENPjPDIJptxkCaEmZlYcWzEAxC46/A13tRnrAREAAA==",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
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

        // - - - - - - - - - - 
        // Phaenna
        // - - - - - - - - - - 

        // - - - - - - - - - - 
        // D Rank
        // - - - - - - - - - - 

        // Export for Mission [965] -  Aquatic Foodstuffs
        [965] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACrVW32+jOBD+Vyo/gwTEOMBbNtt2K2W71abVPazuYYKHYJXg1DZ721vlfz8ZcANJ2m5P7RvMj2++Gc+M/ZvMGiPnoI2eF2uS/SbnNawqnFUVyYxq0CNWuRA17pXcqa44yaIk9ciNElIJ80iy0CNX+vxXXjUc+V5s7Xcd1lcp89KCtR+R/WpxWOKRy+1tqVCXsuIkC4NghPwydIuRTkcewatk5mWzeSYxGgb0FUYORFYV5sZlQsMgHJpFr7OQiguoHAAL6QiA9mYXQpfnj6gHgeIDhnE8YshczeEel6UozCcQLU8r0E6wNJDfa5LFfRVZcow7RE171BswAuscB3zYoR8bVyxyrkr8i3MwXSe4qIfe0UG9J733bQmVgHt9AT+lsgAjgUtn4o3l3zGXP1GRLLRFOtXKLLFHPgjo6vdJrC9h0yY6q9cVKu2C2MPlJJtMA3rEfgSV7HYeOf9lFPSDZit/K5f/wPaqNo0wQtaXIGpXDz/0yKJR+BW1hjWSjBCPXLckyLWskXgdwuMWSWYLcwJvIbX533g3CjWeZkh88oy+i9jq93yWW8yNgmreKIW1eacsD1DfLdeTbI8yPhm9teoaZGnk1s6rqNdLg9t2Me659000U+9DeQjXcrirxUODFpcAcuAspT6LgPs0YCsfEAIfUhYldAIU2YTsPLIQ2nwrbAxNsh9de9oEnoZ7ypIXOM6l3khTnt00W7Bw11JtoPoi5b0FcJviL4T2vxs+q9VobAZuDHtRlyYNp3bVOOelUbJev8U9mAzcF7jGmoN6tAi94dMSKKDS6L0N+E7jZ9n09s6wk7g8R24Rs9l0BvtcnjX5E74nnO803iqxHbPqJKdZTePIZtOZPMdrZPR2Zr27HYpZYVDNoVmXZiE29jYKO8XhtLTvjkZ11539GOz1E8t7Mo3T49vwhavXvhnc2nJ9+h0fGqGQLw2Yxt6I9lFy2Lx/1qNvbcWR4cd20dDq4zvjg1rgqj/z4zfay2c+WI0cU56u0olPOQY+DQPwEwrMD/MoLIq0CIOYkt3fbjf2D9cfT4JuPdr/bhn3q/DzmX82e2jAiPzsQkquTVMUeryVMUynNOaRz4Kg8GlCIz+N2cpnuErCZIWcspDs/gO1tchrngsAAA==",
            },
            AmountRequired = 5,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Eelsplorer"] = new List<uint>()
                {
                    47423,
                    47432,
                    47441,
                    47449,
                    47457,
                    47520,
                },
                ["Lehr Brotula"] = new List<uint>()
                {
                    47424,
                },
                ["Macrobrachium Phaennense"] = new List<uint>()
                {
                    47422,
                    47431,
                    47440,
                    47448,
                    47456,
                    47519,
                },
            },
        },
        // Export for Mission [966] -  Efficient Large Specimen Procurement
        [966] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACuVWTXPaMBD9K4zOdscyxpZ9I4SkmSEpE8j0kOlB2GuswVhEktOkHf57R7YFGEigHW692fvx9u1qd6XfqF8qPqBSyUE6R9FvNCzoLId+nqNIiRIspJUjVsBWmRjVXYIil4QWGgvGBVPvKMIWupPDtzgvE0i2Ym2/rrHuOY8zDVZ9uPqrwvGJhW5X00yAzHieoAg7Tgv5c+gKIwxaHs5JMoOsXBoGHna8ExSMF89ziNWOI941c0+H5SJhNDcAPvZaAF5jdsNkNnwHuROot8ew12sx9E2R6QImGUvVFWUVTy2QRjBRNF5IFPWasvnkEHcXNWxQx1QxKGLY4ePv+/ntirnGVbBfMKCqPvpjfeSTAzB3r/zdBmya0ZzRhbyhr1xovJbAZNe12vJHiPkrCBRhXbMPKHitgKacV2x+S5dV3v1inoOQJog+6wRF3cDxDti3oMh6baHhmxK0GTR9EFM++UlXd4UqmWK8uKWsMLW1sYVGpYB7kJLOAUUIWeihIoEeeAHIqhHeV4AiXZgjeCMu1T/jjQVIOM4Q2egDfR2x0m/5TFYQK0HzQSkEFOpCWe6hXizXo2wPMj4avbKqG2Si+EqPLyvmEwWrajFuuTdN1BeXobwLV3F4KthLCRoXQdBNu2Ec2GmIA9tzw8SmSULsMHUCl2DHxz2M1hYaMam+pTqGRNFz3Z46gc2sBz75hOOAyyVXWWdcrqiGe+BiSfOvnC80gFkc34FW//Xwaa0EpTMwY9iI6jQ9HOjNY5wnSvCiGp3GajPCKc0lWGejOt0d1BHMoUioeL8U8JOEa1429sawlpj0tWbKliD2ltA9fduo9O784uyFcH1dkBrsZDk+9Dwn5SPOU8FW7Ry2BkHP7W5MDpgdMzpGom2nR6ifKhADWs4zNWJLfZXhWrE/W9UrpRT1Xak/dm6BekH3wsPb/ZOLWj8pzFYzbfwILyUTkEwUVaW+P/WbZb+3/6qFz27JluFhN53XIee1wn995k/bzekT7Hszd2aHNE1tD+KZTT2H2F3i4dQJg14cx2j9w6zO5l37vBHU21P/17u62ZTXHbszTFMWMyhUZ0TFHDr6NmFLKDpjweNSwBIK1V7jhGAIZqFnpyR2bM8hqU1j17VnxKUeBDM8IxSt/wANjI0AzwsAAA==",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [967] -  Opalescent Crossing Specimen Survey
        [967] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XS2/jNhD+KwbP4kLv183xZtMATjZYJ8gh6IESRzYbmfSSVLpu4P9eUI9Ysh27CXwoit6U4czHb8YfZyavaFxpMSFKq0kxR+kruuQkK2FclijVsgILmcMp47A9pN3RNUWpGycWupNMSKbXKHUsdK0uf+VlRYFuzcZ/02DdCJEvDFj94ZqvGieMLXS1ul9IUAtRUpQ6tj1APg5dYyTRIMI+SWayqJYdA9+x/RMUuihRlpDrXqDTd3NPXyskZaR8p6SOG4aDovpt2DemFpdrUL2Lgx3GQTBgHHZFJ88wW7BCXxBW8zYG1RlmmuTPCqVBW8Yw3sftoyYt6h3RDHgOPT7hblw4rKDbhUr2F0yIbqTQ3bob7e7U32uj7xekZORZfSMvQhqAgaFLx7OG9h+QixeQKHVMkQ5pOYyNBHoXdvW7YPMrsqwTHfN5CVJ1l5gfm6LUi2x/j/0AKt5sLHT5S0vSvjRT+Xsx+5OsrrmumGaCXxHGu3pgx0LTSsINKEXmgFKELHRbk0C3ggOyGoT1ClBqCnMAbyqU/jTenQQFhxkijN45b26sz7d8ZivItSTlpJISuD5TljuoZ8v1INu9jA/eXns1AplpsTLvlfH5TMOq7oxb7q2IxvI8lPtwNYcHzn5WYHARzSIahoWNYyfMsB8VAY6JHeEEaBxHrg+QuWhjoSlT+nth7lAofWrkaRJ4e9xRGB/hOBFqKfRidFetiIG7FXJJyt+EeDYAXad4BFL/bewK9NsbLEipoHuT7aFJrHudranJ3nci04E6zJmWgvdm1+lw2+uFT2EOnBK5PgOvGvhBwVdRtf6dY2M5kf4AzQ1Nkk3cNsUbxo3XPVvWjSz6YlvoOy/Xjwvg41yzF5iV79DrA/6TpA8E30u22suidYgC13tz2RI+4nSIxNDPPKJxoUFOSDVf6ClbmunlNAe7r6teVCrZjEfz0ZsDXYluhW6qdE2Ba5abGdyU6sA48KIg2d8Ijgx3s4Z0jbBT/g/4WTEJdKaJrsyMNXvOO8/hhLw/quKB40EBHlPah7TznxDJZ3/zXrN1siTzEq/ARRZk2E9iF8dRGODEdYvM8W0KQNHm967btrvw05uhabhPr2jYeX03OdZ5M1Lq0QX7Q1SDKeEcK87bAzAVMTc1DuOlqPjADaV+kOyuNt5wzYzNTZUsSN62n4N7rR8kwYkFL9hY6F/zD8J2XH96SJtgY5mYqtYF7Y/tdlibz8a8dTuk3Z7O/CKmYRa5OHMzF/th7GESJhkOMkoDByIKXog21p6OPPv9BB64ZroEOnoU8nl0K76MPO/sejosi4/L63898XPqKctjoDQIceAmMfa9KMAx9XMce0DdrMghgbzuWw1uS/HrCI++r0gJKgeuRxMplGJ8PjK7MVsCH80q+QLr4T5aUD+zc9vFfhDZ2LdzByeul+EYHM8uHOI6PqDN3wU8SsSZEAAA",
            },
            AmountRequired = 2,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Untitled Work No. 33"] = new List<uint>()
                {
                    47430,
                },
            },
        },
        // Export for Mission [968] -  Bulk Provision Procurement
        [968] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XS2/jOAz+K4XONuCH/JBvaabTLZDpFJMWcyj2QNt0ItSxUlnuTrfIf1/Ij8ZOnGRTdIA97M2hxE8fqU8k80YmlRJTKFU5zRYkeiNXBcQ5TvKcREpWaBC9OOMFbhfTbukmJZETMoPcSS4kV68ksg1yU179SvIqxXRr1vs3DdY3IZKlBqs/HP1V4/ihQa7X90uJ5VLkKYlsyxogH4euMVgw8LBOkpkuq1XHgNoWPUGh8xJ5jonqOdr9bc7pY4VMOeQHUmo7vs92mPjBgAltgb7ycnn1imWPirfj6XkDT7+7BnjC+ZJn6hJ4HYk2lJ1hriB5KknktYn1w33cPiprUe9AcSwS7PHxd/38YU6dzlXyv3EKqhHHQ4nfi/z1J1fLmxQLxRPI9a4uT731SaL4C85zWHeLYzL1wz0mzs7tui2T+yXkHJ7Kr/AipCYzMHSpcY2h/Qcm4gUliWyd8AMU6ODA7i4u+eIaVnXSJsUiR1l2h2gppSRyA4vusR9AhZuNQa5+KQntO9a3eC/mf8H6plAVV1wU18CL7mJM2yCzSuI3LEtYIIkIMchtTYLcigKJ0SC8rpFEOjEjeDOh7+ODeHcSSxxnSExyYL05sV7f8pmvMVES8mklJRbqk6LcQf20WEfZ7kU8enq9qxHIXIm1fvu8WMwVruu6u+XeimgiP4dyH67m8FDw5wo1Lgn8OIud0DcDz/VM6rLYhISCiZCwwM78ENyYbAwy46X6nukzShI9NvLUAbwXisAPncMcp6JciRi4uriEPNeAt0KuIP9DiCcN0dWdnwj1b20vUb2/wgzy8r0wtIs6tO59tqYmfmoHup51mHMlRdHrjafdLbfnPsMFFinI10/gVQM/lPhFVO3+bmNjORH+AM3xdZCN3zbEbRX+gc+jLPp+/ya2EeeHEu8lXw8jaCxnRRB4jk5I43lWDAPP86No3fUrnGQK5RSqxVLN+Eq3UrtZ2H2e9RxVyaZX649eI6lb2RKLW6Gabvbe8460NDfw2P7AcmT20FNSV0m7h/MDnysuMZ0rUJVu+HoMO/CaTryOcx/BYOOofo8J9SxN9neN6uy4oM7Uzm8SyUfvvFetbQBAJ8lMy7F9k7oZMxmkgekFScLCNAgg9sjmz65ct6P647uhqdiPb2RYuql7pHRfYV6ucyFRDpqMfSw1OyPfG2k2TFaiKgbbSEQ9tjsZucOJN9QnVTKDpB0SR4du6jHvxHzobQzyn/n3su32H+7x2llbpjqrdUL7Xb/t9fqzMW+3jSm3pzIKKU0CykyIwTEpQ9+EgGUmIDpu7Nh+yByyMfZV5B4OYIZLeTGHHFZQpL9BSuOKOF9Z/0up+EwpBSxLM8/WUnIzk7pOaoZW4pqWx2gWWn4GlNUFq8FtKX65MC8uq/zp4k6KF15yUeivpJK4wkINB1gbA/Q9Rs3M9V2TWgGYLKXMjF1k1GEhJlZMNv8AaljefSgRAAA=",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [969] -  Opalescent Crossing Distribution Survey
        [969] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1X227jNhD9FYPPIqArdXlznGwawJsN1in6EPRhJI1sNrTopajspoH/vaAutuTYSbMIigKbN5qcOXNmdDhDP5FpreUMKl3NiiVJnshFCanAqRAk0apGi5jDOS9xf5j3R1c5SdwotsiN4lJx/UgSxyJX1cWPTNQ55vttY79tsT5Lma0MWLNwzarBYZFFLje3K4XVSoqcJI5tj5Bfhm4w4nDkYb9KZraq1ycS8x3bf4VRDyKFwEz3mfiO7QzN3NdZSJVzECeIOC5joxr7ndsnXq0uHrEaBA4OGAfBiDHrvwHc42LFC30GvOFtNqp+Y6Ehu69IEnRVZdFz3CFq3KHegOZYZjjgww792LiCbu+q+N84A90qo4966O0e1N/rvG9XIDjcV5/gQSoDMNro0/Gs8f5XzOQDKpI4pkjHpM0iI4FBwL5+Z3x5Cesm0Wm5FKiqPoj52DlJvND2n7EfQUXbrUUufmgF3cUzlb+Vi++wuSp1zTWX5SXwsq8HdSwyrxV+xqqCJZKEEItcNyTItSyRWC3C4wZJYgpzBG8uK/3TeDcKKzzOkFBy4ryN2Jzv+Sw2mGkFYlYrhaV+pywPUN8t16Nsn2V8NHpj1QpkoeXG3FdeLhcaN02j3HPvRDRV70N5CNdw+L3k32o0uMQPXXR936N2kAL14zCgqeeFNM4iJ85YXKCNZGuROa/0l8LEqEhy18rTJLC73CGL3NMcZ7JayxS4npyBEAbwWqo1iN+kvDcQfa/4A6H53V4/c1qhNjn0F7HbahP1ndA0m955oZUsl29xt72B+xyXWOagHg1CZ7hrAwWICq23AZ/LOhW7lEYWLot3BnvaJ02OURta3Sq+ORUpDFxvZ3Iq1sjohWidnZHvtNCoZlAvV3rO12ZuOO3Boa6bF0Ot2sFkFoMO3DbHIH4+Wl+Ykma89x2lF9BX/FZzhflCg67NsDLvh0NV/TvxvFUjH9/8v/nmg67FHI/5WMQUoiynPsOcxikAjXPb9gABXRfI9s++bXVvzLvdRtu57p7IuIX5nn+6hd0gKDFZrFCIUbd1XqrNVY6l5hkIUxATqDWYrmVdjsxI4gfx4RPBGz/XIhOpVgVkuBCm9Rx/qAZx8MpDKdha5H/z7t6PvZ8edsbZ7MxMVZuCDsdfN/TMst3emx2T7kBmacSyLM9jmoeQUb+AgEa+DdSG2A2dmLHcdcnWei6j4KVJmILQkzP+l6w/dPSL6Mj2itQOHAphaFM/NKusAAroxoEDkOehc1RH7HQCc1hvvkt1P5nVWb1OUX2I6dcQU+aBF3ouUBsDRv04t2nKfJsGMXrg5uCwlDWzr8XtKJ5P6OTLBgRWGZZ6MlOyqni5nJzzSiue1uYtNVnU6gEfx38QApblDrhA0zDwqM8gpZEXhxRY5KV5UMRunJLtPz5aAYU5EgAA",
            },
            AmountRequired = 2,
            UniqueFish = true,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Cobalt Bijou"] = new List<uint>()
                {
                    47429,
                    47435,
                    47461,
                    47465,
                    47511,
                    47531,
                },
                ["Lampwork Cucumber"] = new List<uint>()
                {
                    47436,
                },
                ["Pearl Shell"] = new List<uint>()
                {
                    47428,
                    47434,
                    47460,
                    47464,
                    47510,
                    47530,
                },
            },
        },
        // Export for Mission [970] -  Large Mutant Cultivated Specimens
        [970] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XS2/jNhD+KwbPYiHqQT1ujjebBnCyizjBHoIeKGlks5FFh6LcTQP/94J6xJIt23XqAj30Jg9nPn4z/Dgcv6NxqcSEFaqYpHMUvqPrnEUZjLMMhUqWYCC9OOU5bBeTduk2QaHlBwb6LrmQXL2hkBjotrj+GWdlAsnWrP03NdadEPFCg1Uflv6qcKhvoJvV40JCsRBZgkJimj3k49AVRuD1IsyTZCaLctkycIjpnKDQRoksg1h1AknXzTq9rZAJZ9mBkhKL0mCHCe0z6RMdR2INKExZVrQ7fOXF4voNig5HdwfSdXuQtD0f9gKzBU/VFeNVitpQtIaZYvFLgUK3qTj193G7qEGD+p0pDnkMHT50N472i221oZL/CROmatU8FfAtz95+cLW4TSBXPGaZ9moL2Fkfx4qvYZaxVbs4pF/q7zGxdo7dbpg8LljG2Uvxla2F1GR6hrY0ttG3P0As1iBRSHTBD1Bwehu2Z3HF5zdsWRVtnM8zkEW7idZYgkLbM5099j0of7Mx0PVPJVlzwfUpPorZH2x1m6uSKy7yG8bz9mAwMdC0lHAHRcHmgEKEDHRfkUD3Igdk1AhvK0ChLswA3lTo8/gk3ncJBQwzRBgdWK93rNa3fGYriJVk2aSUEnJ1oSx3UC+W6yDbvYwHd6+8aoHMlFjpu8/z+UzBqmrIW+6NiMbyMpS7cBWHp5y/lqBxEUtSEygLsG9bgJ3YDrCfeBGOTOJHSULB9BO0MdCUF+pbqvcoUPhcy1Mn8NEoPOpbhzlORLEUEeNqdMWyTAPeC7lk2a9CvGiItu/8AFb91vYC1MctrHpmeyubRZ1aez8bU52/Qzzdz1rMmZIi7zyap8NNuxM+hTnkCZNvF+BVAT8V8EWUjX/rWFtOpN9Ds6hOso7bpnjHc+31yJd1K/ulbpFNZ36A10FmXay/k+9A8FMBj5Kv+lnVlrOy8lxLF6mO/Md59dDOz6wJ17d1nCqQE1bOF2rKl/rJJfXC7jWuBrFS1m+6/ug8ONWTt4D8Xqj61ft4G488fbbnBvsTz5HhRY9ZbcdtL9gDvJZcQjJTTJV6MNBz3IFbd+IWnXtZeo6DOj8m6LN02vUa1N5xkZ2pnX9JJJ89805XNxOPmL4TY0Igwk5sMhxYaYQ9B2gae8ROUoI2v7VtvZn1nz8MdWd/fkf9Fu/Y/uEWf8VFIpb9l4gcq8vOXPiOaofxUpR5zw2Fjhvsjk92fyz29U6lTFncTJKDI7vjBu6JIdLdGOg/899nOxJ8ehDQwdoy0VWtCtodDZqBQH/W5q3bkGw7EkupZ1ux72GTWAw7FklwYLoWpr7tBa7veMShaGPsSyg4nMCDKGANPBtd8d9FeXElDQvifGH9r6T8kkqivm0CTQPskQCwQ1IPM8fycRrFYDmx5QNxq2ZV4zYUv4zwaMrkHEZ3pWK5Gk3KTPE1U5CM9PTNl5AX/VmX0ti1U9PFJKAMOxElOGLUxg4A+GZgRzS20OYvzP4UmWwRAAA=",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [971] -  Efficient Aquatic Foodstuffs Procurement

        // - - - - - - - - - - 
        // C Rank
        // - - - - - - - - - - 

        [971] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACrVWS2/bMAz+K4HONuD365ZmaVcg7YqlxQ7DDopEJ0IdK5XkbdmQ/z7Itho7cZa1yG42Hx8/UiSl32hcKT7BUslJvkTZbzQt8aKAcVGgTIkKLKSVM1bCXkmN6paizEtSCz0IxgVTW5S5FrqV05+kqCjQvVjb7xqsO87JSoPVH57+qnGixEI3m8eVALniBUWZ6zg95L9D1xhp3PNwzpKZrKq1YRC4TnCGgvHiRQFEdRzdrpl3PiwXlOHiREkjN+jhBa3XNZOr6RZkJ254QDgMe4QjU3P8DPMVy9UVZjVtLZBGMFeYPEuUhW0Vo+QYt4uatqgPWDEoCXT4RId+Ub+AnnEV7BdMsGo6YaitouQIzDs4Db8Fe1zhguFneY2/c6HxegKTnW/15Z+B8O8gUObqmp2gEPQCmnJeseUNXtd5j8tlAUKaIProKcr82AmO2Pegkt3OQtOfSuB27vRBPPL5D7y5LVXFFOPlDWalqa3tWmhWCbgDKfESUIaQhe5rEuiel4CsBmG7AZTpwgzgzbhU78Z7ECBhmCGy0Ql9E7HW7/nMN0CUwMWkEgJKdaEsD1Avlusg26OMB6PXVk2DzBXf6PFl5XKuYFPvyT33tonG4jKUu3A1h6eSvVSgcVGYLoDQxLPThR/bQUqonbixb4cLz0+BprGLF2hnoRmT6lOuY0iUfW3aUyfwOutxlPinOU64XDMymghcwui62GrIey7WuPjI+bMGMcvjC+D6X8slqNc5zHEhwcxlq9TJmQltRU0FAjfWS8lgzpXgZec2O+/u+B33GSyhpFhsL8CrBn6S8IFXrb0xbCRn0u+heZFOsvHbp3jS5F/SGHB+kvAo2KZPtpG8iWwcejr3xvMU3Z7R2wm37nq6xrkCMcHVcqVmbK1vObdRHI5d/Z6pRHON6o/OBTFwC/hxmB4/C/5yw+u3iNl/ptk/w0vFBNC5wqrSN61+7JyYgDMd/dbG7RkO9tzFmqtrNdgwF+2M/9QCt+88886OJXke+IBT28lTYgeO79kJpq7tps4ix4FD/MRBu29mybYP4q+vgmbP6v9mq5udOrJH0zxnhEGpRuOXCitGRtecU6mqPJejB8FJJWANpeovfRpgGuY4sBOfRnYQx9ROwhDblAZutEg9TCig3R8Rkj7TDAwAAA==",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [972] -  West Beaconveil Distribution Survey
        [972] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1Wy27bOhD9FYNrCdCDkijtHDfJDeDmBlWKLIIuKGpkE5FFl6TS+gb+94J62JIfSVNk0cXdycOZM2fGhzN8QdNaixlVWs2KBUpe0GVFsxKmZYkSLWuwkDmc8wr2h3l/dJOjxCOxhe4kF5LrDUpcC92oy5+srHPI92bjv22xPgvBlgas+fDMV4MTEgtdr++XEtRSlDlKXMcZIb8O3WDE0SjCeZPMbFmvegbYdfAbFPooUZbA9CDQHbp5b6cVMue0PNNS1wvDUVNxF3bF1fJyA2qQODhgHAQjxmHfdPoE6ZIX+oLyhrcxqN6QasqeFEqCro0hOcYdosYd6h3VHCoGAz7hYVw47qDXh0r+H8yobqXQZz2M9g7673fR90tacvqkruizkAZgZOjL8a2x/Qsw8QwSJa5p0ikth8RIYJCw798FX1zTVVPotFqUIFWfxPzZOUr8yMFH7EdQZLu10OVPLWl300zn70X6g65vKl1zzUV1TXnV98N2LTSvJXwGpegCUIKQhW4bEuhWVICsFmGzBpSYxpzAmwul/xjvToKC0wyRjc6ctxmb8z2fdA1MS1rOaimh0h9U5QHqh9V6ku1RxSezN16tQFIt1ua+8mqRalg3k3HPvRPRVH4M5SFcw+Frxb/XYHCRm5EiL3zHJl4Y2jj2PDsuKNiM+KyIPXA8RtDWQnOu9L+FyaFQ8tjK0xSwu9xRSPzzHGdCrTibzCStYHJVbgzkrZArWv4jxJMB6afFA9Dmd3sBzakCbaror2JnakvFbmTGTR+caimq5vp0XrtrXNBSgfXbqI4/QJ3DAqqcys1HAX8SdVbuKh15eGG8cziq5tjlFLWh173k63OZosDzdy7nco2cXsnW+RldTwsNckbrxVLP+cosFLc9OBR883aoZbuxzMdgNJ+Yv34UxIeL59UlbvZ+P3l6mX2B7zWXkKea6tosNfOwONTeuyT225L5XwLvk0D/n+N3/ueD6RYFmGAvzGxGsma6FTbBeWjnNMpwznJSZCHafuvHW/f4fNwZ2gn3+ILGow5jfH7UXZdUqUmquWZLkKPB7L7WnpscKs0ZLU1PTK7WYboSdTVyQwkO4sPXhD9+2RGTqZYFZZCWZhidfEri4wt1+KYKthb6a97k+w35x3sx/UHXxjIzXW0aOtyU3X40n61573ZKvQOlBQ7JI8d3bD8Ax8au49kZZtTGDDPXJx7NwqxRWovbUXyMI8/2vk1mE3vyAEpPLoAyUT0DLyefuNKSZ7WZXpO0ls+wGa/uDMcZZMSzic8yG4dRbmfYy+2IAQFM3Jhihra/AOSyijzEDQAA",
                "AH4_H4sIAAAAAAAACu1WS2/jOAz+K4HOFuCX5MctzbTdLjLdouliDsVgoch0ItSxMpLcnWyR/76QH4mdJg1a9DCHuTkU+fEj84nUCxpXRk6YNnqSL1D6gi5LNi9gXBQoNaoCB9nDqShhf5h1RzcZSv04cdCdElIJs0Gp56AbffmTF1UG2d5s/bcN1lcp+dKC1R++/apxaOyg6/XDUoFeyiJDqee6A+S3oWuMJBpEuGfJTJbVqmMQem54hkIXJYsCuOkFen03/3xaqTLBihMt9XxKB00N27AroZeXG9C9xOSAMSEDxrRrOnuC2VLk5oKJmrc16M4wM4w/aZSSto00fo3bR01a1DtmBJQcenzoYRwddtDvQpX4DybMNFLosh5G+wf9D9rohyUrBHvSV+xZKgswMHTlBM7Qfg9cPoNCqWebdEzLNLYS6CXs+nchFtdsVRc6LhcFKN0lsX92htIgcsNX7AdQ8XbroMufRrH2ptnOP8jZv2x9U5pKGCHLaybKrh/Yc9C0UvAVtGYLQClCDrqtSaBbWQJyGoTNGlBqG3MEbyq1+TDenQINxxkijE6cNxnr8z2f2Rq4UayYVEpBaT6pygPUT6v1KNtXFR/NXns1ApkZubb3VZSLmYF1PRn33FsRjdXnUO7D1Rz+LsWPCiwumhMW0zgLMONBjENOKI497mLfnQN1XcIpydHWQVOhzV+5zaFR+tjI0xawu9wRjYPTHCdSrwQfTRQrYXRVbCzkrVQrVvwh5ZMF6abFN2D1b2vXYHb3MGeFhu5etoe2uO6GtqamA6EX2SnUYc6MkmVvf50Pd4Ne+BQWUGZMbT6BVw38RVbz4rDSxsOnyc5hT/ukyzFqfa8HJdanMkXED3Yup3INnN7I1vpZXY9zA2rCqsXSTMXKLhSvOTgUfP12qFSzsexHbzQfmb9BRJLXK/iNbWr3fjd5Opndw49KKMhmhpnKLjX7sDihvTNaeq9kfkvgfRL46H/em26URoQSzjAPwgSHeRDhOPFyHMPcjznlkBOOtt+78dY+Ph93hmbCPb6g4agLQ3J61F0XTOvRn1AUm9xG9Sez91Z/bjIojeCssE2xyRqH8UpW5cANpSFJDp8TwfBpF9tMlcoZh1lhp9HRt2RIEnLmUUW2DvplHuX7FfnhxWiDrWViu1o3tL8q2wVpPxvz3u2YfHtS48xzGacZ9tw5xSEhLk68wMPcT3zCI0oZZGjrvJZSdE5K92I+lyVmavXraOm3eDpV7JSiu4t1oCdWyvKfxyTysf99NBnh0TfQZnQBjMvyGUQx+iK0UWJe2TU1mlXqGTYfk6CbA3eDJMJeEMU4ZCzALOcupkFCIXK9PAqbadfgtoXW1Lz3UOun9LM5ZyHBLCcBDikATpIgxJAHUQQeyeicoe3/yxvJY0gQAAA=",
            },
            AmountRequired = 2,
            UniqueFish = true,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Glass Ribbon-arm"] = new List<uint>()
                {
                    47447,
                },
                ["Untitled Work No. 11"] = new List<uint>()
                {
                    47446,
                },
            },
        },
        // Export for Mission [973] -  Fish Paste Ingredients
        [973] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XyW7jOBD9FYNncaCFWm9uTzoTwEkHcYI+BHOgpZJFRCYdkkp3JvC/D6gllmQ7cRppYA5zk2t59VgsVpVf0LTSYkaVVrN8hZIXdMbpsoRpWaJEywosZJRzxmGnzDrVRYYSN4otdC2ZkEw/o8Sx0IU6+5mWVQbZTmzstw3WpRBpYcDqD9d81ThBZKHzzW0hQRWizFDi2PYA+W3oGiMOBx72u2RmRbXuGBDHJu9Q6LxEWUKqe45O38x9P6yQGaPlkZQ6bhAMkkpat69MFWfPoHqB/RFj3x8wDrqk0wdYFCzXXyireRuB6gQLTdMHhRK/TWMQ7eP2UeMW9ZpqBjyFHp9g7BcMM+h2rpL9AzOqm1K4U/CNl8/fmS4uMuCapbQ0Vl1WevppqtkTLEq66ZSHijKI9pi4o7v0Wia3BS0ZfVBf6ZOQhsxA0KXGs4byG0jFE0iUOCbhRyiQQcDuLr6w1Tld10mb8lUJUnVBTOFkKPFCm+yxH0BF262Fzn5qSdtXa27xVix+0M0F1xXTTPBzynh3Mdix0LyScAlK0RWgBCELXdUk0JXggKwG4XkDKDGJOYA3F+Y+fhHvWoKCwwwRRkf0TcRav+Oz2ECqJS1nlZTA9SedcoT6aWc9yHbvxAej11ZNgSy02Ji3z/hqoWFTd9kd97aIpvJzKPfhag53nD1WYHBR5uSQemGIqRdRTNLcxnHuEUyWcbx0bJ94gY+2Fpozpb/lJoZCyX1TnuYAr40iDCJynONMqLX4IeTaYF0JuablX0I8GO+u5XwHWv9uXp7RKtCGfvcGW1FzRuKEpmd1zgstBV+96V63mwL4ldD7HWeEbXs97DmsgGdUPhv41vC1PeS0VK8d633SNfCdgj9F1dp3ho2kS4LR3LI1yFEfumT8VYUSJ/yj6YBt472Bx4Nx3cDkqomwy9SRo5wYOjKhj0Y5JWcHnO8U3Eq2GWamkfymzIS+a66kiXFSbo74fvzErbtpBdNcg5zRalXoOVubee40inGPqFe3SjYLg/noTbMDI8sL/Xg899/cocza1TXr7oHewGPFJGQLTXVldgqz141f7WmP86PPbGC4/0JOK/DTqrNvtV9xpxbMqZXxm0qgu3PywTvvDQRqEy9dOhHOszTAJPNcvPRcD6ckChzbBRJ4FG3/7iZCu/vfvwqaoXD/gobTgZDo+HS4pKkUS0nTglXryXVBgXPgajjVnLcSNdoxX1BjMF2Lig/MUEL8eLyKecMVOzKRKpnTtJ0RB3d6sv+0xgupv7XQf+bP0W69+OWlwjgbycxktU5of81olwvz2Yh3ZofquL+EpJHt5hDh1Hd9TPI4xVEYZHiZx57nxFkQ5i7aWns15b9xgDkUcjKjcvPpRXS4Fj5eU/8XEf/MIiIeITSgLg4jGmMSExdTGlIcL4lHoyV4fh7VjavB7RbTCZ4Y0Mk1VRomF3wlIWPAtRruycR1cz8kLl66touJb+eY+mBjm0Q2XeZB4PuAtv8C40Jhun0RAAA=",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [974] -  Multi-purpose Bait Test
        [974] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1W227bOBD9FYPPIlYidX9z3TQbwMkGdRb7EPSBokY2EVl0SaptNvC/F9TFlnxJ0iJbLLD7RpEzh2dGhzPzhKa1kTOmjZ4VS5Q+oYuKZSVMyxKlRtXgIHs4FxXsD/P+6CpHKYkTB90qIZUwjyj1HHSlL77xss4h329b+22LdS0lX1mwZkHsqsEJYwddbu5WCvRKljlKPdcdIT8P3WAk0cjDfZHMbFWvzwTme67/AqMeRJYlcNNH4nuuNzQjL7OQKhes7AFCzx8B+J3ZB6FXF4+gBxcFBwyDYMQw7HPOHmCxEoV5x0TD027ofmNhGH/QKA26LIbxMe4QNelQb5kRUHEY8AkP/cJxxkjvqsTfMGOmVUJ/66E3Ocg37bzvVqwU7EF/YF+ksgCjjT4c6oz3PwKXX0Ch1LNJOiXlMLa/fHBhn793YnnJ1k2g02pZgtL9Jfbn5iilkesfsR9Bxdutgy6+GcW6h2YzfycXX9nmqjK1MEJWl0xUfT6w56B5reAatGZLQClCDrppSKAbWQFyWoTHDaDUJuYE3lxq89N4two0nGaIMDpz3t7YnO/5LDbAjWLlrFYKKvNGUR6gvlmsJ9keRXzy9saqFcjCyI19r6JaLgxsmsK4596JaKrehvIQruHwZyU+12BxUR5DFhDfw34QMux7kODMLVzMM+q5hHBOigBtHTQX2vxR2Ds0Su9bedoAdo87CmP/PMeZ1Gv5Vaq1xbqRas3K36V8sN59mfgLWPPdvjx7qsFY+v0b7LbaGH0vsnWmd14YJavm3XRWu/dbsFKD82pUlw5Q57CEKmfq8a2A38s6K3eRjixImOwMjqI5NnkNtRPOd0pszhGIAkJ3JucojIx+nETnbuU/LQyoGauXKzMXa9t3vPbg8F00E0at2sZmF4MKfqJM0yhIDvvTs63eTgd9gepF+RE+10JBvjDM1Lb32fHjUKk/JMhXC+wXCubXKuMfkkD/z/0f/OeDIljQJMlJkODCzRn2gRGc+TzDcchjxiFjUUDR9lNfBbsR9X630RbC+yc0roh+QM9XxMuSaT15LzSv9ah6e88l5yqHygjOSpsRe1NrMF3LuhqZodQPksORg47Hv9jeVKuCcViUtnCdHnSPn9Ph4BVsHfSvmdv3bfSnm6d1tjszm9UmocN22jVRu2y392antDvQGSsS5iYQ4STzcuyT0MMsIh4OqEtiRhNKiG22xzoiL+loYYThK1D/K+m/oSRCozBnLMN5kRfYzzyGMxpSTMMo8xMKQUy8pmK1uB3F+yTyMf2NfprMJnhyXZdG4E2tNlLDxLKc3IE24/mQ8oK7YeDiIkisZCOGE6A55i4JgVAvDgOOtt8B1ICjMygQAAA=",
                "AH4_H4sIAAAAAAAACs1W227bOBD9FYPPYleURN3eHDdNAzjZoPZiH4JiQUljm4gsqiTVJA387wvqYku+xNsgW/RNHs6cOTOe2wsaV1pMmNJqslii+AVdFizJYZznKNayAguZxykvYPeYdU/XGYqdMLLQneRCcv2MYmKha3X5lOZVBtlObPQ3DdaNEOnKgNUfjvmqcfzQQlflfCVBrUSeoZjY9gD5degaIwoGFvZZMpNVte4YeMT2zlDorESeQ6p7hqSv5px3K2TGWX4ipcTx/UFSvdbsE1ery2dQPcd0jzGlA8Z+l3T2ALMVX+gLxmveRqA6wUyz9EGhmLZp9MND3D5q1KLeMc2hSKHHx9+384cZdDpTyX/AhOmmFDqv+9bOXv7d1nq+YjlnD+oT+y6kARgIunBcayj/Aqn4DhLFxCTpWC37oSmBnsMufxd8ecXWdaDjYpmDVJ0T82dnKHYD2ztgP4AKNxsLXT5pydpOM5mfi9kjK68LXXHNRXHFeNHlAxMLTSsJN6AUWwKKEbLQbU0C3YoCkNUgPJeAYpOYI3hTofSb8e4kKDjOEGF04r3xWL/v+MxKSLVk+aSSEgr9TlHuob5brEfZHkR81Hut1RTITIvS9CsvljMNZT0Zd9zbIhrL96Hch6s5/FXwbxUYXLRwIyCL0MaJH2XYczwbRwGkmNokIDTKIAsztLHQlCv958L4UCi+b8rTBLBt7sAPvdMcJ0KtxaOQa4N1K+Sa5Z+FeDDW3Zj4G1j928gV6G0DLliuoGvI9tFE1bVmK2pC90hgxk+HOdNSFL3FdcT8hj0Z6Zyvm97/YB9A2m4PcgpLKDImn9+Baw38UVRJvh99o+H40VZhF8pJlf9C7YjxXPLyFIGAOu5W5RSFgdLPk2jNTUuMFxrkhFXLlZ7ytdlFpHnY75X67Khks+zMR2+qHxndbkCjw+39yiI2J0M3tLpC/QLfKi4hm2mmK7MPzU1yonrPVOPPFtgvLJhfWxn/Uwm89T/vDUabeuBHUYpTnzDsUUgwcxY2Tu2I2NRLfBZ6aPO1m4zt3Xq/FTTD8f4FDaekR+npKXkhpBhdAMsG45y8lpnrDArNU5abdBg3jcJ4LapioIZij0b7N4g7vAdD46mSC5bCLDdTq2VNI3rm9KIbC/02p/tukb55fRpjI5mYNDYl+MjKZqmqbrL0dyyKEStE8c99FHjY/cP9OpqM8OimyjXHZSVLoWBkAEdzUBr14XoujlR5ryKjNHE9IA52SGJjL0pdzBhxsB35rhsxJyHA6opscNvwakLOeUI9R5BRN0gowSRLqLkJGE4Sn2AfqAcpTW0nJGjzL0p6ZfodDgAA",
                "AH4_H4sIAAAAAAAACs1WS2/jNhD+KwYvvUitKFLPm9fNpgGcNFi76CFYLChpZBORRS1JbZIG/u8F9bAlx453g3TRmzyc+eab8bye0bTWYsaUVrN8heJndFGypIBpUaBYyxosZB7nvIT9Y9Y/XWUodsPIQreSC8n1E4qxha7UxWNa1Blke7HR37ZY10KkawPWfLjmq8HxQwtdVsu1BLUWRYZi7Dgj5NehG4woGFk4Z8nM1vWmZ0CxQ89Q6K1EUUCqB4Z4qOaedytkxllxIqXY9f1RUmln9pGr9cUTqIFj74Cx540Y+33S2T0s1jzXHxhveBuB6gULzdJ7hWKvS6MfvsQdokYd6i3THMoUBnz8Qzt/nEG3N5X8H5gx3ZZC7/XQ2j3IP+msl2tWcHavPrJvQhqAkaAPh1hj+SdIxTeQKMYmScdq2Q9NCQwc9vn7wFeXbNMEOi1XBUjVOzF/doZiEjj0BfsRVLjdWujiUUvWdZrJ/FIsHlh1Veqaay7KS8bLPh82ttC8lnANSrEVoBghC900JNCNKAFZLcJTBSg2iTmCNxdKvxnvVoKC4wyRjU68tx6b9z2fRQWplqyY1VJCqd8pygPUd4v1KNsXER/13mi1BbLQojL9ysvVQkPVTMY9966IpvJ9KA/hGg5/lfxrDQYXeQkBQjLPpqlHbBoRz2YpoTYLwAffIYzSEG0tNOdK/5kbHwrFd215mgB2zR34IT3NcSbURjwIuTFYN0JuWPGHEPfGuh8TfwNrfhu5Ar1rwJwVCvqG7B5NVH1rdqI2dIoDM356zIWWohwsrhPmS74BedDx1+xx94RiTH8NXrhyyMDVHFZQZkw+vUMMDfDvok6Kw6y0Gq4f7RT2IZ5U+R5qR4yXklenCASeS3YqpyiMlH6cRGduWmWaa5AzVq/Wes43Zkfh9uGwh5pzpJbtEjQfg2l/ZKSTwItebvVXFrQ5Jfph1hfwJ/hacwnZQjNdmz1pbpUTVX2mSn+0wH5iwfzcyviPSuCt//lgYGKHJCRJsJ2EBNvUpZ6dhB6zPR8zNwcXMPbR9nM/Mbt79m4naIfm3TMaT0/qvTI9b9cMypL9oiZTKcXDaNjj1/JzlUGpecoKkxTjrFWYbkRdjtRQTL3o8EIh42sxNJ5qmbMUFoWZXUfPU+pF3pk7zdta6H9z5++37pt3rTE2kpnJaluXD6xqN7DqczNcyChGrBTll7sooLb7G/k8mU3syXVdaG5XtayEgokBnCxBaTSEG7g4UvrDvU4pxl7g2KHPiE0BsJ24gWsTL3SA5iRPXdaUaYvbhdcQwucJDRxRRvLAzV2bOhDaNIk8O4mSxA7Bz4M0DJkThmj7LzBAYXFKDgAA",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [975] -  Emergency Bulk Provision Procurement
        [975] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XS2/jNhD+KwHPYiFK1Ms3x/WmAZw0iBP0EPRAiSObiCx6KSobd+H/vqAesWTL8SZwgQLdGz2c+Tgz+ubh72hcajlhhS4m6QKNvqNpzuIMxlmGRlqVYCFzORM57C55e3XN0cgJIwvdKSGV0Bs0Iha6LqavSVZy4Dux0d/WWDdSJksDVh0cc6pw/NBCV+uHpYJiKTOORsS2e8jvQ1cYUdCzsE86M1mWq9YDSmx6woXWSmYZJLpjSLpqzulnpeKCZUdSShzf7yWVNmZfRLGcbqDoPOzteex5PY/9NunsGeZLkepLJiq/jaBoBXPNkucCjbwmjX54iNtFjRrUO6YF5Al0/PH37fx+Bp3WVIl/YMJ0TYUhXvnhAZiz9zncBuxhyTLBnosv7EUqg9cTtNG5Vl9+D4l8AYVGxOTsiAu092CbzkuxuGKrKu5xvshAFe0j5ttzNHIDmx5434MKt1sLTV+1Yk3hmQ/xIOff2Po616XQQuZXTORtbjGx0KxUcANFwRaARghZ6LZyAt3KHJBVI2zWgEYmMQN4M1noT+PdKShg2EOE0ZH7+sXqfufPfA2JViyblEpBrs8U5R7q2WId9PYg4sHXK62aIHMt16Z8Rb6Ya1hXjXLne0OisTqPy124yofHXHwtweAiP/SJTxwbezzmmDpRhJntRNj30tjmtmNTkqKthWai0H+m5o0CjZ5qepoA3mo98EPvuI9XUiaKfctAGbBbqVYs+0PKZ2Peto2/gFW/jbwA/VaBKcsKaCuyuTRhtbXZiOrYKQlMO2ox51rJvDPIBsxv2KuRPohVVfz+b/YBpO12IGewgJwztTmDrxXwYwG/y7LRbxVryYmU9NAc3wRe2+3Cfgttr6HdiLwfNXkH7mdCHjB+LOBBiXU/sFryocACzzF5qi3PEVoP8OPBNeamhsepBjVh5WKpZ2JlZimpL/aLu1qbSlUPa3PojKGBWeMGXnS4fbyzSJiVp+2ybWHdw9dSKOBzzXRp5rnZqY5U24nq+WhB9BQHuXyKtD9NxK7WILlOs+gDzPiXKPDZb97p5IlDiO1ELk4jz8Y05gzHvs+xQ7kTpCFLI9tD27/bVt7s3U9vgrqbP31H/bZOvfCdtr5Z5Uwly7LoDSDyXmquOeRaJCwz+TDv1ArjlSzzjtpAUVAv2l+i3P5+G5qHS5WyBOaZ6bpNFF7kndgdva2F/jN/RXabwKfnvzE2konJapXQ7kbQ7AHmWIt3akPM7bCMkICGQcIxY9zFNAwARx4BHDDH8TmLPZpytLUOWRQcD2AKWbHOpAJ1dhZ9mjbD9PvFovOwyIl4xNzExqHtMkydkOKYhhS7sR8QTkjMY3+QRdHxAK7zjG0u7tnmVyv6f5DIYzZ1U4gxtYkZeBDjiNg+JjSxWZBEduCH1cCrcRsXJxf4YroCtYB8sbm4LLPnizslX0QhZG5OSalgBbnu/00KmG8DSTzshm6KKU85jkISYoc4aeByYI6Xou0Pp/hsST8TAAA=",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [976] -  Vitrified Freshwater Fish
        [976] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XTW/jNhD9KwbPYiFK1OfNcZM0QJIN1tnmEPRAUSObjSx6KSq7buD/XlAftmXLNjbIoUBzkznDN4/Dx5nxGxpXWk5YqctJNkPxG7osWJLDOM9RrFUFFjLGW1HA1ph2ppsUxU4YWehBCamEXqGYWOimvPzJ8yqFdLts/NcN1p2UfG7A6g/HfNU4fmih6+XjXEE5l3mKYmLbPeTT0DVGFPR22GfJTObVomNAiU3PUOh2yTwHro9khBKb7O5yzrOQKhUsP4JHHN/v5Zi2265EOb9cQblzAG/vAJ7XO4Df3QF7gelcZPqCifoYZqHsFqaa8ZcSxV6bVT88xN1FjVrUB6YFFBx2+Pj7+/x+Qp1uqxL/wITpRhnfSvhS5Ksnoec3KRRacJYbry4rO/Yx1+IVpjlbdsYhjfrhARNn72rdlsnjnOWCvZRX7FUqQ6a30KXGtfrrX4HLV1AoJibhRyjQXsDuLi7E7Jot6qSNi1kOquyCGOGkKHYDmx6w70GF67WFLn9qxdpHbG7xUU5/sOVNoSuhhSyumSi6i8HEQreVgjsoSzYDFCNkofuaBLqXBSCrQVgtAcUmMQN4t9LcxzvxHhSUMMwQYXTE3kSs7Vs+0yVwrVg+qZSCQn/QKfdQP+ysg2wPTjwYvfZqBDLVcmnevihmUw3LuuhuubciGquPobwLV3P4VojvFRhcxDPPy+w0wWGSMEx54uPIAxenAfPdkAfgJBlaW+hWlPpLZmKUKH5u5GkOsCkUgR+6xzlOZLkQfDRRrIDRVb4ykPdSLVj+h5QvBqSrPE/A6t/NAzTWErQ5RfcUzdKjWIDae6J3otiYUEzob3bja9JQp4USJwhNatpIU61kMTsb6+j+W5hBkTK1MhCt54ZTxvJyU8nOI9vmzn6XVZJvEtDzcPxo47DlfdRliNqu16MSy2ORAs9xNy7HYvWcTkRr/Yzcx5kGNWHVbK5vxcL0LNIY9t9BPa1UqmmK5mOnYtc9Yw7FvdRN29g0lxO9ww286HAsONHSzSzSlaxOn1/heyUUpFPNdGU6qxl29kW7d6tBNCi3obs/JapPkfyaSN5757tlkRCWhh7H4EYRpqFt44TSFBPPDYhLSORGPlr/1dXFdiB+3iw0pfH5DfVrJPVP1PGJTFiuRxfib1n16jk5lZy96eoNNQ7jhayKnhuKqRftDyFuf7g05W1aqYzxdh4bno69yDszinlrC/1n/iVsG+u726nZbFYmJqt1QncbbNtWzWezvHUb0u6OzigEwKPAx47rZZh6SYLDNAwwDV0WZjSlGfXR2jrU0Ylee7kAxfI0Yfzlw2U0rIZfV9WnjD5URsS2I5pBgombEEw938NhGmQ4oVHipg4AT7NBGTnHD3Ahi9XoSTGh55nZ9Sml/4WUuEMdn5EE0zThmFLi4SRLA5w4ruMAcYDbTedrcLveNcKjP4VWIhOQjq5MIn8wDWpUC64XIITQ9SmPMLNthmnkRZhxx8c2uLad+JQyJ0PrfwE9wWYb1hIAAA==",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [977] -  High-silica Environment Observation
        [977] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1YTW/bOBD9KwHPYkFKlCjp5nrTNECaBnUWPRR7oMixzY0suhSVNlv4vy8oWbbl2G6zTYECm5tMDt988PFx6G9o1DgzFrWrx9MZyr+h80oUJYzKEuXONhAgP3mlK9hOqn7qUqE8TLMA3VhtrHYPKKcBuqzPv8qyUaC2w95+1WG9M0bOPVj7EfqvFidJA3SxvJ1bqOemVCinhAyQT0O3GBkfrCDfDWY8bxZHEmOUsO9E1IOYsgTp+kwYJXTXLPx+FMYqLcojgdAwSQY1Zutlb3Q9P3+AesdxvBdxHA8iTvo9EHcwmeupey10G7cfqPuBiRPyrkZ5vK5qkj7G3UXN1qg3wmmoJOzEk+yvS4YVDPulVv8DY+E6ZvRe91eHe/WP1qtv56LU4q5+I+6N9QCDgT6dKBiOfwBp7sGinPoiHaJ2knoK7Djs6/dazy7Eok10VM1KsHXvxG+2QnnECXsU/QAqXa0CdP7VWbE+eL7yt2byRSwvK9dop011IXTV1wPTAF01Ft5BXYsZoByhAF23QaBrUwEKOoSHJaDcF+YA3pWp3X/Gu7FQw+EIEUZH5juP7fw2nskSpLOiHDfWQuWeKcs91GfL9WC0jzI+6L216ggycWbpz6uuZhMHy1Yot7GvSTSyzxPyLlwbw5+V/tyAx0WMS8UjSnFcZClmnEssoqjAWRJTnlEZCg5oFaArXbv3U++jRvmnjp4+gc3h5knKjsc4NvXCfDF24bGujV2I8q0xd351LxMfQbS/u5PnZ2twPvz+DK6HuhwZ5V5n+sUTZ001O7n8fVU+fJxDNZJO38OkPApMoh3gK5hBpYR98Nhrw402TEVZQ/DDEbfAf5imKDfJDizCJNsYbBM6anIotF2rW6uXxzzxOIw2Jsd8DYxOeFvbeU6Ppg7sWDSzubvSC3+Z0G5in+xtG9HY7rbyHzuy3O/UtXGPN+uAMEc8zvZvpJOXve8PeknqafgBPjfagpo44Rp/2/kGZJ+bP0bBp/LphR9P40e/5+yJe74je7GMBAgiMctIjBkhCc5USLHiMZ0yTkAxilbBYZ2Lj+vchTHSii8l2J8Uulu9ALvH9Xe62kyhnLJXJEC+Yz1k68eH9ukr9tMa+qKUv9dJOCKGTz0YL2L4vxZDRbiaqizEvIgyzBgQLGLFMEiuJIQyZUSi1V99E7h+sX/aDHT6+OkbGgolS5LjQjkRlSpKUTuwg96VnqrNpYLKaSlKXxDvqDMYLUxTDcxQzuJs/8EVDR+/qffU2KmQ6/7i8LP/cWux/+yMVwH6bf7F2D4iNhsRZz7xH+rM/TqPMPYFbWu5+45Yvx78Zze8NTvE2h2GcZKGLCliTKkimMmpwlnBUiwpUxwoy1QUtdftPoNOXLVjU4jSnb3Wf5vmhULPSKGeBH0OByj1lCboV3EqjAgFTmIsKKWYhTLEBWEcq6igEEdExYIe5NSJZ+oNCFueTeZQli+U+rWq9DtQiMdRwjkhOKZTgllKQyyAJZhwxtJpxmUKvL34OtxeeM7w2Vs9m+Nal1qKs/PqXltTLaByojx7X9Rg74Xvpob/tMQqAloUKZbMPzkKpXBB0wxzEtEQwgRkqNDqX16LKAiCFwAA",
            },
            AmountRequired = 3,
            UniqueFish = true,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Cobalt Bijou"] = new List<uint>()
                {
                    47429,
                    47435,
                    47461,
                    47465,
                    47511,
                    47531,
                },
                ["Opal Shell"] = new List<uint>()
                {
                    47467,
                },
                ["Pearl Shell"] = new List<uint>()
                {
                    47428,
                    47434,
                    47460,
                    47464,
                    47510,
                    47530,
                },
                ["Sandblaster"] = new List<uint>()
                {
                    47466,
                },
            },
        },

        // - - - - - - - - - - 
        // B Rank
        // - - - - - - - - - - 

        // Export for Mission [978] -  Upper Soda-lime Float Distribution Survey
        [978] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1Xy27rNhD9FYNrqdCLEqWd45ukAZw0uErQRdAFJY1sIrLoS1LpdQP/e0E9Ykm2Eyd1gS66o4czZx46Mxy/omml+IxKJWf5AkWv6LKkSQHTokCREhUYSF/OWQm7y6y7uslQ5JDQQPeCccHUBkW2gW7k5c+0qDLIdmKtv22wbjlPlxqsPjj6VOP4xEDX64elALnkRYYi27IGyO9D1xhhMLCwPgxmtqxWXQSebXkfhNBZ8aKAVPUM7b6a87FbLjJGiyMltR3fHxTVa82umFxebkD2HONRxBgPIva7otNniJcsVxeU1XFrgewEsaLps0QRbsvok33cPmrYot5TxaBMoRePP7bzhxV0OlPB/oIZVQ0VOq9ja2dUf7e1fljSgtFneUVfuNAAA0GXjmsM5d8h5S8gUGTrIh3isk80BXoOu/pdsMU1XdWJTstFAUJ2TvTHzlDkBpa3F/0Aimy3Brr8qQRtO01X/oHHf9L1Takqphgvrykru3qYtoHmlYBbkJIuAEUIGeiuDgLd8RKQ0SBs1oAiXZgDeHMu1Zfx7gVIOBwhMtGR+8Zjfb+LJ15DqgQtZpUQUKozZTlCPVuuB6Pdy/ig91qrIUis+Fr3KysXsYJ1PRl3sbckmorzhNyHq2N4LNmPCjQuwjh3cWaDSZI8NL0w8M0kdTOTpEFOg5yESYrR1kBzJtVvufYhUfTU0FMn8NbcgU/84zFecw6byZyKF6rR7rhY0eJXzp+1fTcofgda/256T99KUDqBrgtvWamlD2zV9OkvloFatSZzzw709OkAYyV4WXdTq/XW1TktJBjHPY1QLbeHOocFlBkVm3MBP0r4xqtWv1NsJF1J3tIezaUPK+L4uiAN2IflOGp5SsoHjB8lPAi2HibWSP55YgF2dPEauE+mNrD9fHKtuW7iaa5AzGi1WKo5W+nX024uxt1dL0qVaJ5nfei9QwceGzfA4fiVfXdj0UtON2a7xvoOPyomIIsVVZV+wfUWNe62TzXQyQ0xUNzn8mn8PI2Ifa19cp1KmFOZ8S9RoPvm3ie/eW+U536YYwy5SfzANr0wzEzqATFt8FNMiU2I56DtH90sbzftpzdBM86fXtFwrnvEOz7XbySXKRQgJzOe0EIN3iH7vQLdZFAqltJCV0V7axSmK16VPbUDreHhcLxLucO9lmjHlchpCnGhZ2+by35LjVdIvDXQf+YvyG4h+PIaoI21ZKarWhe0vxi064A+NuKd2iH+9riWhilNPMcyaYKp6dlBYiYJxmYQhNj1Uhy4WY62xj6XrOMJXAkuFWSTeAlFcXYifZk5hxn4P5Huz0IkANsijpua1HUy08u8wKSpRU0vJDTEvkUCnNRDq8FtQ7yYmJPH9RrEJOYZNQu2gslVwamafGNSCZZU+hmcxJV4gc1w4bUIAT+3c9PHgLUbywz1lIQkCEnoJolLMNr+DcJMSar6EAAA",
            },
            AmountRequired = 3,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Isosceles Cobalt"] = new List<uint>()
                {
                    47484,
                },
            },
        },
        // Export for Mission [979] -  Central Soda-lime Channel Distribution Survey
        [979] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XTW/bOBD9KwbP4kKUqM+b66bdLJI0qB30UOxhJI1sIrLoUlRaN/B/L6iP2HLkpCmy2AW2PsnD4eOb0ZsZ6p5May1nUOlqli9JfE/OSkgKnBYFibWq0SJm8UKUuF/M+qXzjMROGFnkWgmphN6SmFnkvDr7lhZ1htnebPx3LdallOnKgDUPjnlqcPzQIu83i5XCaiWLjMTMtgfIT0M3GFEw2GE/S2a2qtcnAuPM5keMgnDIyO5RZFFgqvtQOLPZoZ/zPA2pMgHFCSbM8f3omMowOcPlaSLvkMQ5FFV/wjtRrc62WB1w9I4gPe8o3/0Lg1ucr0Su34BoYjSGqjfMNaS3FYm97hX44WPgAazTwV6DFlimeMDIP97oDxm5/VYlvuMMdKujMVH64SMwZ5ixsMNarKAQcFu9gzupDNzA0EfnWkP7R0zlHSoSM5O0Ewz4gLzfHfhGLN/Dugl7Wi4LVFV/iNFJRmI3sPkj8gOocLezyNk3raArWvMiFnL+FTbnpa6FFrJ8D6LsU0uZRS5qhZdYVbBEEhNikauGBLmSJRKrRdhukMQmySN4F7LSv4x3rbDCcYaEkhPr7YnN+p7PfIOpVlDMaqWw1K8U5RHqq8U6yvZRxKOnN16tQOZabkz9inI517hpmuyeeyeiqXodyodwDYebUnyp0eASz/U8lvOMAmQ55V7EacQDoLbN7Ch0MnATRnYWuRCV/pCbMyoSf27laQJ4KPXAD4PTHC9hWWIB6g4M2JVUayj+lPLWbO+7xieE5r+xV6gfKrDpeX1FdosmrL42O1MbO2dBZMLvMOdayfJgCJ7YvhBrVEclfynKhyUSs+APbh/+fIt8KIvtpxWWV1JPUy3ucF78HKsLXGKZgdo+S+wAwQlMf7up8K2suw29Z2t5Jn8DOMc3fNp9+xy9dOcgjhGvmwoXSmyGZFvLi8gGnmM03e58Id3B3icId36mMKe5RjWDernSF2JtBiRrF44rtrlH1aodwebhYLaMDBA38KLj+8eTFxpzB+pbZ18tH/FLLRRmcw26NkPaXLJOlNAzJXHkZbunJDrmOKq5nxDXy1U0KpifUsa/LIFffecH7Tl33ZRlPlDX9YDyyA1okmJOnczJWeInEbOB7P7u+3N3Ef/8YGhb9Od7MuzVPAxP9+o3RY2TazgaKuypzJxnWGqRQmHSYY5pHaZrWZcDNxJzLzq+CbnDW6ppc/Na5ZB2DXX8Lu9F3tPXQebtLPKf+TTZT/dfnulms7HMTFabhB5O+W62m8fWvHcbE+6ByJwgSbjjAM0x55RHNqfgQ0pT5gK4XuZxOyU767GIotMBzNS20lAU4jtmk8VKya+Vlv+ApMaV8XKF/ZbUq0oqc30/iiKgLAo8yh3boxDZAfXDkCG4PAffHZWUfzoA47qd/IVFsc3Nrt/d6X8hJeSQo+9lFNwMKM/CgCa+jTTgmeuESRY6aDcjsMXth9iETmZYmm+wyVxmQAuxxslsBWWJxeStqLQSSW0uVpN5re5wO/wssj2GiZf4NEzBo5wzmyaR7VAv4CmzIbQ5OGT3A5FMMbFrEwAA",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [980] -  Arthrolure Field Test
        [980] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XS2/jNhD+KwbPImBKJCX65rhJGsCbBmsXPQQ90NLIJiJLWorabLrwfy+oRyzZst0GPuwhN3k4882DH2fGP9G0NNlMFqaYxWs0+YluU7lKYJokaGJ0CQ6yh3OVwv4wao8eIjRxA+GgJ60yrcwbmhAHPRS3P8KkjCDai63+rsb6kmXhxoJVH679qnB44KD7fLnRUGyyJEITMh73kM9DVxjC71mMLwYz25TbNgJKxvRCCK1VliQQmhMVoWRMulbu5SgyHSmZnMAjLue9GtPG7E4Vm9s3KDoJsIMEGOslwNs7kC+w2KjY3EhVpWEFRStYGBm+FGjCmqry4Bi3iyoa1CdpFKQhdOLhh3a8X1C3NdXqH5hJUzNjiGY8OAJzD27Ha8CWG5ko+VLcye+Ztng9QZud5/TlXyHMvoNGE2JrdiIE2nPYlvNGre/ltsp7mq4T0EXrxN59hCaeP6ZH0feggt3OQbc/jJbNO7QXscwWrzJ/SE2pjMrSe6nStraYOGheavgCRSHXgCYIOeixCgI9Zikgp0Z4ywFNbGEG8OZZYT6M96ShgOEIEUYnzmuP1fk+nkUOodEymZVaQ2qulOUB6tVyHYz2KONB75VWTZCFyXL7fFW6XhjIq765j70h0VRfJ+QuXBXDn6n6VoLFRb4LPATKsEelh6lPAQfxeIVZFHjCBUpdStHOQXNVmD9i66NAk+eanjaB97fuczsITsV4Z2n/Kg3o0VSbjc6SUoPFfcz0Via/Z9mLRWo7yF8gq99WXoB5f4yxTApoH2dzaDNsn2kjqstAiW87U4u5MDpLOyPusvnY65jPYQ1pJPXb/0b4LStXyWFKtYbLxbvCPr6TKr0YBrSWWuWnPPnM9d5VTvnqKZ3x1uhZFk9jA3omy/XGzNXWThNSHxzSu9ojSl2PK/vRacQD3dbzmTgex2dGqd0B2j7T8ukrfCuVhmhhpCntRLNLxgmSXSDNf+bGJwU+RIGP3nmnlwmX+j7QGEeEC0yFjLHwVgTHPADmi7HHPYJ2f7fNrFlEn98FdT97/on6jY3amE41trnawuhhm0MRmkz32jA5V56HCFKjQpnYmlhftcJ0m5VpR21ovWTicJXw+lteYB2XOpYhLBLbm5pMmGAXNii2c9Avs5/v5+GHp6A1tpKZrWpV0O5cbKah/azFe7Uh9naZFjHu+yLGMXcZpoz6WPJVgNkqXMWxBwKAo51zzKQzCSxeVJ6rdP0q365Oow/zZph/nzS6Do0CJoKxCCUWkQBMKXOxYB7HMad+4FMY08gbpJF3hkZlivNS5wlEo6XMoTD6+nz6bEu/JJ985kUrECGOSeBiuiKAJScB5hxi4hM3dGOvGoA1bhPizQh3lvLRnYIkGi2hMP1/CkCikPHAw5LHAlNCXCw8SrGMfcECP2CcuGj3L9Fyo3dREgAA",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [981] -  River Basin Large Aquatic Resources
        [981] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1WTW/jNhD9KwbPEmBJFPVxc9xsGsDJBnGKPQQ9jKmRTUQRHZJKNw383wvqI5ZsOWmDHHrYGzUcvnkcPc7MK5lVRs5BGz3P1yR9JeclrAqcFQVJjarQIXZzIUrcb2bd1mVGUj9OHHKjhFTCvJDUc8ilPv/JiyrDbG+2/rsG60pKvrFg9cK3qxqHxQ652N5tFOqNLDKSetPpAPl96BojiQYnph+SmW+qx44B9ab0AwrdKVkUyE3voNd38z8OK1UmoDiRUs9nbJBU2h77JvTm/AV1L3B4wDgMB4xZl3R4wOVG5OYMRM3bGnRnWBrgD5qkYZtGFh/j9lGTFvUGjMCSY48POzzHhhn0u6NK/I1zMI0UxnTF4iMw/+B3BC3Y3QYKAQ/6GzxLZfEGhu52gTO03yKXz6hI6tmcnaBABwG7dJ6J9QU81veelesCle6C2H+fkTSIpvSI/QAq3u0ccv7TKGgfnv0Rd3L5F2wvS1MJI2R5AaLscut6DllUCq9Qa1gjSQlxyHVNglzLEonTILxskaQ2MSN4C6nNp/FuFGocZ0hccmK/iVjv7/kst8iNgmJeKYWl+aJbHqB+2V1H2R7deDR67dUIZGnk1j5fUa6XBrd1odxzb0U0U19DuQ9Xc/ijFE8VWlwSxAg0YFMXMVm51M+ZC5xSF/KYZ0kCQZ4xsnPIQmjzPbcxNEnvG3naC7y99YjF0WmOV7AusQD1DBbsWqpHKH6X8sEe78rGD4T629o1mrcXmEOhsXuR7aa9Vvc2W1Nzd+pFthx1mEujZNlrZCPHv5fFy48NltfSzLgRz7gsTmJPgx72AtdYZqBe3oUfQ/hNVqvi8L6Nh8+SN4c9+ZMuAw4jXndKbE9FikI/eHM5FWvg9E601s/qepYbVHOo1huzEI+2v3jNxqHg61GiUk0Ds4teaR6pv0EUJscd+Z3maseArvJ0YrvFp0oozJYGTGV7nJ0zTijwA0X9a238ksCnJPDZf96rblkeBQGj1EUWBS7F1dSFMOQuCzJOMYqTVc7I7s+uvLWz6P2boalw969kWOpoEp8udZcc1wVoPbkANSjL3nvJucywNIJDYTNiIzUOs0dZlQM3ktIwOZwlguGYF9tIlcqBt8VsdKSiYRJ+MFGFO4f8bwb0fX/8dFe0h61lbrNaJ7TfJ9vuaJeNee82pt2ezmIKCaerlZvEq8SlWYwu0Ahd7vN4ShmPYi8kO+dYR8k7LVPKUhtZ4uRKKvhyJY0L4r8L65eSyq9UUsKZxxIIXGQBdekKIxeCPHYzRoNkxXiENK4rVoPbUjybuJNb8YxqcgZalJMFqDVOZk8VGMEnt6hlpTjqg9EvgwCmEboBY5kVLXVXsR+71KecTSlHP/LJ7h/yrTMLExAAAA==",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [982] -  Saltpeter Shore Sunken Resources
        [982] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XS2/jNhD+KwZPLSACelDPm+PNpgGcbBBlsYegB1oaWYRl0UtS200D//eCetiSLdubIIei6E0mZz5+M/w4M35F00rxGZVKzrIlil7RdUkXBUyLAkVKVGAgvTlnJew3027rNkWRHYQGehCMC6ZeUGQZ6FZe/0yKKoV0v6zttw3WHedJrsHqD1t/1TheYKCbzVMuQOa8SFFkmeYA+Tx0jRH6Aw/zIplZXq07BsQyyQUKnRcvCkjUiYwQy7T6XvZlFlykjBYn8Czb8wY5Jq3bZybz6xeQvQDcgwBcdxCA190BXUGcs0xdUVaHoRdktxArmqwkitw2q15wjNtHDVvUB6oYlAn0+HiHft4woXbnKtjfMKOqUcZXCV/K4uUbU/ltCqViCS20VZeV3v40UewHxAXddJtjGvWCIyb2wdU6LZOnnBaMruRn+oMLTWaw0KXGMYbrj5DwHyBQZOmEn6BABgd2d3HFljd0XSdtWi4LELI7xDbQMAVnQnR8kxyFODgv2G4NdP1TCTp46TsUffNPPP6Lbm5LVTHFeHlDWdldJrYMNK8E3IGUdAkoQshA9zVxdM9LQC3CywZQpJM5gjfn+g7fifcgQMI4Q4TRif3mxHp/zyfeQKIELWaVEFCqD4ryAPXDYh1lexTx6Om11aM2mvGqVCAaD23f3XojuVjxja4mrFzGCjZ1Gd9H1spyKj4moD5czfBryb5XoHFRktLUz2wXBy44mPh2hhcudXDiOb5pOkniWT7aGmjOpPqS6TMkip5f69N0ALvS43vhGY6fqhKyJt7JVKhc8KISoIHvuVjT4g/OVxqqq2jfgNa/9boEtXs5GS3k7j22m/3stktNHojl60rZYcZK8LL3EC+7m07PfQ5LKFMqXj6AVw38iVeL4lKkA0fbC3d++2jqypxD2VTmXfEaPbqP8CsBjTg/CbY5ot0a+K7t7Ez2DM8YjZEY2umnMs0UiBmtlrmas7XuoFazcfiG6tmpEk2L1h+9/tHl6Z6rE6kaLfNueDyknBkw9GTUFcNO1Y/wvWIC0lhRVek+r0evE1K/IN23KvSy4s5J603a+U+I5L133iupBLLUJYGJvcBPMcnMBaY2DTGEmeVZXhpatoW2f3Y1tR3Pn3cLTVl9fkXD+urqceZUfX0QTK6pYsnkjgvKDhqCdS5DBwPfK2oMpmvdwfZmY3O3Gx5OQM5w/A30wZXIaNJOjG0wbuhemA7drYH+NX9c9p353f1YO+uVei6oE9rv0ChCVxM8iWmxAQViEudcwCSuyhWUk0eQvBIJyMlvOom/owat8d/jjSm9p8oMPDdwXBObabLAJCQmDjzbx07guIFDSOCYIdoax6qzT0d6w4s0K+gKJjEtU93fP1x279bZuF7/l91eOrLLzcPblVhn+V06DCCxKc1c7JOUYLLwbRx6HmDTyXzXSiCjVlBXxwa3De5XON3pQX1wlk984rmegwO6CDEJLQcvHMvCtkMzxw+ykIQUbf8BHyZpJZMRAAA=",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [983] -  Mutant Aquatic Specimen Observations
        [983] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1WS2/jOAz+K4HOFtbv1y3NtN0CaacYd7GHYg60TSdCHSuV5Ha6Rf77Qn4kdpqk20EPA+zcBIr8+JGiSL6Saa34DKSSs2JB4ldyXkFa4rQsSaxEjQbRl3NW4e4y76+uchLbYWSQW8G4YOqFxJZBruT5j6ysc8x3Yq2/abGuOc+WGqw52PrU4PihQS7Xd0uBcsnLnMSWaY6QT0M3GFEwsjDfJTNb1quegWuZ7jsUeitelpipgaE1VLPfd8tFzqA8klLL9v1RUt3O7ILJ5fkLyoFjb4+x540Y+33S4QGTJSvUGbCGtxbIXpAoyB4kib0ujX74FneIGnWot6AYVhkO+Pj7dv44g3ZvKtg/OAPVlkLvdd/a3su/01nfLaFk8CAv4IkLDTAS9OE4xlj+DTP+hILElk7SoVr2Q10CA4d9/s7Y4hJWTaDTalGikL0T/dg5iZ3AdN+wH0GFm41Bzn8oAaOftiWgH+KOJ8+wvqpUzRTj1SWwqk8PtQwyrwVeo5SwQBITYpCbhhO54RWSDuFljSTWeTqAN+dS/TTerUCJhxkSSo7ctx6b+x2fZI2ZElDOaiGwUp8U5R7qp8V6kO2biA96b7QuuMiw+WXPsO4fuxHmWtr8Gy/yPKOrrETxtf7orFokCtdNS91F2VXfVHxOcEO4hu1fFXusUeOSKHfB8jOgkQ0OdUPLpAA2UCdzsLAz33eDlGwMMmdSfS20D0ni+9fGmw5g2xUCX8+IYxwv9H95BoViMhVqKXhZC9S4N1ysoPyT8weN1PeavxEedr9H30ocpbYTtfG6VqCbVW+cKMGrxUfMTWdgPscFVjmIF43QKW5/cQGlRONjwF94nZbbkEYath9tFXa0j6ocojbUuhNsfcxT4NnOVuWYr5HSCW+dnq7iaaFQzKBeLNWcrfTcsdqL/fJuVoxatINNHwYd/ECbdgIv2p9PJ2e9Xg/6jtTX0zd8rJnAPFGgaj379P6xX2T/rZY+WjK/S+BjJdC/ufvBNx/0MojyIgpNn5ppEVA3B4+CbyFNLddz0txE187I5nvfzLod9X4raPvZ/SsZNzbPPNHYknqNYpIoEGktpBr1YetUfq5yrBTLoNRJ0c5ahemK19VIrZkc+1uHM94AQ+2pFgVkmJS6Gx1cOfUEemf38jYG+WV2991A/OkxqI21ZKaz2iR0OBi7caiPrXindqh8B6XmWIUZ+kVBnSJKqZtHAQVApKmTeg64mR26AdkYb0spOB7AleQywxLlZLpCtXz5XU3/l2pKfbR8J/WoH7g2dQMrpGnhAYUwNNMgtyM7bxtXi9tRvI9Ch9rfJ2cTOrmuFVRqMn2sQbFsohdVtsJq8jWVKJ5Az0Q5sf+wx8ufG6AbpmZBLSfNqFtkGU2DIqN+YVmh56eFW6Rk8y8QmAvLPxAAAA==",
                "AH4_H4sIAAAAAAAACu1WyW7jOBD9FYNnEaOVWm6OO8kEcBa0M+hD0BjQVMkmIosOSSXtCfzvA2qxJW9pN3LoAeYmkVWvHouPVfWOhqUWI6q0GmUzlLyjy4JOcxjmOUq0LMFCZnPMC9hupu3WTYoSN4ot9CC5kFyvUOJY6EZd/mB5mUK6XTb26xrrVgg2N2DVh2u+KhwSWeh6+TiXoOYiT1Hi2HYP+TR0hRGHPQ/7QzKjebloGfiO7X9AofUSeQ5Mdxydrpn7cVghU07zIyl1XEJ6SfUbtyuu5pcrUJ3AwQ7jIOgxJm3S6TNM5jzTF5RXvM2CahcmmrJnhZKgSSOJ9nG7qHGD+kA1h4JBhw/Z9SP9DLqtq+T/wIjqWgpt1F1vdyf/XuP9OKc5p8/qir4KaQB6C+1xPKu//hWYeAWJEsck6ZCWSWQk0AnY5u+Cz67pojrosJjlIFUbxFx2ihIvtP099j2oaL220OUPLWnz0kzmH8XkjS5vCl1yzUVxTXnR5gM7FhqXEm5BKToDlCBkobuKBLoTBSCrRlgtASUmMQfwxkLpX8Z7kKDgMEOE0ZH9OmK1v+UzWQLTkuajUkoo9Cedcgf10856kO3eiQ9Gr6xqgUy0WJr3yovZRMOyqoxb7o2IhvJzKHfhKg5/FfylBIOLbObYJHZSHDlejH2WupgGbIoDz6e258VTYAStLTTmSt9nJoZCyVMtT3OAzeMOSUSOc7wWAlaDMZWv1KDdCbmg+Z9CPBv/tlB8A1r912/P7CrQ5gDtK2yW6lP6TmgqTes80VIUs5Pu90W++jaHYsg0f4VJfhTY9jrAY5hBkVK5MtiN4aY6ZDRXYP004wr4iyin+eawPQuXxBuD7YGOmhyi1rV6lHx5LFIYuN7G5FisntGJaI2dUfUw0yBHtJzN9ZgvTDtx6o1duVeTQynrfmU+OoW5vak7ofcv60Bp9sIg3u1JJ/u7GQnaotTK8Cu8lFxCOtFUl6bfmZljV5s/J8Fz9fS/Ps7TR3vn/pl33i18oRekNmE48iKCfQhcTFOf4sBzfaBp6Dt+gNbf28rXzKVPm4W6+D29o34VDOzoeBW8YTDLqVKDq1z0S7ZzKjs3KRSaM5qblJhQtcFwIcqiZ4YSP4h35wyvP/NFJlIpM8qaR9Uw339Au+NVsLbQbzOemyJbT6t1Fdg2z25Hio/fxZWh+kY1yMFQ6rkUeSkBdZBHJru1Ot/osu6nqg3Xba8oQbQQxd9PceRh9/vgYoAHt6WmhR4MX0qqORuYcYAvoBjcTxXIV2rKjhq4f7ioi96JeOA9dLQbOTZ4fgoYYtfGfsYojlxmYx+mHnGcFDwSobW1r81THboS5heuWKl+H2keqPX/NaXuK/PsYW6yJ8kd/Zmh7hd0NJ2GmU2DGPvulGHfDTMcpQCYZdPIt4PMCyNS1cAat6FY6dw5Q+dOpfNu3IjRkKQxJlPiYD9yPEydOMJ+nJKMeDS2sxSt/wVNxQdRfhAAAA==",
            },
            AmountRequired = 4,
            UniqueFish = true,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Glass Discus"] = new List<uint>()
                {
                    47426,
                    47453,
                    47506,
                },
                ["Glass Stitcher"] = new List<uint>()
                {
                    47425,
                    47437,
                    47444,
                    47452,
                    47505,
                },
                ["Iceglass Floe"] = new List<uint>()
                {
                    47508,
                },
                ["Isosceles Amethyst"] = new List<uint>()
                {
                    47507,
                },
                ["Super Starburst"] = new List<uint>()
                {
                    47509,
                },
            },
        },
        // Export for Mission [984] -  Vitrified Freshwater Fish Observations
        [984] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1WS2/jNhD+KwbPIqAHRT1ujpukAbzJYp20h6AHShzbbGTRS1LZTQP/94J6xJKjxEkQoHvoTR7OfPPN+OMMH9G0MnLGtNGz5Qqlj+i0ZFkB06JAqVEVOMgezkUJ+0PeHV1wlPpx4qCvSkglzANKPQdd6NOfeVFx4Huz9d81WF+kzNcWrP7w7VeNQ2MHnW+v1wr0WhYcpZ7rDpBfh64xkmgQ4R4lM1tXm44B8VxyQME/oNBFyaKA3PQCvb6bfzytVFyw4oWWej6lg6aSNuxM6PXpA+he4vCAcRgOGNOu6ewOFmuxNCdM1LytQXeGhWH5nUZp2LaRxs9x+6hJi/qVGQFlDj0+9DCODjvod6FK/AMzZhopdFnpkf4HbfT1mhWC3ekzdi+VBRgYunICZ2j/Brm8B4VSzzZpTMs0thLoJez6dyJW52xTFzotVwUo3SWxfzZHaRC55Bn7AVS82zno9KdRrL1ptvPXcvGDbS9KUwkjZHnORNn1A3sOmlcKvoDWbAUoRchBlzUJdClLQE6D8LAFlNrGjODNpTYfxvuqQMM4Q4TRC+dNxvp8z2exhdwoVswqpaA0n1TlAeqn1TrK9lnFo9lrr0YgCyO39r6KcrUwsK0n4557K6Kp+hzKfbiaw00pvldgcZGfeQlPALAfAsfEpwQznnFMIHJ96lKXuznaOWgutLla2hwapbeNPG0BT5c7onH8MseZ1BuRT66ZYqWpCmYhL6XasOJ3Ke8sSDct/gRW/7Z2DebpHi5ZoaG7l+2hLa67oa2p6QDxIjuFOsyFUbLs7a/j4W7QC5/DCkrO1MO7EW40/Car1r9zbCxH6hyg+dRW08Tta3lv5KCMEa8bDddKbIdkG8u7yEahb2tvIt9JdxD7CuHWz16j6dKAmrFqtTZzsbH7y2sODu9X/VSpVLMg7UdvE4yM+yAKk8ON/+qbwT4zukHXqfobfK+EAr4wzFR2h9p3zAtSPyLdNyt0zHFUc28Q1/tVNCqYNynjP5bAR//z3jDNc88NgzzADBjBJGM5zsCOVWAkyfzcjxOCdn9107R9694+GZqBevuIhpM19IKXJ+vVlhWgcyjNZGFEudJrsR1sA++1Jl1wKI3IWWE7YzM2DtONrMqBG0pJmBw+YYLhczK2mSq1ZDksCjsY2wLCJDzycgt3DvplXv77Pfzh7WuDrWVm21h3sL+P2y1sPxvz3m1MtD2BJS5bEsJcHAeUYxJlHDPm5tgPA5otwedx5KKd81xArxQwkxkrzORE/C2rX1449H/hOB8RDneznEJIcOASign1chz7WYhJTMMAvCAKl1E9mRrcluLJBE/+EEaJpQA+ObON/MEMqInNNLnKNKh7ZtedHj4qKQ/CjHHAYcQyTJIoxlnGKc5iSLLQA4ggR7t/AescJCReEAAA",
            },
            AmountRequired = 17,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Cobalt Bijou"] = new List<uint>()
                {
                    47429,
                    47435,
                    47461,
                    47465,
                    47511,
                    47531,
                },
                ["Kintsugi Chip"] = new List<uint>()
                {
                    47512,
                },
                ["Opalescent Stingship"] = new List<uint>()
                {
                    47513,
                },
                ["Pearl Shell"] = new List<uint>()
                {
                    47428,
                    47434,
                    47460,
                    47464,
                    47510,
                    47530,
                },
            },
        },
        // Export for Mission [985] -  Saltpeter Shore Large Resources
        [985] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XS1PjOBD+Kymdoy1blmU7t5BlWKoCwxKoOVB7kK12osKxgiyzw1L571Pyg9jBgYXNVs1hbk6ru/V16+tHntG0NGrGC1PM0iWaPKPTnMcZTLMMTYwuYYzs4VzmsDsU7dG5QBMSRmN0paXS0jyhiTtG58Xp9yQrBYid2Opva18XSiUr66z6IPar8sPCMTrb3Kw0FCuVCTRxHafn+W3XlY8o6Fk474KZrcp1i4C6Dt2DQPw+hNZKZRkkpmPodtXI+9cqLSTPDqTUJYxFe0hY1EPSBzqN1SOgScqzor3hiyxWp09QdDD6ey79fnCsfR9+D4uVTM0Jl1WIVlC0goXhyX2BJn6TcRa+9tv1GjVer7iRkCfQwUP27Vg/2aQ11fIfmHFTs+a2gK959vRNmtW5gNzIhGdWq01g53yaGPkIi4xv2sMh/rLQYfvPvsc8r0Fys+KZ5PfFF/6otAXTE7Sp8cZ9+TUk6hE0mrg24Qcg0N6F7VucyOUZX1dJm+bLDHTRXmI5JtDECxz6Cn3PVbjdjtHpd6N5U+D2FW/U4m++Oc9NKY1U+RmXefsw2B2jeanhAoqCLwFNEBqjywoEulQ5oHHt4WkDaGITM+Bvrux7fNLflYYChhEijA6c1zdW5zs8iw0kRvNsVmoNuTlSlHtejxbrINpXEQ/eXmnVBFkYtbG1L/PlwsCmasg77A2Jpvo4kLvuKgy3uXwowfpF4IeMEJLiNE4ppjFzcEx8FzPgPHJch4SCo+0YzWVhvqb2jgJN7mp62gBeGkVQtb5DGC/4Moe4XFpXl0qvefaHUvfWuO0434BXv628APNSf1W3bOuxObRBtZXZiOrIqRvYTtb6XBit8s64fN/c8Trmc1hCLrh+OgKuyvFtAb+rstFvFWvJO+H3vBFmg6ztdiFeyNxq3ci1bWLE+c2rGkzTk6/hYRBZ19e/iXfA+LaAGy03/ahqyYeiCnxik1Rb/ue4et4+Hlljbut0mhrQM14uV2Yu13bYuvXBfgFXK1ip62luPzqjZmCeeIEf7W8yb+5Cdn1qO2lbPtfwUEoNYmG4Ke3At/vZgZp6p0Y+Wgo9xUEWv0XXD7GwqzXIrLcp9EFm/E8U+Oybd7o1p2nIPZJin5MEUwAfc5ECTgkIiCgPkzhA27/adt3s8Hcvgrpj3z2jfuv23fBw6/6zlMl9wXMxuuC54VpJ0Zs27ls52tv9nlGtMF2rMu+oDVQH9aP9jcnrb8KhvbjUKU+a5bEJx4/8dxZFfztGP83/m93Y//Swt8ZWMrNZrRLaHf/N0LeftXinNkThDt18oMwLicCBEBGmEU1xHFOGE0eQkEQBkJih7fg1ndjhAKbrtUpWWuUqU0tZmKNz6dPkGSbhLy4dh0sCQvBEwHGSsAjTIAlx5CYBZg4F4rFIBE48yKXgcABXKuN2ExCjU8h+HiL9Yk5+TObQFDjzXY5pQEJMacRwGHgRZm7gMU4SJ+GkGnq13wbiyQiPFjzbgAE9WqyUhtGc6yWMrqFQpU6g6P8REsIDh5EAs5hSTH1P4NhzfeyFMYeYEu5DgLY/AGpfSVeJEwAA",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },

        // - - - - - - - - - - 
        // A Rank
        // - - - - - - - - - - 

        // Export for Mission [986] - Cultivated Specimen Survey
        [986] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XyXLbOBD9FRXOZIYbuN0UxfG4SnZcljw5uOYAgk0JZZJQANBjxaV/nwIXi9Qap1yZHMYnGeh+eHh6jW69oHGl+IRIJSfZAsUv6KIkSQ7jPEexEhUYSG9OWQnbzbTbukpR7ISRgW4F44KpNYptA13Ji2eaVymk22Udv2mwrjmnSw1Wf3D0pxrHDw10uZovBcglz1MU25Y1QD4NXWNEwSDDOktmsqyKjoFnW94ZCl0Wz3Ogqpdo98Oc88dykTKSH5HUdnx/IKrXpn1mcnmxBtk7GO8wxnjA2O9EJ48wW7JMfSSs5q0XZLcwU4Q+ShTjVkY/3Mfto0Yt6i1RDEoKPT7+bp4/VNDpUgX7DhOiGit0p+5mOzv6u232fElyRh7lZ/LEhQYYLHTXcY3h+h1Q/gQCxbYW6ZCX/VBboHdgp99HtrgkRX3RcbnIQcjuEP1lpyh2A8vbYz+ACjcbA108K0HaStPKz/nsH7K6KlXFFOPlJWFlp4dpG2haCbgGKckCUIyQgW5qEuiGl4CMBmG9AhRrYQ7gTblUP413K0DCYYZoPDJHo0mVK/ZEFKSj2QooK6AczSrxBOuR84eDjiA0nJDZnVfv6nwlSD6phIBSvZMOO6jvpsZBtvWNTkT17t1YaKb4Slc0KxczBav67dxyb202Fu9DuQ9Xc7gv2bcKNC5ygsjyXADTCqLI9BI7MkNMwQwxOJmT4cwNQrQx0JRJ9SXTZ0gUPzQG1hd4Lf/Aj9zjHO8gHU24LHhBFguupIa84aIg+Z+cP2qQ7j35CqT+X69LUK+VmpFcQle57aa+XFfD7VKjgGcH+p3qMGdK8LLX4Q6kfynz9dcllDdcjaliTzDLj2Jbbg97CgsoUyLW70C6Br6X8IlXbXwX2Kyc0WaA5vhagSZve/9rVuqoOSv0Wxh9iAZ//laHMyL00X9EgQPJc8FWe1dqAwLsuK8hW/Yngg6R2IljBfBKXZNn3QQ+WAbSRTjOFIgJqRZLNWWF7o92s7FbnfUoVImmAesPvU5zoJ24AY52JwrdeY4OB3qM6Z7Jribu4FvFBKQzRVSle7Sek35NobwV9X3q5yj80Le2rb++Q6gH6+RUQbzJ1f+lfd9k1nsJk0oqXjTO6b8kv8DHvfZig5X6YWSZQeQmpmdn2AyzxDUjSijGNk2SLEObv7v+0v4+eHhdaFrMwwsa9hrsnOg1nzgvIB39xddkAWLQGe1TQl6lUCpGSa4l0mc1AeOCV+UgDMUe1m/lQBR3OHyH+qRKZIS2ldBSxxE+M+fijYF+m99J25nkpycRnaxXJlrGWsH+bPLD86QObhJbIOeI9bfmo7adEp8QM/Ss1PTCgJoJJWBG2AkDJwq9xCVoY+yb68QVZ4qI0USQROnB77fx1oHK/d9q9p7V9PC7Z6Rjb+jWSJBEWRpkjolDapleSEIzotQxie162MsSGnhR/Yo1uC3FM74ezuEEY+zjNDRdmlmmh7PITDIXm65P3cyynYgmGG3+BQH9j32zEQAA",
            },
            AmountRequired = 2,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Bottom-layer"] = new List<uint>()
                {
                    47524,
                },
            },
        },
        // Export for Mission [987] - Stardust Bait Test
        [987] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XTXOjOBD9Ky6d0RQCBIib481kU+VkUoO35pDag4DGVgUjjxDZZFP+71sCY4O/MjvlQw7xSW41r183T63mDY1rLSe80tUkn6PoDV2XPClgXBQo0qoGC5nNqShht5l1W7cZipyQWehBCamEfkURsdBtdf2SFnUG2c5s/Nct1p2U6cKANQvHrBocP7TQzWq2UFAtZJGhiNj2APk8dIPBgsET9rtkJot6eSIxj9jeHiNKh4w6EFkUkOouE4/YpO/mvM9Cqkzw4gQR4vg+22PiD5kMiY4T+QwoynlRdRG+impx/QpVjyM9n5zfvS7+BPFC5PqKiyZFY6g6Q6x5+lShiG5egB8e4vZR2Qb1gWsBZXpKVB6x/X0Yf1h7t0NS4l+YcN1q6phA/fAAzNmTlrMBmy14IfhT9ZU/S2XwBoYuWdca2r9DKp9BoYiYEnYxvUGErpxXYn7Dl03e43JegKo6VCOTDEVuYHsHdAdQ4XptoesXrfjmyJoXMZPxP3x1W+paaCHLGy7K7l1jYqFpreAOqorPAUUIWei+IYHuZQnIahFeV4AiU9YjeFNZ6d/Ge1BQwXGGCKMT+23EZn/HJ15BqhUvJrVSUOoLZbmHerFcj7I9yPho9MarFUis5cocX1HOYw2rpsXuuG9ENFaXodyHazj8VYqfNRhcFBIW0iwJcAY0wR7zAxwG3MWu7UPo5ozaHkdrC01Fpb/lJkaFosdWniaBbe8JfOac5nhV1DCKNVdZXWmDdy/Vkhd/SvlkELrO8QN489/YK9Dbg990PWvTCDabJrOuJWxMbfoeCUxH6jBjrWTZuwVPPD4TS1B7neZOlNst052+ULv/cyx0x196Hq77xT4gY7s9MlOYQ5lx9XqBLBvgP2SdFPt1az0cn20ddkU46XKMWt9rpsTqVKSAOu7W5VSsgdOZaJ2fWIKs9R1/6QprDs0416AmvJ4v9FQszeVF2o3909RMOLVqb0ez6DX6IxeKG1B2MBmcGzXMdNK1tU7G3+FnLRRksea6NheoGX8+gLZ/SY2fsmv8Liayb2Xx+mMB5b3U41SLZ7jNoNQiNSNhW9hfleF7Ouy3c4cnxGYuZjYz7dyxccKpg3PCQ8ZYCmlO0Prvrp9vxvbHraFt6Y9vaNjbqeOf7u3jWknFR/ECimJwD5Fz5dxWw9TQRGodxktZlwM3FHmU7Q9P7nCuDU2kWuU8hbgwyj0+gFJG35kZ6dpCH+ZbZjcQ/PYYYB42lompalPQ/mCwGQfMsjXv3I6pvaezIMsCAi7DJCGAPU4oDlM3wJQmrsec3IXAQ2vrUEfh6QRul0Jz005H1+VccTN8X1pNx0Xx/8X1qaaLqsnzCac2pZgxj2HPdgGz0M+wnUIYBqlNUzs8qiZ2Rk2VrFIooBo9AFeXb0yfUvqQUiIUvNxxXOyHqY09CFIcMprhhCV2SHnmO7nXXIAtbneFjfD262RkaI1mUOnhlxJPeepBGmAv9UPsJZmPmeNwnHDuhyEnhFGG1v8BUQAx63wTAAA=",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [988] - Elemental-esque Aquaculture Specimens
        [988] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XyW7jOBD9FYNnsaGF1OKb40lnAjgL2gn6EMyBoko2J7LoUFQmmcD/PqCWWLLlbPAAA0z7JBdZr4pPr1ilFzQptZyyQhfTdIHGL+g0Z3EGkyxDY61KsJBZnIkctotJu3SeoLEbRha6VkIqoZ/R2LHQeXH6xLMygWRrNvs3NdaFlHxpwKoH1zxVOH5oobP1zVJBsZRZgsaObfeQ34auMKKg52G/m8x0Wa7aDIhjk3dSaL1klgHXHUenu819P6xUiWDZAUod1/d7pJLG7bsolqfPUHQC052MKe1l7Leks3uYL0WqT5io8jaGojXMNeP3BRrThkY/3MftokYN6jXTAnIOnXz8XT+/z6DbuirxN0yZrqXQRt31dnf49xrvmyXLBLsvvrNHqQxAz9Aex7P69h/A5SMoNHYMSUNa9kMjgU7Alr8TsThjq+qgk3yRgSraIOZlJ2jsBTbZy74HFW42Fjp90oo1lWaYv5Hzv9j6PNel0ELmZ0zkLR/YsdCsVHABRcEWgMYIWeiySgJdyhyQVSM8rwGNDTEDeDNZ6C/jXSsoYDhDhNGB9Tpitb7NZ74GrhXLpqVSkOsjnXIH9WhnHcx278SD0atdtUDmWq5NvYp8Mdewrm7Gbe6NiCbqOCl34aocbnPxUILBRTxNqJeEHIfE55iwOMaR6wc49AknwGMS8QRtLDQThb5KTYwCje9qeZoDvBZ34Efu4RxPshJGc81UUhba4F1KtWLZ71LeG4T2qvgJrPpfV59ZLUCbI7R1eCFyY70Rq6pSw2+2hS7YU8fmucbWuNZ8ECcwd1IbZK6VzBefCUP3w7j+QBjb64SZwQLyhKlnE6nZ+HqrpCwrwDqcwBDwbQG/ybLZ326sLS1vrwnuXF8754m+Ubv7C3YCur7hq4besvVB8OAT4B/haMD5toAbJdZ9JmrLsZkIqGu4r8GPz0UP/vNsNO7mNpmkGtSUlYulnomVaeNOvbB7zVQTW6nqOcE8dBriVZ49/1xCPuFaPMI82wpyoB96AY32R6I3phszh7WdoK38H/BQCgXJXDNdmiHDDHq718HHqvmzxdjbuF9HH6iGD2u6u2tfpx9S2yc08y+J46vvvNttPOICpRz7LHQwcW0Xx6EdYeZwSnwvYA4J0OaPtt00HwN3r4a649y9oH7roR453Hqu1iwTGkbnqzUUXEvV65XOWwydJ5BrwVlmaDHh6g2TlSzzzraB2iA02p33gv60GprApUoZb+qsOQyN6DtjLt1Y6D/zmbQdWr48qhhnY5kaVitCu8NLM7KYx9q83TYk4I7YEi+hQRpG2A5sFxOXhJgB9XHMaZzSiAEPYrSx9sXkHT7AVCYrmbNS/xLR/0NEHELHo8zBLKEMk8hOcBgQH7sJRCROfZ56ZFBEbxxgKmOW6dGJ+FOWR9fRAeF49D3hDAvwl46uj6IjloS27acuhpBRTNwoxGEYpTgOaUrBJSn1WNX5atwmxckIj04zWEGuWYaheChhNHkoGS8zXSoYmc9MsYK86H/UAQfb9r0Ee7YTYuLbKY5oHGDHi7lvO0kaexxt/gHTpl913hMAAA==",
            },
            AmountRequired = 15,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Cobalt Bijou"] = new List<uint>()
                {
                    47429,
                    47435,
                    47461,
                    47465,
                    47511,
                    47531,
                },
                ["Codmonaut"] = new List<uint>()
                {
                    47533,
                },
                ["Opalite Impesctor"] = new List<uint>()
                {
                    47534,
                },
                ["Pearl Shell"] = new List<uint>()
                {
                    47428,
                    47434,
                    47460,
                    47464,
                    47510,
                    47530,
                },
                ["Selenium Herring"] = new List<uint>()
                {
                    47532,
                },
            },
        },
        // Export for Mission [989] - Prismatic Pull Distribution Survey
        [989] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1Y227jNhD9FYPP4kKUqOub42TToE4arBP0IegDJY1swrLopag0aeB/L6hLLNlyvAlcYLGNn+Th8HBmeHh4eUHjUokJK1QxSecofEEXOYsyGGcZCpUswUC6ccpz2DYmbdNVgkLLDwx0K7mQXD2jkBjoqrh4irMygWRr1v6bGutaiHihwaoPS39VOK5voMv13UJCsRBZgkJimj3kt6ErjMDr9TCPBjNZlKs2AkpMeiSEtpfIMojVgYpQYpJuL+t4FEImnGUH8IjlusFOYK7TC6wf9zgSj4DClGVFO8JXXiwunqHo5OrsQDp9SLedLraE2YKn6ozxKmNtKFrDTLF4WaDQaSbA9fdxu6hBg3rLFIc8hk487m6KVr/2VttV8n9gwlRNoiFGuv4emLUzkXYDdrdgGWfL4it7FFLj9QxtdrbRt3+DWDyCRCHRNTsQAu0N2JbzjM8v2arKe5zPM5BFO4imSYJC2zPpXvQ9KH+zMdDFk5KsWbJ6Iu7E7G+2vspVyRUX+SXjeVtbTAw0LSVcQ1GwOaAQIQPdVEGgG5EDMmqE5zWgUBdmAG8qCvVhvFsJBQxHiDA60F6PWLVv45mtIVaSZZNSSsjVibLcQT1ZroPR7mU8OHrlVRNkpsRaL1+ez2cK1pXEbmNvSDSWpwm5C1fFcJ/z7yVoXEQCK2LUcTC14xRTYgH2XdPFSWoB8b3IpHGMNgaa8kL9keoxChQ+1PTUCbyudc/VOn0oxvMyh7TOdzSWaiFFVkrQwDdCrlj2mxBLDdVKyJ/Aqv/aXoB6XY2V/LWrs2nUKbbrtDHVdaDE09LUYs6UFHlnOxzofs1zbb3jq0oIzC+mga7ZU8dmky9m90ftvUEtz++MOoU55AmTzydIx9Qze1/AuSgb/9axthypWg/NcnVt6n7byrxmuqN//cJYRBfmINyPpDzQ+b6AO8nX/cRqy7sS8xxL16nueYrUeoDvT67tzlcgSnXNnioW2V0WWQbSijBOFcgJK+cLNeUrvTOTumFXKqrjWynrrV9/dDa1gZ3L9pxg/xT0xglGH71azW6X5jf4XnIJyUwxVerTgT7bHVivR9bfEK3fWi89x0GqH+P0D/O06zXIveMkewdx+n4no8BH57y7L0S261kmxQ5xCKYuZdhPgwgTk7kBmD61TAttjF91I/iwWPzn8v8rKP3/Q9T1ZeFT1T9V/SdS9TSxE89lLo4j4mLquDYO0sDEUer7MYuSyItTtPmrPe43rzoPr4Za6B9eUF/xHTs4rPizMl9CPvqdq/4thbxVm6sEcsVjlumC6IFqh/FKlHnHbeihxgl2b9p2/xFEy/OslCmLYZZpsW3ScALnyAODszHQT/PStb0ufviSqDtry0RXtSpo99rYXBb1Z23eug1Rt0MzyoCQyPdwYEcephYk2CdOhG3qpyyNk5i6UXV42KXRGweHS5ElozOpix6xeHlyKn2YO8Mc/KTS7UmoZDq2w1KSYteiFFNmE+z7iYmjwHYS2zYj8L1KsWrcJsTxCI9uJS9WTPF4dFtm2eicF0ryqNSb32hWykd47r+ERMQn4CcedjxNWmYzHNCAYRKDQ20/ImDHaPMvpxhiq1wXAAA=",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [990] - Aquatic Glass Resource Distribution Survey
        [990] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACt1XW2+jOhD+K5Gf4QiDub6x2banUtqtSo/2oToPBobEKsGpbdrmVPnvK0NIICGpturD6ryR8fibb8ZzyzuKa8WnVCo5LeYoekcXFU1LiMsSRUrUYCB9OGMV6MMfVbluflMxh06hvZF3P69zFNlBaKA7wbhgao0ibKBrefGWlXUO+V6s9TetgRvOs4W20HzY+qvB8QIDXa0eFgLkgpc5irBlDZDPQzcYoT+4YX1IZrqolx0Dgi3yAYXuFi9LyNQ+hoOIEGzh/i37YxZc5IyWJ/Cw7XnhATHPHRAb8o5T/gIoKmgpOwuXTC4u1iB7vroHkO4Q0uueiz5BsmCF+kZZ47EWyE6QKJo9SRS52wfwgmPcPmq4Rb2jikGVQY+Pd3jPG8be7q4K9h9MqWqTaCwjveAIzD54SGcL9rCgJaNP8pK+cKHxBoLOO8cYyu8h4y8gUIR1zE5QIAODXTi/sfkVXTZ+x9W8BCE7IzpNchQ5vkWO2A+ggs3GQBdvStBtHeuHeODJK11dV6pmivHqirKqi62JDTSrBdyAlFTXMkIGum1IoFteATJahPUKUKQDM4I341J9Gu9OgIRxhshEJ85bi835nk+ygkwJWk5rIaBSX+TlAeqX+TrK9sjjUeuNVpsgieIrXb6smicKVk2L3XPfJlEsvoZyH67h8E/FnmvQuIj6LnWClJhF6rkmASs0AwK+aRM/c1Mvx5jYaGOgGZPqR6FtSBQ9tumpHdjVuu+F7mmOlzrtX6kCMYmFWghe1gI07i0XS1r+zfmTRuo6yE+gze+2CvWpBKVd6epRix7YEsRBnd7Qt90RirD/l2U0Q+/nAqpbruJMsRe4zqFSLNO9eY+mo9VEj2BfN7SOSqIEr5qK3GrtLDat2DjPsYdqOT3UGcyhyqlYn/VyzJsDVNsPNgb6zuu03EVtoGJ74U7hyJdjlQGxEa0HwVanLPmu7exUTtkaKJ2x1umxJfBa3dC3LgC6dOJCgZjSer5QM7bUIwy3B4c11Sw7tWhnpP7odf+P82JkCDi+Gx4vFGeWAb3FdO2vS/N7eK6ZgDxRVNV60Oo16TD3fyspP06yMcX/b9p8WZJ89s17LVbP/oymYAa2b5kkJ8QMwbbMrKBpEVIXwiJAm3+7HrtdpR93grbNPr6jYb91CTndb7VLk8uSv4KQg9mAzwVnVwD6urbUKsRLXlc9tbH12A2986unblNJLQqaQVLqFrf1w9Vj4+xa524M9Mf8v9gP6U+P5uSVrrRkqqPaBLQ/rLcjWn+24r3aWO7288wP/JC4vmnTHExiZ44ZUAImhtRKc5r6roObPGtxtxTjiTmJn2uqWDa5KqmUk3uQvBYZTL4zqQRLa926JkktXmA9XB7CwgUcEsvEGfZMQsLMTHMnMz1w0hB7BfVxgDa/ADXdD5qkDgAA",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [991] - EX: Red Cosmomaggot Field Test
        [991] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XW2+rOBD+K5WfYYW5GMNbTrbtVkp7qpLVWanaBweGxCrBHGPaZqv895W5JEBCK1Vd7T7smxmPv/lmPDMe3tCsUmLOSlXO0zUK39BlzlYZzLIMhUpWYCC9ueA56M3vebarv5lcQ6fQnEi6z5sEhTYNDHQvuZBc7VCIDXRTXr7GWZVAchRr/X1j4FaIeKMt1Atbr2ocQg10XSw3EsqNyBIUYssaIL8PXWME/uCE9SGZ+abaHkMxcMzFljti5IwYdSAiyyBWnScutnBfzf6YhZAJZ9kEEWwTEoyYEG/AZEh0thLPgMKUZWVn4YqXm8sdlD2O3gjSG0KS7rrYE0QbnqpvjNcuakHZCSLF4qcShV57AYSe4vZRgxb1nikOeQw9PmR8jgyDbXdHJf8L5kw1SXQuIwk9AbNHN+e0YMsNyzh7Kq/Ys5AabyDovHOMofwBYvEMEoVYx2yCgjsw2IXzG19fs23t9yxfZyDLzohOE51jvuWesB9A0f3eQJevSrK2jvVFLEX0woqbXFVccZFfM553sTWxgRaVhFsoS6ZrGSED3dUk0J3IARkNwq4AFOrAnMFbiFJ9Gu9eQgnnGSITTew3Fuv9I5+ogFhJls0rKSFXX+TlCPXLfD3L9sTjs9ZrrSZBIiUKXb48X0cKirrFHrm3STSTX0O5D1dz+D3nPyvQuIjZsYVTFpjBiqWmS7FvBrAC0yKxZ1MKKzclaG+gBS/V91TbKFH42KSnduBQ6z4JnGmOD5BczEW5FVu2XgtVasg7Ibcs+02IJw3SNY8fwOpvLS9BHeqwbnxdXbab2rmuQltREwEX+7opdZiRkiLvvY4Tx5d8C3JU+Lc8P2yhMPjFMtAte+2JMNEy/eScO67lI31X6+uH+McG8juhZrHizxBlk65YTs+VBawhT5jcfejNGOFXUa2ycXgbDZsEB4VjrCZVBhzOaC0lL6Ys+Z7tHFSmbA2U3rHW6ulimqUK5JxV641a8K1+1HCzMa6yevypZPNq6kXvPZizPIZsphRsC9XFUuss9bzUYJ7c3E0CueKxfukn5ynH94Lx3PHuIKNnn65pdhXyAD8rLiGJFFOVfp71cDUum3+yPv7PyX8hJz+bPr0eH2CXuUkSmy6xHdNNAzApw8SkFraIl7iMpCu0/7Nr8u0s/3gQNH3+8Q0NG77n+dMNP3riRcHz9QvbDd4m/F5sDqWkA6INNQqzrajygRoKXS8YD1TOcNal2lIlUxa3zfX8D4EXeB+Mld7eQP+Z/5vjkPDp0SB6YYWWzHVU64D2h4V2RNDLRnxUO5e6vTTzGSQkSG3TdRLbdFPPMmnqMZM4fkBXXmLbgOs0a3Bbipd/XJgXo/Hg4opDllwsoVTDacWnYGGaWCZOKTVdGqzMgGBq2r67coAmfkIZ2v8NKN0q1xUPAAA=",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [992] - EX: Large River Resources
        [992] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XyW7jOBD9FYNncSBRu29uT5IJ4CyIE/QAwRwosmQTkSU3SaU7E/jfB9QSS7Ycd4I0MEDnZherXi18rCo9o0mpiylVWk3TBRo/o5OcJhlMsgyNtSzBQuZwJnLYHvL26JyjMYliC11LUUihn9DYsdC5OvnBspID34qN/qbGuigKtjRg1Q9iflU4QWShs/XtUoJaFhlHY8e2e8ivQ1cYcdizsI8GM12WqzYCz7G9IyG0VkWWAdMdQ6erRo67LSQXNDtQUocEQa+oXmN2KtTy5AlUx7G/E7Hv9yIO2qLTB5gvRaq/UFHFbQSqFcw1ZQ8Kjf2mjEG0j9tFjRvUa6oF5Aw68QS7dkG/gqQ1leJfmFJdU+FOwVWePX0VennOIdeC0cxotVXpnE+YFo8wz+i6PRwiZRDtRUJ27tJtIrld0kzQB3VKHwtpgukJ2tK4Vl9+A6x4BInGjin4gRC8nsP2Lr6IxRldVUWb5IsMpGqdGOJwNHZD29uLvgcVbTYWOvmhJW1erbnF22L+na7Pc10KLYr8jIq8vRjsWGhWSrgApegC0BghC11WQaDLIgdk1QhPa0BjU5gBvFlh7uOdeNcSFAxHiDA6cF57rM638czXwLSk2bSUEnL9QVnuoH5YroPR7mU86L3SujFK06LMNcjawui3PDstJANuvFevz49932o4NdfF2rQLkS/mGtZVY96m2/BuIj8myy5cFfZdLr6VYHCRHaYh9QjBKecR9lzuYgrUxzHELvGcgPoE0MZCM6H0VWp8KDS+rxltEnjpLWEQ+4djPDUv5TvVIEcTqZeyyEpZ4V4WckWzv4riwSC1Hesr0Oq/kSvQL+83pZl6aSnNYbfijagug+eEphO2mHMti7wzQwfML+gPI70Vq6p1BH/Ye5C224GcwQJyTuXTB8RaAd8p+LMoG/1WsZYcKUkPjQQm8dpum/ZLajvt8ELknayJbbLuV4JEe5XouviZMgwY3ym4lWLdT7aWvCnZ0CemdrXlr0q35+TtCTfm5tlPUg1ySsvFUs/Eyox7pz7Y7QfVZlfKep8wPzrDbmCiuaEf7y9Ir+w6Zitre3n7AG/gWykk8LmmujQrh1n7DrzKI6/srQ+npzjI+WPk/mlydrUGCXecWW9gxi+iwHvvvNP8SUgdL/QY9kPbxV7gJ5gSSHEQRCF3GXgOeGjzT9v9m0+D+xdBPQDun1F/EvhBcHgSXEuhVlQLNppmpdIge6PLea1CO7vnM6oVJiszgLdqA2/D8+Pdjc3tb+KRcVzKlLJmeW2S8c1Ue3VR9TcW+t98NG13iHdvDsbYSKq1pipod5doNgjzsxZv1YYI3CFbagcOiVIXO0HiY4+FBEece5g4TuQkSRrbPEUba59M3uEErhIluKD56KJUCrIPp9K7uTPMwU8qfQyVWOwmnhPFmMWpiz3bBhynlGOfM+6nEYuAxINUcl9JgD4+jaYFZJTRXC8/qfR7UCnisevEIcORH0XYAz/GlCch9pPEScGzqe06g1R65WPnLKNKjeaszNa6+JxvvwmTCIEgDn0bM5cy7FES4wSSFEd2SpzADcFLSbVM1bhNiCd/j/BoRuUCRjfiEeToBlRRSgaq/50OdsJZGLs4CNMAe3bk4NhNbGyHjFNCEk6CBG3+A5Xe9QswFgAA",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [996] - Precision Lens Development Support
        [996] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XTW/bOBD9KwLPYqAPSqJ8c900NeBkgyhFDsEeKHFkE5ZFl6KyyRb+7wUlK5b8lW2RBXrITR4O37wZPs7QP9C41nLCKl1N8jka/UCXJUsLGBcFGmlVg43M4kyUsFvk3dKUo5FHYxvdKiGV0C9o5NpoWl0+Z0XNge/Mxn/TYl1LmS0MWPPhma8GJ6Q2ulrfLxRUC1lwNHIdZ4B8HrrBiKPBDudNMpNFveoYENchb1DodsmigEz3Nrp9N+/tsFJxwYoTJXW9MIz3mJAhkyHRcSqfAI1yVlRdhC+iWly+QNXjGOxBBsEAMuzOhy0hWYhcf2KiSdEYqs6QaJYtKzQKthUP6SFuHzXeot4yLaDMoMcn3N8XDovtdVuV+BcmTLeqOSbBkB6AeXsn52/B7hesEGxZfWFPUhm8gaHLzreH9jvI5BMoNHJNzU5QIIOAXTk/ifkVWzV5j8t5Aarqgng2ehB6kRRs3UEdQfYjhxwkN4hENxsbXT5rxbZX2JzTvUz+YetpqWuhhSyvmCi70mPXRrNawTVUFZsDGiFko5uGI7qRJSC7RXhZAxqZuh3Bm8lK/zberYIKjjNEGJ1YbyM26zs+yRoyrVgxqZWCUr9Tlnuo75brUbYHGR+N3ni1+km0XJvbLcp5omHdtNwd963Gxup9KPfhGg7fSvG9BoOLAjdjHIIQh2meYkJpiGlAchyRNCZO7INDGdrYaCYq/VduYlRo9NjK0yTw2gqiMCanOSYaioIpK2HFSpbWnQQDeiPVihVfpVwamK67PABrfht7Bfr1OjWdsbte20WTXnfRtqa2BsSNTNfqMBOtZNkbjUe2X4vSWO/FqukR5CKw0TV77tm84MI5COP4vTAzmEPJmXp5B/4N8GdZp8V+RVoPL4xfHXbpnXQ5Rq3vda/E+lSkKPD8V5dTsQZOZ6Jt/cwNGOca1ITV84WeiZUZVG67sH81mudLrdpJaD56Pf5ouw2MGA9n78mJbt4eXZPq9HgH32uhgCea6dpMS/O4OSHSN0T3q5r50MCvaeDUoZ99PG4GjdDLUsqJx3EURAQTn8SYOiTF4GY5iXnII4egzd9dJ9w+gB9fDW0zfPyBhl0xIPGZrii5WEGprZnIlqAGTdw9V58ph1KLjBWmKCZY6zBeybocuKERCeL9l4c/fDNSE6lWOcugfcZsuQdx8MZ7LNjY6I/5J7Abn789NM1mY5mYMjYV7I/R7fA0n61553ZMrz1tMTejKXdS7HOfYEJ9H1POAOecR5SnlIQQo419oB1zTme0w3AhVmAlWgFb/Q/yOdJZP9T0jmoaW9Opha2vL2tQmIFegJKZLHmdafEE1jXToAQrqv8sOscjA905jEIWehTHLskxyV0XM/AcTFjEUxLnce6Zd9hhzwpPp/qt1EIXwK0HqZbWjbywYvrnNK7jkv1Q3u279LHIiyIW8AwzHjiYAE9xSiDCmZuGAafMj4nXzMgWd0txbGHrVkEmKiFLawZlZX2GJyjkupl9Sb1eS6WHf0t4HDOapSEmgWMihTlmKaXYDVLX83mWUnDR5iefFn1k+RIAAA==",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [998] - EX: Large Aquatic Resources
        [998] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XS2/jNhD+KwbP0lYv6nXzutk0hZMNLBdbIOiBkkY2EVl0SCobN/B/L6iHLdmSXQQ59NCclOHMNx+H8/I7mpaSzYiQYpatUPiObgoS5zDNcxRKXoKG1OGcFnA8TNujuxSFlh9o6JFTxqncodDU0J24eUvyMoX0KFb6+xrrnrFkrcCqD0t9VTiur6Hb7XLNQaxZnqLQNIwe8mXoCiPwehbGVTKzdblpGTim4Vyh0FqxPIdEdgzNrpp13S3jKSX5SEhNy3V7QXUas29UrG92IDqO8QljjHuM3Tbo5BmiNc3kV0Ir3kogWkEkSfIsUIibMLr+OW4XNWhQH4mkUCTQ4eOe2rn9CFqtKad/w4zIOhWG8sr1z8Csk+ewG7DlmuSUPItv5JVxhdcTtLeztb58AQl7BY5CU8VshILTc9iG8ytd3ZJNde9pscqBi9aJevsUhbZnOGfse1D+fq+hmzfJSVN46iGWLPpJtneFLKmkrLgltGhjq5sampcc7kEIsgIUIqShh4oEemAFIK1G2G0BhSowA3hzJuSH8R45CBhmiHQ0cl57rM6PfKItJJKTfFZyDoX8pFueoH7aXQfZnt140HulVSdIJNlWlS8tVpGEbdUoj9ybJJryz6Hchas4/FHQlxIULsKOFdgeSfQ4SDzdMTzQSUqITrCLDddIPc8z0V5Dcyrk90z5ECh8qtNTXeBQ654b2OMcF5BOZkxs2IasVkwKBfnA+IbkvzH2rEDa5vEDSPW/kguQhzrMSC6grcvmUF2urdBGVEfAMT3VlFrMSHJWdMbZiPmSboCfFP49eTscodD0vhga+l7kux9rKB6YnCaSvkKUj/Iw7A6POaygSAnfXaRyT4uOS8v94gTdP3vYx6+sjPPT6NUalhscFI6hGFXpsRzQWnK6HfPkYcs+qIz56ild8NboqVqZZhL4jJSrtZzTjZpZZn1wWkTVelLyeiiqj067n5EigXwqJWy2so220lkSvoIac6Dv2x4OzjeBC0NdrR9tx2vTewEvJeWQRpLIUs1Wtd+M5PyVHP7XGfZ/mnwoTT765p2umgTYII6T6Y4ZY91xfVuPsZfpVmwaJmSp7Qc+2v/VttVmB346COrO+vSO+i0WY2u8xSrV3eR3yPNdpqy6E8G8FJ+7FApJE5KroChntcJ0w8qip4ZCBwena4zd3zB95ankGUmarji4zTk4wFeWObzX0H/mt8FxNH94IEc/yVZJZiqqVUC7IxqF6ObPiT5ZEA6T6UtJJE0mCxCs5AmIifkLRjVEbXQEGUrubiK6sW9mONED7Ji6Y6S+TiBOdcMLMteJ48xI3SoRa9zmAhWVuWqK51z660NmBU4cZJZu+ODrTux6emzGth67ZurGdoaTJEH7fwA1PiZwVw4AAA==",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },

        // - - - - - - - - - - 
        // Weather-Restricted Rank
        // - - - - - - - - - - 

        // Export for Mission [993] - EX: Capsule Pools Distribution Survey
        [993] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XyW7jOBD9FYNnERAlavPN7U4yAdzpoJ1BBgjmQIkli4gsuikq3Z7A/z6gFlvyCgQ+DDC50cXiq1elxyr6HU0qLaes1OU0XaDxO7opWJzDJM/RWKsKLGQ2Z6KA3Sbvtu45GjthZKFHJaQSeo3GxEL35c3vJK848J3Z+G8arG9SJpkBqxeOWdU4fmihu9VTpqDMZM7RmNj2APk8dI0RBYMT9kUy06xadgwosekFCt0pmeeQ6N5B0ndzLoeViguWnygpcXx/UFTaHrsVZXazhrIX2Ntj7HkDxn5XdPYK80yk+gsTNW9jKDvDXLPktURjry2jHx7i9lGjFvWRaQFFAj0+/v45f1hBpzuqxD8wZbqRwjFd+eEBmLP3OdwW7CljuWCv5S17k8rgDQxddq41tP+ARL6BQmNiatbFpIMIXf2+iMUdW9aJTopFDqrsUM3H5mjsBjY9oDuACjcbC9381oq1N81U/knOf7HVfaEroYUs7pgoumJiYqFZpeAblCVbABojZKGHmgR6kAUgq0FYrwCNTSWO4M1kqT+M96ighOMMEUYn9puI9f6Oz3wFiVYsn1ZKQaGvlOUe6tVyPcr2IOOj0WuvRiBzLVfmvopiMdewqjvjjnsroom6DuU+XM3hz0L8rMDgIsIc3w98wIRGEaaU+DiC0MU+TSLCWepGxEMbC81Eqb+nJkaJxi+NPE0C28sd+JF3muOtkf0vpkGNJkpnSuaVAoP7INWS5X9I+WqQupbxDKz+bewl6G0DSFlegtU2hHbTZNi1htbUlIGSwLSiDnOulSx6Q+zycScIe+dnsICCM7W+ArEG+aus4nw/18bF8aOtw474SZdj3PpeT0qsTkUKPMfdupyKNXA6E631M/KepBrUlFWLTM/E0swV0mzs675+QlSqGVxm0WvJ34t8/ZxB8SD1JNHiDe45FFokZjo2lT0yHNzAiw5n9Zmxax4IXYvqpPgDflZCAZ9rpisz/cwL5IQ+L+htz8t2L6hq4Pgpkosi+eg377VBh4UpZyTBUeR5mIJHcOhRinnopYSkic1ohDZ/d32wfaW+bA1NK3x5R3s9kbine+KUrcoqh9FkGYMaNHByrjrbG2BKYkI1DpOlrIqe25F7Qb1o/xHiDh+EpivNK5WyBOa56V5tIp7p7WcfW97GQv+Zt/tukn54fprDxjI1Va0L2p+o7Rw1y8a8czsm3p7QoiiMfCekmNKAY8qYjeM09DDjrhNxj4c0ZGhjHQrJPpPAOtEiyXS2Lq8uow/r5rj+PmV0HRnRmJDAiwlmLmOY+m6CY48CjokdhgAkJj4/KiPncj8ygpBp+tmR/h9SSh3ihjwIcQguxRRiF7PUcXFgJykLeZq4QOrR1+C2FJ+B6QzU6OavER51ynmUMi9HX0WplYgr85oazSv1Buvhfw7qe04AgY0JEIZpyHwcucTGjhPwmDPCXR/Q5l9NpkasfRIAAA==",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [995] - EX+: Stardust Bait Test
        [995] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1X227jNhD9FYOvNQvqLvrNcZM0gJNdxF6kQLAoKGlksZFFL0Vl4w387wV1sSX5FgR5KIq8yeTwzJnh4cz4FY0LJSYsV/kkXqDRK7rMWJDCOE3RSMkChkhvTnkGu82o2bqJ0Mj06RBdr+aJhDwRaYRGxhB9lVxIrtblj5v88iVMiwii3bI+v6mwb4UIEw1efpj6q8R1/T4uIR3k09AlBvU6J8hZMpOkWDYMbIPYZyg0p0SaQqiOZMg2SCcl5nkWQkacpUfwDNN1+zl3u8S6vMeBeAY0ilmaNx6ueJ5criFvxer0IB2nA+k218WeYJbwWF0wXkasF/JmYaZY+JSjkVNfgOvv47ZRaY36lSkOWQgtPm7/nNvNvdkclfwXTJiqRPQthy9Zun7gKrmJIFM8ZKm2ahLY2h+Hij/DLGWrZvOQvF1/j4nZU4FVM5knLOXsKb9iz0JqMp2FJjXWsLt+D6F4BolGhk74EQp2x6FfO7zgi2u2LJM2zhYpyLxxYg5RNwUnQrQ8Yu+F2PHnbTZDdPmiJOsUiS2Kvvm5mP1kq5tMFVxxkV0znjWXiY0hmhYSbiHP2QLQCKEhuiuJozuRAaoR1itAI53MA3hToe/wnXhfJeRwmCHC6Mh+5bHc3/GZrSBUkqWTQkrI1AdF2UP9sFgPst2L+KD30upeG01EkSmQ1Qlt39x6JbmZEitdTXi2mClYlRV/F1kty7H8mIDacCXDbxn/UYDGRY5txy6xXRw6BsF2QAmmoRNjg9DYjqkZWUGENkM05bn6EmsfORo9vpbedADb0uO51DzO8SItYDBTTEZFrjTenZBLlv4pxJNGaArZA7Dyt17PQW0fTFmEm2dYb7aTWi9V4duGpwtkgzlTUmSt93fk+JwvQfZe6C3Ptlu6gPxOhuiWvbTWTE+vlbUxgWy/NvaIEatFbAoLyCIm12e59RH+EEWQ9pNVWZgu3RrsIj9q0uFwwGou+eqYJ88xra3JMV8doxPeajv9LsaxAjlhxSJRU77U7dKoNvoPppyxCln1Y/3RahbNldwJVd3KtqifrOkO3R9eTgweemJqKl+j5Xv4UXAJ0UwxVeimrkeyIwI/I9g3q+dTJO8SyXvvvFU/qeG5tu1ZmEVugG0v8jDzIoIji1iWa8Qkjny0+d4U0Hpsf9wuVDX08RX1iql9opheSaGSWNu3q75xKjO9qe4VVQbjpW5TO7NDc7hD+2OO1Z1x9Vw1K2TMwrr01UE41DkzAjqbIfrP/JHZtd93N119WK+Uzb9S5E+2qlpx3iS13ZnRCLFMZH8/Uupg4/vgAZhKQA4u//ptgLf9cqAxB3PI1eBWDyNt2JarA+JvCTUOmAsuGNi2LB/bMYsxNWwD+65FzQiYGcQW2gz3hWgdT8JMRAwHurXfLFeQh0rIT0l+SvKtkmQGI8QyTBw4LsM2JSEOTOpj07H9yPGpz5z4oCRPJGG8BJWsNUH+jyg+XI2f8vv/yC9wY8cnHsE28y1sM2Jhaun+7RrEc0xCrCgoW3eFW4dZEjPfQKy8+Y5DO3Q96scxNn3f1A4ppiFQ7BoOBeIz6kce2vwLlC7AMF0UAAA=",
                "AH4_H4sIAAAAAAAACu1XW0/rOBD+K5Vft0Zx7s5b6QKLBBxEilgJHa2cZNJapHGP47BwUP/7yrm0TXo7qlhpV+KtHc98c/HnmckHGpVKjFmhinE6RcEHushZlMEoy1CgZAlDpA9veA7rw6Q9uk5QYPp0iK4Wk5mEYiayBAVkiO4lF5Kr9+rPdXHxFmdlAslarO2XNfatEPFMg1c/TP2rwnX9Pq5hdJAPQ1cY1OtYGEeDGc/KeRuBTQz7SAitlcgyiNWeCtnE6JTEPB6FkAln2R48Yrpuv+ZuN7Bu3KNIvAIKUpYVrYdLXswu3qHYyNXpQTpOB9Jtr4u9QDjjqTpnvMpYC4pWECoWvxQocJoLcP1t3E1U2qDeM8Uhj2EjHrdv53Zrb7amkv+EMVM1iR4L+JZn709cza4TyBWPWaa12gLuYrDrbzkzexdtNc4mM5Zx9lJcslchtb+OoM3eGnblDxCLV5AoILqme0KwOw79xuE5n16xeVWXUT7NQBatE3OIulkeSNHyDHsrxY4/b7kcoos3JVmnD6xQ9OVORPg3W1znquSKi/yK8by9L0yG6KaUcAtFwaaAAoSG6K4KHN2JHFCD8L4AFOhi7sC7EfqaTsS7l1DA7ggRRnvOa4/V+TqecAGxkiwbl1JCrj4pyx7qp+W6M9qtjHd6r7QetNJYlLkCWVto/fbWa8qFSix0w+D5NFSwqJr6OrOGliP5OQltwlURPub8RwkaF4Hr+L4FFnZjQrDtGT6OPLAws9LUphGzWeSg5RDd8EJ9S7WPAgXPH5U3ncCqu3guNffHeJ6VMAgVk0lZKI13J+ScZX8I8aIR2l71BKz6r+UFqNWDqfps+wybw82iNqI6fZt4uge2mKGSIt94f3vMJ3wOsvdCb3m+OkKBd+YM0S172xCZ3pmx5d2wNrzfwBTyhMn3owH0EX4XZZT1K1JrmC5dKazT26vSiWGH1kTyxT5PnmNaK5V9vjpKB7w1epr8o1SBHLNyOlM3fK7HHqkP+q+i2pVKWc9V/WNjIlRjaQb5nVCjWPFXWHXug43bodtLyIEFQm8+bXtrCfsAP0ouIQkVU6Ueznq12sPiI6z8ZfZ8keQkkpx65xtNMrUd6nsswimkNrYd08LMYRZ2Ypf6kUut2HXQ8nvbJZv1+3klqBvl8wfqdUz7QMe8lELNUq2/2drJocr0trMPVCuM5noWrdV27dMO7e8yVndX1ctTWMqUxRBmunE1STjUObLnOcsh+s98kKxn7MmTVRtrSTXhm0+z9axFAbr487cBXg27gdYdTKBQg6pwNUBtsobYReoNAvqRFfm+E+OU2RG2iRFjSlwTW2BRylyH2GaElsNtgln7kwtFwnCk5/L1fAFFrIT8otr/jGr4NDp5CSNe4nrYjkyG7TQ2MAUg2DAM3yImc4jn76KTRfcn8JgrrjJIBk9CvgzuxNmA+p/Op5MJtJuIX3xak6doa9OjGMtF/tczpQ42vw+egKkZyMG/0OJii6au7xmYJJRg20gMHJkexSx2LWYQm5nEqmZsjdukWQVGfiGwW/0V13WYeDSGiOAo0Y8gZhZmphVh2/UBYtuxgTho+Q+gLBnhzhMAAA==",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },

        // - - - - - - - - - - 
        // Time-Restricted Rank
        // - - - - - - - - - - 

        // Export for Mission [994] - EX+: Large Saltpeter Shore Resources
        [994] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1X227bOBD9FYOvKwa6ULSkN9dJswGcNIiT7QLFPoylkc1GEV2KSpsG/vcFdYkvkR0lSLH7UD/JI/LMmeHhzOiRjEotx1DoYpzOSfRITnKYZTjKMhJpVaJFjmWux5DHmJ1LGS9IlEJWoEXMponIcb0pabecJSRyg3D/3kslpBL6gUSORc6Kkx9xViaYrM0GZ1X7aHY+kurBNU8VPg8scrq8XigsFjJLSOTY9hbyYegKIxz24mi/SHK8KO/2JII5NtthOjRMe/htwWWWYazbyJljO322uy+zlioRkO0h7richzvMeeD3cb0d8Ggm77FZ1Xj+KIrFyQMWGzH5O658v1eSeCsTuMXpQqT6A4gqVcZQtIaphvi2IJHfHDwPnvvr4y1svF2CFpjHuMGf7+LxfofstpBK/MQx6FrsNwV+yrOHz0IvzhLMtYghM6vaA+q6cTx4RsLtqTSvIXG9gEzAbfER7qUyPLYMbRY9a9t+hbG8R0Uix5zZHmqsF5H2OD+I+SncVfkd5fMMVdE6N7JOSOQNbfYs2l4ugtXKIic/tIKm5BmBXMvpd1ie5boUWsj8FETeni11LDIpFZ5jUcAcSUSIRS4qcuRC5kisGuFhiSQyiezAm0hzdG/Eu1RYYDdDQsme97XH6v2az3SJsVaQjUulMNfvFOUO6rvF2sn2WcSd3qtVtXCmWi5NuRH5fKpxWbWcNfdGXCP1PpQ34SoON7n4VqLBJb7jpuAnCY1DP6XMS0M6sznQIcSB77IZInKysshEFPpTanwUJPpSy9ME8FRrhtz0rX0cj8sc0zrewUjphZJZqdAAX0h1B9mfUt4aqLaEfUao/ht7gfrp9jYXp/7fvDQhtve6MdV5YM7QlMYWc6qVzDfGiY7t5/DDWK/FnSkcrnNkP4O0vQ3ICc4xT0A9vAPXCvimwGNZNuvXw46xvJCSLTSXm8Drfeuwn0LbKYbnIt+I2uFHzCJmduhaa+w76+0jFm7+ggNk+iSsY/NNgddKLLfTUltelZah75os1zt/eWL4kbeVF3aQzutT02w3tWSUalRjKOcLPRF3ZtZw6he7RaYakEtVDznmYaN9Vr19gfmF1KNYi3t8avIHGrw39MPdUdI03L1Tnhlz237QXvsr/FYKhclUgy7NRGTm6D214IW7/drrurWw86a9dKV6C31zVad4X1bpK7Tzi0Sy78wPfo+stnqOl7AEXJfRGXo2ZXEQ05CDR7k9c/gMQ8fxhmT1T9t0mpnpy5Oh7jtfHslOA/L8/Q3oCrMy1pDrwSUsUX1HMV/ora7pHMrSzrj7SOoFoztZ5lvLSMT8cHcE9MyFWKckMJ5KlUKM08z0g+5PND/0uwbnjaHRX1nkV3+J9v7iXI8vbx5azGZjGZusVgndHGNIRExlTQYnf/8xoIMJqDkOppDpJWpUg+lCKhxcYSFLFWMx+AuUMOftkBq4hlpDdwl/U6QzL7XBS6iX8CFlPB3SWeg4FPw0TRyeQug5ZGU9FyHfH/RpBoUh9lXBV/j+/1Hfb7l1y42+TTocbM9FN6Qc3ZSyGdg0dNyQxuAGnuuFSRywTukcGKDXOr+SBb67dLrrz28l/cdKCj035QEENLH9kDIAlwbMHVKWpHESBMxN3aDqlDVuQ/E1VfLcfL9u+UyDEBzm+dT3YkYZTxI6A9Ongxhse8YZA4+s/gX/CdRvpBUAAA==",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
        // Export for Mission [1001] - Lower Soda-lime Float Distribution Survey
        [1001] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1Xy27bOhD9FYNrKRBp6rlz3CQN4KRBlYssgi4oaWQTkUWXoty4gf+9oB62ZMtOEWRxF93Jw+GZc4bD4fgNTUolpqxQxTSdo+ANXeUsymCSZShQsgQD6cUZz2G/mLRLtwkKiOcb6EFyIbnaoAAb6La4eo2zMoFkb9b+2xrrToh4ocGqD6K/KhzHM9DN6nEhoViILEEBtqwe8nnoCsN3ezusd8lMF+XyhDCKLXrAyLb7jFoQkWUQq1YJxRbuupH3WQiZcJadIIKJ4/gHTJx+bvpEJ5FYAwpSlhVthGteLK42UHQ42ufFOe1xsRcIFzxVl4xXErWhaA2hYvFLgQK7OQDHO8btovoN6gNTHPIYOnycw31OP9mk3Sr5b5gyVRfRUEU63hEYOailcQP2uGAZZy/FNVsLqfF6hlbd2Ojbv0Ms1iBRgHXOTlCgvYBtOi/5/IYtK92TfJ6BLNogxEBPXC1uE8gVj1k2bQEH8MeuRY8k9uJ5262Brl6VZM291qf1KMJfbHWbq5IrLvIbxvP2AExsoFkp4Q6Kgs0BBQgZ6L5iiu5FDsioETYrQIHO3gDeTBTqw3gPEgoYZohMdGK9jlit7/mEK4iVZNm0lBJy9UkqD1A/Tesg2yPFg9Err7qKQiVW+o7zfB4qWFV9eM+9qbSJ/BzKXbiKw385/1mCxkV+mjJCiWVGlheZNHGx6Uepa9pgYycapzhOfbQ10IwX6luqYxQoeK7LUwvYNQTX8clpjpdZCaNQMZmUhdJ490IuWfZViBeN0LaXJ2DV7/oK6dUClJbQXiZteuRLkAeX7I7nuyUUYHphGeiOvXZspLLp92Nov7b3MbwL2ph7MJhq+7c82zwtIL8XahIrvoYw63PUua/OgmJX99BWYKikyOfvSuxst8ad7TOYQ54wuTmL0BdC8LlkHCSOXDinVQ8x+yLKKNudW8+DOP7OYa/7pEtP24DXo+SrU5Fcm4x3Lqdi9ZzORGv89BWdpArklJXzhZrxpX5Pcb1weHeroauU9YOtPzpP0VGx7J6Msy+G7R+OM2fnIz1StW22vVbf4WfJJSShYqrUr76e2Q7v2t+V619X5b8i+VCRfPTMO60c/DSNXN8yEyuKTUoj14xcm5mUuq4XuR5xCUPbH20vb+b6552hbufPb6jf122Pnu7rN5IvR7fLFRSxErL3CuFz6dlPTax+l2uHyVKUecdtaMy3/cNJatyfhD0duJQpi5u23CixffudKdPeGuh/83dnPw58eAjQm7VlqrNaJbQ7FjTDgP6szXu3oertVBqjnh27KTatCI9NGrnUjBKgZhI7LnNSxyKJh7bGcSWdE8DWm9FUQMZilqvFp5fSh2tnuAb/ldLDp5QSuC4jkBDTJgk086ftUpN6KfMiz7djwFXTqnEbis/YsvCPkZ5OkpE5molfIEehSJiZ8SWMrjPB1OgLL5TkUalfxFFYyjVs+rOv52B37KWemToMmxSs1GS275mO57iEuKlnRyna/gGYDDnpcxEAAA==",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },

        // - - - - - - - - - - 
        // Sequence Rank
        // - - - - - - - - - - 

        // Export for Mission [997] - Hyper-aetheroconductive Materials
        [997] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1X207jSBD9laif3ciX9i1vIcswWSUMIrA8oNWq7S4nLRx3pt0GMij/vmpfiB0cMiBGWmknT0511amLT1WXn9GoUGJMc5WPkwUaPqOzjEYpjNIUDZUswED6cMoz2B2y5mjC0NAOQgNdSi4kVxs0tAw0yc+e4rRgwHZirb+tsGZCxEsNVj7Y+qnE8QIDna+vlxLypUgZGlqm2UF+G7rECP2OhXk0mPGyWDUREMskR0JorESaQqxahlZbzT7uVkjGaXqgpJbteZ2iktrsC8+XZxvIW47dvYhdtxOx1xSd3sN8yRN1SnkZtxbkjWCuaHyfo6Fbl9ELXuO2UcMa9ZIqDlkMrXi8fTuvW0G7MZX8B4ypqqjQeN23tvfq79TW10uacnqff6EPQmqAjqBJxzG68iuIxQNINLR0kfq47AWaAi2HTf1O+eKcrspER9kiBZk3TvTLZmjo+CZ5FX0HKthuDXT2pCTtdNpLAPpFXIv5I11PMlVwxUV2TnnWlAdbBpoWEmaQ53QBaIiQgS7KmNCFyADVCJs1oKGuUw/eVOTqw3iXEnLojxBhdOC88lie7+KZryFWkqbjQkrI1CdluYf6abn2Rvsq417vpdaVVhqLIlMgKwut37z1ik1zJda6uXm2mCtYl2N0l1nNuJH8nITacGWENxn/XoDGRabpxaHrRTg0WYSJ6wAOw9jEdhInQUiD2CQm2hpoynP1LdE+cjS8ey696QReJoHvhfbhGE/TAgZzRSUrcqXxLoRc0fSrEPcaoZkrt0DL/1qeg3ppmISmOTQdXB+2i1qLqvSJ5et51WDOlRRZq/8OmF/zFci9Dp3x7OVIz4YT00Az+tSS2faJY7Z/5FUwptMKZgoLyBiVm0/IsgS+yeEPUdT6jWIlOVLMDprt6ZJVdruCHaxLpwZVXb5l6eZ2CdkoVvwB5umBsNuOfqYYPcY3OVxLvu6mXEnelbLv2rqCleWvTbrj6v1p1+Z6aIwSBXJMi8VSTflKX+1WdbA/TcotrpDV7qAfWpdkz03o+G74ehl6Y6/RG1gz9Js2voLvBZfA5oqqQq8XesU70NtHevW9TdRR7OX/MaL/NEXbWr20O86vdzDjF1Hgo++8dXWA50XMBQ9HJDQxiVwTh0EUYN9yacKsKA6Ih7Z/N3dH/Rlw9yKoro+7Z9S9R1z/jXvkJlNcpcAGt0LeDy7EycD33M7lZ71VpQmDTPGYpro02mWlMFrp23qn1tMfxA33tz2nu3kH2nEhExrXg6BOyA3dI0uuuzXQf+YjabeFfHj30MZaUu5AFTsf6braSPKmqO0FBQ0RzUT2z2gwmQzw4OtmDRJTUEuQIhYZK8rxOphRBZLTNB/M9E7Whm256mmEFmltQi2bmSE2LdPChHkBpoQCtihxnIj6ceIztDVay3o9kbskJYF3uChadTP4E9J0k2irz6bnh/nYz+vf9NzjIv4YtTxCbIc5CQ58RjCBJMRBZNmYgBv5fkBN0/JLau3PuzcSGK3XKURSPP4m0f9txv1FJaeZGlgfYyMLIKZh7OLYD2JMXDvGgRn6OGTUpn7C4siLytu5wm349t7o7O7nZOAmkR+7JrbCKMLE1p+TJAkwgOdGIQtYZFlo+y+H0PtBhRQAAA==",
                "AH4_H4sIAAAAAAAACu1X227bOBD9FYPPYiBRJCX5zfWmbRZJWsTJ9qFYLChxbBNRRJei2rqB/31BXWLJseMmSIEFtn6SyZkzF5256B5NKqunorTldL5A43t0Wog0h0meo7E1FXjIXZ6rAraXsrs6k2hM4sRDH43SRtk1GgceOitPv2d5JUFuj538psG60DpbOrD6gbinGofHHnq3ul4aKJc6l2gc+P4A+WnoGiOJBhr+UWemy+qu84AGPj3iQqel8xwy21MM+mLkuFltpBL5gZQGhPNBUmmr9laVy9M1lD3DbMdjxgYe8y7p4hZmSzW3b4Sq/XYHZXcwsyK7LdGYtWnk8WPcPmrSon4UVkGRQc8fvqvHhxkknapRP2AqbEOFzuquNtnJf9hqXy9FrsRt+VZ81cYBDA66cEJveH4Fmf4KBo0Dl6R9XOaxo0DPYJe/N2rxTtzVgU6KRQ6m7Iy4ly3ROIx8+sj7AVS82Xjo9Ls1YlBpDw64F3GtZ9/E6qywlbJKF++EKrr04MBD55WBCyhLsQA0RshDl7VP6FIXgFqE9QrQ2OVpD965Lu2L8T4aKGG/hwijA/eNxfp+689sBZk1Ip9WxkBhXynKHdRXi3Wvt48i3mu9lrpyQlNdFRZMo+Hku7fesGlm9coVtyoWMwuruo1uI2sZNzGvE1AfrvbwplBfKnC4iEeCUBYKzAjnmHIusZA8wwyCjIUsIIQKtPHQuSrth7mzUaLx5/vamgvgoRNEPCGHfXyTVzCaWWFkVVqHd6nNncjfa33rELq+8glE/d+dl2AfCmYu8hK6Cm4v+0ltj5rwaRC5ftVhzqzRRa/+DqhfqzswOxV6oYqHK9fKTiK//ws8dCG+9yQIOQkHEvSRa37Yc+0cFlBIYdavEHMNfFPCH7pq5TvB5uRIagdohLsENnrb9P1klviJ76EPRb7+tIRikln1FWb5Abf7hn4mGXuUb0q4Nmo1DLk5eVbIESMug43mrw16YOr5YbfqroVM5hbMVFSLpT1Xd27QB83Fbm+pd7rKNJuEe+iNzD1zMYxY8ng1emLLcftYNwK6or6CL5UyIGdW2MotG27hO1DpRyr3uUU0ENzL/2NE/2mK9qX20u44v57BjF9EgZe+894gmYu55L6MsaQiwdSPIpzMSYZTQklCUz8VbpD83U2S9qPg88NBM0w+36PhVGHRE1PlprDK5iBHn7S5HV3qk1HE2WAUBk9l6UxCYVUmcpcaZ7IRmNy52b0V21MflCW7u1843MNjZ7gyc5G1jaANiCXsyMrLNh76z3wybXeSF28iTtmd1BtRw85vYtXsJ2WX1P66gsZIFLr4ZzI6Oxvh0fv1CgwWYJdgdKYLWdXtdXQhLBgl8nJ04Ta0PmzP1J5C6JFWEOKDiGNMw8DHNAojnPAkwYxEASNxKIEA2ni91b3tyLskfSIpk9Uqh9Tob69OzBczcT+jfxNzh4X4ZaSCKEpZmlI8D5IAU+EHWPg8wiH1IY6jIJJRWJNqh0Quh4cCcKLr0Z+Q5+u50/pNpf9Zj/tLGCUKOyIv4ySlPM2Y8DGwIMA0Ax8nhFKcCWBpIv2Ep6yezg1u17qe610w/LhMMyIkEI79JJSYUilxkmQxjinnGWN+mLIUbf4FeTD5+5MUAAA=",
                "AH4_H4sIAAAAAAAACu1X227bOBD9FYPPYqEbKclvrjdts0jSok63D8ViMZJGNhFFdCmqrTfwvy+oiy05it0YWWCBrf0iD2fOXHRmOH4gs0rLOZS6nGdLMn0gFwXEOc7ynEy1qtAi5vBKFLg/TLujy5RM3TCyyAclpBJ6Q6aORS7Lix9JXqWY7sVGf9tgXUuZrAxY/eCapxqHhxZ5u75dKSxXMk/J1LHtAfJx6BojCgYW9slg5qvqvovAd2z/RAidlcxzTHTP0OmruafdSpUKyJ8oqeNyPiiq35q9EeXqYoNlzzE7iJixQcS8Kzrc4WIlMv0aRB23EZSdYKEhuSvJlLVl5OFj3D5q1KJ+AC2wSLAXDz+048MKup2pEn/jHHRDhc7robV7UH+vtb5dQS7grnwD36QyAANBl45nDeUfMZHfUJGpY4o0xmUeGgr0HHb1ey2Wb+G+TnRWLHNUZefEvOyUTL3A9h9FP4AKt1uLXPzQCgadtgvAvIhbufgO68tCV0ILWbwFUXTloY5FriqF11iWsEQyJcQiN3VM5EYWSFqEzRrJ1NRpBO9KlvpsvA8KSxyPkFDyxHnjsT7fx7NYY6IV5PNKKSz0C2V5gPpiuY5G+yjjUe+11kejNJdVoVE1Fka/e+tvpEqw7sFDYWqkdVcFPHKtlncLLddmDIhiudC4rgfuvgYtN2fqZVLvw9W5fCrE1woNLklSm3MGEc38NKF+EKU09nhEYwa+z1wnTDKPbC1yJUr9PjM+SjL98lB7MwnsZkaT3VMxvs4rnCw0qLQqtcG7keoe8ndS3hmEbgJ9Rqh/G3mJetdaGeQldr3eHvYr3Yqa9H0nMJOtw1xoJYtepz5hfivuUR308rUodkdm6L0K7P7Hscg1/OhpuO4rb6DhPwrN9nqhXeESixTU5gVyroE/lfibrFr9TrGRnCjtAM3lpoCN3b5874t883mFxSzR4hteplhokZjbbySePsLPZDlifKvE+plhB8z1dpZnBj7AeH7orbnp71mmUc2hWq70lbg397XTHBw2fr2aVapZCMxD7+Ybud68gEWPN5wjy4pZq7pJ3nXcR/xaCYXpQoOuzM5g9rYn2vBEWz2X4QPFUXIeY+GzaHaST8eJ80xm/EsUOPed96a8w30ndnlEs4xn1A9ZRCOwOXVjD4IkDIIMA7L9sxvz7W7/ZSdoJv2XBzIc+Sw4MvI/FVroHNPJZ6nuJjfy1STgbHBPOceqtGtTUxrjslGY3ZsreK820h8+iw5XOG+4TofGcaUySHCRm4naJsQidmJzZVuL/Gf++ewXhrPXBGNsJPViUxe0vzi064J5bMR7tTES9wjHgIfgc5u6LLOp7wScAmOMplmI4EGEISDZWo8JdSSB2XqdY6zk9xcn0dmsGWffLxLtGVN2tTngFRSy+Gs2ubyc0Mm7zRoVBdQrVDKRRVrVF/XkGjQqAXk5+QOUgEJPnPPYyF0eIHge9VwnpD7YLgU/9agNQWy+iOEoG83LeKoSRnUz+R3zfJMZq1+c/J9y0j1zQvpuApA41MY0oj53IxojR5qxIAljn0OYhfWV3OB2M/Bno7s2/28HDiENHWQeo4huTH0OHo1Dl9GY+RnEvpNx3yPbfwCInF74ShQAAA==",
            },
            AmountRequired = 3,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Untitled Work No. 765"] = new List<uint>()
                {
                    47572,
                },
            },
        },
        // Export for Mission [999] - EX: Rare Aquatic Resources
        [999] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACt1XyW7jOBD9FYNnKaN9uzmeJBPASQdWGj1A0BhQUskmQpMOSaWTDvzvA2qxJW+ZDnJojE9yserV41ORVXpD40rxCZZKTso5St7QBcMZhTGlKFGiAgPpxSlhsF0suqXrAiVOFBvoThAuiHpFiW2ga3nxktOqgGJr1v7rBuuG83yhweoHRz/VOEFkoKvV/UKAXHBaoMS2rAHyaegaIw4HEda7ZCaLatkx8GzLe4dCF8UphVz1Au2+m/N+Wi4KgukRSW0nCAaiem3YJZGLi1eQvcT+DmPfHzAOOtHxI6QLUqpzTGre2iA7Q6pw/ihR4rcyBtE+bh81blHvsCLAcujxCXbjgqGCThcqyE+YYNWUQpd1N9rZ0d9to+8XmBL8KC/xMxcaYGDotuMaQ/sMcv4MAiW2FulQLQeRLoFewk6/czK/wst6o2M2pyBkl0S/7AIlbmh5e+wHUNF6baCLFyXw4KRtCOgXcc/TH3h1zVRFFOHsChPWyWPaBppWAm5ASjwHlCBkoNuaE7rlDFCL8LoClGidDuBNuVQfxrsTIOEwQ3Tx98gczbCA0fipworkoxlIXokc5Mj/w0cG0lENwg6bZvMHwRuHk+BuDb7darqCXAlMJ5UQwNQnCbiD+mkyHmSLTHTSq1VFe82004RXTIFoIrR/p2lTqKniK31vEDZPFazqG3q7s7aYx+JzNtSHqxl+ZeSpAo2LQivzMhcXZh7mrumBV5qx5XimZfteEQZR6Wc2WhtoSqT6UuocEiUPb3U2vYHNJRMGsXecY6qAUixGKaZLzkYzDhr0loslpn9x/qhhunvrG+D6v7ZLUJsDWWIqobsh2sW+sq2p0cCzQ30fdpipEpzNP4J6Q5i23pNlc0edWXuZLLeXaQpzYAUWr5+QzD3z4v4vMtANful7RGe+1f+Fh8n9yauM7grbeDhBvHHYU2nf5dD2+l73gqyOZQp9x924HMs1cDqRrfMjS+CVusEvh9XQB21cKhATXM0XakqWutPazcLuCayHqko0rVw/9HrWF0Zfvy2A3XI1zhV5husCmCK5nheal3egdbmhH+9PLycGET0ydRdtdy5m8FQRAUWqsKr0PKBnst/zsPzng/FLCP/f6v2l6vwqYVJJxZdNITTl0fsaOFK4n1SWvZ6RO9iPs6gwsZ+VpudGuZllODDLEhdlEQcxRAFaf++aRvtp8bAxNH3j4Q0NG4gfhscbyGVFqZlR/oONzqsso8OOZ7+nZb2wbcxbMdozrAXUTJrY8VI7bRFqfn68O0m6w6k+0iQqUeIcUqorut2YH/vvDND+2kC/zQfYdgz58PChg7Wl1doejiMoQZhx9s9DHMem/X10apK0d8bULeL+CbEcb1CmnpU5mRVgE3zHMr0sKs3MCwLTdmIAy7LKMIrrMm2g2w3VrPzTrJrhuX8iIAtLBzIzxrFrerljmVHug4mdzA2j0MnDyEfrfwFg+oC21w8AAA==",
                "AH4_H4sIAAAAAAAACtVX227jOAz9lUDP9qwv8vUtk+10C6SdIm4xCxSDhWLTiVBFSiW5M50i/76QL42dpEm36MNsnhyKPCSPSZF+RuNKiwlRWk3KBUqf0RkncwZjxlCqZQUWModTymF7WHRHFwVKvTix0LWkQlL9hFLXQhfq7GfOqgKKrdjobxqsSyHypQGrHzzzVOOEsYXO1zdLCWopWIFS13EGyMeha4wkGlg4J4OZLKtVFwF2HXwihM5KMAa57hm6fTXvtFshC0rYK5S6XhgOSMWt2ReqlmdPoHqOg52Ig2AQcdiRTu4hW9JSfya0jtsIVCfINMnvFUqDlsYw3sftoyYt6jXRFHgOvXjCXbtwyKDXmUr6CyZEN6XQed219nb491vrmyVhlNyrL+RRSAMwEHTp+NZQPoNcPIJEqWtIOlTLYWxKoOew4+8zXZyTVZ3omC8YSNU5MS+7QKkfOXgv+gFUvNlY6OynlqTtNMP8jch+kPUF1xXVVPBzQnnHh+1aaFpJuASlyAJQipCFruog0JXggKwG4WkNKDXEHMCbCqXfjXctQcHhCNHZ3yN7NCMSRuOHimiaj2agRCVzUKPgjwC9Yt3Eg+zOV32arSHXkrBJJSVw/UEc7KB+GBMHo60zOqLVy3tmlCai4hpkY2H0uypsiivTYm16nfJFpmFd36rbzNoCHMuPSagPV0d4y+lDBQYXlW6ZJ1Ewt4t57Nk4wsRO4qKwA1KWc1yUhQNztLHQlCr9tTQ+FErvmtI2CbxcDFGY4NdjzDQwRuQoI2wl+GgmwIBeCbki7C8h7g1Md9d8A1L/N3IF+qWLS8IUdF3dHvaZbUUNB9iNzB3WYWZaCr54D+ol5UZ6Q1fmXvGcT4HT+4V7bh2/53YKC+AFkU8fkE8NfKvgT1G1+p1iIzlB2wDNCw05jd2QmjrPnYtzhwL3E076P99CXzl7+rYEPs41fYSMvZJE3+1bqDlgfCPpei/XViEKPP9FZe+NH1I6FMRQzzTquNQgJ6RaLPWUrsx0dZuD3Q6uF6lKNuPbPPTmVEfRldANSxcFcE1zsyM0VB0YV34UJPsby5Hlw6xJ3VXc9dUMHioqocg00ZXZAcwe9ns225t76T8hHOyPU43w5qL931TnrYJJpbRYNYXQv0GOFe4HlWVv5hQEB6U/d23XIYWNC8+3ExwVdukSSOLYiUg8R5vv3dBpPyfuXgTN3Ll7RsMBFEThkQG0JIxxoUfZEhgbjEv3GJEvXWooMr4ahfHKzPe+GkpxkOzuh/5wV4+Np0qWJG/vyDb0IAlOrMXBxkK/zWfVdlF593pijI2kXpOaiv1B1s3SorpSu+7tMChFhAv+z12SJHbwfXR6Pd0Ctk68V9piW5h+4pOicLEdExzZ2PdCO8ZhbDu5S0JSBn6Uh3VhNrhtgnVI+HhIuA6p5yov/SAnJdhO4pY2LhPXnnvg2qGDHRfChMQhRpt/AUdNNWe9DwAA",
                "AH4_H4sIAAAAAAAACs1X227jNhD9FYPP0laiqOub182mAZw0iLJogWBRUNLIJiKLDklt4gb+94K62JItx9s0aOsnecg5c+Z4RjN+RdNK8RmVSs7yBYpe0UVJkwKmRYEiJSowkD6csxL2h1l3dJWhCAehgW4F44KpDYpsA13Ji5e0qDLI9mZ9f9tgXXOeLjVY/YD1U43jBQa6XN8vBcglLzIU2ZY1QH4busYI/YGHdZbMbFmtOgbEtsgZCp0XLwpIVc/R7l/D58NykTFanJDUxp43EJW0bl+YXF5sQPYCuweMXXfA2OtEp48QL1muPlNW89YG2RliRdNHiSK3ldELjnH7qGGLeksVgzKFHh/v0M8bKog7V8H+hBlVTSl0UQ+98YH+Tut9v6QFo4/yC/3OhQYYGLp0HGNov4OUfweBIluLNFbLXqBLoBew0+8zW1zSVZ3otFwUIGQXRP/YGYoc3yJH7AdQwXZroIsXJWjbaVr5ex4/0/VVqSqmGC8vKSs7PUzbQPNKwDVISReAIoQMdFOTQDe8BGQ0CJs1oEgLM4I351K9G+9WgIRxhshEJ86biPX5nk+8hlQJWswqIaBUH5TlAeqH5TrK9ijj0ej1raZAYsXXul9ZuYgVrOs34557W0RT8TGU+3A1h68le6pA46KMAgnCxDM9m1CTWBibgY8TEycUey4Edp5ZaGugOZPq11zHkCh6aMpTJ7Brbt8LyWmOsYKioGIS02LFy8kdBw16w8WKFr9w/qhhuvfFb0Dr79ouQe06MaeFhK4z20OdXtejranRgNi+fg91mLESvOxNsBPu92wF4qD1r+nL7ghFtv/Jtfof/yiw5fQCz2EBZUbF5gMyqoF/5lVSnNNo4Ii9cOe31+FkuqzspYvxJ+tQAuxr28kQP5LxiPO9YOujvNoLvoud3ZVhCicujZE4uMdWwCt1TV/Gf1TdodNcgZjRarFUc7bSo9FuDg5bt96CKtHMXv3QGzIjk8Tx3fB4mXhjL9AbTPcO7drlDp4qJiCLFVWVHs96Rfp3eugj6ni0Qf55J5wr+R8u2f+yNv9W7X2VMKuk4qumEJry6C3nJ8ryvXXYmx2enSQpDRLTshPXJEkAZphkYGI/z70AuxhyirbfuuHRrvYPO0MzPx5e0XCQuP5bg+SRrdesXDzTzWDm2W/JdpVBqVhKCy2IDtRcmK54VQ6uoYi44eGi5gyX5kBHqkROU4gLXaGjWzpxQ/fMuupuDfS/+buzXz7evXJoZ22ZaVWbcn2m62YRkZ02t729BEWIlrz84yEMQ5N8m1z8PjEnd1TAZPpUUcXSyR1IXokU5IT85KI+YC/IWE/si9TGLiEEPDPL9IJDaWiGBHumHYY08Yjl0dSpi7TBbROsKTlvU3JqSr1QBLtgO+CbIRDfJEEYmIFrBWaQuDlNLS+wvQBt/wJZbl89VQ8AAA==",
                "AH4_H4sIAAAAAAAACt1XW2/rNgz+K4Ge7S2WbfnylpP1dAXSrqg7bEBxMDA2kwh1rFSSz2lX5L8P8iW1Hfeytg/D3hyK/PiRIkXmkcxKLeagtJqv1iR+JCcFLHOc5TmJtSzRIuZwwQt8Oszao7OMxDSMLHIpuZBcP5DYsciZOrlP8zLD7Els9Pc11rkQ6caAVR/UfFU4LLTI6e56I1FtRJ6R2JlOe8gvQ1cYUdCzmL5KZr4pty0Dz5l6r1BorUSeY6o7hk5Xjb7uVsiMQ/5MSh3KWC+pXmP2lavNyQOqjmN/wNj3e4xZm3S4xWTDV/oL8Iq3EahWkGhIbxWJ/SaNLDzG7aJGDeolaI5Fih0+bGjH+hmkrankf+McdF0KrdehNR3k322srzeQc7hVX+G7kAagJ2jDca2+/ApT8R0liR2TpLFaZqEpgY7DNn9f+PoUtlWgs2Kdo1StE3PZGYndYOodse9Bhfu9RU7utYSm00zmr0XyA3ZnhS655qI4BV60+bAdiyxKieeoFKyRxIRY5KIiQS5EgcSqER52SGKTmBG8hVD63XiXEhWOMyQ2eea89lidP/FJdphqCfm8lBIL/UlRDlA/LdZRtkcRj3qvtOoCSbTYmX7lxTrRuKtexifuTRHN5OdQ7sJVHH4v+F2JBpek1HdDdFPb8ymzPRalNiyzwA7DIFqyzPOXNCN7iyy40r+tjA9F4pu6PE0Ah+YOWOQ9zzHRmOcgJwnkW1FMrgQa0Asht5D/KsStgWnfiz8Qqt91C5pThdrE0TajEV3zLcpBk57z4nBkXoefphY5h/uuLDSyBrJOlucE5sFqnSdaiqJqwEbr4GMFuULro6yiY1IOGyE1dTukFrjGIgP58F5eQ+BfRLnMD4l+BrFnSFl0sOvn6A03QUduIjgKuuviLRGPGF9LvjuKq1EIfOoeVI6ueUxpjERfz7TxbKVRzqFcb/SCb838dOqDYX9Xq1Ip6wFtPjqTaGTcuIEfDefsizuLWXPah7btqSu8K7nELNGgSzPDzR41bLQPVtQn9s+bGuDjlf5aSb+5JP8XtdcWm/cvi60zRZYsTCH1fNulfmR7NPBtyDzXXlFvFaSMrhzqkP23dow0S/7NQVBPkptH0h8pfuA/P1JOc1BqMhcS8t70c17KzVmGheYp5CYhxlGtMNuKsuipkdjzo+HK5vbX59B4KuUKUkxyU4YN7+O+HW6q/t4i/5l/Ok97x7u3DWNsJHOTxroIf8Cu3kFU256XnZWExAQKUfx1E0WR7X6bnPw5sSdXIHEyuytB83RyhUqUMkU1cX/2SRew42Sk0jtViRkL0nAZ2l4YurbHwtQGGqJNnaU7XU5DOg2wqsoatwmwokRfpkQrSh1XkQ8rDJhvpyyjtud5zA59oDYLM5dOIVxmAGT/D47Bq/xQDwAA",
                "AH4_H4sIAAAAAAAACs1X227jNhD9FYPPUivqRklvXjebBnDSIMpiCwSLgpJGNhFZdEhqN9nA/15QF1uSlUuTFO2bTM6cOXM0oxk/onml+IJKJRf5CkWP6KSkSQHzokCREhUYSF8uWQmHy6y7OstQZAehgS4F44KpBxRhA53Jk/u0qDLIDsfaftdgnXOerjVY/WDrpxrHDwx0ur1eC5BrXmQowpY1QH4eusYIycDDepHMYl1tOgYuttwXKHRevCggVT1H3DezXw7LRcZo8YSk2Pb9gahu6/aZyfXJA8heYG/E2PMGjP1OdHoL8Zrl6hNlNW99ILuDWNH0VqLIa2X0g2PcPmrYol5SxaBMocfHH/v5QwXtzlWwn7CgqimFLurY2x7p77Te12taMHorP9PvXGiAwUGXjmMMz68g5d9BoAhrkaZq2Q90CfQCdvp9YqtTuqkTnZerAoTsguiXnaHIIZZ7xH4AFex2Bjq5V4K2naaVv+bxD7o9K1XFFOPlKWVlp4eJDbSsBJyDlHQFKELIQBc1CXTBS0BGg/CwBRRpYSbwllyqN+NdCpAwzRCZ6In7JmJ9f+ATbyFVghaLSggo1QdlOUL9sFwn2R5lPBm9tmoKJFZ8q/uVlatYwbb+Mh64t0U0Fx9DuQ9Xc/hSsrsKNC5Kk5wEmNgmkCQ33cQiJvVsbCau71k0B58ARjsDLZlUf+Q6hkTRTVOeOoF9cxM/dJ/mGCsoCipmMS02vJxdcdCgF1xsaPE757capvtefAVa/25aUN9KUDqPrhn10TXbgBg16Tkr91coCn6xDHRO73tH2NdnLWKjlYuJ/l51sWMleFn3X2u1D5HTQoLxXlLhK0lZTo/UElZQZlQ8vJXXGPg3XiXFXucnEAeOth/u/YYavZyzbR8nbZOjpPshXpPxhPO1YNujvFoD4tnO3uToNU8ZTZEY2bEN8Eqd0/vuNeq+nucKxIJWq7Vaso0eqLi5GDd8vTtVopnY+qE3mibmj0O8cDx4n11i9N7TfXm7JruCu4oJyGJFVaWHul6sxp33zhr7wI56VUu8v/ZfKvJXF+l/WY3/qPa+SFhUUvFNUwhNefRW+n+3LHsDyCFZAoHvmFYCoekS4plhCI5peTi3iJU5QWCj3bduArX/D272B80QunlEw2nkEefpafR1zRTMYkVFUgmpBsMTP6fkWQalYikttEY6WGMw3/CqHJihyPXC8cbnDLfvQEeqRE5TiAtdtJPrvnus7njv9XYG+t/8bzpsMW/eXbSzPlloVZsK/kG3zUYjO20uewsOihAtefnXTRiGpv1tdvLnzJxdUQGz+V1FFUtnVyB5JVKQM/tXD/UBe0Gm2uRQqK4FlDhWZtpgp6aburmZeEDMzEpTTHCCSeDUhdrgtgnWlPDzlHBNqRfK93Iv8ezM9K2Ami4NAzMheWg6xM0dij3qeSHa/Q0NZWdrng8AAA==",
            },
            AmountRequired = 5,
            UniqueFish = true,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
                ["Full-blown Bubble"] = new List<uint>()
                {
                    47577,
                },
                ["Glass Coral"] = new List<uint>()
                {
                    47492,
                    47558,
                    47575,
                },
                ["Shallnot Shell"] = new List<uint>()
                {
                    47576,
                },
                ["Skippingway"] = new List<uint>()
                {
                    47491,
                    47557,
                    47574,
                },
                ["White Starburst"] = new List<uint>()
                {
                    47490,
                    47556,
                    47573,
                },
            },
        },
        // Export for Mission [1002] - EX: Bubble Bursters Distribution Survey
        [1002] = new FishingTools()
        {
            FishingPreset = new List<string>()
            {
                "AH4_H4sIAAAAAAAACu1XS2/jOAz+K4HO9sIPWbF9SzOZboC0U9RZzALFHmSZToQ6VkaWO80U+e8D+ZHYidMsih72sDeFpD5+pCmSeUOTUokpLVQxTVcofEOznMYZTLIMhUqWYCCtXPAcjsqkVc0TFDp+YKAHyYXkaodC20DzYvbKsjKB5CjW9vsa604IttZg1cHRpwqH+Aa63S7XEoq1yBIU2pbVQ34fusIIxr0b1lUy03W5aRlg28JXKLS3RJYBU52LdtfMue5WyITT7EJKbYeQXlJxc+0rL9azHRQdx94JY8/rMSZt0ukzRGueqhvKK95aULSCSFH2XKDQa9JI/HPcLmrQoD5QxSFn0OFDTu+Rfgad9qrkv2BKVV0KQ3VF/DMw5+RzuA3Yck0zTp+Lr/RFSI3XE7TRuUZf/ghMvIBEoa1zdoEC7jls03nDV7d0U8U9yVcZyKJ14gwiuWMLnwXTQ/b3ewPNXpWkzTvU32Upop90O89VyRUX+S3leZtq0zbQopRwB0VBV4BChAx0X3FC9yIHZNQIuy2gUOdpAG8hCvVhvAcJBQwzRCa6oK89Vvojn2gLTEmaTUspIVefFOUJ6qfFOsj2LOJB75VVXS+RElv9mnm+ihRsq7555N7U1ER+DuUuXMXhr5z/KEHjItsiY0wYNRnBgYkp9k1KKTOZ58eMEWIFgYf2BlrwQn1LtY8ChU91eeoADk9/TAJ8mWOkIMuoHEU024h89ChAg94LuaHZn0I8a5i2m3wHWv3W8gLU4TmlNCugfV6NUofXPrRGVOcA22PdpVrMSEmRd+bbhetLvgF58n7v6OtBpVvAH9aZK8vtuFrACvKEyt1Vb6cIX0QZZ6fh1xYOCQ4Gx1gumvQ4DFgtJd9e8jT2HPdgcslXz+gdb42dLvdJqkBOablaqwXf6Clk14rTd1AtHKWsx5w+dBr4lOYMsolSsNmqNpfaZknlCmrMb3m2+76G/F6oCVP8BeYJ5IozPW/rC4Md2gvOp/87g1yvHG1bayv4EX6UXEISKapKPU/1TnOhrK+U6b+usP8L6UOF9NFv3mmdTgxOOsZjEywfmzgIHJO6JDAZAws7zAVIE7T/p+2dzd77dBDU7fPpDZ30USe43EdvyjjOYEV/gey1fPu93BzqXydEO6oNJhtR5j0zFGIvON1T3P5G6WtPpUwpgyjT/Wxwe8Ne4F1Z3ry9gf4z/wWOs/fDEzf6SbdaMtVZrRLancHN5NXHWnw0GyrdbpkR5qWBxcyAObGJXUpN32Wp6WPL8n2HgY1JVWY1bkNRj6tkNPt7NJ+PzFFdNqObUhYKZDH6wgsleVzq5jWKSvkCu/5a4GAa+CTFZoJpYOIEiOkzEps0Tr3Yc21CyBjtfwMXShI9Pg4AAA==",
            },
            AmountRequired = 0,
            UniqueFish = false,
            Baits = new Dictionary<string, List<uint>>()
            {
            },
            RequiredFish = new Dictionary<string, List<uint>>()
            {
            },
        },
    };

    public static Dictionary<string, List<uint>> MoonBaits = new();
    public static Dictionary<string, List<uint>> MoonFish = new();
}
