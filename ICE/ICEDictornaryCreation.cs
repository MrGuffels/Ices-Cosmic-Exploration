using System.Collections.Generic;
using FFXIVClientStructs.FFXIV.Client.Game.WKS;
using static ICE.Utilities.CosmicHelper;
using static ICE.Enums.MissionAttributes;
using static ICE.Utilities.ExcelHelper;
using Lumina.Excel.Sheets;

namespace ICE;

public sealed partial class ICE
{
    public static unsafe void DictionaryCreation()
    {
        MoonRecipies = [];

        var wk = WKSManager.Instance();

        foreach (var entry in MoonMissionSheet)
        {
            Dictionary<ushort, int> crafts_Main = new();
            Dictionary<ushort, int> crafts_Pre = new();
            Dictionary<uint, int> gathering_Min = new();
            HashSet<uint> jobs = new();
            Dictionary<int, int> relicXp = new();

            uint keyId = entry.RowId;
            string LeveName = entry.Name.ToString();
            LeveName = LeveName.Replace("<nbsp>", " ");
            LeveName = LeveName.Replace("<->", "");

            if (LeveName == "")
                continue;

            jobs.Add(entry.ClassJobCategory[0].RowId - 1);
            var Job2 = entry.ClassJobCategory[1].RowId;
            if (Job2 != 0)
            {
                jobs.Add(Job2 - 1);
            }
            uint timeLimit = entry.MissionTime;
            uint silver = entry.SilverStarRequirement;
            uint gold = entry.GoldStarRequirement;
            HashSet<uint> previousMissionId = new() { entry.LockedBehind.RowId };

            uint timeAndWeather = entry.WKSMissionLotterySpecialCond.RowId;
            uint startTime = 0;
            uint endTime = 0;
            CosmicWeather weather = CosmicWeather.FairSkies;
            if (!CosmicHelper.WeatherSelection.Contains(timeAndWeather))
            {
                var timeSheet = Svc.Data.GetExcelSheet<WKSMissionLotterySpecialCond>().GetRow(timeAndWeather);
                startTime = timeSheet.Unknown1; // Start Time
                endTime = timeSheet.Unknown2; // End Time
            }
            else
            {
                weather = (CosmicWeather)(timeAndWeather - 12);
                // TODO: Go back and assign enums based on the value instead... or just directly give it a flag. Unsure. Feels dirty
            }

            uint rank = entry.LevelGroup;
            bool isCritical = entry.IsSpecialQuest;

            uint RecipeId = entry.WKSMissionRecipe.RowId;

            uint toDoValue = entry.MissionToDo[0].RowId;

            var todo = ToDoSheet.GetRow(toDoValue);
            uint missionText = todo.WKSMissionText.Value.RowId;
            var marker = MarkerSheet.GetRow(todo.Unknown13);
            uint territoryId = 1237; // TODO: Make this set the correct territoryId once new planets are added and we figure out where it is.

            int _x = marker.Unknown1 - 1024;
            int _y = marker.Unknown2 - 1024;
            int radius = marker.Unknown3;

            MissionAttributes attributes = missionText switch
            {
                99 or 101 or 145 => Craft | Limited,
                100 or 102 or 146 or 147 or 148 => Craft | Limited | Collectables,
                103 => Gather | Limited,
                104 => Gather | ScoreTimeRemaining,
                105 => Gather,
                106 => Gather | ScoreChains,
                107 => Gather | ScoreGatherersBoon,
                108 => Gather | ScoreChains | ScoreGatherersBoon,
                109 or 111 => Gather | Collectables,
                110 => Gather | ReducedItems | ScoreTimeRemaining,
                112 => Gather | ReducedItems,
                113 => Fish | ScoreVariety | ScoreTimeRemaining,
                114 or 115 => Fish | ScoreTimeRemaining,
                116 => Fish | Limited | ScoreVariety,
                117 => Fish | Limited | ScoreLargestSize,
                118 => Fish | Limited | Collectables,
                119 or 121 => Fish,
                120 => Fish | ScoreLargestSize,
                122 => Fish | Collectables,
                >= 123 and <= 134 => Craft | Gather, // Dual class
                >= 135 and <= 138 => Craft | Fish,  // Dual class
                139 => jobs.Contains(18) ? Fish : Gather, // Critical
                140 or 149 => Craft,
                _ => None
            };
            attributes |= isCritical ? Critical : None;
            attributes |= weather != CosmicWeather.FairSkies ? ProvisionalWeather : None;
            attributes |= (startTime != 0 || endTime != 0) ? ProvisionalTimed : None;
            attributes |= !previousMissionId.Contains(0) ? ProvisionalSequential : None;

            uint bronze = todo.Unknown2; // Bronze score for Score missions
            attributes |= bronze > 0 ? ScoreScore : None;

            if (CrafterJobList.Overlaps(jobs))
            {
                bool preCraftsbool = false;

                var toDoRow = ToDoSheet.GetRow(toDoValue);
                if (isCritical) // Criticals are sus
                {
                    UInt16 item1Amount = 1; // It's a pass/fail progress, you need to go till you are full on score
                    var item1RecipeRow = RecipeSheet.Where(e => e.RowId == MoonRecipeSheet.GetRow(entry.WKSMissionRecipe.RowId).Recipe[0].Value.RowId).First();
                    var item1Id = item1RecipeRow.ItemResult.RowId;
                    var item1Name = ItemSheet.GetRow(item1Id).Name.ToString();
                    var craftingType = item1RecipeRow.CraftType.Value.RowId;
                    IceLogging.Verbose($"Recipe Row ID: {item1RecipeRow.RowId} | for item: {item1Id} | {item1Name}");
                    var item1RecipeId = item1RecipeRow.RowId;
                    crafts_Main.Add(((ushort)item1RecipeId), item1Amount);
                }
                if (toDoRow.RequiredItem[0].RowId != 0 && !isCritical) // shouldn't be 0, 1st item entry
                {
                    var item1Amount = toDoRow.RequiredItemQuantity[0]; // unknown 6
                    var item1Id = MoonItemInfoSheet.GetRow(toDoRow.RequiredItem[0].RowId).Item.RowId;
                    var item1Name = ItemSheet.GetRow(item1Id).Name.ToString();
                    var item1RecipeRow = RecipeSheet.Where(e => e.ItemResult.RowId == item1Id)
                                                    .Where(e => e.RowId == MoonRecipeSheet.GetRow(RecipeId).Recipe[0].Value.RowId ||
                                                                e.RowId == MoonRecipeSheet.GetRow(RecipeId).Recipe[1].Value.RowId ||
                                                                e.RowId == MoonRecipeSheet.GetRow(RecipeId).Recipe[2].Value.RowId)
                                                    .First();
                    var craftingType = item1RecipeRow.CraftType.Value.RowId;
                    IceLogging.Verbose($"Recipe Row ID: {item1RecipeRow.RowId} | for item: {item1Id} | {item1Name}");
                    for (var i = 0; i <= 3; i++)
                    {
                        var subitem = item1RecipeRow.Ingredient[i].Value.RowId;
                        if (subitem != 0)
                        {
                            IceLogging.Verbose($"subItemId: {subitem} slot [{i}]");
                            var subitemRecipe = RecipeSheet.Where(x => x.ItemResult.RowId == subitem)
                                                           .Where(e => e.RowId == MoonRecipeSheet.GetRow(RecipeId).Recipe[0].Value.RowId ||
                                                                  e.RowId == MoonRecipeSheet.GetRow(RecipeId).Recipe[1].Value.RowId ||
                                                                  e.RowId == MoonRecipeSheet.GetRow(RecipeId).Recipe[2].Value.RowId)
                                                           .FirstOrDefault();
                            if (subitemRecipe.RowId != 0)
                            {
                                var subItemAmount = item1RecipeRow.AmountIngredient[i].ToInt();
                                subItemAmount = subItemAmount * item1Amount;
                                crafts_Pre.Add(((ushort)subitemRecipe.RowId), subItemAmount);
                                preCraftsbool = true;
                            }
                        }
                    }
                    var item1RecipeId = item1RecipeRow.RowId;
                    crafts_Main.Add(((ushort)item1RecipeId), item1Amount);
                }
                if (toDoRow.RequiredItem[1].RowId != 0) // 2nd item entry
                {
                    var item2Amount = toDoRow.RequiredItemQuantity[1];
                    var item2Id = MoonItemInfoSheet.GetRow(toDoRow.RequiredItem[1].RowId).Item.RowId;
                    var item2Name = ItemSheet.GetRow(item2Id).Name.ToString();

                    var item2RecipeRow = RecipeSheet.Where(e => e.ItemResult.RowId == item2Id)
                                                    .Where(e => e.RowId == MoonRecipeSheet.GetRow(RecipeId).Recipe[0].Value.RowId ||
                                                           e.RowId == MoonRecipeSheet.GetRow(RecipeId).Recipe[1].Value.RowId ||
                                                           e.RowId == MoonRecipeSheet.GetRow(RecipeId).Recipe[2].Value.RowId)
                                                    .First();
                    IceLogging.Verbose($"Recipe Row ID: {item2RecipeRow.RowId} | for item: {item2Id} | {item2Name}");
                    for (var i = 0; i <= 3; i++)
                    {
                        var subitem = item2RecipeRow.Ingredient[i].Value.RowId;
                        if (subitem != 0)
                        {
                            IceLogging.Verbose($"subItemId: {subitem} slot [{i}]");
                            var subitemRecipe = RecipeSheet.Where(e => e.ItemResult.RowId == item2Id)
                                                           .Where(e => e.RowId == MoonRecipeSheet.GetRow(RecipeId).Recipe[0].Value.RowId ||
                                                                  e.RowId == MoonRecipeSheet.GetRow(RecipeId).Recipe[1].Value.RowId ||
                                                                  e.RowId == MoonRecipeSheet.GetRow(RecipeId).Recipe[2].Value.RowId)
                                                           .First();
                            if (subitemRecipe.RowId != 0)
                            {
                                var subItemAmount = item2RecipeRow.AmountIngredient[i].ToInt();
                                subItemAmount = subItemAmount * item2Amount;
                                crafts_Pre.Add(((ushort)subitemRecipe.RowId), subItemAmount);
                                preCraftsbool = true;
                            }
                        }
                    }
                    var item2RecipeId = item2RecipeRow.RowId;
                    crafts_Main.Add(((ushort)item2RecipeId), item2Amount);
                }
                if (toDoRow.RequiredItem[2].RowId != 0) // 3rd item entry
                {
                    var item3Amount = toDoRow.RequiredItemQuantity[2];
                    var item3Id = MoonItemInfoSheet.GetRow(toDoRow.RequiredItem[2].RowId).Item.RowId;
                    var item3Name = ItemSheet.GetRow(item3Id).Name.ToString();

                    var item3RecipeRow = RecipeSheet.Where(e => e.ItemResult.RowId == item3Id)
                                                    .Where(e => e.RowId == MoonRecipeSheet.GetRow(RecipeId).Recipe[0].Value.RowId ||
                                                           e.RowId == MoonRecipeSheet.GetRow(RecipeId).Recipe[1].Value.RowId ||
                                                           e.RowId == MoonRecipeSheet.GetRow(RecipeId).Recipe[2].Value.RowId)
                                                    .First();
                    IceLogging.Verbose($"Recipe Row ID: {item3RecipeRow.RowId} | for item: {item3Id} | {item3Name}");
                    for (var i = 0; i <= 3; i++)
                    {
                        var subitem = item3RecipeRow.Ingredient[i].Value.RowId;
                        if (subitem != 0)
                        {
                            IceLogging.Verbose($"subItemId: {subitem} slot [{i}]");
                            var subitemRecipe = RecipeSheet.FirstOrDefault(x => x.ItemResult.RowId == subitem);
                            if (subitemRecipe.RowId != 0)
                            {
                                var subItemAmount = item3RecipeRow.AmountIngredient[i].ToInt();
                                subItemAmount = subItemAmount * item3Amount;
                                crafts_Pre.Add(((ushort)subitemRecipe.RowId), subItemAmount);
                                preCraftsbool = true;
                            }
                        }
                    }
                    var item3RecipeId = item3RecipeRow.RowId;
                    crafts_Main.Add(((ushort)item3RecipeId), item3Amount);
                }

                if (preCraftsbool)
                {
                    foreach (var preItem in crafts_Pre)
                    {
                        if (crafts_Main.ContainsKey(preItem.Key))
                            crafts_Pre.Remove(preItem.Key);
                    }

                    if (crafts_Pre.Count == 0)
                    {
                        preCraftsbool = false;
                    }
                }

            }

            if (GatheringJobList.Overlaps(jobs))
            {
                var todoRow = ToDoSheet.GetRow(toDoValue);

                if (todoRow.RequiredItem[0].RowId != 0) // First item in the gathering list. Shouldn't be 0...
                {
                    var minAmount = todoRow.RequiredItemQuantity[0].ToInt();
                    var itemInfoId = MoonItemInfoSheet.GetRow(todoRow.RequiredItem[0].RowId).Item.RowId;
                    if (!gathering_Min.ContainsKey(itemInfoId))
                    {
                        gathering_Min.Add(itemInfoId, minAmount);
                    }
                }
                if (todoRow.RequiredItem[1].RowId != 0) // First item in the gathering list. Shouldn't be 0...
                {
                    var minAmount = todoRow.RequiredItemQuantity[1].ToInt();
                    var itemInfoId = MoonItemInfoSheet.GetRow(todoRow.RequiredItem[1].RowId).Item.RowId;
                    if (!gathering_Min.ContainsKey(itemInfoId))
                    {
                        gathering_Min.Add(itemInfoId, minAmount);
                    }
                }
                if (todoRow.RequiredItem[2].RowId != 0) // First item in the gathering list. Shouldn't be 0...
                {
                    var minAmount = todoRow.RequiredItemQuantity[2].ToInt();
                    var itemInfoId = MoonItemInfoSheet.GetRow(todoRow.RequiredItem[2].RowId).Item.RowId;
                    if (!gathering_Min.ContainsKey(itemInfoId))
                    {
                        gathering_Min.Add(itemInfoId, minAmount);
                    }
                }
            }

            if (GatheringJobList.Overlaps(jobs) && CrafterJobList.Overlaps(jobs))
            {
                var MissionRecipe = entry.WKSMissionRecipe.RowId;
                var DualRecipeId = MoonRecipeSheet.GetRow(MissionRecipe).Recipe[0].Value.RowId;
                var Recipe = RecipeSheet.GetRow(DualRecipeId);
                var MainItem = Recipe.ItemResult.Value.RowId;
                var GatherItem = Recipe.Ingredient[0].Value.RowId;
                var GatherAmount = Recipe.AmountIngredient[0].ToInt();

                crafts_Main.Add((ushort)DualRecipeId, 1);
                gathering_Min.Add(GatherItem, GatherAmount);
            }

            // Col 3 -> Cosmocredits - Unknown 0
            // Col 4 -> Lunar Credits - Unknown 1
            // Col 7 ->  Lv. 1 Type - Unknown 12
            // Col 8 ->  Lv. 1 Exp - Unknown 2
            // Col 10 -> Lv. 2 Type - Unknown 13
            // Col 11 -> Lv. 2 Exp - Unknown 3
            // Col 13 -> Lv. 3 Type - Unknown 14
            // Col 14 -> Lv. 3 Exp - Unknown 4

            // Something to note here, a mission can only have a max of 3 types of XP at a time.
            // Which is why there's only 3 entries.

            uint Cosmo = ExpSheet.GetRow(keyId).Unknown0;
            uint Lunar = ExpSheet.GetRow(keyId).Unknown1;

            if (ExpSheet.GetRow(keyId).Unknown2 != 0)
            {
                var xp1Kind = ExpSheet.GetRow(keyId).Unknown12;
                var xp1Amount = ExpSheet.GetRow(keyId).Unknown2;
                relicXp[xp1Kind] = xp1Amount;
            }
            if (ExpSheet.GetRow(keyId).Unknown3 != 0)
            {
                var xp2Kind = ExpSheet.GetRow(keyId).Unknown13;
                var xp2Amount = ExpSheet.GetRow(keyId).Unknown3;
                relicXp[xp2Kind] = xp2Amount;
            }
            if (ExpSheet.GetRow(keyId).Unknown4 != 0)
            {
                var xp3Kind = ExpSheet.GetRow(keyId).Unknown14;
                var xp3Amount = ExpSheet.GetRow(keyId).Unknown4;
                relicXp[xp3Kind] = xp3Amount;
            }

            if (!SheetMissionDict.ContainsKey(keyId))
            {
                SheetMissionDict[keyId] = new CosmicInfo()
                {
                    Name = LeveName,
                    Jobs = jobs,
                    ToDoId = toDoValue,
                    Rank = rank,
                    Attributes = attributes,
                    Weather = weather,
                    StartTime = startTime,
                    EndTime = endTime,
                    CosmoCredit = Cosmo,
                    LunarCredit = Lunar,
                    PreviousMissions = previousMissionId,
                    RelicXpInfo = relicXp,
                    BronzeScore = bronze,
                    SilverScore = silver,
                    GoldScore = gold,

                    MapPosition = new Vector2(_x, _y),
                    Radius = radius,
                    TerritoryId = territoryId,
                    MarkerId = marker.RowId,

                    Gathering_Min = gathering_Min,

                    Crafts_Main = crafts_Main,
                    Crafts_Pre = crafts_Pre,
                };
            }
        }

        foreach (var Icon in LeveAssignmentSheet)
        {
            var iconId = Icon.RowId;

            if (iconId is 2 or 3 or 4)
            {
                iconId += 14;
            }
            else if (iconId > 4 && iconId < 13)
            {
                iconId += 3;
            }
            else
                continue;

            if (Icon.Name != "" && Icon.Icon is { } jobicon)
            {
                if (Svc.Texture.TryGetFromGameIcon(jobicon, out var texture))
                {
                    JobIconDict.TryAdd(iconId, texture);
                }
            }
        }

        for (int i = 0; i < GreyIconList.Count; i++)
        {
            var slot = i + 8;
            var iconId = GreyIconList[i];

            if (Svc.Texture.TryGetFromGameIcon(iconId, out var texture))
            {
                GreyTexture.TryAdd((uint)slot, texture);
            }
        }

        foreach (var entry in SheetMissionDict)
        {
            var id = entry.Key;
            if (MissionScoreDict.TryGetValue(id, out var missionEntry))
            {
                entry.Value.ClassScore = MissionScoreDict[id];
            }
            else
            {
                entry.Value.ClassScore = 0;
            }
        }

        foreach (var item in MoonItemInfoSheet)
        {
            var itemId = item.Item.RowId;
            if (itemId == 0) continue;
            string itemName = ItemSheet.GetRow(itemId).Name.ToString();
            var type = item.WKSItemSubCategory.RowId;
#if DEBUG
            IceLogging.Debug($"RowID: {item.RowId} | ID: {itemId} | Name: {itemName}");
#endif

            if (CosmicHelper.GatheringItems.TryGetValue(itemName, out var itemEntry))
            {
                itemEntry.itemIds.Add(itemId);
            }
            else
            {
#if DEBUG
                IceLogging.Debug($"Adding a new entry: {itemName}");
#endif

                CosmicHelper.GatheringItems[itemName] = new()
                {
                    Type = item.WKSItemSubCategory.RowId,
                    itemIds = new HashSet<uint> { itemId },
                };
            }
        }

        UpdateSheetMissionDict();
    }
    private static MissionType GetMissionType(CosmicInfo mission)
    {
        if (mission.Attributes.HasFlag(Critical))
        {
            return MissionType.Critical;
        }
        else if (mission.Attributes.HasFlag(ProvisionalTimed))
        {
            return MissionType.Timed;
        }
        else if (mission.Attributes.HasFlag(ProvisionalWeather))
        {
            return MissionType.Weather;
        }
        else if (mission.Attributes.HasFlag(ProvisionalSequential))
        {
            return MissionType.Sequential;
        }

        return MissionType.Standard;
    }

