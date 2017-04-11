using System;
using System.Collections.Generic;
using System.Linq;
using Game2Test.Sprites.Helpers;
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
        public TurretType Type { get; set; }

        public Turret() { }

        public Turret(Turret turret)
        {
            Offset = turret.Offset;
            EnergyCost = turret.EnergyCost;
            TurnRate = turret.TurnRate;
            Type = turret.Type;
            //Sprite
            Rotation = turret.Rotation;
            Position = turret.Position;
            Rectangle = turret.Rectangle;
            Origin = turret.Origin;
            Texture = turret.Texture;
            //End
            foreach (var shot in turret.Shots)
            {
                Shots.Add(shot.Key,new Shot(shot.Value));
                ShotDictionary.Add(shot.Key, new List<Shot>());
            }
        }
        public Turret(Texture2D texture, Vector2 position, Vector2 offset, float rotation, Dictionary<string, Shot> shots, float energyCost, float turnRate, TurretType type) : base(texture, position, rotation)
        {
            Offset = offset;
            EnergyCost = energyCost;
            TurnRate = turnRate;
            Type = type;
            foreach(var shot in shots)
            {
                Shots.Add(shot.Key, shot.Value);
                ShotDictionary.Add(shot.Key, new List<Shot>());
            }

        }

        public void SetRotation(float rotation)
        {
            Rotation = rotation;
            if (Rotation > Game1.DoublePI) Rotation -= Game1.DoublePI;
            else if (Rotation < 0) Rotation += Game1.DoublePI;
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
                    t.Value[i].Duration--;
                    var temp = t.Value[i].Position;
                    temp.X += (float)Math.Cos(t.Value[i].Rotation) * t.Value[i].Speed;
                    temp.Y += (float)Math.Sin(t.Value[i].Rotation) * t.Value[i].Speed;
                    t.Value[i].Position = temp;

                    if (t.Value[i].Duration > 00) continue;
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
            ShotDictionary["default"].Add(new Shot(Shots["default"].Texture, Position, Rotation, Shots["default"].Duration, Shots["default"].Speed, Shots["default"].Damage));
        }
        public float Fire(string name) //fire shot by Name
        {
            Shot shot = Shots.FirstOrDefault(x => x.Value.Name == name).Value;
            ShotDictionary[name].Add(new Shot(shot.Texture, Position, Rotation, shot.Duration, shot.Speed, shot.Damage));

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
