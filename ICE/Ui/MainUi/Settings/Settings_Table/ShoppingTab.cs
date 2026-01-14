using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Utility;
using Lumina.Excel.Sheets;
using static ICE.ConfigFiles.Config;

namespace ICE.Ui.MainUi.Settings.Settings_Table
{
    internal class ShoppingTab
    {
        private static string ItemSearch = string.Empty;

        public static unsafe void Draw()
        {
            bool BuyItems = C.BuyItems;

            if (ImGui.Checkbox("Buy Items", ref BuyItems))
            {
                C.BuyItems = BuyItems;
                C.StopOnceHitCosmoCredits = false;
                C.Save();
            }
            ImGui.SameLine();
            ImGuiEx.Icon(FontAwesomeIcon.QuestionCircle);
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.Text("This is your personalized shopping list that you can create that it will run when you hit a certain amount of credits.");
                ImGui.Text("Here's what each of the following does:");
                ImGui.BulletText("Keep: Will buy up to that many items to make sure that you have in your inventory. This count doesn't go down between runs.\n" +
                                 "Useful for things like cordials where you want to always have a certain amount on hand");
                ImGui.BulletText("Buy: Will buy X amount of those items, as it buys it from the vendor, the number will decrease until it hits 0.\n" +
                                 "Good for one off buys, or something that you only need a particular amount of");
                ImGui.BulletText("Keep Buying: Once the other 2 have been met (Keep/Buy), it will constantly buy this item if it has the credits to do so.\n" +
                                 "This can only be set to 1 item, and gererally used for things you want to just spend your credits on");
                ImGui.EndTooltip();
            }
            ImGui.NewLine();

            int buyAtAmount = C.CosmoBuyAtAmount;
            ImGui.SetNextItemWidth(150);
            if (ImGui.InputInt("Go buy items when you reach", ref buyAtAmount, 1))
            {
                if (buyAtAmount < 0)
                    buyAtAmount = 0;
                if (buyAtAmount > 30000)
                    buyAtAmount = 30000;
                C.CosmoBuyAtAmount = buyAtAmount;
                C.Save();
            }

            CheckConfigState();
            if (Task_BuyCosmoItems.CanPurchaseAnyItem())
            {
                ImGui.Text("You can buy cosmocredit items from the list!");
            }
            else
            {
                ImGui.Text("You can't buy any items with your current credit value/items (tis fine, this just a test)");
            }

            if (ImGui.Button("Add Items to List"))
            {
                ImGui.OpenPopup("CosmocreditMateriaPopup");
            }

            ImGui.SetNextWindowSize(new Vector2(400, 0), ImGuiCond.Appearing);

            if (ImGui.BeginPopup("CosmocreditMateriaPopup"))
            {
                ImGui.SetNextItemWidth(380);
                ImGui.InputText("##Item Search", ref ItemSearch, 256);

                ImGui.Spacing();

                // Remove BeginChild and use table scrolling instead
                if (ImGui.BeginTable("Cosmo Materia Shop", 2, ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.ScrollY | ImGuiTableFlags.RowBg, new Vector2(0, 250)))
                {
                    ImGui.TableSetupColumn("Icons", ImGuiTableColumnFlags.WidthFixed, 20);
                    ImGui.TableSetupColumn("Names", ImGuiTableColumnFlags.WidthStretch);

                    foreach (var item in Shop_Cosmocredits.CosmocreditShop)
                    {
                        var id = item.Key;
                        if (Svc.Data.GetExcelSheet<Item>().TryGetRow(id, out var itemInfo))
                        {
                            var name = itemInfo.Name.ToString();

                            if (!ItemSearch.IsNullOrWhitespace() && !name.ToLower().Contains(ItemSearch.ToLower()))
                            {
                                continue;
                            }

                            ImGui.TableNextRow();
                            ImGui.TableSetColumnIndex(0);
                            ImGui.PushID(id);
                            if (itemInfo.Icon is { } itemIcon && Svc.Texture.TryGetFromGameIcon((int)itemIcon, out var texture))
                            {
                                ImGui.Image(texture.GetWrapOrEmpty().Handle, new Vector2(20, 20));
                            }
                            ImGui.TableNextColumn();
                            ImGui.Text($"{itemInfo.Name}");
                            if (ImGui.IsItemHovered() && ImGui.IsItemClicked(ImGuiMouseButton.Left))
                            {
                                AddItem(id);
                                C.Save();
                            }
                            ImGui.PopID();
                        }
                    }
                    ImGui.EndTable();
                }

                ImGui.EndPopup();
            }

            ImGui.Text($"Order Count {C.CosmoShoppingOrder.Count}");

