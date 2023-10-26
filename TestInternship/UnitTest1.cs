namespace TestGeoRegionMapper
{
	public class CoordinateTests
	{
		[Fact]
		public void CoordinateConstructor_ShouldInitializeCorrectly()
		{
			Coordinate coord = new(1, 2);

			Assert.Equal(1, coord.X);
			Assert.Equal(2, coord.Y);
		}

		[Fact]
		public void CreateMethod_ShouldInitializeCorrectly()
		{
			Coordinate coord = Coordinate.Create(new List<double> { 45, 90 });

			Assert.Equal(45, coord.X);
			Assert.Equal(90, coord.Y);
		}

		[Fact]
		public void CreateMethod_ShouldThrowExceptionForInvalidListCount()
		{
			Assert.Throws<ArgumentException>(() => Coordinate.Create(new List<double> { 1.0 }));
			Assert.Throws<ArgumentException>(
				() => Coordinate.Create(new List<double> { 1.0, 2.0, 3.0 })
			);
		}

		[Fact]
		public void CreateMethod_ShouldThrowExceptionForInvalidLongitude()
		{
			Assert.Throws<ArgumentException>(() => Coordinate.Create(new List<double> { 181, 45 }));
			Assert.Throws<ArgumentException>(
				() => Coordinate.Create(new List<double> { -181, 45 })
			);
		}

		[Fact]
		public void CreateMethod_ShouldThrowExceptionForInvalidLatitude()
		{
			Assert.Throws<ArgumentException>(() => Coordinate.Create(new List<double> { 45, 91 }));
			Assert.Throws<ArgumentException>(() => Coordinate.Create(new List<double> { 45, -91 }));
		}

		[Fact]
		public void CoordinateConstructor_ShouldThrowExceptionForInvalidArgs()
		{
			Assert.Throws<ArgumentException>(() => Coordinate.Create(new List<double> { 1.0 }));
		}
	}

	public class LocationConverterTests
	{
		[Fact]
		public void ShouldDeserializeCorrectly()
		{
			string json = "{\"name\":\"TestLocation\",\"coordinates\":[1,2]}";

			Location deserializedLocation = JsonSerializer.Deserialize<Location>(
				json,
				new JsonSerializerOptions { Converters = { new LocationConverter() } }
			);

			Assert.Equal("TestLocation", deserializedLocation.Name);
			Assert.Equal(1, deserializedLocation.Coordinate.X);
			Assert.Equal(2, deserializedLocation.Coordinate.Y);
		}

		[Fact]
		public void ShouldThrowExceptionWhenCoordinatesInvalid()
		{
			string json = "{\"name\":\"TestLocation\",\"coordinates\":\"invalid\"}";
			Assert.Throws<JsonException>(() =>
			{
				JsonSerializer.Deserialize<Location>(
					json,
					new JsonSerializerOptions { Converters = { new LocationConverter() } }
				);
			});
		}

		[Fact]
		public void ShouldThrowExceptionWhenCoordinatesMissing()
		{
			string json = "{\"name\":\"TestLocation\"}";
			Assert.Throws<JsonException>(() =>
			{
				JsonSerializer.Deserialize<Location>(
					json,
					new JsonSerializerOptions { Converters = { new LocationConverter() } }
				);
			});
		}

		[Fact]
		public void ShouldThrowExceptionWhenJsonInvalid()
		{
			string json = "invalid_json";
			Assert.Throws<JsonException>(() =>
			{
				JsonSerializer.Deserialize<Location>(
					json,
					new JsonSerializerOptions { Converters = { new LocationConverter() } }
				);
			});
		}

		[Fact]
		public void ShouldThrowExceptionWhenNameMissing()
		{
			string json = "{\"coordinates\":[1,2]}";
			Assert.Throws<JsonException>(() =>
			{
				JsonSerializer.Deserialize<Location>(
					json,
					new JsonSerializerOptions { Converters = { new LocationConverter() } }
				);
			});
		}
	}

	public class LocationTests
	{
		[Fact]
		public void LocationConstructor_ShouldInitializeCorrectly()
		{
			var location = new Location("TestLocation", new List<double> { 1, 2 });

			Assert.Equal("TestLocation", location.Name);
			Assert.Equal(1, location.Coordinate.X);
			Assert.Equal(2, location.Coordinate.Y);
		}

		[Fact]
		public void LocationConstructor_ShouldThrowException_WhenInvalidCoordinates()
		{
			Assert.Throws<ArgumentException>(
				() => new Location("TestLocation", new List<double> { 1 })
			);
		}

		[Fact]
		public void LocationConstructor_ShouldThrowException_WhenNullCoordinates()
		{
			Assert.Throws<ArgumentException>(() => new Location("TestLocation", null));
		}

		[Fact]
		public void LocationConstructor_ShouldThrowException_WhenNullName()
		{
			Assert.Throws<ArgumentException>(() => new Location(null, new List<double> { 1, 2 }));
		}
	}

	public class PolygonTests
	{
		[Fact]
		public void PolygonConstructor_ShouldInitializeCorrectly()
		{
			var polygon = new Polygon(
				new List<List<double>>
				{
					new() { 1, 2 },
					new() { 3, 4 }
				}
			);

			Assert.Equal(2, polygon.Coordinates.Count);

			Assert.Equal(1, polygon.Coordinates[0].X);
			Assert.Equal(2, polygon.Coordinates[0].Y);

			Assert.Equal(3, polygon.Coordinates[1].X);
			Assert.Equal(4, polygon.Coordinates[1].Y);
		}

		// TODO: how to validate coordinates
		[Fact]
		public void PolygonConstructor_ShouldThrowException_WhenInvalidCoordinateCount()
		{
			Assert.Throws<ArgumentException>(
				() => new Polygon(new List<List<double>> { new() { 1 } })
			);
		}

		[Fact]
		public void PolygonConstructor_ShouldThrowException_WhenNullCoordinate()
		{
			Assert.Throws<ArgumentException>(() => new Polygon(new List<List<double>> { null }));
		}

		[Fact]
		public void PolygonConstructor_ShouldThrowException_WhenNullCoordinatesListElement()
		{
			Assert.Throws<ArgumentException>(
				() =>
					new Polygon(
						new List<List<double>>
						{
							new() { 1, 2 },
							null
						}
					)
			);
		}

		[Fact]
		public void PolygonConstructor_ShouldThrowException_WhenInvalidCoordinates()
		{
			Assert.Throws<ArgumentException>(
				() =>
					new Polygon(
						new List<List<double>>
						{
							new() { 2000, 300 },
							new() { -300, -400 }
						}
					)
			);
		}

		[Fact]
		public void PolygonConstructor_ShouldThrowException_WhenNullCoordinates()
		{
			Assert.Throws<ArgumentException>(() => new Polygon(null));
		}
	}

	public class RegionConverterTests
	{
		[Fact]
		public void ShouldDeserializeCorrectly()
		{
			string json = "{\"name\":\"TestRegion\",\"coordinates\":[[[1,2],[3,4]]]}";

			Region deserializedRegion;
			try
			{
				deserializedRegion = JsonSerializer.Deserialize<Region>(
					json,
					new JsonSerializerOptions { Converters = { new RegionConverter() } }
				);
			}
			catch (JsonException ex)
			{
				Assert.True(false, "Deserialization failed: " + ex.Message);
				return;
			}

			Assert.Equal("TestRegion", deserializedRegion.Name);
			Assert.Single(deserializedRegion.Polygons);
		}

		[Fact]
		public void ShouldHandleMultiplePolygons()
		{
			string json = "{\"name\":\"TestRegion\",\"coordinates\":[[[1,2],[3,4]],[[5,6],[7,8]]]}";
			Region deserializedRegion;
			try
			{
				deserializedRegion = JsonSerializer.Deserialize<Region>(
					json,
					new JsonSerializerOptions { Converters = { new RegionConverter() } }
				);
			}
			catch (JsonException ex)
			{
				Assert.True(false, "Deserialization failed: " + ex.Message);
				return;
			}

			Assert.Equal("TestRegion", deserializedRegion.Name);
			Assert.Equal(2, deserializedRegion.Polygons.Count);
		}

		[Fact]
		public void ShouldThrowExceptionWhenCoordinatesInvalid()
		{
			string json = "{\"name\":\"TestRegion\",\"coordinates\":\"invalid\"}";
			Assert.Throws<JsonException>(() =>
			{
				JsonSerializer.Deserialize<Region>(
					json,
					new JsonSerializerOptions { Converters = { new RegionConverter() } }
				);
			});
		}

		[Fact]
		public void ShouldThrowExceptionWhenCoordinatesMissing()
		{
			string json = "{\"name\":\"TestRegion\"}";
			Assert.Throws<JsonException>(() =>
			{
				JsonSerializer.Deserialize<Region>(
					json,
					new JsonSerializerOptions { Converters = { new RegionConverter() } }
				);
			});
		}

		[Fact]
		public void ShouldThrowExceptionWhenJsonInvalid()
		{
			string json = "invalid_json";
			Assert.Throws<JsonException>(() =>
			{
				JsonSerializer.Deserialize<Region>(
					json,
					new JsonSerializerOptions { Converters = { new RegionConverter() } }
				);
			});
		}

		[Fact]
		public void ShouldThrowExceptionWhenNameMissing()
		{
			string json = "{\"coordinates\":[[[1,2],[3,4]]]}";
			Assert.Throws<JsonException>(() =>
			{
				JsonSerializer.Deserialize<Region>(
					json,
					new JsonSerializerOptions { Converters = { new RegionConverter() } }
				);
			});
		}
	}

	public class RegionTests
	{
		[Fact]
		public void RegionConstructor_ShouldInitializeCorrectly()
		{
			var region = new Region(
				"TestRegion",
				new List<List<List<double>>>
				{
					new()
					{
						new List<double> { 1, 2 },
						new List<double> { 3, 4 }
					}
				}
			);

			Assert.Equal("TestRegion", region.Name);
			Assert.Single(region.Polygons);
		}

		[Fact]
		public void RegionConstructor_ShouldThrowException_WhenInvalidName()
		{
			Assert.Throws<ArgumentException>(
				() =>
					new Region(
						"",
						new List<List<List<double>>>
						{
							new()
							{
								new List<double> { 1, 2 }
							}
						}
					)
			);
		}

		[Fact]
		public void RegionConstructor_ShouldThrowException_WhenNullName()
		{
			Assert.Throws<ArgumentException>(
				() =>
					new Region(
						null,
						new List<List<List<double>>>
						{
							new()
							{
								new List<double> { 1, 2 }
							}
						}
					)
			);
		}

		[Fact]
		public void RegionConstructor_ShouldThrowException_WhenNullPolygons()
		{
			Assert.Throws<ArgumentException>(() => new Region("TestRegion", null));
		}
	}

	public class TaskUtilsTests
	{
		[Fact]
		public void IsPointInPolygon_ShouldReturnFalse_WhenPointIsOutside()
		{
			var point = new Coordinate(3, 3);
			var polygon = new Polygon(
				new List<List<double>>
				{
					new() { 0, 0 },
					new() { 2, 0 },
					new() { 2, 2 },
					new() { 0, 2 }
				}
			);

			bool result = TaskUtils.IsPointInPolygon(point, polygon);

			Assert.False(result);
		}

		[Fact]
		public void IsPointInPolygon_ShouldReturnFalse_WhenPointOnEdge()
		{
			var point = new Coordinate(1, 0);
			var polygon = new Polygon(
				new List<List<double>>
				{
					new() { 0, 0 },
					new() { 2, 0 },
					new() { 2, 2 },
					new() { 0, 2 }
				}
			);

			bool result = TaskUtils.IsPointInPolygon(point, polygon);

			Assert.False(result);
		}

		[Fact]
		public void IsPointInPolygon_ShouldReturnFalse_WhenPointOnVertex()
		{
			var point = new Coordinate(0, 0);
			var polygon = new Polygon(
				new List<List<double>>
				{
					new() { 0, 0 },
					new() { 2, 0 },
					new() { 2, 2 },
					new() { 0, 2 }
				}
			);

			bool result = TaskUtils.IsPointInPolygon(point, polygon);

			Assert.False(result);
		}

		[Fact]
		public void IsPointInPolygon_ShouldReturnTrue_WhenPointIsInside()
		{
			var point = new Coordinate(1, 1);
			var polygon = new Polygon(
				new List<List<double>>
				{
					new() { 0, 0 },
					new() { 2, 0 },
					new() { 2, 2 },
					new() { 0, 2 }
				}
			);

			bool result = TaskUtils.IsPointInPolygon(point, polygon);

			Assert.True(result);
		}

		[Fact]
		public void IsPointInPolygon_ShouldThrowException_WhenPointIsNull()
		{
			var polygon = new Polygon(
				new List<List<double>>
				{
					new() { 0, 0 },
					new() { 2, 0 },
					new() { 2, 2 },
					new() { 0, 2 }
				}
			);
			Assert.Throws<ArgumentNullException>(() => TaskUtils.IsPointInPolygon(null, polygon));
		}

		[Fact]
		public void IsPointInPolygon_ShouldThrowException_WhenPolygonIsNull()
		{
			var point = new Coordinate(1, 1);
			Assert.Throws<ArgumentNullException>(() => TaskUtils.IsPointInPolygon(point, null));
		}

		[Fact]
		public void MatchRegionsAndLocations_ShouldMatchCorrectly()
		{
			var region = new Region(
				"TestRegion",
				new List<List<List<double>>>
				{
					new()
					{
						new List<double> { 0, 0 },
						new List<double> { 2, 0 },
						new List<double> { 2, 2 },
						new List<double> { 0, 2 }
					}
				}
			);
			var location = new Location("TestLocation", new List<double> { 1, 1 });

			List<MatchedRegion> matched = TaskUtils.MatchRegionsAndLocations(
				new List<Region> { region },
				new List<Location> { location }
			);

			Assert.Single(matched);
			Assert.Equal(region.Name, matched.First().Region);
			Assert.Equal(location.Name, matched.First().MatchedLocations.First());
		}

		[Fact]
		public void MatchRegionsAndLocations_ShouldMatchMultipleLocations()
		{
			var region = new Region(
				"TestRegion",
				new List<List<List<double>>>
				{
					new()
					{
						new List<double> { 0, 0 },
						new List<double> { 2, 0 },
						new List<double> { 2, 2 },
						new List<double> { 0, 2 }
					}
				}
			);
			var locations = new List<Location>
			{
				new("Location1", new List<double> { 1, 1 }),
				new("Location2", new List<double> { 1.5, 1.5 })
			};

			List<MatchedRegion> matched = TaskUtils.MatchRegionsAndLocations(
				new List<Region> { region },
				locations
			);

			Assert.Equal(2, matched.First().MatchedLocations.Count);
		}

		[Fact]
		public void MatchRegionsAndLocations_ShouldNotMatch_WhenLocationOutsideRegion()
		{
			var region = new Region(
				"TestRegion",
				new List<List<List<double>>>
				{
					new()
					{
						new List<double> { 0, 0 },
						new List<double> { 2, 0 },
						new List<double> { 2, 2 },
						new List<double> { 0, 2 }
					}
				}
			);
			var location = new Location("OutsideLocation", new List<double> { 3, 3 });

			List<MatchedRegion> matched = TaskUtils.MatchRegionsAndLocations(
				new List<Region> { region },
				new List<Location> { location }
			);

			Assert.Empty(matched.First().MatchedLocations);
		}

		[Fact]
		public void MatchRegionsAndLocations_ShouldThrowException_WhenLocationsListIsNull()
		{
			Assert.Throws<ArgumentException>(
				() => TaskUtils.MatchRegionsAndLocations(new List<Region>(), null)
			);
		}

		[Fact]
		public void MatchRegionsAndLocations_ShouldThrowException_WhenRegionsListIsNull()
		{
			Assert.Throws<ArgumentException>(
				() => TaskUtils.MatchRegionsAndLocations(null, new List<Location>())
			);
		}
	}
}