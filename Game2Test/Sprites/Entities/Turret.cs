using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2Test.Sprites.Entities
{
    public class Turret : Sprite
    {
        public Dictionary<string, List<Shot>> ShotDictionary { get; set; } = new Dictionary<string, List<Shot>>();
        public Dictionary<string, Shot> Shots { get; set; } = new Dictionary<string, Shot>();

        //Offset == turret position compared to ship position
        public Vector2 Offset { get; }
        public float EnergyCost { get; }
        public float TurnRate { get; set; }

        public Turret() { }
        public Turret(Texture2D texture, Vector2 position, Vector2 offset, float rotation, Dictionary<string, Shot> shots, float energyCost, float turnRate) : base(texture, position, rotation)
        {
            Offset = offset;
            EnergyCost = energyCost;
            TurnRate = turnRate;
            foreach(var t in shots)
            {
                Shots.Add(t.Key, t.Value);
                ShotDictionary.Add(t.Key, new List<Shot>());
            }

        }

        public void Update()
        {
            UpdateShots();
        }

        public void UpdateShots()
        {
            foreach (var t in ShotDictionary)
            {
                for (var i = 0; i < t.Value.Count; i++)
                {
                    t.Value[i].duration--;
                    var temp = t.Value[i].Position;
                    temp.X += (float)Math.Cos(t.Value[i].Rotation) * t.Value[i].speed;
                    temp.Y += (float)Math.Sin(t.Value[i].Rotation) * t.Value[i].speed;
                    t.Value[i].Position = temp;

                    if (t.Value[i].duration > 00) continue;
                    t.Value.RemoveAt(i);
                    i--;
                }
            }
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, origin: Origin, rotation: Rotation);
            foreach (var t in ShotDictionary)
            {
                foreach (var e in t.Value)
                {
                    e.Draw(spriteBatch);
                }
            }
        }

        public bool ShotCollision(Rectangle rectangle, out Shot tempShot)
        {
            foreach (var t in ShotDictionary)
            {
                for (var i = 0; i < t.Value.Count; i++)
                {
                    if (!t.Value[i].Rectangle.Intersects(rectangle)) continue;
                    tempShot = t.Value[i];
                    t.Value.RemoveAt(i);
                    return true;
                }
            }
            tempShot = null;
            return false;
        }

        public void Fire() //fire the default shot
        {
            ShotDictionary["default"].Add(new Shot(Shots["default"].Texture, Position, Rotation, Shots["default"].duration, Shots["default"].speed, Shots["default"].Damage));
        }
        public float Fire(string name) //fire shot by Name
        {
            Shot shot = Shots.FirstOrDefault(x => x.Value.name == name).Value;
            ShotDictionary[name].Add(new Shot(shot.Texture, Position, Rotation, shot.duration, shot.speed, shot.Damage));

            return EnergyCost;
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
