using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using MonoGame.Extended;

namespace Game2Test
{
    public class Healthbar : Sprite
    {
        float max; //maxHealth et.c
        float current; //currentHealth et.c

        Sprite front;
        Sprite back;
        Vector2 offset;
        public Healthbar(Sprite front, Sprite back, Vector2 position, int width, int height, Vector2 offset, float max)
        {
            this.front = front;
            this.back = back;
            this.offset = offset;
            this.position = position + offset;
            this.max = max;
            current = max;
            this.front = front;
            this.back = back;

            front.rectangle.X = (int)position.X;
            front.rectangle.Y = (int)position.Y;

            back.rectangle.X = (int)position.X;
            back.rectangle.Y = (int)position.Y;

            front.rectangle.Width = width;
            front.rectangle.Height = height;

            back.rectangle.Width = width;
            back.rectangle.Height = height;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, float current)
        {
            position = position + offset;

            front.rectangle.X = (int)position.X;
            front.rectangle.Y = (int)position.Y;

            back.rectangle.X = (int)position.X;
            back.rectangle.Y = (int)position.Y;

            front.rectangle.Width = (int)(((current) / max) * back.rectangle.Width);

            spriteBatch.Draw(back.texture, back.rectangle, Color.White);
            spriteBatch.Draw(front.texture, front.rectangle, Color.White);
        }
    }
}
