namespace GeoRegionMapper.Classes
{
	public class Coordinate
	{
		public double X { get; }
		public double Y { get; }

		public Coordinate(double x, double y)
		{
			X = x;
			Y = y;
		}

		public static Coordinate Create(List<double> coords)
		{
			if (coords is not { Count: 2 })
			{
				throw new ArgumentException("Invalid coordinate. Expected exactly two numbers.");
			}
			if (coords[0] < -180 || coords[0] > 180)
			{
				throw new ArgumentException(
					"Invalid coordinate. Expected longitude between -180 and 180."
				);
			}
			if (coords[1] < -90 || coords[1] > 90)
			{
				throw new ArgumentException(
					"Invalid coordinate. Expected latitude between -90 and 90."
				);
			}
			return new Coordinate(coords[0], coords[1]);
		}
	}
}