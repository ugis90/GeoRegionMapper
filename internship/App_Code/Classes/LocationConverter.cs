using System.Text.Json;
using System.Text.Json.Serialization;

namespace internship.Classes
{
	public class LocationConverter : JsonConverter<Location>
	{
		public override Location Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options
		)
		{
			using JsonDocument doc = JsonDocument.ParseValue(ref reader);
			JsonElement root = doc.RootElement;

			// Get "name" property
			if (
				!root.TryGetProperty("name", out JsonElement nameElement)
				|| !nameElement.ValueKind.Equals(JsonValueKind.String)
			)
			{
				throw new JsonException("Missing or invalid 'name' property.");
			}

			string name =
				nameElement.GetString() ?? throw new JsonException("Name property is null.");

			// Get "coordinates" property
			if (
				!root.TryGetProperty("coordinates", out JsonElement coordsElement)
				|| !coordsElement.ValueKind.Equals(JsonValueKind.Array)
			)
			{
				throw new JsonException("Missing or invalid 'coordinates' property.");
			}

			List<double> coordinates = coordsElement
				.EnumerateArray()
				.Select(coord => coord.GetDouble())
				.ToList();

			return new Location(name, coordinates);
		}

		public override void Write(
			Utf8JsonWriter writer,
			Location value,
			JsonSerializerOptions options
		)
		{
			throw new NotImplementedException();
		}
	}
}