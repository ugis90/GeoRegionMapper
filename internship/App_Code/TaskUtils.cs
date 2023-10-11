namespace internship;

public static class TaskUtils
{
	/// <summary>
	/// Determines if the given point is inside the polygon using Ray casting algorithm
	/// </summary>
	/// <param name="polygon">the vertices of polygon</param>
	/// <param name="point">the given point</param>
	/// <returns>true if the point is inside the polygon; otherwise, false</returns>
	public static bool IsPointInPolygon(double[]? point, List<double[]> polygon)
	{
		bool isInside = false;
		int j = polygon.Count - 1;
		for (int i = 0; i < polygon.Count; i++)
		{
			if (point != null && (polygon[i][1] < point[1] && polygon[j][1] >= point[1] ||
								  polygon[j][1] < point[1] && polygon[i][1] >= point[1]))
			{
				if (polygon[i][0] + (point[1] - polygon[i][1]) /
					(polygon[j][1] - polygon[i][1]) *
					(polygon[j][0] - polygon[i][0]) < point[0])
				{
					isInside = !isInside;
				}
			}
			j = i;
		}
		return isInside;
	}
}