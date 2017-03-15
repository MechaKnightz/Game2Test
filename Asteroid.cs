﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2Test
{
    public class Asteroid : Sprite
    {
        public float speed;
        public Asteroid(Texture2D texture, Vector2 position, float speed) :base(texture, position)
        {
            this.speed = speed;
        }

        public void MoveTowardsPosition(Vector2 towardsPosition)
        {
            float angle = (float)Math.Atan2(towardsPosition.Y - position.Y, towardsPosition.X - position.X);

            position.X += (float)(System.Math.Cos(angle)) * speed;
            position.Y += (float)(System.Math.Sin(angle)) * speed;

            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
        }
    }
}
