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

            var tempRect = front.Rectangle;
            var tempRect2 = back.Rectangle;
            tempRect.X = (int)position.X;
            tempRect.Y = (int)position.Y;

            tempRect2.X = (int)position.X;
            tempRect2.Y = (int)position.Y;

            tempRect.Width = width;
            tempRect.Height = height;

            tempRect2.Width = width;
            tempRect2.Height = height;

            front.Rectangle = tempRect;
            back.Rectangle = tempRect2;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, float current, float max)
        {
            position = position + offset;

            var tempRect = front.Rectangle;
            var tempRect2 = back.Rectangle;

            tempRect.X = (int)position.X;
            tempRect.Y = (int)position.Y;

            tempRect2.X = (int)position.X;
            tempRect2.Y = (int)position.Y;

            tempRect.Width = (int)(((current) / max) * back.Rectangle.Width);

            front.Rectangle = tempRect;
            back.Rectangle = tempRect2;

            spriteBatch.Draw(back.Texture, back.Rectangle, Color.White);
            spriteBatch.Draw(front.Texture, front.Rectangle, Color.White);
        }
    }
}
