using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Utilities;

public static partial class CosmicHelper
{
    public class CustomNotes
    {
        public string NoteInfo { get; set; }
        public float SPM { get; set; }
    };

    public static Dictionary<uint, CustomNotes> CustomMissionNotes =
        CreateMissionNotes();

    private static Dictionary<uint, CustomNotes> CreateMissionNotes()
    {
        var dict = new Dictionary<uint, CustomNotes>();

        // Basic Sinus A Ranks
        AddMissions(dict, 241f, "Best Score Per Minute outside of weather missions",
                    295, 115, 70, 205, 25, 340, 160, 250);

        // Dual Craft MIN/BTN
        AddMissions(dict, 256f,
                   "Best A Rank for gathering, while also double dipping into crafters which is best for both worlds\n" +
                   "Silver turnin is recommended for this, due to the time not spent on crafting",
                   496, 497, 498, 502, 503, 504);

        // Dual Craft FSH
        AddMissions(dict, 228f,
                   "Best A Rank for FISHING, while also double dipping into crafters which is best for both worlds\n" +
                   "Silver turnin is HIGHLY recommended for this, due to the time not spent on crafting and fish RNG. Silver ends up being ~228 SPM, VS. Gold being ~152 SPM",
                   509);

        // Sinus Weather Missions
        AddMissions(dict, 490f,
                   "Best Score Per Minute on Sinus. Ideally you would want to focus these as much as possible.\n" +
                   "Aim to complete these on silver, that's the best SPM you'll get with the lv. 100 gear",
                    31, 32, 76, 77, 121, 122, 166, 167, 211, 212, 256, 257, 301, 302, 346, 347);

        // Sinus A Rank Missions
        AddMissions(dict, 207f,
                   "If you're not doing the dual craft missions (you should be, this is worse), then this is the 2nd best \n" +
                   "Aim for silver or gold on this, they average about the same",
                   387, 432);

        // Sinus Critical Missions
        AddMissions(dict, 406f,
                    "Always aim to do criticals when you can for score",
                    536, 537, 538, 539, 540, 541, 542, 543, 544);

        // Basic Phaenna
        AddMissions(dict, 336f, "Best Score Per Minute out of weather missions.",
                    569, 611, 653, 695, 737, 779, 821, 863);

        // Phaenna Sequence
        AddMissions(dict, 362f,
                   "This set of sequence missions is the best if you're chaining missions \nThis averages out to being better than a normal mission.",
                    580, 622, 664, 706, 748, 790, 832, 874);

        // Phaenna Weather
        AddMissions(dict, 281f,
                   "Best Weather missions that are here, not really worth doing over the basic missions IMHO. But The info is here for you",
                   573, 615, 657, 699, 741, 783, 825, 867);

        AddMissions(dict, 379f,
                   "Criticals are always worth doing for score",
                    1007, 1008, 1009, 1010, 1011, 1012, 1013, 1014, 1015, 1016, 1017, 1018, 
                    1019, 1020, 1021, 1022, 1023, 1024, 1025, 1026, 1027, 1028, 1029, 1030);



        return dict;
    }

    private static void AddMissions(Dictionary<uint, CustomNotes> dict, float spm, string note, params uint[] missionIds)
    {
        foreach (var id in missionIds)
        {
            dict[id] = new CustomNotes { SPM = spm, NoteInfo = note };
        }
    }
}
