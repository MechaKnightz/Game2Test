using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2Test.Sprites.Entities
{
    public class Asteroid : Sprite
    {
        public float Speed { get; set; }
        public float Acceleration { get; set; } = 1.1f;
        public float Health { get; set; }
        public float HealthMax { get; set; }
        public int Size { get; set; }
        public Bar Bar1 { get; set; }

        public Asteroid(Texture2D texture, Vector2 position, float speed, float healthMax, Bar bar, int size) :base(texture, position)
        {
            Speed = speed;
            HealthMax = healthMax;
            Health = healthMax;
            Bar1 = bar;
            Size = size;
        }

        public Asteroid()
        {

        }

        public void MoveTowardsPosition(Vector2 towardsPosition)
        {
            var angle = (float)Math.Atan2(towardsPosition.Y - Position.Y, towardsPosition.X - Position.X);

            var temp = Position;
            temp.X += (float)Math.Cos(angle) * Speed * Acceleration;
            temp.Y += (float)Math.Sin(angle) * Speed * Acceleration;
            Position = temp;

            Acceleration += 0.005f;

            Rectangle.X = (int)Position.X;
            Rectangle.Y = (int)Position.Y;
        }
        public new void Draw(SpriteBatch spriteBatch)
        {
            if(Health < HealthMax) Bar1.Draw(spriteBatch, Position, Health, HealthMax);
            spriteBatch.Draw(Texture, Position, origin: Origin, rotation: Rotation);
        }
    }
}
