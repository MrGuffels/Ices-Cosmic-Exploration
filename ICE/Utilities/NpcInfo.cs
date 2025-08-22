using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ICE.Utilities;

internal class NpcInfo
{
    public class NpcClass
    {
        public string Name { get; set; }
        public Vector2 BoxCorner1 { get; set; }
        public Vector2 BoxCorner2 { get; set; }
        public Vector3 NpcLocation { get; set; }
    }

    public static Dictionary<uint, NpcClass> NpcLibrary = new()
    {
        [1052610] = new NpcClass // Repair | Gil Gear Vendor
        {
            Name = "Godgyth",
            BoxCorner1 = new Vector2(16.73f, 12.85f),
            BoxCorner2 = new Vector2(14.00f, 19.02f),
            NpcLocation = new Vector3(19.46f, 1.69f, 18.11f)
        },
        [1052608] = new NpcClass // Credit Exchange Vendor
        {
            Name = "Mesouaidonque",
            BoxCorner1 = new Vector2(16.73f, 12.85f),
            BoxCorner2 = new Vector2(14.00f, 19.02f),
            NpcLocation = new Vector3(18.23f, 1.69f, 19.42f)
        },
        [1052605] = new NpcClass // Relic NPC
        {
            Name = "Researchingway",
            BoxCorner1 = new Vector2(-15.13f, 18.63f),
            BoxCorner2 = new Vector2(-17.73f, 13.57f),
            NpcLocation = new Vector3(-18.91f, 2.15f, 18.84f)
        },
        [1052612] = new NpcClass // Cosmic Fortune aka Gamba Wheel
        {
            Name = "Orbitingway",
            BoxCorner1 = new Vector2(14.35f, -19.41f),
            BoxCorner2 = new Vector2(17.43f, -13.28f),
            NpcLocation = new Vector3(18.84f, 2.24f, -18.91f)
        }
    };
}
