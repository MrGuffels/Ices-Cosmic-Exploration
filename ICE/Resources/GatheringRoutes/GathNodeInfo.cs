using System;
using System.Collections.Generic;
using System.Numerics;
using YamlDotNet.Serialization;

namespace ICE.Resources.GatheringRoutes;

public class GathNodeInfo
{
    [YamlMember(Alias = "node_id")]
    public uint NodeId { get; set; }

    [YamlMember(Alias = "position")]
    public Vector3 Position { get; set; }

    [YamlMember(Alias = "land_zone")]
    public Vector3 LandZone { get; set; }

    [YamlMember(Alias = "radius_start")]
    public float RadiusStart { get; set; } = 0.0f;

    [YamlMember(Alias = "radius_end")]
    public float RadiusEnd { get; set; } = 360.0f;

    [YamlMember(Alias = "min_distance")]
    public float MinDistance { get; set; } = 1.0f;

    [YamlMember(Alias = "max_distance")]
    public float MaxDistance { get; set; } = 5.0f;
}

public class GatheringRouteFile
{
    [YamlMember(Alias = "zone_id")]
    public uint ZoneId { get; set; }

    [YamlMember(Alias = "zone_name")]
    public string ZoneName { get; set; } = string.Empty;

    [YamlMember(Alias = "job")]
    public string Job { get; set; } = string.Empty;  // "MIN" or "BTN"

    [YamlMember(Alias = "flag")]
    public Vector2 Flag { get; set; }

    [YamlMember(Alias = "author")]
    public string? Author { get; set; }

    [YamlMember(Alias = "date_modified")]
    public DateTime? DateModified { get; set; }

    [YamlMember(Alias = "nodes")]
    public List<GathNodeInfo> Nodes { get; set; } = new();
}