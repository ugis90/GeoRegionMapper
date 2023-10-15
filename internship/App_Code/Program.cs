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
			var regions = new List<Region>();
			var locations = new List<Location>();

			try
			{
				IConfigurationRoot config = InOutUtils.InitializeConfig(args);
				var (regionsJson, locationsJson, outputFilePath) = InOutUtils.ReadJsonFiles(config);

				JsonSerializerOptions options = InOutUtils.InitializeJsonOptions();
				regions = JsonSerializer.Deserialize<List<Region>>(regionsJson, options);
				locations = JsonSerializer.Deserialize<List<Location>>(locationsJson, options);

				InOutUtils.ValidateData(regions, locations);

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
				Environment.Exit(1);
			}
			catch (JsonException e)
			{
				Console.WriteLine($"JSON error: {e.Message}");
				Environment.Exit(1);
			}
			catch (Exception e)
			{
				Console.WriteLine($"An error occurred: {e.Message}");
				Environment.Exit(1);
			}

			// Optional: open locations and regions in geojson.io
			try
			{
				TaskUtils.OpenInBrowser(regions, locations);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Failed to open browser: {e.Message}");
			}
		}
	}
}