using System;
using System.Collections.Generic;
using System.Linq;
using Game2Test.Sprites.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2Test.Sprites.Entities.Turrets
{
    public class BaseTurret : Sprite
    {
        public List<Shot> ShotList { get; set; } = new List<Shot>();
        public Shot Shot { get; set; } = new Shot();

        //Offset == turret position compared to ship position
        public Vector2 Offset { get; set; }
        public float EnergyCost { get; set; }
        public float TurnRate { get; set; }
        public TurretType Type { get; set; }
        public float Cooldown { get; set; }
        public float CooldownCounter { get; set; }
        public bool IsFiring { get; set; }
        public float Speed { get; set; }
        public float Range { get; set; }

        public BaseTurret() { }

        public BaseTurret(BaseTurret turret)
        {
            Offset = turret.Offset;
            EnergyCost = turret.EnergyCost;
            TurnRate = turret.TurnRate;
            Type = turret.Type;
            Cooldown = turret.Cooldown;
            //Sprite
            Rotation = turret.Rotation;
            Position = turret.Position;
            Rectangle = turret.Rectangle;
            Origin = turret.Origin;
            Texture = turret.Texture;
            //End
            Shot = new Shot(turret.Shot);
        }
        public BaseTurret(Texture2D texture, Vector2 position, Vector2 offset, float rotation, Shot shot, float energyCost, float turnRate, TurretType type, float cooldown) : base(texture, position, rotation)
        {
            Offset = offset;
            EnergyCost = energyCost;
            TurnRate = turnRate;
            Type = type;
            Cooldown = cooldown;
            Shot = shot;
        }

        public void SetRotation(float rotation)
        {
            Rotation = rotation;
            if (Rotation > Game1.DoublePI) Rotation -= Game1.DoublePI;
            else if (Rotation < 0) Rotation += Game1.DoublePI;
        }

        public float Update(float energy)
        {
            if(CooldownCounter != Cooldown) CooldownCounter++;
            UpdateShots();
            return 0f;
        }

        public void UpdateShots()
        {
            for(int i = 0; i < ShotList.Count; i++)
            {
                ShotList[i].Duration--;

                var temp = ShotList[i].Position;
                temp.X += (float)Math.Cos(ShotList[i].Rotation) * ShotList[i].Speed;
                temp.Y += (float)Math.Sin(ShotList[i].Rotation) * ShotList[i].Speed;
                ShotList[i].Position = temp;

                if (ShotList[i].Duration > 00) continue;
                ShotList.RemoveAt(i);
                i--;
            }
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, origin: Origin, rotation: Rotation);
            foreach (var shot in ShotList)
            {
                shot.Draw(spriteBatch);
            }
        }

        public bool ShotCollision(Rectangle rectangle, out Shot tempShot)
        {
            for (int i = 0; i < ShotList.Count; i++) 
            {
                if (!ShotList[i].RotatedRectangle.Intersects(rectangle)) continue;
                tempShot = ShotList[i];
                ShotList.RemoveAt(i);
                return true;
            }
            tempShot = null;
            return false;
        }

        public float Fire()
        {
            if (CooldownCounter != Cooldown) return 0f;

            ShotList.Add(new Shot(Shot.Texture, Position, Rotation, Shot.Duration, Shot.Speed, Shot.Damage));
            CooldownCounter = 0f;

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
        }
    }
}
