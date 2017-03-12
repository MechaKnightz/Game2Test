using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using MonoGame.Extended;


namespace Game2Test
{
    public class Sprite
    {
        public Vector2 position, origin;
        public Rectangle rectangle;
        public Texture2D texture;
        public float rotation = 0;

        public Sprite() { }
        public Sprite(Texture2D texture, Vector2 position, Rectangle rectangle, float rotation, Vector2 origin)
        {
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
            this.texture = texture;
            this.position = position;
            this.rectangle = rectangle;
            this.origin = origin;
            this.rotation = rotation;
        }
        public Sprite(Texture2D texture, Vector2 position, Rectangle rectangle)
        {
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
            this.position = position;
            this.rectangle = rectangle;
            this.texture = texture;
            this.rotation = 0f;
            origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
        }
        public Sprite(Texture2D texture, Vector2 position)
        {
            this.rectangle.X = (int)position.X;
            this.rectangle.Y = (int)position.Y;
            this.rectangle.Width = texture.Width;
            this.rectangle.Height = texture.Height;
            this.position = position;
            this.texture = texture;
            this.rotation = 0f;
            origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
        }
        public void SetPos(float x, float y)
        {
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
        }
        public void SetPos(Vector2 pos)
        {
            position.X = pos.X;
            position.Y = pos.Y;
            rectangle.X = (int)pos.X;
            rectangle.Y = (int)pos.Y;
        }
        public void SetPos(float pos, int var)
        {
            if(var == 0)
            {
                position.X = pos;
                rectangle.X = (int)pos;
            }
            if(var == 1)
            {
                position.Y = pos;
                rectangle.Y = (int)pos;
            }
        }
        public void Update()
        {
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, origin: origin, rotation: rotation);
        }
        public void Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            spriteBatch.Draw(texture, position + camera.Position, origin: origin, rotation: rotation);
        }
    }
}
