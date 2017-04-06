using System;
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
        public float acceleration = 1.1f;

        public float health;
        public float healthMax;
        public Bar bar;
        public Asteroid(Texture2D texture, Vector2 position, float speed, float healthMax, Bar bar) :base(texture, position)
        {
            this.speed = speed;
            this.healthMax = healthMax;
            health = healthMax;
            this.bar = bar;
        }

        public Asteroid()
        {

        }

        public void MoveTowardsPosition(Vector2 towardsPosition)
        {
            float angle = (float)Math.Atan2(towardsPosition.Y - Position.Y, towardsPosition.X - Position.X);

            var temp = Position;
            temp.X += (float)(Math.Cos(angle)) * speed * acceleration;
            temp.Y += (float)(Math.Sin(angle)) * speed * acceleration;
            Position = temp;

            acceleration += 0.005f;

            rectangle.X = (int)Position.X;
            rectangle.Y = (int)Position.Y;
        }
        public new void Draw(SpriteBatch spriteBatch)
        {
            if(health < healthMax) bar.Draw(spriteBatch, Position, health, healthMax);
            spriteBatch.Draw(texture, Position, origin: origin, rotation: rotation);
        }
    }
}
