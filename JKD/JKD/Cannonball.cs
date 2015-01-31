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
		public Vector2d StartPosition { get; set; }
		public Vector2d StartVelocity { get; set; }
		public Vector2d Gravity { get; set; }
		public Vector3d Ground { get; set; }

		public Cannonball( Vector2d pos, Vector2d vel )
		{ 
		StartPosition = pos;
		StartVelocity = vel;
		Gravity = new Vector2d( 0.0, -9.81 );
		Ground = new Vector3d( 1.0, 0.0, 0.0 );
		}

		private Vector2d PositionAt( double t )
		{
			return StartPosition + t*StartVelocity + (0.5*t*t)*Gravity;
		}

		private bool SolveThrow( double a, double b, double c, out double t )
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
				double tMinus = tBase + tOffset;
				double tPlus = tBase - tOffset;
				if( tMinus > tPlus )
				{
					double temp = tMinus;
					tMinus = tPlus;
					tPlus = temp;
				}

				if (tMinus >= 0.0)
				{
					t = tMinus;
					return true;
				}
				else if (tPlus >= 0.0)
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
					return true;
				}

				t = -C/B;
				if(t >= 0.0)
					return true;
				else
				{
					t = 0.0;
					return false;
				}
			}
		}

		public bool Collide(List<Line> lines, out Vector2d position, out int bestIndex, out Line bestLine, out double bestTime, Vector2d gravity)
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
				if( SolveThrow( a, b, c, out t) )
					if( t < bestTime )
					{
						Vector2d pos = PositionAt(t);
						if(line.BoxCheck(pos))
						{
							bestTime = t;
							bestLine = line;
							bestIndex = index;
						}
					}

				index += 1;
			}
			if(double.IsInfinity(bestTime))
			{
				double t;
				SolveThrow( Ground.X, Ground.Y, Ground.Z, out t);
				position = PositionAt( t );
				return false;
			}
			else
			{
				position = PositionAt( bestTime );
				return true;
			}
		}
	}
}

