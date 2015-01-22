
using OpenTK;
using System.Runtime.InteropServices;

namespace JKD
{
	[StructLayout(LayoutKind.Sequential)]
	struct Lined
	{
		public Vector2d a;
		public Vector2d b;

		public Lined( Vector2d a, Vector2d b )
		{
			this.a = a;
			this.b = b;
		}

		public static explicit operator Line(Lined l)
		{
			return new Line((Vector2) l.a, (Vector2) l.b);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	struct Line
	{
		public Vector2 a;
		public Vector2 b;

		public Line( Vector2 a, Vector2 b )
		{
			this.a = a;
			this.b = b;
		}

		public static implicit operator Lined(Line l)
		{
			return new Lined((Vector2d) l.a, (Vector2d) l.b);
		}
	}
}

