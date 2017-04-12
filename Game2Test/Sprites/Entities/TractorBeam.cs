﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2Test.Sprites.Entities
{
    public class TractorBeam : Sprite
    {
        public float Length { get; set; }
        public Crystal LockedOnCrystal { get; set; }
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
            var shortestDist = float.MaxValue;
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
            LockedOnCrystal = closestCrystal;
            Rotation = Game1.AngleToOther(Position, closestCrystal.Position);
            Rectangle.Width = (int)shortestDist;
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, destinationRectangle: Rectangle, origin: Origin, rotation: Rotation);
            DrawBeam = false;
        }
    }
}
