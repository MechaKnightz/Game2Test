using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Game2Test.Sprites.Helpers
{
    public class Line
    {
        public Vector2 p1, p2;

        public Line(Vector2 p1, Vector2 p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        public List<Vector2> GetPoints(int quantity)
        {
            var vectors = new List<Vector2>();
            int ydiff = (int)p2.Y - (int)p1.Y, xdiff = (int)p2.X - (int)p1.X;
            double slope = (double)(p2.Y - p1.Y) / (p2.X - p1.X);
            double x, y;

            --quantity;

            for (double i = 0; i < quantity; i++)
            {
                y = slope == 0 ? 0 : ydiff * (i / quantity);
                x = slope == 0 ? xdiff * (i / quantity) : y / slope;
                vectors.Add(new Vector2((int)Math.Round(x) + p1.X, (int)Math.Round(y) + p1.Y));
            }

            vectors.Add(p2);
            return vectors;
        }
    }
}
