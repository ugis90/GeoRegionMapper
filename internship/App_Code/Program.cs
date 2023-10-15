using System.Text.Json;
using internship.Classes;
using internship.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;

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

				if (regions == null || locations == null)
				{
					Console.WriteLine("Failed to deserialize regions or locations");
					Environment.Exit(1);
				}
				if (!regions.Any() || !locations.Any())
				{
					Console.WriteLine("Regions or locations list is empty");
					Environment.Exit(1);
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

			// Optional: open locations and regions in geojson.io
			string combinedGeoJson = InOutUtils.ToGeoJsonFeatureCollection(regions, locations);
			string urlEncodedData = Uri.EscapeDataString(combinedGeoJson);
			string url = $"http://geojson.io/#data=data:application/json,{urlEncodedData}";
			Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
		}
	}
}