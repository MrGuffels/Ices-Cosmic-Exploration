using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Numerics; // Add this for Vector2 and Vector3

namespace ICE.Utilities;

internal static class NpcData // Renamed the class to avoid conflict
{
    public enum NpcType
    {
        Repair,
        Credit,
        Relic,
        Gamba
    }

    public class NPCInfo // Keep this class for the dictionary
    {
        public NpcType type { get; set; }
        public uint NpcId { get; set; }
        public string Name { get; set; }
        public Vector2 BoxCorner1 { get; set; }
        public Vector2 BoxCorner2 { get; set; }
        public Vector3 NpcLocation { get; set; }
    }

    public static Dictionary<uint, List<NPCInfo>> MoonNpcs = new() // Use NPCInfo instead of NpcInfo
    {
        [1237] = new List<NPCInfo>
        {
            new NPCInfo // Repair | Gil Gear Vendor
            {
                type = NpcType.Repair,
                NpcId = 1052610,
                Name = "Godgyth",
                BoxCorner1 = new Vector2(16.73f, 12.85f),
                BoxCorner2 = new Vector2(14.00f, 19.02f),
                NpcLocation = new Vector3(19.46f, 1.69f, 18.11f)
            },
            new NPCInfo // Credit Exchange Vendor
            {
                type = NpcType.Credit,
                NpcId = 1052608,
                Name = "Mesouaidonque",
                BoxCorner1 = new Vector2(16.73f, 12.85f),
                BoxCorner2 = new Vector2(14.00f, 19.02f),
                NpcLocation = new Vector3(18.23f, 1.69f, 19.42f)
            },
            new NPCInfo // Relic NPC
            {
                type = NpcType.Relic,
                NpcId = 1052605,
                Name = "Researchingway",
                BoxCorner1 = new Vector2(-15.13f, 18.63f),
                BoxCorner2 = new Vector2(-17.73f, 13.57f),
                NpcLocation = new Vector3(-18.91f, 2.15f, 18.84f)
            },
            new NPCInfo // Cosmic Fortune aka Gamba Wheel
            {
                type = NpcType.Gamba,
                NpcId = 1052612,
                Name = "Orbitingway",
                BoxCorner1 = new Vector2(14.35f, -19.41f),
                BoxCorner2 = new Vector2(17.43f, -13.28f),
                NpcLocation = new Vector3(18.84f, 2.24f, -18.91f)
            }
        },
        [1291] = new List<NPCInfo>
        {
            new NPCInfo()
            {
                type = NpcType.Repair,
                NpcId = 1052641,
                Name = "Godgyth",
                BoxCorner1 = new Vector2(360.23f, -405.31f),
                BoxCorner2 = new Vector2(353.70f, -401.53f),
                NpcLocation = new Vector3(359.52f, 52.75f, -401.72f)
            },
            new NPCInfo // Credit Exchange Vendor
            {
                type = NpcType.Credit,
                NpcId = 1052640,
                Name = "Mesouaidonque",
                BoxCorner1 = new Vector2(354.92f, -399.38f),
                BoxCorner2 = new Vector2(358.81f, -406.05f),
                NpcLocation = new Vector3(358.33f, 52.75f, -400.44f)
            },
            new NPCInfo // Relic NPC
            {
                type = NpcType.Relic,
                NpcId = 1052629,
                Name = "Researchingway",
                BoxCorner1 = new Vector2(324.58f, -400.18f),
                BoxCorner2 = new Vector2(320.88f, -407.44f),
                NpcLocation = new Vector3(321.22f, 53.19f, -401.24f)
            },
            new NPCInfo // Cosmic Fortune aka Gamba Wheel
            {
                type = NpcType.Gamba,
                NpcId = 1052642,
                Name = "Orbitingway",
                BoxCorner1 = new Vector2(361.15f, -433.80f),
                BoxCorner2 = new Vector2(352.55f, -438.12f),
                NpcLocation = new Vector3(358.82f, 53.19f, -438.86f)
            }
        },
    };
}