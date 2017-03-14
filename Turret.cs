using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.CodeDom;
using System.Linq;


namespace Game2Test
{
    public class Turret : Sprite
    {
        //offset == turret position compared to ship position
        // TODO: add speed to shot class
        public Vector2 offset;
        public Dictionary<string, List<Shot>> shotDictionary = new Dictionary<string, List<Shot>>();
        public Dictionary<string, Shot> shots = new Dictionary<string, Shot>();
        public Turret(Texture2D texture, Vector2 position, Vector2 offset, float rotation, Shot shot) : base(texture, position, rotation)
        {
            this.offset = offset;
            shots.Add(shot.name, shot);
            shotDictionary.Add(shot.name, new List<Shot>());
        }

        public new void Update()
        {
            UpdateShots();
        }

        public void Fire() //fire the default shot
        {
            shotDictionary["default"].Add(new Shot(shots["default"].texture, position, rotation, 60)); // TODO: doesnt add to list for some reason
        }

        public void UpdateShots()
        {
            foreach (var t in shotDictionary)
            {
                for (int i = 0; i < t.Value.Count; i++)
                {
                    t.Value[i].duration++;
                    var temp = t.Value[i].position;
                    temp.X += (float)(System.Math.Cos(t.Value[i].rotation)) * 15; // TODO: 15 supposed to be speed
                    temp.Y += (float)(System.Math.Sin(t.Value[i].rotation)) * 15;
                    t.Value[i].SetPos(temp);

                    if (t.Value[i].duration > 60)
                    {
                        t.Value.RemoveAt(i);
                    }
                }
            }
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, origin: origin, rotation: rotation);
            foreach (var t in shotDictionary)
            {
                foreach (var e in t.Value)
                {
                    e.Draw(spriteBatch);
                }
            }
        }

        public bool ShotCollision(Rectangle rectangle)
        {
            foreach (var t in shotDictionary)
            {
                for (int i = 0; i < t.Value.Count; i++)
                {
                    if (t.Value[i].rectangle.Intersects(rectangle))
                    {
                        t.Value.RemoveAt(i);
                        return true;
                    }
                }
            }
            return false;
        }

        public void Fire(string name) //fire shot by name
        {
            
        }
    }
}
