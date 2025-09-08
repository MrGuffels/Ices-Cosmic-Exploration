using ECommons;
using FFXIVClientStructs.FFXIV.Client.Game.WKS;
using Lumina.Excel.Sheets;
using System.Collections.Generic;
using static ICE.Enums.MissionAttributes;
using static ICE.Utilities.CosmicHelper;
using static ICE.Utilities.ExcelHelper;

namespace ICE;

public sealed partial class ICE
{
    public static unsafe void DictionaryCreation()
    {
        MoonRecipies = [];

        var wk = WKSManager.Instance();

        foreach (var entry in MoonMissionSheet)
        {
            Dictionary<ushort, CosmicHelper.CraftingInfo> crafts_Main = new();
            Dictionary<ushort, CosmicHelper.CraftingInfo> crafts_Pre = new();
            Dictionary<uint, int> gathering_Min = new();
            HashSet<uint> jobs = new();
            Dictionary<int, int> relicXp = new();

            uint keyId = entry.RowId;
            string missionName = entry.Name.ToString();
            missionName = missionName.Replace("<nbsp>", " ");
            missionName = missionName.Replace("<->", "");

            if (missionName == "")
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

            var wksToDo = ToDoSheet.GetRow(toDoValue);
            uint missionText = wksToDo.WKSMissionText.Value.RowId;
            var marker = MarkerSheet.GetRow(wksToDo.Unknown13);
            uint territoryId = 1237; 
            if (keyId < 545)
            {
                territoryId = 1237;
            }
            else
            {
                territoryId = 1291;
            }
            // TODO: Make this set the correct territoryId once new planets are added and we figure out where it is.

            int _x = marker.Unknown1 - 1024;
            int _y = marker.Unknown2 - 1024;
            int radius = marker.Unknown3;

            MissionAttributes attributes = missionText switch
            {
                (>= 99 and <= 102) or 140 or (>= 145 and <= 149) or 235 or 237 => Craft,
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
                _ => None
            };
            attributes |= isCritical ? Critical : None;
            attributes |= weather != CosmicWeather.FairSkies ? ProvisionalWeather : None;
            attributes |= (startTime != 0 || endTime != 0) ? ProvisionalTimed : None;
            attributes |= !previousMissionId.Contains(0) ? ProvisionalSequential : None;

            // - - - HEY. BRONZE SCORE IS KEPT HERE - - - //
            uint bronze = wksToDo.Unknown2; // Bronze score for Score missions
            attributes |= bronze > 0 ? ScoreScore : None;

            if (CrafterJobList.Overlaps(jobs))
            {
                var wksRecipeRow = wksMissionRecipe.GetRow(RecipeId);

                if (isCritical) // Criticals are sus
                {
                    var itemAmount = 3; // It's a pass/fail progress, you need to go till you are full on score
                                            // Realistically need 3 items. So just going to hard code this as that for now. Until square decides to change the formula haha.
                    var missionRecipeRow = RecipeSheet.Where(e => e.RowId == wksRecipeRow.Recipe[0].RowId).First();
                    var itemId = missionRecipeRow.ItemResult.RowId;
                    var itemName = ItemSheet.GetRow(itemId).Name.ToString();
                    var craftingType = missionRecipeRow.CraftType.Value.RowId;
                    IceLogging.Verbose($"Recipe Row ID: {missionRecipeRow.RowId} | for item: {itemId} | {itemName}");
                    var item1RecipeId = missionRecipeRow.RowId;
                    crafts_Main[(ushort)item1RecipeId] = new CraftingInfo()
                    {
                        ItemId = itemId,
                        Amount = itemAmount,
                    };
                }
                else
                {
                    // Reason for the following code is this:
                    // If it's a pre-craft, it should be further down the list, which means adding it first to the pre-crafts
                    // If it's required, then all of them SHOULD... be required. *-shrugs-*
                    for (int i = 2; i >= 0; i--)
                    {
                        var recipeId = (ushort)wksRecipeRow.Recipe[i].Value.RowId;

                        IceLogging.Info($"MissionID: {keyId} | ToDoId: {toDoValue} | recipeId: {recipeId} @ slot {i}");

                        if (recipeId != 0)
                        {
                            var recipeRow = RecipeSheet.GetRow(recipeId);

                            var itemId = recipeRow.ItemResult.RowId;
                            var amountNeeded = wksToDo.RequiredItemQuantity[i];

                            // Appears to be a valid recipeId, time to grab the infomation from the other sheets.
                            if (amountNeeded == 0)
                            {
                                // Item isn't a required item, but is a pre-craft. Going to set the default of 1 for now, then change post.
                                IceLogging.Info($"Adding Pre-Craft: {itemId}");
                                crafts_Pre[recipeId] = new CraftingInfo()
                                {
                                    Amount = 1,
                                    ItemId = itemId,
                                };
                            }
                            else
                            {
                                // Item count was more than 0. Which means THEORETICALLY... it should be a main item. 
                                crafts_Main[recipeId] = new CraftingInfo()
                                {
                                    Amount = amountNeeded,
                                    ItemId = itemId,
                                };

                                var recipeMaterialId = recipeRow.AmountIngredient[0];

                                // Checking to see if the material exist in the crafts_pre. If so, then updating the value
                                var preCraftId = crafts_Pre.FirstOrDefault(kvp => kvp.Value.ItemId == recipeMaterialId).Key;
                                if (preCraftId != 0)
                                {
                                    crafts_Pre[preCraftId] = new CraftingInfo()
                                    {
                                        Amount = amountNeeded,
                                        ItemId = itemId,
                                    };
                                }
                            }
                        }
                    }

                    // This is just a general sanity check in itself for mission where there isn't a required item count, but moreso just needs score. 
                    if (crafts_Main.Count == 0)
                    {
                        // These are missions that don't require an item, but for the sanity check of it all, going to just have it be 1. 
                        // Still need to hardcode the bronze scores in though

                        foreach (var item in crafts_Pre)
                        {
                            item.Value.Amount = 1;
                            crafts_Main.Add(item);
                            crafts_Pre.Remove(item);
                        }
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
                    Name = missionName,
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

        // UpdateSheetMissionDict();
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
}
