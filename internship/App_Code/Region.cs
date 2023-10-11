using System.Text.Json.Serialization;

namespace internship;

/// <summary>
/// Class to represent a region with a name and a list of polygons
/// </summary>
public class Region
{
	[JsonPropertyName("name")]
	public string? Name { get; set; }

	[JsonPropertyName("coordinates")]
	public List<List<double[]>>? Polygons { get; set; }
}