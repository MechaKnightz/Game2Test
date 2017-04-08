using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using MonoGame.Extended;
using Newtonsoft.Json;


namespace Game2Test
{
    public class Sprite
    {
        private Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                rectangle.X = (int)_position.X;
                rectangle.Y = (int)_position.Y;
            }
        }

        public Vector2 origin;
        public Rectangle rectangle;
        public float rotation;
        public string TextureName { get; set; }
        [JsonIgnore]
        public Texture2D texture;

        public Sprite() { }
        public Sprite(Texture2D texture, Vector2 position, Rectangle rectangle, float rotation, Vector2 origin)
        {
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
            this.texture = texture;
            this._position = position;
            this.rectangle = rectangle;
            this.origin = origin;
            this.rotation = rotation;
            TextureName = texture.Name;
        }
        public Sprite(Texture2D texture, Vector2 position, Rectangle rectangle)
        {
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
            this._position = position;
            this.rectangle = rectangle;
            this.texture = texture;
            this.rotation = 0f;
            origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
            TextureName = texture.Name;
        }
        public Sprite(Texture2D texture, Vector2 position)
        {
            this.rectangle.X = (int)position.X;
            this.rectangle.Y = (int)position.Y;
            this.rectangle.Width = texture.Width;
            this.rectangle.Height = texture.Height;
            this._position = position;
            this.texture = texture;
            origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
            TextureName = texture.Name;
        }
        public Sprite(Texture2D texture, Vector2 position, float rotation)
        {
            this.rectangle.X = (int)position.X;
            this.rectangle.Y = (int)position.Y;
            this.rectangle.Width = texture.Width;
            this.rectangle.Height = texture.Height;
            this._position = position;
            this.texture = texture;
            this.rotation = rotation;
            origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
            TextureName = texture.Name;
        }
        public Sprite(Texture2D texture, float rotation)
        {
            this.rectangle.X = (int)_position.X;
            this.rectangle.Y = (int)_position.Y;
            this.rectangle.Width = texture.Width;
            this.rectangle.Height = texture.Height;
            this.texture = texture;
            this.rotation = rotation;
            origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
            TextureName = texture.Name;
        }

        public Sprite(Texture2D texture)
        {
            this.texture = texture;
            TextureName = texture.Name;
        }
        public void SetPos(float pos, int var)
        {
            if(var == 0)
            {
                _position.X = pos;
                rectangle.X = (int)pos;
            }
            if(var == 1)
            {
                _position.Y = pos;
                rectangle.Y = (int)pos;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, _position, origin: origin, rotation: rotation);
        }
        public void DrawRectangle(SpriteBatch spriteBatch)
        {
            rectangle.X = (int)_position.X;
            rectangle.Y = (int) _position.Y;
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
        public void Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            spriteBatch.Draw(texture, _position + camera.Position, origin: origin, rotation: rotation);
        }
    }
}
