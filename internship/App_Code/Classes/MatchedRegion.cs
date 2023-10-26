namespace GeoRegionMapper.Classes;

/// <summary>
/// Class to represent a matched region with a name and a list of matched locations
/// </summary>
public class MatchedRegion
{
	public string Region { get; }
	public List<string> MatchedLocations { get; }

	public MatchedRegion(string region, List<string> matchedLocations)
	{
		Region = region;
		MatchedLocations = matchedLocations;
	}
}