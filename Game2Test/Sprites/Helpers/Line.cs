using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Game2Test.Sprites.Helpers
{
    public class Line
    {
        public Vector2 P1 { get; set; }
        public Vector2 P2 { get; set; }

        public Line(Vector2 p1, Vector2 p2)
        {
            this.P1 = p1;
            this.P2 = p2;
        }

        public List<Vector2> GetPoints(int quantity)
        {
            var vectors = new List<Vector2>();
            int ydiff = (int)P2.Y - (int)P1.Y, xdiff = (int)P2.X - (int)P1.X;
            var slope = (double)(P2.Y - P1.Y) / (P2.X - P1.X);

            --quantity;

            for (double i = 0; i < quantity; i++)
            {
                var y = slope == 0 ? 0 : ydiff * (i / quantity);
                var x = slope == 0 ? xdiff * (i / quantity) : y / slope;
                vectors.Add(new Vector2((int)Math.Round(x) + P1.X, (int)Math.Round(y) + P1.Y));
            }

            vectors.Add(P2);
            return vectors;
        }
    }
}
