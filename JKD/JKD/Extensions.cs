using System;
using OpenTK;

namespace JKD
{
	public static class Extensions
	{

		public static Vector2d DivideBy( this Vector2d a, Vector2d b )
		{
			return new Vector2d( a.X / b.X, a.Y / b.Y );
		}
	}
}

