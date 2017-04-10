using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json;

namespace Game2Test.Sprites
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
                Rectangle.X = (int)_position.X;
                Rectangle.Y = (int)_position.Y;
            }
        }

        public Vector2 Origin { get; set; }
        public Rectangle Rectangle;
        public float Rotation { get; set; }
        public string TextureName { get; set; }

        [JsonIgnore]
        public Texture2D Texture { get; set; }

        public Sprite() { }
        public Sprite(Texture2D texture, Vector2 position, Rectangle rectangle, float rotation, Vector2 origin)
        {
            rectangle.X = (int) position.X;
            rectangle.Y = (int) position.Y;
            Texture = texture;
            _position = position;
            Rectangle = rectangle;
            Origin = origin;
            Rotation = rotation;
            TextureName = texture.Name;
        }

        public Sprite(Texture2D texture, Vector2 position, Rectangle rectangle)
        {
            rectangle.X = (int) position.X;
            rectangle.Y = (int) position.Y;
            _position = position;
            Rectangle = rectangle;
            Texture = texture;
            Rotation = 0f;
            Origin = new Vector2(rectangle.Width / 2f, rectangle.Height / 2f);
            TextureName = texture.Name;
        }

        public Sprite(Texture2D texture, Vector2 position)
        {
            Rectangle.X = (int) position.X;
            Rectangle.Y = (int) position.Y;
            Rectangle.Width = texture.Width;
            Rectangle.Height = texture.Height;
            _position = position;
            Texture = texture;
            Origin = new Vector2(Rectangle.Width / 2f, Rectangle.Height / 2f);
            TextureName = texture.Name;
        }
        public Sprite(Texture2D texture, Vector2 position, float rotation)
        {
            Rectangle.X = (int) position.X;
            Rectangle.Y = (int) position.Y;
            Rectangle.Width = texture.Width;
            Rectangle.Height = texture.Height;
            _position = position;
            Texture = texture;
            Rotation = rotation;
            Origin = new Vector2(Rectangle.Width / 2f, Rectangle.Height / 2f);
            TextureName = texture.Name;
        }
        public Sprite(Texture2D texture, float rotation)
        {
            Rectangle.X = (int) _position.X;
            Rectangle.Y = (int) _position.Y;
            Rectangle.Width = texture.Width;
            Rectangle.Height = texture.Height;
            Texture = texture;
            Rotation = rotation;
            Origin = new Vector2(Rectangle.Width / 2f, Rectangle.Height / 2f);
            TextureName = texture.Name;
        }

        public Sprite(Texture2D texture)
        {
            this.Texture = texture;
            TextureName = texture.Name;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, _position, origin: Origin, rotation: Rotation);
        }
        public void DrawRectangle(SpriteBatch spriteBatch)
        {
            Rectangle.X = (int)_position.X;
            Rectangle.Y = (int) _position.Y;
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }
        public void Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            spriteBatch.Draw(Texture, _position + camera.Position, origin: Origin, rotation: Rotation);
        }
    }
}
