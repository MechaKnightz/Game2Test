using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Game2Test.Sprites;
using MonoGame.Extended;

namespace Game2Test
{
    public class Bar : Sprite
    {
        float max; //maxHealth et.c
        float current; //currentHealth et.c

        Sprite front;
        Sprite back;
        Vector2 offset;
        public Bar(Sprite front, Sprite back, Vector2 position, int width, int height, Vector2 offset, float max)
        {
            this.front = front;
            this.back = back;
            this.offset = offset;
            Position = position + offset;
            this.max = max;
            current = max;
            this.front = front;
            this.back = back;

            front.Rectangle.X = (int)position.X;
            front.Rectangle.Y = (int)position.Y;

            back.Rectangle.X = (int)position.X;
            back.Rectangle.Y = (int)position.Y;

            front.Rectangle.Width = width;
            front.Rectangle.Height = height;

            back.Rectangle.Width = width;
            back.Rectangle.Height = height;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, float current, float max)
        {
            position = position + offset;

            front.Rectangle.X = (int)position.X;
            front.Rectangle.Y = (int)position.Y;

            back.Rectangle.X = (int)position.X;
            back.Rectangle.Y = (int)position.Y;

            front.Rectangle.Width = (int)(((current) / max) * back.Rectangle.Width);

            spriteBatch.Draw(back.Texture, back.Rectangle, Color.White);
            spriteBatch.Draw(front.Texture, front.Rectangle, Color.White);
        }
    }
}
