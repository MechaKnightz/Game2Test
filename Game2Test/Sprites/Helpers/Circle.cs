using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Game2Test.Sprites.Helpers
{
    public struct Circle
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Circle && Equals((Circle) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Position.GetHashCode() * 397) ^ Radius.GetHashCode();
            }
        }

        public Vector2 Position { get; set; }
        public float Radius;

        public static Circle Empty { get; set; } = new Circle(0, new Vector2(0, 0));
        public static Circle UnitCircle { get; set; } = new Circle(1, new Vector2(0, 0));

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
            var corners = new List<Vector2>
            {
                new Vector2(other.Left, other.Top),
                new Vector2(other.Right, other.Top),
                new Vector2(other.Right, other.Bottom),
                new Vector2(other.Left, other.Bottom)
            };

            var temp = this;
            return corners.Any(corner => Vector2.Distance(temp.Position, corner) < temp.Radius);
        }

        public Vector2 GetSide(Side side)
        {
            switch (side)
            {
                case Side.Right:
                    return Position + new Vector2(Radius, 0);
                case Side.Upper:
                    return Position + new Vector2(0, Radius);
                case Side.Left:
                    return Position + new Vector2(-Radius, 0);
                case Side.Lower:
                    return Position + new Vector2(0, -Radius);
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        public bool Contains(Vector2 position)
        {
            return Vector2.Distance(Position, position) < Radius;
        }
        public bool Contains(Circle other)
        {
            if (other.Radius > Radius) return false;
            return !(Vector2.Distance(Position, other.Position) + other.Radius > Radius);
        }

        public bool Contains(Rectangle other)
        {
            var corners = new List<Vector2>
            {
                new Vector2(other.Left, other.Top),
                new Vector2(other.Right, other.Top),
                new Vector2(other.Right, other.Bottom),
                new Vector2(other.Left, other.Bottom)
            };

            var temp = this;
            return corners.All(corner => !(Vector2.Distance(temp.Position, corner) > temp.Radius));
        }

        public bool Equals(Circle circle)
        {
            return circle.Equals(this);
        }
        public void Offset(Vector2 position)
        {
            Position += position;
        }
        public void Offset(float x, float y)
        {
            Position += new Vector2(x, y);
        }
        public static bool operator ==(Circle circle, Circle other)
        {
            return circle.Equals(other);
        }
        public static bool operator !=(Circle circle, Circle other)
        {
            return !(circle == other);
        }

        public enum Side
        {
            Right,
            Upper,
            Left,
            Lower
        }
    }
}
