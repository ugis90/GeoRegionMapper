using System.Text.Json.Serialization;

namespace internship;

/// <summary>
/// Class to represent a location with name and coordinates
/// </summary>
public class Location
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("coordinates")]
    public double[]? Coordinates { get; set; }
}
