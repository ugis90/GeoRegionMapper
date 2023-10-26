using GeoRegionMapper.Classes;
using System.Diagnostics;

namespace GeoRegionMapper.Utils;

public static class TaskUtils
{
	/// <summary>
	/// Determines if the given point is inside the polygon using Ray casting algorithm
	/// </summary>
	/// <param name="polygon">the vertices of polygon</param>
	/// <param name="point">the given point</param>
	/// <returns>true if the point is inside the polygon; otherwise, false</returns>
	public static bool IsPointInPolygon(Coordinate point, Polygon polygon)
	{
		if (point == null)
		{
			throw new ArgumentNullException(nameof(point), "Point must not be null.");
		}
		if (polygon == null)
		{
			throw new ArgumentNullException(nameof(polygon), "Polygon must not be null.");
		}
		if (polygon.Coordinates.Count < 3)
		{
			return false;
		}

		bool isInside = false;
		int j = polygon.Coordinates.Count - 1;
		for (int i = 0; i < polygon.Coordinates.Count; i++)
		{
			Coordinate pi = polygon.Coordinates[i];
			Coordinate pj = polygon.Coordinates[j];

			if (pi.Y < point.Y && pj.Y >= point.Y || pj.Y < point.Y && pi.Y >= point.Y)
			{
				if (pi.X + (point.Y - pi.Y) / (pj.Y - pi.Y) * (pj.X - pi.X) < point.X)
				{
					isInside = !isInside;
				}
			}
			j = i;
		}
		return isInside;
	}

	/// <summary>
	/// Matches regions and locations
	/// </summary>
	/// <param name="regions">Regions containing polygons</param>
	/// <param name="locations">Locations containing a coordinate</param>
	/// <returns>List of Matched Regions</returns>
	public static List<MatchedRegion> MatchRegionsAndLocations(
		List<Region> regions,
		List<Location> locations
	)
	{
		if (locations == null || !locations.Any())
		{
			throw new ArgumentException(
				"Locations list must not be null or empty.",
				nameof(locations)
			);
		}

		return (
			from region in regions
			let matchedLocations = (
				from polygon in region.Polygons
				from location in locations
				where IsPointInPolygon(location.Coordinate, polygon)
				select location.Name
			).ToList()
			select new MatchedRegion(region.Name, matchedLocations)
		).ToList();
	}

	/// <summary>
	/// Opens the given regions and locations in geojson.io
	/// </summary>
	/// <param name="regions">regions containing polygons</param>
	/// <param name="locations">locations containing a coordinate</param>
	public static void OpenInBrowser(List<Region> regions, List<Location> locations)
	{
		string combinedGeoJson = InOutUtils.ToGeoJsonFeatureCollection(regions, locations);
		string urlEncodedData = Uri.EscapeDataString(combinedGeoJson);
		string url = $"http://geojson.io/#data=data:application/json,{urlEncodedData}";

		Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
	}
}