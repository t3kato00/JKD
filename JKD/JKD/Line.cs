
using OpenTK;
using System.Runtime.InteropServices;

namespace JKD
{
	[StructLayout(LayoutKind.Sequential)]
	class Line
	{
		public Vector2d A { get; set; }
		public Vector2d B { get; set; }

		public double XCofficient { get { return B.Y - A.Y; } }
		public double YCofficient { get { return A.X - B.X; } }
		public double Constant { get { return ((A.Y-B.Y)*A.X + (B.X-A.X)*A.Y); } }

		private void MinMax( double a, double b, out double min, out double max )
		{
			if( a <= b )
			{
				min = a;
				max = b;
			}
			else
			{
				min = b;
				max = a;
			}
		}

		const double epsilon = 0.00001;
		public bool BoxCheck( Vector2d pos )
		{
			double xMin, xMax, yMin, yMax;
			MinMax( A.X, B.X, out xMin, out xMax );
			MinMax( A.Y, B.Y, out yMin, out yMax );
			return xMin <= pos.X + epsilon && pos.X <= xMax + epsilon && yMin <= pos.Y + epsilon && pos.Y <= yMax + epsilon;
		}

		public Line()
		{
		}

		public Line( Vector2d a, Vector2d b )
		{
			A = a;
			B = b;
		}

	}
}

