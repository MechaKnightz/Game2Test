using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Game2Test
{
    public class Shot : Sprite
    {
        public string name = "default";
        public int duration;
        public float speed;
        public Shot(Texture2D texture, Vector2 position, float rotation, int duration, string name, float speed) : base(texture, position, rotation)
        {
            this.duration = duration;
        }
        public Shot(Texture2D texture, Vector2 position, float rotation, int duration) : base(texture, position, rotation)
        {
            this.duration = duration;
        }
        public Shot(Texture2D texture, Vector2 position, float rotation, int duration, float speed) : base(texture, position, rotation)
        {
            this.duration = duration;
            this.speed = speed;
        }
        public Shot(Texture2D texture, int duration, string name, int speed) : base(texture)
        {
            this.duration = duration;
            this.speed = speed;
        }

        public void Update()
        {
            
        }
    }
}