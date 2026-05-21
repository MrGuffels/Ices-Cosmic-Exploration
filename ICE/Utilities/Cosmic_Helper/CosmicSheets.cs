using System;
using System.Collections.Generic;
using System.Text;

namespace ICE.Utilities.Cosmic_Helper;

public static unsafe partial class CosmicHelper
{
    public class CraftingInfo
    {
        public uint ItemId { get; set; }
        public int RequiredAmount { get; set; }
        public uint RecipeId { get; set; }
        public RecipeInfo RecipeInfo { get; set; } = new();
        public bool ExpertCraft { get; set; } = false;
        public Dictionary<uint, int> RequiredItems { get; set; } = new();
        public int IconId { get; set; } = 0;
        public string ItemName { get; set; } = "???";
    }
    public enum Status
    {
        None,
        Completed,
        Gold,
    }
    public class CosmicInfo
    {
        // - - - Crafter Specific - - - //
        /// <summary>
        /// Key = What's used in the config per recipe. This keeps track of it on a per-recipe basis
        /// </summary>
        public Dictionary<ushort, CraftingInfo> Crafts_Main { get; set; } = new();
        public Dictionary<ushort, CraftingInfo> Crafts_Pre { get; set; } = new();
        public bool IsExpert { get; set; } = false;

        // - - - BTN | MIN Specific - - - //
        public Dictionary<uint, int> Gathering_Min { get; set; } = new();

        // - - - FSH Specific - - - //
        public int Fish_AmountRequired { get; set; } = 0;
        public int Fish_VarietyAmount { get; set; } = 0;
        public List<string> Fish_Presets { get; set; } = new();

        // - - - Map Related - - - // 
        public Vector2 MapPosition { get; set; } = new();
        public int Radius { get; set; } = 0;
        public uint TerritoryId { get; set; }
        public uint MarkerId { get; set; }

        // - - - Exp Modifier Section - - - // 

        public uint ExpModifier_1 { get; set; } = 0;
        public uint ExpModifier_2 { get; set; } = 0;
        public uint ExpModifier_3 { get; set; } = 0;

        // - - - Universal Info - - - //
        public string Name { get; set; }
        public List<uint> Jobs { get; set; } = new();
        public uint ToDoId { get; set; } = 0;
        public uint Rank { get; set; } = 1;
        public uint Level { get; set; } = 0;
        public MissionAttributes Attributes { get; set; }
        public CosmicWeather Weather { get; set; }
        public uint StartTime { get; set; }
        public uint EndTime { get; set; }
        public uint ClassScore { get; set; } = 0;
        public uint CosmoCredit { get; set; } = 0;
        public uint LunarCredit { get; set; } = 0;
        public uint RewardItem { get; set; } = 0;
        public uint RewardItemAmount { get; set; } = 0;
        public uint DronebitReward { get; set; } = 0;
        public uint PreviousMissionId { get; set; } = new();
        public Dictionary<int, int> RelicXpInfo { get; set; } = new();
        public uint BronzeScore { get; set; } = 0;
        public uint SilverScore { get; set; } = 0;
        public uint GoldScore { get; set; } = 0;
        public uint TemporaryActionId { get; set; } = 0;
        public uint TemporaryActionCount { get; set; } = 0;
        public Status CompletionStatus { get; set; } = Status.None;
        public List<uint> SequenceMissions_Previous { get; set; } = new();
        public List<uint> SequenceMissions_Next { get; set; } = new();

        public bool IsProvisional => Attributes.HasFlag(MissionAttributes.ProvisionalWeather)
            || Attributes.HasFlag(MissionAttributes.ProvisionalSequential)
            || Attributes.HasFlag(MissionAttributes.ProvisionalTimed);

        public bool IsCritical => Attributes.HasFlag(MissionAttributes.Critical);
        public bool IsWeather => Attributes.HasFlag(MissionAttributes.ProvisionalWeather);
        public bool IsTimed => Attributes.HasFlag(MissionAttributes.ProvisionalTimed);
        public bool IsSequence => Attributes.HasFlag(MissionAttributes.ProvisionalSequential);
        public bool ARank => Rank is 5 or 4;
        public bool BRank => Rank is 3;
        public bool CRank => Rank is 2;
        public bool Drank => Rank is 1;

    }
    public static Dictionary<uint, CosmicInfo> SheetMissionDict = new();

    public class MissionInfo
    {
        public uint Id { get; set; } = 0;
        public bool Enabled => C.MissionConfig[Id].Enabled;
        public CosmicInfo SheetInfo => SheetMissionDict[Id];
    }
}
