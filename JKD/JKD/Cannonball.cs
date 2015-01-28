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
        public Vector2d startPosition { get; set; }
        public Vector2d startVelocity { get; set; }

        public Cannonball()
        { 
        }

        public bool Collide(List<Line> lines, out Vector2d position, out int bestIndex, out double bestTime, Vector2d gravity)
        {
            int index = 0;
            bestIndex = 0;
            bestTime = double.PositiveInfinity;
            foreach (Line line in lines)
            {
                double a = line.XCofficient;
                double b = line.YCofficient;
                double c = line.Constant;
                double A = b * gravity.Y + a * gravity.X;
                double B = a * startVelocity.X + b * startVelocity.Y;
                double C = a * startPosition.X + b * startPosition.Y + c;
                double timeBase = -B / A;
                double discriminate = Math.Sqrt(B * B - 2 * A * C) / A;
                double tPlus = timeBase + discriminate;
                double tMinus = timeBase - discriminate;

                if (tMinus >= 0.0 && tMinus < bestTime)
                {
                    bestTime = tMinus;
                    bestIndex = index;
                }
                else if (tPlus >= 0.0 && tPlus < bestTime)
                {
                    bestTime = tPlus;
                    bestIndex = index;
                }
                index += 1;
            }
            position = startPosition + startVelocity * bestTime + 0.5 * bestTime * bestTime * gravity;
            return !double.IsInfinity(bestTime);
        }
    }
}