    public static void UpdateSheetMissionDict()
    {
        foreach (var kvp in Dict_CosmicMissions)
        {
            uint missionId = kvp.Key;
            CosmicInfo sourceInfo = kvp.Value;

            // Check if the mission exists in SheetMissionDict
            if (SheetMissionDict.ContainsKey(missionId))
            {
                CosmicInfo targetInfo = SheetMissionDict[missionId];

                // Update all properties from Dict_CosmicMissions to SheetMissionDict

                // Fishing Specific
                targetInfo.RequiredFish = new Dictionary<string, HashSet<uint>>(sourceInfo.RequiredFish);
                targetInfo.FishCountRequired = sourceInfo.FishCountRequired;
                targetInfo.FishingBait = new Dictionary<string, HashSet<uint>>(sourceInfo.FishingBait);

                // Crafter Specific
                targetInfo.Crafts_Main = new Dictionary<ushort, int>(sourceInfo.Crafts_Main);
                targetInfo.Crafts_Pre = new Dictionary<ushort, int>(sourceInfo.Crafts_Pre);

                // BTN | MIN Specific
                targetInfo.Gathering_Min = new Dictionary<uint, int>(sourceInfo.Gathering_Min);

                // Map Related
                targetInfo.MapPosition = sourceInfo.MapPosition;
                targetInfo.Radius = sourceInfo.Radius;
                targetInfo.TerritoryId = sourceInfo.TerritoryId;
                targetInfo.MarkerId = sourceInfo.MarkerId;

                // Universal Info
                // targetInfo.Name = sourceInfo.Name; // Don't want this to get overwrote. This should always just be what the sheet has it as
                targetInfo.Jobs = new HashSet<uint>(sourceInfo.Jobs);
                targetInfo.ToDoId = sourceInfo.ToDoId;
                // targetInfo.Rank = sourceInfo.Rank; // Also something we don't really care about editing. This should be static from the sheet
                targetInfo.Attributes = sourceInfo.Attributes;
                targetInfo.Weather = sourceInfo.Weather;
                targetInfo.StartTime = sourceInfo.StartTime;
                targetInfo.EndTime = sourceInfo.EndTime;
                targetInfo.ClassScore = sourceInfo.ClassScore;
                targetInfo.CosmoCredit = sourceInfo.CosmoCredit;
                targetInfo.LunarCredit = sourceInfo.LunarCredit;
                targetInfo.PreviousMissions = new HashSet<uint>(sourceInfo.PreviousMissions);
                targetInfo.RelicXpInfo = new Dictionary<int, int>(sourceInfo.RelicXpInfo);
                targetInfo.BronzeScore = sourceInfo.BronzeScore;
                targetInfo.SilverScore = sourceInfo.SilverScore;
                targetInfo.GoldScore = sourceInfo.GoldScore;
            }
            else
            {
                // If the mission doesn't exist in SheetMissionDict, add it
                // this... shouldn't be possible. But better ot be safer than sorry
                var newInfo = new CosmicInfo
                {
                    // Fishing Specific
                    RequiredFish = new Dictionary<string, HashSet<uint>>(sourceInfo.RequiredFish),
                    FishCountRequired = sourceInfo.FishCountRequired,
                    FishingBait = new Dictionary<string, HashSet<uint>>(sourceInfo.FishingBait),

                    // Crafter Specific
                    Crafts_Main = new Dictionary<ushort, int>(sourceInfo.Crafts_Main),
                    Crafts_Pre = new Dictionary<ushort, int>(sourceInfo.Crafts_Pre),

                    // BTN | MIN Specific
                    Gathering_Min = new Dictionary<uint, int>(sourceInfo.Gathering_Min),

                    // Map Related
                    MapPosition = sourceInfo.MapPosition,
                    Radius = sourceInfo.Radius,
                    TerritoryId = sourceInfo.TerritoryId,
                    MarkerId = sourceInfo.MarkerId,

                    // Universal Info
                    Name = sourceInfo.Name,
                    Jobs = new HashSet<uint>(sourceInfo.Jobs),
                    ToDoId = sourceInfo.ToDoId,
                    Rank = sourceInfo.Rank,
                    Attributes = sourceInfo.Attributes,
                    Weather = sourceInfo.Weather,
                    StartTime = sourceInfo.StartTime,
                    EndTime = sourceInfo.EndTime,
                    ClassScore = sourceInfo.ClassScore,
                    CosmoCredit = sourceInfo.CosmoCredit,
                    LunarCredit = sourceInfo.LunarCredit,
                    PreviousMissions = new HashSet<uint>(sourceInfo.PreviousMissions),
                    RelicXpInfo = new Dictionary<int, int>(sourceInfo.RelicXpInfo),
                    BronzeScore = sourceInfo.BronzeScore,
                    SilverScore = sourceInfo.SilverScore,
                    GoldScore = sourceInfo.GoldScore
                };

                SheetMissionDict[missionId] = newInfo;
            }
        }
    }
}
