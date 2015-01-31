
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
		public double YCofficient { get { return B.X - A.X; } }
		public double Constant { get { return (YCofficient * A.Y - XCofficient * A.X); } }

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

		public bool BoxCheck( Vector2d pos )
		{
			double xMin, xMax, yMin, yMax;
			MinMax( A.X, B.X, out xMin, out xMax );
			MinMax( A.Y, B.Y, out yMin, out yMax );
			return xMin <= pos.X && pos.X <= xMax && yMin <= pos.Y && pos.Y <= yMax;
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

