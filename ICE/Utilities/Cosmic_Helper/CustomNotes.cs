using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        // Dual Class Missions
        AddMissions(dict, 256f, "I would HIGHLY recommend doing these as you first start out.\n" +
                                "Knocks out 2 classes at once, allowing you to double dip and get done quicker.\n" +
                                "Make sure the turnin is on silver, gold isn't worth the extra time" +
                                "Also, best for A Rank level on Sinus",
                                496, 497, 498, 502, 503, 504);

        AddMissions(dict, 228f, "Best fishing mission.... period. Which sucks.\n" +
                                "Turnin on silver for best results. Don't do the other dual class, it's not worth",
                                509);

        // Basic Sinus A Ranks
        AddMissions(dict, 241f, "Best Score Per Minute outside of weather missions",
                    295, 115, 70, 205, 25, 340, 160, 250);

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

        AddMissions(dict, 211f,
                    "Best general gathering ones to do on Phaenna, not better than dual class.\n" +
                    "But if you're sticking around for weather missions, might as well do this.",
                    909, 951);

        AddMissions(dict, 437f,
                    "Really good weather missions for scoring, if these are up, you typically want to aim to do them.",
                    917, 959);

        AddMissions(dict, 392f,
                    "Second best weather missions for scoring, still good to focus over the basic A Ranks",
                    896, 938);



        return dict;
    }

    private static void AddMissions(Dictionary<uint, CustomNotes> dict, float spm, string note, params uint[] missionIds)
    {
        foreach (var id in missionIds)
        {
            dict[id] = new CustomNotes { SPM = spm, NoteInfo = note };
        }
    }

    public static Dictionary<uint, List<LevelInfo>> QuickLevelDict = new()
    {
        [8] = new()
        {
            new LevelInfo() { Level = 10, MissionId = new() { 3, }},
            new LevelInfo() { Level = 50, MissionId = new() { 8, }},
            new LevelInfo() { Level = 90, MissionId = new() { 19, }},
        },
        [9] = new()
        {
            new LevelInfo() { Level = 10, MissionId = new() { 48, }},
            new LevelInfo() { Level = 50, MissionId = new() { 53, }},
            new LevelInfo() { Level = 90, MissionId = new() { 64, }},
        },
        [10] = new()
        {
            new LevelInfo() { Level = 10, MissionId = new() { 93, }},
            new LevelInfo() { Level = 50, MissionId = new() { 98, }},
            new LevelInfo() { Level = 90, MissionId = new() { 109, }},
        },
        [11] = new()
        {
            new LevelInfo() { Level = 10, MissionId = new() { 138, }},
            new LevelInfo() { Level = 50, MissionId = new() { 143, }},
            new LevelInfo() { Level = 90, MissionId = new() { 154, }},
        },
        [12] = new()
        {
            new LevelInfo() { Level = 10, MissionId = new() { 183, }},
            new LevelInfo() { Level = 50, MissionId = new() { 188, }},
            new LevelInfo() { Level = 90, MissionId = new() { 199, }},
        },
        [13] = new()
        {
            new LevelInfo() { Level = 10, MissionId = new() { 228, }},
            new LevelInfo() { Level = 50, MissionId = new() { 233, }},
            new LevelInfo() { Level = 90, MissionId = new() { 244, }},
        },
        [14] = new()
        {
            new LevelInfo() { Level = 10, MissionId = new() { 273, 797}},
            new LevelInfo() { Level = 50, MissionId = new() { 278, }},
            new LevelInfo() { Level = 90, MissionId = new() { 289, }},
        },
        [15] = new()
        {
            new LevelInfo() { Level = 10, MissionId = new() { 318, 839}},
            new LevelInfo() { Level = 50, MissionId = new() { 323, 850}},
            new LevelInfo() { Level = 90, MissionId = new() { 334, 855}},
        },
        [16] = new()
        {
            new LevelInfo() { Level = 10, MissionId = new() { 365, 883}},
            new LevelInfo() { Level = 50, MissionId = new() { 369, 903}},
            new LevelInfo() { Level = 90, MissionId = new() { 374, 886}},
        },
        [17] = new()
        {
            new LevelInfo() { Level = 10, MissionId = new() { 365, 925}},
            new LevelInfo() { Level = 50, MissionId = new() { 369, 945}},
            new LevelInfo() { Level = 90, MissionId = new() { 374, 928}},
        },
        [18] = new()
        {
            new LevelInfo() { Level = 10, MissionId = new() { 453, 967}},
            new LevelInfo() { Level = 50, MissionId = new() { 458, 973}},
            new LevelInfo() { Level = 90, MissionId = new() { 465, 979}},
        }
    };

    public class LevelInfo
    {
        public uint Level { get; set; } = 10;
        public List<uint> MissionId { get; set; } = new();
    }
}
