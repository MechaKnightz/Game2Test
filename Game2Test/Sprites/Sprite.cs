using System;
using System.Collections.Generic;
using System.Linq;
using Game2Test.Sprites.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json;

namespace Game2Test.Sprites
{
    public class Sprite
    {
        private Vector2 _origin;
        public Vector2 Origin
        {
            get { return _origin; }
            set
            {
                _origin = value;
            }
        }

        public Rectangle Rectangle { get; set; }

        public float Rotation { get; set; }

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

        public bool Intersects(Sprite sprite)
        {
            //Calculate the Axis we will use to determine if a collision has occurred
            //Since the objects are rectangles, we only have to generate 4 Axis (2 for
            //each rectangle) since we know the other 2 on a rectangle are parallel.
            List<Vector2> aRectangleAxis = new List<Vector2>();
            aRectangleAxis.Add(UpperRightCorner() - UpperLeftCorner());
            aRectangleAxis.Add(UpperRightCorner() - LowerRightCorner());
            aRectangleAxis.Add(sprite.UpperLeftCorner() - sprite.LowerLeftCorner());
            aRectangleAxis.Add(sprite.UpperLeftCorner() - sprite.UpperRightCorner());

            //Cycle through all of the Axis we need to check. If a collision does not occur
            //on ALL of the Axis, then a collision is NOT occurring. We can then exit out 
            //immediately and notify the calling function that no collision was detected. If
            //a collision DOES occur on ALL of the Axis, then there is a collision occurring
            //between the rotated rectangles. We know this to be true by the Seperating Axis Theorem
            foreach (Vector2 aAxis in aRectangleAxis)
            {
                if (!IsAxisCollision(sprite, aAxis))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines if a collision has occurred on an Axis of one of the
        /// planes parallel to the Rectangle
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="aAxis"></param>
        /// <returns></returns>
        private bool IsAxisCollision(Sprite sprite, Vector2 aAxis)
        {
            //Project the corners of the Rectangle we are checking on to the Axis and
            //get a scalar value of that project we can then use for comparison
            List<int> aRectangleAScalars = new List<int>();
            aRectangleAScalars.Add(GenerateScalar(sprite.UpperLeftCorner(), aAxis));
            aRectangleAScalars.Add(GenerateScalar(sprite.UpperRightCorner(), aAxis));
            aRectangleAScalars.Add(GenerateScalar(sprite.LowerLeftCorner(), aAxis));
            aRectangleAScalars.Add(GenerateScalar(sprite.LowerRightCorner(), aAxis));

            //Project the corners of the current Rectangle on to the Axis and
            //get a scalar value of that project we can then use for comparison
            List<int> aRectangleBScalars = new List<int>();
            aRectangleBScalars.Add(GenerateScalar(UpperLeftCorner(), aAxis));
            aRectangleBScalars.Add(GenerateScalar(UpperRightCorner(), aAxis));
            aRectangleBScalars.Add(GenerateScalar(LowerLeftCorner(), aAxis));
            aRectangleBScalars.Add(GenerateScalar(LowerRightCorner(), aAxis));

            //Get the Maximum and Minium Scalar values for each of the Rectangles
            int aRectangleAMinimum = aRectangleAScalars.Min();
            int aRectangleAMaximum = aRectangleAScalars.Max();
            int aRectangleBMinimum = aRectangleBScalars.Min();
            int aRectangleBMaximum = aRectangleBScalars.Max();

            //If we have overlaps between the Rectangles (i.e. Min of B is less than Max of A)
            //then we are detecting a collision between the rectangles on this Axis
            if (aRectangleBMinimum <= aRectangleAMaximum && aRectangleBMaximum >= aRectangleAMaximum)
            {
                return true;
            }
            else if (aRectangleAMinimum <= aRectangleBMaximum && aRectangleAMaximum >= aRectangleBMaximum)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Generates a scalar value that can be used to compare where corners of 
        /// a rectangle have been projected onto a particular axis. 
        /// </summary>
        /// <param name="theRectangleCorner"></param>
        /// <param name="theAxis"></param>
        /// <returns></returns>
        private int GenerateScalar(Vector2 theRectangleCorner, Vector2 theAxis)
        {
            //Using the formula for Vector projection. Take the corner being passed in
            //and project it onto the given Axis
            float aNumerator = (theRectangleCorner.X * theAxis.X) + (theRectangleCorner.Y * theAxis.Y);
            float aDenominator = (theAxis.X * theAxis.X) + (theAxis.Y * theAxis.Y);
            float aDivisionResult = aNumerator / aDenominator;
            Vector2 aCornerProjected = new Vector2(aDivisionResult * theAxis.X, aDivisionResult * theAxis.Y);

            //Now that we have our projected Vector, calculate a scalar of that projection
            //that can be used to more easily do comparisons
            float aScalar = (theAxis.X * aCornerProjected.X) + (theAxis.Y * aCornerProjected.Y);
            return (int)aScalar;
        }

        /// <summary>
        /// Rotate a point from a given location and adjust using the Origin we
        /// are rotating around
        /// </summary>
        /// <param name="thePoint"></param>
        /// <param name="theOrigin"></param>
        /// <param name="theRotation"></param>
        /// <returns></returns>
        private Vector2 RotatePoint(Vector2 thePoint, Vector2 theOrigin, float theRotation)
        {
            Vector2 aTranslatedPoint = new Vector2();
            aTranslatedPoint.X = (float)(theOrigin.X + (thePoint.X - theOrigin.X) * Math.Cos(theRotation)
                                         - (thePoint.Y - theOrigin.Y) * Math.Sin(theRotation));
            aTranslatedPoint.Y = (float)(theOrigin.Y + (thePoint.Y - theOrigin.Y) * Math.Cos(theRotation)
                                         + (thePoint.X - theOrigin.X) * Math.Sin(theRotation));
            return aTranslatedPoint;
        }

        public Vector2 UpperLeftCorner()
        {
            Vector2 aUpperLeft = new Vector2(Rectangle.Left, Rectangle.Top);
            aUpperLeft = RotatePoint(aUpperLeft, aUpperLeft + Origin, Rotation);
            return aUpperLeft;
        }

        public Vector2 UpperRightCorner()
        {
            Vector2 aUpperRight = new Vector2(Rectangle.Right, Rectangle.Top);
            aUpperRight = RotatePoint(aUpperRight, aUpperRight + new Vector2(-Origin.X, Origin.Y), Rotation);
            return aUpperRight;
        }

        public Vector2 LowerLeftCorner()
        {
            Vector2 aLowerLeft = new Vector2(Rectangle.Left, Rectangle.Bottom);
            aLowerLeft = RotatePoint(aLowerLeft, aLowerLeft + new Vector2(Origin.X, -Origin.Y), Rotation);
            return aLowerLeft;
        }

        public Vector2 LowerRightCorner()
        {
            Vector2 aLowerRight = new Vector2(Rectangle.Right, Rectangle.Bottom);
            aLowerRight = RotatePoint(aLowerRight, aLowerRight + new Vector2(-Origin.X, -Origin.Y), Rotation);
            return aLowerRight;
        }

        public int X
        {
            get { return Rectangle.X; }
        }

        public int Y
        {
            get { return Rectangle.Y; }
        }

        public int Width
        {
            get { return Rectangle.Width; }
        }

        public int Height
        {
            get { return Rectangle.Height; }
        }
        //not mine end
    }
}
