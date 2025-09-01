using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.DebugWindowTabs
{
    internal class MoonRecipeTable
    {
        private static string RecipeTableSearchText = "";

        public static unsafe void Draw()
        {
            ImGui.SetNextItemWidth(250);
            ImGui.InputText("Search by Name", ref RecipeTableSearchText, 100);

            if (ImGui.BeginTable("Mission Info List", 9, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.Resizable))
            {
                ImGui.TableSetupColumn("Key", ImGuiTableColumnFlags.WidthFixed, 100);
                ImGui.TableSetupColumn("Mission Name", ImGuiTableColumnFlags.WidthFixed, 100);
                ImGui.TableSetupColumn("Bool", ImGuiTableColumnFlags.WidthFixed, 100);
                ImGui.TableSetupColumn("Pre-Craft 1", ImGuiTableColumnFlags.WidthFixed, 100);
                ImGui.TableSetupColumn("Pre-Craft 2", ImGuiTableColumnFlags.WidthFixed, 100);
                ImGui.TableSetupColumn("Pre-Craft 3", ImGuiTableColumnFlags.WidthFixed, 100);
                ImGui.TableSetupColumn("MainCraft 1", ImGuiTableColumnFlags.WidthFixed, 100);
                ImGui.TableSetupColumn("MainCraft 2", ImGuiTableColumnFlags.WidthFixed, 100);
                ImGui.TableSetupColumn("MainCraft 3", ImGuiTableColumnFlags.WidthFixed, 100);

                ImGui.TableHeadersRow();

                var recipeList = CosmicHelper.MoonRecipies.Where(recipe => CosmicHelper.SheetMissionDict.First(x => x.Key == recipe.Key).Value.Name.ToLower().Contains(RecipeTableSearchText.ToLower()));
                foreach (var entry in recipeList)
                {
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);

                    ImGui.Text($"{entry.Key}");

                    ImGui.TableNextColumn();
                    var missionName = CosmicHelper.SheetMissionDict.First(x => x.Key == entry.Key).Value.Name;
                    ImGui.Text($"{missionName}");

                    ImGui.TableNextColumn();
                    ImGui.Text($"{entry.Value.PreCrafts}");

                    ImGui.TableNextColumn();
                    if (entry.Value.PreCrafts == true)
                    {
                        foreach (var sub in entry.Value.PreCraftDict)
                        {
                            ImGui.Text($"Recipe: {sub.Key} | Amount: {sub.Value}");
                            ImGui.TableNextColumn();
                        }
                    }

                    ImGui.TableSetColumnIndex(5);
                    foreach (var main in entry.Value.MainCraftsDict)
                    {
                        ImGui.TableNextColumn();
                        ImGui.Text($"Recipe: {main.Key} | Amount: {main.Value}");
                    }
                }

                ImGui.EndTable();
            }
        }
    }
}
