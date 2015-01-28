
using OpenTK;
using System.Runtime.InteropServices;

namespace JKD
{
	[StructLayout(LayoutKind.Sequential)]
	class Line
	{
        public Vector2d A { get; set; }
        public Vector2d B { get; set; }

        public double XCofficient { get { return B.X - A.X; } }
        public double YCofficient { get { return B.Y - A.Y; } }
        public double Constant { get { return ((B.X - A.X) * A.Y - (B.Y - A.Y) * A.X); } }

		public Line( Vector2d a, Vector2d b )
		{
			A = a;
			B = b;
		}

	}


}

