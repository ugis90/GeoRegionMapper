using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace internship
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			IConfigurationRoot config = new ConfigurationBuilder()
				.AddCommandLine(args)
				.Build();

			// Looks for files in App_Data folder
			string regionsFilePath = Path.Combine("App_Data", config["regions"] ?? "regions.json");
			string locationsFilePath = Path.Combine("App_Data", config["locations"] ?? "locations.json");
			string outputFilePath = Path.Combine("App_Data", config["output"] ?? "results.json");

			string regionsJson = File.ReadAllText(regionsFilePath);
			string locationsJson = File.ReadAllText(locationsFilePath);

			List<Region>? regions = JsonSerializer.Deserialize<List<Region>>(regionsJson);
			List<Location>? locations = JsonSerializer.Deserialize<List<Location>>(locationsJson);

			if (regions == null || locations == null)
			{
				Console.WriteLine("Failed to deserialize regions or locations");
				return;
			}

			List<MatchedRegion> results = (
				from region in regions
				let matchedLocations = (
					from polygon in region.Polygons
					from location in locations
					where TaskUtils.IsPointInPolygon(location.Coordinates, polygon) // check if the location is inside the polygon
					select location.Name
				).ToList()
				select new MatchedRegion
				{
					Region = region.Name,
					MatchedLocations = matchedLocations
				}
			).ToList();

			JsonSerializerOptions options = new() { WriteIndented = true };
			string resultsJson = JsonSerializer.Serialize(results, options);
			File.WriteAllText(outputFilePath, resultsJson);
		}
	}
}