            if (ImGui.BeginTable("Current Shopping List", 10, ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.RowBg | ImGuiTableFlags.Borders))
            {
                ImGui.TableSetupColumn("Up");
                ImGui.TableSetupColumn("Down");
                ImGui.TableSetupColumn("Name");
                ImGui.TableSetupColumn("Have");
                ImGui.TableSetupColumn("Cost");
                ImGui.TableSetupColumn("Kind");
                ImGui.TableSetupColumn("Keep");
                ImGui.TableSetupColumn("Buy");
                ImGui.TableSetupColumn("Keep Buying");
                ImGui.TableSetupColumn("Remove");

                ImGui.TableHeadersRow();

                for (int i = 0; i < C.CosmoShoppingOrder.Count; i++)
                {
                    uint itemId = C.CosmoShoppingOrder[i];
                    var setting = C.CosmoShopping[itemId];
                    var itemInfo = Svc.Data.GetExcelSheet<Item>().GetRow(itemId);

                    ImGui.TableNextRow();

                    ImGui.PushID(itemId);

                    ImGui.TableSetColumnIndex(0);
                    using (ImRaii.Disabled(i == 0))
                    {
                        if (ImGuiEx.IconButton(FontAwesomeIcon.ArrowUp, $"##drag_{itemId}"))
                        {
                            MoveItemUp(itemId);
                            C.Save();
                        }
                    }

                    ImGui.TableNextColumn();
                    if (ImGuiEx.IconButton(FontAwesomeIcon.ArrowDown, $"##drag_{itemId}"))
                    {
                        MoveItemDown(itemId);
                        C.Save();
                    }

                    // Name
                    ImGui.TableNextColumn();
                    if (itemInfo.Icon is { } itemIcon && Svc.Texture.TryGetFromGameIcon((int)itemIcon, out var texture))
                    {
                        ImGui.Image(texture.GetWrapOrEmpty().Handle, new Vector2(24, 24));
                        ImGui.SameLine();
                    }
                    ImGui.Text($"{itemInfo.Name}");

                    ImGui.TableNextColumn();
                    PlayerHelper.GetItemCount(itemId, out var count);
                    ImGui.Text($"{count}");

                    // Cost
                    ImGui.TableNextColumn();
                    if (Shop_Cosmocredits.CosmocreditShop.TryGetValue(itemId, out var shopInfo))
                    {
                        ImGui.Text($"{shopInfo.Cost}");
                    }

                    // Kind (you can add logic for this)
                    ImGui.TableNextColumn();
                    ImGui.Text("Material"); // Replace with actual kind logic

                    // Keep Amount
                    ImGui.TableNextColumn();
                    ImGui.SetNextItemWidth(80);
                    var keepAmount = setting.KeepAmount;
                    if (ImGui.InputInt($"##keep_{itemId}", ref keepAmount))
                    {
                        setting.KeepAmount = keepAmount;
                        C.SaveDebounced();
                    }

                    // Buy Amount
                    ImGui.TableNextColumn();
                    ImGui.SetNextItemWidth(80);
                    var buyAmount = setting.BuyAmount;
                    if (ImGui.InputInt($"##buy_{itemId}", ref buyAmount))
                    {
                        setting.BuyAmount = buyAmount;
                        C.SaveDebounced();
                    }

                    // Keep Buying
                    ImGui.TableNextColumn();
                    var keepBuying = setting.KeepBuying;
                    if (ImGui.Checkbox($"##keepbuying_{itemId}", ref keepBuying))
                    {
                        foreach (var enabled in C.CosmoShopping)
                        {
                            enabled.Value.KeepBuying = false;
                        }

                        setting.KeepBuying = keepBuying;
                        C.Save();
                    }

                    // Remove Button
                    ImGui.TableNextColumn();
                    if (ImGuiEx.IconButton(Dalamud.Interface.FontAwesomeIcon.Trash, "##Remove Item"))
                    {
                        RemoveItem(itemId);
                        C.Save();
                    }

                    ImGui.PopID();
                }

                ImGui.EndTable();
            }
        }
        private static void AddItem(uint itemId)
        {
            if (C.CosmoShopping.ContainsKey(itemId))
                return;


            C.CosmoShopping[itemId] = new CosmoShoppingList();
            C.CosmoShoppingOrder.Add(itemId);
        }

        private static void RemoveItem(uint itemId)
        {
            C.CosmoShopping.Remove(itemId);
            C.CosmoShoppingOrder.Remove(itemId);
        }

        public static void MoveItemUp(uint itemId)
        {
            int index = C.CosmoShoppingOrder.IndexOf(itemId);
            if (index > 0)
            {
                C.CosmoShoppingOrder.RemoveAt(index);
                C.CosmoShoppingOrder.Insert(index - 1, itemId);
            }
        }

        public static void MoveItemDown(uint itemId)
        {
            int index = C.CosmoShoppingOrder.IndexOf(itemId);
            if (index >= 0 && index < C.CosmoShoppingOrder.Count - 1)
            {
                C.CosmoShoppingOrder.RemoveAt(index);
                C.CosmoShoppingOrder.Insert(index + 1, itemId);
            }
        }

        private static void MoveItemToTop(uint itemId)
        {
            int index = C.CosmoShoppingOrder.IndexOf(itemId);
            if (index > 0)
            {
                C.CosmoShoppingOrder.RemoveAt(index);
                C.CosmoShoppingOrder.Insert(0, itemId);
            }
        }

        public static void CheckConfigState()
        {
            if (C.CosmoShopping == null)
            {
                C.CosmoShopping = new();
                C.Save();
            }
            if (C.CosmoShoppingOrder == null)
            {
                C.CosmoShoppingOrder = new();
                C.Save();
            }
        }
    }
}