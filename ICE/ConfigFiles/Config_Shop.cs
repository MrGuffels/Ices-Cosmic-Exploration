using System.Collections.Generic;

namespace ICE.ConfigFiles;

public partial class Config
{
    public Dictionary<uint, CosmoShoppingList> CosmoShopping { get; set; } = new();
    public List<uint> CosmoShoppingOrder { get; set; } = new();
    public List<uint> CosmoShoppingOrder_Gear { get; set; } = new();

    public bool BuyItems { get; set; } = false;
    public int CosmoBuyAtAmount { get; set; } = 10000;
    public int CosmoKeepAmount { get; set; } = 0;

    public class CosmoShoppingList
    {
        public int KeepAmount { get; set; } = 0;
        public int BuyAmount { get; set; } = 0;
        public bool KeepBuying { get; set; } = false;
    }
}
