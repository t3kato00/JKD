using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace JKD
{
	class Cannonball
	{
		const double epsilon = 0.00001;
		public Vector2d StartPosition { get; set; }
		public Vector2d StartVelocity { get; set; }
		public Vector2d Gravity { get; set; }
		public Vector3d Ground { get; set; }

		public Cannonball( Vector2d pos, Vector2d vel )
		{ 
			StartPosition = pos;
			StartVelocity = vel;
			Gravity = new Vector2d( 0.0, -9.81 );
			Ground = new Vector3d( 0.0, 1.0, 0.0 );
		}

		public Vector2d PositionAt( double t )
		{
			return StartPosition + t*StartVelocity + (0.5*t*t)*Gravity;
		}

		delegate bool SolveCheck( double t );
		private bool SolveThrow( double a, double b, double c, out double t, SolveCheck check )
		{
			double A = a * Gravity.X       + b * Gravity.Y;
			double B = a * StartVelocity.X + b * StartVelocity.Y;
			double C = a * StartPosition.X + b * StartPosition.Y + c;
			if( A != 0 )
			{
				double tBase = -B / A;
				double discriminant = B * B - 2 * A * C;
				if( discriminant < 0 )
				{
					t = 0.0;
					return false;
				}
				double tOffset = Math.Sqrt(discriminant) / A;
				double tMinus = tBase - tOffset;
				double tPlus = tBase + tOffset;
				if( tMinus > tPlus )
				{
					double temp = tMinus;
					tMinus = tPlus;
					tPlus = temp;
				}

				if (tMinus > epsilon && check(tMinus))
				{
					t = tMinus;
					return true;
				}
				else if (tPlus > epsilon && check(tPlus))
				{
					t = tPlus;
					return true;
				}
				else
				{
					t = 0.0;
					return false;
				}
			}
			else // A == 0.0
			{
				if( B == 0.0 )
				{
					t = 0.0;
					return C == 0.0 && check(t);
				}

				t = -C/B;
				if(t >= 0.0 && check(t))
					return true;
				else
				{
					t = 0.0;
					return false;
				}
			}
		}

		public bool Collide(List<Line> lines, out Vector2d position, out int bestIndex, out Line bestLine, out double bestTime)
		{
			int index = 0;
			bestIndex = 0;
			bestTime = double.PositiveInfinity;
			bestLine = new Line();
			foreach (Line line in lines)
			{
				double a = line.XCofficient;
				double b = line.YCofficient;
				double c = line.Constant;

				double t;
				if( SolveThrow( a, b, c, out t, (double tc) => line.BoxCheck(PositionAt(tc))) )
					if( t < bestTime )
					{
						bestTime = t;
						bestLine = line;
						bestIndex = index;
					}

				index += 1;
			}
			if(double.IsInfinity(bestTime))
			{
				double t;
				SolveThrow( Ground.X, Ground.Y, Ground.Z, out t, (double tc) => true);
				position = PositionAt( t );
				return false;
			}
			else
			{
				position = PositionAt( bestTime );
				return true;
			}
		}

		public Vector2[] ArcPoints(double endTime, Vector2d pixelZoom)
		{ 
			double currentTime = 0;
			List<Vector2d> points = new List<Vector2d>();
			do
			{
				if (currentTime > endTime)
					currentTime = endTime;
				points.Add(PositionAt(currentTime));
				Vector2d deltaTimeComponents = pixelZoom.DivideBy(Gravity * currentTime + StartVelocity);
				double deltaTime = Math.Min(Math.Min(Math.Abs(deltaTimeComponents.X), Math.Abs(deltaTimeComponents.Y)), 0.01);
				currentTime += deltaTime;
			} while (currentTime < endTime);
			Vector2[] pointsArray = new Vector2[points.Count];
			for (int i = 0; i < points.Count; i += 1)
			{
				pointsArray[i] = (Vector2)points[i];
			}
			return pointsArray;
		}
	}
}

