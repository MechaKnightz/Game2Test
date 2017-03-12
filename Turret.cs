using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Game2Test
{
    public class Turret : Sprite
    {
        //offset == turret position compared to ship position
        public Vector2 offset;
        public Turret(Texture2D texture, Vector2 position, Vector2 offset, float rotation) : base(texture, position, rotation)
        {
            this.offset = offset;
        }
    }
}
