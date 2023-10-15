using System.Text.Json;
using System.Text.Json.Serialization;

namespace internship.Classes;

/// <summary>
/// Class to represent a location with name and coordinate
/// </summary>
public class Location
{
	public string Name { get; }
	public Coordinate Coordinate { get; }

	/// <summary>
	/// Converts coordinates to a single coordinate
	/// </summary>
	/// <param name="name">Location name</param>
	/// <param name="coordinates">coordinates - 2 numbers in a list</param>
	/// <exception cref="ArgumentException"></exception>
	[JsonConstructor]
	public Location(string name, List<double> coordinates)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentException("Name must not be null or empty.", nameof(name));
		}
		if (coordinates is not { Count: 2 })
		{
			throw new ArgumentException("Invalid coordinate. Expected exactly two numbers.");
		}

		Name = name;
		Coordinate = new Coordinate(coordinates[0], coordinates[1]);
	}

	/// <summary>
	/// Converts the location to GeoJson
	/// </summary>
	/// <returns>Json string of GeoJson location</returns>
	public string ToGeoJson()
	{
		var feature = new
		{
			type = "Feature",
			geometry = new
			{
				type = "Point",
				coordinates = new double[] { Coordinate.X, Coordinate.Y }
			},
			properties = new { name = Name }
		};

		return JsonSerializer.Serialize(feature);
	}
}