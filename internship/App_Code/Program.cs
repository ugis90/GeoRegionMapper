using System.Text.Json;
using internship.Classes;
using internship.Utils;
using Microsoft.Extensions.Configuration;

namespace internship
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			try
			{
				IConfigurationRoot config = InOutUtils.InitializeConfig(args);
				var (regionsJson, locationsJson, outputFilePath) = InOutUtils.ReadJsonFiles(config);

				JsonSerializerOptions options = InOutUtils.InitializeJsonOptions();
				var regions = JsonSerializer.Deserialize<List<Region>>(regionsJson, options);
				var locations = JsonSerializer.Deserialize<List<Location>>(locationsJson, options);

				if (regions == null || locations == null)
				{
					Console.WriteLine("Failed to deserialize regions or locations");
					return;
				}
				if (!regions.Any() || !locations.Any())
				{
					Console.WriteLine("Regions or locations list is empty");
					return;
				}

				List<MatchedRegion> results = TaskUtils.MatchRegionsAndLocations(
					regions,
					locations
				);
				string resultsJson = JsonSerializer.Serialize(results, options);

				File.WriteAllText(outputFilePath, resultsJson);
			}
			catch (FileNotFoundException e)
			{
				Console.WriteLine($"File not found: {e.Message}");
			}
			catch (JsonException e)
			{
				Console.WriteLine($"JSON error: {e.Message}");
			}
			catch (Exception e)
			{
				Console.WriteLine($"An error occurred: {e.Message}");
			}
		}
	}
}