using System.Text.Json.Serialization;

namespace internship.Classes;

/// <summary>
/// Class to represent a location with name and coordinate
/// </summary>
public class Location
{
	public string Name { get; }
	public Coordinate Coordinate { get; }

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
}