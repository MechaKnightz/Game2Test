using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Game2Test
{
    public class Turret : Sprite
    {
        public int duration;
        public Turret(Texture2D texture, Vector2 position, float rotation, int duration) : base(texture, position, rotation)
        {
            this.duration = duration;
        }
        public int GetDuration()
        {
            return duration;
        }
    }
}
