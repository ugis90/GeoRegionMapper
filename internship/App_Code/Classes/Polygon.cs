using System.Text.Json.Serialization;

namespace internship.Classes;

public class Polygon
{
	public List<Coordinate> Coordinates { get; }

	[JsonConstructor]
	public Polygon(List<List<double>> coordinates)
	{
		if (coordinates == null || !coordinates.Any())
		{
			throw new ArgumentException(
				"Coordinates list must not be null or empty.",
				nameof(coordinates)
			);
		}

		Coordinates = coordinates.Select(Coordinate.Create).ToList();
	}
}