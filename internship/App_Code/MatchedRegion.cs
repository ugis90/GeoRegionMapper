using System.Text.Json.Serialization;

namespace internship;

/// <summary>
/// Class to represent a matched region with a name and a list of matched locations
/// </summary>
public class MatchedRegion
{
    [JsonPropertyName("region")]
    public string? Region { get; set; }

    [JsonPropertyName("matched_locations")]
    public List<string>? MatchedLocations { get; set; }
}
