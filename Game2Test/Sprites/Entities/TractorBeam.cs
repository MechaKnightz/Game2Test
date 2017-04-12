using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2Test.Sprites.Entities
{
    public class TractorBeam : Sprite
    {
        private float _length;
        public float Length
        {
            get { return _length; }
            set
            {
                _length = value;
                Rectangle.Width = (int)value;
            }
        }

        public bool DrawBeam { get; set; }

        public TractorBeam(TractorBeam tractorBeam)
        {
            Length = tractorBeam.Length;
            //Sprite
            Rotation = tractorBeam.Rotation;
            Position = tractorBeam.Position;
            Rectangle = tractorBeam.Rectangle;
            Origin = tractorBeam.Origin;
            Texture = tractorBeam.Texture;
            //End
        }

        public TractorBeam(Texture2D texture, float length)
        {
            Origin = new Vector2(0, texture.Height / 2f);
            Texture = texture;
            Length = length;
            Rectangle = new Rectangle(0 ,0 , texture.Width, texture.Height);
        }

        public void Update(Sector sector)
        {
            var shortestDist = Length + 1;
            var closestCrystal = new Crystal();
            foreach (var asteroid in sector.Asteroids)
            {
                if (asteroid.Destroyed)
                {
                    foreach (var crystal in asteroid.Crystals)
                    {
                        var deltaDistance = Vector2.Distance(Position, crystal.Position);

                        if (deltaDistance < Length && deltaDistance < shortestDist)
                        {
                            shortestDist = deltaDistance;
                            closestCrystal = crystal;
                            DrawBeam = true;
                        }
                    }
                }
            }
            Rotation = Game1.AngleToOther(Position, closestCrystal.Position);
            Length = shortestDist;
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, destinationRectangle: Rectangle, origin: Origin, rotation: Rotation);
            DrawBeam = false;
        }
    }
}
