using System;
using Game2Test.Sprites.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json;

namespace Game2Test.Sprites
{
    public class Sprite
    {
        public Vector2 Origin { get; set; }
        public Rectangle Rectangle { get; set; }
        public RotatedRectangle RotatedRectangle { get; set; }

        private float _rotation;
        public float Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                if(RotatedRectangle != null) RotatedRectangle.Rotation = value;
            }
        }

        [JsonIgnore]
        public Texture2D Texture { get; set; }

        private Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;

                var tempRect = Rectangle;
                tempRect.X = (int)Math.Round(_position.X, 0);
                tempRect.Y = (int)Math.Round(_position.Y, 0);
                Rectangle = tempRect;

                if (RotatedRectangle != null) RotatedRectangle.SetPosition((int)_position.X, (int)_position.Y);
            }
        }

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
        }

        public Sprite(Sprite sprite)
        {
            Origin = sprite.Origin;
            Rectangle = new Rectangle(sprite.Rectangle.X, sprite.Rectangle.Y, sprite.Rectangle.Width, sprite.Rectangle.Height);
            RotatedRectangle = new RotatedRectangle(sprite.RotatedRectangle);
            Rotation = sprite.Rotation;
            Texture = sprite.Texture;
            Position = sprite.Position;
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
        }

        public Sprite(Texture2D texture, Vector2 position)
        {
            var tempRect = new Rectangle();
            tempRect.X = (int) position.X;
            tempRect.Y = (int) position.Y;
            tempRect.Width = texture.Width;
            tempRect.Height = texture.Height;
            Rectangle = tempRect;

            RotatedRectangle = new RotatedRectangle(Rectangle, Rotation);
            Position = position;
            Texture = texture;
            Origin = new Vector2(Rectangle.Width / 2f, Rectangle.Height / 2f);
        }
        public Sprite(Texture2D texture, Vector2 position, float rotation)
        {
            Rotation = rotation;

            var tempRect = new Rectangle();
            tempRect.X = (int) position.X;
            tempRect.Y = (int) position.Y;
            tempRect.Width = texture.Width;
            tempRect.Height = texture.Height;
            Rectangle = tempRect;

            RotatedRectangle = new RotatedRectangle(Rectangle, Rotation);
            Position = position;
            Texture = texture;
            Origin = new Vector2(Rectangle.Width / 2f, Rectangle.Height / 2f);
        }
        public Sprite(Texture2D texture, float rotation)
        {
            var tempRect = new Rectangle();
            tempRect.X = (int) _position.X;
            tempRect.Y = (int) _position.Y;
            tempRect.Width = texture.Width;
            tempRect.Height = texture.Height;
            Rectangle = tempRect;

            Texture = texture;
            Rotation = rotation;
            Origin = new Vector2(Rectangle.Width / 2f, Rectangle.Height / 2f);
        }

        public Sprite(Texture2D texture)
        {
            Texture = texture;

            var tempRect = new Rectangle();
            tempRect.Width = texture.Width;
            tempRect.Height = texture.Height;
            Rectangle = tempRect;

            RotatedRectangle = new RotatedRectangle(Rectangle, Rotation);
            Origin = new Vector2(Rectangle.Width / 2f, Rectangle.Height / 2f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, _position, origin: Origin, rotation: Rotation);
        }
        public void DrawRectangle(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, destinationRectangle: Rectangle, origin: Origin, rotation: Rotation);
        }
        public void Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            spriteBatch.Draw(Texture, _position + camera.Position, origin: Origin, rotation: Rotation);
        }

        //not mine
        public bool CollidesWith(Sprite other)
        {
            // Default behavior uses per-pixel collision detection
            return CollidesWith(other, true);
        }

        public bool CollidesWith(Sprite other, bool calcPerPixel)
        {
            // Get dimensions of texture
            int widthOther = other.Texture.Width;
            int heightOther = other.Texture.Height;
            int widthMe = Texture.Width;
            int heightMe = Texture.Height;

            if (calcPerPixel &&                                // if we need per pixel
                ((Math.Min(widthOther, heightOther) > 10) ||  // at least avoid doing it
                (Math.Min(widthMe, heightMe) > 10)))          // for small sizes (nobody will notice :P)
            {
                return Rectangle.Intersects(other.Rectangle) // If simple intersection fails, don't even bother with per-pixel
                    && PerPixelCollision(this, other);
            }

            return Rectangle.Intersects(other.Rectangle);
        }

        public bool PerPixelCollision(Sprite a, Sprite b)
        {
            // Get Color data of each Texture
            Color[] bitsA = new Color[a.Texture.Width * a.Texture.Height];
            a.Texture.GetData(bitsA);
            Color[] bitsB = new Color[b.Texture.Width * b.Texture.Height];
            b.Texture.GetData(bitsB);

            // Calculate the intersecting rectangle
            int x1 = Math.Max(a.Rectangle.X, b.Rectangle.X);
            int x2 = Math.Min(a.Rectangle.X + a.Rectangle.Width, b.Rectangle.X + b.Rectangle.Width);

            int y1 = Math.Max(a.Rectangle.Y, b.Rectangle.Y);
            int y2 = Math.Min(a.Rectangle.Y + a.Rectangle.Height, b.Rectangle.Y + b.Rectangle.Height);

            // For each single pixel in the intersecting rectangle
            for (int y = y1; y < y2; ++y)
            {
                for (int x = x1; x < x2; ++x)
                {
                    // Get the color from each texture
                    Color a1 = bitsA[(x - a.Rectangle.X) + (y - a.Rectangle.Y) * a.Texture.Width];
                    Color b1 = bitsB[(x - b.Rectangle.X) + (y - b.Rectangle.Y) * b.Texture.Width];

                    if (a1.A != 0 && b1.A != 0) // If both colors are not transparent (the alpha channel is not 0), then there is a collision
                    {
                        return true;
                    }
                }
            }
            // If no collision occurred by now, we're clear.
            return false;
        }
        //not mine end
    }
}
