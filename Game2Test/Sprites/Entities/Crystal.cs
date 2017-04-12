﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2Test.Sprites.Entities
{
    public class Crystal : Sprite
    {
        public float Size { get; set; }
        public float Speed { get; set; }
        public float Duration { get; set; }

        public Crystal() { }
        public Crystal(Texture2D texture, float size)
        {
            Texture = texture;
            Size = size;
            Speed = 0.1f * size;
            Duration = size * 200;
            Rectangle = new Rectangle(0 , 0, texture.Width, texture.Height);
            Origin = new Vector2(texture.Width/2f, texture.Height/2f);
        }
    }
}
