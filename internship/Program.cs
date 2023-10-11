using System.Text.Json;

namespace internship
{
	internal class Program
	{
		private static void Main()
		{
			string regionsJson = File.ReadAllText("regions.json");
			string locationsJson = File.ReadAllText("locations.json");

			List<Region>? regions = JsonSerializer.Deserialize<List<Region>>(regionsJson);
			List<Location>? locations = JsonSerializer.Deserialize<List<Location>>(locationsJson);

			if (regions == null || locations == null)
			{
				Console.WriteLine("Failed to deserialize regions or locations");
				return;
			}

			List<MatchedRegion> results = (from region in regions
										   let matchedLocations = (from polygon in region.Polygons
																   from location in locations
																   where TaskUtils.IsPointInPolygon(location.Coordinates, polygon) // check if the location is inside the polygon
																   select location.Name).ToList() // select the name of the matched locations
										   select new MatchedRegion { Region = region.Name, MatchedLocations = matchedLocations }).ToList();

			string resultsJson = JsonSerializer.Serialize(results);
			File.WriteAllText("results.json", resultsJson);
		}
	}
}