using System.Text.Json.Serialization;

namespace internship.Classes;

/// <summary>
/// Class to represent a region with a name and a list of polygons
/// </summary>
public class Region
{
    public string Name { get; }
    public List<Polygon> Polygons { get; }

    [JsonConstructor]
    public Region(string name, List<List<List<double>>> coordinates)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name must not be null or empty.", nameof(name));
        }

        if (coordinates == null || !coordinates.Any())
        {
            throw new ArgumentException(
                "Coordinates list must not be null or empty.",
                nameof(coordinates)
            );
        }

        Name = name;
        Polygons = coordinates.Select(coords => new Polygon(coords)).ToList();
    }
}
