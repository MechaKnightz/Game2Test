using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2Test.Sprites.Entities
{
    public class TractorBeam : Sprite
    {
        public float Length { get; set; }

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
        }
    }
}
