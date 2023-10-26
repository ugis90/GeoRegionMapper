using System.Text.Json;
using GeoRegionMapper.Classes;
using Microsoft.Extensions.Configuration;

namespace GeoRegionMapper.Utils
{
	public static class InOutUtils
	{
		private const string DefaultLocationsFile = "locations.json";
		private const string DefaultOutputFile = "results.json";
		private const string DefaultRegionsFile = "regions.json";

		/// <summary>
		/// Initialize configuration from command line arguments
		/// </summary>
		/// <param name="args">command line arguments</param>
		/// <returns>configuration</returns>
		public static IConfigurationRoot InitializeConfig(string[] args)
		{
			return new ConfigurationBuilder().AddCommandLine(args).Build();
		}

		/// <summary>
		/// Initialize JSON options with custom converters
		/// </summary>
		/// <returns>Json Serializer Options</returns>
		public static JsonSerializerOptions InitializeJsonOptions()
		{
			var options = new JsonSerializerOptions { WriteIndented = true };
			options.Converters.Add(new RegionConverter());
			options.Converters.Add(new LocationConverter());
			return options;
		}

		/// <summary>
		/// Read JSON files and return their contents
		/// </summary>
		/// <param name="config">configuration with command line arguments</param>
		/// <returns>input file contents and output file path</returns>
		/// <exception cref="FileNotFoundException">input files missing</exception>
		public static (
			string regionsJson,
			string locationsJson,
			string outputFilePath
		) ReadJsonFiles(IConfigurationRoot config)
		{
			string regionsFilePath = Path.Combine(
				"App_Data",
				config["regions"] ?? DefaultRegionsFile
			);
			string locationsFilePath = Path.Combine(
				"App_Data",
				config["locations"] ?? DefaultLocationsFile
			);
			string outputFilePath = Path.Combine("App_Data", config["output"] ?? DefaultOutputFile);

			if (!File.Exists(regionsFilePath) || !File.Exists(locationsFilePath))
			{
				throw new FileNotFoundException(
					"Either regions or locations JSON file is missing."
				);
			}

			string regionsJson = File.ReadAllText(regionsFilePath);
			string locationsJson = File.ReadAllText(locationsFilePath);

			return (regionsJson, locationsJson, outputFilePath);
		}

		/// <summary>
		/// Convert regions and locations to GeoJSON FeatureCollection
		/// </summary>
		/// <param name="regions">regions containing polygons</param>
		/// <param name="locations">locations containing a coordinate</param>
		/// <returns>JSON string of GeoJSON FeatureCollection</returns>
		public static string ToGeoJsonFeatureCollection(
			List<Region> regions,
			List<Location> locations
		)
		{
			var features = regions
				.Select(region => JsonSerializer.Deserialize<object>(region.ToGeoJson()))
				.ToList();

			features.AddRange(
				locations.Select(
					location => JsonSerializer.Deserialize<object>(location.ToGeoJson())
				)
			);

			var featureCollection = new { type = "FeatureCollection", features };

			return JsonSerializer.Serialize(featureCollection);
		}

		/// <summary>
		/// Check if regions and locations are valid
		/// </summary>
		/// <param name="regions">regions containing polygons</param>
		/// <param name="locations">locations containing a coordinate</param>
		public static void ValidateData(List<Region> regions, List<Location> locations)
		{
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
		}
	}
}