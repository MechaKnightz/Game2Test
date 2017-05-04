using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ClassLibary
{
    public static class Angle
    {
        public static float AngleToOther(Vector2 main, Vector2 other)
        {
            return (float)Math.Atan2(other.Y - main.Y, other.X - main.X);
        }

        public static Vector2 MoveAngle(Vector2 position, float angle, float length)
        {
            position.X += (float)Math.Cos(angle) * length;
            position.Y += (float)Math.Sin(angle) * length;

            return position;
        }
    }
}
