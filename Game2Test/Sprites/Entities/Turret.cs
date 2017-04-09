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
        public float TurnRate { get; set; }
        public Dictionary<string, List<Shot>> shotDictionary = new Dictionary<string, List<Shot>>();
        public Dictionary<string, Shot> shots = new Dictionary<string, Shot>();
        public float energyCost;
        public Turret() { }
        public Turret(Texture2D texture, Vector2 position, Vector2 offset, float rotation, Dictionary<string, Shot> shots, float energyCost, float turnRate) : base(texture, position, rotation)
        {
            this.offset = offset;
            this.energyCost = energyCost;
            TurnRate = turnRate;
            foreach(var t in shots)
            {
                this.shots.Add(t.Key, t.Value);
                shotDictionary.Add(t.Key, new List<Shot>());
            }

        }

        public void Update()
        {
            UpdateShots();
        }

        public void UpdateShots()
        {
            foreach (var t in shotDictionary)
            {
                for (int i = 0; i < t.Value.Count; i++)
                {
                    t.Value[i].duration--;
                    var temp = t.Value[i].Position;
                    temp.X += (float)(System.Math.Cos(t.Value[i].Rotation)) * t.Value[i].speed;
                    temp.Y += (float)(System.Math.Sin(t.Value[i].Rotation)) * t.Value[i].speed;
                    t.Value[i].Position = temp;

                    if (t.Value[i].duration < 00)
                    {
                        t.Value.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, origin: origin, rotation: Rotation);
            foreach (var t in shotDictionary)
            {
                foreach (var e in t.Value)
                {
                    e.Draw(spriteBatch);
                }
            }
        }

        public bool ShotCollision(Rectangle rectangle, out Shot tempShot)
        {
            foreach (var t in shotDictionary)
            {
                for (int i = 0; i < t.Value.Count; i++)
                {
                    if (t.Value[i].rectangle.Intersects(rectangle))
                    {
                        tempShot = t.Value[i];
                        t.Value.RemoveAt(i);
                        i--;
                        return true;
                    }
                }
            }
            tempShot = null;
            return false;
        }

        public void Fire() //fire the default shot
        {
            shotDictionary["default"].Add(new Shot(shots["default"].texture, Position, Rotation, shots["default"].duration, shots["default"].speed, shots["default"].Damage));
        }
        public float Fire(string name) //fire shot by Name
        {
            Shot shot = shots.FirstOrDefault(x => x.Value.name == name).Value;
            shotDictionary[name].Add(new Shot(shot.texture, Position, Rotation, shot.duration, shot.speed, shot.Damage));

            return energyCost;
        }
        public void Turn(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    Rotation -= TurnRate;
                    break;
                case Direction.Right:
                    Rotation += TurnRate;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
            Update();
        }
    }
}
