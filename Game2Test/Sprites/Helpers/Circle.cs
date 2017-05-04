using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Game2Test.Sprites.Helpers
{
    struct Circle
    {
        public Vector2 Position { get; set; }
        public float Radius { get; set; }

        public Circle(float radius, Vector2 position)
        {
            Radius = radius;
            Position = position;
        }

        public Circle(float radius)
        {
            Radius = radius;
            Position = Vector2.Zero;
        }

        public bool Intersect(Circle other)
        {
            var distance = Vector2.Distance(Position, other.Position);
            return distance < Radius || distance < other.Radius;
        }

        public bool Intersect(Rectangle other)
        {
            var corners = new List<Vector2>();
            corners.Add(new Vector2(other.Left, other.Top));
            corners.Add(new Vector2(other.Right, other.Top));
            corners.Add(new Vector2(other.Right, other.Bottom));
            corners.Add(new Vector2(other.Left, other.Bottom));

            foreach (var corner in corners)
            {
                if (Vector2.Distance(Position, corner) < Radius) return true;
            }

            return false;
        }
    }
}
