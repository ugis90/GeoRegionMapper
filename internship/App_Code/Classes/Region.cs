using System.Text.Json;
using System.Text.Json.Serialization;

namespace internship.Classes;

/// <summary>
/// Class to represent a region with a name and a list of polygons
/// </summary>
public class Region
{
	public string Name { get; }
	public List<Polygon> Polygons { get; }

	/// <summary>
	/// Converts coordinates to List of Polygons
	/// </summary>
	/// <param name="name">Region name</param>
	/// <param name="coordinates">List of Polygons</param>
	/// <exception cref="ArgumentException">List of Polygons is null or empty</exception>
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

	/// <summary>
	/// Converts the region to GeoJson
	/// </summary>
	/// <returns>JSON string of GeoJson region</returns>
	public string ToGeoJson()
	{
		List<List<double[]>> polygons = Polygons
			.Select(
				polygon => polygon.Coordinates.Select(coord => new[] { coord.X, coord.Y }).ToList()
			)
			.ToList();

		var feature = new
		{
			type = "Feature",
			geometry = new { type = "Polygon", coordinates = polygons },
			properties = new { name = Name }
		};

		return JsonSerializer.Serialize(feature);
	}
}